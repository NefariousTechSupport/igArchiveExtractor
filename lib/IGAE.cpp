#include <cstring>
#include <sstream>
#include <string>
#include <stdint.h>
#include <errno.h>
#include <cstddef>
#include <iostream>
#include <cstdlib>

#include "IGAE_helpers.h"

#if defined(BUILD_LIB)
#include "libIGAE.h"
#else
#include "C:\Users\jaska\Documents\source\VScode\igArchiveExtractor\lib\libIGAE.h"
#endif

const uint32_t locations[0x06][0x0E] = {
	//SSA WII U
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00000018,		//Unknown
		0x0000000C,		//Number of Files
		0x0000001C,		//Nametable Location
		0x00000020,		//Nametable Size
		0x0000000C,		//Length of indiviual local file header
		0x00000034,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000004,		//Position of a local file's size inside of a local header
		0x00000024,		//Postion of first padding
		0x00000010,		//Length of first padding
		0x00000008		//Unknown but important adddress
	},
	//SSA WII
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00000018,		//Unknown
		0x0000000C,		//Number of Files
		0x00000018,		//Nametable Location
		0x0000001C,		//Nametable Size
		0x0000000C,		//Length of indiviual local file header
		0x00000030,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000004		//Position of a local file's size inside of a local header
	},
	//SG
	{},
	//SSF
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00,			//Unknown
		0x0000000C,		//Number of Files
		0x00000028,		//Nametable Location
		0x00000030,		//Nametable Size
		0x00000010,		//Length of individual local file header
		0x00000038,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000008		//Position of a local file's size inside of a local header
	},
	//STT
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00000018,		//Unknown
		0x0000000C,		//Number of Files
		0x00000028,		//Nametable Location
		0x00000030,		//Nametable Size
		0x00000010,		//Length of individual local file header
		0x00000038,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000008,		//Position of a local file's size inside of a local header
		0x0000001C,		//Postion of first padding
		0x0000000C		//Length of first padding
	},
	//SSC
	{}
};

//Loads a file and its data
int IGAE_LoadFile(char* filePath, IGAE_File* output)
{
	IGAE_File temp;													//Temporary container for the file data
	temp.fs = fopen(filePath, "rb");								//Open a file handle
	if(temp.fs == NULL)
	{
		return -2;
	}
	bool headerCheck = IGAE_CheckFileHeader(&temp);
	if(!headerCheck)												//Check to see if the magic number is correct
	{
		return -3;
	}
	temp.version = IGAE_ReadVersion(temp, &temp.rawVersion);		//Read and assign the version number
	if(temp.version == 0xFF)										//If unsupported version
	{
		fclose(temp.fs);
		return -4;
	}
	temp.numberOfFiles = IGAE_GetNumberOfFiles(temp);				//Read and assign the number of files
	
	IGAE_PopulateDescHeaderArray(&temp);							//Fill the data for the local file headers
	temp.nameTableStartAddress = IGAE_GetNameTableStartAddr(temp);	//Get the number of files in this archive
	temp.nameTableLength = IGAE_GetNameTableLength(temp);

	*output = temp;													//Set the output variable
	return 0;
}

//Gets the number of files
long long int IGAE_GetNumberOfFiles(IGAE_File file)
{
	if(file.fs == NULL) return -1;									//If the file handle's not assigned then return -1

	fseek(file.fs, locations[file.version][IGAE_LOCATION_NUMBER_OF_FILES], SEEK_SET);			//Go to the address of the number of files in the file
	uint32_t numFiles = 0;										//Variable to read the number of files into
	fread(&numFiles, 0x04, 0x01, file.fs);						//Read the number of files into the variable (byte order must be reversed for big endian)
	if(file.endianness == IGAE_BE)
	{
		return invertByteOrder_u32(numFiles);
	}
	else
	{
		return numFiles;
	}
}

