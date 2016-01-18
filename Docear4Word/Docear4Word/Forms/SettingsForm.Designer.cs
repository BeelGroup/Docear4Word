namespace Docear4Word.Forms
{
	partial class SettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.chkRefreshUpdatesCitationsFromDatabase = new System.Windows.Forms.CheckBox();
			this.gbDefaultDatabase = new System.Windows.Forms.GroupBox();
			this.rbDocearDefault = new System.Windows.Forms.RadioButton();
			this.lblEnvironmentVariable = new System.Windows.Forms.Label();
			this.lblCustomDataFilename = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.rbCustom = new System.Windows.Forms.RadioButton();
			this.footer = new Docear4Word.Forms.DialogFooter();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.gbOptions.SuspendLayout();
			this.gbDefaultDatabase.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 350);
			this.panel1.Margin = new System.Windows.Forms.Padding(4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(680, 52);
			this.panel1.TabIndex = 1;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.AutoSize = true;
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Location = new System.Drawing.Point(454, 12);
			this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 17, 4);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(100, 26);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.AutoSize = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(564, 12);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 17, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 26);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox1.Image = global::Docear4Word.Images.LogoVeryTiny;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
			this.pictureBox1.Size = new System.Drawing.Size(202, 52);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.gbOptions);
			this.panel2.Controls.Add(this.gbDefaultDatabase);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(10);
			this.panel2.Size = new System.Drawing.Size(680, 350);
			this.panel2.TabIndex = 0;
			// 
			// gbOptions
			// 
			this.gbOptions.Controls.Add(this.chkRefreshUpdatesCitationsFromDatabase);
			this.gbOptions.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbOptions.Location = new System.Drawing.Point(10, 153);
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.Size = new System.Drawing.Size(660, 52);
			this.gbOptions.TabIndex = 1;
			this.gbOptions.TabStop = false;
			this.gbOptions.Text = "Options";
			// 
			// chkRefreshUpdatesCitationsFromDatabase
			// 
			this.chkRefreshUpdatesCitationsFromDatabase.AutoSize = true;
			this.chkRefreshUpdatesCitationsFromDatabase.Location = new System.Drawing.Point(13, 27);
			this.chkRefreshUpdatesCitationsFromDatabase.Name = "chkRefreshUpdatesCitationsFromDatabase";
			this.chkRefreshUpdatesCitationsFromDatabase.Size = new System.Drawing.Size(280, 17);
			this.chkRefreshUpdatesCitationsFromDatabase.TabIndex = 0;
			this.chkRefreshUpdatesCitationsFromDatabase.Text = "Refresh automatically updates citations from database";
			this.chkRefreshUpdatesCitationsFromDatabase.UseVisualStyleBackColor = true;
			// 
			// gbDefaultDatabase
			// 
			this.gbDefaultDatabase.Controls.Add(this.rbDocearDefault);
			this.gbDefaultDatabase.Controls.Add(this.lblEnvironmentVariable);
			this.gbDefaultDatabase.Controls.Add(this.lblCustomDataFilename);
			this.gbDefaultDatabase.Controls.Add(this.btnSelect);
			this.gbDefaultDatabase.Controls.Add(this.rbCustom);
			this.gbDefaultDatabase.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbDefaultDatabase.Location = new System.Drawing.Point(10, 10);
			this.gbDefaultDatabase.Name = "gbDefaultDatabase";
			this.gbDefaultDatabase.Size = new System.Drawing.Size(660, 143);
			this.gbDefaultDatabase.TabIndex = 0;
			this.gbDefaultDatabase.TabStop = false;
			this.gbDefaultDatabase.Text = "BibTex Database";
			// 
			// rbDocearDefault
			// 
			this.rbDocearDefault.AutoSize = true;
			this.rbDocearDefault.Location = new System.Drawing.Point(13, 30);
			this.rbDocearDefault.Name = "rbDocearDefault";
			this.rbDocearDefault.Size = new System.Drawing.Size(169, 17);
			this.rbDocearDefault.TabIndex = 0;
			this.rbDocearDefault.TabStop = true;
			this.rbDocearDefault.Text = "Use &Docear default BibTex file";
			this.rbDocearDefault.UseVisualStyleBackColor = true;
			// 
			// lblEnvironmentVariable
			// 
			this.lblEnvironmentVariable.AutoSize = true;
			this.lblEnvironmentVariable.Location = new System.Drawing.Point(60, 54);
			this.lblEnvironmentVariable.Name = "lblEnvironmentVariable";
			this.lblEnvironmentVariable.Size = new System.Drawing.Size(50, 13);
			this.lblEnvironmentVariable.TabIndex = 4;
			this.lblEnvironmentVariable.Text = "currently:";
			// 
			// lblCustomDataFilename
			// 
			this.lblCustomDataFilename.AutoEllipsis = true;
			this.lblCustomDataFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblCustomDataFilename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblCustomDataFilename.Location = new System.Drawing.Point(147, 110);
			this.lblCustomDataFilename.Name = "lblCustomDataFilename";
			this.lblCustomDataFilename.Size = new System.Drawing.Size(489, 23);
			this.lblCustomDataFilename.TabIndex = 3;
			this.lblCustomDataFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSelect
			// 
			this.btnSelect.AutoSize = true;
			this.btnSelect.Location = new System.Drawing.Point(36, 110);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(91, 23);
			this.btnSelect.TabIndex = 2;
			this.btnSelect.Text = "Select file...";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// rbCustom
			// 
			this.rbCustom.AutoSize = true;
			this.rbCustom.Location = new System.Drawing.Point(13, 83);
			this.rbCustom.Name = "rbCustom";
			this.rbCustom.Size = new System.Drawing.Size(158, 17);
			this.rbCustom.TabIndex = 1;
			this.rbCustom.TabStop = true;
			this.rbCustom.Text = "Specify your &own BibTex file";
			this.rbCustom.UseVisualStyleBackColor = true;
			// 
			// footer
			// 
			this.footer.AutoSize = true;
			this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.footer.Location = new System.Drawing.Point(0, 402);
			this.footer.MaximumSize = new System.Drawing.Size(0, 21);
			this.footer.MinimumSize = new System.Drawing.Size(0, 21);
			this.footer.Name = "footer";
			this.footer.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.footer.Size = new System.Drawing.Size(680, 21);
			this.footer.TabIndex = 15;
			this.footer.TabStop = false;
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(680, 427);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.footer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(585, 330);
			this.Name = "SettingsForm";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Docear4Word Settings";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.gbOptions.ResumeLayout(false);
			this.gbOptions.PerformLayout();
			this.gbDefaultDatabase.ResumeLayout(false);
			this.gbDefaultDatabase.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox gbDefaultDatabase;
		private System.Windows.Forms.Label lblCustomDataFilename;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.RadioButton rbCustom;
		private System.Windows.Forms.RadioButton rbDocearDefault;
		private System.Windows.Forms.Label lblEnvironmentVariable;
		private System.Windows.Forms.GroupBox gbOptions;
		private System.Windows.Forms.CheckBox chkRefreshUpdatesCitationsFromDatabase;
		private DialogFooter footer;
	}
}