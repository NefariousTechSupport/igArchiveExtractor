#include <cstring>
#include <sstream>
#include <string>
#include <stdint.h>
#include <errno.h>
#include <cstddef>
#include <iostream>
#include <cstdlib>

#include "helpers.h"
#include "iAE_io.h"

//Loads a file and its data
uint32_t iAE_LoadFile(char* filePath, iAE_File* output)
{
	iAE_File temp;													//Temporary container for the file data
	temp.fs = fopen(filePath, "rb");								//Open a file handle
	iAE_SetNumberOfFiles(&temp);									//Read and assign the number of files
	
	iAE_PopulateDescHeaderArray(&temp);								//Fill the data for the local file headers
	temp.nameTableStartAddress = iAE_GetNameTableStartAddr(temp);	//Get the number of files in this archive
	//sortArrayDescs(temp.localFileHeaders, temp.numberOfFiles);	//Sort the files based on their starting addresses (to be deprecated)
	*output = temp;													//Set the output variable
	return 0;
}

//Set the number of files
int iAE_SetNumberOfFiles(iAE_File* file)
{
	if(file->fs == NULL) return -1;									//If the file handle's not assigned then return -1
	if(iAE_CheckFileHeader(*file))									//Check if the file's magic number is genuine
	{
		fseek(file->fs, 0x0C, SEEK_SET);							//Go to address 0x0C in the file
		uint32_t numFiles = 0;										//Variable to read the number of files into
		fread(&numFiles, 0x04, 0x01, file->fs);						//Read the number of files into the variable (byte order must be reversed for big endian)
		if(isBigEndian())
		{
			file->numberOfFiles = invertByteOrder_u32(numFiles);
		}
		else
		{
			file->numberOfFiles = numFiles;
		}
		return 0;
	}
	else return -2;
}

//Checks the file's magic number
bool iAE_CheckFileHeader(iAE_File file)
{
	if(file.fs == NULL) return -1;					//If the file handle's not assigned then return -1

	fseek(file.fs, 0x00, SEEK_SET);					//go to address 0x00 in the file

	uint32_t magicNumber = 0;						//The variable to read the magic number into
	fread(&magicNumber, 0x04, 0x01, file.fs);		//Read the magic number into the variable

	if(isBigEndian())
	{
		return magicNumber == 0x4947411A;
	}
	else
	{
		return magicNumber == 0x1A414749;
	}
		
}
//Reads the local file header for the specified file
uint32_t iAE_SetHeaderValues(iAE_File* file, uint32_t fileNo)
{
	if(file->fs == NULL) return -1;											//If the file handle's not assigned then return -1

	uint32_t startingAddress = iAE_GetFileDescsEndingAddr(*file) - ((file->numberOfFiles - fileNo) * 12) + 1;		//Read the local file header's starting address
	uint32_t readBuffer;													//The variable to read into
	fseek(file->fs, startingAddress, SEEK_SET);								//Go to the local file header's starting address, aka where the local file header contains data on where the file would actually start
	fread(&readBuffer, 0x04, 0x01, file->fs);								//Read into the read buffer
	file->localFileHeaders[fileNo].startingAddress = readBuffer;			//Set the starting address
	fread(&readBuffer, 0x04, 0x01, file->fs);								//Read into the read buffer, at this point the read head would be 4 in front now, aka where the local file's size is stored
	file->localFileHeaders[fileNo].size = readBuffer;						//Set the size
	return 0;
}
//Fills the local file header array
uint32_t iAE_PopulateDescHeaderArray(iAE_File* file)
{
	file->localFileHeaders = (iAE_FileDescHeader*)malloc(sizeof(iAE_FileDescHeader) * file->numberOfFiles);	//Allocate the appropriate memory for the local file headers
	for (size_t i = 0; i < file->numberOfFiles; i++)	//For every file in the archive
	{
		iAE_SetHeaderValues(file, i);					//Get the header values for this header
		file->localFileHeaders[i].index = i;			//Set the index number of this header (soon to be deprecated)
	}
	return 0;
}
//Ez
uint32_t iAE_ExtractFile(iAE_File file, uint32_t fileNo, const char* outputPath)
{
	unsigned char readBuffer[0x40];						//Byte array of length 64
	memset(readBuffer, 0x00, 0x40);						//Initialise the array

	FILE* outputfs = fopen(outputPath, "wb");			//Open the file that'll be the output file

	if(outputfs == NULL)								//If the file wasn't opened successfully
	{
		printf("fopen failed with error %d\n", errno);	//Print the error
		return 1;
	}

	fseek(file.fs, file.localFileHeaders[fileNo].startingAddress, SEEK_SET);	//Go to the local file's starting address
	uint32_t i = 0;
	for (; i < file.localFileHeaders[fileNo].size - 0x40; i += 0x40)			//Loop over the file's length
	{
		int readres = fread(readBuffer, 0x01, 0x40, file.fs);					//Read the file
		if(readres < 64)														//If less than 64 bytes were read then an error occured
		{
			printf("fread failed with error %d", errno);						//Print the error code
			goto ExitExtract;
		}
		int writeres = fwrite(readBuffer, 0x01, 0x40, outputfs);				//Write to the output file

		if(writeres < 64)														//If less than 64 bytes were written then an error occured
		{
			printf("fwrite failed with error %d", errno);						//Print the error code
			goto ExitExtract;
		}
	}
	if(0x40 >= file.localFileHeaders[fileNo].size - i && file.localFileHeaders[fileNo].size - i > 0)	//If the bytes remaining is in between 0 and 40
	{
		fread(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, file.fs);						//Read the remaining bytes
		fwrite(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, outputfs);						//Write the remaining bytes
	}
	printf("extracted\n");
ExitExtract:
	fclose(outputfs);															//Close the output file
	return 0;
}

//Returns the end of the local file headers
uint32_t iAE_GetFileDescsEndingAddr(iAE_File file)
{
	uint64_t readBuffer = 0;						//The buffer to read into
	uint32_t position = 0;							//The current position

	for (; position < 800; position += 8)			//Loop over the first 800 bytes
	{
		fseek(file.fs, position, SEEK_SET);			//Go to the position
		fread(&readBuffer, 0x08, 0x01, file.fs);	//Read 8 bytes
		if(readBuffer == 0xFFFFFFFFFFFFFFFF)		//Check if it equals that number
		{
			break;									//Exit the loop
		}
	}
	unsigned char currByte = 0x00;
	do
	{
		fseek(file.fs, position--, SEEK_SET);		//Go to the position where the files end
		fread(&currByte, 0x01, 0x01, file.fs);		//Read the byte
	} while(currByte == 0xFF);						//Check if the byte is 0xFF
	position += 5;									//Add 5 to the position in order to get to the end of the local headers
	return position;								//Return this position
}
//Get the start of the nametable's address
uint32_t iAE_GetNameTableStartAddr(iAE_File file)
{
	fseek(file.fs, 0x20, SEEK_SET);								//Go to address 0x20

	uint32_t nameTableSize;

	fread(&nameTableSize, 0x04, 0x01, file.fs);					//Read 4 bytes to get the size of the nametable

	fseek(file.fs, 0x00, SEEK_END);								//Go to the end of the file

	uint32_t fileSize = ftell(file.fs);							//Read the current position

	return fileSize - nameTableSize;							//Get the difference to find the nametable's starting address
}
//Find the name of the specified file
uint32_t iAE_FindName(iAE_File file, uint32_t fileNo, std::string* output)
{
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
	//*output = (char*)temp.c_str();

	return 0;
}