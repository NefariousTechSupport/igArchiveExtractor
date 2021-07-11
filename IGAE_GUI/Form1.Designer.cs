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
			this.btnLoadFile = new System.Windows.Forms.Button();
			this.SelectIGAFile = new System.Windows.Forms.OpenFileDialog();
			this.treeLocalFiles = new System.Windows.Forms.TreeView();
			this.btnExtractAllLoaded = new System.Windows.Forms.Button();
			this.lblSelectedFile = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.lblSize = new System.Windows.Forms.Label();
			this.lblIndex = new System.Windows.Forms.Label();
			this.btnExtractFile = new System.Windows.Forms.Button();
			this.SelectOutputDir = new System.Windows.Forms.FolderBrowserDialog();
			this.prgProgressBar = new System.Windows.Forms.ProgressBar();
			this.lblComplete = new System.Windows.Forms.Label();
			this.btnClearLog = new System.Windows.Forms.Button();
			this.btnSettings = new System.Windows.Forms.Button();
			this.btnQuit = new System.Windows.Forms.Button();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.lblLol = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnLoadFile
			// 
			this.btnLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadFile.Location = new System.Drawing.Point(701, 533);
			this.btnLoadFile.Name = "btnLoadFile";
			this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
			this.btnLoadFile.TabIndex = 0;
			this.btnLoadFile.Text = "Load File";
			this.btnLoadFile.UseVisualStyleBackColor = true;
			this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
			// 
			// SelectIGAFile
			// 
			this.SelectIGAFile.FileName = "SelectIGAFile";
			this.SelectIGAFile.Title = "Open IGA File";
			// 
			// treeLocalFiles
			// 
			this.treeLocalFiles.Location = new System.Drawing.Point(12, 12);
			this.treeLocalFiles.Name = "treeLocalFiles";
			this.treeLocalFiles.Size = new System.Drawing.Size(515, 515);
			this.treeLocalFiles.TabIndex = 1;
			this.treeLocalFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeLocalFiles_AfterSelect);
			// 
			// btnExtractAllLoaded
			// 
			this.btnExtractAllLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExtractAllLoaded.Enabled = false;
			this.btnExtractAllLoaded.Location = new System.Drawing.Point(620, 533);
			this.btnExtractAllLoaded.Name = "btnExtractAllLoaded";
			this.btnExtractAllLoaded.Size = new System.Drawing.Size(75, 23);
			this.btnExtractAllLoaded.TabIndex = 2;
			this.btnExtractAllLoaded.Text = "Extract All";
			this.btnExtractAllLoaded.UseVisualStyleBackColor = true;
			this.btnExtractAllLoaded.Click += new System.EventHandler(this.btnExtractAllLoaded_Click);
			// 
			// lblSelectedFile
			// 
			this.lblSelectedFile.AutoSize = true;
			this.lblSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSelectedFile.Location = new System.Drawing.Point(533, 12);
			this.lblSelectedFile.Name = "lblSelectedFile";
			this.lblSelectedFile.Size = new System.Drawing.Size(118, 20);
			this.lblSelectedFile.TabIndex = 3;
			this.lblSelectedFile.Text = "Selected: None";
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(540, 40);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(57, 13);
			this.lblType.TabIndex = 4;
			this.lblType.Text = "Type: N/A";
			// 
			// lblSize
			// 
			this.lblSize.AutoSize = true;
			this.lblSize.Location = new System.Drawing.Point(540, 60);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(53, 13);
			this.lblSize.TabIndex = 5;
			this.lblSize.Text = "Size: N/A";
			// 
			// lblIndex
			// 
			this.lblIndex.AutoSize = true;
			this.lblIndex.Location = new System.Drawing.Point(540, 80);
			this.lblIndex.Name = "lblIndex";
			this.lblIndex.Size = new System.Drawing.Size(90, 13);
			this.lblIndex.TabIndex = 6;
			this.lblIndex.Text = "Index In File: N/A";
			// 
			// btnExtractFile
			// 
			this.btnExtractFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExtractFile.Enabled = false;
			this.btnExtractFile.Location = new System.Drawing.Point(620, 504);
			this.btnExtractFile.Name = "btnExtractFile";
			this.btnExtractFile.Size = new System.Drawing.Size(75, 23);
			this.btnExtractFile.TabIndex = 7;
			this.btnExtractFile.Text = "Extract File";
			this.btnExtractFile.UseVisualStyleBackColor = true;
			this.btnExtractFile.Click += new System.EventHandler(this.btnExtractFile_Click);
			// 
			// prgProgressBar
			// 
			this.prgProgressBar.Location = new System.Drawing.Point(13, 534);
			this.prgProgressBar.Name = "prgProgressBar";
			this.prgProgressBar.Size = new System.Drawing.Size(514, 23);
			this.prgProgressBar.TabIndex = 8;
			// 
			// lblComplete
			// 
			this.lblComplete.AutoSize = true;
			this.lblComplete.Location = new System.Drawing.Point(240, 538);
			this.lblComplete.Name = "lblComplete";
			this.lblComplete.Size = new System.Drawing.Size(54, 13);
			this.lblComplete.TabIndex = 9;
			this.lblComplete.Text = "Complete!";
			// 
			// btnClearLog
			// 
			this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearLog.Location = new System.Drawing.Point(701, 503);
			this.btnClearLog.Name = "btnClearLog";
			this.btnClearLog.Size = new System.Drawing.Size(75, 23);
			this.btnClearLog.TabIndex = 10;
			this.btnClearLog.Text = "Clear Log";
			this.btnClearLog.UseVisualStyleBackColor = true;
			this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
			// 
			// btnSettings
			// 
			this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSettings.Enabled = false;
			this.btnSettings.Location = new System.Drawing.Point(539, 504);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(75, 23);
			this.btnSettings.TabIndex = 11;
			this.btnSettings.Text = "Settings";
			this.btnSettings.UseVisualStyleBackColor = true;
			// 
			// btnQuit
			// 
			this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQuit.Location = new System.Drawing.Point(539, 533);
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.Size = new System.Drawing.Size(75, 23);
			this.btnQuit.TabIndex = 12;
			this.btnQuit.Text = "Quit";
			this.btnQuit.UseVisualStyleBackColor = true;
			this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
			// 
			// lstLog
			// 
			this.lstLog.FormattingEnabled = true;
			this.lstLog.HorizontalScrollbar = true;
			this.lstLog.Location = new System.Drawing.Point(539, 100);
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
			this.lblLol.Location = new System.Drawing.Point(567, 206);
			this.lblLol.Name = "lblLol";
			this.lblLol.Size = new System.Drawing.Size(111, 13);
			this.lblLol.TabIndex = 14;
			this.lblLol.Text = "Why are you like this?";
			// 
			// Form_igArchiveExtractor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(788, 568);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.btnQuit);
			this.Controls.Add(this.btnSettings);
			this.Controls.Add(this.btnClearLog);
			this.Controls.Add(this.lblComplete);
			this.Controls.Add(this.prgProgressBar);
			this.Controls.Add(this.btnExtractFile);
			this.Controls.Add(this.lblIndex);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.lblSelectedFile);
			this.Controls.Add(this.btnExtractAllLoaded);
			this.Controls.Add(this.treeLocalFiles);
			this.Controls.Add(this.btnLoadFile);
			this.Controls.Add(this.lblLol);
			this.Name = "Form_igArchiveExtractor";
			this.Text = "igArchiveExtractor (0.11)";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnLoadFile;
		private System.Windows.Forms.OpenFileDialog SelectIGAFile;
		private System.Windows.Forms.TreeView treeLocalFiles;
		private System.Windows.Forms.Button btnExtractAllLoaded;
		private System.Windows.Forms.Label lblSelectedFile;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblIndex;
		private System.Windows.Forms.Button btnExtractFile;
		private System.Windows.Forms.FolderBrowserDialog SelectOutputDir;
		private System.Windows.Forms.ProgressBar prgProgressBar;
		private System.Windows.Forms.Label lblComplete;
		private System.Windows.Forms.Button btnSettings;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.ListBox lstLog;
		private System.Windows.Forms.Label lblLol;
		private System.Windows.Forms.Button btnClearLog;
	}
}

