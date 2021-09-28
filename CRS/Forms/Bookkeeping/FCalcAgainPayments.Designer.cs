namespace CRS.Forms.Bookkeeping
{
    partial class FCalcAgainPayments
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
            ManiXButton.Office2010Blue office2010Blue1 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCalcAgainPayments));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.SearchBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.DockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.SearchDockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.YearComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FromDateValue = new DevExpress.XtraEditors.DateEdit();
            this.YearLabel = new DevExpress.XtraEditors.LabelControl();
            this.ToDateValue = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.PaymentsGridControl = new DevExpress.XtraGrid.GridControl();
            this.PaymentsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Payment_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Payment_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Payment_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MonthComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Payment_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Payment_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).BeginInit();
            this.SearchDockPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YearComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(32, 446);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(504, 50);
            this.PanelOption.TabIndex = 50;
            // 
            // BOK
            // 
            this.BOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Blue1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Blue1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Blue1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010Blue1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010Blue1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Blue1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Blue1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue1.TextColor = System.Drawing.Color.White;
            this.BOK.ColorTable = office2010Blue1;
            this.BOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BOK.Location = new System.Drawing.Point(313, 13);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(96, 25);
            this.BOK.TabIndex = 4;
            this.BOK.Text = "Yenidən hesabla";
            this.BOK.Theme = ManiXButton.Theme.MSOffice2010_BLUE;
            this.BOK.UseVisualStyleBackColor = true;
            this.BOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // BCancel
            // 
            this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Red1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Red1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Red1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Red1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Red1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Red1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Red1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red1.TextColor = System.Drawing.Color.White;
            this.BCancel.ColorTable = office2010Red1;
            this.BCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(415, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(32, 496);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.DockControls.Add(this.StandaloneBarDockControl);
            this.BarManager.DockManager = this.DockManager;
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton,
            this.SearchBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 4;
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "Main menu";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton)});
            this.ToolBar.OptionsBar.DrawBorder = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
            this.ToolBar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.ToolBar.Text = "Main menu";
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 0;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.LargeImage")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 1;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.LargeImage")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarButton
            // 
            this.ExportBarButton.Caption = "İxrac et";
            this.ExportBarButton.Id = 2;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // SearchBarButton
            // 
            this.SearchBarButton.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.SearchBarButton.Caption = "Ətraflı axtar";
            this.SearchBarButton.Down = true;
            this.SearchBarButton.Id = 3;
            this.SearchBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.Image")));
            this.SearchBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.LargeImage")));
            this.SearchBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.F));
            this.SearchBarButton.Name = "SearchBarButton";
            this.SearchBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.SearchBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SearchBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(536, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 496);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(536, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 496);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(536, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 496);
            // 
            // DockManager
            // 
            this.DockManager.Form = this;
            this.DockManager.MenuManager = this.BarManager;
            this.DockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.SearchDockPanel});
            this.DockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
            // 
            // SearchDockPanel
            // 
            this.SearchDockPanel.Controls.Add(this.dockPanel1_Container);
            this.SearchDockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
            this.SearchDockPanel.ID = new System.Guid("afaa5c61-3977-497c-91bc-9485e4927c70");
            this.SearchDockPanel.Location = new System.Drawing.Point(32, 0);
            this.SearchDockPanel.Name = "SearchDockPanel";
            this.SearchDockPanel.OriginalSize = new System.Drawing.Size(200, 70);
            this.SearchDockPanel.Size = new System.Drawing.Size(504, 70);
            this.SearchDockPanel.Text = "Ətraflı axtar";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.MonthComboBox);
            this.dockPanel1_Container.Controls.Add(this.labelControl1);
            this.dockPanel1_Container.Controls.Add(this.YearComboBox);
            this.dockPanel1_Container.Controls.Add(this.FromDateValue);
            this.dockPanel1_Container.Controls.Add(this.YearLabel);
            this.dockPanel1_Container.Controls.Add(this.ToDateValue);
            this.dockPanel1_Container.Controls.Add(this.labelControl2);
            this.dockPanel1_Container.Controls.Add(this.DateLabel);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(496, 42);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // YearComboBox
            // 
            this.YearComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.YearComboBox.Location = new System.Drawing.Point(21, 11);
            this.YearComboBox.Name = "YearComboBox";
            this.YearComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç")});
            this.YearComboBox.Properties.NullValuePrompt = "Seçin";
            this.YearComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.YearComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.YearComboBox.Size = new System.Drawing.Size(56, 20);
            this.YearComboBox.TabIndex = 235;
            this.YearComboBox.SelectedIndexChanged += new System.EventHandler(this.YearComboBox_SelectedIndexChanged);
            // 
            // FromDateValue
            // 
            this.FromDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FromDateValue.EditValue = null;
            this.FromDateValue.Location = new System.Drawing.Point(296, 11);
            this.FromDateValue.Name = "FromDateValue";
            this.FromDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.FromDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.FromDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.FromDateValue.Size = new System.Drawing.Size(85, 20);
            this.FromDateValue.TabIndex = 236;
            this.FromDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // YearLabel
            // 
            this.YearLabel.Location = new System.Drawing.Point(9, 14);
            this.YearLabel.Name = "YearLabel";
            this.YearLabel.Size = new System.Drawing.Size(6, 13);
            this.YearLabel.TabIndex = 239;
            this.YearLabel.Text = "İl";
            // 
            // ToDateValue
            // 
            this.ToDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ToDateValue.EditValue = null;
            this.ToDateValue.Location = new System.Drawing.Point(399, 11);
            this.ToDateValue.Name = "ToDateValue";
            this.ToDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.ToDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.ToDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.ToDateValue.Size = new System.Drawing.Size(85, 20);
            this.ToDateValue.TabIndex = 237;
            this.ToDateValue.EditValueChanged += new System.EventHandler(this.ToDateValue_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(387, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(4, 13);
            this.labelControl2.TabIndex = 240;
            this.labelControl2.Text = "-";
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(225, 14);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(65, 13);
            this.DateLabel.TabIndex = 238;
            this.DateLabel.Text = "Tarix intervalı";
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // PaymentsGridControl
            // 
            this.PaymentsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaymentsGridControl.Location = new System.Drawing.Point(32, 70);
            this.PaymentsGridControl.MainView = this.PaymentsGridView;
            this.PaymentsGridControl.Name = "PaymentsGridControl";
            this.PaymentsGridControl.Size = new System.Drawing.Size(504, 376);
            this.PaymentsGridControl.TabIndex = 75;
            this.PaymentsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PaymentsGridView});
            // 
            // PaymentsGridView
            // 
            this.PaymentsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PaymentsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentsGridView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Purple;
            this.PaymentsGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.PaymentsGridView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.PaymentsGridView.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.GroupFooter.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.PaymentsGridView.Appearance.GroupFooter.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.GroupRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.PaymentsGridView.Appearance.GroupRow.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentsGridView.Appearance.GroupRow.ForeColor = System.Drawing.Color.Purple;
            this.PaymentsGridView.Appearance.GroupRow.Options.UseBackColor = true;
            this.PaymentsGridView.Appearance.GroupRow.Options.UseFont = true;
            this.PaymentsGridView.Appearance.GroupRow.Options.UseForeColor = true;
            this.PaymentsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentsGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PaymentsGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.PaymentsGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PaymentsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PaymentsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Payment_SS,
            this.Payment_Type,
            this.Payment_ContractCode,
            this.Payment_Date,
            this.Payment_CurrencyCode});
            this.PaymentsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.PaymentsGridView.FormatRules.Add(gridFormatRule1);
            this.PaymentsGridView.GridControl = this.PaymentsGridControl;
            this.PaymentsGridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Custom, "SUB_ACCOUNT_NAME", this.Payment_ContractCode, "")});
            this.PaymentsGridView.Name = "PaymentsGridView";
            this.PaymentsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PaymentsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PaymentsGridView.OptionsBehavior.Editable = false;
            this.PaymentsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PaymentsGridView.OptionsFind.FindDelay = 100;
            this.PaymentsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PaymentsGridView.OptionsView.ShowFooter = true;
            this.PaymentsGridView.OptionsView.ShowGroupPanel = false;
            this.PaymentsGridView.OptionsView.ShowIndicator = false;
            this.PaymentsGridView.OptionsView.ShowViewCaption = true;
            this.PaymentsGridView.PaintStyleName = "Skin";
            this.PaymentsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PaymentsGridView.ViewCaption = "Kassa əməliyyatlarının siyahısı";
            this.PaymentsGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.PaymentsGridView_CustomColumnDisplayText);
            this.PaymentsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaymentsGridView_MouseUp);
            // 
            // Payment_Type
            // 
            this.Payment_Type.Caption = "Ödənişin tipi";
            this.Payment_Type.FieldName = "PTYPE";
            this.Payment_Type.Name = "Payment_Type";
            this.Payment_Type.Visible = true;
            this.Payment_Type.VisibleIndex = 1;
            this.Payment_Type.Width = 120;
            // 
            // Payment_ContractCode
            // 
            this.Payment_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Payment_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Payment_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Payment_ContractCode.Caption = "Müqavilə";
            this.Payment_ContractCode.FieldName = "CONTRACT_CODE";
            this.Payment_ContractCode.Name = "Payment_ContractCode";
            this.Payment_ContractCode.Visible = true;
            this.Payment_ContractCode.VisibleIndex = 2;
            this.Payment_ContractCode.Width = 150;
            // 
            // Payment_Date
            // 
            this.Payment_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Payment_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Payment_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Payment_Date.Caption = "Ödənişin tarixi";
            this.Payment_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Payment_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Payment_Date.FieldName = "PDATE";
            this.Payment_Date.Name = "Payment_Date";
            this.Payment_Date.Visible = true;
            this.Payment_Date.VisibleIndex = 3;
            this.Payment_Date.Width = 130;
            // 
            // MonthComboBox
            // 
            this.MonthComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MonthComboBox.Location = new System.Drawing.Point(110, 11);
            this.MonthComboBox.Name = "MonthComboBox";
            this.MonthComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç")});
            this.MonthComboBox.Properties.DropDownRows = 12;
            this.MonthComboBox.Properties.Items.AddRange(new object[] {
            "",
            "Yanvar",
            "Fevral",
            "Mart",
            "Aprel",
            "May",
            "İyun",
            "İyul",
            "Avqust",
            "Sentyabr",
            "Oktyabr",
            "Noyabr",
            "Dekabr"});
            this.MonthComboBox.Properties.NullValuePrompt = "Seçin";
            this.MonthComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.MonthComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.MonthComboBox.Size = new System.Drawing.Size(96, 20);
            this.MonthComboBox.TabIndex = 242;
            this.MonthComboBox.SelectedIndexChanged += new System.EventHandler(this.MonthComboBox_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(91, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(13, 13);
            this.labelControl1.TabIndex = 243;
            this.labelControl1.Text = "Ay";
            // 
            // Payment_SS
            // 
            this.Payment_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Payment_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Payment_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Payment_SS.Caption = "S/s";
            this.Payment_SS.FieldName = "SS";
            this.Payment_SS.Name = "Payment_SS";
            this.Payment_SS.OptionsColumn.FixedWidth = true;
            this.Payment_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Payment_SS.Visible = true;
            this.Payment_SS.VisibleIndex = 0;
            this.Payment_SS.Width = 50;
            // 
            // Payment_CurrencyCode
            // 
            this.Payment_CurrencyCode.AppearanceCell.Options.UseTextOptions = true;
            this.Payment_CurrencyCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Payment_CurrencyCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Payment_CurrencyCode.Caption = "Valyuta";
            this.Payment_CurrencyCode.FieldName = "CURRENCY_CODE";
            this.Payment_CurrencyCode.Name = "Payment_CurrencyCode";
            this.Payment_CurrencyCode.Visible = true;
            this.Payment_CurrencyCode.VisibleIndex = 4;
            this.Payment_CurrencyCode.Width = 52;
            // 
            // FCalcAgainPayments
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(536, 496);
            this.Controls.Add(this.PaymentsGridControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.SearchDockPanel);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCalcAgainPayments";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ödənişlərin təkrar hesablanması";
            this.Load += new System.EventHandler(this.FCalcAgainPayments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).EndInit();
            this.SearchDockPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanel1_Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YearComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthComboBox.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarButtonItem SearchBarButton;
        private DevExpress.XtraBars.Docking.DockManager DockManager;
        private DevExpress.XtraBars.Docking.DockPanel SearchDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraEditors.ComboBoxEdit YearComboBox;
        private DevExpress.XtraEditors.DateEdit FromDateValue;
        private DevExpress.XtraEditors.LabelControl YearLabel;
        private DevExpress.XtraEditors.DateEdit ToDateValue;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraGrid.GridControl PaymentsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PaymentsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Payment_Type;
        private DevExpress.XtraGrid.Columns.GridColumn Payment_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Payment_Date;
        private DevExpress.XtraEditors.ComboBoxEdit MonthComboBox;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn Payment_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Payment_CurrencyCode;
    }
}