using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Windows.Forms;

namespace IGAE_GUI
{
	public partial class Form_igArchiveExtractor : Form
	{
		private CommonOpenFileDialog cofdSelectExtractOutputDir = new CommonOpenFileDialog();
		private CommonOpenFileDialog cofdSelectInputDir = new CommonOpenFileDialog();

		List<IGAE_File> files;

		const int prgBarMax = 100000;

		public Form_igArchiveExtractor(Config config)
		{
			InitializeComponent();

			ApplySettings(config);

			cofdSelectExtractOutputDir.IsFolderPicker = true;
			cofdSelectInputDir.IsFolderPicker = true;
			prgProgressBar.Minimum = 0;
			prgProgressBar.Maximum = prgBarMax;
			
			lblComplete.Visible = false;
		}

		private void OpenIGAFile(IGAE_Version version)
		{
			SelectIGAFile.Filter = "Supported game files|*.arc;*.bld;*.pak;*.iga|All files (*.*)|*.*";
			if(SelectIGAFile.ShowDialog() == DialogResult.OK)
			{
				prgProgressBar.Value = 0;
				lblComplete.Visible = false;
				files = new List<IGAE_File>();
				files.Add(new IGAE_File(SelectIGAFile.FileName, version));
				tmsi_ExtractAll.Enabled = true;
				treeLocalFiles.Nodes.Clear();
				List<string> containedFiles = new List<string>();
				for (uint i = 0; i < files[0].numberOfFiles; i++)
				{
					containedFiles.Add(files[0].ReadName(i));
					prgProgressBar.Value = (int)(((float)i / (float)files[0].numberOfFiles) * prgBarMax);
				}
				treeLocalFiles.Nodes.Add(MakeTreeFromPaths(containedFiles));
				treeLocalFiles.Sort();
				lstLog.Items.Add($"Opened IGA file \"{SelectIGAFile.FileName}\"");
				lblComplete.Visible = true;
				prgProgressBar.Value = prgBarMax;
				tmsi_ExtractFile.Enabled = false;
			}
		}
		private void treeLocalFiles_AfterSelect(object sender, EventArgs e)
		{
			lblSelectedFile.Text = $"Selected: {treeLocalFiles.SelectedNode.Text}";
			string type;
			if(treeLocalFiles.SelectedNode.Nodes.Count > 0)
			{
				tmsi_ExtractFile.Enabled = false;
				type = "Directory";
				lblSize.Text = $"Size: N/A";
				lblIndex.Text = $"Index: N/A";
			}
			else
			{
				string[] parts = treeLocalFiles.SelectedNode.Text.Split('.');
				string extension = parts[parts.Length - 1];
				switch (extension)
				{
					case "enc":
						type = "FSB Audio File";
						break;
					default:
						type = $"{extension.ToUpper()} File";
						break;
				}

				for (int i = 0; i < files.Count; i++)
				{
					if(files[i].localFileHeaders.Any(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text)))
					{
						IGAE_FileDescHeader selected = files[i].localFileHeaders.First(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text));
						lblSize.Text = $"Size: {selected.size} bytes";
						lblIndex.Text = $"Index: {selected.index}";
						tmsi_ExtractFile.Enabled = true;//(selected.mode & 0xFF000000) != 0x10000000;
					}
				}
			}
			lblType.Text = $"Type: {type}";
		}

		private void ExtractAllFiles(object sender, EventArgs e)
		{

			cofdSelectExtractOutputDir.Title = "Select Output Folder";

			uint totalFiles = 0;
			for (int j = 0; j < files.Count; j++)
			{
				for (uint i = 0; i < files[j].numberOfFiles; i++)
				{
					totalFiles += files[j].numberOfFiles;
				}
			}

			uint currFiles = 0;
			//Console.WriteLine($"Max Files: {totalFiles}");
			if (cofdSelectExtractOutputDir.ShowDialog() == CommonFileDialogResult.Ok)
			{
				lblComplete.Visible = false;
				prgProgressBar.Value = 0;
				for (int i = 0; i < files.Count; i++)
				{
					for (uint j = 0; j < files[i].numberOfFiles; j++)
					{
						files[i].ExtractFile(j, cofdSelectExtractOutputDir.FileName, out int res);
						lstLog.Items.Add($"Extracting file {j} {(res == 0 ? "succeeded" : "failed due to: unsupported compression")}...");
						currFiles++;
						//Console.WriteLine(((float)currFiles / totalFiles) * 100);
						prgProgressBar.Value = (int)(((float)currFiles / totalFiles) * prgBarMax);
					}
				}
				//ThreadManager.Extract(files.ToArray(), cofdSelectExtractOutputDir.FileName);
				//IGAR_File reb = new IGAR_File(ref file);
				//Bad repeated code
				//reb.Generate($"{cofdSelectDir.FileName}/rebuild-{SelectIGAFile.FileName.Split(new char[] { '/', '\\'}).Last()}.igar");
				//lstLog.Items.Add($"Generated Rebuild File \"rebuild -{SelectIGAFile.FileName.Split(new char[] { '/', '\\'}).Last()}.igar\"");
				prgProgressBar.Value = prgBarMax;
				lblComplete.Visible = true;
			}
		}

		private void ExtractSingleFile(object sender, EventArgs e)
		{

			uint index = 0;
			int igaIndex;
			for (igaIndex = 0; igaIndex < files.Count; igaIndex++)
			{
				try
				{
					index = files[igaIndex].localFileHeaders.First(x => x.path.Contains(treeLocalFiles.SelectedNode.Text)).index;
					break;
				}
				catch (InvalidOperationException)
				{
					continue;
				}
			}

			cofdSelectExtractOutputDir.Title = "Select Output Folder";

			if (cofdSelectExtractOutputDir.ShowDialog() == CommonFileDialogResult.Ok)
			{
				prgProgressBar.Value = 0;
				lblComplete.Visible = false;
				files[igaIndex].ExtractFile(index, cofdSelectExtractOutputDir.FileName, out int res);
				prgProgressBar.Value = prgBarMax;
				lblComplete.Visible = true;
			}
		}

		private void ExitApplication(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			lstLog.Items.Clear();
		}

		private void OpenFolder(IGAE_Version version)
		{
			prgProgressBar.Value = 0;
			lblComplete.Visible = false;

			cofdSelectInputDir.Title = "Select Folder To Load";

			if (cofdSelectInputDir.ShowDialog() == CommonFileDialogResult.Ok)
			{
				treeLocalFiles.Nodes.Clear();
				//I don't like this line of code either
				string[] igaFiles = Directory.GetFiles(cofdSelectInputDir.FileName, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".arc", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".bld", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".pak", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".iga", StringComparison.OrdinalIgnoreCase)).ToArray();
				if (igaFiles.Length == 0) throw new InvalidOperationException("There are no IGA type files in this directory");
				List<string> containedFiles = new List<string>();
				files = new List<IGAE_File>();
				for (int i = 0; i < igaFiles.Length; i++)
				{
					prgProgressBar.Value = (int)(((float)i / igaFiles.Length) * prgBarMax);
					//Some arc files on 3DS are 0 bytes, this line of code prevents them from loading
					if (new FileInfo(igaFiles[i]).Length == 0) continue;
					//Most BLD files contain bld and pak files which are not iga
					if (Path.GetFileName(igaFiles[i]) == "level.bld") continue;
					if (Path.GetFileName(igaFiles[i]) == "ENGLISH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "FRENCH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "SPANISH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "PORTUGUESE.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "SWEDISH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "FINNISH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "DUTCH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "NORWEGIAN.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "DANISH.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "GERMAN.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "HISPANIC.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "ITALIAN.pak") continue;
					if (Path.GetFileName(igaFiles[i]) == "JAPANESE.pak") continue;


					files.Add(new IGAE_File(igaFiles[i], version));
					bool isBLD = igaFiles[i].EndsWith("bld", StringComparison.OrdinalIgnoreCase);
					for (uint j = 0; j < files.Last().numberOfFiles; j++)
					{
						if(isBLD)
						{
							//Done to prevent a newly loaded bld overwriting previously loaded ones
							containedFiles.Add($"c:/{Path.GetFileNameWithoutExtension(igaFiles[i])}/{files.Last().ReadName(j)}");
						}
						else
						{
							containedFiles.Add(files.Last().ReadName(j));
						}
					}
					lstLog.Items.Add($"Opened IGA file \"{igaFiles[i]}\"");
				}

				treeLocalFiles.Nodes.Add(MakeTreeFromPaths(containedFiles));
				treeLocalFiles.Sort();
				tmsi_ExtractAll.Enabled = true;
			}
			tmsi_ExtractFile.Enabled = false;
			prgProgressBar.Value = prgBarMax;
			lblComplete.Visible = true;
		}

		//Stolen from ykm29's reply to https://stackoverflow.com/questions/1155977/populate-treeview-from-a-list-of-path with some slight alterations
		static TreeNode MakeTreeFromPaths(List<string> paths, string rootNodeName = "c")
		{
			var rootNode = new TreeNode(rootNodeName);
			for (int i = 0; i < paths.Count; i++)
			{
				var currentNode = rootNode;
				var pathItems = paths[i].Split(new char[2] { '/', '\\' });
				foreach (var item in pathItems)
				{
					if (item[1] == ':') continue;
					var tmp = currentNode.Nodes.Cast<TreeNode>().Where(x => x.Text.Equals(item));
					currentNode = tmp.Count() > 0 ? tmp.Single() : currentNode.Nodes.Add(item);
				}
			}
			return rootNode;
		}

		private void OpenSettings(object sender, EventArgs e)
		{
			SettingsMenu menu = new SettingsMenu(Config.Read());

			menu.Show();

			menu.btnSaveSettings.Click += (_, __) => ApplySettings(menu.config);
		}
		void ApplySettings(Config config)
		{
			Themes.mainForm = this;

			if (config.darkMode)
			{
				Themes.SwitchMainFormToDarkTheme();
			}
			else
			{
				Themes.SwitchMainFormToLightTheme();
			}
		}
		#region Version ToolStripMenuItems
		private void OpenFile_SSAWii_3DS_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersSpyrosAdventureWii);
		}
		//Sg and ssa wii u are the same
		private void OpenFile_SG_SSAWiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersSpyrosAdventureWiiU);
		}
		private void OpenFile_SSF_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersSwapForce);
		}
		private void OpenFile_STT_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersTrapTeam);
		}
		//They're the same
		private void OpenFile_SSC_SI2016_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersSuperChargers);
		}
		//Lost islands
		private void OpenFile_SLI_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGAE_Version.SkylandersLostIslands);
		}

		private void OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersSpyrosAdventureWii);
		}
		//Sg and ssa wii u are the same
		private void OpenFolder_SG_SSAWiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersSpyrosAdventureWiiU);
		}
		private void OpenFolder_SSF_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersSwapForce);
		}
		private void OpenFolder_STT_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersTrapTeam);
		}
		//They're the same
		private void OpenFolder_SSC_SI2016_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersSuperChargers);
		}
		//Lost islands
		private void OpenFolder_SLI_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGAE_Version.SkylandersLostIslands);
		}
		#endregion
	}
}
