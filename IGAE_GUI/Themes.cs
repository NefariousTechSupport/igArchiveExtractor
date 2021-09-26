using System.Drawing;

//Extremely important feature

namespace IGAE_GUI
{
	public static class Themes
	{
		public static Form_igArchiveExtractor mainForm;

		public static void SwitchMainFormToLightTheme()
		{
			mainForm.BackColor = Color.FromName("Control");
			mainForm.ForeColor = Color.FromName("ControlText");

			mainForm.btnClearLog.BackColor = Color.FromName("Control");
			mainForm.btnClearLog.ForeColor = Color.FromName("ControlText");

			mainForm.menuStrip1.BackColor = Color.FromName("Control");
			mainForm.menuStrip1.ForeColor = Color.FromName("ControlText");

			mainForm.lstLog.BackColor = Color.FromName("Window");
			mainForm.lstLog.ForeColor = Color.FromName("WindowText");

			mainForm.treeLocalFiles.BackColor = Color.FromName("Window");
			mainForm.treeLocalFiles.ForeColor = Color.FromName("WindowText");
		}
		public static void SwitchMainFormToDarkTheme()
		{
			mainForm.BackColor = Color.FromArgb(064, 064, 064);
			mainForm.ForeColor = Color.FromName("White");

			mainForm.btnClearLog.BackColor = Color.FromName("ControlDarkDark");
			mainForm.btnClearLog.ForeColor = Color.FromName("ControlLight");

			mainForm.menuStrip1.BackColor = Color.FromArgb(070, 070, 070);
			mainForm.menuStrip1.ForeColor = Color.FromName("Control");

			mainForm.lstLog.BackColor = Color.FromArgb(32, 32, 32);
			mainForm.lstLog.ForeColor = Color.FromName("White");

			mainForm.treeLocalFiles.BackColor = Color.FromArgb(32, 32, 32);
			mainForm.treeLocalFiles.ForeColor = Color.FromName("White");
		}
		public static void SwitchSettingsFormToDarkTheme(SettingsMenu menu)
		{
			menu.BackColor = Color.FromArgb(064, 064, 064);
			menu.ForeColor = Color.FromArgb(191, 191, 191);

			menu.btnCancel.BackColor = Color.FromName("ControlDarkDark");
			menu.btnCancel.ForeColor = Color.FromName("ControlLight");

			menu.btnSaveSettings.BackColor = Color.FromName("ControlDarkDark");
			menu.btnSaveSettings.ForeColor = Color.FromName("ControlLight");

			menu.chkDarkMode.ForeColor = Color.FromName("Control");
		}
		public static void SwitchSettingsFormToLightTheme(SettingsMenu menu)
		{
			menu.BackColor = Color.FromName("Control");
			menu.ForeColor = Color.FromName("ControlText");

			menu.btnCancel.BackColor = Color.FromName("Control");
			menu.btnCancel.ForeColor = Color.FromName("ControlText");

			menu.btnSaveSettings.BackColor = Color.FromName("Control");
			menu.btnSaveSettings.ForeColor = Color.FromName("ControlText");

			menu.chkDarkMode.ForeColor = Color.FromName("ControlText");
		}
	}
}
