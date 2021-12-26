using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashDepot;

namespace IGAE_GUI
{
	struct IGA_Descriptor
	{
		public uint startingAddress;
		public uint size;
		public uint mode;
		public string path;
		public uint hash
		{
			get
			{
				return Fnv1a.Hash32(System.Text.Encoding.ASCII.GetBytes(path));
			}
		}
		public uint index;
		public uint chunkPropertiesOffset;
	}
}
