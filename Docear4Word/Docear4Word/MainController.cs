using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Docear4Word.BibTex;
using Docear4Word.Forms;

using Word;

namespace Docear4Word
{
	public class MainController
	{
		const string StyleTitleRegistryValueName = "DefaultStyleTitle";
		const string StyleFilenameRegistryValueName = "DefaultStyleFilename";

		readonly AddinModule addinModule;
		readonly _Application wordApp;
		readonly Dictionary<_Document, DocumentController> documentControllers;
		readonly List<StyleInfo> styles;

		DocumentController currentDocumentController;
		StyleInfo defaultStyle;

		public MainController(AddinModule addinModule)
		{
			this.addinModule = addinModule;
			wordApp = addinModule.WordApp;
			documentControllers = new Dictionary<_Document, DocumentController>();
			styles = StyleHelper.GetStylesInfo();

			addinModule.UpdateRibbonStyles(styles);

			EnsureDocearFoldersExists();
		}

		static void EnsureDocearFoldersExists()
		{
			FolderHelper.EnsureFolderExists(FolderHelper.DocearPersonalDataFolder);
			FolderHelper.EnsureFolderExists(FolderHelper.DocearStylesFolder);
		}

		public DocumentController CurrentDocumentController
		{
			get { return currentDocumentController; }
		}

		public bool CurrentDocumentControllerIsReady
		{
			get { return currentDocumentController != null && currentDocumentController.CiteProc != null; }
		}

		public StyleInfo DefaultStyle
		{
			get
			{
				if (defaultStyle == null)
				{
					var styleTitleFromRegistry = RegistryHelper.ReadApplicationString(StyleTitleRegistryValueName);
					defaultStyle = FindStyleByTitle(styleTitleFromRegistry);

					if (defaultStyle == null)
					{
						if (defaultStyle == null && styles.Count > 0)
						{
							defaultStyle = styles[0];
						}
					}
				}

				return defaultStyle;
			}
			private set
			{
				if (value == defaultStyle || value == null) return;

				defaultStyle = value;

				RegistryHelper.WriteApplicationString(StyleTitleRegistryValueName, defaultStyle.Title);
				RegistryHelper.WriteApplicationString(StyleFilenameRegistryValueName, defaultStyle.FileInfo.Name);
			}
		}

		public void DoInsertTestData()
		{
			Debug.Assert(currentDocumentController != null);

			var bibTexDatabase = currentDocumentController.GetDatabase();
			if (bibTexDatabase == null) return;

			var entries = new List<Entry>(bibTexDatabase.Entries);
			if (entries.Count < 1) return;

			var selectedEntry = new List<EntryAndPagePair>
    		           	{
    		           		new EntryAndPagePair(entries[0]),
    		           	};
    		currentDocumentController.InsertCitation(selectedEntry);

			if (entries.Count >= 3)
			{
				var selectedEntries = new List<EntryAndPagePair>
				                      	{
				                      		new EntryAndPagePair(entries[1], pageNumberOverride: "123"),
				                      		new EntryAndPagePair(entries[2], pageNumberOverride: "456"),
				                      	};

				currentDocumentController.InsertCitation(selectedEntries);
			}

			var document = currentDocumentController.Document;
			var selection = document.Application.Selection;
			selection.Collapse(WdCollapseDirection.wdCollapseEnd);
			selection.Paragraphs.Add();

			currentDocumentController.InsertBibliography();
			OnWindowSelectionChange();
		}

		public void DoAddReference()
    	{
			if (!CurrentDocumentControllerIsReady) return;

			if (addinModule.IsEditReference())
			{
				DoEditReference();
				return;
			}

			BibTexDatabase currentDatabase = currentDocumentController.GetDatabase();
			if (currentDatabase == null)
			{
				ShowNoDatabaseMessage();                
				return;
			}
            

			var addReferencesForm = new AddReferencesForm();				

    		addReferencesForm.Reset(currentDatabase);

			var result = addReferencesForm.ShowDialog();
			if (result == DialogResult.Cancel)
			{
				addReferencesForm.Dispose();
				return;
			}

			// Keep a note of these as early as possible after closing dialog
			var isSequence = (Control.ModifierKeys & Keys.Control) != 0;
			var isLineSequence = isSequence && (Control.ModifierKeys & Keys.Shift) != 0;

			var entryAndPagePairs = addReferencesForm.GetSelectedReferences();            

			if (entryAndPagePairs.Count == 0) return;
            

			// Did the user change the database?
			if (addReferencesForm.Database != currentDatabase)
			{
				try
				{
					// Yes, so store it with the document
					currentDocumentController.SetDocumentDatabaseFilename(addReferencesForm.Database.Filename);
				}
				catch
				{}
			}

			addReferencesForm.Dispose();

			currentDocumentController.DoInsertCitation(entryAndPagePairs, isSequence, isLineSequence);
    	}

