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
	//FILE* fs = fopen("C:\\Users\\jaska\\Emulators\\cemu_1.22.12\\mlc01\\usr\\title\\00050000\\10142d00\\content\\level\\Title.arc", "rb");

	iAE_LoadFile(argv[iterator], &file);

	char outputPath[255];
	char fileNumber[127];

	iterator++;

	for (size_t i = 0; i < file.numberOfFiles; i++)
	{
		itoa(i, fileNumber, 10);
		sprintf(outputPath, "%s%s%s%s", argv[iterator], "/", fileNumber, ".dat");
		printf("%s\n", outputPath);
		iAE_ExtractFile(file, i, outputPath);
	}
	
	fclose(file.fs);

	return 0;
}