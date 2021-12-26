using System;
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
	public partial class IGZ_TextEditor : Form
	{
		public IGZ_TextEditor(IGZ_Text igz)
		{
			InitializeComponent();

			for (int i = 0; i < igz.texts.Count; i++)
			{
				lstTextItems.Items.Add(igz.texts[i]);
			}

			Config cfg = Config.Read();

			if(cfg.darkMode)
			{
				Themes.SwitchTextEditingFormToDarkTheme(this);
			}
		}

		void PreviewTextItem(object sender, MouseEventArgs args)
		{
			int index = 0;
			try
			{
				index = lstTextItems.IndexFromPoint(args.Location);
				tooltipTextItems.SetToolTip(lstTextItems, lstTextItems.Items[index].ToString());
			}
			catch (InvalidOperationException) {}
			catch (IndexOutOfRangeException) {}
			catch (ArgumentOutOfRangeException) {}
		}
	}
}
