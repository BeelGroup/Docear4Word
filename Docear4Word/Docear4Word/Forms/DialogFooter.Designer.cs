namespace Docear4Word.Forms
{
	partial class DialogFooter
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.llDocearHomePage = new System.Windows.Forms.LinkLabel();
			this.llDocear4WordHomePage = new System.Windows.Forms.LinkLabel();
			this.llReportABug = new System.Windows.Forms.LinkLabel();
			this.llRequestAFeature = new System.Windows.Forms.LinkLabel();
			this.llDonate = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox1.Image = global::Docear4Word.Images.AboutSmall;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this.pictureBox1.Size = new System.Drawing.Size(19, 21);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// llDocearHomePage
			// 
			this.llDocearHomePage.AutoSize = true;
			this.llDocearHomePage.Dock = System.Windows.Forms.DockStyle.Left;
			this.llDocearHomePage.LinkArea = new System.Windows.Forms.LinkArea(0, 15);
			this.llDocearHomePage.Location = new System.Drawing.Point(19, 0);
			this.llDocearHomePage.Name = "llDocearHomePage";
			this.llDocearHomePage.Padding = new System.Windows.Forms.Padding(8, 4, 0, 0);
			this.llDocearHomePage.Size = new System.Drawing.Size(105, 17);
			this.llDocearHomePage.TabIndex = 1;
			this.llDocearHomePage.TabStop = true;
			this.llDocearHomePage.Tag = "http://www.docear.org/";
			this.llDocearHomePage.Text = "Docear Homepage";
			this.llDocearHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDocearHomePage_LinkClicked);
			// 
			// llDocear4WordHomePage
			// 
			this.llDocear4WordHomePage.AutoSize = true;
			this.llDocear4WordHomePage.Dock = System.Windows.Forms.DockStyle.Left;
			this.llDocear4WordHomePage.LinkArea = new System.Windows.Forms.LinkArea(0, 20);
			this.llDocear4WordHomePage.Location = new System.Drawing.Point(136, 0);
			this.llDocear4WordHomePage.Name = "llDocear4WordHomePage";
			this.llDocear4WordHomePage.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.llDocear4WordHomePage.Size = new System.Drawing.Size(129, 17);
			this.llDocear4WordHomePage.TabIndex = 2;
			this.llDocear4WordHomePage.TabStop = true;
			this.llDocear4WordHomePage.Tag = "http://www.docear.org/software/add-ons/docear4word/overview/";
			this.llDocear4WordHomePage.Text = "Docear4Word Homepage";
			this.llDocear4WordHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDocearHomePage_LinkClicked);
			// 
			// llReportABug
			// 
			this.llReportABug.AutoSize = true;
			this.llReportABug.Dock = System.Windows.Forms.DockStyle.Left;
			this.llReportABug.LinkArea = new System.Windows.Forms.LinkArea(0, 12);
			this.llReportABug.Location = new System.Drawing.Point(277, 0);
			this.llReportABug.Name = "llReportABug";
			this.llReportABug.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.llReportABug.Size = new System.Drawing.Size(69, 17);
			this.llReportABug.TabIndex = 3;
			this.llReportABug.TabStop = true;
			this.llReportABug.Tag = "http://www.docear.org/support/bug-report/";
			this.llReportABug.Text = "Report a bug";
			this.llReportABug.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDocearHomePage_LinkClicked);
			// 
			// llRequestAFeature
			// 
			this.llRequestAFeature.AutoSize = true;
			this.llRequestAFeature.Dock = System.Windows.Forms.DockStyle.Left;
			this.llRequestAFeature.LinkArea = new System.Windows.Forms.LinkArea(0, 17);
			this.llRequestAFeature.Location = new System.Drawing.Point(358, 0);
			this.llRequestAFeature.Name = "llRequestAFeature";
			this.llRequestAFeature.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.llRequestAFeature.Size = new System.Drawing.Size(92, 17);
			this.llRequestAFeature.TabIndex = 4;
			this.llRequestAFeature.TabStop = true;
			this.llRequestAFeature.Tag = "http://www.docear.org/software/add-ons/docear4word/feature-requests/";
			this.llRequestAFeature.Text = "Request a feature";
			this.llRequestAFeature.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDocearHomePage_LinkClicked);
			// 
			// llDonate
			// 
			this.llDonate.AutoSize = true;
			this.llDonate.Dock = System.Windows.Forms.DockStyle.Left;
			this.llDonate.LinkArea = new System.Windows.Forms.LinkArea(0, 20);
			this.llDonate.Location = new System.Drawing.Point(462, 0);
			this.llDonate.Name = "llDonate";
			this.llDonate.Padding = new System.Windows.Forms.Padding(0, 4, 8, 0);
			this.llDonate.Size = new System.Drawing.Size(49, 21);
			this.llDonate.TabIndex = 5;
			this.llDonate.TabStop = true;
			this.llDonate.Tag = "http://www.docear.org/give-back/donate/";
			this.llDonate.Text = "Donate";
			this.llDonate.UseCompatibleTextRendering = true;
			this.llDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDocearHomePage_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(450, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.label1.Size = new System.Drawing.Size(12, 17);
			this.label1.TabIndex = 6;
			this.label1.Text = "–";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new System.Drawing.Point(346, 0);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.label2.Size = new System.Drawing.Size(12, 17);
			this.label2.TabIndex = 7;
			this.label2.Text = "–";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Left;
			this.label3.Location = new System.Drawing.Point(265, 0);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.label3.Size = new System.Drawing.Size(12, 17);
			this.label3.TabIndex = 8;
			this.label3.Text = "–";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Left;
			this.label4.Location = new System.Drawing.Point(124, 0);
			this.label4.Name = "label4";
			this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.label4.Size = new System.Drawing.Size(12, 17);
			this.label4.TabIndex = 9;
			this.label4.Text = "–";
			// 
			// DialogFooter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.llDonate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.llRequestAFeature);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.llReportABug);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.llDocear4WordHomePage);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.llDocearHomePage);
			this.Controls.Add(this.pictureBox1);
			this.MaximumSize = new System.Drawing.Size(0, 21);
			this.MinimumSize = new System.Drawing.Size(0, 21);
			this.Name = "DialogFooter";
			this.Size = new System.Drawing.Size(511, 21);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel llDocearHomePage;
		private System.Windows.Forms.LinkLabel llDocear4WordHomePage;
		private System.Windows.Forms.LinkLabel llReportABug;
		private System.Windows.Forms.LinkLabel llRequestAFeature;
		private System.Windows.Forms.LinkLabel llDonate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}
