
namespace IGAE_GUI.IGZ
{
	partial class IGZ_TextEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lstTextItems = new System.Windows.Forms.ListBox();
			this.tooltipTextItems = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// lstTextItems
			// 
			this.lstTextItems.FormattingEnabled = true;
			this.lstTextItems.Location = new System.Drawing.Point(13, 13);
			this.lstTextItems.Name = "lstTextItems";
			this.lstTextItems.Size = new System.Drawing.Size(459, 420);
			this.lstTextItems.TabIndex = 0;
			this.lstTextItems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreviewTextItem);
			// 
			// IGZ_TextEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 450);
			this.Controls.Add(this.lstTextItems);
			this.Name = "IGZ_TextEditor";
			this.Text = "IGZ Text Viewer";
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.ListBox lstTextItems;
		private System.Windows.Forms.ToolTip tooltipTextItems;
	}
}