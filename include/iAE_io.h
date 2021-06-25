#pragma once

#include <fstream>
#include <stdint.h>

#define IAE_FILE_VER_SSA_WIIU	0x08
#define IAE_FILE_VER_SSA_WII	0xFF
#define IAE_FILE_VER_SG			0xFF
#define IAE_FILE_VER_STT		0x0B

#define IAE_FILE_TYPE_ARC		NULL
#define IAE_FILE_TYPE_BLD		NULL

#define IAE_FILE_LOCATION_MAGIC_NUMBER					0x00
#define IAE_FILE_LOCATION_VERSION 						0x01
#define IAE_FILE_LOCATION_NUMBER_OF_FILES				0x03
#define IAE_FILE_LOCATION_NAMETABLE_LOCATION			0x04
#define IAE_FILE_LOCATION_NAMETABLE_SIZE				0x05
#define IAE_FILE_LOCATION_LENGTH_OF_LOCAL_HEADER		0x06
#define IAE_FILE_LOCATION_UNKNOWN_2_STARTING_LOCATION	0x07
#define IAE_FILE_LOCATION_UNKNOWN_2_LENGTH				0x08
#define IAE_FILE_LOCATION_LOCAL_START					0x09
#define IAE_FILE_LOCATION_LOCAL_SIZE					0x0A

const uint32_t locations[0x04][0x0B]
{
	//SSA WII U
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00000008,		//Unknown, presumed to be type
		0x0000000C,		//Number of Files
		0x0000001C,		//Nametable Location
		0x00000020,		//Nametable Size
		0x0000000C,		//Length of indiviual local file header
		0x00000034,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000004		//Position of a local file's size inside of a local header
	},
	//SSA WII
	{
		NULL,			//Magic Number
		NULL,			//Version
		NULL,			//Unknown, presumed to be type
		NULL,			//Number of Files
		NULL,			//Nametable Location
		NULL			//Nametable Size
	},
	//SG
	{
		NULL,			//Magic Number
		NULL,			//Version
		NULL,			//Unknown, presumed to be type
		NULL,			//Number of Files
		NULL,			//Nametable Location
		NULL			//Nametable Size
	},
	//STT
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		NULL,			//Unknown, presumed to be type
		0x0000000C,		//Number of Files
		0x00000028,		//Nametable Location
		0x00000030,		//Nametable Size
		0x00000010,		//Length of individual local file header
		0x00000038,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000008		//Position of a local file's size inside of a local header
	}
};

typedef struct
{
    uint32_t startingAddress;
    uint32_t size;
    uint32_t index;
} iAE_FileDescHeader;

typedef struct
{
    FILE* fs;
    uint8_t version;
    uint8_t type;
    uint8_t format;
    uint32_t numberOfFiles;
    iAE_FileDescHeader* localFileHeaders;
    uint32_t nameTableStartAddress;
} iAE_File;

uint32_t iAE_LoadFile(char* filePath, iAE_File* output);
uint32_t iAE_GetNumberOfFiles(iAE_File file);
bool iAE_CheckFileHeader(iAE_File file);
uint32_t iAE_SetHeaderValues(iAE_File* file, uint32_t fileNo);
uint32_t iAE_PopulateDescHeaderArray(iAE_File* file);
uint32_t iAE_ExtractFile(iAE_File file, uint32_t fileNo, const char* outputPath);
uint32_t iAE_GetFileDescsStartingAddr(iAE_File file);
uint32_t iAE_GetNameTableStartAddr(iAE_File file);
uint32_t iAE_FindName(iAE_File file, uint32_t fileNo, std::string* output);
uint8_t iAE_ReadVersion(iAE_File file);