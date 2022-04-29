using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI.IGZ
{
	public class IGZ_Texture : IGZ_File
	{
		ushort width;
		ushort height;
		ushort depth;
		ushort mipmaps;
		ushort array;
		IGZ_TextureFormat format;
		uint size;

		public IGZ_Texture(IGZ_File igz)
		{
			this.version = igz.version;
			this.crc = igz.crc;
			this.attributes = igz.attributes;
			this.descriptors = igz.descriptors;
			this.ebr = igz.ebr;
			this.fixups = igz.fixups;

			for(int i = 0; i < this.fixups.Count; i++)
			{
				Console.WriteLine(fixups[i].GetType());
			}
		}

		public void ReadImageMetaData()
		{
			ebr.BaseStream.Seek(descriptors[1].offset + descriptors[1].unknown1 + IGZ_Structure.locations[version][0x03], SeekOrigin.Begin);

			width = ebr.ReadUInt16();
			height = ebr.ReadUInt16();
			depth = ebr.ReadUInt16();
			mipmaps = ebr.ReadUInt16();
			array = ebr.ReadUInt16();
			if(version <= 0x06)
			{
				ebr.BaseStream.Seek(0x08, SeekOrigin.Current);
				ushort formatdesc = ebr.ReadUInt16();
				format = SimplifyTextureFormat((IGZ_TextureFormat)formatdesc);
			}
			else
			{
				IGZ_EXID exid = fixups.First(x => x.magicNumber == 0x45584944) as IGZ_EXID;
				format = SimplifyTextureFormat((IGZ_TextureFormat)exid.hashes[0]);
			}

			size = CalculateTextureSize();
		}

		//"Borrowed" from IGZ Model Converter: https://github.com/AdventureT/IgzModelConverter/blob/master/IgzModelConverterGUI/IGZTEX.cpp#L51

		public void ExtractImage(string output)
		{
			ReadImageMetaData();
			Console.WriteLine($"Read MetaData\nSize: {size.ToString("X08")}\nWidth: {width.ToString("X04")}\nHeight: {height.ToString("X04")}");

			FileStream ofs = File.Open(output, FileMode.Create, FileAccess.ReadWrite);
			
			byte[] ddsHeader = new byte[0x0C]
			{
				0x44, 0x44, 0x53, 0x20,
				0x7C, 0x00, 0x00, 0x00,
				0x07, 0x10, 0x08, 0x00
			};

			byte[] zeros = new byte[0x34];

			ofs.Write(ddsHeader, 0x00, 0x0C);
			ofs.Write(BitConverter.GetBytes((uint)height), 0x00, 0x04);
			ofs.Write(BitConverter.GetBytes((uint)width), 0x00, 0x04);
			ofs.Write(BitConverter.GetBytes((uint)size), 0x00, 0x04);
			ofs.Write(zeros, 0x00, 0x34);
			byte[] formatBytes = new byte[0x34]
			{
				0x20, 0x00, 0x00, 0x00,
				0x04, 0x00, 0x00, 0x00,
				0x44, 0x58, 0x54, 0x30,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x10, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			};
			switch(format)
			{
				case IGZ_TextureFormat.dxt1:
					formatBytes[0x0B] = 0x31;
					break;
				case IGZ_TextureFormat.dxt3:
					formatBytes[0x0B] = 0x33;
					break;
				case IGZ_TextureFormat.dxt5:
					formatBytes[0x0B] = 0x35;
					break;
			}
			ofs.Write(formatBytes, 0x00, 0x34);
			ebr.BaseStream.Seek(descriptors.Last().offset + descriptors.Last().unknown1, SeekOrigin.Begin);
			byte[] imageBuffer = new byte[(int)size];
			ebr.BaseStream.Read(imageBuffer, 0x00, (int)size);
			ofs.Write(flipDDSVertically(imageBuffer), 0x00, (int)size);
			ofs.Close();
		}

		//I will cry as this project goes on
		uint CalculateTextureSize()
		{
			switch(format)
			{
				case IGZ_TextureFormat.dxt1:
					return (uint)(Math.Max( 1, ((width+3)/4) ) * Math.Max(1, ( (height + 3) / 4 ) ) * 0x08);		//Taken from the DDS programming guide
				case IGZ_TextureFormat.dxt3:
				case IGZ_TextureFormat.dxt5:
					return (uint)(Math.Max( 1, ((width+3)/4) ) * Math.Max(1, ( (height + 3) / 4 ) ) * 0x10);		//Taken from the DDS programming guide
				default:
					throw new NotImplementedException("This format is unsupported");
			}
		}

		static IGZ_TextureFormat SimplifyTextureFormat(IGZ_TextureFormat _format)
		{
			switch(_format)
			{
				case IGZ_TextureFormat.dxt1:
				case IGZ_TextureFormat.dxt1_android:
				case IGZ_TextureFormat.dxt1_big_ps3:
				case IGZ_TextureFormat.dxt1_big_xenon:
				case IGZ_TextureFormat.dxt1_cafe:
				case IGZ_TextureFormat.dxt1_durango:
				case IGZ_TextureFormat.dxt1_dx:
				case IGZ_TextureFormat.dxt1_dx11:
				case IGZ_TextureFormat.dxt1_linux:
				case IGZ_TextureFormat.dxt1_old:
				case IGZ_TextureFormat.dxt1_osx:
				case IGZ_TextureFormat.dxt1_ps4:
				case IGZ_TextureFormat.dxt1_wgl:
				case IGZ_TextureFormat.dxt1_srgb:
				case IGZ_TextureFormat.dxt1_srgb_android:
				case IGZ_TextureFormat.dxt1_srgb_big_ps3:
				case IGZ_TextureFormat.dxt1_srgb_big_xenon:
				case IGZ_TextureFormat.dxt1_srgb_cafe:
				case IGZ_TextureFormat.dxt1_srgb_durango:
				case IGZ_TextureFormat.dxt1_srgb_dx:
				case IGZ_TextureFormat.dxt1_srgb_dx11:
				case IGZ_TextureFormat.dxt1_srgb_linux:
				case IGZ_TextureFormat.dxt1_srgb_osx:
				case IGZ_TextureFormat.dxt1_srgb_ps4:
				case IGZ_TextureFormat.dxt1_srgb_wgl:
				case IGZ_TextureFormat.dxt1_tile:
				case IGZ_TextureFormat.dxt1_tile_android:
				case IGZ_TextureFormat.dxt1_tile_big_ps3:
				case IGZ_TextureFormat.dxt1_tile_big_xenon:
				case IGZ_TextureFormat.dxt1_tile_cafe:
				case IGZ_TextureFormat.dxt1_tile_durango:
				case IGZ_TextureFormat.dxt1_tile_dx:
				case IGZ_TextureFormat.dxt1_tile_dx11:
				case IGZ_TextureFormat.dxt1_tile_linux:
				case IGZ_TextureFormat.dxt1_tile_osx:
				case IGZ_TextureFormat.dxt1_tile_ps4:
				case IGZ_TextureFormat.dxt1_tile_wgl:
					return IGZ_TextureFormat.dxt1;
				case IGZ_TextureFormat.dxt3:
				case IGZ_TextureFormat.dxt3_android:
				case IGZ_TextureFormat.dxt3_big_ps3:
				case IGZ_TextureFormat.dxt3_big_xenon:
				case IGZ_TextureFormat.dxt3_cafe:
				case IGZ_TextureFormat.dxt3_durango:
				case IGZ_TextureFormat.dxt3_dx:
				case IGZ_TextureFormat.dxt3_dx11:
				case IGZ_TextureFormat.dxt3_linux:
				case IGZ_TextureFormat.dxt3_osx:
				case IGZ_TextureFormat.dxt3_ps4:
				case IGZ_TextureFormat.dxt3_wgl:
				case IGZ_TextureFormat.dxt3_srgb:
				case IGZ_TextureFormat.dxt3_srgb_android:
				case IGZ_TextureFormat.dxt3_srgb_big_ps3:
				case IGZ_TextureFormat.dxt3_srgb_big_xenon:
				case IGZ_TextureFormat.dxt3_srgb_cafe:
				case IGZ_TextureFormat.dxt3_srgb_durango:
				case IGZ_TextureFormat.dxt3_srgb_dx:
				case IGZ_TextureFormat.dxt3_srgb_dx11:
				case IGZ_TextureFormat.dxt3_srgb_linux:
				case IGZ_TextureFormat.dxt3_srgb_osx:
				case IGZ_TextureFormat.dxt3_srgb_ps4:
				case IGZ_TextureFormat.dxt3_srgb_wgl:
				case IGZ_TextureFormat.dxt3_tile:
				case IGZ_TextureFormat.dxt3_tile_android:
				case IGZ_TextureFormat.dxt3_tile_big_ps3:
				case IGZ_TextureFormat.dxt3_tile_big_xenon:
				case IGZ_TextureFormat.dxt3_tile_cafe:
				case IGZ_TextureFormat.dxt3_tile_durango:
				case IGZ_TextureFormat.dxt3_tile_dx:
				case IGZ_TextureFormat.dxt3_tile_dx11:
				case IGZ_TextureFormat.dxt3_tile_linux:
				case IGZ_TextureFormat.dxt3_tile_osx:
				case IGZ_TextureFormat.dxt3_tile_ps4:
				case IGZ_TextureFormat.dxt3_tile_wgl:
					return IGZ_TextureFormat.dxt3;
				case IGZ_TextureFormat.dxt5:
				case IGZ_TextureFormat.dxt5_android:
				case IGZ_TextureFormat.dxt5_big_ps3:
				case IGZ_TextureFormat.dxt5_big_xenon:
				case IGZ_TextureFormat.dxt5_cafe:
				case IGZ_TextureFormat.dxt5_durango:
				case IGZ_TextureFormat.dxt5_dx:
				case IGZ_TextureFormat.dxt5_dx11:
				case IGZ_TextureFormat.dxt5_linux:
				case IGZ_TextureFormat.dxt5_old:
				case IGZ_TextureFormat.dxt5_osx:
				case IGZ_TextureFormat.dxt5_ps4:
				case IGZ_TextureFormat.dxt5_wgl:
				case IGZ_TextureFormat.dxt5_srgb:
				case IGZ_TextureFormat.dxt5_srgb_android:
				case IGZ_TextureFormat.dxt5_srgb_big_ps3:
				case IGZ_TextureFormat.dxt5_srgb_big_xenon:
				case IGZ_TextureFormat.dxt5_srgb_cafe:
				case IGZ_TextureFormat.dxt5_srgb_durango:
				case IGZ_TextureFormat.dxt5_srgb_dx:
				case IGZ_TextureFormat.dxt5_srgb_dx11:
				case IGZ_TextureFormat.dxt5_srgb_linux:
				case IGZ_TextureFormat.dxt5_srgb_osx:
				case IGZ_TextureFormat.dxt5_srgb_ps4:
				case IGZ_TextureFormat.dxt5_srgb_wgl:
				case IGZ_TextureFormat.dxt5_tile:
				case IGZ_TextureFormat.dxt5_tile_android:
				case IGZ_TextureFormat.dxt5_tile_big_ps3:
				case IGZ_TextureFormat.dxt5_tile_big_xenon:
				case IGZ_TextureFormat.dxt5_tile_cafe:
				case IGZ_TextureFormat.dxt5_tile_durango:
				case IGZ_TextureFormat.dxt5_tile_dx:
				case IGZ_TextureFormat.dxt5_tile_dx11:
				case IGZ_TextureFormat.dxt5_tile_linux:
				case IGZ_TextureFormat.dxt5_tile_osx:
				case IGZ_TextureFormat.dxt5_tile_ps4:
				case IGZ_TextureFormat.dxt5_tile_wgl:
					return IGZ_TextureFormat.dxt5;
				default:
					throw new NotImplementedException($"{((uint)_format).ToString("X08")} is unsupported");
			}
		}

		//From https://github.com/ata4/disunity/pull/43/files

		private byte[] flipDDSVertically(byte[] imageData)
		{
			if(format != IGZ_TextureFormat.dxt1 && format != IGZ_TextureFormat.dxt5)
			{
				throw new NotSupportedException("Only DXT1 and DXT5 compressed image are supported. Texture format: " + format);
			}

			byte[] imageDataCopy = new byte[size];

			int mipmapByteOffset = 0;

			int blockByteSize = 8;
			if(format == IGZ_TextureFormat.dxt5)
			{
				blockByteSize = 16;
			}

			// For each texture mipmap plane
			for( int i = 0; i < mipmaps; ++i )
			{
				int byteCount = ( ( width + 3 ) / 4 ) * ( ( height + 3 ) / 4 ) * blockByteSize;
				int widthBlockCount = ( ( width + 3 ) / 4 );
				int heightBlockCount = ( ( height + 3 ) / 4 );
				int blockRowByteCount =  widthBlockCount * blockByteSize;

				// Process one row of block at a time
				for( int j = 0; j < heightBlockCount; ++j )
				{
					int srcRowOffset = mipmapByteOffset + j * blockRowByteCount;
					int dstRowOffset = mipmapByteOffset + ( heightBlockCount - j - 1 ) * blockRowByteCount;

					// Copy each src block row from top to bottom to dst from bottom to top. This flips vertically DXTn block rows.
					Array.Copy(imageData, srcRowOffset, imageDataCopy, dstRowOffset, blockRowByteCount);

					/**
					* Flip vertically pixels of each DXTn block
					* 
					* DXT1 color block layout (64 bits, 4x4 pixel block)
					* bytes 0 and 1: color0
					* bytes 2 and 3: color1
					* bytes 4 to 7: 4x4 2 bits lookup table (32 bits)
					* 
					* DXT5 color + alpha block layout (128 bits, 4x4 pixel block)
					* byte 0 alpha0
					* byte 1 alpha1
					* bytes 2 to 7 4x4 3 bits lookup table (48 bits)
					* bytes 8 to 15 DXT1 block
					* 
					* See: http://www.opengl.org/registry/specs/EXT/texture_compression_s3tc.txt
					*/
					for( int k = 0; k < widthBlockCount; ++k )
					{
						// color block data start position in DTX1 block (no alpha data).
						int blockByteOffset = dstRowOffset + k * blockByteSize + 4;

						if(format == IGZ_TextureFormat.dxt5)
						{
							// 4x4 pixel alpha block vertical flip
							//

							// alpha block data start position
							blockByteOffset = dstRowOffset + k * blockByteSize + 2;

							/**
							* Since row are made of 12 bits (4 pixels of 3 bits each) row data is not byte aligned, so append data in integers, to work with bit masks.
							* In row12 and row34:
							* Nibbles (half-bytes) are ordered like this: 0,1,2,3,4,5,6,7,8,9,10,11
							* After vertical flip, nibbles follow this order: 9,10,11,6,7,8,3,4,5,0,1,2
							*/
							int row12 = ( ( imageDataCopy[ blockByteOffset + 5 ] << 16 ) & 0x00ff0000 ) |
										( ( imageDataCopy[ blockByteOffset + 4 ] << 8 ) & 0x0000ff00 ) |
										( ( imageDataCopy[ blockByteOffset + 3 ] ) & 0x000000ff );
							int row34 = ( ( imageDataCopy[ blockByteOffset + 2 ] << 16 ) & 0x00ff0000 ) |
										( ( imageDataCopy[ blockByteOffset + 1 ] << 8 ) & 0x0000ff00 ) |
										( ( imageDataCopy[ blockByteOffset + 0 ] ) & 0x000000ff );

							// After these two lines nibbles follow this order : 3,4,5,0,1,2,9,10,11,6,7,8
							row12 = ( ( row12 & 0x00000fff ) << 12 ) | ( ( row12 & 0x00fff000 ) >> 12 );
							row34 = ( ( row34 & 0x00000fff ) << 12 ) | ( ( row34 & 0x00fff000 ) >> 12 );

							// Nibbles can now be swapped by pair
							imageDataCopy[ blockByteOffset + 5 ] = (byte)( ( row34 & 0x00ff0000 ) >> 16 );
							imageDataCopy[ blockByteOffset + 4 ] = (byte)( ( row34 & 0x0000ff00 ) >> 8 );
							imageDataCopy[ blockByteOffset + 3 ] = (byte)( ( row34 & 0x000000ff ) );
							imageDataCopy[ blockByteOffset + 2 ] = (byte)( ( row12 & 0x00ff0000 ) >> 16 );
							imageDataCopy[ blockByteOffset + 1 ] = (byte)( ( row12 & 0x0000ff00 ) >> 8 );
							imageDataCopy[ blockByteOffset + 0 ] = (byte)( ( row12 & 0x000000ff ) );

							// color block data start position after alpha block
							blockByteOffset += 10;
						}

						// 4x4 pixel color block vertical flip
						//

						// switch row 1 and 4
						byte temp = imageDataCopy[ blockByteOffset ];
						imageDataCopy[ blockByteOffset ] = imageDataCopy[ blockByteOffset + 3 ];
						imageDataCopy[ blockByteOffset + 3 ] = temp;

						// switch row 2 and 3
						temp = imageDataCopy[ blockByteOffset + 1 ];
						imageDataCopy[ blockByteOffset + 1 ] = imageDataCopy[ blockByteOffset + 2 ];
						imageDataCopy[ blockByteOffset + 2 ] = temp;
					}
				}

				mipmapByteOffset += byteCount;
				width = (ushort)Math.Max( 1 , width / 2 );
				height = (ushort)Math.Max( 1 , height / 2 );
			}

			return imageDataCopy;
		}

		~IGZ_Texture()
		{
			ebr.BaseStream.Close();
			ebr.Close();
		}
	}
}
