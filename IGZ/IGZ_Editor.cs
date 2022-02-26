using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGAE_GUI.IGZ
{
	public partial class IGZ_Editor : Form
	{
		TreeNode fixups = new TreeNode("Fixups");
		TreeNode objects = new TreeNode("Object List");
		IGZ_File _igz;
		public IGZ_Editor(IGZ_File igz)
		{
			InitializeComponent();

			_igz = igz;

			if(_igz.sh.BaseStream.GetType() == typeof(FileStream))
			{
				btnSaveIGZ.Enabled = false;
				btnSaveIGZ.Visible = false;
			}

			AddObject(igz.igObjectList.offsets.ToArray(), objects);

			treeItems.Nodes.Add(fixups);
			treeItems.Nodes.Add(objects);

			Config config = Config.Read();

			if (config.darkMode)
			{
				foreach(Control control in Controls)
				{
					Themes.SetControlToDark(control);
				}
				Themes.SetWindowControlToDark(this);
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

		void AddObject(uint[] offsets, TreeNode parentNode)
		{
			TreeNode potentialParentNode = null;
			for(int i = 0; i < offsets.Length; i++)
			{
				string objectType = "unknown";
				_igz.sh.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
				uint typeIndex = _igz.sh.ReadUInt32();
				try
				{
					objectType = _igz.tmet.typeNames[_igz.sh.ReadUInt32WithOffset(offsets[i])];
				}
				catch (Exception)
				{
					//objectType = offsets[(int)i].name.ToString("X08");
				}
				potentialParentNode = objects.Nodes.Add(offsets[i].ToString("X04") + ": " + objectType);
			}
		}
		
		void SelectionChange(object sender, TreeViewEventArgs e)
		{
			if(treeItems.SelectedNode.Parent == objects)
			{
				int objectIndex = treeItems.SelectedNode.Parent.Nodes.IndexOf(treeItems.SelectedNode);
				/*if(_igz.objects[objectIndex].GetType() == typeof(igImage2))
				{
					Console.WriteLine("igImage2 Selected");
					MemoryStream msImage = new MemoryStream();
					(_igz.objectList._objects[objectIndex] as igImage2).Extract(msImage);
					msImage.Seek(0x00, SeekOrigin.Begin);
					pbTexturePreview.Image = IGAE_GUI.Utils.TextureHelper.BitmapFromDDS(msImage);
					pbTexturePreview.Visible = true;
					btnTextureExtract.Visible = true;
					btnTextureReplace.Visible = true;
					msImage.Close();
				}
				else
				{
					pbTexturePreview.Image = null;
					pbTexturePreview.Visible = false;
					btnTextureExtract.Visible = false;
					btnTextureReplace.Visible = false;
				}*/
			}
			else
			{
				pbTexturePreview.Image = null;
				pbTexturePreview.Visible = false;
				btnTextureExtract.Visible = false;
				btnTextureReplace.Visible = false;
			}
		}

		void TextureExtract(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "DirectDraw Surface Files (*.dds)|*.dds|All Files (*.*)|*.*";
				sfd.RestoreDirectory = true;

				if(sfd.ShowDialog() == DialogResult.OK)
				{
					FileStream ofs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);
					int objectIndex = treeItems.SelectedNode.Parent.Nodes.IndexOf(treeItems.SelectedNode);
					//(_igz.objectList._objects[objectIndex] as igImage2).Extract(ofs);
					ofs.Close();
				}
			}
		}
		void TextureReplace(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.RestoreDirectory = true;
				ofd.Filter = "DirectDraw Surface Files (*.dds)|*.dds|All Files (*.*)|*.*";

				if(ofd.ShowDialog() == DialogResult.OK)
				{
					FileStream ifs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.ReadWrite);
					int objectIndex = treeItems.SelectedNode.Parent.Nodes.IndexOf(treeItems.SelectedNode);
					//(_igz.objectList._objects[objectIndex] as igImage2).Replace(ifs);
				}
			}
		}

		void Save(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "IGZ Files (*.igz)|*.igz;level.bld;*.pak;*.lang|All Files (*.*)|*.*";
				sfd.RestoreDirectory = true;

				if(sfd.ShowDialog() == DialogResult.OK)
				{
					FileStream ofs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);
					_igz.sh.BaseStream.Seek(0x00, SeekOrigin.Begin);
					_igz.sh.BaseStream.CopyTo(ofs);
					ofs.Flush();
					ofs.Close();
				}
			}
		}

		void Closing(object sender, EventArgs e)
		{
			_igz.sh.Close();
		}
	}
}