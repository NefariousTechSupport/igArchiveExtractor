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
					0x00000030,		//Unknown data, local file headers start after this data
					0x00000004,		//Unknown data's size
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000004,		//Position of a local file's size inside of a local header
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
					0x00000034,		//Unknown data, local file headers start after this data
					0x00000004,		//Unknown data's size
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000004,		//Position of a local file's size inside of a local header
					0x00000024,		//Postion of first padding
					0x00000010,		//Length of first padding
					0x00000008		//Unknown but important adddress
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
					0x00000038,		//Unknown data, local file headers start after this data
					0x00000004,		//Unknown data's size
					0x00000000,		//Position of a local file's starting location inside of a local header
					0x00000008,		//Position of a local file's size inside of a local header
					0x0000001C,		//Postion of first padding
					0x0000000C		//Length of first padding
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
		Unknown2Location	= 0005,
		Unknown2Length		= 0006,
		FileStartInLocal	= 0007,
		FileLengthInLocal	= 0008,
		Padding1			= 0009,
		Padding2			= 0010,
		Unknown3			= 0011
	}
}
