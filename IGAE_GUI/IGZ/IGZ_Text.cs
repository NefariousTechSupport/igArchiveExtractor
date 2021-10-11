using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI.IGZ
{
	public class IGZ_Text : IGZ_File
	{
		public List<string> texts = new List<string>();

		public IGZ_Text(IGZ_File igz)
		{
			this.version = igz.version;
			this.crc = igz.crc;
			this.attributes = igz.attributes;
			this.descriptors = igz.descriptors;
			this.ebr = igz.ebr;
		}

		public void ReadStrings()
		{
			ebr.BaseStream.Seek(descriptors.Last().offset, SeekOrigin.Begin);

			byte readBuffer;

			do
			{
				readBuffer = 0;
				readBuffer = ebr.ReadByte();
			} while (readBuffer == 0);

			ebr.BaseStream.Position -= 2;

			do
			{
				texts.Add(ebr.ReadUnicodeString());
			} while (texts.Last().Length > 2);

			texts.RemoveAt(texts.Count - 1);
		}

		~IGZ_Text()
		{
			ebr.BaseStream.Close();
			ebr.Close();
		}
	}
}
