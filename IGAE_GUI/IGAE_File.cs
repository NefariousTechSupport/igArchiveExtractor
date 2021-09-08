using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip.Compression.LZMA;
using ManagedLzma.LZMA;
namespace IGAE_GUI
{
	class IGAE_File
	{
		public FileStream fs;
		public IGAE_FileDescHeader[] localFileHeaders;
		public bool compressed = false;

		static uint ioBlockSize = 0x40;

		public IGAE_Version version;
		public uint numberOfFiles;
		public uint dictionarySize;
		public uint nametableLocation;
		public uint nametableLength;

		public IGAE_File(string filepath)
		{
			fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
			/*if(filepath.Substring(filepath.Length - 4) == "arc")
			{
				compressed = false;
			}
			else if (filepath.Substring(filepath.Length - 4) == "bld" || filepath.Substring(filepath.Length - 4) == "pak")
			{
				compressed = true;
			}*/

			try
			{
				version = (IGAE_Version)ReadUInt32(0x04);
			}
			catch (Exception)
			{
				throw new NotImplementedException($"IGA version {version} is not yet supported");
			}
			numberOfFiles = ReadUInt32(IGAE_HeaderData.NumberOfFiles);
			//dictionarySize = ReadUInt32(IGAE_HeaderData.DictionarySize);				//Not Implemented
			nametableLocation = ReadUInt32(IGAE_HeaderData.NametableLocation);
			nametableLength = ReadUInt32(IGAE_HeaderData.NametableLength);

			localFileHeaders = new IGAE_FileDescHeader[numberOfFiles];
			for(uint i = 0; i < numberOfFiles; i++)
			{
				uint headerStartingAddress = IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLocation] + numberOfFiles * IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLength] + i * IGAE_Globals.headerData[version][(int)IGAE_HeaderData.LocalHeaderLength];        //Read the local file header's starting address
				byte[] readBuffer = new byte[0x04];																//The variable to read into

				fs.Seek(headerStartingAddress + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.FileStartInLocal], SeekOrigin.Begin);         //Go to where the local file header contains data on where the file would actually start
				fs.Read(readBuffer, 0x00, 0x04);																//Read into the read buffer
				localFileHeaders[i].startingAddress = BitConverter.ToUInt32(readBuffer, 0x00);					//Set the starting address

