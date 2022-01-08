
namespace IGAE_GUI.IGA
{
	public partial class IGA_BuildForm
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
			this.dgvItems = new System.Windows.Forms.DataGridView();
			this.btnAddRow = new System.Windows.Forms.Button();
			this.btnRemoveRow = new System.Windows.Forms.Button();
			this.btnSaveCSV = new System.Windows.Forms.Button();
			this.btnLoadCSV = new System.Windows.Forms.Button();
			this.btnBuild = new System.Windows.Forms.Button();
			this.cbVersion = new System.Windows.Forms.ComboBox();
			this.chkCaseInsensitiveHash = new System.Windows.Forms.CheckBox();
			this.chkHashNameAndExtensionOnly = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			// dgvItems
			//
			this.dgvItems.Location = new System.Drawing.Point(12, 12);
			this.dgvItems.Name = "dgvItems";
			this.dgvItems.Size = new System.Drawing.Size(476, 446);
			this.dgvItems.TabIndex = 1;
			this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvItems.ColumnCount = 2;
			this.dgvItems.Columns[0].Name = "Real File Path";
			this.dgvItems.Columns[0].ValueType = typeof(string);
			this.dgvItems.Columns[0].Width = 203;
			this.dgvItems.Columns[1].Name = "In-Game File Path";
			this.dgvItems.Columns[1].ValueType = typeof(string);
			this.dgvItems.Columns[1].Width = 203;
			// 
			// btnAddRow
			// 
			this.btnAddRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddRow.Location = new System.Drawing.Point(326, 495);
			this.btnAddRow.Name = "btnAddRow";
			this.btnAddRow.Size = new System.Drawing.Size(75, 23);
			this.btnAddRow.TabIndex = 10;
			this.btnAddRow.Text = "Add Row";
			this.btnAddRow.UseVisualStyleBackColor = true;
			this.btnAddRow.Click += new System.EventHandler(this.AddRow);
			// 
			// btnRemoveRow
			// 
			this.btnRemoveRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveRow.Location = new System.Drawing.Point(418, 495);
			this.btnRemoveRow.Name = "btnRemoveRow";
			this.btnRemoveRow.Size = new System.Drawing.Size(75, 23);
			this.btnRemoveRow.TabIndex = 10;
			this.btnRemoveRow.Text = "Remove Row";
			this.btnRemoveRow.UseVisualStyleBackColor = true;
			this.btnRemoveRow.Click += new System.EventHandler(this.RemoveRow);
			// 
			// btnSaveCSV
			// 
			this.btnSaveCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveCSV.Location = new System.Drawing.Point(248, 495);
			this.btnSaveCSV.Name = "btnSaveCSV";
			this.btnSaveCSV.Size = new System.Drawing.Size(75, 23);
			this.btnSaveCSV.TabIndex = 10;
			this.btnSaveCSV.Text = "Save as CSV";
			this.btnSaveCSV.UseVisualStyleBackColor = true;
			this.btnSaveCSV.Click += new System.EventHandler(this.SaveCSV);
			// 
			// btnLoadCSV
			// 
			this.btnLoadCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadCSV.Location = new System.Drawing.Point(167, 495);
			this.btnLoadCSV.Name = "btnLoadCSV";
			this.btnLoadCSV.Size = new System.Drawing.Size(75, 23);
			this.btnLoadCSV.TabIndex = 10;
			this.btnLoadCSV.Text = "Load CSV";
			this.btnLoadCSV.UseVisualStyleBackColor = true;
			this.btnLoadCSV.Click += new System.EventHandler(this.LoadCSV);
			// 
			// btnBuild
			// 
			this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBuild.Location = new System.Drawing.Point(88, 495);
			this.btnBuild.Name = "btnBuild";
			this.btnBuild.Size = new System.Drawing.Size(75, 23);
			this.btnBuild.TabIndex = 10;
			this.btnBuild.Text = "Build";
			this.btnBuild.UseVisualStyleBackColor = true;
			this.btnBuild.Click += new System.EventHandler(this.Build);
			// 
			// cbVersion
			// 
			this.cbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVersion.Location = new System.Drawing.Point(7, 495);
			this.cbVersion.Name = "cbVersion";
			this.cbVersion.Size = new System.Drawing.Size(75, 23);
			this.cbVersion.DropDownWidth = 75;
			this.cbVersion.TabIndex = 10;
			this.cbVersion.Items.AddRange(new string[]{"SSA Wii/3DS", "SSA Wii U", "SG", "SG 3DS", "SG Alpha", "SSF", "SSF 3DS", "SSF Alpha", "STT", "STT 3DS", "SSC"});
			//
			// chkCaseInsensitiveHash
			//
			this.chkCaseInsensitiveHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkCaseInsensitiveHash.Location = new System.Drawing.Point(12, 465);
			this.chkCaseInsensitiveHash.Name = "chkCaseInsensitiveHash";
			this.chkCaseInsensitiveHash.Text = "kCaseInsensitiveHash";
			this.chkCaseInsensitiveHash.Size = new System.Drawing.Size(120, 23);
			this.chkCaseInsensitiveHash.TabIndex = 10;
			this.chkCaseInsensitiveHash.CheckedChanged += new System.EventHandler(UpdateFlags);
			//
			// chkHashNameAndExtensionOnly
			// 
			this.chkHashNameAndExtensionOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkHashNameAndExtensionOnly.Location = new System.Drawing.Point(135, 465);
			this.chkHashNameAndExtensionOnly.Name = "chkHashNameAndExtensionOnly";
			this.chkHashNameAndExtensionOnly.Text = "kHashNameAndExtensionOnly";
			this.chkHashNameAndExtensionOnly.Size = new System.Drawing.Size(180, 23);
			this.chkHashNameAndExtensionOnly.TabIndex = 10;
			this.chkHashNameAndExtensionOnly.CheckedChanged += new System.EventHandler(UpdateFlags);
			//
			// IGZ_TextEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(500, 524);
			this.Controls.Add(this.dgvItems);
			this.Controls.Add(this.btnAddRow);
			this.Controls.Add(this.btnRemoveRow);
			this.Controls.Add(this.btnSaveCSV);
			this.Controls.Add(this.btnLoadCSV);
			this.Controls.Add(this.btnBuild);
			this.Controls.Add(this.cbVersion);
			this.Controls.Add(this.chkCaseInsensitiveHash);
			this.Controls.Add(this.chkHashNameAndExtensionOnly);
			this.Name = "IGA_BuildForm";
			this.Text = "Build IGA File";
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataGridView dgvItems;
		public System.Windows.Forms.Button btnAddRow;
		public System.Windows.Forms.Button btnRemoveRow;
		public System.Windows.Forms.Button btnSaveCSV;
		public System.Windows.Forms.Button btnLoadCSV;
		public System.Windows.Forms.Button btnBuild;
		public System.Windows.Forms.ComboBox cbVersion;
		public System.Windows.Forms.CheckBox chkCaseInsensitiveHash;
		public System.Windows.Forms.CheckBox chkHashNameAndExtensionOnly;
	}
}