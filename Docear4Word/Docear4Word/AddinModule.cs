using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinExpress.MSO;

namespace Docear4Word
{
	/// <summary>
	///   Add-in Express Add-in Module
	/// </summary>
	[Guid("34F82078-360B-4682-8275-0D9B0B641CBD"), ProgId("Docear4Word.AddinModule")]
	public class AddinModule: ADXAddinModule
	{
		const string ShowInsertTestDataButtonRegistryKey = "ShowInsertTestDataButton";
		const string EditReferenceCaption = "Edit Reference";

		private ADXRibbonBox adxRibbonBox4;

		MainController mainController;
		ADXRibbonButton rbSettings;
		ADXCommandBarButton cbbSettings;
		bool showInsertTestDataButton;
		bool selectionChangedWhilstNotOnReferencesTab;

		public AddinModule()
		{
			Application.EnableVisualStyles();

			InitializeComponent();
			// Please add any initialization code to the AddinInitialize event handler
		}

		private ADXWordAppEvents adxWordEvents;
		private ADXRibbonTab adxRibbonTab;
		private ADXCommandBar adxCommandBar;
		private ADXRibbonGroup adxRibbonGroup1;
		private ADXRibbonButton rbAddReference;
		private ADXRibbonSeparator adxRibbonSeparator1;
		private ADXRibbonComboBox rcbStyle;
		private ImageList ilSmall;
		private ADXRibbonButton rbEditReference;
		private ADXRibbonButton rbInsertBibliography;
		private ADXRibbonBox adxRibbonBox1;
		private ADXRibbonButton rbRefresh;
		private ADXRibbonButton rbAbout;
		private ADXRibbonDialogBoxLauncher adxRibbonDialogBoxLauncher;
		private ADXCommandBarButton cbbAddReference;
		private ADXRibbonButton rbInsertTestData;
		private ADXCommandBarButton cbbEditReference;
		private ADXCommandBarButton cbbRefresh;
		private ADXCommandBarButton cbbInsertBibliography;
		private ADXCommandBarComboBox ccbStyle;
		private ADXCommandBarButton cbbAbout;
		private ADXRibbonItem adxRibbonItem1;
		private ADXRibbonButton rbMovePrevious;
		private ADXRibbonButton rbMoveNext;
		private ADXRibbonBox adxRibbonBox2;
		private ADXRibbonBox adxRibbonBox3;
		private ADXCommandBarButton cbbMovePrevious;
		private ADXCommandBarButton cbbMoveNext;

		#region Component Designer generated code
		/// <summary>
		/// Required by designer
		/// </summary>
		System.ComponentModel.IContainer components;

