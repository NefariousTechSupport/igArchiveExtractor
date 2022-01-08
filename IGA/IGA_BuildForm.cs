using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace IGAE_GUI.IGA
{
	public partial class IGA_BuildForm : Form
	{
		uint[] crc;
		uint slop;
		uint flags;
		public IGA_BuildForm(IGA_File? file)
		{
			InitializeComponent();

			DataGridViewComboBoxColumn cbCompressionMode = new DataGridViewComboBoxColumn();
			cbCompressionMode.Items.AddRange("Uncompressed", "Zlib", "LZMA");
			cbCompressionMode.Name = "Compression Mode";
			cbCompressionMode.ValueType = typeof(string);
			cbCompressionMode.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgvItems.Columns.Add(cbCompressionMode);

			if(file != null)
			{
				crc = new uint[file.numberOfFiles];
				for(int i = 0; i < file.numberOfFiles; i++)
				{
					string realName = string.Empty;
					if((Path.GetExtension(file.name) == ".bld" || Path.GetExtension(file.name) == ".pak"))
					{
						realName = Path.ChangeExtension(file.name, null) + "/" +  file.names[i].Substring(file.names[i][1] == ':' ? 2 : 0);
					}
					else
					{
						realName = Path.GetDirectoryName(file.name) + file.names[i].Substring(file.names[i][1] == ':' ? 2 : 0);
					}
					string mode;
					switch((file.localFileHeaders[i].mode & 0xFF) >> 24)
					{
						default:
						case 0xFF:
							mode = "Uncompressed";
							break;
					}
					crc[i] = file.stream.ReadUInt32WithOffset((uint)IGA_Structure.headerData[file._version][(int)IGA_HeaderData.ChecksumLocation] + (uint)(i * 4u));
					dgvItems.Rows.Add(realName, file.names[i], mode);
				}
				switch(file._version)
				{
					case IGA_Version.SkylandersSpyrosAdventureWii:
						cbVersion.SelectedIndex = 0;
						break;
					case IGA_Version.SkylandersSpyrosAdventureWiiU:
						cbVersion.SelectedIndex = 2;
						break;
					case IGA_Version.SkylandersSwapForce:
						cbVersion.SelectedIndex = 5;
						break;
					case IGA_Version.SkylandersTrapTeam:
						cbVersion.SelectedIndex = 8;
						break;
					case IGA_Version.SkylandersSuperChargers:
						cbVersion.SelectedIndex = 10;
						break;
					default:
						MessageBox.Show("This version does not support rebuilding.", "Rebuild Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
						break;
				}
				flags = file.flags;
				slop = file.slop;
				UpdateCheckboxes();
			}

			Config cfg = Config.Read();

			if(cfg.darkMode)
			{
				//Themes.SwitchTextEditingFormToDarkTheme(this);
			}
		}
		public void AddRow(object sender, EventArgs e)
		{
			dgvItems.Rows.Add();
		}
		public void RemoveRow(object sender, EventArgs e)
		{
			for(int i = 0; i < dgvItems.SelectedRows.Count; i++)
			{
				try
				{
					dgvItems.Rows.RemoveAt(dgvItems.SelectedRows[i].Index);
				}
				catch (InvalidOperationException)
				{
					break;
				}
			}
		}
		public void SaveCSV(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
				saveFileDialog.FilterIndex = 0;
				saveFileDialog.RestoreDirectory = true;

				if (saveFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					StreamWriter fs = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.ASCII);

					if(cbVersion.SelectedIndex != -1)
					{
						fs.WriteLine($"{cbVersion.Items[cbVersion.SelectedIndex].ToString()},{slop.ToString()},{flags.ToString()}");
					}
					else
					{
						fs.WriteLine($"-,{slop.ToString()},{flags.ToString()}");
					}
					for(int y = 0; y < dgvItems.Rows.Count - 1; y++)
					{
						for(int x = 0; x < dgvItems.Columns.Count; x++)
						{
							if(dgvItems.Rows[y].Cells[x].Value == null)
							{
								fs.Write(",");
								continue;
							}
							//Semi colons aren't allowed so we're using them to deal with commas in vv's filenames
							fs.Write($"{dgvItems.Rows[y].Cells[x].Value.ToString().Replace(',', ';')}{(x == (dgvItems.Columns.Count - 1) ? string.Empty : ",")}");
						}
						fs.Write($",{crc[y].ToString()}");
						fs.Write($"{(y == (dgvItems.Rows.Count - 2) ? string.Empty : "\n")}");
					}
					fs.Close();
				}
			}
		}
		public void LoadCSV(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 0;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					StreamReader fs = new StreamReader(openFileDialog.FileName, System.Text.Encoding.ASCII);
					string[] metadata = fs.ReadLine().Split(',');
					if(metadata[0] == "-")
					{
						cbVersion.SelectedIndex = -1;
					}
					else
					{
						cbVersion.SelectedIndex = cbVersion.FindStringExact(metadata[0]);
					}
					try
					{
						slop = uint.Parse(metadata[1]);
						Console.WriteLine($"Successfully read slop ({slop.ToString("X08")})");
					}
					catch(Exception)
					{
						slop = 0;
						Console.WriteLine("Failure reading slop");
					}
					flags = uint.Parse(metadata[2]);
					UpdateCheckboxes();
					string[] cells;
					while(!fs.EndOfStream)
					{
						cells = fs.ReadLine().Split(",");

						//Cry about no for loop i dare you
						cells[0] = cells[0].Replace(';', ',');
						cells[1] = cells[1].Replace(';', ',');
						cells[2] = cells[2].Replace(';', ',');

						dgvItems.Rows.Add(new string[3]{cells[0], cells[1], cells[2]});
					}
					crc = new uint[dgvItems.RowCount];
					fs.DiscardBufferedData();
					fs.BaseStream.Seek(0x00, SeekOrigin.Begin);
					fs.ReadLine();									//Skip Metadata
					for(uint i = 0; i < dgvItems.RowCount; i++)
					{
						string line = fs.ReadLine();
						if(line == null) break;
						string txt = line.Split(",").Last();
						//Console.WriteLine($"Reading CSV: {line}");
						try
						{
							crc[i] = uint.Parse(txt);
							Console.WriteLine($"Successfully read crc {i}");
						}
						catch(Exception ex)
						{
							crc = null;
							Console.WriteLine($"Failure reading crc {i}; error {ex.Message} \"{txt}\"");
							break;
						}
					}
					fs.Close();
				}
			}
		}
		public void Build(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "iga files|*.arc;*.pak;*.bld;*.iga|All files (*.*)|*.*";
				saveFileDialog.FilterIndex = 0;
				saveFileDialog.RestoreDirectory = true;

				if (saveFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					string[] internalFilenames = new string[dgvItems.RowCount - 1];
					for(int i = 0; i < dgvItems.RowCount - 1; i++)
					{
						try
						{
							internalFilenames[i] = dgvItems.Rows[i].Cells[1].Value.ToString();
						}
						catch (NullReferenceException)
						{
							MessageBox.Show("Please fill in all boxes.", "Empty Boxes", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
						}
					}
					string[] realFileNames = new string[dgvItems.RowCount - 1];
					for(int i = 0; i < dgvItems.RowCount - 1; i++)
					{
						try
						{
							realFileNames[i] = dgvItems.Rows[i].Cells[0].Value.ToString();
						}
						catch (NullReferenceException)
						{
							MessageBox.Show("Please fill in all boxes.", "Empty Boxes", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
						}
					}
					try
					{
						Console.WriteLine(realFileNames[0]);
						Console.WriteLine(internalFilenames[0]);
					}
					catch(IndexOutOfRangeException)
					{
						MessageBox.Show("Please insert at least one row.", "No Files Selected", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
						return;
					}
					if(cbVersion.SelectedIndex == -1)
					{
						MessageBox.Show("Please Select a Version.", "No Version Selected", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
						return;
					}
					IGA_Version version = IGA_Version.SkylandersSpyrosAdventureWiiU;
					switch(cbVersion.Items[cbVersion.SelectedIndex].ToString())
					{
						case "SSA Wii/3DS":
						case "SG Alpha":
						case "SG 3DS":
						case "SSF 3DS":
						case "STT 3DS":
							version = IGA_Version.SkylandersSpyrosAdventureWii;
							break;
						case "SSA Wii U":
						case "SG":
						case "SSF Alpha":
							version = IGA_Version.SkylandersSpyrosAdventureWiiU;
							break;
						case "SSF":
							version = IGA_Version.SkylandersSwapForce;
							break;
						case "STT":
							version = IGA_Version.SkylandersTrapTeam;
							break;
						case "SSC":
							version = IGA_Version.SkylandersSuperChargers;
							break;
					}
					IGA_File outputFile = new IGA_File(version, internalFilenames, crc);
					outputFile.slop = slop;
					outputFile.flags = flags;
					outputFile.Build(saveFileDialog.FileName, realFileNames);
				}
			}
		}
		void UpdateFlags(object sender, EventArgs e)
		{
			flags = 0;
			if(chkHashNameAndExtensionOnly.Checked)
			{
				flags |= 2u;
			}
			if(chkCaseInsensitiveHash.Checked)
			{
				flags |= 1u;
			}
		}
		void UpdateCheckboxes()
		{
			chkCaseInsensitiveHash.Checked = (flags & 1u) != 0;
			chkHashNameAndExtensionOnly.Checked = (flags & 2u) != 0;
		}
	}
}
