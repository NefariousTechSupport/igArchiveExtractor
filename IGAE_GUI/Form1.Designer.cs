using Microsoft.WindowsAPICodePack.Dialogs;

namespace IGAE_GUI
{
	partial class Form_igArchiveExtractor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_igArchiveExtractor));
			this.SelectIGAFile = new System.Windows.Forms.OpenFileDialog();
			this.treeLocalFiles = new System.Windows.Forms.TreeView();
			this.lblSelectedFile = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.lblSize = new System.Windows.Forms.Label();
			this.lblIndex = new System.Windows.Forms.Label();
			this.SelectOutputDir = new System.Windows.Forms.FolderBrowserDialog();
			this.prgProgressBar = new System.Windows.Forms.ProgressBar();
			this.lblComplete = new System.Windows.Forms.Label();
			this.btnClearLog = new System.Windows.Forms.Button();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.lblLol = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.extractFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// SelectIGAFile
			// 
			this.SelectIGAFile.FileName = "SelectIGAFile";
			this.SelectIGAFile.Title = "Open IGA File";
			// 
			// treeLocalFiles
			// 
			this.treeLocalFiles.Location = new System.Drawing.Point(12, 40);
			this.treeLocalFiles.Name = "treeLocalFiles";
			this.treeLocalFiles.Size = new System.Drawing.Size(515, 480);
			this.treeLocalFiles.TabIndex = 1;
			this.treeLocalFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeLocalFiles_AfterSelect);
			// 
			// lblSelectedFile
			// 
			this.lblSelectedFile.AutoSize = true;
			this.lblSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSelectedFile.Location = new System.Drawing.Point(535, 40);
			this.lblSelectedFile.Name = "lblSelectedFile";
			this.lblSelectedFile.Size = new System.Drawing.Size(118, 20);
			this.lblSelectedFile.TabIndex = 3;
			this.lblSelectedFile.Text = "Selected: None";
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(542, 68);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(57, 13);
			this.lblType.TabIndex = 4;
			this.lblType.Text = "Type: N/A";
			// 
			// lblSize
			// 
			this.lblSize.AutoSize = true;
			this.lblSize.Location = new System.Drawing.Point(542, 88);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(53, 13);
			this.lblSize.TabIndex = 5;
			this.lblSize.Text = "Size: N/A";
			// 
			// lblIndex
			// 
			this.lblIndex.AutoSize = true;
			this.lblIndex.Location = new System.Drawing.Point(542, 108);
			this.lblIndex.Name = "lblIndex";
			this.lblIndex.Size = new System.Drawing.Size(90, 13);
			this.lblIndex.TabIndex = 6;
			this.lblIndex.Text = "Index In File: N/A";
			// 
			// prgProgressBar
			// 
			this.prgProgressBar.Location = new System.Drawing.Point(13, 528);
			this.prgProgressBar.Name = "prgProgressBar";
			this.prgProgressBar.Size = new System.Drawing.Size(514, 23);
			this.prgProgressBar.TabIndex = 8;
			// 
			// lblComplete
			// 
			this.lblComplete.AutoSize = true;
			this.lblComplete.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.lblComplete.Location = new System.Drawing.Point(244, 533);
			this.lblComplete.Name = "lblComplete";
			this.lblComplete.Size = new System.Drawing.Size(54, 13);
			this.lblComplete.TabIndex = 9;
			this.lblComplete.Text = "Complete!";
			this.lblComplete.Visible = false;
			// 
			// btnClearLog
			// 
			this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearLog.Location = new System.Drawing.Point(701, 528);
			this.btnClearLog.Name = "btnClearLog";
			this.btnClearLog.Size = new System.Drawing.Size(75, 23);
			this.btnClearLog.TabIndex = 10;
			this.btnClearLog.Text = "Clear Log";
			this.btnClearLog.UseVisualStyleBackColor = true;
			this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
			// 
			// lstLog
			// 
			this.lstLog.FormattingEnabled = true;
			this.lstLog.HorizontalScrollbar = true;
			this.lstLog.Location = new System.Drawing.Point(539, 126);
			this.lstLog.Name = "lstLog";
			this.lstLog.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.lstLog.Size = new System.Drawing.Size(237, 394);
			this.lstLog.TabIndex = 13;
			// 
			// lblLol
			// 
			this.lblLol.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblLol.AutoSize = true;
			this.lblLol.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.lblLol.Location = new System.Drawing.Point(567, 202);
			this.lblLol.Name = "lblLol";
			this.lblLol.Size = new System.Drawing.Size(111, 13);
			this.lblLol.TabIndex = 14;
			this.lblLol.Text = "Why are you like this?";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.menuStrip1.Size = new System.Drawing.Size(788, 24);
			this.menuStrip1.TabIndex = 17;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openFolderToolStripMenuItem,
            this.extractFileToolStripMenuItem,
            this.extractAllToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openFileToolStripMenuItem
			// 
			this.openFileToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
			this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
			this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openFileToolStripMenuItem.Text = "Open File";
			this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
			// 
			// openFolderToolStripMenuItem
			// 
			this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
			this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openFolderToolStripMenuItem.Text = "Open Folder";
			this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
			// 
			// extractFileToolStripMenuItem
			// 
			this.extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
			this.extractFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.extractFileToolStripMenuItem.Text = "Extract File";
			this.extractFileToolStripMenuItem.Click += new System.EventHandler(this.extractFileToolStripMenuItem_Click);
			// 
			// extractAllToolStripMenuItem
			// 
			this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
			this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.extractAllToolStripMenuItem.Text = "Extract All";
			this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.settingsToolStripMenuItem.Text = "Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// Form_igArchiveExtractor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(788, 561);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.btnClearLog);
			this.Controls.Add(this.lblComplete);
			this.Controls.Add(this.prgProgressBar);
			this.Controls.Add(this.lblIndex);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.lblSelectedFile);
			this.Controls.Add(this.treeLocalFiles);
			this.Controls.Add(this.lblLol);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form_igArchiveExtractor";
			this.Text = "igArchiveExtractor (1.00)";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.OpenFileDialog SelectIGAFile;
		public System.Windows.Forms.TreeView treeLocalFiles;
		public System.Windows.Forms.Label lblSelectedFile;
		public System.Windows.Forms.Label lblType;
		public System.Windows.Forms.Label lblSize;
		public System.Windows.Forms.Label lblIndex;
		public System.Windows.Forms.FolderBrowserDialog SelectOutputDir;
		public System.Windows.Forms.ProgressBar prgProgressBar;
		public System.Windows.Forms.Label lblComplete;
		public System.Windows.Forms.ListBox lstLog;
		public System.Windows.Forms.Label lblLol;
		public System.Windows.Forms.Button btnClearLog;
		public System.Windows.Forms.MenuStrip menuStrip1;
		public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem extractFileToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
	}
}

