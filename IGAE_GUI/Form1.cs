using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGAE_GUI
{
	public partial class Form_igArchiveExtractor : Form
	{
		IGAE_File file;
		public Form_igArchiveExtractor()
		{
			InitializeComponent();

			SelectIGAFile.Filter = "Arc files (*.arc)|*.arc|All files (*.*)|*.*";
			prgProgressBar.Minimum = 0;
			lblComplete.Visible = false;
		}

		private void btnLoadFile_Click(object sender, EventArgs e)
		{
			SelectIGAFile.ShowDialog();
		}

		private void SelectIGAFile_FileOk(object sender, CancelEventArgs e)
		{
			file = new IGAE_File(SelectIGAFile.FileName);
			btnExtractAllLoaded.Enabled = true;
			treeLocalFiles.Nodes.Clear();
			for (uint i = 0; i < file.numberOfFiles; i++)
			{
				string currFile = file.ReadName(i);
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
				btnExtractFile.Enabled = true;
				switch (extension)
				{
					case "igz":
						type = "IGZ Image File";
						break;
					case "enc":
						type = "FSB Audio File";
						break;
					default:
						type = $"{extension.ToUpper()} File";
						break;
				}

				IGAE_FileDescHeader selected = file.localFileHeaders.First(x => x.path.EndsWith(treeLocalFiles.SelectedNode.Text));
				lblSize.Text = $"Size: {selected.size} bytes";
				lblIndex.Text = $"Index: {selected.index}";
			}
			lblType.Text = $"Type: {type}";
		}

		private void btnExtractAllLoaded_Click(object sender, EventArgs e)
		{
			lblComplete.Visible = false;

			uint totalSize = 0;

			for(uint i = 0; i < file.numberOfFiles; i++)
			{
				totalSize += file.localFileHeaders[i].size;
			}

			prgProgressBar.Maximum = (int)totalSize;

			if (SelectOutputDir.ShowDialog() == DialogResult.OK)
			{
				for (uint i = 0; i < file.numberOfFiles; i++)
				{
					file.ExtractFile(i, SelectOutputDir.SelectedPath, prgProgressBar);
				}
			}
			lblComplete.Visible = true;
		}

		private void btnExtractFile_Click(object sender, EventArgs e)
		{
			lblComplete.Visible = false;

			uint index = file.localFileHeaders.First(x => x.path.Contains(treeLocalFiles.SelectedNode.Text)).index;

			prgProgressBar.Maximum = (int)file.localFileHeaders[index].size;

			if (SelectOutputDir.ShowDialog() == DialogResult.OK)
			{
				file.ExtractFile(index, SelectOutputDir.SelectedPath, prgProgressBar);
			}

			lblComplete.Visible = true;
		}

		private void btnQuit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
