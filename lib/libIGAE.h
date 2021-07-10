#pragma once

#ifdef BUILD_LIB
#define IGAE_API __declspec(dllexport)
#else
#define IGAE_API __declspec(dllimport)
#endif

#include <fstream>
#include <stdint.h>

#define IGAE_VER_SSA_WIIU	0x08
#define IGAE_VER_SSA_WII	0x04
#define IGAE_VER_SG			0xFF
#define IGAE_VER_SSF		0x0A
#define IGAE_VER_STT		0x0B
#define IGAE_VER_SSC		0xFF

#define IGAE_TYPE_ARC		NULL
#define IGAE_TYPE_BLD		NULL

#define IGAE_LE				0x00
#define IGAE_BE				0x01

#define IGAE_LOCATION_MAGIC_NUMBER					0x00
#define IGAE_LOCATION_VERSION 						0x01
#define IGAE_LOCATION_UNKNOWN_1						0x02
#define IGAE_LOCATION_NUMBER_OF_FILES				0x03
#define IGAE_LOCATION_NAMETABLE_LOCATION			0x04
#define IGAE_LOCATION_NAMETABLE_SIZE				0x05
#define IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER		0x06
#define IGAE_LOCATION_UNKNOWN_2_STARTING_LOCATION	0x07
#define IGAE_LOCATION_UNKNOWN_2_LENGTH				0x08
#define IGAE_LOCATION_LOCAL_START					0x09
#define IGAE_LOCATION_LOCAL_SIZE					0x0A
#define IGAE_LOCATION_PADDING_1						0x0B
#define IGAE_LOCATION_PADDING_1_LENGTH				0x0C
#define IGAE_LOCATION_ADDRESS_1						0x0D

extern const uint32_t locations[0x06][0x0E];

typedef struct
{
    uint32_t startingAddress;
    uint32_t size;
    uint32_t index;
} IGAE_FileDescHeader;

typedef struct
{
    FILE* fs;
    uint8_t version;
	uint32_t rawVersion;
	uint8_t endianness;
    uint32_t unknown;
    uint32_t numberOfFiles;
    IGAE_FileDescHeader* localFileHeaders;
    uint32_t nameTableStartAddress;
    uint32_t nameTableLength;
} IGAE_File;

extern "C" IGAE_API int IGAE_LoadFile(char* filePath, IGAE_File* output);
extern "C" IGAE_API long long int IGAE_GetNumberOfFiles(IGAE_File file);
extern "C" IGAE_API bool IGAE_CheckFileHeader(IGAE_File* file);
extern "C" IGAE_API long long int IGAE_SetHeaderValues(IGAE_File* file, uint32_t fileNo);
extern "C" IGAE_API void IGAE_PopulateDescHeaderArray(IGAE_File* file);
extern "C" IGAE_API int IGAE_ExtractFile(IGAE_File file, uint32_t fileNo, const char* outputPath);
extern "C" IGAE_API long long int IGAE_GetFileDescsStartingAddr(IGAE_File file);
extern "C" IGAE_API long long int IGAE_GetNameTableStartAddr(IGAE_File file);
extern "C" IGAE_API long long int IGAE_GetNameTableLength(IGAE_File file);
extern "C" IGAE_API long long int IGAE_FindName(IGAE_File file, uint32_t fileNo, std::string* output);
extern "C" IGAE_API int IGAE_ReadVersion(IGAE_File file, uint32_t* rawVersion, bool readFromFile = true);
extern "C" IGAE_API void test();