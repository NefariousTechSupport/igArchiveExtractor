#include <cstring>
#include <stdint.h>
#include <errno.h>
#include <cstddef>
#include <iostream>
#include <cstdlib>

#include "helpers.h"
#include "iAE_io.h"

uint32_t iAE_LoadFile(char* filePath, iAE_File* output)
{
	iAE_File temp;
	temp.fs = fopen(filePath, "rb");
	iAE_SetNumberOfFiles(&temp);
	
	iAE_PopulateDescHeaderArray(&temp);
	temp.nameTableStartAddress = iAE_GetNameTableStartAddr(temp);
	sortArrayDescs(temp.localFileHeaders, temp.numberOfFiles);
	*output = temp;
	return 0;
}

int iAE_SetNumberOfFiles(iAE_File* file)
{
	if(file->fs == NULL) return -1;
	if(iAE_CheckFileHeader(*file))
	{
		fseek(file->fs, 0x0C, SEEK_SET);
		uint32_t numFiles = 0;
		fread(&numFiles, 0x04, 0x01, file->fs);
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
bool iAE_CheckFileHeader(iAE_File file)
{
	if(file.fs == NULL) return -1;

	fseek(file.fs, 0x00, SEEK_SET);

	uint32_t magicNumber = 0;
	fread(&magicNumber, 0x04, 0x01, file.fs);

	if(isBigEndian())
	{
		return magicNumber == 0x4947411A;
	}
	else
	{
		return magicNumber == 0x1A414749;
	}
		
}
uint32_t iAE_SetHeaderValues(iAE_File* file, uint32_t fileNo)
{
	if(file->fs == NULL) return -1;

	uint32_t startingAddress = iAE_GetFileDescsEndingAddr(*file) - ((file->numberOfFiles - fileNo) * 12) + 1;
	uint32_t readBuffer;
	fseek(file->fs, startingAddress, SEEK_SET);
	fread(&readBuffer, 0x04, 0x01, file->fs);
	file->localFileHeaders[fileNo].startingAddress = readBuffer;
	fread(&readBuffer, 0x04, 0x01, file->fs);
	file->localFileHeaders[fileNo].size = readBuffer;
	return 0;
}
uint32_t iAE_GetFileSize(iAE_File file, uint32_t fileNo)
{
	if(file.fs == NULL) return -1;

	uint32_t size = 0;

	fseek(file.fs, file.localFileHeaders[fileNo].startingAddress + 0x04, SEEK_SET);
	fread(&size, 0x04, 0x01, file.fs);

	return size;
}
uint32_t iAE_PopulateDescHeaderArray(iAE_File* file)
{
	file->localFileHeaders = (iAE_FileDescHeader*)malloc(sizeof(iAE_FileDescHeader) * file->numberOfFiles);
	for (size_t i = 0; i < file->numberOfFiles; i++)
	{
		iAE_SetHeaderValues(file, i);
		file->localFileHeaders[i].index = i;
	}
	return 0;
}
uint32_t iAE_ExtractFile(iAE_File file, uint32_t fileNo, const char* outputPath)
{
	fseek(file.fs, 0x00, SEEK_END);

	uint32_t endingAddr = file.localFileHeaders[fileNo].startingAddress + file.localFileHeaders[fileNo].size;

	unsigned char readBuffer[0x40];
	memset(readBuffer, 0x00, 0x40);

	errno = 0;
	FILE* outputfs = fopen(outputPath, "wb");

	if(outputfs == NULL)
	{
		printf("fopen failed with error %d\n", errno);
		return 1;
	}

	fseek(file.fs, file.localFileHeaders[fileNo].startingAddress, SEEK_SET);
	uint32_t i = 0;
	for (; i < file.localFileHeaders[fileNo].size - 0x40; i += 0x40)
	{
		int readres = fread(readBuffer, 0x01, 0x40, file.fs);
		if(readres < 64)
		{
			printf("fread failed with error %d", errno);
			break;
		}
		int writeres = fwrite(readBuffer, 0x01, 0x40, outputfs);

		if(writeres < 64)
		{
			printf("fwrite failed with error %d", errno);
			break;
		}
	}
	//printf("remaining = %08X - %08X = %08X\n", file.localFileHeaders[fileNo].size, i, file.localFileHeaders[fileNo].size - i);
	if(0x40 >= file.localFileHeaders[fileNo].size - i && file.localFileHeaders[fileNo].size - i > 0)
	{
		fread(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, file.fs);
		fwrite(readBuffer, 0x01, file.localFileHeaders[fileNo].size - i, outputfs);
	}
	fclose(outputfs);
	printf("extracted\n");
	return 0;
}


uint32_t iAE_GetFileDescsEndingAddr(iAE_File file)
{
	uint64_t readBuffer = 0;
	uint32_t position = 0;

	for (; position < 100; position++)
	{
		fseek(file.fs, position*8, SEEK_SET);
		fread(&readBuffer, 0x08, 0x01, file.fs);
		if(readBuffer == 0xFFFFFFFFFFFFFFFF)
		{
			break;
		}
	}
	unsigned char currByte = 0x00;
	position *= 8;
	do
	{
		fseek(file.fs, position--, SEEK_SET);
		fread(&currByte, 0x01, 0x01, file.fs);
	} while(currByte == 0xFF);
	position += 5;
	return position;
}
uint32_t iAE_GetNameTableStartAddr(iAE_File file)
{
	fseek(file.fs, 0x20, SEEK_SET);

	fread(&file.nameTableStartAddress, 0x04, 0x01, file.fs);

	fseek(file.fs, 0x00, SEEK_END);

	uint32_t fileSize = ftell(file.fs);

	return fileSize - file.nameTableStartAddress;
}