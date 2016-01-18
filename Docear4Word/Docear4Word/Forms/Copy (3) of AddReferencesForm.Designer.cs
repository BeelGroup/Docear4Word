namespace Docear4Word.Forms
{
	partial class AddReferencesForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.grid = new System.Windows.Forms.DataGridView();
			this.colCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.pagesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.authorsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.yearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timestampDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.bsSelectableReferences = new System.Windows.Forms.BindingSource(this.components);
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblFilter = new System.Windows.Forms.Label();
			this.lblTotal = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.rbAnd = new System.Windows.Forms.RadioButton();
			this.rbOr = new System.Windows.Forms.RadioButton();
			this.cmbYear = new System.Windows.Forms.ComboBox();
			this.lblYear = new System.Windows.Forms.Label();
			this.rbExact = new System.Windows.Forms.RadioButton();
			this.btnClearFilter = new System.Windows.Forms.Button();
			this.btnChooseDatabase = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnPaste = new System.Windows.Forms.Button();
			this.pnlAuthorControl = new System.Windows.Forms.Panel();
			this.rbAuthorStandard = new System.Windows.Forms.RadioButton();
			this.rbAuthorOnly = new System.Windows.Forms.RadioButton();
			this.rbAuthorSuppressAuthor = new System.Windows.Forms.RadioButton();
			this.rbSplitAuthor = new System.Windows.Forms.RadioButton();
			this.lblFiller = new System.Windows.Forms.Label();
			this.footer = new Docear4Word.Forms.DialogFooter();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsSelectableReferences)).BeginInit();
			this.panel1.SuspendLayout();
			this.pnlAuthorControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoGenerateColumns = false;
			this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.grid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckbox,
            this.pagesDataGridViewTextBoxColumn,
            this.Column1,
            this.titleDataGridViewTextBoxColumn,
            this.authorsDataGridViewTextBoxColumn,
            this.yearDataGridViewTextBoxColumn,
            this.timestampDataGridViewTextBoxColumn});
			this.grid.DataSource = this.bsSelectableReferences;
			this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.grid.Location = new System.Drawing.Point(14, 39);
			this.grid.Name = "grid";
			this.grid.RowHeadersVisible = false;
			this.grid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.Size = new System.Drawing.Size(804, 222);
			this.grid.TabIndex = 2;
			this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
			this.grid.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
			this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
			this.grid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grid_KeyPress);
			// 
			// colCheckbox
			// 
			this.colCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colCheckbox.DataPropertyName = "Selected";
			this.colCheckbox.HeaderText = "";
			this.colCheckbox.Name = "colCheckbox";
			this.colCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCheckbox.Width = 20;
			// 
			// pagesDataGridViewTextBoxColumn
			// 
			this.pagesDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.pagesDataGridViewTextBoxColumn.DataPropertyName = "Pages";
			this.pagesDataGridViewTextBoxColumn.HeaderText = "Pages";
			this.pagesDataGridViewTextBoxColumn.Name = "pagesDataGridViewTextBoxColumn";
			this.pagesDataGridViewTextBoxColumn.Width = 75;
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column1.DataPropertyName = "ID";
			this.Column1.FillWeight = 35F;
			this.Column1.HeaderText = "BibTex Key";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// titleDataGridViewTextBoxColumn
			// 
			this.titleDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
			this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
			this.titleDataGridViewTextBoxColumn.MinimumWidth = 50;
			this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
			this.titleDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// authorsDataGridViewTextBoxColumn
			// 
			this.authorsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.authorsDataGridViewTextBoxColumn.DataPropertyName = "Authors";
			this.authorsDataGridViewTextBoxColumn.HeaderText = "Authors";
			this.authorsDataGridViewTextBoxColumn.MinimumWidth = 50;
			this.authorsDataGridViewTextBoxColumn.Name = "authorsDataGridViewTextBoxColumn";
			this.authorsDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// yearDataGridViewTextBoxColumn
			// 
			this.yearDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.yearDataGridViewTextBoxColumn.DataPropertyName = "Year";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.yearDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.yearDataGridViewTextBoxColumn.HeaderText = "Year";
			this.yearDataGridViewTextBoxColumn.Name = "yearDataGridViewTextBoxColumn";
			this.yearDataGridViewTextBoxColumn.ReadOnly = true;
			this.yearDataGridViewTextBoxColumn.Width = 54;
			// 
			// timestampDataGridViewTextBoxColumn
			// 
			this.timestampDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.timestampDataGridViewTextBoxColumn.DataPropertyName = "Timestamp";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.timestampDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.timestampDataGridViewTextBoxColumn.HeaderText = "Timestamp";
			this.timestampDataGridViewTextBoxColumn.Name = "timestampDataGridViewTextBoxColumn";
			this.timestampDataGridViewTextBoxColumn.ReadOnly = true;
			this.timestampDataGridViewTextBoxColumn.Width = 75;
			// 
			// bsSelectableReferences
			// 
			this.bsSelectableReferences.AllowNew = false;
			this.bsSelectableReferences.DataSource = typeof(Docear4Word.SelectableReference);
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilter.Location = new System.Drawing.Point(52, 12);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(426, 20);
			this.txtFilter.TabIndex = 1;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			this.txtFilter.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtFilter_PreviewKeyDown);
			// 
			// lblFilter
			// 
			this.lblFilter.AutoSize = true;
			this.lblFilter.Location = new System.Drawing.Point(13, 16);
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(32, 13);
			this.lblFilter.TabIndex = 0;
			this.lblFilter.Text = "&Filter:";
			// 
			// lblTotal
			// 
			this.lblTotal.AutoSize = true;
			this.lblTotal.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblTotal.Location = new System.Drawing.Point(0, 0);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Padding = new System.Windows.Forms.Padding(0, 4, 4, 0);
			this.lblTotal.Size = new System.Drawing.Size(65, 17);
			this.lblTotal.TabIndex = 10;
			this.lblTotal.Text = "Total: 1234";
			// 
			// btnAdd
			// 
			this.btnAdd.AutoSize = true;
			this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnAdd.Location = new System.Drawing.Point(640, 0);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(89, 22);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "Add Reference";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnClose.Location = new System.Drawing.Point(729, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 22);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// rbAnd
			// 
			this.rbAnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbAnd.AutoSize = true;
			this.rbAnd.Checked = true;
			this.rbAnd.Location = new System.Drawing.Point(609, 13);
			this.rbAnd.Name = "rbAnd";
			this.rbAnd.Size = new System.Drawing.Size(44, 17);
			this.rbAnd.TabIndex = 6;
			this.rbAnd.TabStop = true;
			this.rbAnd.Text = "And";
			this.rbAnd.UseVisualStyleBackColor = true;
			this.rbAnd.CheckedChanged += new System.EventHandler(this.rbExact_CheckedChanged);
			// 
			// rbOr
			// 
			this.rbOr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbOr.AutoSize = true;
			this.rbOr.Location = new System.Drawing.Point(659, 13);
			this.rbOr.Name = "rbOr";
			this.rbOr.Size = new System.Drawing.Size(36, 17);
			this.rbOr.TabIndex = 7;
			this.rbOr.Text = "Or";
			this.rbOr.UseVisualStyleBackColor = true;
			this.rbOr.CheckedChanged += new System.EventHandler(this.rbExact_CheckedChanged);
			// 
			// cmbYear
			// 
			this.cmbYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbYear.FormattingEnabled = true;
			this.cmbYear.Location = new System.Drawing.Point(743, 12);
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Size = new System.Drawing.Size(75, 21);
			this.cmbYear.TabIndex = 9;
			this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
			// 
			// lblYear
			// 
			this.lblYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblYear.AutoSize = true;
			this.lblYear.Location = new System.Drawing.Point(705, 15);
			this.lblYear.Name = "lblYear";
			this.lblYear.Size = new System.Drawing.Size(32, 13);
			this.lblYear.TabIndex = 8;
			this.lblYear.Text = "Year:";
			// 
			// rbExact
			// 
			this.rbExact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbExact.AutoSize = true;
			this.rbExact.Location = new System.Drawing.Point(551, 13);
			this.rbExact.Name = "rbExact";
			this.rbExact.Size = new System.Drawing.Size(52, 17);
			this.rbExact.TabIndex = 5;
			this.rbExact.Text = "Exact";
			this.rbExact.UseVisualStyleBackColor = true;
			this.rbExact.CheckedChanged += new System.EventHandler(this.rbExact_CheckedChanged);
			// 
			// btnClearFilter
			// 
			this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClearFilter.Location = new System.Drawing.Point(478, 12);
			this.btnClearFilter.Name = "btnClearFilter";
			this.btnClearFilter.Size = new System.Drawing.Size(19, 20);
			this.btnClearFilter.TabIndex = 11;
			this.btnClearFilter.Text = "X";
			this.btnClearFilter.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnChooseDatabase
			// 
			this.btnChooseDatabase.AutoSize = true;
			this.btnChooseDatabase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnChooseDatabase.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnChooseDatabase.Location = new System.Drawing.Point(65, 0);
			this.btnChooseDatabase.Name = "btnChooseDatabase";
			this.btnChooseDatabase.Size = new System.Drawing.Size(111, 22);
			this.btnChooseDatabase.TabIndex = 12;
			this.btnChooseDatabase.Text = "Choose Database...";
			this.btnChooseDatabase.UseVisualStyleBackColor = true;
			this.btnChooseDatabase.Visible = false;
			this.btnChooseDatabase.Click += new System.EventHandler(this.btnChooseDatabase_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnPaste);
			this.panel1.Controls.Add(this.pnlAuthorControl);
			this.panel1.Controls.Add(this.txtFilter);
			this.panel1.Controls.Add(this.grid);
			this.panel1.Controls.Add(this.btnClearFilter);
			this.panel1.Controls.Add(this.lblFilter);
			this.panel1.Controls.Add(this.rbExact);
			this.panel1.Controls.Add(this.lblYear);
			this.panel1.Controls.Add(this.cmbYear);
			this.panel1.Controls.Add(this.rbOr);
			this.panel1.Controls.Add(this.rbAnd);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(12);
			this.panel1.Size = new System.Drawing.Size(830, 289);
			this.panel1.TabIndex = 13;
			// 
			// btnPaste
			// 
			this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPaste.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPaste.Enabled = false;
			this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnPaste.Location = new System.Drawing.Point(508, 12);
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Size = new System.Drawing.Size(31, 20);
			this.btnPaste.TabIndex = 17;
			this.btnPaste.Text = "PM";
			this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
			// 
			// pnlAuthorControl
			// 
			this.pnlAuthorControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAuthorControl.AutoSize = true;
			this.pnlAuthorControl.Controls.Add(this.btnChooseDatabase);
			this.pnlAuthorControl.Controls.Add(this.lblTotal);
			this.pnlAuthorControl.Controls.Add(this.rbAuthorStandard);
			this.pnlAuthorControl.Controls.Add(this.rbAuthorOnly);
			this.pnlAuthorControl.Controls.Add(this.rbAuthorSuppressAuthor);
			this.pnlAuthorControl.Controls.Add(this.rbSplitAuthor);
			this.pnlAuthorControl.Controls.Add(this.lblFiller);
			this.pnlAuthorControl.Controls.Add(this.btnAdd);
			this.pnlAuthorControl.Controls.Add(this.btnClose);
			this.pnlAuthorControl.Location = new System.Drawing.Point(14, 267);
			this.pnlAuthorControl.Name = "pnlAuthorControl";
			this.pnlAuthorControl.Size = new System.Drawing.Size(804, 22);
			this.pnlAuthorControl.TabIndex = 16;
			// 
			// rbAuthorStandard
			// 
			this.rbAuthorStandard.AutoSize = true;
			this.rbAuthorStandard.Checked = true;
			this.rbAuthorStandard.Dock = System.Windows.Forms.DockStyle.Right;
			this.rbAuthorStandard.Location = new System.Drawing.Point(267, 0);
			this.rbAuthorStandard.Name = "rbAuthorStandard";
			this.rbAuthorStandard.Size = new System.Drawing.Size(68, 22);
			this.rbAuthorStandard.TabIndex = 19;
			this.rbAuthorStandard.TabStop = true;
			this.rbAuthorStandard.Text = "Standard";
			this.rbAuthorStandard.UseVisualStyleBackColor = true;
			this.rbAuthorStandard.CheckedChanged += new System.EventHandler(this.rbAuthorStandard_CheckedChanged);
			// 
			// rbAuthorOnly
			// 
			this.rbAuthorOnly.AutoSize = true;
			this.rbAuthorOnly.Dock = System.Windows.Forms.DockStyle.Right;
			this.rbAuthorOnly.Location = new System.Drawing.Point(335, 0);
			this.rbAuthorOnly.Name = "rbAuthorOnly";
			this.rbAuthorOnly.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.rbAuthorOnly.Size = new System.Drawing.Size(86, 22);
			this.rbAuthorOnly.TabIndex = 20;
			this.rbAuthorOnly.Text = "Author-Only";
			this.rbAuthorOnly.UseVisualStyleBackColor = true;
			this.rbAuthorOnly.CheckedChanged += new System.EventHandler(this.rbAuthorStandard_CheckedChanged);
			// 
			// rbAuthorSuppressAuthor
			// 
			this.rbAuthorSuppressAuthor.AutoSize = true;
			this.rbAuthorSuppressAuthor.Dock = System.Windows.Forms.DockStyle.Right;
			this.rbAuthorSuppressAuthor.Location = new System.Drawing.Point(421, 0);
			this.rbAuthorSuppressAuthor.Name = "rbAuthorSuppressAuthor";
			this.rbAuthorSuppressAuthor.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.rbAuthorSuppressAuthor.Size = new System.Drawing.Size(109, 22);
			this.rbAuthorSuppressAuthor.TabIndex = 21;
			this.rbAuthorSuppressAuthor.Text = "Suppress-Author";
			this.rbAuthorSuppressAuthor.UseVisualStyleBackColor = true;
			this.rbAuthorSuppressAuthor.CheckedChanged += new System.EventHandler(this.rbAuthorStandard_CheckedChanged);
			// 
			// rbSplitAuthor
			// 
			this.rbSplitAuthor.AutoSize = true;
			this.rbSplitAuthor.Dock = System.Windows.Forms.DockStyle.Right;
			this.rbSplitAuthor.Location = new System.Drawing.Point(530, 0);
			this.rbSplitAuthor.Name = "rbSplitAuthor";
			this.rbSplitAuthor.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.rbSplitAuthor.Size = new System.Drawing.Size(96, 22);
			this.rbSplitAuthor.TabIndex = 22;
			this.rbSplitAuthor.Text = "Author + Year";
			this.rbSplitAuthor.UseVisualStyleBackColor = true;
			this.rbSplitAuthor.CheckedChanged += new System.EventHandler(this.rbAuthorStandard_CheckedChanged);
			// 
			// lblFiller
			// 
			this.lblFiller.AutoSize = true;
			this.lblFiller.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblFiller.Location = new System.Drawing.Point(626, 0);
			this.lblFiller.Name = "lblFiller";
			this.lblFiller.Padding = new System.Windows.Forms.Padding(10, 4, 4, 0);
			this.lblFiller.Size = new System.Drawing.Size(14, 17);
			this.lblFiller.TabIndex = 23;
			// 
			// footer
			// 
			this.footer.AutoSize = true;
			this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.footer.Location = new System.Drawing.Point(0, 289);
			this.footer.MaximumSize = new System.Drawing.Size(0, 23);
			this.footer.MinimumSize = new System.Drawing.Size(0, 23);
			this.footer.Name = "footer";
			this.footer.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this.footer.Size = new System.Drawing.Size(830, 23);
			this.footer.TabIndex = 14;
			this.footer.TabStop = false;
			// 
			// AddReferencesForm
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(830, 312);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.footer);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(620, 300);
			this.Name = "AddReferencesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add References...";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddReferencesForm_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsSelectableReferences)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.pnlAuthorControl.ResumeLayout(false);
			this.pnlAuthorControl.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.BindingSource bsSelectableReferences;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.RadioButton rbAnd;
		private System.Windows.Forms.RadioButton rbOr;
		private System.Windows.Forms.ComboBox cmbYear;
		private System.Windows.Forms.Label lblYear;
		private System.Windows.Forms.RadioButton rbExact;
		private System.Windows.Forms.Button btnClearFilter;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckbox;
		private System.Windows.Forms.DataGridViewTextBoxColumn pagesDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn authorsDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn yearDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewTextBoxColumn;
		private System.Windows.Forms.Button btnChooseDatabase;
		private System.Windows.Forms.Panel panel1;
		private DialogFooter footer;
		private System.Windows.Forms.Panel pnlAuthorControl;
		private System.Windows.Forms.RadioButton rbAuthorSuppressAuthor;
		private System.Windows.Forms.RadioButton rbAuthorOnly;
		private System.Windows.Forms.RadioButton rbAuthorStandard;
		private System.Windows.Forms.RadioButton rbSplitAuthor;
		private System.Windows.Forms.Label lblFiller;
		private System.Windows.Forms.Button btnPaste;
	}
}