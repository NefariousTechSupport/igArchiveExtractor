#include <string.h>
#include <iostream>
#include <cstdlib>

#include "IGAR.h"

#define IGAR_BLOCK_SIZE 0x04

void IGAR_GenerateFile(IGAE_File file, char* output)
{
	FILE* fs = fopen(output, "wb");
	fseek(fs, 0x00, SEEK_SET);

	fwrite("IGAE", 0x01, 0x04, fs);
	fwrite(&file.rawVersion, 0x04, 0x01, fs);

	uint32_t unknown1;
	fseek(file.fs, locations[file.version][IGAE_LOCATION_UNKNOWN_1], SEEK_SET);
	fread(&unknown1, 0x04, 0x01, file.fs);
	fwrite(&unknown1, 0x04, 0x01, fs);

	uint32_t unknown2;
	fseek(file.fs, 0x08, SEEK_SET);
	fread(&unknown2, 0x04, 0x01, file.fs);
	fwrite(&unknown2, 0x04, 0x01, fs);

	fwrite(&file.numberOfFiles, 0x04, 0x01, fs);
	fwrite(&file.nameTableLength, 0x04, 0x01, fs);
	

	unsigned char unknownDataBuffer[file.numberOfFiles * 0x04];
	memset(unknownDataBuffer, 0x00, file.numberOfFiles * 0x04);
	fseek(file.fs, locations[file.version][IGAE_LOCATION_UNKNOWN_2_STARTING_LOCATION], SEEK_SET);
	fread(unknownDataBuffer, 0x01, file.numberOfFiles * 0x04, file.fs);
	fwrite(unknownDataBuffer, 0x01, file.numberOfFiles * 0x04, fs);
	
	unsigned char nametable[file.nameTableLength];
	memset(nametable, 0x00, file.nameTableLength);
	fseek(file.fs, file.nameTableStartAddress, SEEK_SET);
	fread(nametable, 0x01, file.nameTableLength, file.fs);
	fwrite(nametable, 0x01, file.nameTableLength, fs);

	fclose(file.fs);
	fclose(fs);
}
int IGAR_RebuildArchive(char* input, char* output)
{
	FILE* ifs = fopen(input, "rb");
	FILE* ofs = fopen(output, "wb");

	if(ifs == NULL)
	{
		printf("incorrect input path\n");
		return -1;
	}

	IGAE_File ifile;

	uint32_t readBuffer;

	fseek(ifs, 0x04, SEEK_SET);
	fread(&readBuffer, 0x04, 0x01, ifs);
	ifile.rawVersion = readBuffer;

	ifile.version = IGAE_ReadVersion(ifile, &ifile.rawVersion, false);

	if(ifile.version == 0xFF)
	{
		printf("rebuilding file of version %08X is not supported\n", ifile.rawVersion);
		return -1;
	}

	fseek(ifs, 0x0C, SEEK_SET);
	fread(&readBuffer, 0x04, 0x01, ifs);
	uint32_t important = readBuffer;

	fseek(ifs, 0x10, SEEK_SET);
	fread(&readBuffer, 0x04, 0x01, ifs);
	ifile.numberOfFiles = readBuffer;

	ifile.localFileHeaders = (IGAE_FileDescHeader*)malloc(ifile.numberOfFiles * sizeof(IGAE_FileDescHeader));

	fseek(ifs, 0x14, SEEK_SET);
	fread(&readBuffer, 0x04, 0x01, ifs);
	ifile.nameTableLength = readBuffer;

	fseek(ofs, 0x00, SEEK_SET);
	uint32_t igaMagicNumber = 0x1A414749;
	uint32_t idontknow1 = 0x00000800;
	uint32_t someAddressLol = 0xFFFF;
	uint32_t zeros = 0x00000000;
	uint32_t ones = 0xFFFFFFFF;
	fseek(ofs, 0x00, SEEK_SET);
	fwrite(&igaMagicNumber, 0x04, 0x01, ofs);
	fseek(ofs, 0x04, SEEK_SET);
	fwrite(&ifile.rawVersion, 0x04, 0x01, ofs);
	fseek(ofs, 0x08, SEEK_SET);
	fwrite(&important, 0x04, 0x01, ofs);
	printf("ver: %d", ifile.version);
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_1], SEEK_SET);
	fwrite(&someAddressLol, 0x04, 0x01, ofs);			//cannot be zeroed,between 0xFF and 0xFFFFFF idk
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_NUMBER_OF_FILES], SEEK_SET);
	fwrite(&ifile.numberOfFiles, 0x04, 0x01, ofs);
	fseek(ofs, 0x10, SEEK_SET);
	fwrite(&zeros, 0x04, 0x01, ofs);					//can be zeroed
	fwrite(&zeros, 0x04, 0x01, ofs);					//an be zeroed
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_1], SEEK_SET);
	fwrite(&ifile.unknown, 0x04, 0x01, ofs);			//must be exact, so taken whilst generating rebuild file
	//fwrite(&zeros, 0x04, 0x01, ofs);
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_NAMETABLE_SIZE], SEEK_SET);
	fwrite(&ifile.nameTableLength, 0x04, 0x01, ofs);
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_PADDING_1], SEEK_SET);
	for (size_t i = 0; i < locations[ifile.version][IGAE_LOCATION_PADDING_1_LENGTH]; i++)
	{
		fwrite(&zeros, 0x04, 0x01, ofs);	
	}

	unsigned char unknownDataBuffer[ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH]];
	memset(unknownDataBuffer, 0x00, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH]);
	fseek(ifs, 0x18, SEEK_SET);
	fread(unknownDataBuffer, 0x01, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH], ifs);
	fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_STARTING_LOCATION], SEEK_SET);
	fwrite(unknownDataBuffer, 0x01, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH], ofs);
	
	uint32_t startOfLocalHeaders = ftell(ofs);
	printf("%08X\n", startOfLocalHeaders);

	for (size_t i = 0; i < ifile.numberOfFiles; i++)
	{
		fwrite(&zeros, 0x04, 0x01, ofs);
		fwrite(&zeros, 0x04, 0x01, ofs);
		fwrite(&ones, 0x04, 0x01, ofs);		
	}

	fwrite(&ones, 0x04, 0x01, ofs);
	fwrite(&idontknow1, 0x04, 0x01, ofs);
	fwrite(&ones, 0x04, 0x01, ofs);
	fwrite(&ones, 0x04, 0x01, ofs);
	fwrite(&ones, 0x04, 0x01, ofs);
	fwrite(&ones, 0x04, 0x01, ofs);

	std::string inputStr(input);

	uint16_t strLen = 0;

	for(uint16_t i = 0; i < inputStr.length(); i++)
	{
		if(input[i] == '/' || input[i] == '\\')
		{
			strLen = i;
		}
	}

	char outputFolder[0xFF];
	memset(outputFolder, 0x00, 0xFF);
	memcpy(outputFolder, input, strLen);

	unsigned char fRWBuf[0x40];
	memset(fRWBuf, 0x00, 0x40);

	for (size_t i = 0; i < ifile.numberOfFiles; i++)
	{
		std::string currentFile(outputFolder);
		fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04 + i * 0x04, SEEK_SET);
		fread(&readBuffer, 0x04, 0x01, ifs);
		fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04 + readBuffer + 2, SEEK_SET);
		char readCharacter;
		do
		{
			fread(&readCharacter, 0x01, 0x01, ifs);
			if(readCharacter == 0x00)
			{
				break;
			}
			currentFile += readCharacter;
		} while(true);
		printf("file %d: %s\n", (int)i, currentFile.c_str());
		FILE* cfs = fopen(currentFile.c_str(), "rb");
		fseek(cfs, 0x00, SEEK_END);
		fseek(ofs, ((ftell(ofs) / IGAR_BLOCK_SIZE) + 1) * IGAR_BLOCK_SIZE, SEEK_SET);
		ifile.localFileHeaders[i].size = ftell(cfs);
		ifile.localFileHeaders[i].startingAddress = ftell(ofs);
		fseek(cfs, 0x00, SEEK_SET);
		uint32_t j = 0;
		for (; j < ifile.localFileHeaders[i].size; j += 0x40)
		{
			fread(fRWBuf, 0x01, 0x40, cfs);
			fwrite(fRWBuf, 0x01, 0x40, ofs);
		}
		if(0x40 >= ifile.localFileHeaders[i].size - j && ifile.localFileHeaders[i].size - j > 0)	//If the bytes remaining is in between 0 and 40
		{
			fread(fRWBuf, 0x01, ifile.localFileHeaders[i].size - j, cfs);						//Read the remaining bytes
			fwrite(fRWBuf, 0x01, ifile.localFileHeaders[i].size - j, ofs);						//Write the remaining bytes
		}
		fclose(cfs);
	}
	for (size_t i = 0; i < ifile.numberOfFiles; i++)
	{
		fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * i + locations[ifile.version][IGAE_LOCATION_LOCAL_START], SEEK_SET);
		fwrite(&ifile.localFileHeaders[i].startingAddress, 0x04, 0x01, ofs);
		fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * i + locations[ifile.version][IGAE_LOCATION_LOCAL_SIZE], SEEK_SET);
		fwrite(&ifile.localFileHeaders[i].size, 0x04, 0x01, ofs);
		fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * (i + 1) - 4, SEEK_SET);
		fwrite(&ones, 0x04, 0x01, ofs);
	}

	fseek(ofs, 0x00, SEEK_END);
	fseek(ofs, ((ftell(ofs) / IGAR_BLOCK_SIZE) + 1) * IGAR_BLOCK_SIZE, SEEK_SET);
	ifile.nameTableStartAddress = ftell(ofs);
	fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04, SEEK_SET);

	uint32_t j = 0;
	for (; j < ifile.nameTableLength; j += 0x40)
	{
		fread(fRWBuf, 0x01, 0x40, ifs);
		fwrite(fRWBuf, 0x01, 0x40, ofs);
	}
	if(0x40 >= ifile.nameTableLength - j && ifile.nameTableLength - j > 0)	//If the bytes remaining is in between 0 and 40
	{
		fread(fRWBuf, 0x01, ifile.nameTableLength - j, ifs);				//Read the remaining bytes
		fwrite(fRWBuf, 0x01, ifile.nameTableLength - j, ofs);				//Write the remaining bytes
	}

	printf("%08X\n", (unsigned int)ftell(ofs));
	fseek(ofs, 0x1C, SEEK_SET);
	fwrite(&ifile.nameTableStartAddress, 0x04, 0x01, ofs);

	fclose(ifs);
	fclose(ofs);
}

extern void test(const char* input)
{
	printf("hi\n");
}