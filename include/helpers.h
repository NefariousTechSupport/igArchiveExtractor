#pragma once

#include "IGAE.h"
#include <stdint.h>

//Invert byte order for a 32 bit value
#define invertByteOrder_u32(a) ((a & 0xFF000000) >> 24) | ((a & 0x00FF0000) >> 8) | ((a & 0x0000FF00) << 8) | ((a & 0x000000FF) << 24)

//#define ucharArrToU32(a) (a[3] << 24) | (a[2] << 16) | (a[1] << 8) | (a[0])

//Check if the system is big endian
bool isBigEndian();
//Make folders in the specified path
void makeFolders(char* filepath);