using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Docear4Word.BibTex;

using System.Linq;

namespace Docear4Word.Forms
{
	[ComVisible(false)]
	public partial class AddReferencesForm: Form
	{
		static int LastHeight = -1;

		readonly ReferenceFilter referenceFilter = new ReferenceFilter { FilterType = FilterType.And };

		BibTexDatabase database;
		BindingListAce<SelectableReference> source;
		List<SelectableReference> allEntries;

		AuthorProcessorControl authorProcessorControl;

		public AddReferencesForm()
		{
			InitializeComponent();

			rbExact.Tag = FilterType.Exact;
			rbAnd.Tag = FilterType.And;
			rbOr.Tag = FilterType.Or;

			rbAuthorStandard.Tag = AuthorProcessorControl.Standard;
			rbAuthorOnly.Tag = AuthorProcessorControl.AuthorOnly;
			rbAuthorSuppressAuthor.Tag = AuthorProcessorControl.SuppressAuthor;
			rbSplitAuthor.Tag = AuthorProcessorControl.SplitAuthor;
		}

		public BibTexDatabase Database
		{
			get { return database; }
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			LastHeight = Height;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (LastHeight != -1)
			{
				Height = LastHeight;
				Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2);
			}

			new HeaderCheckBoxHandler(colCheckbox, grid);

			foreach(DataGridViewColumn column in grid.Columns)
			{
				if (column.ReadOnly)
				{
					column.DefaultCellStyle.BackColor = Color.Azure;
				}
			}

			Icon = ImageHelper.CreateIcon(Images.AddReferenceSmall, 15, true);

			UpdateClipboardButton();

			StartListenToClipboard();
		}

		void StartListenToClipboard()
		{
			try
			{
				NativeMethods.AddClipboardFormatListener(Handle);
			}	
			catch (Exception ex)
			{
				Helper.LogUnexpectedException("Failed starting listening to the Clipboard", ex);
			}
		}

		void StopListenToClipboard()
		{
			try
			{
				NativeMethods.RemoveClipboardFormatListener(Handle);
			}
			catch (Exception ex)
			{
				Helper.LogUnexpectedException("Failed stopping listening to the Clipboard", ex);
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
			{
				UpdateClipboardButton();
			}

			base.WndProc(ref m);
		}

		void UpdateClipboardButton()
		{
			btnPaste.Enabled = Database != null && Clipboard.ContainsText();
		}

		void PasteAndSelect()
		{
			try
			{
				if (!Clipboard.ContainsText())
				{
					btnPaste.Enabled = false;
					return;
				}

				if (DoSelect(Clipboard.GetText()))
				{
//					Clipboard.Clear();
				}
			}
			catch(Exception ex)
			{
				Helper.LogUnexpectedException("Failing whilst pasting text", ex);
			}
		}

		bool DoSelect(string text)
		{
			if (string.IsNullOrEmpty(text)) return false;

			text = text.Replace('\r', ' ');
			text = text.Replace('\n', ' ');

			var names = text.Split(new[] { ',', ';', '|', ' '}, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());

			var exactMatches = new List<string>();
			var nonMatches = new List<string>();

			foreach (var selectableRefence in allEntries)
			{
				selectableRefence.Selected = false;
			}

			foreach (var name in names)
			{
				var entry = Database.FindMatch(name);

				if (entry == null)
				{
					nonMatches.Add(name);
				}
				else
				{
					exactMatches.Add(entry.Name);
				}
			}

			foreach (var name in exactMatches)
			{
				var entry = allEntries.Find(reference => reference.ID == name);
				entry.Selected = true;
			}

			if (nonMatches.Count > 0)
			{
				cmbYear.SelectedIndex = 0;

				if (nonMatches.Count > 1)
				{
					rbOr.Checked = true;
				}
			}

			txtFilter.Text = string.Join(" ", nonMatches.ToArray());

			UpdateFilter();

			return true;
		}

		void btnPaste_Click(object sender, EventArgs e)
		{
			PasteAndSelect();
		}

		void btnClear_Click(object sender, EventArgs e)
		{
			txtFilter.Text = string.Empty;
		}

		void txtFilter_TextChanged(object sender, EventArgs e)
		{
			UpdateFilter();
		}

		void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateFilter();
		}

