#include <fstream>
#include <stdint.h>

#if defined(BUILD_LIB)
#include "libIGAE.h"
#else
#include "C:\Users\jaska\Documents\source\VScode\igArchiveExtractor\lib\libIGAE.h"
#endif

void IGAR_GenerateFile(IGAE_File file, char* output);
int IGAR_RebuildArchive(char* input, char* output);