using System;
using System.IO;
using System.IO.Compression;
using SevenZip.Compression.LZMA;

namespace IGAE_GUI
{
	class IGAE_File
	{
		StreamHelper stream;
		readonly string name;
		public IGA_Descriptor[] localFileHeaders;

		public IGAE_Version _version;
		public uint numberOfFiles;
		public uint chunkAlignment;
		public uint nametableLocation;
		public uint nametableLength;
		public string[] names;

		public IGAE_File(string filepath, IGAE_Version version)
		{
			FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

			name = filepath;

			byte[] magicNumber = new byte[4];

			fs.Read(magicNumber, 0x00, 0x04);

			StreamHelper.Endianness endianness;

			if (BitConverter.ToUInt32(magicNumber, 0) == 0x4947411A) endianness = StreamHelper.Endianness.Big;
			else if (BitConverter.ToUInt32(magicNumber, 0) == 0x1A414749) endianness = StreamHelper.Endianness.Little;
			else throw new InvalidOperationException("File is corrupt.");

			stream = new StreamHelper(fs, endianness);

			_version = version;

			numberOfFiles = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.NumberOfFiles]);
			chunkAlignment = stream.ReadUInt32WithOffset(0x00000010);
			nametableLocation = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.NametableLocation]);
			nametableLength = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.NametableLength]);

			localFileHeaders = new IGA_Descriptor[numberOfFiles];
			names = new string[numberOfFiles];

			Console.WriteLine(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLocation] + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLength] * numberOfFiles);

			for (uint i = 0; i < numberOfFiles; i++)
			{
				names[i] = stream.ReadStringFromOffset(nametableLocation + stream.ReadUInt32WithOffset(nametableLocation + 0x04 * i));

				localFileHeaders[i].startingAddress = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLocation] + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLength] * numberOfFiles + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.LocalHeaderLength] * i + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.FileStartInLocal]);
				localFileHeaders[i].size = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLocation] + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLength] * numberOfFiles + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.LocalHeaderLength] * i + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.FileLengthInLocal]);
				localFileHeaders[i].mode = stream.ReadUInt32WithOffset(IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLocation] + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ChecksumLength] * numberOfFiles + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.LocalHeaderLength] * i + IGAE_Globals.headerData[_version][(uint)IGAE_HeaderData.ModeInLocal]);
				localFileHeaders[i].path = names[i];

				//For some reason the 3ds games have mode 0x10000000 as lzma whereas ssf has it as 0x20000000, so yeah this exists to make that work
				if (_version == IGAE_Version.SkylandersSpyrosAdventureWii && this.localFileHeaders[i].mode == 0x10000000)
				{
					localFileHeaders[i].mode = 0x20000000;
				}

				localFileHeaders[i].index = i;
			}
		}

		public void ExtractFile(uint index, string outputDir, out int res, bool trueName = true)
		{
			string outputFilePath;			

			if(trueName && Path.GetExtension(name) == ".bld")
			{
				outputFilePath = Path.ChangeExtension(name, null) + "/" +  names[index].Substring(names[index][1] == ':' ? 2 : 0);
			}
			else
			{
				outputFilePath = outputDir + names[index].Substring(names[index][1] == ':' ? 2 : 0);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

			FileStream outputfs = null;
			outputfs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

			switch (localFileHeaders[index].mode >> 24)
			{
				case 0x00:
				case 0x10:
					{
						uint compressedSize = stream.ReadUInt16WithOffset(localFileHeaders[index].startingAddress, StreamHelper.Endianness.Little);
						MemoryStream ms = new MemoryStream((int)compressedSize);
						byte[] compressedBytes = stream.ReadBytes((int)compressedSize);
						ms.Write(compressedBytes, 0x00, (int)compressedSize);
						ms.Seek(0x00, SeekOrigin.Begin);
						DeflateStream decompressionStream = new DeflateStream(ms, CompressionMode.Decompress, true);
						decompressionStream.CopyTo(outputfs);
						outputfs.Close();
						res = 0;
					}
					break;
				case 0x20:
					{
						//The following was adapted from https://github.com/KillzXGaming/Switch-Toolbox/blob/master/File_Format_Library/FileFormats/CrashBandicoot/IGA_PAK.cs

						Decoder decoder = new Decoder();

						stream.BaseStream.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);

						//uint chunkSize = (((int)_version & 0x000000FF) < 0x09) ? 0x00008000u : 0x00800000u;
						uint chunkSize = 0x00008000u;

						uint attempts = 0;

						uint bytesDecompressed = 0;

						while (bytesDecompressed < localFileHeaders[index].size)
						{
							uint compressedSize;

							if (((uint)_version & 0x000000FF) <= 0x0B)
							{
								compressedSize = stream.ReadUInt16(StreamHelper.Endianness.Little);
							}
							else
							{
								compressedSize = stream.ReadUInt32(StreamHelper.Endianness.Little);
							}

							if(_version == IGAE_Version.SkylandersTrapTeam)
							{
								compressedSize += 4;
							}

							byte[] properties = stream.ReadBytes(5);

							if(properties[0] == 0x5D && BitConverter.ToUInt32(properties, 0x01) <= chunkSize)
							{
								Console.WriteLine($"Decompressing file {index}; {bytesDecompressed.ToString("X08")}/{localFileHeaders[index].size.ToString("X08")}; {stream.BaseStream.Position.ToString("X08")}");
								decoder.SetDecoderProperties(properties);

								//Console.WriteLine($"{index.ToString("X08")}; cosize: {compressedSize.ToString("X08")}; position: {(fs.Position - 7).ToString("X08")}");
								byte[] compressedBytes = new byte[compressedSize];
								stream.Read(compressedBytes, 0x00, (int)compressedSize);

								uint def_block = (uint)Math.Min(chunkSize, localFileHeaders[index].size - bytesDecompressed);

								if(((int)localFileHeaders[index].size - (int)bytesDecompressed) <= 10)
								{
									break;
								}

								MemoryStream ms = new MemoryStream(compressedBytes);

								try
								{
									decoder.Code(ms, outputfs, compressedSize, def_block, null);
								}
								catch(Exception e)
								{
									Console.WriteLine($"{e.Message} but let's ignore that");
									break;
								}

								bytesDecompressed += def_block;

							}
							else
							{
								Console.WriteLine($"Uncompressed chunk of size {chunkSize.ToString("X08")}, loc {stream.BaseStream.Position.ToString("X08")}");
								byte[] uncompressedData = new byte[chunkSize];
								stream.BaseStream.Read(uncompressedData, 0x00, (int)chunkSize);
								//stream.ReadBytes((int)chunkSize);
								outputfs.Write(uncompressedData, 0x00, (int)chunkSize);

								bytesDecompressed += chunkSize;
							}

							if((((stream.BaseStream.Position - 0x10) / chunkAlignment) + 0x01) * chunkAlignment > stream.BaseStream.Length)
							{
								throw new Exception("File truncated");
							}
							else
							{
								stream.BaseStream.Seek((((stream.BaseStream.Position - 0x10) / chunkAlignment) + 0x01) * chunkAlignment, SeekOrigin.Begin);
							}
							attempts++;
						}

						outputfs.Close();
						res = 0;
					}
					break;
				case 0x30:
					{
						//This functions like 0xFF except the size is stored at the start.
						//I don't know why either.
						//We're just gonna subtract 2 from the size and start at localFileHeaders[index].startingAddress + 4 so as not to break compatibility with any programs

						uint size = stream.ReadUInt16(StreamHelper.Endianness.Little) - 2u;

						stream.BaseStream.Seek(0x02, SeekOrigin.Current);

						byte[] buffer = stream.ReadBytes((int)size);

						outputfs.Write(buffer, 0x00, (int)size);

						outputfs.Close();
						res = 0;
					}
					break;
				case 0xFF:
					{
						byte[] buffer = stream.ReadBytes((int)localFileHeaders[index].size);

						outputfs.Write(buffer, 0x00, (int)localFileHeaders[index].size);

						outputfs.Close();
						res = 0;
					}
					break;
				default:
					res = -1;
					break;
			}
		}
		~IGAE_File()
		{
			stream.Close();
		}
	}
}
