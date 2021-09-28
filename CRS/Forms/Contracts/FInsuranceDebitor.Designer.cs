namespace CRS.Forms.Contracts
{
    partial class FInsuranceDebitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInsuranceDebitor));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.Insurance_Diff = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.DiffCheck = new DevExpress.XtraEditors.CheckEdit();
            this.PoliceCheck = new DevExpress.XtraEditors.CheckEdit();
            this.LastCheck = new DevExpress.XtraEditors.CheckEdit();
            this.AgainCheck = new DevExpress.XtraEditors.CheckEdit();
            this.ClosedCheck = new DevExpress.XtraEditors.CheckEdit();
            this.CancelCheck = new DevExpress.XtraEditors.CheckEdit();
            this.ActiveCheck = new DevExpress.XtraEditors.CheckEdit();
            this.DebtCheck = new DevExpress.XtraEditors.CheckEdit();
            this.NotPoliceEqualCheck = new DevExpress.XtraEditors.CheckEdit();
            this.InsuranceGridControl = new DevExpress.XtraGrid.GridControl();
            this.InsuranceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Insurance_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_EndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Hostage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Police = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_TransferDebt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Police2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.PoliceEqualCheck = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).BeginInit();
            this.SearchDockPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DiffCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PoliceCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AgainCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CancelCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebtCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotPoliceEqualCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PoliceEqualCheck.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Insurance_Diff
            // 
            this.Insurance_Diff.Caption = "Fərq";
            this.Insurance_Diff.DisplayFormat.FormatString = "n2";
            this.Insurance_Diff.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_Diff.FieldName = "DIFF";
            this.Insurance_Diff.Name = "Insurance_Diff";
            this.Insurance_Diff.OptionsColumn.FixedWidth = true;
            this.Insurance_Diff.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "DIFF", "{0:n2}")});
            this.Insurance_Diff.Visible = true;
            this.Insurance_Diff.VisibleIndex = 9;
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 711);
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
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton, true)});
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
            this.SearchBarButton.Id = 3;
            this.SearchBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.Image")));
            this.SearchBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.LargeImage")));
            this.SearchBarButton.Name = "SearchBarButton";
            this.SearchBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SearchBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1665, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 711);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1665, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 711);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1665, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 711);
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
            this.SearchDockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.SearchDockPanel.ID = new System.Guid("1336955c-e4bc-43c6-98ba-4ed98cd7f30c");
            this.SearchDockPanel.Location = new System.Drawing.Point(1451, 0);
            this.SearchDockPanel.Name = "SearchDockPanel";
            this.SearchDockPanel.OriginalSize = new System.Drawing.Size(214, 200);
            this.SearchDockPanel.Size = new System.Drawing.Size(214, 711);
            this.SearchDockPanel.Text = "Ətraflı axtar";
            this.SearchDockPanel.ClosedPanel += new DevExpress.XtraBars.Docking.DockPanelEventHandler(this.SearchDockPanel_ClosedPanel);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.PoliceEqualCheck);
            this.dockPanel1_Container.Controls.Add(this.DiffCheck);
            this.dockPanel1_Container.Controls.Add(this.PoliceCheck);
            this.dockPanel1_Container.Controls.Add(this.LastCheck);
            this.dockPanel1_Container.Controls.Add(this.AgainCheck);
            this.dockPanel1_Container.Controls.Add(this.ClosedCheck);
            this.dockPanel1_Container.Controls.Add(this.CancelCheck);
            this.dockPanel1_Container.Controls.Add(this.ActiveCheck);
            this.dockPanel1_Container.Controls.Add(this.DebtCheck);
            this.dockPanel1_Container.Controls.Add(this.NotPoliceEqualCheck);
            this.dockPanel1_Container.Location = new System.Drawing.Point(7, 27);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(202, 679);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // DiffCheck
            // 
            this.DiffCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DiffCheck.Location = new System.Drawing.Point(12, 246);
            this.DiffCheck.MenuManager = this.BarManager;
            this.DiffCheck.Name = "DiffCheck";
            this.DiffCheck.Properties.Caption = "Borcu fərqli olanlar";
            this.DiffCheck.Size = new System.Drawing.Size(204, 20);
            this.DiffCheck.TabIndex = 87;
            this.DiffCheck.CheckedChanged += new System.EventHandler(this.DebtCheck_CheckedChanged);
            // 
            // PoliceCheck
            // 
            this.PoliceCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PoliceCheck.Location = new System.Drawing.Point(12, 220);
            this.PoliceCheck.MenuManager = this.BarManager;
            this.PoliceCheck.Name = "PoliceCheck";
            this.PoliceCheck.Properties.Caption = "Sistemdə qeydiyyatı olmayanlar";
            this.PoliceCheck.Size = new System.Drawing.Size(204, 20);
            this.PoliceCheck.TabIndex = 86;
            this.PoliceCheck.CheckedChanged += new System.EventHandler(this.DebtCheck_CheckedChanged);
            // 
            // LastCheck
            // 
            this.LastCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LastCheck.Location = new System.Drawing.Point(12, 12);
            this.LastCheck.MenuManager = this.BarManager;
            this.LastCheck.Name = "LastCheck";
            this.LastCheck.Properties.Caption = "Sonunçu sığortalar";
            this.LastCheck.Size = new System.Drawing.Size(143, 20);
            this.LastCheck.TabIndex = 78;
            this.LastCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // AgainCheck
            // 
            this.AgainCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AgainCheck.Location = new System.Drawing.Point(12, 38);
            this.AgainCheck.MenuManager = this.BarManager;
            this.AgainCheck.Name = "AgainCheck";
            this.AgainCheck.Properties.Caption = "Təkrar sığortalar";
            this.AgainCheck.Size = new System.Drawing.Size(143, 20);
            this.AgainCheck.TabIndex = 79;
            this.AgainCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // ClosedCheck
            // 
            this.ClosedCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClosedCheck.Location = new System.Drawing.Point(12, 194);
            this.ClosedCheck.MenuManager = this.BarManager;
            this.ClosedCheck.Name = "ClosedCheck";
            this.ClosedCheck.Properties.Caption = "Bağlanmış müqavilələr";
            this.ClosedCheck.Size = new System.Drawing.Size(160, 20);
            this.ClosedCheck.TabIndex = 85;
            this.ClosedCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // CancelCheck
            // 
            this.CancelCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CancelCheck.Location = new System.Drawing.Point(12, 64);
            this.CancelCheck.MenuManager = this.BarManager;
            this.CancelCheck.Name = "CancelCheck";
            this.CancelCheck.Properties.Caption = "İmtina edilmiş sığortalar";
            this.CancelCheck.Size = new System.Drawing.Size(160, 20);
            this.CancelCheck.TabIndex = 80;
            this.CancelCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // ActiveCheck
            // 
            this.ActiveCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActiveCheck.Location = new System.Drawing.Point(12, 168);
            this.ActiveCheck.MenuManager = this.BarManager;
            this.ActiveCheck.Name = "ActiveCheck";
            this.ActiveCheck.Properties.Caption = "Aktiv müqavilələr";
            this.ActiveCheck.Size = new System.Drawing.Size(160, 20);
            this.ActiveCheck.TabIndex = 84;
            this.ActiveCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // DebtCheck
            // 
            this.DebtCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DebtCheck.Location = new System.Drawing.Point(12, 90);
            this.DebtCheck.MenuManager = this.BarManager;
            this.DebtCheck.Name = "DebtCheck";
            this.DebtCheck.Properties.Caption = "Borcu olanlar";
            this.DebtCheck.Size = new System.Drawing.Size(160, 20);
            this.DebtCheck.TabIndex = 81;
            this.DebtCheck.CheckedChanged += new System.EventHandler(this.DebtCheck_CheckedChanged);
            // 
            // NotPoliceEqualCheck
            // 
            this.NotPoliceEqualCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotPoliceEqualCheck.Location = new System.Drawing.Point(12, 116);
            this.NotPoliceEqualCheck.MenuManager = this.BarManager;
            this.NotPoliceEqualCheck.Name = "NotPoliceEqualCheck";
            this.NotPoliceEqualCheck.Properties.Caption = "Qeydiyyatı uyğun olmayanlar";
            this.NotPoliceEqualCheck.Size = new System.Drawing.Size(187, 20);
            this.NotPoliceEqualCheck.TabIndex = 82;
            this.NotPoliceEqualCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // InsuranceGridControl
            // 
            this.InsuranceGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InsuranceGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Location = new System.Drawing.Point(37, 0);
            this.InsuranceGridControl.MainView = this.InsuranceGridView;
            this.InsuranceGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Name = "InsuranceGridControl";
            this.InsuranceGridControl.Size = new System.Drawing.Size(1414, 711);
            this.InsuranceGridControl.TabIndex = 71;
            this.InsuranceGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.InsuranceGridView});
            // 
            // InsuranceGridView
            // 
            this.InsuranceGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.InsuranceGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.InsuranceGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.InsuranceGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.InsuranceGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Maroon;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.InsuranceGridView.ColumnPanelRowHeight = 40;
            this.InsuranceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Insurance_SS,
            this.Insurance_ContractCode,
            this.Insurance_StartDate,
            this.Insurance_EndDate,
            this.Insurance_Hostage,
            this.Insurance_Police,
            this.Insurance_TransferDebt,
            this.Insurance_Police2,
            this.Insurance_Amount,
            this.Insurance_Diff});
            this.InsuranceGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.Column = this.Insurance_Diff;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.NotEqual;
            formatConditionRuleValue1.PredefinedName = "Red Text";
            formatConditionRuleValue1.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            gridFormatRule1.Rule = formatConditionRuleValue1;
            this.InsuranceGridView.FormatRules.Add(gridFormatRule1);
            this.InsuranceGridView.GridControl = this.InsuranceGridControl;
            this.InsuranceGridView.Name = "InsuranceGridView";
            this.InsuranceGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.InsuranceGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.InsuranceGridView.OptionsBehavior.Editable = false;
            this.InsuranceGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.InsuranceGridView.OptionsFind.FindDelay = 100;
            this.InsuranceGridView.OptionsPrint.RtfPageFooter = resources.GetString("InsuranceGridView.OptionsPrint.RtfPageFooter");
            this.InsuranceGridView.OptionsPrint.RtfReportHeader = resources.GetString("InsuranceGridView.OptionsPrint.RtfReportHeader");
            this.InsuranceGridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.InsuranceGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.InsuranceGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.InsuranceGridView.OptionsView.ShowFooter = true;
            this.InsuranceGridView.OptionsView.ShowGroupPanel = false;
            this.InsuranceGridView.OptionsView.ShowIndicator = false;
            this.InsuranceGridView.PaintStyleName = "Skin";
            this.InsuranceGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.InsuranceGridView.ViewCaption = "Sığortaların siyahısı";
            this.InsuranceGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.InsuranceGridView_CustomDrawFooterCell);
            this.InsuranceGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.InsuranceGridView_RowCellStyle);
            this.InsuranceGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.InsuranceGridView_CustomUnboundColumnData);
            this.InsuranceGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InsuranceGridView_MouseUp);
            // 
            // Insurance_SS
            // 
            this.Insurance_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_SS.Caption = "S/s";
            this.Insurance_SS.FieldName = "Insurance_SS";
            this.Insurance_SS.Name = "Insurance_SS";
            this.Insurance_SS.OptionsColumn.FixedWidth = true;
            this.Insurance_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Insurance_SS", "{0}")});
            this.Insurance_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Insurance_SS.Visible = true;
            this.Insurance_SS.VisibleIndex = 0;
            this.Insurance_SS.Width = 55;
            // 
            // Insurance_ContractCode
            // 
            this.Insurance_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_ContractCode.Caption = "Müqavilə";
            this.Insurance_ContractCode.FieldName = "CONTRACT_CODE";
            this.Insurance_ContractCode.Name = "Insurance_ContractCode";
            this.Insurance_ContractCode.OptionsColumn.FixedWidth = true;
            this.Insurance_ContractCode.Visible = true;
            this.Insurance_ContractCode.VisibleIndex = 1;
            this.Insurance_ContractCode.Width = 70;
            // 
            // Insurance_StartDate
            // 
            this.Insurance_StartDate.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_StartDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_StartDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_StartDate.Caption = "Başlama tarixi";
            this.Insurance_StartDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Insurance_StartDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Insurance_StartDate.FieldName = "START_DATE";
            this.Insurance_StartDate.Name = "Insurance_StartDate";
            this.Insurance_StartDate.OptionsColumn.FixedWidth = true;
            this.Insurance_StartDate.Visible = true;
            this.Insurance_StartDate.VisibleIndex = 2;
            this.Insurance_StartDate.Width = 100;
            // 
            // Insurance_EndDate
            // 
            this.Insurance_EndDate.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_EndDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_EndDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_EndDate.Caption = "Bitmə tarixi";
            this.Insurance_EndDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Insurance_EndDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Insurance_EndDate.FieldName = "END_DATE";
            this.Insurance_EndDate.Name = "Insurance_EndDate";
            this.Insurance_EndDate.OptionsColumn.FixedWidth = true;
            this.Insurance_EndDate.Visible = true;
            this.Insurance_EndDate.VisibleIndex = 3;
            this.Insurance_EndDate.Width = 100;
            // 
            // Insurance_Hostage
            // 
            this.Insurance_Hostage.Caption = "Lizinq predmeti";
            this.Insurance_Hostage.FieldName = "HOSTAGE";
            this.Insurance_Hostage.Name = "Insurance_Hostage";
            this.Insurance_Hostage.OptionsColumn.FixedWidth = true;
            this.Insurance_Hostage.Visible = true;
            this.Insurance_Hostage.VisibleIndex = 4;
            this.Insurance_Hostage.Width = 200;
            // 
            // Insurance_Police
            // 
            this.Insurance_Police.Caption = "Sistemdəki qeydiyyatı";
            this.Insurance_Police.FieldName = "POLICE";
            this.Insurance_Police.Name = "Insurance_Police";
            this.Insurance_Police.OptionsColumn.FixedWidth = true;
            this.Insurance_Police.Visible = true;
            this.Insurance_Police.VisibleIndex = 5;
            this.Insurance_Police.Width = 130;
            // 
            // Insurance_TransferDebt
            // 
            this.Insurance_TransferDebt.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Insurance_TransferDebt.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_TransferDebt.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_TransferDebt.AppearanceCell.Options.UseFont = true;
            this.Insurance_TransferDebt.Caption = "Sistemdə olan borc";
            this.Insurance_TransferDebt.DisplayFormat.FormatString = "n2";
            this.Insurance_TransferDebt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_TransferDebt.FieldName = "TRANSFER_DEBT";
            this.Insurance_TransferDebt.Name = "Insurance_TransferDebt";
            this.Insurance_TransferDebt.OptionsColumn.FixedWidth = true;
            this.Insurance_TransferDebt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TRANSFER_DEBT", "{0:n2}")});
            this.Insurance_TransferDebt.Visible = true;
            this.Insurance_TransferDebt.VisibleIndex = 6;
            this.Insurance_TransferDebt.Width = 140;
            // 
            // Insurance_Police2
            // 
            this.Insurance_Police2.Caption = "Göndərilən qeydiyyat";
            this.Insurance_Police2.FieldName = "POLICE2";
            this.Insurance_Police2.Name = "Insurance_Police2";
            this.Insurance_Police2.OptionsColumn.FixedWidth = true;
            this.Insurance_Police2.ToolTip = "Şirkət tərəfindən göndərilən polisdəki qeydiyyat";
            this.Insurance_Police2.Visible = true;
            this.Insurance_Police2.VisibleIndex = 7;
            this.Insurance_Police2.Width = 130;
            // 
            // Insurance_Amount
            // 
            this.Insurance_Amount.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Insurance_Amount.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_Amount.Caption = "Göndərilən borc";
            this.Insurance_Amount.DisplayFormat.FormatString = "n2";
            this.Insurance_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_Amount.FieldName = "AMOUNT";
            this.Insurance_Amount.Name = "Insurance_Amount";
            this.Insurance_Amount.OptionsColumn.FixedWidth = true;
            this.Insurance_Amount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", "{0:n2}")});
            this.Insurance_Amount.ToolTip = "Şirkət tərəfindən göndərilən borc";
            this.Insurance_Amount.Visible = true;
            this.Insurance_Amount.VisibleIndex = 8;
            this.Insurance_Amount.Width = 140;
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton, true)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // PoliceEqualCheck
            // 
            this.PoliceEqualCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PoliceEqualCheck.EditValue = true;
            this.PoliceEqualCheck.Location = new System.Drawing.Point(12, 142);
            this.PoliceEqualCheck.MenuManager = this.BarManager;
            this.PoliceEqualCheck.Name = "PoliceEqualCheck";
            this.PoliceEqualCheck.Properties.Caption = "Qeydiyyatı uyğun olanlar";
            this.PoliceEqualCheck.Size = new System.Drawing.Size(187, 20);
            this.PoliceEqualCheck.TabIndex = 83;
            this.PoliceEqualCheck.CheckedChanged += new System.EventHandler(this.LastCheck_CheckedChanged);
            // 
            // FInsuranceDebitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1665, 711);
            this.Controls.Add(this.InsuranceGridControl);
            this.Controls.Add(this.SearchDockPanel);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FInsuranceDebitor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sığorta qalıqlarının müqayisəsi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FInsuranceDebitor_FormClosing);
            this.Load += new System.EventHandler(this.FInsuranceDebitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).EndInit();
            this.SearchDockPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DiffCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PoliceCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AgainCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CancelCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebtCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotPoliceEqualCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PoliceEqualCheck.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl InsuranceGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView InsuranceGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_EndDate;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Police;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_TransferDebt;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Police2;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem SearchBarButton;
        private DevExpress.XtraBars.Docking.DockManager DockManager;
        private DevExpress.XtraBars.Docking.DockPanel SearchDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.CheckEdit PoliceCheck;
        private DevExpress.XtraEditors.CheckEdit LastCheck;
        private DevExpress.XtraEditors.CheckEdit AgainCheck;
        private DevExpress.XtraEditors.CheckEdit ClosedCheck;
        private DevExpress.XtraEditors.CheckEdit CancelCheck;
        private DevExpress.XtraEditors.CheckEdit ActiveCheck;
        private DevExpress.XtraEditors.CheckEdit DebtCheck;
        private DevExpress.XtraEditors.CheckEdit NotPoliceEqualCheck;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Hostage;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Diff;
        private DevExpress.XtraEditors.CheckEdit DiffCheck;
        private DevExpress.XtraEditors.CheckEdit PoliceEqualCheck;
    }
}