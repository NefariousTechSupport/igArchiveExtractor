using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	enum IGAE_Version
	{
		SkylandersSpyrosAdventureWii	= 0x00000004,
		SkylandersSpyrosAdventureWiiU	= 0x00000008,		//Is also Giants
		SkylandersSwapForce				= 0x0000000A,
		SkylandersLostIslands			= 0x1000000A,		//The 1 is added to differentiate the two, but they do both have version 0x0A for some reason
		SkylandersTrapTeam				= 0x0000000B,		//Is also Superchargers yet different from superchargers
		SkylandersSuperChargers			= 0x1000000B,		//The 1 is added to differentiate the two, but they do all have version 0x0B for some reason
		SkylandersImaginatorsPS4		= 0x2000000B,		//The 2 is added to differentiate the two, but they do all have version 0x0B for some reason
		CrashNST						= 0x0000000C		
	}
}
