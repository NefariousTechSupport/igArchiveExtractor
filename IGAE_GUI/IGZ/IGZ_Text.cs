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
			this.fixups = igz.fixups;
		}

		public void ReadStrings()
		{
			ebr.BaseStream.Seek(descriptors.Last().offset + descriptors.Last().unknown1, SeekOrigin.Begin);

			//Console.WriteLine($"{(ebr.BaseStream as FileStream).Name}; IGZ location:{(descriptors.Last().offset + descriptors.Last().unknown1).ToString("X08")} {ebr.BaseStream.Position.ToString("X08")}");

			//Console.WriteLine(BitConverter.ToString(ebr.ReadBytes(0x1000), 0, 0x1000));

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
