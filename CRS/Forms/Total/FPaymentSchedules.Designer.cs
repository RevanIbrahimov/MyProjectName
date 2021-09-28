namespace CRS.Forms.Total
{
    partial class FPaymentSchedules
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
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPaymentSchedules));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.Schedules_BasicAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExtendBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.FirstBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PaymentSchedulesGridControl = new DevExpress.XtraGrid.GridControl();
            this.PaymentSchedulesGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Schedules_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_MontlyPayment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_InterestAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_IsChangeDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_OrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_Version = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Schedules_VersionDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // Schedules_BasicAmount
            // 
            this.Schedules_BasicAmount.Caption = "Əsas məbləğ";
            this.Schedules_BasicAmount.DisplayFormat.FormatString = "n2";
            this.Schedules_BasicAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Schedules_BasicAmount.FieldName = "BASIC_AMOUNT";
            this.Schedules_BasicAmount.Name = "Schedules_BasicAmount";
            this.Schedules_BasicAmount.OptionsColumn.FixedWidth = true;
            this.Schedules_BasicAmount.Visible = true;
            this.Schedules_BasicAmount.VisibleIndex = 3;
            this.Schedules_BasicAmount.Width = 100;
            // 
            // Schedules_Date
            // 
            this.Schedules_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Schedules_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Schedules_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Schedules_Date.Caption = "Tarix";
            this.Schedules_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Schedules_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Schedules_Date.FieldName = "MONTH_DATE";
            this.Schedules_Date.Name = "Schedules_Date";
            this.Schedules_Date.OptionsColumn.FixedWidth = true;
            this.Schedules_Date.Visible = true;
            this.Schedules_Date.VisibleIndex = 1;
            this.Schedules_Date.Width = 60;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 594);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(831, 62);
            this.PanelOption.TabIndex = 50;
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
            this.BCancel.Location = new System.Drawing.Point(727, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
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
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton,
            this.ExtendBarCheck,
            this.FirstBarCheck});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 5;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.ExtendBarCheck),
            new DevExpress.XtraBars.LinkPersistInfo(this.FirstBarCheck)});
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
            this.ExportBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.E));
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // ExtendBarCheck
            // 
            this.ExtendBarCheck.Caption = "Uzadılmış";
            this.ExtendBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.ExtendBarCheck.Id = 3;
            this.ExtendBarCheck.Name = "ExtendBarCheck";
            this.ExtendBarCheck.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // FirstBarCheck
            // 
            this.FirstBarCheck.Caption = "Əsas";
            this.FirstBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.FirstBarCheck.Id = 4;
            this.FirstBarCheck.Name = "FirstBarCheck";
            this.FirstBarCheck.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(831, 32);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(831, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 656);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(831, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 656);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(831, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 656);
            // 
            // PaymentSchedulesGridControl
            // 
            this.PaymentSchedulesGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaymentSchedulesGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PaymentSchedulesGridControl.Location = new System.Drawing.Point(0, 32);
            this.PaymentSchedulesGridControl.MainView = this.PaymentSchedulesGridView;
            this.PaymentSchedulesGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PaymentSchedulesGridControl.Name = "PaymentSchedulesGridControl";
            this.PaymentSchedulesGridControl.Size = new System.Drawing.Size(831, 562);
            this.PaymentSchedulesGridControl.TabIndex = 62;
            this.PaymentSchedulesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PaymentSchedulesGridView});
            // 
            // PaymentSchedulesGridView
            // 
            this.PaymentSchedulesGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PaymentSchedulesGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PaymentSchedulesGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PaymentSchedulesGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Schedules_SS,
            this.Schedules_ID,
            this.Schedules_Date,
            this.Schedules_MontlyPayment,
            this.Schedules_BasicAmount,
            this.Schedules_InterestAmount,
            this.Schedules_Debt,
            this.Schedules_CurrencyCode,
            this.Schedules_IsChangeDate,
            this.Schedules_OrderID,
            this.Schedules_Version,
            this.Schedules_VersionDescription});
            this.PaymentSchedulesGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Column = this.Schedules_BasicAmount;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.PredefinedName = "Red Text";
            formatConditionRuleValue1.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            gridFormatRule1.Rule = formatConditionRuleValue1;
            gridFormatRule2.Column = this.Schedules_Date;
            gridFormatRule2.Name = "Format1";
            formatConditionRuleExpression1.Expression = "[IS_CHANGE_DATE] = 1";
            formatConditionRuleExpression1.PredefinedName = "Red Text";
            gridFormatRule2.Rule = formatConditionRuleExpression1;
            this.PaymentSchedulesGridView.FormatRules.Add(gridFormatRule1);
            this.PaymentSchedulesGridView.FormatRules.Add(gridFormatRule2);
            this.PaymentSchedulesGridView.GridControl = this.PaymentSchedulesGridControl;
            this.PaymentSchedulesGridView.GroupCount = 1;
            this.PaymentSchedulesGridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "INTEREST_AMOUNT", this.Schedules_InterestAmount, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MONTHLY_PAYMENT", this.Schedules_MontlyPayment, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "BASIC_AMOUNT", this.Schedules_BasicAmount, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", this.Schedules_SS, "{0}")});
            this.PaymentSchedulesGridView.Name = "PaymentSchedulesGridView";
            this.PaymentSchedulesGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PaymentSchedulesGridView.OptionsBehavior.Editable = false;
            this.PaymentSchedulesGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PaymentSchedulesGridView.OptionsFind.FindDelay = 100;
            this.PaymentSchedulesGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PaymentSchedulesGridView.OptionsView.ShowGroupPanel = false;
            this.PaymentSchedulesGridView.OptionsView.ShowIndicator = false;
            this.PaymentSchedulesGridView.PaintStyleName = "Skin";
            this.PaymentSchedulesGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PaymentSchedulesGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Schedules_VersionDescription, DevExpress.Data.ColumnSortOrder.Descending)});
            this.PaymentSchedulesGridView.ViewCaption = "11300";
            this.PaymentSchedulesGridView.CustomDrawRowFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.PaymentSchedulesGridView_CustomDrawRowFooterCell);
            this.PaymentSchedulesGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.PaymentSchedulesGridView_CustomDrawFooterCell);
            this.PaymentSchedulesGridView.CustomDrawGroupRow += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(this.PaymentSchedulesGridView_CustomDrawGroupRow);
            this.PaymentSchedulesGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.PaymentSchedulesGridView_RowCellStyle);
            this.PaymentSchedulesGridView.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.PaymentSchedulesGridView_RowStyle);
            this.PaymentSchedulesGridView.EndGrouping += new System.EventHandler(this.PaymentSchedulesGridView_EndGrouping);
            this.PaymentSchedulesGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.PaymentSchedulesGridView_CustomUnboundColumnData);
            this.PaymentSchedulesGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaymentSchedulesGridView_MouseUp);
            // 
            // Schedules_SS
            // 
            this.Schedules_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Schedules_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Schedules_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Schedules_SS.Caption = "S/s";
            this.Schedules_SS.FieldName = "SS";
            this.Schedules_SS.Name = "Schedules_SS";
            this.Schedules_SS.OptionsColumn.FixedWidth = true;
            this.Schedules_SS.Visible = true;
            this.Schedules_SS.VisibleIndex = 0;
            this.Schedules_SS.Width = 40;
            // 
            // Schedules_ID
            // 
            this.Schedules_ID.Caption = "ID";
            this.Schedules_ID.FieldName = "ID";
            this.Schedules_ID.Name = "Schedules_ID";
            // 
            // Schedules_MontlyPayment
            // 
            this.Schedules_MontlyPayment.Caption = "Aylıq ödəniş";
            this.Schedules_MontlyPayment.DisplayFormat.FormatString = "n2";
            this.Schedules_MontlyPayment.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Schedules_MontlyPayment.FieldName = "MONTHLY_PAYMENT";
            this.Schedules_MontlyPayment.Name = "Schedules_MontlyPayment";
            this.Schedules_MontlyPayment.OptionsColumn.FixedWidth = true;
            this.Schedules_MontlyPayment.Visible = true;
            this.Schedules_MontlyPayment.VisibleIndex = 2;
            this.Schedules_MontlyPayment.Width = 100;
            // 
            // Schedules_InterestAmount
            // 
            this.Schedules_InterestAmount.Caption = "Faiz məbləği";
            this.Schedules_InterestAmount.DisplayFormat.FormatString = "n2";
            this.Schedules_InterestAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Schedules_InterestAmount.FieldName = "INTEREST_AMOUNT";
            this.Schedules_InterestAmount.Name = "Schedules_InterestAmount";
            this.Schedules_InterestAmount.OptionsColumn.FixedWidth = true;
            this.Schedules_InterestAmount.Visible = true;
            this.Schedules_InterestAmount.VisibleIndex = 4;
            this.Schedules_InterestAmount.Width = 100;
            // 
            // Schedules_Debt
            // 
            this.Schedules_Debt.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Schedules_Debt.AppearanceCell.Options.UseFont = true;
            this.Schedules_Debt.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Schedules_Debt.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.Schedules_Debt.AppearanceHeader.Options.UseFont = true;
            this.Schedules_Debt.AppearanceHeader.Options.UseForeColor = true;
            this.Schedules_Debt.Caption = "Cari balans";
            this.Schedules_Debt.DisplayFormat.FormatString = "n2";
            this.Schedules_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Schedules_Debt.FieldName = "DEBT";
            this.Schedules_Debt.Name = "Schedules_Debt";
            this.Schedules_Debt.OptionsColumn.FixedWidth = true;
            this.Schedules_Debt.Visible = true;
            this.Schedules_Debt.VisibleIndex = 5;
            this.Schedules_Debt.Width = 100;
            // 
            // Schedules_CurrencyCode
            // 
            this.Schedules_CurrencyCode.AppearanceCell.Options.UseTextOptions = true;
            this.Schedules_CurrencyCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Schedules_CurrencyCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Schedules_CurrencyCode.Caption = "Valyuta";
            this.Schedules_CurrencyCode.FieldName = "CURRENCY_CODE";
            this.Schedules_CurrencyCode.Name = "Schedules_CurrencyCode";
            this.Schedules_CurrencyCode.OptionsColumn.FixedWidth = true;
            this.Schedules_CurrencyCode.Visible = true;
            this.Schedules_CurrencyCode.VisibleIndex = 6;
            // 
            // Schedules_IsChangeDate
            // 
            this.Schedules_IsChangeDate.Caption = "IsChangeDate";
            this.Schedules_IsChangeDate.FieldName = "IS_CHANGE_DATE";
            this.Schedules_IsChangeDate.Name = "Schedules_IsChangeDate";
            // 
            // Schedules_OrderID
            // 
            this.Schedules_OrderID.Caption = "OrderID";
            this.Schedules_OrderID.FieldName = "ORDER_ID";
            this.Schedules_OrderID.Name = "Schedules_OrderID";
            // 
            // Schedules_Version
            // 
            this.Schedules_Version.Caption = "Versiya";
            this.Schedules_Version.FieldName = "VERSION";
            this.Schedules_Version.Name = "Schedules_Version";
            // 
            // Schedules_VersionDescription
            // 
            this.Schedules_VersionDescription.Caption = "Qrafikin növü";
            this.Schedules_VersionDescription.FieldName = "VERSION_DESCRIPTION";
            this.Schedules_VersionDescription.Name = "Schedules_VersionDescription";
            this.Schedules_VersionDescription.Visible = true;
            this.Schedules_VersionDescription.VisibleIndex = 7;
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // FPaymentSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 656);
            this.Controls.Add(this.PaymentSchedulesGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimizeBox = false;
            this.Name = "FPaymentSchedules";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Normal ödəniş qrafiki";
            this.Load += new System.EventHandler(this.FPaymentSchedules_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentSchedulesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl PaymentSchedulesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PaymentSchedulesGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_MontlyPayment;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_BasicAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_InterestAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_Debt;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_CurrencyCode;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_IsChangeDate;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_OrderID;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_Version;
        private DevExpress.XtraGrid.Columns.GridColumn Schedules_VersionDescription;
        private DevExpress.XtraBars.BarCheckItem ExtendBarCheck;
        private DevExpress.XtraBars.BarCheckItem FirstBarCheck;
    }
}