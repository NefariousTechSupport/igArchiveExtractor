using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	[Serializable]
	class IGAR_File
	{
		public IGAE_Version version;
		public UInt32 unknown1;
		public UInt32 unknown2;
		public UInt32 numberOfFiles;
		public bool isBigEndian;
		public byte[] checksumBuffer;
		public byte[] badcodepog;

		public IGAR_File(ref IGAE_File file)
		{
			byte[] readBuffer = new byte[4];

			version = file.version;

			file.fs.Seek(IGAE_Globals.headerData[version][(int)IGAE_HeaderData.Unknown1], SeekOrigin.Begin);
			file.fs.Read(readBuffer, 0x00, 0x04);
			unknown1 = BitConverter.ToUInt32(readBuffer, 0x00);

			numberOfFiles = file.numberOfFiles;

			file.fs.Seek(IGAE_Globals.headerData[version][(int)IGAE_HeaderData.ChecksumLocation], SeekOrigin.Begin);
			checksumBuffer = new byte[numberOfFiles * 4];
			file.fs.Read(checksumBuffer, 0x00, (int)numberOfFiles * 4);

			Console.WriteLine($"nametableLocation: {file.nametableLocation}");
			file.fs.Seek(file.nametableLocation, SeekOrigin.Begin);
			Console.WriteLine($"fs position: {file.fs.Position}");
			badcodepog = new byte[file.fs.Length - file.nametableLocation];
			int i = 0;
			for (; i + 80 < badcodepog.Length; i += 0x40)
			{
				file.fs.Read(badcodepog, i, 0x40);
			}
			if (0x40 >= badcodepog.Length - i && badcodepog.Length - i > 0)   //If the bytes remaining is in between 0 and 40
			{
				file.fs.Read(badcodepog, badcodepog.Length - i, badcodepog.Length - i);
			}
		}
		public static IGAR_File ReadIGARFile(string igarFilePath)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open(igarFilePath, FileMode.Open);
			IGAR_File res = (IGAR_File)bf.Deserialize(fs);
			fs.Close();
			return res;
		}
		public void Generate(string output)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Create(output);
			bf.Serialize(fs, this);
			fs.Close();
		}
		//Only little endian is supported at the moment
		public static void RebuildIGAFile(ref IGAR_File igarFile, string output)
		{
			FileStream igaefs = File.Create(output);

			//Write the magic number

			igaefs.Seek(0x00, SeekOrigin.Begin);

			if(igarFile.isBigEndian)
			{
				igaefs.Write(IGAE_Globals.bigEndianMagic, 0x00, 0x04);
			}
			else
			{
				igaefs.Write(IGAE_Globals.littleEndianMagic, 0x00, 0x04);
			}

			//Write the version number

			igaefs.Write(BitConverter.GetBytes((uint)igarFile.version), 0x00, 0x04);

			igaefs.Seek(IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.Unknown1], SeekOrigin.Begin);
			igaefs.Write(BitConverter.GetBytes((uint)igarFile.unknown1), 0x00, 0x04);

			igaefs.Seek(IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.NumberOfFiles], SeekOrigin.Begin);
			igaefs.Write(BitConverter.GetBytes((uint)igarFile.numberOfFiles), 0x00, 0x04);

			igaefs.Seek(IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.NametableLength], SeekOrigin.Begin);
			igaefs.Write(BitConverter.GetBytes((uint)igarFile.badcodepog.Length), 0x00, 0x04);

			igaefs.Seek(IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.ChecksumLocation], SeekOrigin.Begin);
			igarFile.checksumBuffer = new byte[igarFile.numberOfFiles * 4];
			igaefs.Write(igarFile.checksumBuffer, 0x00, (int)igarFile.numberOfFiles * 4);

			igaefs.Seek(IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.ChecksumLocation] + IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.ChecksumLength] * igarFile.numberOfFiles, SeekOrigin.Begin);
			byte[] placeholderLocalHeader = new byte[IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.LocalHeaderLength]];
			placeholderLocalHeader[placeholderLocalHeader.Length - 4] = 0xFF;
			placeholderLocalHeader[placeholderLocalHeader.Length - 3] = 0xFF;
			placeholderLocalHeader[placeholderLocalHeader.Length - 2] = 0xFF;
			placeholderLocalHeader[placeholderLocalHeader.Length - 1] = 0xFF;
			for (uint i = 0; i < igarFile.numberOfFiles; i++)
			{
				igaefs.Write(placeholderLocalHeader, 0x00, (int)IGAE_Globals.headerData[igarFile.version][(uint)IGAE_HeaderData.LocalHeaderLength]);
			}

			string[] dirs = output.Split(new char[] { '/', '\\' });
			string outputDir = string.Empty;
			int iolen = 0x200;
			byte[] fRWBuf = new byte[iolen];
			for (uint i = 0; i < dirs.Length - 1; i++)
			{
				outputDir += dirs[i] + '/';
			}
			for (int i = 0; i < igarFile.numberOfFiles; i++)
			{
				uint nameStartAddr = BitConverter.ToUInt32(igarFile.badcodepog, i * 4);
				nameStartAddr.ToString();
				string currentFile = outputDir;

				uint j = 0;
				while (true)
				{
					/*Console.WriteLine(nameStartAddr);
					Console.WriteLine(j);
					Console.WriteLine(nameStartAddr + j);*/
					if (igarFile.badcodepog[nameStartAddr + j] == 0x00)					//If the character that was just read is a null character
					{
						break;															//Then exit out of the loop as you'd have reached the end of the file
					}
					else																//Otherwise
					{
						currentFile += (char)igarFile.badcodepog[nameStartAddr + j];			//Add to the output
					}
					j++;
				}
				Console.WriteLine(currentFile);
				FileStream cfs = File.OpenRead(currentFile);
				cfs.Seek(0x00, SeekOrigin.Begin);
				igaefs.Seek(((igaefs.Position / 0x200) + 1) * 0x200, SeekOrigin.Begin);
				uint beginningOfCurrFile = (uint)igaefs.Position;
				uint sizeOfCurrFile = (uint)cfs.Length;

				
				j = 0;
				for (; j < sizeOfCurrFile; j += (uint)iolen)
				{
					cfs.Read(fRWBuf, 0x00, iolen);
					igaefs.Write(fRWBuf, 0x00, iolen);
				}
				if (0x40 >= sizeOfCurrFile - j && sizeOfCurrFile- j > 0)   //If the bytes remaining is in between 0 and 40
				{
					cfs.Read(fRWBuf, 0x00, (int)(sizeOfCurrFile - j));
					igaefs.Write(fRWBuf, 0x00, (int)(sizeOfCurrFile - j));
				}

				cfs.Close();
			}

			igaefs.Close();

