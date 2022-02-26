namespace IGAE_GUI.IGZ
{
	public class IGZ_File
	{
		public StreamHelper sh;
		public uint version;
		public uint crc;
		public uint platform;
		public uint fixupCount;
		public List<IGZ_SectionHeader> sections = new List<IGZ_SectionHeader>();
		public List<IGZ_igObject> igObjects = new List<IGZ_igObject>();
		public IGZ_igObjectList igObjectList; 

		//Fixups

		public IGZ_TDEP tdep;
		public IGZ_TSTR tstr;
		public IGZ_TMET tmet;
		public IGZ_TMHN tmhn;
		public IGZ_MTSZ mtsz;
		public IGZ_EXID exid;
		public IGZ_EXNM exnm;
		public IGZ_RVTB rvtb;
		public IGZ_RSTT rstt;
		public IGZ_ROFS rofs;
		public IGZ_RPID rpid;
		public IGZ_RHND rhnd;
		public IGZ_RNEX rnex;
		public IGZ_ROOT root;
		public IGZ_ONAM onam;

		public IGZ_File(Stream input)
		{
			byte[] magicNumber = new byte[4];

			input.Seek(0x00, SeekOrigin.Begin);

			input.Read(magicNumber, 0x00, 0x04);

			StreamHelper.Endianness endianness;

			if (BitConverter.ToUInt32(magicNumber, 0) == 0x49475A01) endianness = StreamHelper.Endianness.Little;
			else if (BitConverter.ToUInt32(magicNumber, 0) == 0x015A4749) endianness = StreamHelper.Endianness.Big;
			else
			{
				input.Close();
				throw new InvalidOperationException("File is corrupt.");
			}

			sh = new StreamHelper(input, endianness);

			version = sh.ReadUInt32();
			crc = sh.ReadUInt32();
			platform = sh.ReadUInt32();
			if(version >= 0x07)
			{
				fixupCount = sh.ReadUInt32();
				Console.WriteLine($"{fixupCount} fixups");
			}


			while(true)
			{
				IGZ_SectionHeader currentSection = new IGZ_SectionHeader();

				if(version <= 0x06)
				{
					currentSection.offset = sh.ReadUInt32();
					currentSection.length = sh.ReadUInt32();
					currentSection.alignment = sh.ReadUInt32();
					currentSection.unknown = sh.ReadUInt32();
				}
				else
				{
					currentSection.unknown = sh.ReadUInt32();
					currentSection.offset = sh.ReadUInt32();
					currentSection.length = sh.ReadUInt32();
					currentSection.alignment = sh.ReadUInt32();
				}

				//if(currentSection.unknown == 0xFFFFFFFF) continue;
				if(currentSection.offset == 0x00000000) break;
				//if(currentSection.length == 0x00000000) continue;

				sections.Add(currentSection);
			}

			ProcessFixups();
			ProcessObjects();
		}

		public void ProcessFixups()
		{
			if(version <= 0x06)
			{
				fixupCount = sh.ReadUInt32WithOffset(sections[0].offset + 0x10);
				Console.WriteLine($"{fixupCount} fixups");
			}

			uint bytesRead = (version <= 0x06 ? 0x1Cu : 0x00u);

			for(uint i = 0; i < fixupCount; i++)
			{
				sh.BaseStream.Seek(sections[0].offset + bytesRead, SeekOrigin.Begin);

				uint fixupMagicNumber = sh.ReadUInt32();

				sh.BaseStream.Seek(sections[0].offset + bytesRead, SeekOrigin.Begin);

				uint fixupLength;
				
				Console.WriteLine($"Reading {System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(fixupMagicNumber))}");

				switch(fixupMagicNumber)
				{
					//TDEP
					case 0x50454454:
						tdep = new IGZ_TDEP();
						tdep.Process(sh, this);
						tdep.magicNumber = 0x50454454;
						fixupLength = tdep.length;
						break;

					//TSTR
					case 0x01:
					case 0x52545354:
						tstr = new IGZ_TSTR();
						tstr.Process(sh, this);
						tstr.magicNumber = 0x52545354;
						fixupLength = tstr.length;
						break;

					//TMET
					case 0x00:
					case 0x54454D54:
						tmet = new IGZ_TMET();
						tmet.Process(sh, this);
						tmet.magicNumber = 0x54454D54;
						fixupLength = tmet.length;
						break;

					//TMHN
					case 0x0A:
					case 0x544D484E:
						tmhn = new IGZ_TMHN();
						tmhn.Process(sh, this);
						tmhn.magicNumber = 0x544D484E;
						fixupLength = tmhn.length;
						break;

					//MTSZ
					case 0x0C:
					case 0x5A53544D:
						mtsz = new IGZ_MTSZ();
						mtsz.Process(sh, this);
						mtsz.magicNumber = 0x5A53544D;
						fixupLength = mtsz.length;
						break;

					//EXID
					case 0x02:
					case 0x44495845:
						exid = new IGZ_EXID();
						exid.Process(sh, this);
						exid.magicNumber = 0x44495845;
						fixupLength = exid.length;
						break;

					//EXNM
					case 0x03:
					case 0x4D4E5845:
						exnm = new IGZ_EXNM();
						exnm.Process(sh, this);
						exnm.magicNumber = 0x4D4E5845;
						fixupLength = exnm.length;
						break;

					//RVTB
					case 0x42545652:
						rvtb = new IGZ_RVTB();
						rvtb.Process(sh, this);
						rvtb.magicNumber = 0x42545652;
						fixupLength = rvtb.length;
						break;

					//RSTT
					case 0x54545352:
						rstt = new IGZ_RSTT();
						rstt.Process(sh, this);
						rstt.magicNumber = 0x54545352;
						fixupLength = rstt.length;
						break;

					//ROFS
					case 0x53464F52:
						rofs = new IGZ_ROFS();
						rofs.Process(sh, this);
						rofs.magicNumber = 0x53464F52;
						fixupLength = rofs.length;
						break;

					//RPID
					case 0x44495052:
						rpid = new IGZ_RPID();
						rpid.Process(sh, this);
						rpid.magicNumber = 0x44495052;
						fixupLength = rpid.length;
						break;

					//RHND
					case 0x444E4852:
						rhnd = new IGZ_RHND();
						rhnd.Process(sh, this);
						rhnd.magicNumber = 0x444E4852;
						fixupLength = rhnd.length;
						break;

					//RNEX
					case 0x58454E52:
						rnex = new IGZ_RNEX();
						rnex.Process(sh, this);
						rnex.magicNumber = 0x58454E52;
						fixupLength = rnex.length;
						break;

					//ROOT
					case 0x544F4F52:
						root = new IGZ_ROOT();
						root.Process(sh, this);
						root.magicNumber = 0x544F4F52;
						fixupLength = root.length;
						break;

					//ONAM
					case 0x4D414E4F:
						onam = new IGZ_ONAM();
						onam.Process(sh, this);
						onam.magicNumber = 0x4D414E4F;
						fixupLength = onam.length;
						break;
					
					default:
						IGZ_Fixup generic = new IGZ_Fixup();
						generic.Process(sh, this);
						fixupLength = generic.length;
						break;
				}
				bytesRead += fixupLength;
			}
		}

		public void ProcessObjects()
		{
			sh.BaseStream.Seek(sections[1].offset + (version < 0x09 ? 0x04 : 0x00), SeekOrigin.Begin);
			Console.WriteLine("Seeked");
			igObjectList = IGZ_igObject.ReadWithoutFields<IGZ_igObjectList>(this);
			Console.WriteLine("Read without fields");
			igObjectList.ReadWithFields();
			Console.WriteLine($"Read fields, {igObjectList.numObjects} objects");

			for(int i = 0; i < igObjectList.numObjects; i++)
			{
				Console.WriteLine(igObjectList.offsets[i].ToString("X08"));
			}
		}

		public uint ReadOffset()
		{
			uint offset = sh.ReadUInt32();
			try
			{
				return sections[(int)(offset >> 0x1B) + 1].offset + (offset & 0x7FFFFFF);
			}
			catch(ArgumentOutOfRangeException)
			{
				return 0xFFFFFFFF;
			}
		}
	}
	public struct IGZ_SectionHeader
	{
		public uint offset;
		public uint length;
		public uint unknown;
		public uint alignment;
	}
}