#include <iostream>
#include <fstream>
#include <cstring>
#include <stdint.h>
#include <stdlib.h>
#include <string>

#include "IGAE.h"
#include "IGAR.h"
#include "helpers.h"

#define IGAE_VERSION_NUMBER "0.06"

void printHelpMessage();

int main(int argc, char** argv)
{
	if(argc == 1)
	{
		printHelpMessage();
		return -1;
	}

	int iterator = 1;

	IGAE_File file;											//Contains data about the file

	uint8_t inputIndex = 0;
	uint8_t outputIndex = 0;
	uint8_t rebuildIndex = 0;

	for (; iterator < argc; iterator++)
	{
		if(argv[iterator][0] == '-')
		{
			switch (argv[iterator][1])
			{
				case 'i':
					if(inputIndex > 0)
					{
						printf("Too many inputs specified\n");
						return -1;
					}

					inputIndex = iterator + 1;
					break;
				case 'o':
					if(outputIndex > 0)
					{
						printf("Too many outputs specified\n");
						return -1;
					}

					outputIndex = iterator + 1;
					break;
				case 'r':
					if(rebuildIndex > 0)
					{
						printf("Too many rebuild inputs specified\n");
						return -1;
					}

					rebuildIndex = iterator + 1;
					break;
			default:
				printf("unrecognised option.\n");
				printHelpMessage();
				break;
			}
		}
	}
	
	if(inputIndex > 0)
	{
		if(IGAE_LoadFile(argv[inputIndex], &file) != 0)			//Load and interpret the file's data, checking the output in the case of an error
		{
			return -1;
		}

		std::string rawFileName;								//This will store the raw name of the file, it's to be somewhat processed as it contains "c:", which's isn't allowed for normal filenames
		std::string outputPath(argv[outputIndex]);				//Contains the final output path of the file being extracted


		for (size_t i = 0; i < file.numberOfFiles; i++)			//Loop over every file in the archive
		{
			outputPath = argv[outputIndex];						//Reset the output path variable
			IGAE_FindName(file, i, &rawFileName);				//Find and set the name of the file being extracted
			outputPath += (rawFileName.c_str() + 0x02);			//Add the file being extracted's filepath to the user's desired filepath
			makeFolders((char*)outputPath.c_str());				//Make the folders that are in the file being extracted's filepath if they do not already exist
			printf("extracting file %d of starting location %08X and size %08X to path \"%s\"... ", (int)i, file.localFileHeaders[i].startingAddress, file.localFileHeaders[i].size, outputPath.c_str());		//Print a message to the user
			IGAE_ExtractFile(file, i, outputPath.c_str());		//Extract the file
		}

		char rebuildFileOutput[0xFF];
		memset(rebuildFileOutput, 0x00, 0xFF);
		sprintf(rebuildFileOutput, "%s/rebuild.igae", argv[outputIndex]);
		IGAR_GenerateFile(file, rebuildFileOutput);

		fclose(file.fs);										//Close the file

		return 0;
	}

	if(rebuildIndex > 0)
	{
		IGAR_RebuildArchive(argv[rebuildIndex], argv[outputIndex]);
		return 0;
	}

	printf("please specify an input file\n");
	return 0;
}

void printHelpMessage()
{
	printf(
		"\n"
		"igArchiveExtractor version %s, created by NefariousTechSupport\n"
		"\n"
		"Usage:\n"
		"    igArchiveExtractor -i \"[path to .arc file]\" -o \"[path to output folder without '/' or '\\' at end]\"\n"
		"\n"
		"Supported Games:\n"
		"- Skylanders Spyro's Adventure (Wii/Wii U)\n"
		"- Skylanders Trap Team\n"
		"\n",
		IGAE_VERSION_NUMBER
		);
}