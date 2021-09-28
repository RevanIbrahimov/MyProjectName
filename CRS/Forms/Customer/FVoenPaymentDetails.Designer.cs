namespace CRS.Forms.Customer
{
    partial class FVoenPaymentDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FVoenPaymentDetails));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression2 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PaymentBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.SearchBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CustomersGridControl = new DevExpress.XtraGrid.GridControl();
            this.CustomersGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Customer_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_FullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Voen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Avans = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Payed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_Total = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Customer_ContractID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.Customer_Period = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton,
            this.SearchBarButton,
            this.PaymentBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 5;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.PaymentBarButton)});
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
            // PaymentBarButton
            // 
            this.PaymentBarButton.Caption = "Ödənişlər";
            this.PaymentBarButton.Id = 4;
            this.PaymentBarButton.ImageOptions.Image = global::CRS.Properties.Resources.payment_16;
            this.PaymentBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.PaymentBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.PaymentBarButton.Name = "PaymentBarButton";
            this.PaymentBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PaymentBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PaymentBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1321, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 660);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1321, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 630);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1321, 30);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 630);
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
            // 
            // CustomersGridControl
            // 
            this.CustomersGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomersGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomersGridControl.Location = new System.Drawing.Point(0, 30);
            this.CustomersGridControl.MainView = this.CustomersGridView;
            this.CustomersGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomersGridControl.Name = "CustomersGridControl";
            this.CustomersGridControl.Size = new System.Drawing.Size(1321, 630);
            this.CustomersGridControl.TabIndex = 56;
            this.CustomersGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.CustomersGridView});
            // 
            // CustomersGridView
            // 
            this.CustomersGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.CustomersGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.CustomersGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.CustomersGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
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
            this.Customer_FullName,
            this.Customer_Voen,
            this.Customer_Period,
            this.Customer_Avans,
            this.Customer_Payed,
            this.Customer_Total,
            this.Customer_ContractID});
            this.CustomersGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule2.ApplyToRow = true;
            gridFormatRule2.Name = "Format0";
            formatConditionRuleExpression2.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression2.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule2.Rule = formatConditionRuleExpression2;
            this.CustomersGridView.FormatRules.Add(gridFormatRule2);
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
            this.CustomersGridView.PaintStyleName = "Skin";
            this.CustomersGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.CustomersGridView.ViewCaption = "Müştərilərin siyahısı";
            this.CustomersGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.CustomersGridView_CustomDrawFooterCell);
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
            // Customer_FullName
            // 
            this.Customer_FullName.Caption = "Müqavilə";
            this.Customer_FullName.FieldName = "CONTRACT_CODE";
            this.Customer_FullName.Name = "Customer_FullName";
            this.Customer_FullName.OptionsColumn.FixedWidth = true;
            this.Customer_FullName.Visible = true;
            this.Customer_FullName.VisibleIndex = 1;
            this.Customer_FullName.Width = 70;
            // 
            // Customer_Voen
            // 
            this.Customer_Voen.Caption = "Lizinq predmeti";
            this.Customer_Voen.FieldName = "HOSTAGE";
            this.Customer_Voen.Name = "Customer_Voen";
            this.Customer_Voen.Visible = true;
            this.Customer_Voen.VisibleIndex = 2;
            this.Customer_Voen.Width = 824;
            // 
            // Customer_Avans
            // 
            this.Customer_Avans.Caption = "Avans";
            this.Customer_Avans.DisplayFormat.FormatString = "n2";
            this.Customer_Avans.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Customer_Avans.FieldName = "ADVANCE_PAYMENT";
            this.Customer_Avans.Name = "Customer_Avans";
            this.Customer_Avans.OptionsColumn.FixedWidth = true;
            this.Customer_Avans.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ADVANCE_PAYMENT", "{0:n2}")});
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
            this.Customer_Payed.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PAYED_AMOUNT", "{0:n2}")});
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
            this.Customer_Total.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Customer_Total", "{0:n2}")});
            this.Customer_Total.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Customer_Total.Visible = true;
            this.Customer_Total.VisibleIndex = 6;
            this.Customer_Total.Width = 100;
            // 
            // Customer_ContractID
            // 
            this.Customer_ContractID.Caption = "ContractID";
            this.Customer_ContractID.FieldName = "CONTRACT_ID";
            this.Customer_ContractID.Name = "Customer_ContractID";
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PaymentBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // Customer_Period
            // 
            this.Customer_Period.AppearanceCell.Options.UseTextOptions = true;
            this.Customer_Period.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Customer_Period.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Customer_Period.Caption = "Tarix intervalı";
            this.Customer_Period.FieldName = "PERIOD";
            this.Customer_Period.Name = "Customer_Period";
            this.Customer_Period.OptionsColumn.FixedWidth = true;
            this.Customer_Period.Visible = true;
            this.Customer_Period.VisibleIndex = 3;
            this.Customer_Period.Width = 130;
            // 
            // FVoenPaymentDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 660);
            this.Controls.Add(this.CustomersGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FVoenPaymentDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FVoenPaymentDetails";
            this.Load += new System.EventHandler(this.FVoenPaymentDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarButtonItem PaymentBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem SearchBarButton;
        private DevExpress.XtraGrid.GridControl CustomersGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView CustomersGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_FullName;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Voen;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Avans;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Payed;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Total;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_ContractID;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn Customer_Period;
    }
}