using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Windows.Forms;
using IGAE_GUI.IGA;
using IGAE_GUI.IGZ;

namespace IGAE_GUI
{
	public partial class Form_igArchiveExtractor : Form
	{
		private CommonOpenFileDialog cofdSelectExtractOutputDir = new CommonOpenFileDialog();
		private CommonOpenFileDialog cofdSelectInputDir = new CommonOpenFileDialog();

		List<IGA_File> files = new List<IGA_File>();

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

			tmsi_ExtractAll.Enabled = false;
			tmsi_ExtractFile.Enabled = false;
		}

		private void OpenIGAFile(IGA_Version version)
		{
			SelectIGAFile.Filter = "Supported game files|*.arc;*.bld;*.pak;*.iga;*.igz;*.lang|All files (*.*)|*.*";
			if(SelectIGAFile.ShowDialog() == DialogResult.OK)
			{
				prgProgressBar.Value = 0;
				lblComplete.Visible = false;
				if(files != null)
				{
					if(files.Count > 0)
					{
						for(int i = 0; i < files.Count; i++)
						{
							files[i].Close();
						}
					}
				}
				files = new List<IGA_File>();
					files.Add(new IGA_File(SelectIGAFile.FileName, version));
				/*try
				{
				}
				catch(Exception)
				{
					IGZ_File igz = new IGZ_File(new FileStream(SelectIGAFile.FileName, FileMode.Open, FileAccess.ReadWrite));

					IGZ_GeneralForm igzForm = new IGZ_GeneralForm(igz);
					igzForm.Show();
					return;
				}*/
				tmsi_ExtractAll.Enabled = true;
				treeLocalFiles.Nodes.Clear();
				List<string> containedFiles = new List<string>();
				for (uint i = 0; i < files[0].numberOfFiles; i++)
				{
					containedFiles.Add(files[0].names[i]);
					prgProgressBar.Value = (int)(((float)i / (float)files[0].numberOfFiles) * prgBarMax);
				}
				MakeTreeFromPaths(containedFiles);
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
						IGA_Descriptor selected = files[i].localFileHeaders.First(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text));
						lblSize.Text = $"Size: {selected.size} bytes";
						lblIndex.Text = $"Index: {selected.index}";
						tmsi_ExtractFile.Enabled = true;
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
						prgProgressBar.Value = (int)(((float)currFiles / totalFiles) * prgBarMax);
					}
				}
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

		private void PreviewFile(object sender, EventArgs e)
		{
			if(treeLocalFiles.SelectedNode.Nodes.Count != 0)
			{
				return;
			}

			int index = -1;
			int igaIndex;

			for (igaIndex = 0; igaIndex < files.Count; igaIndex++)
			{
				try
				{
					index = (int)files[igaIndex].localFileHeaders.First(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text)).index;
					break;
				}
				catch (InvalidOperationException)
				{
					continue;
				}
			}

			if(index == -1) return;

			MemoryStream igzms = new MemoryStream((int)files[igaIndex].localFileHeaders[index].size);

			files[igaIndex].ExtractFile((uint)index, igzms, out int res, true);

			//return;

			IGZ_File igz = new IGZ_File(igzms);

			//if(igz.version != 0x06 && igz.version != 0x08 && igz.version != 0x09 ) return;				//temporary line of code cos these two definitely work and other ones are buggy as hell

			IGZ_Editor igzForm = new IGZ_Editor(igz);
			igzForm.Show();
		}

		private void ExitApplication(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			lstLog.Items.Clear();
		}

		private void OpenFolder(IGA_Version version)
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
				if(files != null)
				{
					if(files.Count > 0)
					{
						for(int i = 0; i < files.Count; i++)
						{
							files[i].Close();
						}
					}
				}
				files = new List<IGA_File>();
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


					files.Add(new IGA_File(igaFiles[i], version));
					bool isBLD = igaFiles[i].EndsWith("bld", StringComparison.OrdinalIgnoreCase);
					for (uint j = 0; j < files.Last().numberOfFiles; j++)
					{
						if(isBLD)
						{
							//Done to prevent a newly loaded bld overwriting previously loaded ones
							containedFiles.Add($"{Path.GetFileNameWithoutExtension(igaFiles[i])}/{files.Last().names[j]}");
						}
						else
						{
							containedFiles.Add(files.Last().names[j]);
						}
					}
					lstLog.Items.Add($"Opened IGA file \"{igaFiles[i]}\"");
				}

				MakeTreeFromPaths(containedFiles);
				treeLocalFiles.Sort();
				tmsi_ExtractAll.Enabled = true;
			}
			tmsi_ExtractFile.Enabled = false;
			prgProgressBar.Value = prgBarMax;
			lblComplete.Visible = true;
		}

		//Stolen from ykm29's reply to https://stackoverflow.com/questions/1155977/populate-treeview-from-a-list-of-path with some alterations
		void MakeTreeFromPaths(List<string> paths)
		{
			for (int i = 0; i < paths.Count; i++)
			{
				TreeNode currentNode = null;
				string[] items = paths[i].Split(new char[]{'/', '\\'});
				foreach(string item in items)
				{
					if(currentNode == null)
					{
						TreeNode[] rootNodes = treeLocalFiles.Nodes.Cast<TreeNode>().ToArray();
						if(rootNodes.Any(x => x.Text.Equals(item, StringComparison.OrdinalIgnoreCase)))
						{
							currentNode = rootNodes.First(x => x.Text.Equals(item, StringComparison.OrdinalIgnoreCase));
						}
						else
						{
							currentNode = treeLocalFiles.Nodes.Add(item);
						}
					}
					else
					{
						TreeNode[] currentNodes = currentNode.Nodes.Cast<TreeNode>().ToArray();
						if(currentNodes.Any(x => x.Text.Equals(item, StringComparison.OrdinalIgnoreCase)))
						{
							currentNode = currentNodes.First(x => x.Text.Equals(item, StringComparison.OrdinalIgnoreCase));
						}
						else
						{
							currentNode = currentNode.Nodes.Add(item);
						}
					}
				}
			}
		}

		private void OpenSettings(object sender, EventArgs e)
		{
			SettingsMenu menu = new SettingsMenu(Config.Read());

			menu.Show();

			menu.btnSaveSettings.Click += (_, __) => ApplySettings(menu.config);
		}
		void ApplySettings(Config config)
		{
			if (config.darkMode)
			{
				Themes.SetWindowControlToDark(this);
				foreach(Control control in Controls)
				{
					Themes.SetControlToDark(control);
				}
			}
			else
			{
				foreach(Control control in Controls)
				{
					Themes.SetControlToLight(control);
				}
				Themes.SetControlToLight(this);
			}
		}
		void SaveAs(object sender, EventArgs e)
		{
			IGA_BuildForm buildForm;
			if(files.Count != 1)
			{
				buildForm = new IGA_BuildForm(null);
			}
			else
			{
				buildForm = new IGA_BuildForm(files[0]);
			}
			buildForm.Show();
			return;
			//Console.WriteLine("Opening save dialogue");
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "iga files|*.arc;*.bld;*.pak|All files (*.*)|*.*";
				saveFileDialog.FilterIndex = 0;
				saveFileDialog.RestoreDirectory = true;

				if (saveFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					files[0].Build(saveFileDialog.FileName);
				}
			}
		}
		#region Version ToolStripMenuItems
		private void OpenFile_SSAWii_3DS_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersSpyrosAdventureWii);
		}
		//Sg and ssa wii u are the same
		private void OpenFile_SG_SSAWiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersSpyrosAdventureWiiU);
		}
		private void OpenFile_SSF_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersSwapForce);
		}
		private void OpenFile_STT_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersTrapTeam);
		}
		//They're the same, also for clarity it's: SSC, SI (PS3, Xbox 360, Wii U)
		private void OpenFile_SSC_SI_PS3_X360_WiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersSuperChargers);
		}
		//I'm mad
		private void OpenFile_SI_PS4_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersImaginatorsPS4);
		}
		//Lost islands
		private void OpenFile_SLI_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenIGAFile(IGA_Version.SkylandersLostIslands);
		}

		private void OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersSpyrosAdventureWii);
		}
		//Sg and ssa wii u are the same
		private void OpenFolder_SG_SSAWiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersSpyrosAdventureWiiU);
		}
		private void OpenFolder_SSF_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersSwapForce);
		}
		private void OpenFolder_STT_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersTrapTeam);
		}
		//They're the same, also for clarity it's: SSC, SI (PS3, Xbox 360, Wii U)
		private void OpenFolder_SSC_SI_PS3_X360_WiiU_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersSuperChargers);
		}
		//I'm mad
		private void OpenFolder_SI_PS4_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersImaginatorsPS4);
		}
		//Lost islands
		private void OpenFolder_SLI_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFolder(IGA_Version.SkylandersLostIslands);
		}
		#endregion
	}
}
