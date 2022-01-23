using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	public static class IGZ_Structure
	{
		public static uint[][] locations = new uint[][]
		{
			//Version 0
			new uint[]{},
			//Version 1
			new uint[]{},
			//Version 2
			new uint[]{},
			//Version 3
			new uint[]{},
			//Version 4
			new uint[]{},
			//Version 5
			new uint[]{},
			//Version 6
			new uint[]
			{
				0x0000056C,			//The location of the attributes
				0x00000010,			//The location where the chunk descriptors start
				0x0000001C,			//The location of the start of the tags in the first chunk
				0x000000A0,			//Location of texture meta data in chunk 1
			},
			//Version 7
			new uint[]
			{
				0x0000056C,			//The location of the attributes
				0x00000018,			//The location where the chunk descriptors start
				0x00000000,			//The location of the start of the tags in the first chunk
				0x00000080,			//Location of texture meta data in chunk 1
			},
			//Version 8
			new uint[]
			{
				0x00000224,			//The location of the attributes
				0x00000018,			//The location where the chunk descriptors start
				0x00000000,			//The location of the start of the tags in the first chunk
				0x000000B0,			//Location of texture meta data in chunk 1
			},
			//Version 9
			new uint[]
			{
				0x0000056C,			//The location of the attributes
				0x00000018,			//The location where the chunk descriptors start
				0x00000000,			//The location of the start of the tags in the first chunk
				0x0000006C,			//Location of texture meta data in chunk 1 (technicaally it starts in chunk 2 but i messed up so yeah deal with it)
			},
		};
	}
}
