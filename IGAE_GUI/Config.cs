using Newtonsoft.Json;
using System.IO;
using System;

namespace IGAE_GUI
{
	public class Config
	{
		public bool darkMode = false;

		public static Config Read()
		{
			string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/NefariousTechSupport/IGAE/config.json";
			if (File.Exists(path))
			{
				string configJson = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<Config>(configJson);
			}
			else return new Config();
		}
		public void Save()
		{
			string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/NefariousTechSupport/IGAE/config.json";
			string configJson = JsonConvert.SerializeObject(this);
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, configJson);
		}
	}
}
