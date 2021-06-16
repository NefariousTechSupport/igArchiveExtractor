#include <iostream>
#include <fstream>
#include <cstring>
#include <stdint.h>
#include <stdlib.h>
#include <string>

#include "iAE_io.h"
#include "helpers.h"

#define IAE_VERSION_NUMBER "0.04"

void printHelpMessage();

int main(int argc, char** argv)
{
	if(argc != 3)
	{
		printHelpMessage();
		return -1;
	}

	//int iterator = 1;

	iAE_File file;											//Contains data about the file

	iAE_LoadFile(argv[1], &file);							//Load and interpret the file's data

	std::string rawFileName;								//This will store the raw name of the file, it's to be somewhat processed as it contains "c:", which's isn't allowed for normal filenames
	std::string outputPath(argv[2]);						//Contains the final output path of the file being extracted

	for (size_t i = 0; i < file.numberOfFiles; i++)			//Loop over every file in the archive
	{
		outputPath = argv[2];								//Reset the output path variable
		iAE_FindName(file, i, &rawFileName);				//Find and set the name of the file being extracted
		outputPath += (rawFileName.c_str() + 0x02);			//Add the file being extracted's filepath to the user's desired filepath
		makeFolders((char*)outputPath.c_str());				//Make the folders that are in the file being extracted's filepath if they do not already exist
		printf("extracting file %d of size %08X to path \"%s\"... \n", (int)i, file.localFileHeaders[i].size, outputPath.c_str());		//Print a message to the user
		iAE_ExtractFile(file, i, outputPath.c_str());		//Extract the file
	}
	
	fclose(file.fs);										//Close the file

	return 0;
}

void printHelpMessage()
{
	printf(
		"\n"
		"igArchiveExtractor version %s, created by NefariousTechSupport\n"
		"\n"
		"Usage:\n"
		"    igArchiveExtractor \"[path to .arc file]\" \"[path to output folder without '/' or '\\' at end]\"\n"
		"\n"
		"Supported Games:\n"
		"- Skylanders Spyro's Adventure (Wii U)\n",
		"\n"
		IAE_VERSION_NUMBER
		);
}