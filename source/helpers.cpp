#include "helpers.h"
#include "iAE_io.h"
#include <stdint.h>
#include <iostream>
#include <cstdio>
#include <cstring>

#if defined(_WIN32)
#include <fileapi.h>
#include <windows.h>
#endif

//Checks if the system is big endian
bool isBigEndian()
{
	int num = 1;
	return *(char *)&num != 1;
}

//Sorts the files in the archive in order of increasing starting addresses (Yes this uses bubble sort, but don't fret as this function will be removed in a later release)
void sortArrayDescs(iAE_FileDescHeader headers[], uint32_t length)
{
	bool sorted = true;
	do
	{	
		sorted = true;
		for (size_t i = 0; i < length - 1; i++)
		{
			if(headers[i].startingAddress > headers[i + 1].startingAddress)
			{
				iAE_FileDescHeader a = headers[i];
				iAE_FileDescHeader b = headers[i + 1];

				headers[i + 1] = a;
				headers[i] = b;

				sorted = false;
			}
		}
	} while (!sorted);
}

//Creates the folders in the specified filepath
void makeFolders(char* filepath)
{
	std::string temp(filepath);							//Contains the filepath (used to get the length of the file)
	char currentFolder[255];							//The path to the folder being checked



	for (size_t i = 0; i < temp.length(); i++)			//Loop over every character in the specified filepath
	{
		if(filepath[i] == '/' || filepath[i] == '\\')	//If the character is a '/' or a '\' 
		{
			if(i - 1 == 0 && filepath[0] == '.')		//If the folder being checked is just '.', which means the current working directory
			{
				continue;								//Skip to the next iteration
			}

			memset(currentFolder, 0x00, 0xFF);			//Initialise the currentFolder variable
			memcpy(currentFolder, filepath, i);			//Copy the specified filepath to the currentFolder variable up to and including the folder being checked, but no more

			//The following will a folder

#if defined(_WIN32)
			CreateDirectoryA(currentFolder, NULL);		//If folder already exists then an error will be returned, but for all intents and purposes, this'll be ignored
#elif defined(__linux__) || defined(__APPLE__)
			//The following will be replaced for more performant method
			char currentFolderOutput[i];
			memset(currentFolderOutput, 0x00, i);
			sprintf(currentFolderOutput, "mkdir -p %s", currentFolder);
			system(currentFolderOutput);
#endif
		}
	}	
}