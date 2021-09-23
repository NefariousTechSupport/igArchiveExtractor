using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace IGAE_GUI
{
	class IGAE_File
	{
		public FileStream fs;
		public IGAE_FileDescHeader[] localFileHeaders;

		public IGAE_Version version;
		public uint numberOfFiles;
		public uint nametableLocation;
		public uint nametableLength;

		bool swapEndianness = false;

		public IGAE_File(string filepath)
		{
			fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

			byte[] readBuffer = new byte[0x04];

			fs.Read(readBuffer, 0x00, 0x04);
			if (BitConverter.ToUInt32(readBuffer, 0x00) == 0x1A414749)
			{
				swapEndianness = false;
			}
			else if (BitConverter.ToUInt32(readBuffer, 0x00) == 0x4947411A)
			{
				swapEndianness = true;
			}
			else
			{
				throw new InvalidOperationException("File is corrupt.");
			}

			try
			{
				uint rawVersion = ReadUInt32(0x04);

				if(rawVersion != 0x0B)
				{
					version = (IGAE_Version)rawVersion;
				}
				else
				{
					version = swapEndianness ? IGAE_Version.SkylandersSuperChargers : IGAE_Version.SkylandersTrapTeam;
				}
			}
			catch (Exception)
			{
				throw new NotImplementedException($"IGA version {version} is not yet supported");
			}

			numberOfFiles = ReadUInt32(IGAE_HeaderData.NumberOfFiles);
			nametableLocation = ReadUInt32(IGAE_HeaderData.NametableLocation);
			nametableLength = ReadUInt32(IGAE_HeaderData.NametableLength);

			localFileHeaders = new IGAE_FileDescHeader[numberOfFiles];
			for (uint i = 0; i < numberOfFiles; i++)
			{
				//The following is bad code

				uint headerStartingAddress = IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLocation] + numberOfFiles * IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLength] + i * IGAE_Globals.headerData[version][(int)IGAE_HeaderData.LocalHeaderLength];        //Read the local file header's starting address

				localFileHeaders[i].startingAddress = ReadUInt32(headerStartingAddress + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.FileStartInLocal]);              //Set the starting address

				localFileHeaders[i].size = ReadUInt32(headerStartingAddress + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.FileLengthInLocal]);                        //Set the size

				localFileHeaders[i].mode = ReadUInt32(headerStartingAddress + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ModeInLocal]);

				localFileHeaders[i].index = i;
			}
		}

		//This function contains commented out references to the progress bar, yeah that's cause it kept causing headaches but i wanna bring it back.
		public int ExtractFile(uint index, string outputDir, ProgressBar prgBar, int current, uint max)
		{
			//int startValue = current;

			//The following code up until outputfs is created should be rewritten cos it's hard to read

			string outputFileName = ReadName(index);
			outputFileName = outputFileName.Substring(outputFileName[1] == ':' ? 3 : 0);
			string outputPath = $"{outputDir}/{outputFileName}";
			string[] parts = outputPath.Split(new char[] { '/', '\\' });
			string parentDir = parts[0];
			for (int i = 1; i < parts.Length - 1; i++)
			{
				parentDir += "/" + parts[i];
			}
			Console.WriteLine(parentDir);
			DirectoryInfo info = Directory.CreateDirectory(parentDir);
			FileStream outputfs = File.Create($"{outputDir}/{outputFileName}");

			switch(localFileHeaders[index].mode >> 24)
			{
				case 0x00:
				case 0x10:
					{
						uint compressedSize = ReadUInt16(localFileHeaders[index].startingAddress);
						MemoryStream ms = new MemoryStream((int)compressedSize);
						byte[] compressedBytes = new byte[compressedSize];
						fs.Read(compressedBytes, 0x00, (int)compressedSize);
						ms.Write(compressedBytes, 0x00, (int)compressedSize);
						ms.Seek(0x00, SeekOrigin.Begin);
						DeflateStream decompressionStream = new DeflateStream(ms, CompressionMode.Decompress, true);
						decompressionStream.CopyTo(outputfs);
						outputfs.Close();
						return 0;
					}
				case 0x20:
					{
						//The following was adapted from https://github.com/KillzXGaming/Switch-Toolbox/blob/master/File_Format_Library/FileFormats/CrashBandicoot/IGA_PAK.cs
						uint compressedSize;
						uint def_block = 0x8000;
						byte[] readBuffer = new byte[0x40];
						fs.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);

						if ((uint)version <= 0x0B || version == IGAE_Version.SkylandersSuperChargers)       //I assigned superchargers 0x1000000B cos it has the same version number as stt but is different to stt
						{
							compressedSize = ReadUInt16((uint)fs.Position);
						}
						else
						{
							compressedSize = ReadUInt32((uint)fs.Position);
						}


						if (def_block > localFileHeaders[index].size)
						{
							def_block = localFileHeaders[index].size;
						}

						byte[] properties = new byte[0x05];
						fs.Read(properties, 0x00, 0x05);

						fs.Seek(localFileHeaders[index].startingAddress + 0x07, SeekOrigin.Begin);

						Console.WriteLine($"{index.ToString("X08")}; cosize: {compressedSize.ToString("X08")}; position: {(fs.Position - 7).ToString("X08")}");
						byte[] compressedBytes = new byte[compressedSize];
						fs.Read(compressedBytes, 0x00, (int)compressedSize);

						MemoryStream ms = new MemoryStream(compressedBytes);

						SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
						decoder.SetDecoderProperties(properties);
						decoder.Code(ms, outputfs, compressedSize, def_block, null);
						outputfs.Close();
						return 0;
					}
				case 0x30:
					{
						//This functions like 0xFF except the size is stored at the start.
						//I don't know why either.
						//We're just gonna subtract 2 from the size and start at localFileHeaders[index].startingAddress + 4 so as not to break compatibility with any programs

						uint size = ReadUInt16(localFileHeaders[index].startingAddress) - 2;
						byte[] buffer = new byte[size];

						fs.Seek(localFileHeaders[index].startingAddress + 0x04, SeekOrigin.Begin);

						fs.Read(buffer, 0x00, (int)size);
						outputfs.Write(buffer, 0x00, (int)size);

						outputfs.Close();
					}
					return 0;
				case 0xFF:
					{
						byte[] buffer = new byte[localFileHeaders[index].size];
						fs.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);

						fs.Read(buffer, 0x00, (int)localFileHeaders[index].size);
						outputfs.Write(buffer, 0x00, (int)localFileHeaders[index].size);

						outputfs.Close();
					}
					return 0;
				default:
					return -1;
			}
		}

		public string ReadName(uint index)
		{
			uint nameStartAddress = ReadUInt32(nametableLocation + index * 4);		//The name's starting address

			fs.Seek(nametableLocation + nameStartAddress, SeekOrigin.Begin);		//Go to where the name would start

			byte[] readChar = new byte[1] { 0x00 };									//The character being read

			string output = string.Empty;											//The name

			while (true)
			{
				fs.Read(readChar, 0x00, 0x01);										//Read the character
				if (readChar[0] == 0x00)											//If the character that was just read is a null character
				{
					break;															//Then exit out of the loop as you'd have reached the end of the file
				}
				else																//Otherwise
				{
					output += (char)readChar[0];									//Add to the output
				}
			}
			localFileHeaders[index].path = output;
			return output;
		}
		uint ReadUInt32(IGAE_HeaderData value)
		{
			fs.Seek(IGAE_Globals.headerData[version][(uint)value], SeekOrigin.Begin);
			byte[] readBuffer = new byte[0x04];
			fs.Read(readBuffer, 0x00, 0x04);
			if(swapEndianness)
			{
				Array.Reverse(readBuffer);
			}
			return BitConverter.ToUInt32(readBuffer, 0x00);
		}
		uint ReadUInt32(uint location)
		{
			fs.Seek(location, SeekOrigin.Begin);
			byte[] readBuffer = new byte[0x04];
			fs.Read(readBuffer, 0x00, 0x04);
			if (swapEndianness)
			{
				Array.Reverse(readBuffer);
			}
			return BitConverter.ToUInt32(readBuffer, 0x00);
		}
		//This one doesn't have endianness swapping cos this is only used to read compressed sizes which are always little endian, and this only runs on windows, which is little endian, so there's no need to swap the endianness
		uint ReadUInt16(uint location)
		{
			fs.Seek(location, SeekOrigin.Begin);
			byte[] readBuffer = new byte[0x02];
			fs.Read(readBuffer, 0x00, 0x02);
			return BitConverter.ToUInt16(readBuffer, 0x00);
		}
		~IGAE_File()
		{
			fs.Close();
		}
	}
}
