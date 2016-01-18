using System;
using System.Drawing;
using System.Windows.Forms;

namespace Docear4Word.Forms
{
	public class HeaderCheckBoxHandler
	{
		readonly UnselectableCheckbox headerCheckBox;
		readonly DataGridViewCheckBoxColumn checkBoxColumn;
		readonly DataGridView grid;
		bool isHeaderCheckboxClicked;

		public HeaderCheckBoxHandler(DataGridViewCheckBoxColumn checkBoxColumn, DataGridView grid)
		{
			this.checkBoxColumn = checkBoxColumn;
			this.grid = grid;

			headerCheckBox = new UnselectableCheckbox
			                 	{
			                 		Size = new Size(15, 15),
									TabStop = false,
			                 	};
			

			grid.Controls.Add(headerCheckBox);

			headerCheckBox.MouseClick += OnHeaderCheckBoxMouseClick;

			grid.CellValueChanged += OnGridCellValueChanged;
			grid.CurrentCellDirtyStateChanged += OnGridCurrentCellDirtyStateChanged;
			grid.CellPainting += OnGridCellPainting;
		}


		void OnGridCurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (grid.CurrentCell is DataGridViewCheckBoxCell)
			{
				grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}

		void OnGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex == -1 && e.ColumnIndex == checkBoxColumn.Index)
			{
	            var headerRectangle = grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

				var point = new Point
				            	{
				            		X = headerRectangle.Location.X + (headerRectangle.Width - headerCheckBox.Width) / 2 + 1,
				            		Y = headerRectangle.Location.Y + (headerRectangle.Height - headerCheckBox.Height) / 2 + 1
				            	};


				headerCheckBox.Location = point;
			}
		}

		void OnGridCellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (isHeaderCheckboxClicked || e.ColumnIndex != checkBoxColumn.Index) return;

			headerCheckBox.Checked = (bool) grid[e.ColumnIndex, e.RowIndex].Value
			                         && AreAllVisibleRowsChecked();
		}

		bool AreAllVisibleRowsChecked()
		{
			var columnIndex = checkBoxColumn.Index;

			foreach(DataGridViewRow row in grid.Rows)
			{
				if ((bool) row.Cells[columnIndex].Value == false) return false;
			}

			return true;
		}

		void OnHeaderCheckBoxMouseClick(object sender, MouseEventArgs e)
		{
			isHeaderCheckboxClicked = true;

			try
			{
				var columnIndex = checkBoxColumn.Index;
				var isChecked = headerCheckBox.Checked;

				foreach (DataGridViewRow row in grid.Rows)
				{
					row.Cells[columnIndex].Value = isChecked;
				}

				((BindingSource) grid.DataSource).ResetBindings(false);
				//grid.RefreshEdit();
			}
			finally
			{
				isHeaderCheckboxClicked = false;
			}
		}

		class UnselectableCheckbox: CheckBox
		{
			public UnselectableCheckbox()
			{
				SetStyle(ControlStyles.Selectable, false);
			}
		}

	}
}