		void rbExact_CheckedChanged(object sender, EventArgs e)
		{
			var radioButton = (RadioButton) sender;
			if (!radioButton.Checked) return;

			referenceFilter.FilterType = (FilterType) radioButton.Tag;
			UpdateFilter();
		}

		void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			var hitLocation = grid.PointToClient(MousePosition);
			var hit = grid.HitTest(hitLocation.X, hitLocation.Y);
			if (hit.Type != DataGridViewHitTestType.Cell) return;

			var row = grid.Rows[hit.RowIndex];
			var cell = row.Cells[hit.ColumnIndex];
			if (!cell.ReadOnly) return;

			var item = (SelectableReference) row.DataBoundItem;
			if (item == null) return;

			row.Cells[colCheckbox.Name].Value = !item.Selected;
		}

		void grid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				btnAdd.PerformClick();
			}
		}

		void grid_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ' && grid.CurrentCell.ColumnIndex != colCheckbox.Index)
			{
				ToggleRowSelection();
				e.Handled = true;
			}
		}

		void ToggleRowSelection()
		{
			var currentRow = grid.CurrentRow;
			if (currentRow == null) return;

			var newSelectionState = !((bool) currentRow.Cells[colCheckbox.Name].Value);

			foreach(DataGridViewRow row in grid.SelectedRows)
			{
				row.Cells[colCheckbox.Name].Value = newSelectionState;
			}

			UpdateAddButton();
		}

		void UpdateFilter()
		{
			referenceFilter.TextFilter = txtFilter.Text;
			referenceFilter.YearFilter = (string) (cmbYear.SelectedIndex == 0 ? string.Empty : cmbYear.SelectedItem);

			if (txtFilter.Text.Length == 0 && cmbYear.SelectedIndex == 0)
			{
				source.ApplyFilter(null);
			}
			else
			{
				Predicate<SelectableReference> filter = referenceFilter.IsMatch;

				if (referenceFilter.IsTextFilterSingleWordAndSingleSpace && referenceFilter.YearFilter.Length == 0)
				{
					var exactMatchToFind = referenceFilter.TextFilter.Substring(0, referenceFilter.TextFilter.Length - 1);

					foreach (var selectableReference in allEntries)
					{
						if (string.Equals(selectableReference.ID, exactMatchToFind, StringComparison.CurrentCultureIgnoreCase))
						{
							var referenceCopy = selectableReference;
							filter = reference => reference == referenceCopy;
							break;
						}
					}
				}

				source.ApplyFilter(filter);
			}

			UpdateTotalsLabel();
		}

		void UpdateTotalsLabel()
		{
			if (source.TotalItemCount == source.Count)
			{
				lblTotal.Text = string.Format("Total: {0}", source.TotalItemCount);
			}
			else
			{
				lblTotal.Text = string.Format("Total: {0} ({1} matching)", source.TotalItemCount, source.Count);
			}

			UpdateAddButton();
		}

		void UpdateAddButton()
		{
			var selectedCount = 0;

			foreach (var entry in allEntries)
			{
				if (entry.Selected) selectedCount++;
			}

			if (selectedCount == 0 && source.Count != 1)
			{
				btnAdd.Text = "Add Reference";
				btnAdd.Enabled = false;
			}
			else
			{
				var text = string.Format(selectedCount > 1 ? "Add {0} References" : "Add Reference", selectedCount);
				btnAdd.Text = text;

				btnAdd.Enabled = true;
			}
		}

		public void Reset(BibTexDatabase database)
		{
			if (database == null)
			{
				database = new BibTexDatabase();
			}

			this.database = database;

			allEntries = new List<SelectableReference>(database.EntryCount);

			var yearList = new List<string>();

			foreach(var entry in database.Entries)
			{
				var selectableReference = new SelectableReference
				                         	{
				                         		Selected = false,
												ID = entry.Name,
				                         		Title = entry["title", TagEntry.Empty].Display,
				                         		Authors = entry["author", TagEntry.Empty].Display,
				                         		Year = entry["year", TagEntry.Empty].Display,
				                         		Timestamp = entry["timestamp", TagEntry.Empty].Display,
												Pages = string.Empty
				                         	};

				allEntries.Add(selectableReference);

				if (selectableReference.Year.Length == 4 && !yearList.Contains(selectableReference.Year))
				{
					yearList.Add(selectableReference.Year);
				}
			}

			source = new BindingListAce<SelectableReference>(allEntries)
			             	{
			             		AllowNew = false,
			             		AllowRemove = false
			             	};


			bsSelectableReferences.DataSource = source;

			grid.AutoResizeColumns();

			bsSelectableReferences.ListChanged +=
				(sender, args) =>
					{
						Debug.WriteLine("bsSelectableReferences.ListChanged.ListChanged " + args.ListChangedType);
						UpdateAddButton();
					};

			yearList.Sort();
			yearList.Insert(0, "(any)");
			cmbYear.Items.AddRange(yearList.ToArray());
			cmbYear.SelectedIndex = 0;

			UpdateFilter();

			btnChooseDatabase.Visible = Settings.Instance.AllowPerDocumentDatabases;
		}

		private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex > -1 && (bool) grid.Rows[e.RowIndex].Cells[colCheckbox.Index].Value)
			{
				e.CellStyle.BackColor = Color.PowderBlue;
			}
		}

		public List<EntryAndPagePair> GetSelectedReferences()
		{
			var result = new List<EntryAndPagePair>();

			var selectedEntries = allEntries.FindAll(reference => reference.Selected);

			// Nothing selected?
			if (selectedEntries.Count == 0)
			{
				// No, so was only one item available anyway?
				if (source.Count != 1)
				{
					// No, so just return nothing (should not get here since Add Reference button should be disabled for this scenario)
					return result;
				}
				
				// Yes, so pretend it was selected
				selectedEntries.Add(source[0]);
			}

			foreach(var selectedEntry in selectedEntries)
			{
				var entry = database[selectedEntry.ID];
				if (entry == null) continue;

				result.Add(new EntryAndPagePair(entry, selectedEntry.Pages)
				           {
					           AuthorProcessorControl = authorProcessorControl
				           });
			}

			return result;
		}

		public void SetSelectedReferences(IEnumerable<EntryAndPagePair> entryAndPagePairs)
		{
			Text = "Edit References...";

			rbSplitAuthor.Visible = false;

			btnChooseDatabase.Visible = false;

			SelectableReference firstReference = null;

			foreach(var entryAndPagePair in entryAndPagePairs)
			{
				var entry = allEntries.Find(reference => reference.ID == entryAndPagePair.EntryName);
				if (entry == null) continue;

				entry.Selected = true;
				entry.Pages = entryAndPagePair.PageNumberOverride;

				if (firstReference == null)
				{
					firstReference = entry;

					switch (entryAndPagePair.AuthorProcessorControl)
					{
						case AuthorProcessorControl.AuthorOnly:
							rbAuthorOnly.Checked = true;
							break;

						case AuthorProcessorControl.SuppressAuthor:
							rbAuthorSuppressAuthor.Checked = true;
							break;

						default:
							rbAuthorStandard.Checked = true;
							break;
					}
				}
			}

			if (firstReference != null)
			{
				var firstIndex = bsSelectableReferences.IndexOf(firstReference);

				if (firstIndex != -1)
				{
					bsSelectableReferences.Position = firstIndex;
				}
			}

			UpdateAddButton();
		}

		void btnChooseDatabase_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			                     	{
			                     		Title = "Choose BibTex database...",
										ValidateNames = true,
			                     	};

			if (database != null)
			{
				if (database.Filename != null)
				{
					var fileInfo = new FileInfo(database.Filename);

					openFileDialog.InitialDirectory = fileInfo.DirectoryName;
					openFileDialog.FileName = fileInfo.Name;
				}
			}

			if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

			// If the user selected the same file we already had, then do nothing
			if (database != null && openFileDialog.FileName == database.Filename) return;

			var newDatabase = BibTexHelper.LoadBibTexDatabase(openFileDialog.FileName);
			if (newDatabase == null)
			{
				Helper.ShowCorruptBibtexDatabaseMessage(openFileDialog.FileName);

				return;
			}

			Reset(newDatabase);
		}

		private void txtFilter_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Up || e.KeyData == Keys.Down)
			{
				grid.Focus();
			}
		}

		private void AddReferencesForm_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case (Keys.Alt | Keys.Down):
					{
						var lastIndex = bsSelectableReferences.Count - 1;
						var index = bsSelectableReferences.Position;

						while (++index <= lastIndex)
						{
							var dataRow = bsSelectableReferences[index] as SelectableReference;
							if (dataRow == null || !dataRow.Selected) continue;

							bsSelectableReferences.Position = index;
							break;
						}

						e.Handled = true;
					}
					break;

				case (Keys.Alt | Keys.Up):
					{
						var index = bsSelectableReferences.Position;

						while (--index >= 0)
						{
							var dataRow = bsSelectableReferences[index] as SelectableReference;
							if (dataRow == null || !dataRow.Selected) continue;

							bsSelectableReferences.Position = index;
							break;
						}

						e.Handled = true;
					}
					break;
			}
		}

		void rbAuthorStandard_CheckedChanged(object sender, EventArgs e)
		{
			var radioButton = (RadioButton) sender;
			if (!radioButton.Checked) return;

			authorProcessorControl = (AuthorProcessorControl) radioButton.Tag;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			StopListenToClipboard();

			base.Dispose(disposing);
		}
	}

	public enum FilterType
	{
		Exact,
		And,
		Or
	}

	[ComVisible(false)]
	public class ReferenceFilter
	{
		string textFilter;
		string yearFilter;
		FilterType filterType;
		bool isTextFilterSingleWordAndSingleSpace;

		public bool IsTextFilterSingleWordAndSingleSpace
		{
			get { return isTextFilterSingleWordAndSingleSpace; }
		}

		public string TextFilter
		{
			get { return textFilter; }
			set
			{
				textFilter = value;

				var spacePosition = textFilter.IndexOf(' ');
				isTextFilterSingleWordAndSingleSpace = spacePosition > 0 && spacePosition == textFilter.Length - 1;
			}
		}

		public string YearFilter
		{
			get { return yearFilter; }
			set { yearFilter = value; }
		}

		public FilterType FilterType
		{
			get { return filterType; }
			set { filterType = value; }
		}

		public bool IsMatch(SelectableReference reference)
		{
			// Quick return if year does not match
			if (!string.IsNullOrEmpty(yearFilter) && reference.Year != yearFilter) return false;

			if (string.IsNullOrEmpty(textFilter)) return true;

			if (FilterType == FilterType.Exact)
			{
				return IsTextMatch(reference, textFilter);
			}

			var textItems = textFilter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (FilterType == FilterType.And)
			{
				foreach (var textItem in textItems)
				{
					if (!IsTextMatch(reference, textItem)) return false;
				}

				return true;
			}
			
			foreach (var textItem in textItems)
			{
				if (IsTextMatch(reference, textItem)) return true;
			}

			return false;
		}

		static bool IsTextMatch(SelectableReference reference, string text)
		{
			return reference.Title.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
			       reference.Authors.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
			       reference.ID.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
		}
	}

	internal static class NativeMethods
	{
		// See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
		public const int WM_CLIPBOARDUPDATE = 0x031D;
		public static IntPtr HWND_MESSAGE = new IntPtr(-3);

		// See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

		// See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
		// See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
	}
}
