namespace CRS.Forms.Contracts
{
    partial class FInsuranceTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInsuranceTransfer));
            ManiXButton.Office2010Blue office2010Blue1 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.Insurance_TransferDebt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.PaymentTaskCheck = new DevExpress.XtraEditors.CheckEdit();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.Bar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.SearchBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.CountSelectedRowsBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.SumSelectedRowsBarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.DockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.SearchDockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ClosedCheck = new DevExpress.XtraEditors.CheckEdit();
            this.ActiveCheck = new DevExpress.XtraEditors.CheckEdit();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.InsuranceGridControl = new DevExpress.XtraGrid.GridControl();
            this.InsuranceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Insurance_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Police = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_CompanyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Period = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Interest = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Cost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_PayedAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Compensation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_SumTransfer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_TransferAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_CarNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_StatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTaskCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).BeginInit();
            this.SearchDockPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // Insurance_TransferDebt
            // 
            this.Insurance_TransferDebt.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_TransferDebt.AppearanceHeader.Options.UseFont = true;
            this.Insurance_TransferDebt.Caption = "Hesabda qalacaq";
            this.Insurance_TransferDebt.DisplayFormat.FormatString = "n2";
            this.Insurance_TransferDebt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_TransferDebt.FieldName = "Insurance_TransferDebt";
            this.Insurance_TransferDebt.Name = "Insurance_TransferDebt";
            this.Insurance_TransferDebt.OptionsColumn.AllowEdit = false;
            this.Insurance_TransferDebt.OptionsColumn.FixedWidth = true;
            this.Insurance_TransferDebt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Insurance_TransferDebt", "{0:n2}")});
            this.Insurance_TransferDebt.UnboundExpression = "[PAYED_AMOUNT] - [SUM_TRANSFER] - [TRANSFER_AMOUNT] - [COMPENSATION]";
            this.Insurance_TransferDebt.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Insurance_TransferDebt.Visible = true;
            this.Insurance_TransferDebt.VisibleIndex = 9;
            this.Insurance_TransferDebt.Width = 110;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.PaymentTaskCheck);
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(37, 618);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(1734, 62);
            this.PanelOption.TabIndex = 52;
            // 
            // PaymentTaskCheck
            // 
            this.PaymentTaskCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PaymentTaskCheck.EditValue = true;
            this.PaymentTaskCheck.Location = new System.Drawing.Point(63, 21);
            this.PaymentTaskCheck.MenuManager = this.BarManager;
            this.PaymentTaskCheck.Name = "PaymentTaskCheck";
            this.PaymentTaskCheck.Properties.Caption = "Ödəniş tapşırığı yaradılsın";
            this.PaymentTaskCheck.Size = new System.Drawing.Size(190, 20);
            this.PaymentTaskCheck.TabIndex = 236;
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.Bar,
            this.bar3});
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
            this.SumSelectedRowsBarStaticItem,
            this.CountSelectedRowsBarStaticItem,
            this.SearchBarButton});
            this.BarManager.MainMenu = this.Bar;
            this.BarManager.MaxItemId = 6;
            this.BarManager.StatusBar = this.bar3;
            // 
            // Bar
            // 
            this.Bar.BarName = "Main menu";
            this.Bar.DockCol = 0;
            this.Bar.DockRow = 0;
            this.Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton, true)});
            this.Bar.OptionsBar.DrawBorder = false;
            this.Bar.OptionsBar.DrawDragBorder = false;
            this.Bar.OptionsBar.MultiLine = true;
            this.Bar.OptionsBar.UseWholeRow = true;
            this.Bar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.Bar.Text = "Main menu";
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
            this.SearchBarButton.Id = 5;
            this.SearchBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.Image")));
            this.SearchBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("SearchBarButton.ImageOptions.LargeImage")));
            this.SearchBarButton.Name = "SearchBarButton";
            this.SearchBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SearchBarButton_ItemClick);
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 680);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.CountSelectedRowsBarStaticItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SumSelectedRowsBarStaticItem)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // CountSelectedRowsBarStaticItem
            // 
            this.CountSelectedRowsBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.CountSelectedRowsBarStaticItem.Caption = "Count";
            this.CountSelectedRowsBarStaticItem.Id = 4;
            this.CountSelectedRowsBarStaticItem.Name = "CountSelectedRowsBarStaticItem";
            // 
            // SumSelectedRowsBarStaticItem
            // 
            this.SumSelectedRowsBarStaticItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            this.SumSelectedRowsBarStaticItem.Caption = "Sum";
            this.SumSelectedRowsBarStaticItem.Id = 3;
            this.SumSelectedRowsBarStaticItem.Name = "SumSelectedRowsBarStaticItem";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1771, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 680);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1771, 34);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 680);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1771, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 680);
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
            this.SearchDockPanel.ID = new System.Guid("65d4fe2a-6fa5-461c-9c62-31e114578962");
            this.SearchDockPanel.Location = new System.Drawing.Point(37, 0);
            this.SearchDockPanel.Name = "SearchDockPanel";
            this.SearchDockPanel.OriginalSize = new System.Drawing.Size(200, 77);
            this.SearchDockPanel.Size = new System.Drawing.Size(1734, 77);
            this.SearchDockPanel.Text = "Ətraflı axtar";
            this.SearchDockPanel.ClosedPanel += new DevExpress.XtraBars.Docking.DockPanelEventHandler(this.SearchDockPanel_ClosedPanel);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.ClosedCheck);
            this.dockPanel1_Container.Controls.Add(this.ActiveCheck);
            this.dockPanel1_Container.Location = new System.Drawing.Point(5, 27);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1724, 43);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // ClosedCheck
            // 
            this.ClosedCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClosedCheck.Location = new System.Drawing.Point(224, 12);
            this.ClosedCheck.MenuManager = this.BarManager;
            this.ClosedCheck.Name = "ClosedCheck";
            this.ClosedCheck.Properties.Caption = "Bağlanmış müqavilələr";
            this.ClosedCheck.Size = new System.Drawing.Size(160, 20);
            this.ClosedCheck.TabIndex = 236;
            this.ClosedCheck.CheckedChanged += new System.EventHandler(this.ActiveCheck_CheckedChanged);
            // 
            // ActiveCheck
            // 
            this.ActiveCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActiveCheck.EditValue = true;
            this.ActiveCheck.Location = new System.Drawing.Point(58, 12);
            this.ActiveCheck.MenuManager = this.BarManager;
            this.ActiveCheck.Name = "ActiveCheck";
            this.ActiveCheck.Properties.Caption = "Aktiv müqavilələr";
            this.ActiveCheck.Size = new System.Drawing.Size(160, 20);
            this.ActiveCheck.TabIndex = 235;
            this.ActiveCheck.CheckedChanged += new System.EventHandler(this.ActiveCheck_CheckedChanged);
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
            this.BOK.Location = new System.Drawing.Point(1536, 16);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(87, 31);
            this.BOK.TabIndex = 4;
            this.BOK.TabStop = false;
            this.BOK.Text = "Təsdiqlə";
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
            this.BCancel.Location = new System.Drawing.Point(1630, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.TabStop = false;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // InsuranceGridControl
            // 
            this.InsuranceGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InsuranceGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Location = new System.Drawing.Point(37, 77);
            this.InsuranceGridControl.MainView = this.InsuranceGridView;
            this.InsuranceGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Name = "InsuranceGridControl";
            this.InsuranceGridControl.Size = new System.Drawing.Size(1734, 541);
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
            this.InsuranceGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.InsuranceGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Maroon;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.InsuranceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Insurance_SS,
            this.Insurance_ContractCode,
            this.Insurance_Police,
            this.Insurance_CompanyName,
            this.Insurance_Amount,
            this.Insurance_Period,
            this.Insurance_Interest,
            this.Insurance_Cost,
            this.Insurance_PayedAmount,
            this.Insurance_Debt,
            this.Insurance_Compensation,
            this.Insurance_SumTransfer,
            this.Insurance_TransferAmount,
            this.Insurance_TransferDebt,
            this.Insurance_Note,
            this.Insurance_CarNumber,
            this.Insurance_StatusID});
            gridFormatRule1.Column = this.Insurance_TransferDebt;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Less;
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
            this.InsuranceGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.InsuranceGridView.OptionsFind.FindDelay = 100;
            this.InsuranceGridView.OptionsPrint.RtfPageFooter = resources.GetString("InsuranceGridView.OptionsPrint.RtfPageFooter");
            this.InsuranceGridView.OptionsPrint.RtfReportHeader = resources.GetString("InsuranceGridView.OptionsPrint.RtfReportHeader");
            this.InsuranceGridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.InsuranceGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.InsuranceGridView.OptionsSelection.MultiSelect = true;
            this.InsuranceGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.InsuranceGridView.OptionsView.ShowFooter = true;
            this.InsuranceGridView.OptionsView.ShowGroupPanel = false;
            this.InsuranceGridView.OptionsView.ShowIndicator = false;
            this.InsuranceGridView.PaintStyleName = "Skin";
            this.InsuranceGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.InsuranceGridView.ViewCaption = "Sığortaların siyahısı";
            this.InsuranceGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.InsuranceGridView_CustomDrawFooterCell);
            this.InsuranceGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.InsuranceGridView_RowCellStyle);
            this.InsuranceGridView.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.InsuranceGridView_SelectionChanged);
            this.InsuranceGridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.InsuranceGridView_CellValueChanged);
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
            this.Insurance_SS.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_SS.Name = "Insurance_SS";
            this.Insurance_SS.OptionsColumn.AllowEdit = false;
            this.Insurance_SS.OptionsColumn.FixedWidth = true;
            this.Insurance_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Insurance_SS", "{0}")});
            this.Insurance_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Insurance_SS.Visible = true;
            this.Insurance_SS.VisibleIndex = 1;
            this.Insurance_SS.Width = 40;
            // 
            // Insurance_ContractCode
            // 
            this.Insurance_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_ContractCode.Caption = "Müqavilə";
            this.Insurance_ContractCode.FieldName = "CONTRACT_CODE";
            this.Insurance_ContractCode.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_ContractCode.Name = "Insurance_ContractCode";
            this.Insurance_ContractCode.OptionsColumn.AllowEdit = false;
            this.Insurance_ContractCode.OptionsColumn.FixedWidth = true;
            this.Insurance_ContractCode.Visible = true;
            this.Insurance_ContractCode.VisibleIndex = 2;
            this.Insurance_ContractCode.Width = 50;
            // 
            // Insurance_Police
            // 
            this.Insurance_Police.Caption = "Polisdəki qeydiyyatı";
            this.Insurance_Police.FieldName = "POLICE";
            this.Insurance_Police.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Police.Name = "Insurance_Police";
            this.Insurance_Police.OptionsColumn.FixedWidth = true;
            this.Insurance_Police.Visible = true;
            this.Insurance_Police.VisibleIndex = 3;
            this.Insurance_Police.Width = 110;
            // 
            // Insurance_CompanyName
            // 
            this.Insurance_CompanyName.Caption = "Sığorta şirkəti";
            this.Insurance_CompanyName.FieldName = "COMPANY_NAME";
            this.Insurance_CompanyName.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_CompanyName.Name = "Insurance_CompanyName";
            this.Insurance_CompanyName.OptionsColumn.AllowEdit = false;
            this.Insurance_CompanyName.OptionsColumn.AllowShowHide = false;
            this.Insurance_CompanyName.Width = 721;
            // 
            // Insurance_Amount
            // 
            this.Insurance_Amount.Caption = "Sığorta dəyəri";
            this.Insurance_Amount.DisplayFormat.FormatString = "{0:n2} AZN";
            this.Insurance_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Insurance_Amount.FieldName = "INSURANCE_AMOUNT";
            this.Insurance_Amount.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Amount.Name = "Insurance_Amount";
            this.Insurance_Amount.OptionsColumn.AllowEdit = false;
            this.Insurance_Amount.OptionsColumn.AllowShowHide = false;
            this.Insurance_Amount.OptionsColumn.FixedWidth = true;
            this.Insurance_Amount.Width = 100;
            // 
            // Insurance_Period
            // 
            this.Insurance_Period.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_Period.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Insurance_Period.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_Period.Caption = "Sığorta müddəti";
            this.Insurance_Period.DisplayFormat.FormatString = "{0} ay";
            this.Insurance_Period.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Insurance_Period.FieldName = "INSURANCE_PERIOD";
            this.Insurance_Period.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Period.Name = "Insurance_Period";
            this.Insurance_Period.OptionsColumn.AllowEdit = false;
            this.Insurance_Period.OptionsColumn.AllowShowHide = false;
            this.Insurance_Period.OptionsColumn.FixedWidth = true;
            this.Insurance_Period.Width = 90;
            // 
            // Insurance_Interest
            // 
            this.Insurance_Interest.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_Interest.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Insurance_Interest.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_Interest.Caption = "Sığorta dərəcəsi";
            this.Insurance_Interest.DisplayFormat.FormatString = "{0} %";
            this.Insurance_Interest.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Insurance_Interest.FieldName = "INSURANCE_INTEREST";
            this.Insurance_Interest.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Interest.Name = "Insurance_Interest";
            this.Insurance_Interest.OptionsColumn.AllowEdit = false;
            this.Insurance_Interest.OptionsColumn.AllowShowHide = false;
            this.Insurance_Interest.OptionsColumn.FixedWidth = true;
            this.Insurance_Interest.Width = 90;
            // 
            // Insurance_Cost
            // 
            this.Insurance_Cost.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Insurance_Cost.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_Cost.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_Cost.AppearanceCell.Options.UseFont = true;
            this.Insurance_Cost.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_Cost.AppearanceHeader.Options.UseFont = true;
            this.Insurance_Cost.Caption = "Sığorta haqqı";
            this.Insurance_Cost.DisplayFormat.FormatString = "n2";
            this.Insurance_Cost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_Cost.FieldName = "INSURANCE_COST";
            this.Insurance_Cost.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Cost.Name = "Insurance_Cost";
            this.Insurance_Cost.OptionsColumn.AllowEdit = false;
            this.Insurance_Cost.OptionsColumn.FixedWidth = true;
            this.Insurance_Cost.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "INSURANCE_COST", "{0:n2}")});
            this.Insurance_Cost.Visible = true;
            this.Insurance_Cost.VisibleIndex = 4;
            this.Insurance_Cost.Width = 110;
            // 
            // Insurance_PayedAmount
            // 
            this.Insurance_PayedAmount.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Insurance_PayedAmount.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_PayedAmount.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_PayedAmount.AppearanceCell.Options.UseFont = true;
            this.Insurance_PayedAmount.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_PayedAmount.AppearanceHeader.Options.UseFont = true;
            this.Insurance_PayedAmount.Caption = "Ödənilən";
            this.Insurance_PayedAmount.DisplayFormat.FormatString = "n2";
            this.Insurance_PayedAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_PayedAmount.FieldName = "PAYED_AMOUNT";
            this.Insurance_PayedAmount.Name = "Insurance_PayedAmount";
            this.Insurance_PayedAmount.OptionsColumn.AllowEdit = false;
            this.Insurance_PayedAmount.OptionsColumn.FixedWidth = true;
            this.Insurance_PayedAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PAYED_AMOUNT", "{0:n2}")});
            this.Insurance_PayedAmount.Visible = true;
            this.Insurance_PayedAmount.VisibleIndex = 5;
            this.Insurance_PayedAmount.Width = 110;
            // 
            // Insurance_Debt
            // 
            this.Insurance_Debt.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Insurance_Debt.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_Debt.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_Debt.AppearanceCell.Options.UseFont = true;
            this.Insurance_Debt.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_Debt.AppearanceHeader.Options.UseFont = true;
            this.Insurance_Debt.Caption = "Borc";
            this.Insurance_Debt.DisplayFormat.FormatString = "{0:n2} AZN";
            this.Insurance_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Insurance_Debt.FieldName = "Insurance_Debt";
            this.Insurance_Debt.Name = "Insurance_Debt";
            this.Insurance_Debt.OptionsColumn.AllowEdit = false;
            this.Insurance_Debt.OptionsColumn.FixedWidth = true;
            this.Insurance_Debt.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Insurance_Debt", "{0:n2} AZN")});
            this.Insurance_Debt.UnboundExpression = "[INSURANCE_COST] - [PAYED_AMOUNT]";
            this.Insurance_Debt.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Insurance_Debt.Width = 110;
            // 
            // Insurance_Compensation
            // 
            this.Insurance_Compensation.Caption = "Əvəzləşdirilib";
            this.Insurance_Compensation.DisplayFormat.FormatString = "n2";
            this.Insurance_Compensation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_Compensation.FieldName = "COMPENSATION";
            this.Insurance_Compensation.Name = "Insurance_Compensation";
            this.Insurance_Compensation.OptionsColumn.AllowEdit = false;
            this.Insurance_Compensation.OptionsColumn.FixedWidth = true;
            this.Insurance_Compensation.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "COMPENSATION", "{0:n2}")});
            this.Insurance_Compensation.Visible = true;
            this.Insurance_Compensation.VisibleIndex = 6;
            this.Insurance_Compensation.Width = 110;
            // 
            // Insurance_SumTransfer
            // 
            this.Insurance_SumTransfer.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_SumTransfer.AppearanceCell.Options.UseFont = true;
            this.Insurance_SumTransfer.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_SumTransfer.AppearanceHeader.Options.UseFont = true;
            this.Insurance_SumTransfer.Caption = "Köçürülüb";
            this.Insurance_SumTransfer.DisplayFormat.FormatString = "n2";
            this.Insurance_SumTransfer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_SumTransfer.FieldName = "SUM_TRANSFER";
            this.Insurance_SumTransfer.Name = "Insurance_SumTransfer";
            this.Insurance_SumTransfer.OptionsColumn.AllowEdit = false;
            this.Insurance_SumTransfer.OptionsColumn.FixedWidth = true;
            this.Insurance_SumTransfer.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "SUM_TRANSFER", "{0:n2}")});
            this.Insurance_SumTransfer.Visible = true;
            this.Insurance_SumTransfer.VisibleIndex = 7;
            this.Insurance_SumTransfer.Width = 110;
            // 
            // Insurance_TransferAmount
            // 
            this.Insurance_TransferAmount.AppearanceCell.BackColor = System.Drawing.Color.AntiqueWhite;
            this.Insurance_TransferAmount.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_TransferAmount.Caption = "Köçürülə bilən";
            this.Insurance_TransferAmount.DisplayFormat.FormatString = "n2";
            this.Insurance_TransferAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_TransferAmount.FieldName = "TRANSFER_AMOUNT";
            this.Insurance_TransferAmount.Name = "Insurance_TransferAmount";
            this.Insurance_TransferAmount.OptionsColumn.FixedWidth = true;
            this.Insurance_TransferAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TRANSFER_AMOUNT", "{0:n2}")});
            this.Insurance_TransferAmount.ToolTip = "Sığorta şirkətinə köçürüləcək məbləğ";
            this.Insurance_TransferAmount.Visible = true;
            this.Insurance_TransferAmount.VisibleIndex = 8;
            this.Insurance_TransferAmount.Width = 110;
            // 
            // Insurance_Note
            // 
            this.Insurance_Note.Caption = "Köçürmənin qeydi";
            this.Insurance_Note.FieldName = "NOTE";
            this.Insurance_Note.Name = "Insurance_Note";
            this.Insurance_Note.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.Insurance_Note.Visible = true;
            this.Insurance_Note.VisibleIndex = 10;
            this.Insurance_Note.Width = 715;
            // 
            // Insurance_CarNumber
            // 
            this.Insurance_CarNumber.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_CarNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_CarNumber.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_CarNumber.Caption = "Avtomobilin nömrəsi";
            this.Insurance_CarNumber.FieldName = "CAR_NUMBER";
            this.Insurance_CarNumber.Name = "Insurance_CarNumber";
            this.Insurance_CarNumber.OptionsColumn.FixedWidth = true;
            this.Insurance_CarNumber.Visible = true;
            this.Insurance_CarNumber.VisibleIndex = 11;
            this.Insurance_CarNumber.Width = 115;
            // 
            // Insurance_StatusID
            // 
            this.Insurance_StatusID.Caption = "StatusID";
            this.Insurance_StatusID.FieldName = "STATUS_ID";
            this.Insurance_StatusID.Name = "Insurance_StatusID";
            this.Insurance_StatusID.OptionsColumn.AllowShowHide = false;
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // FInsuranceTransfer
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(1771, 714);
            this.Controls.Add(this.InsuranceGridControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.SearchDockPanel);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FInsuranceTransfer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sığorta köçürmələri";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FInsuranceTransfer_FormClosing);
            this.Load += new System.EventHandler(this.FInsuranceTransfer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTaskCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).EndInit();
            this.SearchDockPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar Bar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl InsuranceGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView InsuranceGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_CompanyName;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Period;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Interest;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Cost;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_PayedAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Debt;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_TransferAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_TransferDebt;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Compensation;
        private DevExpress.XtraBars.BarStaticItem SumSelectedRowsBarStaticItem;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarStaticItem CountSelectedRowsBarStaticItem;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_SumTransfer;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Note;
        private DevExpress.XtraBars.BarButtonItem SearchBarButton;
        private DevExpress.XtraBars.Docking.DockManager DockManager;
        private DevExpress.XtraBars.Docking.DockPanel SearchDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.CheckEdit ClosedCheck;
        private DevExpress.XtraEditors.CheckEdit ActiveCheck;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_StatusID;
        private DevExpress.XtraEditors.CheckEdit PaymentTaskCheck;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_CarNumber;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Police;
    }
}