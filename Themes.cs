using System.Drawing;
using System.Windows.Forms;

//Extremely important feature

namespace IGAE_GUI
{
	public static class Themes
	{
		#region Dark Mode
		public static void SetControlToDark(Control control)
		{
			if(control.GetType() == typeof(TreeView) || control.GetType() == typeof(ListBox))
			{
				SetBoxControlToDark(control);
				return;
			}
			if(control.GetType() == typeof(DataGridView))
			{
				SetDGVControlToDark(control as DataGridView);
			}
			if(control.GetType() == typeof(Label) || control.GetType() == typeof(CheckBox))
			{
				SetWindowControlToDark(control);
				return;
			}
			if(control.GetType() == typeof(MenuStrip))
			{
				SetMenuStripToDark(control);
				return;
			}
			control.BackColor = Color.FromName("ControlDarkDark");
			control.ForeColor = Color.FromName("ControlLight");
		}
		public static void SetBoxControlToDark(Control control)
		{
			control.BackColor = Color.FromArgb(032, 032, 032);
			control.ForeColor = Color.FromName("White");
		}
		public static void SetDGVControlToDark(DataGridView dgv)
		{
			dgv.BackgroundColor = Color.FromArgb(032, 032, 032);
		}
		public static void SetWindowControlToDark(Control control)
		{
			control.BackColor = Color.FromArgb(064, 064, 064);
			control.ForeColor = Color.FromName("White");
		}
		public static void SetLabelToDark(Control control)
		{
			control.BackColor = control.Parent.BackColor;
			control.ForeColor = Color.FromName("White");
		}
		public static void SetMenuStripToDark(Control control)
		{
			control.BackColor = Color.FromArgb(070, 070, 070);
			control.ForeColor = Color.FromName("White");
		}
		#endregion
		#region Light Mode
		public static void SetControlToLight(Control control)
		{
			if(control.GetType() == typeof(DataGridView))
			{
				(control as DataGridView).BackgroundColor = Color.White;
				return;
			}
			if(control.GetType() == typeof(TreeView) || control.GetType() == typeof(ListBox))
			{
				SetBoxControlToLight(control);
				return;
			}
			if(control.GetType() == typeof(MenuStrip))
			{
				SetMenuStripToLight(control);
				return;
			}
			if(control.GetType() == typeof(Form))
			{
				control.BackColor = Color.FromName("Window");
				control.ForeColor = Color.FromName("WindowText");
			}
			control.BackColor = Color.FromName("Control");
			control.ForeColor = Color.FromName("ControlText");
		}
		public static void SetBoxControlToLight(Control control)
		{
			control.BackColor = Color.FromArgb(255, 255, 255);
			control.ForeColor = Color.FromName("ControlText");
		}
		public static void SetMenuStripToLight(Control control)
		{
			control.BackColor = Color.FromName("Window");
			control.ForeColor = Color.FromName("WindowText");
		}
		#endregion
	}
}
