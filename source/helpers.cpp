#include "helpers.h"
#include "iAE_io.h"
#include <stdint.h>

bool isBigEndian()
{
	int num = 1;
	return *(char *)&num != 1;
}
bool sortArrayDescs(iAE_FileDescHeader headers[], uint32_t length)
{
	bool sorted = true;
	do
	{	
		sorted = true;
		for (size_t i = 0; i < length - 1; i++)
		{
			//printf("i: %d\n", i);
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