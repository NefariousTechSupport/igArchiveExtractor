using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGAE_GUI.Types;

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

		public int objectSectionSpan = 0;

		public StreamHelper ebr;

		public uint version = 0;
		public uint crc = 0;

		public IGZ_Type type;

		public List<IGZ_Descriptor> descriptors = new List<IGZ_Descriptor>();
		public List<IGZ_Fixup> fixups = new List<IGZ_Fixup>();
		public List<string> attributes = new List<string>();
		public igObjectList objectList;
		public uint[] metaoffsets;

		public IGZ_File(){}
		public IGZ_File(Stream sigz)
		{
			sigz.Seek(0x00, SeekOrigin.Begin);
			Init(sigz);
		}

		public IGZ_File(string filePath)
		{
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			Init(fs);
		}
		public void Init(Stream sigz)
		{
			byte[] readBuffer = new byte[0x04];

			sigz.Read(readBuffer, 0x00, 0x04);

			StreamHelper.Endianness endianness = StreamHelper.Endianness.Little;

			if (BitConverter.ToUInt32(readBuffer, 0) == 0x015A4749) endianness = StreamHelper.Endianness.Big;
			else if (BitConverter.ToUInt32(readBuffer, 0) == 0x48475A01) endianness = StreamHelper.Endianness.Little;
			else throw new InvalidOperationException("File is corrupt.");

			ebr = new StreamHelper(sigz, endianness);

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

			fixups.Sort();

			objectList = igObjectList.ReadObjectList(this);
			
			IGZ_RVTB rvtb = fixups.First(x => x.magicNumber == 0x52565442) as IGZ_RVTB;
			for(int i = 0; i < rvtb.count; i++)
			{
				//Console.WriteLine($"checking offset {i.ToString("X04")} or {(descriptors[1].offset + rvtb.offsets[i]).ToString("X08")}");
				if(rvtb.offsets[i] > ebr.BaseStream.Length) break;
				ebr.BaseStream.Seek(rvtb.offsets[i], SeekOrigin.Begin);
				objectList._objects.Add(igObject.ReadObjectWithoutFields(this));
			}

			IGZ_TMET tmet = fixups.First(x => x.magicNumber == 0x544D4554) as IGZ_TMET;
			for(int i = 0; i < rvtb.count; i++)
			{
				if(rvtb.offsets[i] > ebr.BaseStream.Length) break;
				ebr.BaseStream.Seek(rvtb.offsets[i], SeekOrigin.Begin);
				//Console.WriteLine($"{(rvtb.offsets[i]).ToString("X08")}: {objectList._objects[i].name}");

				if(tmet.typeNames[objectList._objects[i].name].Equals("igImage2"))
				{
					objectList._objects[i] = new igImage2(objectList._objects[i]);
					objectList._objects[i].ReadObjectFields();
				}
			}
		}
		void ReadOldFixups()
		{
			uint bytesPassed = IGZ_Structure.locations[version][0x02];

			uint numberOfFixups = ebr.ReadUInt32WithOffset(descriptors[0].offset + 0x10);

			for(uint i = 0; i < numberOfFixups; i++)
			{
				ebr.BaseStream.Seek(descriptors[0].offset + bytesPassed, SeekOrigin.Begin);
				uint fixupMagicNumber = ebr.ReadUInt32();

				switch(fixupMagicNumber)
				{
					case 0x00:
						IGZ_TMET tmet = new IGZ_TMET();
						tmet.Process(ebr, this);
						tmet.magicNumber = 0x544D4554;
						fixups.Add(tmet);
						break;
					case 0x01:
						IGZ_TSTR tstr = new IGZ_TSTR();
						tstr.Process(ebr, this);
						tstr.magicNumber = 0x54535452;
						fixups.Add(tstr);
						break;
					case 0x02:
						IGZ_EXID exid = new IGZ_EXID();
						exid.Process(ebr, this);
						exid.magicNumber = 0x45584944;
						fixups.Add(exid);
						break;
					case 0x03:
						IGZ_EXNM exnm = new IGZ_EXNM();
						exnm.Process(ebr, this);
						exnm.magicNumber = 0x45584E4D;
						fixups.Add(exnm);
						break;
					//case 0x04: (Unknown, Couldn't find any uses)
					case 0x05:
						IGZ_RVTB rvtb = new IGZ_RVTB();
						rvtb.Process(ebr, this);
						rvtb.magicNumber = 0x52565442;
						objectSectionSpan = rvtb.sectionSpan;
						fixups.Add(rvtb);
						break;
					//case 0x06: (Unknown, Packed Ints)
					//case 0x07: (Unknown, Packed Ints)
					//case 0x08: (Unknown)
					//case 0x09: (Unknown, Couldn't find any uses)
					case 0x0A:
						IGZ_TMHN tmhn = new IGZ_TMHN();
						tmhn.Process(ebr, this);
						tmhn.magicNumber = 0x544D484E;
						fixups.Add(tmhn);
						break;
					//case 0x0B: (Unknown, Couldn't find any uses)
					case 0x0C:
						IGZ_MTSZ mtsz = new IGZ_MTSZ();
						mtsz.Process(ebr, this);
						mtsz.magicNumber = 0x4D54535A;
						fixups.Add(mtsz);
						break;
					//case 0x0D: (Unknown, Couldn't find any uses)
					//case 0x0E: (Unknown, Packed Ints)
					//case 0x0F: (Unknown, Packed Ints)
					//case 0x10: (Unknown, Packed Ints)
					default:
						Console.WriteLine($"No case for fixup number {fixupMagicNumber}");
						IGZ_Fixup generic = new IGZ_Fixup();
						generic.Process(ebr, this);
						generic.magicNumber = 0x554E4B4E;
						fixups.Add(generic);
						break;
				}
				bytesPassed += fixups.Last().length;

			}
		}
		void ReadNewFixups()
		{
			uint bytesPassed = 0;
			while(bytesPassed < descriptors[0].size)
			{
				ebr.BaseStream.Seek(descriptors[0].offset + bytesPassed, SeekOrigin.Begin);

				uint fixupMagicNumber = ebr.ReadUInt32();

				switch(fixupMagicNumber)
				{
					case 0x54535452:	//TSTR
					case 0x52545354:	//RTSR
						IGZ_TSTR tstr = new IGZ_TSTR();
						tstr.Process(ebr, this);
						fixups.Add(tstr);
						break;
					case 0x54454D54:	//TMET
						IGZ_TMET tmet = new IGZ_TMET();
						tmet.Process(ebr, this);
						fixups.Add(tmet);
						break;
					case 0x54444550:
					case 0x50454454:
						IGZ_TDEP tdep = new IGZ_TDEP();
						tdep.Process(ebr, this);
						fixups.Add(tdep);
						break;
					case 0x45584944:	//EXID
					case 0x44495845:
						IGZ_EXID exid = new IGZ_EXID();
						exid.Process(ebr, this);
						fixups.Add(exid);
						break;
					case 0x45584E4D:	//EXNM
					case 0x4D4E5845:
						IGZ_EXNM exnm = new IGZ_EXNM();
						exnm.Process(ebr, this);
						fixups.Add(exnm);
						break;
					case 0x4D54535A:	//MTSZ
					case 0x5A53544D:
						IGZ_MTSZ mtsz = new IGZ_MTSZ();
						mtsz.Process(ebr, this);
						fixups.Add(mtsz);
						break;
					case 0x544D484E:	//TMHN
					case 0x4E484D54:
						IGZ_TMHN tmhn = new IGZ_TMHN();
						tmhn.Process(ebr, this);
						fixups.Add(tmhn);
						break;
					case 0x52565442:
					case 0x42545652:
						IGZ_RVTB rvtb = new IGZ_RVTB();
						rvtb.Process(ebr, this);
						objectSectionSpan = rvtb.sectionSpan;
						fixups.Add(rvtb);
						break;
					default:
						Console.WriteLine($"No case for {System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(fixupMagicNumber))}");
						IGZ_Fixup generic = new IGZ_Fixup();
						generic.Process(ebr, this);
						fixups.Add(generic);
						break;
				}
				bytesPassed += fixups.Last().length;
			}
		}
		public void Close()
		{
			ebr.Close();
			ebr.Dispose();
		}
	}
}