				fs.Seek(headerStartingAddress + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.FileLengthInLocal], SeekOrigin.Begin);          //Go to where the local file header contains data on where the file would actually start
				fs.Read(readBuffer, 0x00, 0x04);																//Read into the read buffer, at this point the read head would be 4 in front now, aka where the local file's size is stored
				localFileHeaders[i].size = BitConverter.ToUInt32(readBuffer, 0x00);								//Set the size

				localFileHeaders[i].index = i;
			}
		}

		public void ExtractFile(uint index, string outputDir, ProgressBar prgBar, int current, uint max)
		{
			int startValue = current;
			string outputPath = $"{outputDir}/{ReadName(index).Substring(3)}";
			string[] parts = outputPath.Split(new char[] { '/', '\\' });
			string parentDir = parts[0];
			for (int i = 1; i < parts.Length - 1; i++)
			{
				parentDir += "/" + parts[i];
			}
			Console.WriteLine(parentDir);
			DirectoryInfo info = Directory.CreateDirectory(parentDir);
			FileStream outputfs = File.Create($"{outputDir}/{ReadName(index).Substring(3)}");
			byte[] buffer = new byte[ioBlockSize];
			fs.Seek(localFileHeaders[index].startingAddress, SeekOrigin.Begin);
			uint j = 0;
			
			while (j < localFileHeaders[index].size - ioBlockSize)
			{
				fs.Read(buffer, 0x00, (int)ioBlockSize);
				outputfs.Write(buffer, 0x00, (int)ioBlockSize);
				/*if (startValue + j < prgBar.Maximum)
				{
					prgBar.Value = (int)(startValue + j);
				}*/
				j += ioBlockSize;
			}
			if (ioBlockSize >= localFileHeaders[index].size - j && localFileHeaders[index].size - j > 0)	//If the bytes remaining is in between 0 and 40
			{
				fs.Read(buffer, 0x00, (int)(localFileHeaders[index].size - j));								//Read the remaining bytes
				outputfs.Write(buffer, 0x00, (int)(localFileHeaders[index].size - j));						//Write the remaining bytes
				//prgBar.Value = (int)((float)((startValue + localFileHeaders[index].size) / max) * 1000);
			}
			#if false
				//byte[] fileContents = new byte[nametableLocation - IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLocation] + (4 + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.LocalHeaderLength]) * numberOfFiles];

				SevenZip.Compression.LZMA.Decoder dec = new SevenZip.Compression.LZMA.Decoder();
				int dictionaryPos = (int)(IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLocation] + (4 + IGAE_Globals.headerData[version][(int)IGAE_HeaderData.LocalHeaderLength]) * numberOfFiles);
				using (MemoryStream input = new MemoryStream((int)(fs.Length - dictionaryPos - nametableLength)))
				{
					fs.Seek(dictionaryPos, SeekOrigin.Begin);
					//fs.CopyTo(input, (int)(fs.Length - nametableLength - dictionaryPos));
					//input.Seek(0, SeekOrigin.Begin);

					//byte lc = 0x08;								//Number of literal context bits
					//byte lp = 0x04;								//Number of literal position bits
					//byte pb = 0x04;								//Number of position bits
					// Set the decoder properties as there isn't a header for this afaik
					byte[] properties = new byte[5];
					/*properties[0] = (byte)((pb * 5 + lp) * 9 + lc);
					Array.Copy(BitConverter.GetBytes(dictionarySize), 0, properties, 1, 0x04);*/
					fs.Seek(localFileHeaders[0].startingAddress, SeekOrigin.Begin);
					fs.Read(properties, 0x00, 0x05);
					properties = properties.Reverse().ToArray();
					Console.WriteLine($"properties at {fs.Position - 5}: {BitConverter.ToString(properties, 0x00, 0x05)}");
					dec.SetDecoderProperties(properties);
					fs.Seek(0x03, SeekOrigin.Current);
					//Console.WriteLine($"ipos: {input.Position}");
					//byte[] test = new byte[4];
					//fs.Seek(dictionaryPos, SeekOrigin.Begin);
					//fs.Read(test, 0x00, 0x04);
					//Console.WriteLine($"first 4 bytes at pos {fs.Position}: {BitConverter.ToUInt32(test, 0x00).ToString("X8")}");
					//fs.Seek(dictionaryPos, SeekOrigin.Begin);					
					//Console.WriteLine($"dictionaryPos: {dictionaryPos.ToString("X2")}");
					//Console.WriteLine($"len: {(int)(fs.Length - nametableLength - dictionaryPos)}");
					outputfs.Seek(0, SeekOrigin.Begin);
					//byte[] bad = new byte[fs.Length - nametableLength - dictionaryPos];
					//fs.Seek(dictionaryPos, SeekOrigin.Begin);
					//fs.Read(bad, 0x00, bad.Length);//(outputfs, (int)(fs.Length - nametableLength - dictionaryPos));
					//outputfs.Write(bad, 0x00, bad.Length);
					dec.Code(fs, outputfs, -1, localFileHeaders[0].size, null);
				}
			#endif
			outputfs.Close();
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
			return BitConverter.ToUInt32(readBuffer, 0x00);
		}
		uint ReadUInt32(uint location)
		{
			fs.Seek(location, SeekOrigin.Begin);
			byte[] readBuffer = new byte[0x04];
			fs.Read(readBuffer, 0x00, 0x04);
			return BitConverter.ToUInt32(readBuffer, 0x00);
		}
		~IGAE_File()
		{
			Console.WriteLine("destructed");
			fs.Close();
		}
	}
}
