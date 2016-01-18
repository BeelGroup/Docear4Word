namespace Docear4Word.Forms
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblHomePage = new System.Windows.Forms.Label();
			this.llHomePage = new System.Windows.Forms.LinkLabel();
			this.lblContactAndSupport = new System.Windows.Forms.Label();
			this.llContactAndSupport = new System.Windows.Forms.LinkLabel();
			this.llApplicationFolder = new System.Windows.Forms.LinkLabel();
			this.lblApplicationFolder = new System.Windows.Forms.Label();
			this.lblCitationStyleFolder = new System.Windows.Forms.Label();
			this.llCitationStyleFolder = new System.Windows.Forms.LinkLabel();
			this.lblBibTexFileFolder = new System.Windows.Forms.Label();
			this.llBibTexFileFolder = new System.Windows.Forms.LinkLabel();
			this.lblLicence = new System.Windows.Forms.Label();
			this.llLicence = new System.Windows.Forms.LinkLabel();
			this.lblCredits = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.flpCitationStyles = new System.Windows.Forms.FlowLayoutPanel();
			this.lblCSL = new System.Windows.Forms.Label();
			this.lblCSL2 = new System.Windows.Forms.Label();
			this.llCSL = new System.Windows.Forms.LinkLabel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblCiteProc = new System.Windows.Forms.Label();
			this.lblFrank = new System.Windows.Forms.Label();
			this.llFrank = new System.Windows.Forms.LinkLabel();
			this.flpDevelopment = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDevelopment = new System.Windows.Forms.Label();
			this.lblSimon = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.footer = new Docear4Word.Forms.DialogFooter();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.flpCitationStyles.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flpDevelopment.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 356);
			this.panel1.Margin = new System.Windows.Forms.Padding(4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(680, 52);
			this.panel1.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.AutoSize = true;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(563, 12);
			this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 17, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(100, 26);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
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
			// lblHomePage
			// 
			this.lblHomePage.AutoSize = true;
			this.lblHomePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHomePage.Location = new System.Drawing.Point(8, 45);
			this.lblHomePage.Margin = new System.Windows.Forms.Padding(3);
			this.lblHomePage.Name = "lblHomePage";
			this.lblHomePage.Size = new System.Drawing.Size(86, 17);
			this.lblHomePage.TabIndex = 2;
			this.lblHomePage.Text = "Home Page:";
			// 
			// llHomePage
			// 
			this.llHomePage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llHomePage.AutoEllipsis = true;
			this.llHomePage.AutoSize = true;
			this.llHomePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llHomePage.Location = new System.Drawing.Point(152, 45);
			this.llHomePage.Margin = new System.Windows.Forms.Padding(3);
			this.llHomePage.Name = "llHomePage";
			this.llHomePage.Size = new System.Drawing.Size(555, 17);
			this.llHomePage.TabIndex = 3;
			this.llHomePage.TabStop = true;
			this.llHomePage.Text = "http://docear.org";
			this.llHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// lblContactAndSupport
			// 
			this.lblContactAndSupport.AutoSize = true;
			this.lblContactAndSupport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblContactAndSupport.Location = new System.Drawing.Point(8, 68);
			this.lblContactAndSupport.Margin = new System.Windows.Forms.Padding(3);
			this.lblContactAndSupport.Name = "lblContactAndSupport";
			this.lblContactAndSupport.Size = new System.Drawing.Size(127, 17);
			this.lblContactAndSupport.TabIndex = 4;
			this.lblContactAndSupport.Text = "Contact && Support:";
			// 
			// llContactAndSupport
			// 
			this.llContactAndSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llContactAndSupport.AutoEllipsis = true;
			this.llContactAndSupport.AutoSize = true;
			this.llContactAndSupport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llContactAndSupport.Location = new System.Drawing.Point(152, 68);
			this.llContactAndSupport.Margin = new System.Windows.Forms.Padding(3);
			this.llContactAndSupport.Name = "llContactAndSupport";
			this.llContactAndSupport.Size = new System.Drawing.Size(555, 17);
			this.llContactAndSupport.TabIndex = 5;
			this.llContactAndSupport.TabStop = true;
			this.llContactAndSupport.Text = "http://docear.org/docear/contact";
			this.llContactAndSupport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// llApplicationFolder
			// 
			this.llApplicationFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llApplicationFolder.AutoEllipsis = true;
			this.llApplicationFolder.AutoSize = true;
			this.llApplicationFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llApplicationFolder.Location = new System.Drawing.Point(152, 91);
			this.llApplicationFolder.Margin = new System.Windows.Forms.Padding(3);
			this.llApplicationFolder.Name = "llApplicationFolder";
			this.llApplicationFolder.Size = new System.Drawing.Size(555, 17);
			this.llApplicationFolder.TabIndex = 6;
			this.llApplicationFolder.TabStop = true;
			this.llApplicationFolder.Text = "C:\\Program Files\\Microsoft Office";
			this.llApplicationFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// lblApplicationFolder
			// 
			this.lblApplicationFolder.AutoSize = true;
			this.lblApplicationFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblApplicationFolder.Location = new System.Drawing.Point(8, 91);
			this.lblApplicationFolder.Margin = new System.Windows.Forms.Padding(3);
			this.lblApplicationFolder.Name = "lblApplicationFolder";
			this.lblApplicationFolder.Size = new System.Drawing.Size(125, 17);
			this.lblApplicationFolder.TabIndex = 7;
			this.lblApplicationFolder.Text = "Application Folder:";
			// 
			// lblCitationStyleFolder
			// 
			this.lblCitationStyleFolder.AutoSize = true;
			this.lblCitationStyleFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCitationStyleFolder.Location = new System.Drawing.Point(8, 114);
			this.lblCitationStyleFolder.Margin = new System.Windows.Forms.Padding(3);
			this.lblCitationStyleFolder.Name = "lblCitationStyleFolder";
			this.lblCitationStyleFolder.Size = new System.Drawing.Size(138, 17);
			this.lblCitationStyleFolder.TabIndex = 8;
			this.lblCitationStyleFolder.Text = "Citation Style Folder:";
			// 
			// llCitationStyleFolder
			// 
			this.llCitationStyleFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llCitationStyleFolder.AutoEllipsis = true;
			this.llCitationStyleFolder.AutoSize = true;
			this.llCitationStyleFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llCitationStyleFolder.Location = new System.Drawing.Point(152, 114);
			this.llCitationStyleFolder.Margin = new System.Windows.Forms.Padding(3);
			this.llCitationStyleFolder.Name = "llCitationStyleFolder";
			this.llCitationStyleFolder.Size = new System.Drawing.Size(555, 17);
			this.llCitationStyleFolder.TabIndex = 9;
			this.llCitationStyleFolder.TabStop = true;
			this.llCitationStyleFolder.Text = "C:\\Users\\Simon\\Docear4Word\\Styles";
			this.llCitationStyleFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// lblBibTexFileFolder
			// 
			this.lblBibTexFileFolder.AutoSize = true;
			this.lblBibTexFileFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBibTexFileFolder.Location = new System.Drawing.Point(8, 137);
			this.lblBibTexFileFolder.Margin = new System.Windows.Forms.Padding(3);
			this.lblBibTexFileFolder.Name = "lblBibTexFileFolder";
			this.lblBibTexFileFolder.Size = new System.Drawing.Size(125, 17);
			this.lblBibTexFileFolder.TabIndex = 10;
			this.lblBibTexFileFolder.Text = "BibTex File Folder:";
			// 
			// llBibTexFileFolder
			// 
			this.llBibTexFileFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llBibTexFileFolder.AutoEllipsis = true;
			this.llBibTexFileFolder.AutoSize = true;
			this.llBibTexFileFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llBibTexFileFolder.Location = new System.Drawing.Point(152, 137);
			this.llBibTexFileFolder.Margin = new System.Windows.Forms.Padding(3);
			this.llBibTexFileFolder.Name = "llBibTexFileFolder";
			this.llBibTexFileFolder.Size = new System.Drawing.Size(555, 17);
			this.llBibTexFileFolder.TabIndex = 11;
			this.llBibTexFileFolder.TabStop = true;
			this.llBibTexFileFolder.Text = "C:\\Users\\Simon\\Docear4Word\\BibTex";
			this.llBibTexFileFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// lblLicence
			// 
			this.lblLicence.AutoSize = true;
			this.lblLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLicence.Location = new System.Drawing.Point(8, 160);
			this.lblLicence.Margin = new System.Windows.Forms.Padding(3);
			this.lblLicence.Name = "lblLicence";
			this.lblLicence.Size = new System.Drawing.Size(61, 17);
			this.lblLicence.TabIndex = 12;
			this.lblLicence.Text = "Licence:";
			// 
			// llLicence
			// 
			this.llLicence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.llLicence.AutoEllipsis = true;
			this.llLicence.AutoSize = true;
			this.llLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llLicence.Location = new System.Drawing.Point(152, 160);
			this.llLicence.Margin = new System.Windows.Forms.Padding(3);
			this.llLicence.Name = "llLicence";
			this.llLicence.Size = new System.Drawing.Size(555, 17);
			this.llLicence.TabIndex = 13;
			this.llLicence.TabStop = true;
			this.llLicence.Text = "licence.txt";
			this.llLicence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// lblCredits
			// 
			this.lblCredits.AutoSize = true;
			this.lblCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCredits.Location = new System.Drawing.Point(8, 198);
			this.lblCredits.Margin = new System.Windows.Forms.Padding(3);
			this.lblCredits.Name = "lblCredits";
			this.lblCredits.Size = new System.Drawing.Size(56, 17);
			this.lblCredits.TabIndex = 14;
			this.lblCredits.Text = "Credits:";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 561F));
			this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.lblCopyright, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblHomePage, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.llLicence, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.llBibTexFileFolder, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.llHomePage, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.llCitationStyleFolder, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblContactAndSupport, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.llApplicationFolder, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblApplicationFolder, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.llContactAndSupport, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblCitationStyleFolder, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblCredits, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.lblLicence, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblBibTexFileFolder, 0, 5);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel1.RowCount = 10;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(680, 408);
			this.tableLayoutPanel1.TabIndex = 15;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.flpCitationStyles);
			this.panel4.Controls.Add(this.flowLayoutPanel1);
			this.panel4.Controls.Add(this.flpDevelopment);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(149, 198);
			this.panel4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(558, 150);
			this.panel4.TabIndex = 2;
			// 
			// flpCitationStyles
			// 
			this.flpCitationStyles.AutoSize = true;
			this.flpCitationStyles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flpCitationStyles.Controls.Add(this.lblCSL);
			this.flpCitationStyles.Controls.Add(this.lblCSL2);
			this.flpCitationStyles.Controls.Add(this.llCSL);
			this.flpCitationStyles.Dock = System.Windows.Forms.DockStyle.Top;
			this.flpCitationStyles.Location = new System.Drawing.Point(0, 71);
			this.flpCitationStyles.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.flpCitationStyles.Name = "flpCitationStyles";
			this.flpCitationStyles.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this.flpCitationStyles.Size = new System.Drawing.Size(558, 44);
			this.flpCitationStyles.TabIndex = 2;
			// 
			// lblCSL
			// 
			this.lblCSL.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCSL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCSL.Location = new System.Drawing.Point(3, 0);
			this.lblCSL.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblCSL.Name = "lblCSL";
			this.lblCSL.Size = new System.Drawing.Size(135, 17);
			this.lblCSL.TabIndex = 16;
			this.lblCSL.Text = "Citation Styles:";
			// 
			// lblCSL2
			// 
			this.lblCSL2.AutoSize = true;
			this.lblCSL2.Dock = System.Windows.Forms.DockStyle.Top;
			this.flpCitationStyles.SetFlowBreak(this.lblCSL2, true);
			this.lblCSL2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCSL2.Location = new System.Drawing.Point(138, 0);
			this.lblCSL2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblCSL2.Name = "lblCSL2";
			this.lblCSL2.Size = new System.Drawing.Size(227, 17);
			this.lblCSL2.TabIndex = 17;
			this.lblCSL2.Text = "Citation Style Language (CSL)";
			// 
			// llCSL
			// 
			this.llCSL.AutoEllipsis = true;
			this.llCSL.AutoSize = true;
			this.llCSL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llCSL.Location = new System.Drawing.Point(3, 17);
			this.llCSL.Name = "llCSL";
			this.llCSL.Size = new System.Drawing.Size(158, 17);
			this.llCSL.TabIndex = 18;
			this.llCSL.TabStop = true;
			this.llCSL.Text = "http://citationstyles.org/ ";
			this.llCSL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.lblCiteProc);
			this.flowLayoutPanel1.Controls.Add(this.lblFrank);
			this.flowLayoutPanel1.Controls.Add(this.llFrank);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 27);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(558, 44);
			this.flowLayoutPanel1.TabIndex = 19;
			// 
			// lblCiteProc
			// 
			this.lblCiteProc.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCiteProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCiteProc.Location = new System.Drawing.Point(3, 0);
			this.lblCiteProc.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblCiteProc.Name = "lblCiteProc";
			this.lblCiteProc.Size = new System.Drawing.Size(135, 17);
			this.lblCiteProc.TabIndex = 16;
			this.lblCiteProc.Text = "JavaScript CiteProc:";
			// 
			// lblFrank
			// 
			this.lblFrank.AutoSize = true;
			this.lblFrank.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.SetFlowBreak(this.lblFrank, true);
			this.lblFrank.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrank.Location = new System.Drawing.Point(138, 0);
			this.lblFrank.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblFrank.Name = "lblFrank";
			this.lblFrank.Size = new System.Drawing.Size(161, 17);
			this.lblFrank.TabIndex = 17;
			this.lblFrank.Text = "Frank G. Bennett, Jr.";
			// 
			// llFrank
			// 
			this.llFrank.AutoEllipsis = true;
			this.llFrank.AutoSize = true;
			this.llFrank.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.llFrank.Location = new System.Drawing.Point(3, 17);
			this.llFrank.Name = "llFrank";
			this.llFrank.Size = new System.Drawing.Size(325, 17);
			this.llFrank.TabIndex = 18;
			this.llFrank.TabStop = true;
			this.llFrank.Text = "https://bitbucket.org/fbennett/citeproc-js/wiki/Home";
			this.llFrank.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// flpDevelopment
			// 
			this.flpDevelopment.AutoSize = true;
			this.flpDevelopment.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flpDevelopment.Controls.Add(this.lblDevelopment);
			this.flpDevelopment.Controls.Add(this.lblSimon);
			this.flpDevelopment.Dock = System.Windows.Forms.DockStyle.Top;
			this.flpDevelopment.Location = new System.Drawing.Point(0, 0);
			this.flpDevelopment.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.flpDevelopment.Name = "flpDevelopment";
			this.flpDevelopment.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this.flpDevelopment.Size = new System.Drawing.Size(558, 27);
			this.flpDevelopment.TabIndex = 19;
			// 
			// lblDevelopment
			// 
			this.lblDevelopment.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblDevelopment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDevelopment.Location = new System.Drawing.Point(3, 0);
			this.lblDevelopment.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblDevelopment.Name = "lblDevelopment";
			this.lblDevelopment.Size = new System.Drawing.Size(135, 17);
			this.lblDevelopment.TabIndex = 16;
			this.lblDevelopment.Text = "Development:";
			// 
			// lblSimon
			// 
			this.lblSimon.AutoSize = true;
			this.lblSimon.Dock = System.Windows.Forms.DockStyle.Top;
			this.flpDevelopment.SetFlowBreak(this.lblSimon, true);
			this.lblSimon.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSimon.Location = new System.Drawing.Point(138, 0);
			this.lblSimon.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.lblSimon.Name = "lblSimon";
			this.lblSimon.Size = new System.Drawing.Size(101, 17);
			this.lblSimon.TabIndex = 17;
			this.lblSimon.Text = "Simon Hewitt";
			// 
			// lblCopyright
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.lblCopyright, 2);
			this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCopyright.Location = new System.Drawing.Point(8, 10);
			this.lblCopyright.Margin = new System.Windows.Forms.Padding(3, 5, 3, 15);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(699, 17);
			this.lblCopyright.TabIndex = 2;
			this.lblCopyright.Text = "(C) 2012 Docear by Joeran Beel, Stefan Langer, Marcel Genzmehr, and others";
			// 
			// footer
			// 
			this.footer.AutoSize = true;
			this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.footer.Location = new System.Drawing.Point(0, 408);
			this.footer.MaximumSize = new System.Drawing.Size(0, 21);
			this.footer.MinimumSize = new System.Drawing.Size(0, 21);
			this.footer.Name = "footer";
			this.footer.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.footer.Size = new System.Drawing.Size(680, 21);
			this.footer.TabIndex = 2;
			this.footer.TabStop = false;
			// 
			// AboutForm
			// 
			this.AcceptButton = this.btnClose;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(680, 433);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.footer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(585, 330);
			this.Name = "AboutForm";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Docear4Word v1.0";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.flpCitationStyles.ResumeLayout(false);
			this.flpCitationStyles.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flpDevelopment.ResumeLayout(false);
			this.flpDevelopment.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblHomePage;
		private System.Windows.Forms.LinkLabel llHomePage;
		private System.Windows.Forms.Label lblContactAndSupport;
		private System.Windows.Forms.LinkLabel llContactAndSupport;
		private System.Windows.Forms.LinkLabel llApplicationFolder;
		private System.Windows.Forms.Label lblApplicationFolder;
		private System.Windows.Forms.Label lblCitationStyleFolder;
		private System.Windows.Forms.LinkLabel llCitationStyleFolder;
		private System.Windows.Forms.Label lblBibTexFileFolder;
		private System.Windows.Forms.LinkLabel llBibTexFileFolder;
		private System.Windows.Forms.Label lblLicence;
		private System.Windows.Forms.LinkLabel llLicence;
		private System.Windows.Forms.Label lblCredits;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.FlowLayoutPanel flpCitationStyles;
		private System.Windows.Forms.Label lblCSL;
		private System.Windows.Forms.Label lblCSL2;
		private System.Windows.Forms.LinkLabel llCSL;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblCiteProc;
		private System.Windows.Forms.Label lblFrank;
		private System.Windows.Forms.LinkLabel llFrank;
		private System.Windows.Forms.FlowLayoutPanel flpDevelopment;
		private System.Windows.Forms.Label lblDevelopment;
		private System.Windows.Forms.Label lblSimon;
		private DialogFooter footer;
	}
}