		/// <summary>
		/// Required by designer support - do not modify
		/// the following method
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinModule));
			this.adxWordEvents = new AddinExpress.MSO.ADXWordAppEvents(this.components);
			this.adxRibbonTab = new AddinExpress.MSO.ADXRibbonTab(this.components);
			this.adxRibbonGroup1 = new AddinExpress.MSO.ADXRibbonGroup(this.components);
			this.rbAddReference = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.ilSmall = new System.Windows.Forms.ImageList(this.components);
			this.rbEditReference = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.rbInsertBibliography = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.rbRefresh = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.adxRibbonSeparator1 = new AddinExpress.MSO.ADXRibbonSeparator(this.components);
			this.adxRibbonBox2 = new AddinExpress.MSO.ADXRibbonBox(this.components);
			this.rcbStyle = new AddinExpress.MSO.ADXRibbonComboBox(this.components);
			this.adxRibbonItem1 = new AddinExpress.MSO.ADXRibbonItem(this.components);
			this.adxRibbonBox1 = new AddinExpress.MSO.ADXRibbonBox(this.components);
			this.adxRibbonBox3 = new AddinExpress.MSO.ADXRibbonBox(this.components);
			this.rbMovePrevious = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.rbMoveNext = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.adxRibbonBox4 = new AddinExpress.MSO.ADXRibbonBox(this.components);
			this.rbAbout = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.rbSettings = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.rbInsertTestData = new AddinExpress.MSO.ADXRibbonButton(this.components);
			this.adxRibbonDialogBoxLauncher = new AddinExpress.MSO.ADXRibbonDialogBoxLauncher(this.components);
			this.adxCommandBar = new AddinExpress.MSO.ADXCommandBar(this.components);
			this.cbbAddReference = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbEditReference = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbInsertBibliography = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.ccbStyle = new AddinExpress.MSO.ADXCommandBarComboBox(this.components);
			this.cbbRefresh = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbMovePrevious = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbMoveNext = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbAbout = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			this.cbbSettings = new AddinExpress.MSO.ADXCommandBarButton(this.components);
			// 
			// adxWordEvents
			// 
			this.adxWordEvents.DocumentBeforeClose += new AddinExpress.MSO.ADXHostBeforeAction_EventHandler(this.adxWordEvents_DocumentBeforeClose);
			this.adxWordEvents.DocumentBeforeSave += new AddinExpress.MSO.ADXHostBeforeSave_EventHandler(this.adxWordEvents_DocumentBeforeSave);
			this.adxWordEvents.DocumentChange += new System.EventHandler(this.adxWordEvents_DocumentChange);
			this.adxWordEvents.DocumentOpen += new AddinExpress.MSO.ADXHostActiveObject_EventHandler(this.adxWordEvents_DocumentOpen);
			this.adxWordEvents.NewDocument += new AddinExpress.MSO.ADXHostActiveObject_EventHandler(this.adxWordEvents_NewDocument);
			this.adxWordEvents.Startup += new System.EventHandler(this.adxWordEvents_Startup);
			this.adxWordEvents.WindowActivate += new AddinExpress.MSO.ADXHostWindow_EventHandler(this.adxWordEvents_WindowActivate);
			this.adxWordEvents.WindowSelectionChange += new AddinExpress.MSO.ADXHostActiveObject_EventHandler(this.adxWordEvents_WindowSelectionChange);
			// 
			// adxRibbonTab
			// 
			this.adxRibbonTab.Caption = "Docear4Word";
			this.adxRibbonTab.Controls.Add(this.adxRibbonGroup1);
			this.adxRibbonTab.Id = "adxRibbonTab_9ec41076f19448008200b738fd45d29a";
			this.adxRibbonTab.IdMso = "TabReferences";
			this.adxRibbonTab.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.adxRibbonTab.PropertyChanging += new AddinExpress.MSO.ADXRibbonPropertyChanging_EventHandler(this.adxRibbonTab_PropertyChanging);
			// 
			// adxRibbonGroup1
			// 
			this.adxRibbonGroup1.AutoScale = true;
			this.adxRibbonGroup1.Caption = "Docear4Word";
			this.adxRibbonGroup1.Controls.Add(this.rbAddReference);
			this.adxRibbonGroup1.Controls.Add(this.rbEditReference);
			this.adxRibbonGroup1.Controls.Add(this.rbInsertBibliography);
			this.adxRibbonGroup1.Controls.Add(this.rbRefresh);
			this.adxRibbonGroup1.Controls.Add(this.adxRibbonSeparator1);
			this.adxRibbonGroup1.Controls.Add(this.adxRibbonBox2);
			this.adxRibbonGroup1.Controls.Add(this.adxRibbonBox1);
			this.adxRibbonGroup1.Controls.Add(this.adxRibbonBox4);
			this.adxRibbonGroup1.Controls.Add(this.adxRibbonDialogBoxLauncher);
			this.adxRibbonGroup1.Id = "adxRibbonGroup_08905b6658624f098244bcdcf154a984";
			this.adxRibbonGroup1.Image = 1;
			this.adxRibbonGroup1.ImageList = this.ilSmall;
			this.adxRibbonGroup1.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.adxRibbonGroup1.InsertBeforeIdMso = "GroupCitationsAndBibliography";
			this.adxRibbonGroup1.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// rbAddReference
			// 
			this.rbAddReference.Caption = "Add Reference";
			this.rbAddReference.Id = "adxRibbonButton_a8af3773e4cb4c54a273540f3b5afb09";
			this.rbAddReference.Image = 1;
			this.rbAddReference.ImageList = this.ilSmall;
			this.rbAddReference.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbAddReference.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbAddReference.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbAddReference_OnClick);
			// 
			// ilSmall
			// 
			this.ilSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSmall.ImageStream")));
			this.ilSmall.TransparentColor = System.Drawing.Color.Transparent;
			this.ilSmall.Images.SetKeyName(0, "AboutSmall.png");
			this.ilSmall.Images.SetKeyName(1, "AddReferenceSmall.png");
			this.ilSmall.Images.SetKeyName(2, "BibTexSettingsSmall.png");
			this.ilSmall.Images.SetKeyName(3, "CreateBibliographySmall.png");
			this.ilSmall.Images.SetKeyName(4, "EditReferenceSmall.png");
			this.ilSmall.Images.SetKeyName(5, "RefreshSmall.png");
			this.ilSmall.Images.SetKeyName(6, "StatusBarSmall.png");
			this.ilSmall.Images.SetKeyName(7, "MoveNextSmall.png");
			this.ilSmall.Images.SetKeyName(8, "MovePreviousSmall.png");
			// 
			// rbEditReference
			// 
			this.rbEditReference.Caption = "Edit Reference";
			this.rbEditReference.Id = "adxRibbonButton_9563a97bce9c40ce9f235691f6b22d1b";
			this.rbEditReference.Image = 4;
			this.rbEditReference.ImageList = this.ilSmall;
			this.rbEditReference.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbEditReference.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbEditReference.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbEditReference_OnClick);
			// 
			// rbInsertBibliography
			// 
			this.rbInsertBibliography.Caption = "Insert Bibliography";
			this.rbInsertBibliography.Id = "adxRibbonButton_2c46efe1f0df41e39b3a992e02822a36";
			this.rbInsertBibliography.Image = 3;
			this.rbInsertBibliography.ImageList = this.ilSmall;
			this.rbInsertBibliography.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbInsertBibliography.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbInsertBibliography.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbInsertBibliography_OnClick);
			// 
			// rbRefresh
			// 
			this.rbRefresh.Caption = "Refresh";
			this.rbRefresh.Id = "adxRibbonButton_65882f54a6c3486aacf5244813c76ed2";
			this.rbRefresh.Image = 5;
			this.rbRefresh.ImageList = this.ilSmall;
			this.rbRefresh.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbRefresh.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbRefresh.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbRefresh_OnClick);
			// 
			// adxRibbonSeparator1
			// 
			this.adxRibbonSeparator1.Id = "adxRibbonSeparator_890311211ebe4a47850fc49bc39a522f";
			this.adxRibbonSeparator1.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// adxRibbonBox2
			// 
			this.adxRibbonBox2.Controls.Add(this.rcbStyle);
			this.adxRibbonBox2.Id = "adxRibbonBox_e9202bec5e3b4df5af2a32fa98b416e0";
			this.adxRibbonBox2.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// rcbStyle
			// 
			this.rcbStyle.Caption = "Style:";
			this.rcbStyle.Id = "adxRibbonComboBox_6323cfef61ea48eab211e5e209e089ba";
			this.rcbStyle.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rcbStyle.Items.Add(this.adxRibbonItem1);
			this.rcbStyle.MaxLength = 200;
			this.rcbStyle.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rcbStyle.SizeString = "Modern Language Association";
			this.rcbStyle.OnChange += new AddinExpress.MSO.ADXRibbonOnActionChange_EventHandler(this.rcbStyle_OnChange);
			// 
			// adxRibbonItem1
			// 
			this.adxRibbonItem1.Caption = "adxRibbonItem1";
			this.adxRibbonItem1.Id = "adxRibbonItem_591cb68dcd4d4060bbff94138af0dcb2";
			this.adxRibbonItem1.ImageTransparentColor = System.Drawing.Color.Transparent;
			// 
			// adxRibbonBox1
			// 
			this.adxRibbonBox1.Controls.Add(this.adxRibbonBox3);
			this.adxRibbonBox1.Id = "adxRibbonBox_d00382a109564b4392aa2e4618dffa50";
			this.adxRibbonBox1.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// adxRibbonBox3
			// 
			this.adxRibbonBox3.Controls.Add(this.rbMovePrevious);
			this.adxRibbonBox3.Controls.Add(this.rbMoveNext);
			this.adxRibbonBox3.Id = "adxRibbonBox_b30ee4f3fd29467f9988ec4d08abce85";
			this.adxRibbonBox3.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// rbMovePrevious
			// 
			this.rbMovePrevious.Caption = "Previous";
			this.rbMovePrevious.Description = "Moves to the Previous citation/bibliography field";
			this.rbMovePrevious.Id = "adxRibbonButton_0fad5de143b54bf387b9685df35a73c2";
			this.rbMovePrevious.Image = 8;
			this.rbMovePrevious.ImageList = this.ilSmall;
			this.rbMovePrevious.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbMovePrevious.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbMovePrevious.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbMovePrevious_OnClick);
			// 
			// rbMoveNext
			// 
			this.rbMoveNext.Caption = "Next";
			this.rbMoveNext.Description = "Moves to the next citation/bibliography field";
			this.rbMoveNext.Id = "adxRibbonButton_93ed45fa27cd493fb52e5c1e3c647bcd";
			this.rbMoveNext.Image = 7;
			this.rbMoveNext.ImageList = this.ilSmall;
			this.rbMoveNext.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbMoveNext.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbMoveNext.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbMoveNext_OnClick);
			// 
			// adxRibbonBox4
			// 
			this.adxRibbonBox4.Controls.Add(this.rbAbout);
			this.adxRibbonBox4.Controls.Add(this.rbSettings);
			this.adxRibbonBox4.Controls.Add(this.rbInsertTestData);
			this.adxRibbonBox4.Id = "adxRibbonBox_d5e0c551562846629972089581f7ee4e";
			this.adxRibbonBox4.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			// 
			// rbAbout
			// 
			this.rbAbout.Caption = "About";
			this.rbAbout.Id = "adxRibbonButton_4013b15ca46947cebc49e9f35a09ccb6";
			this.rbAbout.Image = 0;
			this.rbAbout.ImageList = this.ilSmall;
			this.rbAbout.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbAbout.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbAbout.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbAbout_OnClick);
			// 
			// rbSettings
			// 
			this.rbSettings.Caption = "Settings";
			this.rbSettings.Id = "adxRibbonButton_25f422a4d23f471aa7ef1860f7fa7e81";
			this.rbSettings.Image = 2;
			this.rbSettings.ImageList = this.ilSmall;
			this.rbSettings.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbSettings.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbSettings.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbSettings_OnClick);
			// 
			// rbInsertTestData
			// 
			this.rbInsertTestData.Caption = "(Insert Test Data)";
			this.rbInsertTestData.Id = "adxRibbonButton_649532e9e90e4e9795850ad027df9c51";
			this.rbInsertTestData.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.rbInsertTestData.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.rbInsertTestData.Visible = false;
			this.rbInsertTestData.OnClick += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.rbInsertTestData_OnClick);
			// 
			// adxRibbonDialogBoxLauncher
			// 
			this.adxRibbonDialogBoxLauncher.Id = "adxRibbonDialogBoxLauncher_9a779bc67f4d4025bfa3f1977db513d2";
			this.adxRibbonDialogBoxLauncher.Ribbons = AddinExpress.MSO.ADXRibbons.msrWordDocument;
			this.adxRibbonDialogBoxLauncher.Visible = false;
			this.adxRibbonDialogBoxLauncher.OnAction += new AddinExpress.MSO.ADXRibbonOnAction_EventHandler(this.adxRibbonDialogBoxLauncher_OnAction);
			// 
			// adxCommandBar
			// 
			this.adxCommandBar.CommandBarName = "Docear4Word";
			this.adxCommandBar.CommandBarTag = "fc1ef6a0-c023-42f5-be32-b555b58cfcf3";
			this.adxCommandBar.Controls.Add(this.cbbAddReference);
			this.adxCommandBar.Controls.Add(this.cbbEditReference);
			this.adxCommandBar.Controls.Add(this.cbbInsertBibliography);
			this.adxCommandBar.Controls.Add(this.ccbStyle);
			this.adxCommandBar.Controls.Add(this.cbbRefresh);
			this.adxCommandBar.Controls.Add(this.cbbMovePrevious);
			this.adxCommandBar.Controls.Add(this.cbbMoveNext);
			this.adxCommandBar.Controls.Add(this.cbbAbout);
			this.adxCommandBar.Controls.Add(this.cbbSettings);
			this.adxCommandBar.SupportedApps = ((AddinExpress.MSO.ADXOfficeHostApp)((AddinExpress.MSO.ADXOfficeHostApp.ohaWord | AddinExpress.MSO.ADXOfficeHostApp.ohaInfoPath)));
			this.adxCommandBar.UpdateCounter = 13;
			// 
			// cbbAddReference
			// 
			this.cbbAddReference.Caption = "Add Reference";
			this.cbbAddReference.ControlTag = "be038f4a-3757-433e-90e9-dc8db2325510";
			this.cbbAddReference.Image = 1;
			this.cbbAddReference.ImageList = this.ilSmall;
			this.cbbAddReference.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbAddReference.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbAddReference.UpdateCounter = 9;
			this.cbbAddReference.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbAddReference_Click);
			// 
			// cbbEditReference
			// 
			this.cbbEditReference.Caption = "Edit Reference";
			this.cbbEditReference.ControlTag = "adec0a14-05a3-4d0f-9fd2-a4e2cd071cdc";
			this.cbbEditReference.Image = 4;
			this.cbbEditReference.ImageList = this.ilSmall;
			this.cbbEditReference.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbEditReference.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbEditReference.UpdateCounter = 13;
			this.cbbEditReference.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbEditReference_Click);
			// 
			// cbbInsertBibliography
			// 
			this.cbbInsertBibliography.Caption = "Insert Bibliography";
			this.cbbInsertBibliography.ControlTag = "7e4b0b15-442e-45a0-b692-f7d6f503fcc4";
			this.cbbInsertBibliography.Image = 3;
			this.cbbInsertBibliography.ImageList = this.ilSmall;
			this.cbbInsertBibliography.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbInsertBibliography.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbInsertBibliography.UpdateCounter = 20;
			this.cbbInsertBibliography.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbInsertBibliography_Click);
			// 
			// ccbStyle
			// 
			this.ccbStyle.Caption = "Style Chooser";
			this.ccbStyle.ControlTag = "39771c5a-5e82-4c23-8911-08fd20c28dd4";
			this.ccbStyle.DropDownWidth = 300;
			this.ccbStyle.UpdateCounter = 11;
			this.ccbStyle.Width = 300;
			this.ccbStyle.Change += new AddinExpress.MSO.ADXChange_EventHandler(this.ccbStyle_Change);
			// 
			// cbbRefresh
			// 
			this.cbbRefresh.Caption = "Refresh";
			this.cbbRefresh.ControlTag = "68458201-08f9-4bd9-8658-0592fe87c846";
			this.cbbRefresh.Image = 5;
			this.cbbRefresh.ImageList = this.ilSmall;
			this.cbbRefresh.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbRefresh.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbRefresh.UpdateCounter = 15;
			this.cbbRefresh.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbRefresh_Click);
			// 
			// cbbMovePrevious
			// 
			this.cbbMovePrevious.Caption = "Previous";
			this.cbbMovePrevious.ControlTag = "5fa544de-041a-4e1f-b3af-0b5c7aa65b05";
			this.cbbMovePrevious.Image = 8;
			this.cbbMovePrevious.ImageList = this.ilSmall;
			this.cbbMovePrevious.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbMovePrevious.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbMovePrevious.UpdateCounter = 8;
			this.cbbMovePrevious.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbPrevious_Click);
			// 
			// cbbMoveNext
			// 
			this.cbbMoveNext.Caption = "Next";
			this.cbbMoveNext.ControlTag = "15171e68-fc45-440c-8f82-ea7fcee862f3";
			this.cbbMoveNext.Image = 7;
			this.cbbMoveNext.ImageList = this.ilSmall;
			this.cbbMoveNext.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbMoveNext.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbMoveNext.UpdateCounter = 8;
			this.cbbMoveNext.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbNext_Click);
			// 
			// cbbAbout
			// 
			this.cbbAbout.BeginGroup = true;
			this.cbbAbout.Caption = "About";
			this.cbbAbout.ControlTag = "e8b7bcaf-c97d-4d56-adc6-8d917f1d9d44";
			this.cbbAbout.Image = 0;
			this.cbbAbout.ImageList = this.ilSmall;
			this.cbbAbout.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbAbout.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbAbout.UpdateCounter = 13;
			this.cbbAbout.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbAbout_Click);
			// 
			// cbbSettings
			// 
			this.cbbSettings.Caption = "Settings";
			this.cbbSettings.ControlTag = "3cdae496-5e01-4f18-9bd3-ac3b89f8f0f9";
			this.cbbSettings.Image = 2;
			this.cbbSettings.ImageList = this.ilSmall;
			this.cbbSettings.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.cbbSettings.Style = AddinExpress.MSO.ADXMsoButtonStyle.adxMsoButtonIcon;
			this.cbbSettings.UpdateCounter = 7;
			this.cbbSettings.Click += new AddinExpress.MSO.ADXClick_EventHandler(this.cbbSettings_Click);
			// 
			// AddinModule
			// 
			this.AddinName = "Docear4Word";
			this.Description = "Inserts formatted references and reference lists from BibTeX/CSL files";
			this.RegisterForAllUsers = true;
			this.SupportedApps = AddinExpress.MSO.ADXOfficeHostApp.ohaWord;
			this.AddinStartupComplete += new AddinExpress.MSO.ADXEvents_EventHandler(this.AddinModule_AddinStartupComplete);
		}
		#endregion

		#region Add-in Express automatic code
		// Required by Add-in Express - do not modify
		// the methods within this region

		public override System.ComponentModel.IContainer GetContainer()
		{
			if (components == null)
				components = new System.ComponentModel.Container();
			return components;
		}

		[ComRegisterFunction]
		public static void AddinRegister(Type t)
		{
			ADXRegister(t);
		}

		[ComUnregisterFunction]
		public static void AddinUnregister(Type t)
		{
			ADXUnregister(t);
		}

