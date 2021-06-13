#include <iostream>
#include <fstream>
#include <cstring>
#include <stdint.h>
#include <stdlib.h>
#include <string>

#include "iAE_io.h"
#include "helpers.h"

int main(int argc, char** argv)
{
	if(argc == 0)
	{
		return -1;
	}

	int iterator = 1;

	iAE_File file;

	iAE_LoadFile(argv[iterator], &file);

	char outputPath[255];

	iterator++;

	for (size_t i = 0; i < file.numberOfFiles; i++)
	{
		sprintf(outputPath, "%s/%d.dat", argv[iterator], (int)i);
		printf("extracting file %d of size %08X to path \"%s\"... ", (int)i, file.localFileHeaders[i].size, outputPath);
		iAE_ExtractFile(file, i, outputPath);
	}
	
	fclose(file.fs);

	return 0;
}