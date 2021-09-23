using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	static class IGAE_Globals
	{
		public static Dictionary<IGAE_Version, uint[]> headerData = new Dictionary<IGAE_Version, uint[]>()
		{
			{
				IGAE_Version.SkylandersSpyrosAdventureWii,
				new uint[]
				{
					0x00000018,		//Unknown but important address
					0x0000000C,		//Number of Files
					0x00000018,		//Nametable Location
					0x0000001C,		//Nametable Size
					0x0000000C,		//Length of indiviual local file header
					0x00000030,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000004,		//Position of a local file's size inside of a local header
					0x00000008,		//The compression mode of the file
				}
			},
			{
				IGAE_Version.SkylandersSpyrosAdventureWiiU,
				new uint[]
				{
					0x00000018,		//Unknown but important address
					0x0000000C,		//Number of Files
					0x0000001C,		//Nametable Location
					0x00000020,		//Nametable Size
					0x0000000C,		//Length of indiviual local file header
					0x00000034,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000004,		//Position of a local file's size inside of a local header
					0x00000008,		//The compression mode of the file
				}
			},
			{
				IGAE_Version.SkylandersSwapForce,
				new uint[]
				{
					0x00000018,		//Unknown but important address
					0x0000000C,		//Number of Files
					0x0000002C,		//Nametable Location
					0x00000030,		//Nametable Size
					0x00000010,		//Length of indiviual local file header
					0x00000038,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000004,		//Position of a local file's starting location inside of a local header
					0x00000008,		//Position of a local file's size inside of a local header
					0x0000000C,		//Position of a local file's compression mode inside of a local header
				}
			},
			{
				IGAE_Version.SkylandersLostIslands,
				new uint[]
				{
					0x00000018,		//Unknown but important address
					0x0000000C,		//Number of Files
					0x00000028,		//Nametable Location
					0x00000030,		//Nametable Size
					0x00000010,		//Length of indiviual local file header
					0x00000038,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000008,		//Position of a local file's size inside of a local header
					0x0000000C,		//Position of a local file's compression mode inside of a local header
				}
			},
			{
				IGAE_Version.SkylandersTrapTeam,
				new uint[]
				{
					0x00000018,		//Unknown
					0x0000000C,		//Number of Files
					0x00000028,		//Nametable Location
					0x00000030,		//Nametable Size
					0x00000010,		//Length of individual local file header
					0x00000038,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000008,		//Position of a local file's size inside of a local header
					0x0000000C		//The compression mode of the file
				}
			},
			{
				IGAE_Version.SkylandersSuperChargers,
				new uint[]
				{
					0x00000018,		//Unknown
					0x0000000C,		//Number of Files
					0x0000002C,		//Nametable Location
					0x00000030,		//Nametable Size
					0x00000010,		//Length of individual local file header
					0x00000038,		//Checksum starting location
					0x00000004,		//Size of individual checksum
					0x00000004,		//Position of a local file's starting location inside of a local header
					0x00000008,		//Position of a local file's size inside of a local header
					0x0000000C		//The compression mode of the file
				}
			},
		};
	}
	enum IGAE_HeaderData
	{
		Unknown1			= 0000,
		NumberOfFiles		= 0001,
		NametableLocation	= 0002,
		NametableLength		= 0003,
		LocalHeaderLength	= 0004,
		ChecksumLocation	= 0005,
		ChecksumLength		= 0006,
		FileStartInLocal	= 0007,
		FileLengthInLocal	= 0008,
		ModeInLocal			= 0009
	}
}
