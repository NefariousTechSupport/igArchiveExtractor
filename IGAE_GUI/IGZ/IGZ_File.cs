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
			Unknown,
			Text,
			Texture,
			Model		//Unsupported
		}

		public StreamHelper ebr;

		public uint version = 0;
		public uint crc = 0;

		public IGZ_Type type;

		public List<IGZ_Descriptor> descriptors = new List<IGZ_Descriptor>();
		public List<IGZ_Fixup> fixups = new List<IGZ_Fixup>();

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

			ebr.BaseStream.Seek(IGZ_Structure.locations[version][0x01], SeekOrigin.Begin);

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

			if(version > 0x06)
			{
				ReadNewFixups();
			}
			else
			{
				ReadOldFixups();
			}

			IGZ_TMET tmet = fixups.First(x => x.GetType() == typeof(IGZ_TMET)) as IGZ_TMET;

			if(tmet.typeNames.Any(x => x == "LanguagePackInfo"))
			{
				Console.WriteLine("Text");
				type = IGZ_Type.Text;
			}
			else if(tmet.typeNames.Contains("igImage2"))
			{
				Console.WriteLine("Texture");
				type = IGZ_Type.Texture;
			}
			else
			{
				Console.WriteLine("Unknown IGZ, attributes include:");
				for(int i = 0; i < tmet.typeNames.Length; i++)
				{
					Console.WriteLine($"    - {tmet.typeNames[i]}");
				}
			}
		}
		~IGZ_File()
		{
			ebr.Close();
		}
		void ReadOldFixups()
		{
			uint bytesPassed = IGZ_Structure.locations[version][0x02];
			ebr.BaseStream.Seek(descriptors[0].offset + bytesPassed, SeekOrigin.Begin);
			
			IGZ_TMET tmet = new IGZ_TMET();

			tmet.offset = (uint)ebr.BaseStream.Position;
			tmet.sectionCount = ebr.ReadUInt32();
			tmet.length = ebr.ReadUInt32();
			tmet.startOfData = 0x0C;

			tmet.typeNames = new string[tmet.sectionCount];

			ebr.BaseStream.Seek(tmet.offset + tmet.startOfData, SeekOrigin.Begin);

			for(uint i = 0; i < tmet.sectionCount; i++)
			{
				tmet.typeNames[i] = ebr.ReadString();
				Console.WriteLine(tmet.typeNames[i]);
			}

			fixups.Add(tmet);
		}
		void ReadNewFixups()
		{
			uint bytesPassed = 0;

			//ebr.BaseStream.Seek(descriptors[0].offset, SeekOrigin.Begin);

			while(bytesPassed < descriptors[0].size)
			{
				ebr.BaseStream.Seek(descriptors[0].offset + bytesPassed, SeekOrigin.Begin);

				uint fixupOffset = (uint)ebr.BaseStream.Position;
				uint fixupMagicNumber = ebr.ReadUInt32();
				uint fixupSectionCount = ebr.ReadUInt32();
				uint fixupSize = ebr.ReadUInt32();
				uint fixupDataStart = ebr.ReadUInt32();

				switch(fixupMagicNumber)
				{
					case 0x54535452:	//TSTR
					case 0x52545354:
						IGZ_TSTR tstr = new IGZ_TSTR();
						tstr.magicNumber = 0x54535452;
						tstr.offset = fixupOffset;
						tstr.sectionCount = fixupSectionCount;
						tstr.length = fixupSize;
						tstr.startOfData = fixupDataStart;
						tstr.strings = new string[tstr.sectionCount];

						ebr.BaseStream.Seek(tstr.offset + tstr.startOfData, SeekOrigin.Begin);

						for(uint i = 0; i < tstr.sectionCount; i++)
						{
							tstr.strings[i] = ebr.ReadString();
							while(ebr.ReadByte() == 0x00){}
							ebr.BaseStream.Seek(-0x01, SeekOrigin.Current);
						}

						fixups.Add(tstr);
						break;
					case 0x544D4554:	//TMET
					case 0x54454D54:
						IGZ_TMET tmet = new IGZ_TMET();
						tmet.magicNumber = 0x54454D54;
						tmet.offset = fixupOffset;
						tmet.sectionCount = fixupSectionCount;
						tmet.length = fixupSize;
						tmet.startOfData = fixupDataStart;
						tmet.typeNames = new string[tmet.sectionCount];

						ebr.BaseStream.Seek(tmet.offset + tmet.startOfData, SeekOrigin.Begin);

						for(uint i = 0; i < tmet.sectionCount; i++)
						{
							tmet.typeNames[i] = ebr.ReadString();
							while(ebr.ReadByte() == 0x00){}
							ebr.BaseStream.Seek(-0x01, SeekOrigin.Current);
							Console.WriteLine(tmet.typeNames[i]);
						}

						if(tmet.typeNames.Any(x => x == "LanguagePackInfo"))
						{
							type = IGZ_Type.Text;
						}

						fixups.Add(tmet);
						break;
					case 0x45584944:	//EXID
					case 0x44495845:
						IGZ_EXID exid = new IGZ_EXID();
						exid.magicNumber = 0x45584944;
						exid.offset = fixupOffset;
						exid.sectionCount = fixupSectionCount;
						exid.length = fixupSize;
						exid.startOfData = fixupDataStart;
						exid.hashes = new uint[exid.sectionCount];
						exid.types = new uint[exid.sectionCount];
						ebr.BaseStream.Seek(exid.offset + exid.startOfData, SeekOrigin.Begin);
						for(uint i = 0; i < exid.sectionCount; i++)
						{
							exid.hashes[i] = ebr.ReadUInt32();
							exid.types[i] = ebr.ReadUInt32();
						}
						fixups.Add(exid);
						break;
					default:
						Console.WriteLine($"No case for {fixupMagicNumber.ToString("X08")}");
						break;
				}
				bytesPassed += fixupSize;
			}
		}
	}
}
