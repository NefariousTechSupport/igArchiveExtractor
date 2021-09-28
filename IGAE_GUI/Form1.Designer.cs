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
			this.tmsi_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSA = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSA_3DS_Wii = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSA_WiiU = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SG = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SG_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SG_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSF = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSF_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSF_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_STT = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_STT_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_STT_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SSC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SI = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SI_2016 = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SI_Switch = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFile_SLI = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSA = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSA_3DS_Wii = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSA_WiiU = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SG = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SG_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SG_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSF = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSF_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSF_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_STT = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_STT_3DS = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_STT_HC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SSC = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SI = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SI_2016 = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SI_Switch = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_OpenFolder_SLI = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_ExtractFile = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_ExtractAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_Settings = new System.Windows.Forms.ToolStripMenuItem();
			this.tmsi_Exit = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tmsi_OpenFile,
            this.tmsi_OpenFolder,
            this.tmsi_ExtractFile,
            this.tmsi_ExtractAll,
            this.tmsi_Settings,
            this.tmsi_Exit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// tmsi_OpenFile
			// 
			this.tmsi_OpenFile.BackColor = System.Drawing.SystemColors.Control;
			this.tmsi_OpenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_SSA,
            this.tmsi_OpenFile_SG,
            this.tmsi_OpenFile_SSF,
            this.tmsi_OpenFile_STT,
            this.tmsi_OpenFile_SSC,
            this.tmsi_OpenFile_SI,
            this.tmsi_OpenFile_SLI});
			this.tmsi_OpenFile.Name = "tmsi_OpenFile";
			this.tmsi_OpenFile.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFile.Text = "Open File";
			// 
			// tmsi_OpenFile_SSA
			// 
			this.tmsi_OpenFile_SSA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_SSA_3DS_Wii,
            this.tmsi_OpenFile_SSA_WiiU});
			this.tmsi_OpenFile_SSA.Name = "tmsi_OpenFile_SSA";
			this.tmsi_OpenFile_SSA.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SSA.Text = "Skylanders Spyro\'s Adventure";
			// 
			// tmsi_OpenFile_SSA_3DS_Wii
			// 
			this.tmsi_OpenFile_SSA_3DS_Wii.Name = "tmsi_OpenFile_SSA_3DS_Wii";
			this.tmsi_OpenFile_SSA_3DS_Wii.Size = new System.Drawing.Size(116, 22);
			this.tmsi_OpenFile_SSA_3DS_Wii.Text = "3DS/Wii";
			this.tmsi_OpenFile_SSA_3DS_Wii.Click += new System.EventHandler(this.OpenFile_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SSA_WiiU
			// 
			this.tmsi_OpenFile_SSA_WiiU.Name = "tmsi_OpenFile_SSA_WiiU";
			this.tmsi_OpenFile_SSA_WiiU.Size = new System.Drawing.Size(116, 22);
			this.tmsi_OpenFile_SSA_WiiU.Text = "Wii U";
			this.tmsi_OpenFile_SSA_WiiU.Click += new System.EventHandler(this.OpenFile_SG_SSAWiiU_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SG
			// 
			this.tmsi_OpenFile_SG.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_SG_3DS,
            this.tmsi_OpenFile_SG_HC});
			this.tmsi_OpenFile_SG.Name = "tmsi_OpenFile_SG";
			this.tmsi_OpenFile_SG.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SG.Text = "Skylanders Giants";
			// 
			// tmsi_OpenFile_SG_3DS
			// 
			this.tmsi_OpenFile_SG_3DS.Name = "tmsi_OpenFile_SG_3DS";
			this.tmsi_OpenFile_SG_3DS.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_SG_3DS.Text = "3DS";
			this.tmsi_OpenFile_SG_3DS.Click += new System.EventHandler(this.OpenFile_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SG_HC
			// 
			this.tmsi_OpenFile_SG_HC.Name = "tmsi_OpenFile_SG_HC";
			this.tmsi_OpenFile_SG_HC.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_SG_HC.Text = "Home Console";
			this.tmsi_OpenFile_SG_HC.Click += new System.EventHandler(this.OpenFile_SG_SSAWiiU_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SSF
			// 
			this.tmsi_OpenFile_SSF.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_SSF_3DS,
            this.tmsi_OpenFile_SSF_HC});
			this.tmsi_OpenFile_SSF.Name = "tmsi_OpenFile_SSF";
			this.tmsi_OpenFile_SSF.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SSF.Text = "Skylanders Swap Force";
			// 
			// tmsi_OpenFile_SSF_3DS
			// 
			this.tmsi_OpenFile_SSF_3DS.Name = "tmsi_OpenFile_SSF_3DS";
			this.tmsi_OpenFile_SSF_3DS.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_SSF_3DS.Text = "3DS";
			this.tmsi_OpenFile_SSF_3DS.Click += new System.EventHandler(this.OpenFile_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SSF_HC
			// 
			this.tmsi_OpenFile_SSF_HC.Name = "tmsi_OpenFile_SSF_HC";
			this.tmsi_OpenFile_SSF_HC.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_SSF_HC.Text = "Home Console";
			this.tmsi_OpenFile_SSF_HC.Click += new System.EventHandler(this.OpenFile_SSF_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_STT
			// 
			this.tmsi_OpenFile_STT.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_STT_3DS,
            this.tmsi_OpenFile_STT_HC});
			this.tmsi_OpenFile_STT.Name = "tmsi_OpenFile_STT";
			this.tmsi_OpenFile_STT.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_STT.Text = "Skylanders Trap Team";
			// 
			// tmsi_OpenFile_STT_3DS
			// 
			this.tmsi_OpenFile_STT_3DS.Name = "tmsi_OpenFile_STT_3DS";
			this.tmsi_OpenFile_STT_3DS.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_STT_3DS.Text = "3DS";
			this.tmsi_OpenFile_STT_3DS.Click += new System.EventHandler(this.OpenFile_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_STT_HC
			// 
			this.tmsi_OpenFile_STT_HC.Name = "tmsi_OpenFile_STT_HC";
			this.tmsi_OpenFile_STT_HC.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFile_STT_HC.Text = "Home Console";
			this.tmsi_OpenFile_STT_HC.Click += new System.EventHandler(this.OpenFile_STT_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SSC
			// 
			this.tmsi_OpenFile_SSC.Name = "tmsi_OpenFile_SSC";
			this.tmsi_OpenFile_SSC.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SSC.Text = "Skylanders Superchargers";
			this.tmsi_OpenFile_SSC.Click += new System.EventHandler(this.OpenFile_SSC_SI2016_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SI
			// 
			this.tmsi_OpenFile_SI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFile_SI_2016,
            this.tmsi_OpenFile_SI_Switch});
			this.tmsi_OpenFile_SI.Name = "tmsi_OpenFile_SI";
			this.tmsi_OpenFile_SI.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SI.Text = "Skylanders Imaginators";
			// 
			// tmsi_OpenFile_SI_2016
			// 
			this.tmsi_OpenFile_SI_2016.Name = "tmsi_OpenFile_SI_2016";
			this.tmsi_OpenFile_SI_2016.Size = new System.Drawing.Size(109, 22);
			this.tmsi_OpenFile_SI_2016.Text = "2016";
			this.tmsi_OpenFile_SI_2016.Click += new System.EventHandler(this.OpenFile_SSC_SI2016_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFile_SI_Switch
			// 
			this.tmsi_OpenFile_SI_Switch.Name = "tmsi_OpenFile_SI_Switch";
			this.tmsi_OpenFile_SI_Switch.Size = new System.Drawing.Size(109, 22);
			this.tmsi_OpenFile_SI_Switch.Text = "Switch";
			// 
			// tmsi_OpenFile_SLI
			// 
			this.tmsi_OpenFile_SLI.Name = "tmsi_OpenFile_SLI";
			this.tmsi_OpenFile_SLI.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFile_SLI.Text = "Lost Islands";
			this.tmsi_OpenFile_SLI.Click += new System.EventHandler(this.OpenFile_SLI_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder
			// 
			this.tmsi_OpenFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_SSA,
            this.tmsi_OpenFolder_SG,
            this.tmsi_OpenFolder_SSF,
            this.tmsi_OpenFolder_STT,
            this.tmsi_OpenFolder_SSC,
            this.tmsi_OpenFolder_SI,
            this.tmsi_OpenFolder_SLI});
			this.tmsi_OpenFolder.Name = "tmsi_OpenFolder";
			this.tmsi_OpenFolder.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFolder.Text = "Open Folder";
			// 
			// tmsi_OpenFolder_SSA
			// 
			this.tmsi_OpenFolder_SSA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_SSA_3DS_Wii,
            this.tmsi_OpenFolder_SSA_WiiU});
			this.tmsi_OpenFolder_SSA.Name = "tmsi_OpenFolder_SSA";
			this.tmsi_OpenFolder_SSA.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SSA.Text = "Skylanders Spyro\'s Adventure";
			// 
			// tmsi_OpenFolder_SSA_3DS_Wii
			// 
			this.tmsi_OpenFolder_SSA_3DS_Wii.Name = "tmsi_OpenFolder_SSA_3DS_Wii";
			this.tmsi_OpenFolder_SSA_3DS_Wii.Size = new System.Drawing.Size(116, 22);
			this.tmsi_OpenFolder_SSA_3DS_Wii.Text = "3DS/Wii";
			this.tmsi_OpenFolder_SSA_3DS_Wii.Click += new System.EventHandler(this.OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SSA_WiiU
			// 
			this.tmsi_OpenFolder_SSA_WiiU.Name = "tmsi_OpenFolder_SSA_WiiU";
			this.tmsi_OpenFolder_SSA_WiiU.Size = new System.Drawing.Size(116, 22);
			this.tmsi_OpenFolder_SSA_WiiU.Text = "Wii U";
			this.tmsi_OpenFolder_SSA_WiiU.Click += new System.EventHandler(this.OpenFolder_SG_SSAWiiU_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SG
			// 
			this.tmsi_OpenFolder_SG.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_SG_3DS,
            this.tmsi_OpenFolder_SG_HC});
			this.tmsi_OpenFolder_SG.Name = "tmsi_OpenFolder_SG";
			this.tmsi_OpenFolder_SG.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SG.Text = "Skylanders Giants";
			// 
			// tmsi_OpenFolder_SG_3DS
			// 
			this.tmsi_OpenFolder_SG_3DS.Name = "tmsi_OpenFolder_SG_3DS";
			this.tmsi_OpenFolder_SG_3DS.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFolder_SG_3DS.Text = "3DS";
			this.tmsi_OpenFolder_SG_3DS.Click += new System.EventHandler(this.OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SG_HC
			// 
			this.tmsi_OpenFolder_SG_HC.Name = "tmsi_OpenFolder_SG_HC";
			this.tmsi_OpenFolder_SG_HC.Size = new System.Drawing.Size(153, 22);
			this.tmsi_OpenFolder_SG_HC.Text = "Home Console";
			this.tmsi_OpenFolder_SG_HC.Click += new System.EventHandler(this.OpenFolder_SG_SSAWiiU_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SSF
			// 
			this.tmsi_OpenFolder_SSF.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_SSF_3DS,
            this.tmsi_OpenFolder_SSF_HC});
			this.tmsi_OpenFolder_SSF.Name = "tmsi_OpenFolder_SSF";
			this.tmsi_OpenFolder_SSF.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SSF.Text = "Skylanders Swap Force";
			// 
			// tmsi_OpenFolder_SSF_3DS
			// 
			this.tmsi_OpenFolder_SSF_3DS.Name = "tmsi_OpenFolder_SSF_3DS";
			this.tmsi_OpenFolder_SSF_3DS.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFolder_SSF_3DS.Text = "3DS";
			this.tmsi_OpenFolder_SSF_3DS.Click += new System.EventHandler(this.OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SSF_HC
			// 
			this.tmsi_OpenFolder_SSF_HC.Name = "tmsi_OpenFolder_SSF_HC";
			this.tmsi_OpenFolder_SSF_HC.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFolder_SSF_HC.Text = "Home Console";
			this.tmsi_OpenFolder_SSF_HC.Click += new System.EventHandler(this.OpenFolder_SSF_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_STT
			// 
			this.tmsi_OpenFolder_STT.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_STT_3DS,
            this.tmsi_OpenFolder_STT_HC});
			this.tmsi_OpenFolder_STT.Name = "tmsi_OpenFolder_STT";
			this.tmsi_OpenFolder_STT.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_STT.Text = "Skylanders Trap Team";
			// 
			// tmsi_OpenFolder_STT_3DS
			// 
			this.tmsi_OpenFolder_STT_3DS.Name = "tmsi_OpenFolder_STT_3DS";
			this.tmsi_OpenFolder_STT_3DS.Size = new System.Drawing.Size(195, 22);
			this.tmsi_OpenFolder_STT_3DS.Text = "3DS";
			this.tmsi_OpenFolder_STT_3DS.Click += new System.EventHandler(this.OpenFolder_SSAWii_3DS_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_STT_HC
			// 
			this.tmsi_OpenFolder_STT_HC.Name = "tmsi_OpenFolder_STT_HC";
			this.tmsi_OpenFolder_STT_HC.Size = new System.Drawing.Size(195, 22);
			this.tmsi_OpenFolder_STT_HC.Text = "Home Console/Mobile";
			this.tmsi_OpenFolder_STT_HC.Click += new System.EventHandler(this.OpenFolder_SSC_SI2016_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SSC
			// 
			this.tmsi_OpenFolder_SSC.Name = "tmsi_OpenFolder_SSC";
			this.tmsi_OpenFolder_SSC.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SSC.Text = "Skylanders Superchargers";
			// 
			// tmsi_OpenFolder_SI
			// 
			this.tmsi_OpenFolder_SI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_OpenFolder_SI_2016,
            this.tmsi_OpenFolder_SI_Switch});
			this.tmsi_OpenFolder_SI.Name = "tmsi_OpenFolder_SI";
			this.tmsi_OpenFolder_SI.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SI.Text = "Skylanders Imaginators";
			// 
			// tmsi_OpenFolder_SI_2016
			// 
			this.tmsi_OpenFolder_SI_2016.Name = "tmsi_OpenFolder_SI_2016";
			this.tmsi_OpenFolder_SI_2016.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFolder_SI_2016.Text = "2016";
			this.tmsi_OpenFolder_SI_2016.Click += new System.EventHandler(this.OpenFolder_SSC_SI2016_ToolStripMenuItem_Click);
			// 
			// tmsi_OpenFolder_SI_Switch
			// 
			this.tmsi_OpenFolder_SI_Switch.Name = "tmsi_OpenFolder_SI_Switch";
			this.tmsi_OpenFolder_SI_Switch.Size = new System.Drawing.Size(180, 22);
			this.tmsi_OpenFolder_SI_Switch.Text = "Switch";
			// 
			// tmsi_OpenFolder_SLI
			// 
			this.tmsi_OpenFolder_SLI.Name = "tmsi_OpenFolder_SLI";
			this.tmsi_OpenFolder_SLI.Size = new System.Drawing.Size(229, 22);
			this.tmsi_OpenFolder_SLI.Text = "Skylanders Lost Islands";
			this.tmsi_OpenFolder_SLI.Click += new System.EventHandler(this.OpenFolder_SLI_ToolStripMenuItem_Click);
			// 
			// tmsi_ExtractFile
			// 
			this.tmsi_ExtractFile.Name = "tmsi_ExtractFile";
			this.tmsi_ExtractFile.Size = new System.Drawing.Size(180, 22);
			this.tmsi_ExtractFile.Text = "Extract File";
			this.tmsi_ExtractFile.Click += new System.EventHandler(this.ExtractSingleFile);
			// 
			// tmsi_ExtractAll
			// 
			this.tmsi_ExtractAll.Name = "tmsi_ExtractAll";
			this.tmsi_ExtractAll.Size = new System.Drawing.Size(180, 22);
			this.tmsi_ExtractAll.Text = "Extract All";
			this.tmsi_ExtractAll.Click += new System.EventHandler(this.ExtractAllFiles);
			// 
			// tmsi_Settings
			// 
			this.tmsi_Settings.Name = "tmsi_Settings";
			this.tmsi_Settings.Size = new System.Drawing.Size(180, 22);
			this.tmsi_Settings.Text = "Settings";
			this.tmsi_Settings.Click += new System.EventHandler(this.OpenSettings);
			// 
			// tmsi_Exit
			// 
			this.tmsi_Exit.Name = "tmsi_Exit";
			this.tmsi_Exit.Size = new System.Drawing.Size(180, 22);
			this.tmsi_Exit.Text = "Exit";
			this.tmsi_Exit.Click += new System.EventHandler(this.ExitApplication);
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
			this.Text = "igArchiveExtractor (1.01)";
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
		public System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile;
		public System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder;
		public System.Windows.Forms.ToolStripMenuItem tmsi_ExtractFile;
		public System.Windows.Forms.ToolStripMenuItem tmsi_ExtractAll;
		public System.Windows.Forms.ToolStripMenuItem tmsi_Exit;
		private System.Windows.Forms.ToolStripMenuItem tmsi_Settings;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSA;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SG;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSF;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_STT;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SI;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSA_3DS_Wii;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSA_WiiU;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SG_3DS;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SG_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSF_3DS;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SSF_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_STT_3DS;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_STT_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SI_2016;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SI_Switch;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFile_SLI;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSA;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSA_3DS_Wii;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSA_WiiU;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SG;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SG_3DS;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SG_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSF;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSF_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_STT;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_STT_3DS;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_STT_HC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSC;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SI;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SI_2016;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SI_Switch;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SLI;
		private System.Windows.Forms.ToolStripMenuItem tmsi_OpenFolder_SSF_3DS;
	}
}

