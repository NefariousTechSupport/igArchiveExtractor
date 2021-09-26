using System;
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
	public partial class SettingsMenu : Form
	{
		public Config config;

		public SettingsMenu(Config _config)
		{
			InitializeComponent();

			config = _config;

			chkDarkMode.Checked = config.darkMode;

			if(config.darkMode)
			{
				Themes.SwitchSettingsFormToDarkTheme(this);
			}
		}

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			config.darkMode = chkDarkMode.Checked;

			config.Save();

			this.Close();
		}
	}
}