//Checks the file's magic number
bool IGAE_CheckFileHeader(IGAE_File* file)
{
	if(file->fs == NULL) return false;				//If the file handle's not assigned then return -1

	fseek(file->fs, 0x00, SEEK_SET);				//go to address 0x00 in the file

	uint32_t magicNumber = 0;						//The variable to read the magic number into
	fread(&magicNumber, 0x04, 0x01, file->fs);		//Read the magic number into the variable

	bool returnVal = true;							//The value to return, will remain true unless the headers don't match

	if(magicNumber == 0x4947411A)					//If the header is BE
	{
		file->endianness = IGAE_BE; 
	}
	else if(magicNumber == 0x1A414749)				//If the header is LE
	{
		file->endianness = IGAE_LE;
	}
	else											//If the magic number just doesn't match
	{
		returnVal = false;
	}
	return returnVal;
}
//Reads the local file header for the specified file
long long int IGAE_SetHeaderValues(IGAE_File* file, uint32_t fileNo)
{
	if(file->fs == NULL) return -1;											//If the file handle's not assigned then return -1

	uint32_t startingAddress = IGAE_GetFileDescsStartingAddr(*file);		//Read the local file header's starting address
	uint32_t readBuffer;													//The variable to read into

	fseek(file->fs, startingAddress + locations[file->version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * fileNo + locations[file->version][IGAE_LOCATION_LOCAL_START], SEEK_SET);			//Go to where the local file header contains data on where the file would actually start
	fread(&readBuffer, 0x04, 0x01, file->fs);								//Read into the read buffer
	file->localFileHeaders[fileNo].startingAddress = readBuffer;			//Set the starting address

	fseek(file->fs, startingAddress + locations[file->version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * fileNo + locations[file->version][IGAE_LOCATION_LOCAL_SIZE], SEEK_SET);			//Go to where the local file header contains data on where the file would actually start
	fread(&readBuffer, 0x04, 0x01, file->fs);								//Read into the read buffer, at this point the read head would be 4 in front now, aka where the local file's size is stored
	file->localFileHeaders[fileNo].size = readBuffer;						//Set the size
	return 0;
}
//Fills the local file header array
void IGAE_PopulateDescHeaderArray(IGAE_File* file)
{
	file->localFileHeaders = (IGAE_FileDescHeader*)malloc(sizeof(IGAE_FileDescHeader) * file->numberOfFiles);	//Allocate the appropriate memory for the local file headers
	for (size_t i = 0; i < file->numberOfFiles; i++)	//For every file in the archive
	{
		IGAE_SetHeaderValues(file, i);					//Get the header values for this header
		file->localFileHeaders[i].index = i;			//Set the index number of this header (soon to be deprecated)
	}
}
//Ez
int IGAE_ExtractFile(IGAE_File file, uint32_t fileNo, const char* outputPath)
{
	if(file.fs == NULL)								//If the file wasn't opened successfully
	{
		return -1;
	}

	FILE* outputfs = fopen(outputPath, "wb");			//Open the file that'll be the output file

	if(outputfs == NULL)								//If the file wasn't opened successfully
	{
		return -2;
	}

	unsigned char readBuffer[0x40];						//Byte array of length 64
	memset(readBuffer, 0x00, 0x40);						//Initialise the array


	fseek(file.fs, file.localFileHeaders[fileNo].startingAddress, SEEK_SET);	//Go to the local file's starting address
	uint32_t i = 0;
	for (; i < file.localFileHeaders[fileNo].size - 0x40; i += 0x40)			//Loop over the file's length
	{
		int readres = fread(readBuffer, 0x01, 0x40, file.fs);					//Read the file
		if(readres < 64)														//If less than 64 bytes were read then an error occured
		{
			goto ExitExtract;
		}
		int writeres = fwrite(readBuffer, 0x01, 0x40, outputfs);				//Write to the output file

		if(writeres < 64)														//If less than 64 bytes were written then an error occured
		{
			goto ExitExtract;
		}
	}
	if(0x40 >= file.localFileHeaders[fileNo].size - i && file.localFileHeaders[fileNo].size - i > 0)	//If the bytes remaining is in between 0 and 40
	{
		fread(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, file.fs);						//Read the remaining bytes
		fwrite(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, outputfs);						//Write the remaining bytes
	}
ExitExtract:
	fclose(outputfs);															//Close the output file
	return 0;
}

//Returns the end of the local file headers
long long int IGAE_GetFileDescsStartingAddr(IGAE_File file)
{
	if(file.numberOfFiles == 0)
	{
		return -1;
	}
	return locations[file.version][IGAE_LOCATION_UNKNOWN_2_STARTING_LOCATION] + file.numberOfFiles * locations[file.version][IGAE_LOCATION_UNKNOWN_2_LENGTH];
}
//Get the start of the nametable's address
long long int IGAE_GetNameTableStartAddr(IGAE_File file)
{
	if(file.fs == NULL)
	{
		return -1;
	}

	fseek(file.fs, locations[file.version][IGAE_LOCATION_NAMETABLE_LOCATION], SEEK_SET);	//Go to the nametable's location's location

	uint32_t nameTableLocation;

	fread(&nameTableLocation, 0x04, 0x01, file.fs);					//Read 4 bytes to get the location of the nametable

	return nameTableLocation;										//return
}
//Get the size of the nametable
long long int IGAE_GetNameTableLength(IGAE_File file)
{
	if(file.fs == NULL)
	{
		return -1;
	}

	fseek(file.fs, locations[file.version][IGAE_LOCATION_NAMETABLE_SIZE], SEEK_SET);		//Go to the nametable's location's location

	uint32_t nameTableSize;

	fread(&nameTableSize, 0x04, 0x01, file.fs);						//Read 4 bytes to get the location of the nametable

	return nameTableSize;											//return
}
//Find the name of the specified file
long long int IGAE_FindName(IGAE_File file, uint32_t fileNo, std::string* output)
{
	if(file.fs == NULL)
	{
		return -1;
	}

	uint32_t nameStartAddress;													//The name's starting address

	fseek(file.fs, file.nameTableStartAddress + fileNo * 4, SEEK_SET);			//Go to where the name's location will be stored
	fread(&nameStartAddress, 0x04, 0x01, file.fs);								//Read this location
	
	fseek(file.fs, file.nameTableStartAddress + nameStartAddress, SEEK_SET);	//Go to where the name would start

	char readChar = 0x00;							//The character being read

	std::stringstream outputss;						//the name

	while(true)
	{
		fread(&readChar, 0x01, 0x01, file.fs);		//Read the character
		if(readChar == 0x00)						//If the character that was just read is a null character
		{
			break;									//Then exit out of the loop as you'd have reached the end of the file
		}
		else										//Otherwise
		{
			outputss << readChar;					//Add to the output
		}
	}

	*output = outputss.str();						//The conversion's done this way to avoid the str() being destroyed before you can get the c_str()

	return 0;
}
int IGAE_ReadVersion(IGAE_File file, uint32_t* rawVersion, bool readFromFile)
{
	if(readFromFile)
	{
		if(file.fs == NULL) return -1;					//If the file's handle is not assigned, return -1
		fseek(file.fs, 0x04, SEEK_SET);					//Go to address 0x04
		fread(rawVersion, 0x04, 0x01, file.fs);		//Read 4 bytes and output to the raw version number
		if(file.endianness == IGAE_BE)
		{
			uint32_t temp = invertByteOrder_u32(*rawVersion);
			*rawVersion = temp;
		}
	}
	else
	{
		if(file.fs == NULL)
		{
			return -1;
		}
	}
	switch(*rawVersion)
	{
		case IGAE_VER_SSA_WIIU:	return 0x00;
		case IGAE_VER_SSA_WII:	return 0x01;
		//case IGAE_VER_SG:		return 0x02;
		//case IGAE_VER_SSF:		return 0x03;
		case IGAE_VER_STT:		return 0x04;
		//case IGAE_VER_SSC:	return 0x05;
		default:
			return -4;						//Return 255, signifies error
	}
}
extern void test()
{
	printf("test\n");
}