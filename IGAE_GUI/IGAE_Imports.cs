using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAE_GUI
{
	class IGAE_Imports
	{
		const string dllName = "libIGAE";

		[DllImport(dllName)] public extern static int IGAE_LoadFile(string filePath, out IntPtr output);
		[DllImport(dllName)] public extern static long IGAE_GetNumberOfFiles(IGAE_File file);
		[DllImport(dllName)] public extern static bool IGAE_CheckFileHeader(ref IGAE_File file);
		[DllImport(dllName)] public extern static long IGAE_SetHeaderValues(ref IGAE_File file, UInt32 fileNo);
		[DllImport(dllName)] public extern static void IGAE_PopulateDescHeaderArray(ref IGAE_File file);
		[DllImport(dllName)] public extern static int IGAE_ExtractFile(IGAE_File file, UInt32 fileNo, string outputPath);
		[DllImport(dllName)] public extern static long IGAE_GetFileDescsStartingAddr(IGAE_File file);
		[DllImport(dllName)] public extern static long IGAE_GetNameTableStartAddr(IGAE_File file);
		[DllImport(dllName)] public extern static long IGAE_GetNameTableLength(IGAE_File file);
		[DllImport(dllName)] public extern static long IGAE_FindName(IGAE_File file, UInt32 fileNo, StringBuilder output);
		[DllImport(dllName)] public extern static int IGAE_ReadVersion(IGAE_File file, out UInt32 rawVersion, bool readFromFile = true);
		[DllImport(dllName)] public extern static void test();
	}
}
