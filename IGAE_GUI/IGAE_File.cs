using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	class IGAE_File
	{
		public FileStream fs;
		public UInt32 unknown1;
		public IGAE_FileDescHeader[] localFileHeaders;
		bool supported;
		bool initialised;

		static uint ioBlockSize = 0x40;

		public IGAE_File(string filepath)
		{
			initialised = true;
			fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
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
		public IGAE_Version version
		{
			get
			{
				uint ver = ReadUInt32(0x04);
				try
				{
					supported = true;
					return (IGAE_Version)ver;
				}
				catch(Exception)
				{
					supported = false;
					initialised = false;
					throw new NotImplementedException($"IGA version {ver} is not yet supported");
				}
			}
		}
		public uint numberOfFiles
		{
			get
			{
				return ReadUInt32(IGAE_HeaderData.NumberOfFiles);
			}
		}
		public uint nametableLocation
		{
			get
			{
				return ReadUInt32(IGAE_HeaderData.NametableLocation);
			}
		}
		public void ExtractFile(uint index, string outputDir, ProgressBar prgBar)
		{
			int startValue = prgBar.Value;
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
				if (startValue + j < prgBar.Maximum)
				{
					prgBar.Value = (int)(startValue + j);
				}
				j += ioBlockSize;
			}
			if (ioBlockSize >= localFileHeaders[index].size - j && localFileHeaders[index].size - j > 0)	//If the bytes remaining is in between 0 and 40
			{
				fs.Read(buffer, 0x00, (int)(localFileHeaders[index].size - j));								//Read the remaining bytes
				outputfs.Write(buffer, 0x00, (int)(localFileHeaders[index].size - j));						//Write the remaining bytes
				prgBar.Value = (int)(startValue + localFileHeaders[index].size);
			}
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
