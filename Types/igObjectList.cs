using System.Collections.Generic;
using System.Linq;
using IGAE_GUI.IGZ;
using System;
using System.IO;

namespace IGAE_GUI.Types
{
	public class igObjectList : igObject
	{
		public List<igObject> _objects;
		public igObjectList(igObject list)
		{
			_container = list._container;
			offset = list.offset;
			name = list.name;
			itemCount = list.itemCount;
			length = list.length;
			_objects = new List<igObject>();
		}
		public igObject FindObject(uint offset)
		{
			try
			{
				return _objects.First(x => x.offset == offset);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public static igObjectList ReadObjectList(IGZ_File igz)
		{
			uint listOffset = igz.descriptors[1].offset;
			if(igz.version == 0x06 || igz.version == 0x08)
			{
				listOffset += igz.descriptors[1].unknown2;
			}
			igz.ebr.BaseStream.Seek(listOffset, SeekOrigin.Begin);
			igObjectList temp = new igObjectList(igObject.ReadObjectWithoutFields(igz));
			temp.ReadObjectFields();
			return temp;
		}
		public override void ReadObjectFields()
		{
			_container.ebr.BaseStream.Seek(offset + 0x14, SeekOrigin.Begin);
			for(int i = 0; i < itemCount; i++)
			{
				uint potentialOffset = _container.ebr.ReadUInt32();
				if((potentialOffset & 0x80000000) != 0) continue;
				fields.Add(potentialOffset);
			}
			itemCount = (uint)fields.Count;
		}
	}
}