using System.Collections.Generic;
using System.IO;
using IGAE_GUI.IGZ;

namespace IGAE_GUI.Types
{
	public class igObject
	{
		public IGZ_File _container;
		public uint offset;
		public uint name;
		public uint itemCount;
		public uint length;
		public byte[] data;
		public List<object> fields = new List<object>();
		public List<igObject> children = new List<igObject>();

		public static igObject ReadObjectWithoutFields(IGZ_File igz)
		{
			igObject obj = new igObject();
			obj._container = igz;
			obj.offset = (uint)igz.ebr.BaseStream.Position;

			if(igz.version == 0x06 || igz.version == 0x08)
			{
				igz.ebr.ReadUInt16();
				obj.name = igz.ebr.ReadUInt16();
				obj.itemCount = igz.ebr.ReadUInt32();
				ushort flags1 = igz.ebr.ReadUInt16();
				if((flags1 & 0x3000) != 0)
				{
					obj.length = igz.ebr.ReadUInt16();
					igz.ebr.ReadUInt32();
				}
				else
				{
					igz.ebr.ReadUInt32();
					obj.length = igz.ebr.ReadUInt16();
				}
				igz.ebr.ReadUInt32();

				obj.data = igz.ebr.ReadBytes((int)obj.length);
			}
			else
			{
				obj.name = igz.ebr.ReadUInt32();
				igz.ebr.ReadUInt32();
				obj.itemCount = igz.ebr.ReadUInt32();
				igz.ebr.ReadUInt32();
				igz.ebr.ReadUInt16();
				obj.length = igz.ebr.ReadUInt16();
				igz.ebr.ReadUInt32();
				obj.data = igz.ebr.ReadBytes((int)obj.length);
			}
			return obj;
		}
		public virtual void ReadObjectFields()
		{
			_container.ebr.BaseStream.Seek(offset + 0x14, SeekOrigin.Begin);
			for(int i = 0; i < length / 4; i++)
			{
				fields.Add(_container.ebr.ReadUInt32());	//Bad
			}
		}
	}
}
