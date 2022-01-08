using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	public struct IGA_Descriptor
	{
		public uint startingAddress;
		public uint size;
		public uint mode;
		public string path;
		public uint index;
		public uint chunkPropertiesOffset;
	}
}
