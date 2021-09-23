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

		IGAE_File[] files;
		public Form_igArchiveExtractor()
		{
			InitializeComponent();

			cofdSelectExtractOutputDir.IsFolderPicker = true;
			cofdSelectInputDir.IsFolderPicker = true;
			prgProgressBar.Minimum = 0;
			prgProgressBar.Maximum = 1000;
			
			lblComplete.Visible = false;
		}

		private void btnLoadFile_Click(object sender, EventArgs e)
		{
			SelectIGAFile.Filter = "Supported game files|*.arc;*.bld;*.pak|All files (*.*)|*.*";
			if(SelectIGAFile.ShowDialog() == DialogResult.OK)
			{
				files = new IGAE_File[1];
				files[0] = new IGAE_File(SelectIGAFile.FileName);
				btnExtractAllLoaded.Enabled = true;
				treeLocalFiles.Nodes.Clear();
				for (uint i = 0; i < files[0].numberOfFiles; i++)
				{
					string currFile = files[0].ReadName(i);
					string[] contents = currFile.Split(new char[] { '\\', '/' });
					TreeNodeCollection parentDirNodeCollection = treeLocalFiles.Nodes;
					for (int j = 0; j < contents.Length; j++)
					{
						if (contents[j] == "C:" || contents[j] == "c:") continue;
						string curdir = string.Empty;
						for (int k = 0; k <= j; k++)
						{
							if (contents[k] == "C:" || contents[k] == "c:") continue;
							curdir += contents[k];
						}
						TreeNode[] searchNodes = treeLocalFiles.Nodes.Find(curdir, true);
						if (searchNodes.Length > 0)
						{
							parentDirNodeCollection = searchNodes[0].Nodes;
						}
						else
						{
							parentDirNodeCollection = parentDirNodeCollection.Add(curdir, contents[j]).Nodes;
						}
					}
				}
				treeLocalFiles.Sort();
				lstLog.Items.Add($"Opened IGA file \"{SelectIGAFile.FileName}\"");
			}
			btnExtractFile.Enabled = false;
		}
		private void treeLocalFiles_AfterSelect(object sender, EventArgs e)
		{
			lblSelectedFile.Text = $"Selected: {treeLocalFiles.SelectedNode.Text}";
			string type;
			if(treeLocalFiles.SelectedNode.Nodes.Count > 0)
			{
				btnExtractFile.Enabled = false;
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

				for (int i = 0; i < files.Length; i++)
				{
					if(files[i].localFileHeaders.Any(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text)))
					{
						IGAE_FileDescHeader selected = files[i].localFileHeaders.First(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text));
						lblSize.Text = $"Size: {selected.size} bytes";
						lblIndex.Text = $"Index: {selected.index}";
						btnExtractFile.Enabled = (selected.mode & 0xFF000000) != 0x10000000;
					}
				}
			}
			lblType.Text = $"Type: {type}";
		}

		private void btnExtractAllLoaded_Click(object sender, EventArgs e)
		{
			lblComplete.Visible = false;
			prgProgressBar.Value = 0;

			int currentProgress = 0;

			uint totalSize = 0;

			for (int j = 0; j < files.Length; j++)
			{
				for(uint i = 0; i < files[j].numberOfFiles; i++)
				{
					totalSize += files[j].localFileHeaders[i].size;
				}
			}

			cofdSelectExtractOutputDir.Title = "Select Output Folder";

			if (cofdSelectExtractOutputDir.ShowDialog() == CommonFileDialogResult.Ok)
			{
				for (int i = 0; i < files.Length; i++)
				{
					for (uint j = 0; j < files[i].numberOfFiles; j++)
					{
						int res = files[i].ExtractFile(j, cofdSelectExtractOutputDir.FileName, prgProgressBar, currentProgress, totalSize);
						lstLog.Items.Add($"Extracting file {j} {(res == 0 ? "succeeded" : "failed due to: unsupported compression")}...");
					}
				}
				//IGAR_File reb = new IGAR_File(ref file);
				//Bad repeated code
				//reb.Generate($"{cofdSelectDir.FileName}/rebuild-{SelectIGAFile.FileName.Split(new char[] { '/', '\\'}).Last()}.igar");
				//lstLog.Items.Add($"Generated Rebuild File \"rebuild -{SelectIGAFile.FileName.Split(new char[] { '/', '\\'}).Last()}.igar\"");
			}
			//lblComplete.Visible = true;
		}

		private void btnExtractFile_Click(object sender, EventArgs e)
		{
			lblComplete.Visible = false;

			uint index = 0;
			uint igaIndex;
			for (igaIndex = 0; igaIndex < files.Length; igaIndex++)
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
				files[igaIndex].ExtractFile(index, cofdSelectExtractOutputDir.FileName, prgProgressBar, 0, files[igaIndex].localFileHeaders[index].size);
			}

			//lblComplete.Visible = true;
		}

		private void btnQuit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			lstLog.Items.Clear();
		}

		private void btnRebuild_Click(object sender, EventArgs e)
		{
			SelectIGAFile.Filter = "Igar files (*.igar)|*.igar|All files (*.*)|*.*";
			if(SelectIGAFile.ShowDialog() == DialogResult.OK)
			{
				IGAR_File igarfile = IGAR_File.ReadIGARFile(SelectIGAFile.FileName);
				SelectIGAFile.Filter = "Supported game files|*.arc;*.bld;*.pak|All files (*.*)|*.*";
				if (SelectIGAFile.ShowDialog() == DialogResult.OK)
				{
					IGAR_File.RebuildIGAFile(ref igarfile, SelectIGAFile.FileName);
				}
			}
		}

		private void btnGenerateIGAR_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException("Ha Ha Ha Ha Ha Ha Ha Ha Ha Ha Ha Ha Ha.");

			SelectIGAFile.Filter = "Igar files (*.igar)|*.igar|All files (*.*)|*.*";
			if (SelectIGAFile.ShowDialog() == DialogResult.OK)
			{
				//IGAR_File rebfile = new IGAR_File(ref file, file.fs);
				//rebfile.Generate(SelectIGAFile.FileName);
			}
		}

		private void btnLoadFolder_Click(object sender, EventArgs e)
		{
			cofdSelectInputDir.Title = "Select Folder To Load";

			if (cofdSelectInputDir.ShowDialog() == CommonFileDialogResult.Ok)
			{
				treeLocalFiles.Nodes.Clear();
				string[] igaFiles = Directory.GetFiles(cofdSelectInputDir.FileName, "*.arc;*.bld;*.pak", SearchOption.AllDirectories);
				List<string> containedFiles = new List<string>();
				files = new IGAE_File[igaFiles.Length];
				for (int i = 0; i < files.Length; i++)
				{
					files[i] = new IGAE_File(igaFiles[i]);
					for (uint j = 0; j < files[i].numberOfFiles; j++)
					{
						containedFiles.Add(files[i].ReadName(j));
					}
					lstLog.Items.Add($"Opened IGA file \"{igaFiles[i]}\"");
				}

				treeLocalFiles.Nodes.Add(MakeTreeFromPaths(containedFiles));
				treeLocalFiles.Sort();
				btnExtractAllLoaded.Enabled = true;
			}
			btnExtractFile.Enabled = false;
		}

		//Stolen from ykm29's reply to https://stackoverflow.com/questions/1155977/populate-treeview-from-a-list-of-path with some slight alterations
		static TreeNode MakeTreeFromPaths(List<string> paths, string rootNodeName = "", char separator = '/')
		{
			var rootNode = new TreeNode(rootNodeName);
			for (int i = 0; i < paths.Count; i++)
			{
				var currentNode = rootNode;
				var pathItems = paths[i].Split(separator);
				foreach (var item in pathItems)
				{
					if (item == "C:" || item == "c:") continue;
					var tmp = currentNode.Nodes.Cast<TreeNode>().Where(x => x.Text.Equals(item));
					currentNode = tmp.Count() > 0 ? tmp.Single() : currentNode.Nodes.Add(item);
				}
			}
			return rootNode.FirstNode;
		}
	}
}
