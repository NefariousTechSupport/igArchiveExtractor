using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using SevenZip.Compression.LZMA;
using HashDepot;

namespace IGAE_GUI
{
	public class IGA_File
	{
		public StreamHelper stream;
		public readonly string name;
		public IGA_Descriptor[] localFileHeaders;
		public IGA_Version _version;
		public uint numberOfFiles;
		public uint chunkAlignment;
		public uint nametableLocation;
		public uint nametableLength;
		public string[] names;
		public uint[] crc = null;
		public uint slop;
		public uint flags;
		uint chunkPropertiesStart = 0;

		StreamHelper.Endianness outputEndianess = StreamHelper.Endianness.Little;

		public IGA_File(string filepath, IGA_Version version)
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

			numberOfFiles = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NumberOfFiles]);
			chunkAlignment = stream.ReadUInt32WithOffset(0x00000010);
			slop = stream.ReadUInt32WithOffset(0x00000018);
			if(_version == IGA_Version.SkylandersSpyrosAdventureWii)
			{
				chunkAlignment = 0x0800;	//It doesn't store it, i found that funny
			}
			nametableLocation = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLocation]);
			nametableLength = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLength]);
			flags = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.Flags]);

			localFileHeaders = new IGA_Descriptor[numberOfFiles];
			names = new string[numberOfFiles];
			crc = new uint[numberOfFiles];

			for (uint i = 0; i < numberOfFiles; i++)
			{
				names[i] = stream.ReadStringFromOffset(nametableLocation + stream.ReadUInt32WithOffset(nametableLocation + 0x04 * i));
				crc[i] = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + i * 0x04);

				localFileHeaders[i].startingAddress = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileStartInLocal]);
				localFileHeaders[i].size = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileLengthInLocal]);
				localFileHeaders[i].mode = stream.ReadUInt32WithOffset(IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ModeInLocal]);
				localFileHeaders[i].path = names[i];
				localFileHeaders[i].chunkPropertiesOffset = localFileHeaders[i].mode & 0xFFFFFF;

				//For some reason the 3ds games have mode 0x10000000 as lzma whereas ssf has it as 0x20000000, so yeah this exists to make that work
				if (_version == IGA_Version.SkylandersSpyrosAdventureWii && this.localFileHeaders[i].mode == 0x10000000)
				{
					localFileHeaders[i].mode = 0x20000000;
				}

				localFileHeaders[i].index = i;
			}
			chunkPropertiesStart = (uint)stream.BaseStream.Position;
		}

		public IGA_File(IGA_Version version, string[] filenames, uint[]? crcs)
		{
			_version = version;
			switch(_version)
			{
				case IGA_Version.SkylandersSpyrosAdventureWii:
				case IGA_Version.SkylandersSpyrosAdventureWiiU:
				case IGA_Version.SkylandersTrapTeam:
					outputEndianess = StreamHelper.Endianness.Little;
					break;
				case IGA_Version.SkylandersSwapForce:
				case IGA_Version.SkylandersSuperChargers:
					outputEndianess = StreamHelper.Endianness.Big;
					break;
			}
			names = filenames;
			numberOfFiles = (uint)filenames.Length;
			chunkAlignment = 0x0000000800u;
			localFileHeaders = new IGA_Descriptor[numberOfFiles];
			if(crcs != null)
			{
				crc = new uint[numberOfFiles];
				for(uint i = 0; i < numberOfFiles; i++)
				{
					crc[i] = crcs[i];
				}
			}
			for(uint i = 0; i < numberOfFiles; i++)
			{
				if(false)
				{
					throw new NotSupportedException("Compression is not supported");
				}
				else
				{
					localFileHeaders[i].mode = 0xFFFFFFFF;
				}
				localFileHeaders[i].path = names[i];
				localFileHeaders[i].index = i;
			}
		}

		public void ExtractFile(uint index, string outputDir, out int res, bool trueName = true)
		{
			string outputFilePath;			

			if(trueName && (Path.GetExtension(name) == ".bld" || Path.GetExtension(name) == ".pak"))
			{
				outputFilePath = Path.ChangeExtension(name, null) + "/" +  names[index].Substring(names[index][1] == ':' ? 2 : 0);
			}
			else if(!trueName && (Path.GetExtension(name) == ".arc" || Path.GetExtension(name) == ".pak"))
			{
				outputFilePath = outputDir + Path.GetFileName(names[index]);
			}
			else
			{
				outputFilePath = outputDir + names[index].Substring(names[index][1] == ':' ? 2 : 0);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

			FileStream outputfs = null;
			outputfs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

			Console.WriteLine(outputfs.Name);	
			switch (localFileHeaders[index].mode >> 24)
			{
				case 0x00:
				case 0x10:
					{
						uint chunkCount = 0;
						uint nextChunk = localFileHeaders[index].startingAddress;
						uint decompressedBytes = 0;
						uint chunkSize = 0x8000;

						//Very bad code but there's no other way to do thiss

						while(decompressedBytes < localFileHeaders[index].size)
						{
							//stream.BaseStream.Seek(chunkPropertiesStart + localFileHeaders[index].chunkPropertiesOffset * 2 + chunkCount * 2, SeekOrigin.Begin);
							//Console.WriteLine($"Properties: {stream.BaseStream.Position.ToString("X08")}");
							//uint chunkProperties = stream.ReadUInt16();
							stream.BaseStream.Seek(nextChunk, SeekOrigin.Begin);

							try
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

								Console.WriteLine($"Chunk: {stream.BaseStream.Position.ToString("X08")} of size {compressedSize.ToString("X08")}");

								MemoryStream ms = new MemoryStream((int)compressedSize);
								byte[] compressedBytes = stream.ReadBytes((int)compressedSize);
								ms.Write(compressedBytes, 0x00, (int)compressedSize);
								ms.Seek(0x00, SeekOrigin.Begin);
								DeflateStream decompressionStream = new DeflateStream(ms, CompressionMode.Decompress, true);
								decompressionStream.CopyTo(outputfs);
								decompressionStream.Close();
							}
							catch(InvalidDataException)
							{
								stream.BaseStream.Seek(nextChunk, SeekOrigin.Begin);

								Console.WriteLine($"Chunk: {stream.BaseStream.Position.ToString("X08")} of size {chunkSize.ToString("X08")}");
								byte[] uncompressedData = new byte[chunkSize];
								stream.BaseStream.Read(uncompressedData, 0x00, (int)chunkSize);
								outputfs.Write(uncompressedData, 0x00, (int)chunkSize);
							}

							chunkCount++;
							decompressedBytes += 0x8000;		//Bad code that's definitely fail, you just don't know why yet


							nextChunk = (uint)(((int)stream.BaseStream.Position + (chunkAlignment - 1)) / chunkAlignment) * chunkAlignment;

							if(stream.BaseStream.Position % chunkAlignment == 0)
							{
								nextChunk = (uint)stream.BaseStream.Position;
							}

							if(nextChunk > stream.BaseStream.Length)
							{
								throw new Exception($"File truncated, {nextChunk.ToString("X08")} does not exist");
							}
						}
						outputfs.Close();
						res = 0;
					}
					break;
				case 0x20:
					{
						//The following was adapted from https://github.com/KillzXGaming/Switch-Toolbox/blob/master/File_Format_Library/FileFormats/CrashBandicoot/IGA_PAK.cs

						Decoder decoder = new Decoder();

						stream.BaseStream.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);

						//The following is lies and deceit
						//uint chunkSize = (((int)_version & 0x000000FF) < 0x09) ? 0x00008000u : 0x00800000u;

						uint chunkSize = 0x00008000u;

						uint attempts = 0;

						uint bytesDecompressed = 0;

						while (bytesDecompressed < localFileHeaders[index].size)
						{
							uint compressedSize;

							if (((uint)_version & 0x000000FF) <= 0x0B)
							{
								compressedSize = stream.ReadUInt16((_version == IGA_Version.SkylandersSpyrosAdventureWii) ? StreamHelper.Endianness.Big : StreamHelper.Endianness.Little);
							}
							else
							{
								compressedSize = stream.ReadUInt32(StreamHelper.Endianness.Little);
							}

							byte[] properties = stream.ReadBytes(5);

							if(properties[0] == 0x5D && BitConverter.ToUInt32(properties, 0x01) <= chunkSize)
							{
								decoder.SetDecoderProperties(properties);

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
								if (((uint)_version & 0x000000FF) <= 0x0B)
								{
									stream.BaseStream.Seek(-7, SeekOrigin.Current);
								}
								else
								{
									stream.BaseStream.Seek(-9, SeekOrigin.Current);
								}
								byte[] uncompressedData = new byte[chunkSize];
								stream.BaseStream.Read(uncompressedData, 0x00, (int)chunkSize);
								outputfs.Write(uncompressedData, 0x00, (int)chunkSize);

								bytesDecompressed += chunkSize;
							}

							uint nextChunk = (uint)(((int)stream.BaseStream.Position + (chunkAlignment - 1)) / chunkAlignment) * chunkAlignment;

							if(stream.BaseStream.Position % chunkAlignment == 0)
							{
								nextChunk = (uint)stream.BaseStream.Position;
							}

							if(nextChunk > stream.BaseStream.Length)
							{
								throw new Exception("File truncated");
							}
							else
							{
								stream.BaseStream.Seek(nextChunk, SeekOrigin.Begin);
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
						stream.BaseStream.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);

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
		public void Build(string output)
		{
			string[] inputFiles = new string[numberOfFiles];
			for(int i = 0; i < numberOfFiles; i++)
			{
				if((Path.GetExtension(name) == ".bld" || Path.GetExtension(name) == ".pak"))
				{
					inputFiles[i] = Path.ChangeExtension(name, null) + "/" +  names[i].Substring(names[i][1] == ':' ? 2 : 0);
				}
				else
				{
					inputFiles[i] = Path.GetDirectoryName(name) + names[i].Substring(names[i][1] == ':' ? 2 : 0);
				}
			}
			Build(output, inputFiles);
		}
		public unsafe void Build(string output, string[] inputFiles)
		{
			FileStream ofs = new FileStream(output, FileMode.Create, FileAccess.ReadWrite);
			StreamHelper osh = new StreamHelper(ofs);
			osh._endianness = outputEndianess;

			osh.WriteUInt32WithOffset(0x1A414749, 0x00000000);
			osh.WriteUInt32((uint)_version & 0xFF);
			osh.WriteUInt32(numberOfFiles * (4 + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength]));
			osh.WriteUInt32(numberOfFiles);
			osh.WriteUInt32(0x00000800);
			osh.WriteUInt32(0xFFFFFFFF / numberOfFiles);
			osh.WriteUInt32(slop);
			osh.WriteUInt32WithOffset(0x00000000, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLocation]);
			osh.WriteUInt32WithOffset(0x00000000, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLength]);
			osh.WriteUInt32WithOffset(flags, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.Flags]);

			for(uint i = 0; i < numberOfFiles; i++)
			{
				if(crc == null)
				{
					string caseStringToHash = ((flags & 1u) != 0) ? localFileHeaders[i].path.ToLower() : localFileHeaders[i].path;	//If hash is case insensitive
					string stringToHash =     ((flags & 2u) != 0) ? Path.GetFileName(caseStringToHash) : caseStringToHash;			//If hash is of just filename and extension
					osh.WriteUInt32WithOffset(Fnv1a.Hash32(System.Text.Encoding.ASCII.GetBytes(stringToHash)), IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + 4 * i);
				}
				else
				{
					osh.WriteUInt32WithOffset(crc[i], IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + 4 * i);
				}

				osh.WriteUInt32WithOffset(0x00000000, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileStartInLocal]);
				osh.WriteUInt32WithOffset(0x00000000, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileLengthInLocal]);
				//Mode always set to uncompressed for now
				//osh.WriteUInt32WithOffset(localFileHeaders[i].mode, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ModeInLocal]);
				osh.WriteUInt32WithOffset(0xFFFFFFFF, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ModeInLocal]);
			}
			for(uint i = 0; i < numberOfFiles; i++)
			{
				const int bufferLength = 0x8000;
				byte[] readBuffer = new byte[bufferLength];
				
				string currentFilePath = Path.GetDirectoryName(name) + "/" + names[i].Substring((names[i][1] == ':' ? 3 : 0));
				Console.WriteLine($"Packing {currentFilePath}");
				switch(localFileHeaders[i].mode & 0xFF)
				{
					default:
						FileStream cfs = new FileStream(inputFiles[i], FileMode.Open, FileAccess.Read);
						localFileHeaders[i].startingAddress = (uint)(((int)osh.BaseStream.Position + (chunkAlignment - 1)) / chunkAlignment) * chunkAlignment;
						localFileHeaders[i].size = (uint)(cfs.Length);
						osh.WriteUInt32WithOffset(localFileHeaders[i].startingAddress, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileStartInLocal]);
						osh.WriteUInt32WithOffset(localFileHeaders[i].size, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLocation] + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.ChecksumLength] * numberOfFiles + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.LocalHeaderLength] * i + IGA_Structure.headerData[_version][(uint)IGA_HeaderData.FileLengthInLocal]);
						osh.BaseStream.Seek(localFileHeaders[i].startingAddress, SeekOrigin.Begin);
						cfs.CopyTo(osh.BaseStream);
						cfs.Close();
						break;
				}
			}
			osh.BaseStream.Seek((((int)osh.BaseStream.Position + (chunkAlignment - 1)) / chunkAlignment) * chunkAlignment, SeekOrigin.Begin);
			nametableLocation = (uint)osh.BaseStream.Position;
			uint nameOffset = nametableLocation + 0x04 * numberOfFiles;
			for(uint i = 0; i < numberOfFiles; i++)
			{
				osh.WriteUInt32WithOffset(nameOffset - nametableLocation, nametableLocation + 0x04 * i);
				osh.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
				osh.WriteString(localFileHeaders[i].path);
				osh.BaseStream.WriteByte(0x00);
				if(_version == IGA_Version.SkylandersSuperChargers)
				{
					string[] dirs = localFileHeaders[i].path.Split(new char[]{'/', '\\'});
					string trimmedPath = string.Empty;
					for(uint j = 4; j < dirs.Length; j++)
					{
						trimmedPath += dirs[j] + "/";
					}
					trimmedPath = trimmedPath.TrimEnd('/');
					osh.WriteString(trimmedPath);
					osh.BaseStream.WriteByte(0x00);
				}
				osh.WriteUInt32(0x00000000);
				nameOffset = (uint)osh.BaseStream.Position;
			}
			nametableLength = (uint)osh.BaseStream.Length - nametableLocation;
			osh.WriteUInt32WithOffset(nametableLocation, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLocation]);
			osh.WriteUInt32WithOffset(nametableLength, IGA_Structure.headerData[_version][(uint)IGA_HeaderData.NametableLength]);
			osh.Close();
		}
		
		//Credit goes to DTZxPorter for this function and HashSearcher
		private uint CalculateSlop()
		{
			var HashBuffer = new byte[numberOfFiles * 4];
			var HashStaging = new List<uint>();

			if(crc == null)
			{
				foreach (var descriptor in localFileHeaders)
				{
					string caseStringToHash = ((flags & 1u) != 0) ? descriptor.path.ToLower() : descriptor.path;					//If hash is case insensitive
					string stringToHash =     ((flags & 2u) != 0) ? Path.GetFileName(caseStringToHash) : caseStringToHash;			//If hash is of just filename and extension
					HashStaging.Add(Fnv1a.Hash32(System.Text.Encoding.ASCII.GetBytes(stringToHash)));
				}
			}
			else
			foreach (var hash in crc)
				HashStaging.Add(hash);

			Buffer.BlockCopy(HashStaging.ToArray(), 0, HashBuffer, 0, HashBuffer.Length);

			int TopMatchIndex = 0;
			for (int i = 0x0; i < numberOfFiles; i++)
			{
				int Matches = 0;

				foreach (uint hash in HashStaging)
				{
					if (HashSearcher(HashBuffer, numberOfFiles, 0xFFFFFFFF / numberOfFiles, (uint)i, hash))
						Matches++;
				}

				if (Matches == numberOfFiles)
				{
					TopMatchIndex = i;
					break;
				}
			}

			return (uint)TopMatchIndex;
		}
		//Credit goes to DTZxPorter for this function and CalculateSlop
		private static unsafe bool HashSearcher(byte[] HashBuffer, uint EntryCountFile, uint HashSearchDivider, uint HashSearchSlop, uint EntryHash)
		{
			fixed (byte* PackageHashTable = &HashBuffer[0])
			{
				uint EntryCount = (uint)EntryCountFile;
				var EntryHashDivideByHashSearchDivider = EntryHash / HashSearchDivider;
				uint v7 = 0;

				if (EntryHashDivideByHashSearchDivider <= HashSearchSlop)
					v7 = 0;
				else
					v7 = EntryHashDivideByHashSearchDivider - HashSearchSlop;

				var v9 = HashSearchSlop + 1 + EntryHashDivideByHashSearchDivider;

				if (v9 < EntryCount)
					EntryCount = v9;

				var v11 = (uint*)(PackageHashTable + 4 * v7);
				var v12 = (4 * EntryCount - 4 * v7) >> 2;

				while (v12 > 0)
				{
					var v13 = (int)v12 / 2;
					if (v11[(int)v12 / 2] >= EntryHash)
					{
						v12 = (v12 / 2);
					}
					else
					{
						v11 += v13 + 1;
						v12 = (uint)(-1 - v13 + (int)v12);
					}
				}

				if (*v11 == EntryHash)
					return true;
				else
					return false;
			}
		}

		public void Close()
		{
			stream.Close();
			stream.Dispose();
		}
	}
}
