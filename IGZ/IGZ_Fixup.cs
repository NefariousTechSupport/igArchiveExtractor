using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI.IGZ
{
	public class IGZ_Fixup : IComparable<IGZ_Fixup>
	{
		public uint magicNumber;
		public uint offset;
		public uint count;
		public uint length;
		public uint startOfData;
		public IGZ_File _parent;

		public int CompareTo(IGZ_Fixup other)
		{
			return this.magicNumber.CompareTo(other.magicNumber);
		}

		public virtual void Process(StreamHelper sh, IGZ_File parent)
		{
			_parent = parent;
			sh.BaseStream.Seek(-4, SeekOrigin.Current);
			magicNumber = sh.ReadUInt32();
			offset      = (uint)(sh.BaseStream.Position - 4);

			if(magicNumber <= 0x10)		//Old fixups
			{
				sh.ReadUInt32();
				sh.ReadUInt32();
			}
			else						//New fixups
			{
				if(sh._endianness == StreamHelper.Endianness.Big)
				{
					magicNumber = BitConverter.ToUInt32(BitConverter.GetBytes(magicNumber).Reverse().ToArray());
				}
			}

			count = sh.ReadUInt32();
			length       = sh.ReadUInt32();
			startOfData  = sh.ReadUInt32();				
			sh.BaseStream.Seek(offset + startOfData, SeekOrigin.Begin);
		}
	}

	//String List
	public class IGZ_TSTR : IGZ_Fixup
	{
		public string[] strings;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			strings = new string[count];

			for(uint i = 0; i < count; i++)
			{
				strings[i] = sh.ReadString();
				while(sh.ReadByte() == 0x00){}
				sh.BaseStream.Seek(-0x01, SeekOrigin.Current);
			}
		}
	}

	//Type Names
	public class IGZ_TMET : IGZ_Fixup
	{
		public string[] typeNames;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			typeNames = new string[count];

			for(uint i = 0; i < count; i++)
			{
				typeNames[i] = sh.ReadString();
				while(sh.ReadByte() == 0x00){}
				sh.BaseStream.Seek(-0x01, SeekOrigin.Current);
			}
		}
	}

	//Dependencies
	public class IGZ_TDEP : IGZ_Fixup
	{
		public string[] dependancies;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			dependancies = new string[count];

			for(uint i = 0; i < count; i++)
			{
				dependancies[i] = sh.ReadString();
				while(sh.ReadByte() == 0x00){}
				sh.BaseStream.Seek(-0x01, SeekOrigin.Current);
			}
		}

	}

	//Meta Sizes
	public class IGZ_MTSZ : IGZ_Fixup
	{
		public uint[] metaSizes;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			metaSizes = new uint[count];

			for(uint i = 0; i < count; i++)
			{
				metaSizes[i] = sh.ReadUInt32();
			}
		}
	}

	//External ID
	public class IGZ_EXID : IGZ_Fixup
	{
		public uint[] hashes;
		public uint[] types;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			hashes = new uint[count];
			types = new uint[count];

			for(uint i = 0; i < count; i++)
			{
				hashes[i] = sh.ReadUInt32();
				types[i]  = sh.ReadUInt32();
			}
		}
	}

	//Named Handle List
	public class IGZ_EXNM : IGZ_Fixup
	{
		public uint[] names;
		public uint[] types;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			names = new uint[count];
			types = new uint[count];

			for(uint i = 0; i < count; i++)
			{
				types[i] = sh.ReadUInt32();
				names[i] = (uint)(sh.ReadUInt32() & ~0x80000000);
			}
		}
	}

	//Thumbnails
	public class IGZ_TMHN : IGZ_Fixup
	{
		public uint[] sizes;
		public uint[] offsets;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			sizes = new uint[count];
			offsets = new uint[count];

			for(uint i = 0; i < count; i++)
			{
				sizes[i]    = (uint)(sh.ReadUInt32() & ~0x28000000);
				offsets[i]  = (uint)(sh.ReadUInt32() &  0x00FFFFFF);
			}
		}
	}

	//Root Virtual Table
	public class IGZ_RVTB : IGZ_Fixup
	{
		public uint[] offsets;
		public int sectionSpan = 1;

		public override void Process(StreamHelper sh, IGZ_File parent)
		{
			base.Process(sh, parent);
			offsets = new uint[count];

			uint previousInt = 0;

			bool shiftMoveOrMask = false;

			unsafe
			{
				fixed(byte *fixedData = sh.ReadBytes((int)(length - startOfData)))
				{
					byte* data = fixedData;
					for (int i = 0; i < count; i++)
					{
						uint currentByte;

						if (!shiftMoveOrMask)
						{
							currentByte = (uint)*data & 0xf;
							shiftMoveOrMask = true;
						}
						else
						{
							currentByte = (uint)(*data >> 4);
							data++;
							shiftMoveOrMask = false;
						}
						byte shiftAmount = 3;
						uint unpackedInt = currentByte & 7;
						while ((currentByte & 8) != 0)
						{
							if (!shiftMoveOrMask)
							{
								currentByte = (uint)*data & 0xf;
								shiftMoveOrMask = true;
							}
							else
							{
								currentByte = (uint)(*data >> 4);
								data++;
								shiftMoveOrMask = false;
							}
							unpackedInt = unpackedInt | (currentByte & 7) << (byte)(shiftAmount & 0x1f);
							shiftAmount += 3;
						}

						previousInt = (uint)(previousInt + (unpackedInt * 4) + (parent.version < 9 ? 4 : 0));
						Console.WriteLine(previousInt.ToString("X08"));
						if((previousInt & 0x08000000) != 0x00)
						{
							previousInt = (uint)(previousInt & ~0x08000000) + parent.descriptors[sectionSpan + 1].offset - parent.descriptors[sectionSpan].offset;
							sectionSpan++;
						}
						offsets[i] = previousInt;
					}
				}
			}
		}
	}

	//No idea
	public class IGZ_RSTR : IGZ_Fixup
	{
		//No idea
	}

	//No idea
	public class IGZ_ROFS : IGZ_Fixup
	{
		//No idea
	}

	//No Idea
	public class IGZ_RPID : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_REXT : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_RHND : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_RNEX : IGZ_Fixup
	{
		//No Idea
	}
}
