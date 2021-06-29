#pragma once

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

const uint32_t locations[0x06][0x0B]
{
	//SSA WII U
	{
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00000008,		//Unknown
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
		0x00000000,		//Magic Number
		0x00000004,		//Version
		0x00,			//Unknown, presumed to be type
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
		0x00,			//Unknown, presumed to be type
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
		0x00,			//Unknown, presumed to be type
		0x0000000C,		//Number of Files
		0x0000002C,		//Nametable Location
		0x00000030,		//Nametable Size
		0x00000010,		//Length of individual local file header
		0x00000038,		//Unknown data, local file headers start after this data
		0x00000004,		//Unknown data's size
		0x00000000,		//Position of a local file's starting location inside of a local header
		0x00000008		//Position of a local file's size inside of a local header
	},
	//SSC
	{}
};

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

uint32_t IGAE_LoadFile(char* filePath, IGAE_File* output);
uint32_t IGAE_GetNumberOfFiles(IGAE_File file);
bool IGAE_CheckFileHeader(IGAE_File* file);
uint32_t IGAE_SetHeaderValues(IGAE_File* file, uint32_t fileNo);
uint32_t IGAE_PopulateDescHeaderArray(IGAE_File* file);
uint32_t IGAE_ExtractFile(IGAE_File file, uint32_t fileNo, const char* outputPath);
uint32_t IGAE_GetFileDescsStartingAddr(IGAE_File file);
uint32_t IGAE_GetNameTableStartAddr(IGAE_File file);
uint32_t IGAE_GetNameTableLength(IGAE_File file);
uint32_t IGAE_FindName(IGAE_File file, uint32_t fileNo, std::string* output);
uint8_t IGAE_ReadVersion(IGAE_File file, uint32_t* rawVersion);