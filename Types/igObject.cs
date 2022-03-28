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

		public static igObject ReadObjectWithoutFields(IGZ_File igz)
		{
			igObject obj = new igObject();
			obj._container = igz;
			obj.offset = (uint)igz.ebr.BaseStream.Position;
			obj.name = igz.ebr.ReadUInt32();

			return obj;
		}
		public virtual void ReadObjectFields(){}
	}
}
