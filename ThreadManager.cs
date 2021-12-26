using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

//Unused since there is some overlap in assets causing a race condition where two files are extracted to the same place

namespace IGAE_GUI
{
	static class ThreadManager
	{
		static uint currFile = 0;
		static uint totalFiles = 0;
		public static int threads = 8;

		public static void InitialiseThreads()
		{
			ThreadPool.SetMinThreads(threads, threads);
			ThreadPool.SetMaxThreads(threads, threads);
		}
		public static void Extract(IGAE_File[] files, string outputDir)
		{
			Stopwatch sw = Stopwatch.StartNew();

			totalFiles = 0;
			currFile = 0;
			for (int j = 0; j < files.Length; j++)
			{
				for (uint i = 0; i < files[j].numberOfFiles; i++)
				{
					totalFiles += files[j].numberOfFiles;
				}
			}

			for (uint i = 0; i < files.Length; i++)
			{
				Console.WriteLine($"{i}/{files.Length}");

				//Ok so for some reason the following works:
				IGAE_File f = files[i];
				ThreadPool.QueueUserWorkItem(state => CallExtract(f, outputDir));
				//But not this:
				//ThreadPool.QueueUserWorkItem(state => CallExtract(files[i], outputDir));
				//I wish I could tell you why
			}
			while(sw.ElapsedMilliseconds < 1200000) { Thread.Sleep(100); }
			Console.WriteLine(((float)currFile / totalFiles) * 100);
			throw new Exception("Done");
		}
		static void CallExtract(IGAE_File file, string outputDir)
		{
			for (uint i = 0; i < file.numberOfFiles; i++)
			{
				file.ExtractFile(i, outputDir, out int res);
				currFile++;
			}
		}
	}
}
