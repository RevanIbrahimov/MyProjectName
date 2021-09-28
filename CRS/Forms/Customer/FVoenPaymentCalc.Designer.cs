namespace CRS.Forms.Customer
{
    partial class FVoenPaymentCalc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FVoenPaymentCalc));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.SearchBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DetailsBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.DockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.SearchDockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.FromDateValue = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ToDateValue = new DevExpress.XtraEditors.DateEdit();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemColorEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.CustomersGridControl = new DevExpress.XtraGrid.GridControl();
            this.CustomersGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Customer_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_FullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Voen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Avans = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Payed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Total = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_TypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).BeginInit();
            this.SearchDockPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar,
            this.bar1});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.DockManager = this.DockManager;
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton,
            this.SearchBarButton,
            this.DetailsBarButton,
            this.barEditItem1,
            this.barEditItem2,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barStaticItem3});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 12;
            this.BarManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemColorEdit1,
            this.repositoryItemPictureEdit1});
            this.BarManager.StatusBar = this.bar1;
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "Main menu";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DetailsBarButton)});
            this.ToolBar.OptionsBar.DrawBorder = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
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
            this.SearchBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.F));
            this.SearchBarButton.Name = "SearchBarButton";
            this.SearchBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.SearchBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SearchBarButton_ItemClick);
            // 
            // DetailsBarButton
            // 
            this.DetailsBarButton.Caption = "Detalları bax";
            this.DetailsBarButton.Id = 4;
            this.DetailsBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("DetailsBarButton.ImageOptions.Image")));
            this.DetailsBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("DetailsBarButton.ImageOptions.LargeImage")));
            this.DetailsBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.DetailsBarButton.Name = "DetailsBarButton";
            this.DetailsBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DetailsBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DetailsBarButton_ItemClick);
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 3";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "< 170.000,00";
            this.barStaticItem3.Id = 11;
            this.barStaticItem3.ItemAppearance.Normal.BackColor = System.Drawing.Color.White;
            this.barStaticItem3.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "170.000,00 - 200.000,00";
            this.barStaticItem1.Id = 9;
            this.barStaticItem1.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.barStaticItem1.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "> 200.000,00";
            this.barStaticItem2.Id = 10;
            this.barStaticItem2.ItemAppearance.Normal.BackColor = System.Drawing.Color.Red;
            this.barStaticItem2.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1543, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 595);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1543, 34);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 565);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1543, 30);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 565);
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
            this.SearchDockPanel.ID = new System.Guid("1dc250b2-6442-4123-ad63-a7a7922ac951");
            this.SearchDockPanel.Location = new System.Drawing.Point(0, 30);
            this.SearchDockPanel.Name = "SearchDockPanel";
            this.SearchDockPanel.OriginalSize = new System.Drawing.Size(200, 78);
            this.SearchDockPanel.Size = new System.Drawing.Size(1543, 78);
            this.SearchDockPanel.Text = "Ətraflı axtar";
            this.SearchDockPanel.ClosedPanel += new DevExpress.XtraBars.Docking.DockPanelEventHandler(this.SearchDockPanel_ClosedPanel);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.DateLabel);
            this.dockPanel1_Container.Controls.Add(this.FromDateValue);
            this.dockPanel1_Container.Controls.Add(this.labelControl1);
            this.dockPanel1_Container.Controls.Add(this.ToDateValue);
            this.dockPanel1_Container.Location = new System.Drawing.Point(5, 27);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1533, 44);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(75, 15);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(78, 16);
            this.DateLabel.TabIndex = 64;
            this.DateLabel.Text = "Tarix intervalı";
            // 
            // FromDateValue
            // 
            this.FromDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FromDateValue.EditValue = null;
            this.FromDateValue.Location = new System.Drawing.Point(198, 12);
            this.FromDateValue.MenuManager = this.BarManager;
            this.FromDateValue.Name = "FromDateValue";
            this.FromDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.FromDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.FromDateValue.Size = new System.Drawing.Size(100, 22);
            this.FromDateValue.TabIndex = 61;
            this.FromDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(304, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(5, 16);
            this.labelControl1.TabIndex = 63;
            this.labelControl1.Text = "-";
            // 
            // ToDateValue
            // 
            this.ToDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ToDateValue.EditValue = null;
            this.ToDateValue.Location = new System.Drawing.Point(315, 12);
            this.ToDateValue.MenuManager = this.BarManager;
            this.ToDateValue.Name = "ToDateValue";
            this.ToDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.ToDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.ToDateValue.Size = new System.Drawing.Size(100, 22);
            this.ToDateValue.TabIndex = 62;
            this.ToDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = this.repositoryItemColorEdit1;
            this.barEditItem1.Id = 5;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemColorEdit1
            // 
            this.repositoryItemColorEdit1.AutoHeight = false;
            this.repositoryItemColorEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
            // 
            // barEditItem2
            // 
            this.barEditItem2.Caption = "barEditItem2";
            this.barEditItem2.Edit = this.repositoryItemPictureEdit1;
            this.barEditItem2.Id = 6;
            this.barEditItem2.Name = "barEditItem2";
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.ZoomAccelerationFactor = 1D;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "170000 - 200000";
            this.barButtonItem1.Id = 7;
            this.barButtonItem1.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.barButtonItem1.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "> 200000";
            this.barButtonItem2.Id = 8;
            this.barButtonItem2.ItemAppearance.Normal.BackColor = System.Drawing.Color.Red;
            this.barButtonItem2.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // CustomersGridControl
            // 
            this.CustomersGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomersGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomersGridControl.Location = new System.Drawing.Point(0, 108);
            this.CustomersGridControl.MainView = this.CustomersGridView;
            this.CustomersGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomersGridControl.Name = "CustomersGridControl";
            this.CustomersGridControl.Size = new System.Drawing.Size(1543, 487);
            this.CustomersGridControl.TabIndex = 55;
            this.CustomersGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CustomersGridView});
            // 
            // CustomersGridView
            // 
            this.CustomersGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.CustomersGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.CustomersGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.CustomersGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.CustomersGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CustomersGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.CustomersGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CustomersGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CustomersGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.CustomersGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.CustomersGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CustomersGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.CustomersGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Customer_SS,
            this.Customer_ID,
            this.Customer_Type,
            this.Customer_FullName,
            this.Customer_Voen,
            this.Customer_Avans,
            this.Customer_Payed,
            this.Customer_Total,
            this.Customer_TypeID});
            this.CustomersGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.CustomersGridView.FormatRules.Add(gridFormatRule1);
            this.CustomersGridView.GridControl = this.CustomersGridControl;
            this.CustomersGridView.Name = "CustomersGridView";
            this.CustomersGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.CustomersGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.CustomersGridView.OptionsBehavior.Editable = false;
            this.CustomersGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.CustomersGridView.OptionsFind.FindDelay = 100;
            this.CustomersGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.CustomersGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.CustomersGridView.OptionsView.ShowFooter = true;
            this.CustomersGridView.OptionsView.ShowGroupPanel = false;
            this.CustomersGridView.OptionsView.ShowIndicator = false;
            this.CustomersGridView.OptionsView.ShowViewCaption = true;
            this.CustomersGridView.PaintStyleName = "Skin";
            this.CustomersGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.CustomersGridView.ViewCaption = "Müştərilərin siyahısı";
            this.CustomersGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.CustomersGridView_CustomDrawFooterCell);
            this.CustomersGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.CustomersGridView_RowCellStyle);
            this.CustomersGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.CustomersGridView_FocusedRowObjectChanged);
            this.CustomersGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.CustomersGridView_CustomUnboundColumnData);
            this.CustomersGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.CustomersGridView_CustomColumnDisplayText);
            this.CustomersGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomersGridView_MouseUp);
            this.CustomersGridView.DoubleClick += new System.EventHandler(this.CustomersGridView_DoubleClick);
            // 
            // Customer_SS
            // 
            this.Customer_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Customer_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Customer_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Customer_SS.Caption = "S/s";
            this.Customer_SS.FieldName = "SS";
            this.Customer_SS.Name = "Customer_SS";
            this.Customer_SS.OptionsColumn.FixedWidth = true;
            this.Customer_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Customer_SS.Visible = true;
            this.Customer_SS.VisibleIndex = 0;
            this.Customer_SS.Width = 50;
            // 
            // Customer_ID
            // 
            this.Customer_ID.Caption = "ID";
            this.Customer_ID.FieldName = "ID";
            this.Customer_ID.Name = "Customer_ID";
            // 
            // Customer_Type
            // 
            this.Customer_Type.Caption = "Tipi";
            this.Customer_Type.FieldName = "Customer_Type";
            this.Customer_Type.Name = "Customer_Type";
            this.Customer_Type.OptionsColumn.FixedWidth = true;
            this.Customer_Type.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.Customer_Type.Visible = true;
            this.Customer_Type.VisibleIndex = 1;
            this.Customer_Type.Width = 73;
            // 
            // Customer_FullName
            // 
            this.Customer_FullName.Caption = "Şəxsin tam adı";
            this.Customer_FullName.FieldName = "CUSTOMER_NAME";
            this.Customer_FullName.Name = "Customer_FullName";
            this.Customer_FullName.OptionsColumn.FixedWidth = true;
            this.Customer_FullName.Visible = true;
            this.Customer_FullName.VisibleIndex = 2;
            this.Customer_FullName.Width = 220;
            // 
            // Customer_Voen
            // 
            this.Customer_Voen.AppearanceCell.Options.UseTextOptions = true;
            this.Customer_Voen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Customer_Voen.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Customer_Voen.Caption = "VÖEN";
            this.Customer_Voen.FieldName = "VOEN";
            this.Customer_Voen.Name = "Customer_Voen";
            this.Customer_Voen.OptionsColumn.FixedWidth = true;
            this.Customer_Voen.Visible = true;
            this.Customer_Voen.VisibleIndex = 3;
            this.Customer_Voen.Width = 100;
            // 
            // Customer_Avans
            // 
            this.Customer_Avans.Caption = "Avans";
            this.Customer_Avans.DisplayFormat.FormatString = "n2";
            this.Customer_Avans.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Customer_Avans.FieldName = "ADVANCE_PAYMENT";
            this.Customer_Avans.Name = "Customer_Avans";
            this.Customer_Avans.OptionsColumn.FixedWidth = true;
            this.Customer_Avans.Visible = true;
            this.Customer_Avans.VisibleIndex = 4;
            this.Customer_Avans.Width = 100;
            // 
            // Customer_Payed
            // 
            this.Customer_Payed.Caption = "Ödəniş";
            this.Customer_Payed.DisplayFormat.FormatString = "n2";
            this.Customer_Payed.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Customer_Payed.FieldName = "PAYED_AMOUNT";
            this.Customer_Payed.Name = "Customer_Payed";
            this.Customer_Payed.OptionsColumn.FixedWidth = true;
            this.Customer_Payed.Visible = true;
            this.Customer_Payed.VisibleIndex = 5;
            this.Customer_Payed.Width = 100;
            // 
            // Customer_Total
            // 
            this.Customer_Total.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Customer_Total.AppearanceCell.Options.UseFont = true;
            this.Customer_Total.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Customer_Total.AppearanceHeader.Options.UseFont = true;
            this.Customer_Total.Caption = "CƏMİ";
            this.Customer_Total.DisplayFormat.FormatString = "n2";
            this.Customer_Total.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Customer_Total.FieldName = "Customer_Total";
            this.Customer_Total.Name = "Customer_Total";
            this.Customer_Total.OptionsColumn.FixedWidth = true;
            this.Customer_Total.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Customer_Total.Visible = true;
            this.Customer_Total.VisibleIndex = 6;
            this.Customer_Total.Width = 100;
            // 
            // Customer_TypeID
            // 
            this.Customer_TypeID.Caption = "TypeID";
            this.Customer_TypeID.FieldName = "CUS_TYPE";
            this.Customer_TypeID.Name = "Customer_TypeID";
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SearchBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DetailsBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // FVoenPaymentCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1543, 629);
            this.Controls.Add(this.CustomersGridControl);
            this.Controls.Add(this.SearchDockPanel);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FVoenPaymentCalc";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VÖEN-li fiziki müştərilərin dövriyyəsi";
            this.Load += new System.EventHandler(this.FVoenPaymentCalc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DockManager)).EndInit();
            this.SearchDockPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanel1_Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private DevExpress.XtraGrid.GridControl CustomersGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CustomersGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Type;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_FullName;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Voen;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Avans;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Payed;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Total;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_TypeID;
        private DevExpress.XtraBars.Docking.DockManager DockManager;
        private DevExpress.XtraBars.Docking.DockPanel SearchDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraEditors.DateEdit FromDateValue;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit ToDateValue;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem DetailsBarButton;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit repositoryItemColorEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
    }
}