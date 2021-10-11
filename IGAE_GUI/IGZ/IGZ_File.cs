using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI.IGZ
{
	public class IGZ_File
	{
		public struct IGZ_Descriptor
		{
			public uint offset;
			public uint size;
			public uint unknown1;
			public uint unknown2;
		}

		public enum IGZ_Type
		{
			Text,
			//Texture,
			//Model,
			Unknown
		}

		public StreamHelper ebr;

		public uint version = 0;
		public uint crc = 0;

		public IGZ_Type type;

		public List<IGZ_Descriptor> descriptors = new List<IGZ_Descriptor>();

		public List<string> attributes = new List<string>();

		public IGZ_File(){}

		public IGZ_File(string filePath)
		{
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

			byte[] readBuffer = new byte[0x04];

			fs.Read(readBuffer, 0x00, 0x04);

			StreamHelper.Endianness endianness = StreamHelper.Endianness.Little;

			if (BitConverter.ToUInt32(readBuffer, 0) == 0x015A4749) endianness = StreamHelper.Endianness.Big;
			else if (BitConverter.ToUInt32(readBuffer, 0) == 0x48475A01) endianness = StreamHelper.Endianness.Little;
			else throw new InvalidOperationException("File is corrupt.");

			ebr = new StreamHelper(fs, endianness);

			version = ebr.ReadUInt32();
			crc = ebr.ReadUInt32();

			ebr.ReadUInt32();   //Skip the padding

			while(true)
			{
				IGZ_Descriptor currentRead = new IGZ_Descriptor();
				currentRead.offset = ebr.ReadUInt32();
				currentRead.size = ebr.ReadUInt32();
				currentRead.unknown1 = ebr.ReadUInt32();
				currentRead.unknown2 = ebr.ReadUInt32();

				if (currentRead.offset != 0)
				{
					descriptors.Add(currentRead);
				}
				else break;
			}

			ebr.BaseStream.Seek(IGZ_Structure.locations[version][0x00], SeekOrigin.Begin);		//Go to the attributes

			do
			{
				attributes.Add(ebr.ReadString());
			} while (attributes.Last().Length > 1);

			if(attributes.Contains("String") && attributes.Contains("Text"))
			{
				type = IGZ_Type.Text;
			}
		}
		~IGZ_File()
		{
			ebr.Close();
		}
	}
}