// ReSharper disable RedundantOverridenMember
		public override void UninstallControls()
		{
			base.UninstallControls();
		}

// ReSharper restore RedundantOverridenMember
		#endregion

		public new static AddinModule CurrentInstance
		{
			get { return ADXAddinModule.CurrentInstance as AddinModule; }
		}

		public Word._Application WordApp
		{
			get { return (HostApplication as Word._Application); }
		}

		#region Ribbon Events
		void rbAddReference_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoAddReferences();
		}

		void rbEditReference_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoEditReference();
		}

		void rbInsertBibliography_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoInsertBibliography();
		}

		void rbRefresh_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoRefresh();
		}

		void rcbStyle_OnChange(object sender, IRibbonControl control, string text)
		{
			DoChangeStyle(text);
		}

		void rbMoveNext_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoMoveNext();
		}

		void rbMovePrevious_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoMovePrevious();
		}

		void adxRibbonDialogBoxLauncher_OnAction(object sender, IRibbonControl control, bool pressed)
		{
			DoShowAboutDialog();
		}

		void rbAbout_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoShowAboutDialog();
		}

		void rbSettings_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoShowSettingsDialog();
		}
		#endregion Ribbon Events

		#region Toolbar Events
		void cbbAddReference_Click(object sender)
		{
			DoAddReferences();
		}

		void cbbEditReference_Click(object sender)
		{
			DoEditReference();
		}

		void cbbInsertBibliography_Click(object sender)
		{
			DoInsertBibliography();
		}

		void cbbRefresh_Click(object sender)
		{
			DoRefresh();
		}

		void ccbStyle_Change(object sender)
		{
			DoChangeStyle(ccbStyle.Text);
		}

		void cbbAbout_Click(object sender)
		{
			DoShowAboutDialog();
		}

		void cbbPrevious_Click(object sender)
		{
			DoMovePrevious();
		}

		void cbbNext_Click(object sender)
		{
			DoMoveNext();
		}

		private void cbbSettings_Click(object sender)
		{
			DoShowSettingsDialog();
		}
		#endregion Toolbar Events

		#region Word Events
		void adxWordEvents_DocumentOpen(object sender, object hostObj)
		{
			try
			{
				mainController.OnDocumentOpen();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_DocumentChange(object sender, EventArgs e)
		{
			try
			{
				mainController.OnDocumentChange();
			}
			catch(Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_DocumentBeforeSave(object sender, ADXHostBeforeSaveEventArgs e)
		{
			try
			{
				mainController.OnDocumentBeforeSave();
			}
			catch(Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_DocumentBeforeClose(object sender, ADXHostBeforeActionEventArgs e)
		{
			try
			{
				mainController.OnDocumentBeforeClose();
			}
			catch(Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_NewDocument(object sender, object hostObj)
		{
			try
			{
				mainController.OnNewDocument();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_Startup(object sender, EventArgs e)
		{
			try
			{
				mainController.OnStartup();

				try
				{
					var registryValue = RegistryHelper.ReadApplicationString(ShowInsertTestDataButtonRegistryKey, string.Empty).ToLowerInvariant();
					showInsertTestDataButton = registryValue == "true" || registryValue == "1" || registryValue == "yes" || registryValue == "on";
				}
				catch
				{}
			}
			catch(Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_WindowActivate(object sender, object hostObj, object window)
		{
			try
			{
				mainController.OnWindowActive();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void adxWordEvents_WindowSelectionChange(object sender, object hostObj)
		{
			try
			{
				mainController.OnWindowSelectionChange();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void AddinModule_AddinStartupComplete(object sender, EventArgs e)
		{
			try
			{
				mainController = new MainController(this);

				rbRefresh.PropertyChanging += OnRefreshPropertyChanging;
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void OnRefreshPropertyChanging(object sender, ADXRibbonPropertyChangingEventArgs e)
		{
			if (e.PropertyType != ADXRibbonControlPropertyType.Visible) return;

			if (selectionChangedWhilstNotOnReferencesTab)
			{
				if (mainController.CurrentDocumentController != null)
				{
					selectionChangedWhilstNotOnReferencesTab = false;

					try
					{
						using (var selectionManager = new SelectionManager(mainController.CurrentDocumentController))
						{
							UpdateState(selectionManager);
						}
					}
					catch(Exception ex)
					{
						Debug.WriteLine("Attempting to update via RefreshButton but an exception occurred: " + ex);
					}
				}
				else
				{
					Debug.WriteLine("Attempting to update via RefreshButton but CurrentDocumentController is null so doing nothing");
				}
			}
			else
			{
				Debug.WriteLine("Refresh button is updating but selectionChangedWhilstNotOnReferencesTab=false so doing nothing");
			}
		}

		void adxRibbonTab_PropertyChanging(object sender, ADXRibbonPropertyChangingEventArgs e)
		{
			Debug.WriteLine(e.PropertyType);
		}
		#endregion Word Events

		#region Handlers
		void DoAddReferences()
		{
			try
			{
				mainController.DoAddReference();

				SetFocusToActiveDocument();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoEditReference()
		{
			try
			{
				mainController.DoEditReference();

				SetFocusToActiveDocument();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoInsertBibliography()
		{
			try
			{
				mainController.DoInsertBibliography();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoRefresh()
		{
			try
			{
				mainController.DoRefresh();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoShowSettingsDialog()
		{
			try
			{
				mainController.DoShowSettingsDialog();

				SetFocusToActiveDocument();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoShowAboutDialog()
		{
			try
			{
				mainController.DoShowAboutDialog();

				SetFocusToActiveDocument();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void SetFocusToActiveDocument()
		{
			try
			{
				if (WordApp.Documents.Count == 0) return;

				WordApp.ActiveDocument.ActiveWindow.SetFocus();
			}
			catch
			{}
		}

		void DoChangeStyle(string styleName)
		{
			try
			{
				var style = mainController.FindStyleByTitle(styleName);

				mainController.TryChangeStyle(style);
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoInsertTestData()
		{
			try
			{
				mainController.DoInsertTestData();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoMovePrevious()
		{
			try
			{
				mainController.DoMovePrevious();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void DoMoveNext()
		{
			try
			{
				mainController.DoMoveNext();
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		void rbInsertTestData_OnClick(object sender, IRibbonControl control, bool pressed)
		{
			DoInsertTestData();
		}
		#endregion Handlers

		void LogException(Exception ex)
		{
			try
			{
				var wordAppVersion = "Unknown";
				try
				{
					wordAppVersion = WordApp.Version;
				}
				catch
				{}

				var logText = string.Format("{0} [{2} / Word {3}]\r\n{1}\r\n\r\n", DateTime.UtcNow, ex, OSVersionInfo.FullVersionString, wordAppVersion);

				File.AppendAllText(FolderHelper.DocearErrorLogFilename, logText);
			}
			catch
			{}
		}

		public void UpdateRibbonStyles(IEnumerable<StyleInfo> styles)
		{
			rcbStyle.Items.Clear();
			ccbStyle.Items.Clear();

			rcbStyle.Items.Add(CreateRibbonItemForStyle(StyleHelper.FindMoreCitationStyles));
			ccbStyle.Items.Add(StyleHelper.FindMoreCitationStyles.Title);

			foreach (var style in styles)
			{
				rcbStyle.Items.Add(CreateRibbonItemForStyle(style));
				ccbStyle.Items.Add(style.Title);
			}
		}

		ADXRibbonItem CreateRibbonItemForStyle(StyleInfo style)
		{
			return new ADXRibbonItem(components)
			       	{
			       		Caption = style.Title,
			       		ScreenTip = style.Title,
			       		SuperTip = style.Summary,
			       	};
		}

		public void SetSelectedStyle(StyleInfo style)
		{
			if (style == null)
			{
				rcbStyle.Text = StyleHelper.FindMoreCitationStyles.Title;
				ccbStyle.Text = StyleHelper.FindMoreCitationStyles.Title;

				return;
			}

			rcbStyle.Text = style.Title;
			ccbStyle.Text = style.Title;
		}

		public void UpdateState(SelectionManager selectionManager)
		{
			selectionChangedWhilstNotOnReferencesTab = false;

			var hasSelectionManager = selectionManager != null;
			var currentDocumentControllerIsReady = mainController.CurrentDocumentControllerIsReady;

			var fullyEnabled = hasSelectionManager && currentDocumentControllerIsReady;
			var partlyEnabled = hasSelectionManager;
			var noFieldsSelected = !hasSelectionManager || selectionManager.IsNoFieldsSelected;

			var canAdd = fullyEnabled && noFieldsSelected;
			var canEdit = fullyEnabled && selectionManager.IsAtOrBeforeSingleCitation;
			var canInsertBibliography = fullyEnabled && noFieldsSelected && !selectionManager.IsAtOrBeforeSingleCitation && !selectionManager.IsInBibliography;
			//var canInsertBibliography = fullyEnabled && noFieldsSelected && !selectionManager.IsAtOrBeforeSingleCitation;

			// The properties don't seem to perform a NOP when assigning the existing value
			// therefore we have to check ourselves! Unbelievable but true!
			// Setting the properties takes no measureable time (tested by Stopwatch) but Word
			// must be using the new values outside this call as moving the cursor by keyboard
			// becomes noticable slowly if we don't do this.
			// ReSharper disable RedundantCheckBeforeAssignment
			if (rbAddReference.Visible != !canEdit) rbAddReference.Visible = !canEdit;
			if (rbAddReference.Enabled != canAdd) rbAddReference.Enabled = canAdd;
			if (rbEditReference.Visible != canEdit) rbEditReference.Visible = canEdit;
			if (rbRefresh.Enabled != fullyEnabled) rbRefresh.Enabled = fullyEnabled;
			if (rbInsertBibliography.Enabled != canInsertBibliography) rbInsertBibliography.Enabled = canInsertBibliography;
			if (rcbStyle.Enabled != partlyEnabled) rcbStyle.Enabled = partlyEnabled;
			if (rbMovePrevious.Enabled != partlyEnabled) rbMovePrevious.Enabled = partlyEnabled;
			if (rbMoveNext.Enabled != partlyEnabled) rbMoveNext.Enabled = partlyEnabled;

			if (rbInsertTestData.Visible != showInsertTestDataButton) rbInsertTestData.Visible = showInsertTestDataButton;
			if (rbInsertTestData.Enabled != fullyEnabled) rbInsertTestData.Enabled = fullyEnabled;

			if (cbbAddReference.Visible != !canEdit) cbbAddReference.Visible = !canEdit;
			if (cbbAddReference.Enabled != canAdd) cbbAddReference.Enabled = canAdd;
			if (cbbEditReference.Visible != canEdit) cbbEditReference.Visible = canEdit;
			if (cbbRefresh.Enabled != fullyEnabled) cbbRefresh.Enabled = fullyEnabled;
			if (cbbInsertBibliography.Enabled != canInsertBibliography) cbbInsertBibliography.Enabled = canInsertBibliography;
			if (ccbStyle.Enabled != partlyEnabled) ccbStyle.Enabled = partlyEnabled;
			if (cbbMovePrevious.Enabled != partlyEnabled) cbbMovePrevious.Enabled = partlyEnabled;
			if (cbbMoveNext.Enabled != partlyEnabled) cbbMoveNext.Enabled = partlyEnabled;
			// ReSharper restore RedundantCheckBeforeAssignment
		}

		public bool IsEditReference()
		{
			return rbAddReference.Caption == EditReferenceCaption;
		}

		public void NotifySelectionChangedWhilstNotOnReferenceTab()
		{
			selectionChangedWhilstNotOnReferencesTab = true;

			rbRefresh.Invalidate();
		}
	}
}