/*			ifile.localFileHeaders = (IGAE_FileDescHeader*)malloc(ifile.numberOfFiles * sizeof(IGAE_FileDescHeader));

			fseek(ifs, 0x14, SEEK_SET);
			fread(&readBuffer, 0x04, 0x01, ifs);
			ifile.nameTableLength = readBuffer;

			fseek(ofs, 0x00, SEEK_SET);
			uint32_t igaMagicNumber = 0x1A414749;
			uint32_t idontknow1 = 0x00000800;
			uint32_t someAddressLol = 0xFFFF;
			uint32_t zeros = 0x00000000;
			uint32_t ones = 0xFFFFFFFF;
			fseek(ofs, 0x00, SEEK_SET);
			fwrite(&igaMagicNumber, 0x04, 0x01, ofs);
			fseek(ofs, 0x04, SEEK_SET);
			fwrite(&ifile.rawVersion, 0x04, 0x01, ofs);
			fseek(ofs, 0x08, SEEK_SET);
			fwrite(&important, 0x04, 0x01, ofs);
			printf("ver: %d", ifile.version);
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_1], SEEK_SET);
			fwrite(&someAddressLol, 0x04, 0x01, ofs);           //cannot be zeroed,between 0xFF and 0xFFFFFF idk
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_NUMBER_OF_FILES], SEEK_SET);
			fwrite(&ifile.numberOfFiles, 0x04, 0x01, ofs);
			fseek(ofs, 0x10, SEEK_SET);
			fwrite(&zeros, 0x04, 0x01, ofs);                    //can be zeroed
			fwrite(&zeros, 0x04, 0x01, ofs);                    //an be zeroed
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_1], SEEK_SET);
			fwrite(&ifile.unknown, 0x04, 0x01, ofs);            //must be exact, so taken whilst generating rebuild file
																//fwrite(&zeros, 0x04, 0x01, ofs);
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_NAMETABLE_SIZE], SEEK_SET);
			fwrite(&ifile.nameTableLength, 0x04, 0x01, ofs);
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_PADDING_1], SEEK_SET);
			for (size_t i = 0; i < locations[ifile.version][IGAE_LOCATION_PADDING_1_LENGTH]; i++)
			{
				fwrite(&zeros, 0x04, 0x01, ofs);
			}

			unsigned char unknownDataBuffer[ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH]];
			memset(unknownDataBuffer, 0x00, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH]);
			fseek(ifs, 0x18, SEEK_SET);
			fread(unknownDataBuffer, 0x01, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH], ifs);
			fseek(ofs, locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_STARTING_LOCATION], SEEK_SET);
			fwrite(unknownDataBuffer, 0x01, ifile.numberOfFiles * locations[ifile.version][IGAE_LOCATION_UNKNOWN_2_LENGTH], ofs);

			uint32_t startOfLocalHeaders = ftell(ofs);
			printf("%08X\n", startOfLocalHeaders);

			for (size_t i = 0; i < ifile.numberOfFiles; i++)
			{
				fwrite(&zeros, 0x04, 0x01, ofs);
				fwrite(&zeros, 0x04, 0x01, ofs);
				fwrite(&ones, 0x04, 0x01, ofs);
			}

			fwrite(&ones, 0x04, 0x01, ofs);
			fwrite(&idontknow1, 0x04, 0x01, ofs);
			fwrite(&ones, 0x04, 0x01, ofs);
			fwrite(&ones, 0x04, 0x01, ofs);
			fwrite(&ones, 0x04, 0x01, ofs);
			fwrite(&ones, 0x04, 0x01, ofs);

			std::string inputStr(input);

			uint16_t strLen = 0;

			for (uint16_t i = 0; i < inputStr.length(); i++)
			{
				if (input[i] == '/' || input[i] == '\\')
				{
					strLen = i;
				}
			}

			char outputFolder[0xFF];
			memset(outputFolder, 0x00, 0xFF);
			memcpy(outputFolder, input, strLen);

			unsigned char fRWBuf[0x40];
			memset(fRWBuf, 0x00, 0x40);

			for (size_t i = 0; i < ifile.numberOfFiles; i++)
			{
				std::string currentFile(outputFolder);
				fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04 + i * 0x04, SEEK_SET);
				fread(&readBuffer, 0x04, 0x01, ifs);
				fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04 + readBuffer + 2, SEEK_SET);
				char readCharacter;
				do
				{
					fread(&readCharacter, 0x01, 0x01, ifs);
					if (readCharacter == 0x00)
					{
						break;
					}
					currentFile += readCharacter;
				} while (true);
				printf("file %d: %s\n", (int)i, currentFile.c_str());
				FILE* cfs = fopen(currentFile.c_str(), "rb");
				fseek(cfs, 0x00, SEEK_END);
				fseek(ofs, ((ftell(ofs) / IGAR_BLOCK_SIZE) + 1) * IGAR_BLOCK_SIZE, SEEK_SET);
				ifile.localFileHeaders[i].size = ftell(cfs);
				ifile.localFileHeaders[i].startingAddress = ftell(ofs);
				fseek(cfs, 0x00, SEEK_SET);
				uint32_t j = 0;
				for (; j < ifile.localFileHeaders[i].size; j += 0x40)
				{
					fread(fRWBuf, 0x01, 0x40, cfs);
					fwrite(fRWBuf, 0x01, 0x40, ofs);
				}
				if (0x40 >= ifile.localFileHeaders[i].size - j && ifile.localFileHeaders[i].size - j > 0)   //If the bytes remaining is in between 0 and 40
				{
					fread(fRWBuf, 0x01, ifile.localFileHeaders[i].size - j, cfs);                       //Read the remaining bytes
					fwrite(fRWBuf, 0x01, ifile.localFileHeaders[i].size - j, ofs);                      //Write the remaining bytes
				}
				fclose(cfs);
			}
			for (size_t i = 0; i < ifile.numberOfFiles; i++)
			{
				fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * i + locations[ifile.version][IGAE_LOCATION_LOCAL_START], SEEK_SET);
				fwrite(&ifile.localFileHeaders[i].startingAddress, 0x04, 0x01, ofs);
				fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * i + locations[ifile.version][IGAE_LOCATION_LOCAL_SIZE], SEEK_SET);
				fwrite(&ifile.localFileHeaders[i].size, 0x04, 0x01, ofs);
				fseek(ofs, startOfLocalHeaders + locations[ifile.version][IGAE_LOCATION_LENGTH_OF_LOCAL_HEADER] * (i + 1) - 4, SEEK_SET);
				fwrite(&ones, 0x04, 0x01, ofs);
			}

			fseek(ofs, 0x00, SEEK_END);
			fseek(ofs, ((ftell(ofs) / IGAR_BLOCK_SIZE) + 1) * IGAR_BLOCK_SIZE, SEEK_SET);
			ifile.nameTableStartAddress = ftell(ofs);
			fseek(ifs, 0x18 + ifile.numberOfFiles * 0x04, SEEK_SET);

			uint32_t j = 0;
			for (; j < ifile.nameTableLength; j += 0x40)
			{
				fread(fRWBuf, 0x01, 0x40, ifs);
				fwrite(fRWBuf, 0x01, 0x40, ofs);
			}
			if (0x40 >= ifile.nameTableLength - j && ifile.nameTableLength - j > 0) //If the bytes remaining is in between 0 and 40
			{
				fread(fRWBuf, 0x01, ifile.nameTableLength - j, ifs);                //Read the remaining bytes
				fwrite(fRWBuf, 0x01, ifile.nameTableLength - j, ofs);               //Write the remaining bytes
			}

			printf("%08X\n", (unsigned int)ftell(ofs));
			fseek(ofs, 0x1C, SEEK_SET);
			fwrite(&ifile.nameTableStartAddress, 0x04, 0x01, ofs);

			fclose(ifs);
			fclose(ofs);*/
		}
	}
}
