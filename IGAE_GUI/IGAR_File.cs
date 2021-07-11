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
		public byte[] checksumBuffer;
		public byte[] badcodepog;

		public IGAR_File(IGAE_File file)
		{
			byte[] readBuffer = new byte[4];

			version = file.version;

			file.fs.Seek(IGAE_Globals.headerData[version][(int)IGAE_HeaderData.Unknown1], SeekOrigin.Begin);
			file.fs.Read(readBuffer, 0x00, 0x04);
			unknown1 = BitConverter.ToUInt32(readBuffer, 0x00);

			file.fs.Seek(IGAE_Globals.headerData[version][(int)IGAE_HeaderData.Unknown2Location], SeekOrigin.Begin);
			file.fs.Read(readBuffer, 0x00, 0x04);
			unknown2 = BitConverter.ToUInt32(readBuffer, 0x00);
			numberOfFiles = file.numberOfFiles;
			checksumBuffer = new byte[numberOfFiles * 4];
			for (int i = 0; i < numberOfFiles; i++)
			{
				file.fs.Read(checksumBuffer, i * 4, 4);
			}

			file.fs.Seek(file.nametableLocation, SeekOrigin.End);
			badcodepog = new byte[file.fs.Length - file.nametableLocation];
			file.fs.Read(badcodepog, 0x00, (int)(file.fs.Length - file.nametableLocation));
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
	}
}
