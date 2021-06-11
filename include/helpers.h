#pragma once

#include "iAE_io.h"
#include <stdint.h>

#define invertByteOrder_u32(a) ((a & 0xFF000000) >> 24) | ((a & 0x00FF0000) >> 8) | ((a & 0x0000FF00) << 8) | ((a & 0x000000FF) << 24)
#define ucharArrToU32(a) (a[3] << 24) | (a[2] << 16) | (a[1] << 8) | (a[0])

bool isBigEndian();
void sortArrayDescs(iAE_FileDescHeader headers[], uint32_t length);