		static void ShowNoDatabaseMessage()
		{
			var filename = Settings.Instance.GetDefaultDatabaseFilename();

			if (File.Exists(filename))
			{
				Helper.ShowCorruptBibtexDatabaseMessage(filename);
			}
			else if (string.IsNullOrEmpty(filename))
			{
				MessageBox.Show("No BibTex database has been specified yet. \r\n\r\nGo into Settings and choose a source database first.", "Docear4Word: No BibTex database specified", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("The file \"" + filename + "\"\r\ncould not be found.\r\n\r\nPlease check Settings.", "Docear4Word: Missing BibTex database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		public void DoEditReference()
    	{
			if (!CurrentDocumentControllerIsReady) return;

			var currentDatabase = currentDocumentController.GetDatabase();
			if (currentDatabase == null)
			{
				ShowNoDatabaseMessage();
				return;
			}

			var selectionManager = new SelectionManager(currentDocumentController);
			if (!selectionManager.IsAtOrBeforeSingleCitation) return;

    		var fieldMatch = selectionManager.FirstFieldMatch;
    		var field = fieldMatch.Field;
    		var inlineCitation = currentDocumentController.CiteProc.CreateInlineCitationFromFieldJSON(field);

			var entryAndPagePairs = Helper.ExtractSources(inlineCitation, currentDatabase);

			var expectedItemCount = inlineCitation.CitationItems.Length;
			var foundItemCount = entryAndPagePairs.Count;

			if (foundItemCount != expectedItemCount)
			{
				var missingItemCount = expectedItemCount - foundItemCount;
				var message = expectedItemCount == 1
				              	? "The reference"
				              	: foundItemCount == 0
				              	  	? "None of references"
				              	  	: missingItemCount == 1
				              	  	  	? "One of the references"
				              	  	  	: missingItemCount + " references";

				message += " could not be found in the current database\r\nIf you continue and make changes, these will be lost.\r\n\r\nAre you sure you want to continue?";

				if (MessageBox.Show(message, "Edit Items Missing References Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}

			var editReferencesForm = new AddReferencesForm();				
    		editReferencesForm.Reset(currentDatabase);
    		editReferencesForm.SetSelectedReferences(entryAndPagePairs);

			var result = editReferencesForm.ShowDialog();
			if (result == DialogResult.Cancel) return;

    		entryAndPagePairs = editReferencesForm.GetSelectedReferences();
			if (entryAndPagePairs.Count == 0) return;

			currentDocumentController.EditCitation(field, entryAndPagePairs);
    	}

    	public void DoInsertBibliography()
    	{
			if (!CurrentDocumentControllerIsReady) return;

			if (currentDocumentController.ContainsBibliography)
			{
				if (MessageBox.Show("The document already contains a Bibliography - are you sure you want to insert another?",
				                    "Bibliography already exists",
				                    MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}

			currentDocumentController.InsertBibliography();
			OnWindowSelectionChange();
    	}

    	public void DoRefresh()
    	{
			if (!CurrentDocumentControllerIsReady) return;

			var currentDatabase = currentDocumentController.GetDatabase();
			if (currentDatabase == null)
			{
				ShowNoDatabaseMessage();
				return;
			}

    		var citationsUpdated = currentDocumentController.Refresh(true);

    		if (citationsUpdated > 0)
    		{
    			MessageBox.Show(citationsUpdated == 1
    			                	? "One citation has been updated to match its entry in the Bibtex database."
    			                	: citationsUpdated + " citations have been updated to match their entries in the Bibtex database."
    			                , "Docear4Word");
    		}
    	}

		public void DoMovePrevious()
		{
			currentDocumentController.MovePrevious();
		}

		public void DoMoveNext()
		{
			currentDocumentController.MoveNext();
		}

    	public void DoShowAboutDialog()
    	{
    		using (var aboutForm = new AboutForm())
    		{
    			aboutForm.ShowDialog();
    		}
    	}

		public void DoShowSettingsDialog()
		{
			using (var settingsDialog = new SettingsForm
			                            {
				                            UseDocearDefaultDatabase = Settings.Instance.UseDocearDefaultDatabase,
				                            CustomDatabaseFilename = Settings.Instance.CustomDatabaseFilename,
				                            RefreshUpdatesCitationsFromDatabase = Settings.Instance.RefreshUpdatesCitationsFromDatabase
			                            })
			{

				if (settingsDialog.ShowDialog() == DialogResult.OK)
				{
					Settings.Instance.UseDocearDefaultDatabase = settingsDialog.UseDocearDefaultDatabase;
					Settings.Instance.CustomDatabaseFilename = settingsDialog.CustomDatabaseFilename;
					Settings.Instance.RefreshUpdatesCitationsFromDatabase = settingsDialog.RefreshUpdatesCitationsFromDatabase;
				}
			}
		}

		/// <summary>
		/// Called when user changes style from dropdown list.
		/// </summary>
		/// <param name="newStyle"></param>
		public void TryChangeStyle(StyleInfo newStyle)
		{
			Debug.Assert(currentDocumentController != null);

			if (newStyle == null)
			{
				Process.Start(StyleHelper.FindMoreCitationStylesUrl);

				addinModule.SetSelectedStyle(currentDocumentController.Style);

				return;
			}

			if (IsValidStyle(newStyle))
			{
				currentDocumentController.Style = newStyle;

				DefaultStyle = newStyle;
			}

			addinModule.SetSelectedStyle(currentDocumentController.Style);

			using (var selectionManager = new SelectionManager(currentDocumentController))
			{
				addinModule.UpdateState(selectionManager);
			}
		}

		public void OnDocumentOpen()
		{
			Debug.WriteLine("MainController.OnDocumentOpen()");
			UpdateCurrentDocumentController();
			OnWindowSelectionChange();
		}

		/// <summary>
		/// This gets called for all changes, so we can do
		/// all updates here
		/// </summary>
		public void OnDocumentChange()
		{
			Debug.WriteLine("MainController.OnDocumentChange()");
			UpdateCurrentDocumentController();
		}

		public void OnDocumentBeforeSave()
		{
			Debug.WriteLine("MainController.BeforeSave()");
			if (currentDocumentController != null)
			{
				currentDocumentController.UpdateDocumentProperties();
			}
		}

		public void OnDocumentBeforeClose()
		{
			Debug.WriteLine("MainController.BeforeClose()");

			var closingDocument = GetActiveDocument();
			documentControllers.Remove(closingDocument);
		}

		public void OnNewDocument()
		{
			Debug.WriteLine("MainController.OnNewDocument()");
			UpdateCurrentDocumentController();
		}

		public void OnStartup()
		{
			Debug.WriteLine("MainController.OnStartup()");
			UpdateCurrentDocumentController();
		}

		public void OnWindowActive()
		{
			Debug.WriteLine("MainController.OnWindowActive()");
		}

		public void OnWindowSelectionChange()
		{
			if (currentDocumentController == null || currentDocumentController.IsUpdating) return;

			if (currentDocumentController.IsReferencesTabSelected() == false)
			{
				addinModule.NotifySelectionChangedWhilstNotOnReferenceTab();
				return;
			}

			using (var selectionManager = new SelectionManager(currentDocumentController))
			{
				addinModule.UpdateState(selectionManager);
			}
		}

		void UpdateCurrentDocumentController()
		{
			var activeDocument = GetActiveDocument();
			if (activeDocument == null)
			{
				Debug.WriteLine("UpdateCurrentDocumentController(): No active Document.");
				currentDocumentController = null;
				addinModule.UpdateState(null);
				return;
			}

			Debug.WriteLine("WindowCount=" + wordApp.Application.Windows.Count);

			Debug.WriteLine("UpdateCurrentDocumentController(): Updating current");
			currentDocumentController = GetControllerForDocument(activeDocument);
			currentDocumentController.Activate();

			currentDocumentController.Document.Application.ScreenUpdating = false;
			try
			{

				addinModule.SetSelectedStyle(currentDocumentController.Style ?? DefaultStyle);

				using (var selectionManager = new SelectionManager(currentDocumentController))
				{
					addinModule.UpdateState(selectionManager);
				}
			}
			finally
			{
				currentDocumentController.Document.Application.ScreenUpdating = true;
			}
		}

		DocumentController GetControllerForDocument(Document document)
		{
			if (document == null) throw new ArgumentNullException("document");

			DocumentController result;

			if (!documentControllers.TryGetValue(document, out result))
			{
				//Debug.WriteLine("GetControllerForDocument(): No controller found for document - creating new.");
				result = new DocumentController(this, document);

				documentControllers[document] = result;
			}

			//Debug.WriteLine("GetControllerForDocument(): Returning controller for document.");
			return result;
		}

		[DebuggerStepThrough]
		Document GetActiveDocument()
		{
			try
			{
				return wordApp.ActiveDocument;
			}
			catch
			{
				return null;
			}
		}

		bool IsValidStyle(StyleInfo style)
		{
			return styles.Contains(style);
		}

		public StyleInfo FindStyleByTitle(string title)
		{
			if (string.IsNullOrEmpty(title)) return null;

			foreach(var style in styles)
			{
				if (style.Title == title) return style;
			}

			return null;
		}
	}
}