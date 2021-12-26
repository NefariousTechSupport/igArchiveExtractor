using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	public abstract class IGZ_Fixup
	{
		public uint magicNumber;
		public uint offset;
		public uint sectionCount;
		public uint length;
		public uint startOfData;
	}

	//String List
	public class IGZ_TSTR : IGZ_Fixup
	{
		public string[] strings;
	}

	//Type Names
	public class IGZ_TMET : IGZ_Fixup
	{
		public string[] typeNames;
	}

	//Dependencies
	public class IGZ_TDEP : IGZ_Fixup
	{
		public string[] dependancies;
	}

	//Meta Sizes (unconfirmed)
	public class IGZ_MTSZ : IGZ_Fixup
	{
		public uint[] metaSizes;
	}

	//External ID
	public class IGZ_EXID : IGZ_Fixup
	{
		public uint[] hashes;
		public uint[] types;
	}

	//Named Handle List
	public class IGZ_EXNM : IGZ_Fixup
	{
		//Idk what to put here
	}

	//No Idea
	public class IGZ_TMHN : IGZ_Fixup
	{
		uint[] something;
		uint[] something2;
	}

	//Root Virtual Table apparently
	public class IGZ_RVTB : IGZ_Fixup
	{
		//No idea what this does lol
	}

	//No idea
	public class IGZ_RSTR : IGZ_Fixup
	{
		//No idea
	}

	//No idea
	public class IGZ_ROFS : IGZ_Fixup
	{
		//No idea
	}

	//No Idea
	public class IGZ_RPID : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_REXT : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_RHND : IGZ_Fixup
	{
		//No Idea
	}

	//No Idea
	public class IGZ_RNEX : IGZ_Fixup
	{
		//No Idea
	}
}
