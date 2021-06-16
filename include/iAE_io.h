#pragma once

#include <fstream>
#include <stdint.h>

typedef struct
{
    uint32_t startingAddress;
    uint32_t size;
    uint32_t index;
} iAE_FileDescHeader;

typedef struct
{
    FILE* fs;
    uint8_t format;
    uint32_t numberOfFiles;
    iAE_FileDescHeader* localFileHeaders;
    uint32_t nameTableStartAddress;
} iAE_File;

uint32_t iAE_LoadFile(char* filePath, iAE_File* output);
int iAE_SetNumberOfFiles(iAE_File* file);
bool iAE_CheckFileHeader(iAE_File file);
uint32_t iAE_SetHeaderValues(iAE_File* file, uint32_t fileNo);
uint32_t iAE_PopulateDescHeaderArray(iAE_File* file);
uint32_t iAE_ExtractFile(iAE_File file, uint32_t fileNo, const char* outputPath);
uint32_t iAE_GetFileDescsEndingAddr(iAE_File file);
uint32_t iAE_GetNameTableStartAddr(iAE_File file);
uint32_t iAE_FindName(iAE_File file, uint32_t fileNo, std::string* output);