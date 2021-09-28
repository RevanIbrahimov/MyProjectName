namespace CRS.Forms.Commons
{
    partial class FCommons
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCommons));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.OrderBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ShowFileBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.ExcelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PdfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RtfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.HtmlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TxtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CsvBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.MhtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ClosedCommonBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.AktiveCommonBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.CommonRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.OutRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ContractsGridControl = new DevExpress.XtraGrid.GridControl();
            this.ContractsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Contract_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_CommonDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_BankName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_CustomerFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_TotalDebt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Voen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_AccountNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_CommonAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_TempAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_TotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_OrderAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Conract_Debt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_InsertUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Conract_BankID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_CurrencyID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_StatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Contract_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.OrderBarButton,
            this.DeleteBarButton,
            this.RefreshBarButton,
            this.ShowFileBarButton,
            this.PrintBarButton,
            this.barSubItem1,
            this.ExcelBarButton,
            this.PdfBarButton,
            this.RtfBarButton,
            this.HtmlBarButton,
            this.TxtBarButton,
            this.CsvBarButton,
            this.MhtBarButton,
            this.ClosedCommonBarCheck,
            this.AktiveCommonBarCheck});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 16;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.CommonRibbonPage});
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1710, 179);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // OrderBarButton
            // 
            this.OrderBarButton.Caption = "Tələbnamələr";
            this.OrderBarButton.Id = 1;
            this.OrderBarButton.ImageOptions.Image = global::CRS.Properties.Resources.invoice_32;
            this.OrderBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.OrderBarButton.Name = "OrderBarButton";
            this.OrderBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.OrderBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.OrderBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.OrderBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 2;
            this.DeleteBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("DeleteBarButton.ImageOptions.Image")));
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 3;
            this.RefreshBarButton.ImageOptions.Image = global::CRS.Properties.Resources.Refresh_32;
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // ShowFileBarButton
            // 
            this.ShowFileBarButton.Caption = "Fayla bax";
            this.ShowFileBarButton.Id = 4;
            this.ShowFileBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ShowFileBarButton.ImageOptions.Image")));
            this.ShowFileBarButton.ItemAppearance.Disabled.Options.UseTextOptions = true;
            this.ShowFileBarButton.ItemAppearance.Disabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ShowFileBarButton.ItemAppearance.Hovered.Options.UseTextOptions = true;
            this.ShowFileBarButton.ItemAppearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ShowFileBarButton.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.ShowFileBarButton.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ShowFileBarButton.ItemAppearance.Pressed.Options.UseTextOptions = true;
            this.ShowFileBarButton.ItemAppearance.Pressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ShowFileBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.ShowFileBarButton.Name = "ShowFileBarButton";
            this.ShowFileBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.ShowFileBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ShowFileBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowFileBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 5;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "İxrac et";
            this.barSubItem1.Id = 6;
            this.barSubItem1.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.barSubItem1.ItemAppearance.Disabled.Options.UseTextOptions = true;
            this.barSubItem1.ItemAppearance.Disabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.barSubItem1.ItemAppearance.Hovered.Options.UseTextOptions = true;
            this.barSubItem1.ItemAppearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.barSubItem1.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.barSubItem1.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.barSubItem1.ItemAppearance.Pressed.Options.UseTextOptions = true;
            this.barSubItem1.ItemAppearance.Pressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PdfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RtfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.HtmlBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.TxtBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.CsvBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.MhtBarButton)});
            this.barSubItem1.MultiColumn = DevExpress.Utils.DefaultBoolean.True;
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.OptionsMultiColumn.ImageHorizontalAlignment = DevExpress.Utils.Drawing.ItemHorizontalAlignment.Right;
            this.barSubItem1.OptionsMultiColumn.ShowItemText = DevExpress.Utils.DefaultBoolean.True;
            this.barSubItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "Excel";
            this.ExcelBarButton.Id = 7;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButton
            // 
            this.PdfBarButton.Caption = "Pdf";
            this.PdfBarButton.Id = 8;
            this.PdfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButton.Name = "PdfBarButton";
            this.PdfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButton_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.Id = 9;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.Id = 10;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.Id = 11;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.Id = 12;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.Id = 13;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // ClosedCommonBarCheck
            // 
            this.ClosedCommonBarCheck.Caption = "Bağlanılmış sərəncamlar";
            this.ClosedCommonBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.ClosedCommonBarCheck.Id = 14;
            this.ClosedCommonBarCheck.Name = "ClosedCommonBarCheck";
            this.ClosedCommonBarCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ClosedCommonBarCheck_ItemClick);
            // 
            // AktiveCommonBarCheck
            // 
            this.AktiveCommonBarCheck.BindableChecked = true;
            this.AktiveCommonBarCheck.Caption = "Aktiv sərəncamlar";
            this.AktiveCommonBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.AktiveCommonBarCheck.Checked = true;
            this.AktiveCommonBarCheck.Id = 15;
            this.AktiveCommonBarCheck.Name = "AktiveCommonBarCheck";
            this.AktiveCommonBarCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ClosedCommonBarCheck_ItemClick);
            // 
            // CommonRibbonPage
            // 
            this.CommonRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.OutRibbonPageGroup,
            this.ribbonPageGroup1});
            this.CommonRibbonPage.Name = "CommonRibbonPage";
            this.CommonRibbonPage.Text = "Sərəncamlar";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.OrderBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.ShowFileBarButton);
            this.InfoRibbonPageGroup.Name = "InfoRibbonPageGroup";
            this.InfoRibbonPageGroup.Text = "Məlumat";
            // 
            // OutRibbonPageGroup
            // 
            this.OutRibbonPageGroup.ItemLinks.Add(this.PrintBarButton);
            this.OutRibbonPageGroup.ItemLinks.Add(this.barSubItem1);
            this.OutRibbonPageGroup.Name = "OutRibbonPageGroup";
            this.OutRibbonPageGroup.Text = "Çıxış";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.ClosedCommonBarCheck);
            this.ribbonPageGroup1.ItemLinks.Add(this.AktiveCommonBarCheck);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Filter";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 718);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1710, 40);
            // 
            // ContractsGridControl
            // 
            this.ContractsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContractsGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ContractsGridControl.Location = new System.Drawing.Point(0, 179);
            this.ContractsGridControl.MainView = this.ContractsGridView;
            this.ContractsGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ContractsGridControl.Name = "ContractsGridControl";
            this.ContractsGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemPictureEdit});
            this.ContractsGridControl.Size = new System.Drawing.Size(1710, 539);
            this.ContractsGridControl.TabIndex = 56;
            this.ContractsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ContractsGridView});
            // 
            // ContractsGridView
            // 
            this.ContractsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.ContractsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.ContractsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ContractsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ContractsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ContractsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ContractsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Contract_SS,
            this.Contract_CommonDate,
            this.Contract_Code,
            this.Contract_BankName,
            this.Contract_CustomerFullName,
            this.Contract_TotalDebt,
            this.Contract_Voen,
            this.Contract_AccountNumber,
            this.Contract_CommonAmount,
            this.Contract_TempAmount,
            this.Contract_TotalAmount,
            this.Contract_OrderAmount,
            this.Conract_Debt,
            this.Contract_CurrencyCode,
            this.Contract_InsertUser,
            this.Contract_InsertDate,
            this.Conract_BankID,
            this.Contract_CurrencyID,
            this.Contract_UsedUserID,
            this.Contract_StatusID,
            this.Contract_Amount});
            this.ContractsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.ContractsGridView.GridControl = this.ContractsGridControl;
            this.ContractsGridView.Name = "ContractsGridView";
            this.ContractsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.ContractsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.ContractsGridView.OptionsBehavior.Editable = false;
            this.ContractsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.ContractsGridView.OptionsFind.FindDelay = 100;
            this.ContractsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ContractsGridView.OptionsView.ColumnAutoWidth = false;
            this.ContractsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.ContractsGridView.OptionsView.ShowFooter = true;
            this.ContractsGridView.OptionsView.ShowGroupPanel = false;
            this.ContractsGridView.OptionsView.ShowIndicator = false;
            this.ContractsGridView.PaintStyleName = "Skin";
            this.ContractsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.ContractsGridView.ViewCaption = "Sərəncamların siyahısı";
            this.ContractsGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.ContractsGridView_RowCellStyle);
            this.ContractsGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.ContractsGridView_FocusedRowObjectChanged);
            this.ContractsGridView.ColumnFilterChanged += new System.EventHandler(this.ContractsGridView_ColumnFilterChanged);
            this.ContractsGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.ContractsGridView_CustomUnboundColumnData);
            this.ContractsGridView.PrintInitialize += new DevExpress.XtraGrid.Views.Base.PrintInitializeEventHandler(this.ContractsGridView_PrintInitialize);
            this.ContractsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContractsGridView_MouseUp);
            this.ContractsGridView.DoubleClick += new System.EventHandler(this.ContractsGridView_DoubleClick);
            // 
            // Contract_SS
            // 
            this.Contract_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_SS.Caption = "S/s";
            this.Contract_SS.FieldName = "Contract_SS";
            this.Contract_SS.Name = "Contract_SS";
            this.Contract_SS.OptionsColumn.FixedWidth = true;
            this.Contract_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Contract_SS", "{0}")});
            this.Contract_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Contract_SS.Visible = true;
            this.Contract_SS.VisibleIndex = 0;
            this.Contract_SS.Width = 45;
            // 
            // Contract_CommonDate
            // 
            this.Contract_CommonDate.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_CommonDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_CommonDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_CommonDate.Caption = "Tarix";
            this.Contract_CommonDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Contract_CommonDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Contract_CommonDate.FieldName = "COMMON_DATE";
            this.Contract_CommonDate.Name = "Contract_CommonDate";
            this.Contract_CommonDate.OptionsColumn.FixedWidth = true;
            this.Contract_CommonDate.Visible = true;
            this.Contract_CommonDate.VisibleIndex = 1;
            this.Contract_CommonDate.Width = 65;
            // 
            // Contract_Code
            // 
            this.Contract_Code.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_Code.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_Code.Caption = "Müqavilə";
            this.Contract_Code.FieldName = "CONTRACT_CODE";
            this.Contract_Code.Name = "Contract_Code";
            this.Contract_Code.OptionsColumn.FixedWidth = true;
            this.Contract_Code.Visible = true;
            this.Contract_Code.VisibleIndex = 2;
            this.Contract_Code.Width = 60;
            // 
            // Contract_BankName
            // 
            this.Contract_BankName.Caption = "Bank";
            this.Contract_BankName.FieldName = "BANK_NAME";
            this.Contract_BankName.Name = "Contract_BankName";
            this.Contract_BankName.Visible = true;
            this.Contract_BankName.VisibleIndex = 3;
            this.Contract_BankName.Width = 268;
            // 
            // Contract_CustomerFullName
            // 
            this.Contract_CustomerFullName.Caption = "Müştəri";
            this.Contract_CustomerFullName.FieldName = "CUSTOMER_NAME";
            this.Contract_CustomerFullName.Name = "Contract_CustomerFullName";
            this.Contract_CustomerFullName.Visible = true;
            this.Contract_CustomerFullName.VisibleIndex = 4;
            this.Contract_CustomerFullName.Width = 350;
            // 
            // Contract_TotalDebt
            // 
            this.Contract_TotalDebt.Caption = "Ümumi borc";
            this.Contract_TotalDebt.DisplayFormat.FormatString = "n2";
            this.Contract_TotalDebt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_TotalDebt.FieldName = "TOTAL_DEBT";
            this.Contract_TotalDebt.Name = "Contract_TotalDebt";
            this.Contract_TotalDebt.OptionsColumn.FixedWidth = true;
            this.Contract_TotalDebt.Visible = true;
            this.Contract_TotalDebt.VisibleIndex = 5;
            this.Contract_TotalDebt.Width = 100;
            // 
            // Contract_Voen
            // 
            this.Contract_Voen.Caption = "VÖEN";
            this.Contract_Voen.FieldName = "VOEN";
            this.Contract_Voen.Name = "Contract_Voen";
            this.Contract_Voen.OptionsColumn.FixedWidth = true;
            // 
            // Contract_AccountNumber
            // 
            this.Contract_AccountNumber.Caption = "Hesab";
            this.Contract_AccountNumber.FieldName = "ACCOUNT_NUMBER";
            this.Contract_AccountNumber.Name = "Contract_AccountNumber";
            this.Contract_AccountNumber.OptionsColumn.FixedWidth = true;
            this.Contract_AccountNumber.Width = 180;
            // 
            // Contract_CommonAmount
            // 
            this.Contract_CommonAmount.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Contract_CommonAmount.AppearanceCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Contract_CommonAmount.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Contract_CommonAmount.AppearanceCell.Options.UseBackColor = true;
            this.Contract_CommonAmount.AppearanceCell.Options.UseFont = true;
            this.Contract_CommonAmount.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Contract_CommonAmount.AppearanceHeader.Options.UseFont = true;
            this.Contract_CommonAmount.Caption = "Sərəncam";
            this.Contract_CommonAmount.DisplayFormat.FormatString = "n2";
            this.Contract_CommonAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_CommonAmount.FieldName = "AMOUNT";
            this.Contract_CommonAmount.Name = "Contract_CommonAmount";
            this.Contract_CommonAmount.OptionsColumn.FixedWidth = true;
            this.Contract_CommonAmount.ToolTip = "Sərəncam məbləği";
            this.Contract_CommonAmount.Visible = true;
            this.Contract_CommonAmount.VisibleIndex = 6;
            this.Contract_CommonAmount.Width = 100;
            // 
            // Contract_TempAmount
            // 
            this.Contract_TempAmount.Caption = "Müvəqqəti düzəliş";
            this.Contract_TempAmount.DisplayFormat.FormatString = "n2";
            this.Contract_TempAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_TempAmount.FieldName = "TEMP_AMOUNT";
            this.Contract_TempAmount.Name = "Contract_TempAmount";
            this.Contract_TempAmount.OptionsColumn.FixedWidth = true;
            this.Contract_TempAmount.Visible = true;
            this.Contract_TempAmount.VisibleIndex = 7;
            this.Contract_TempAmount.Width = 100;
            // 
            // Contract_TotalAmount
            // 
            this.Contract_TotalAmount.Caption = "Yekun Sərəncam";
            this.Contract_TotalAmount.DisplayFormat.FormatString = "n2";
            this.Contract_TotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_TotalAmount.FieldName = "TOTAL_AMOUNT";
            this.Contract_TotalAmount.Name = "Contract_TotalAmount";
            this.Contract_TotalAmount.OptionsColumn.FixedWidth = true;
            this.Contract_TotalAmount.Visible = true;
            this.Contract_TotalAmount.VisibleIndex = 8;
            this.Contract_TotalAmount.Width = 100;
            // 
            // Contract_OrderAmount
            // 
            this.Contract_OrderAmount.Caption = "Tələbnamələr";
            this.Contract_OrderAmount.DisplayFormat.FormatString = "n2";
            this.Contract_OrderAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_OrderAmount.FieldName = "ORDER_AMOUNT";
            this.Contract_OrderAmount.Name = "Contract_OrderAmount";
            this.Contract_OrderAmount.OptionsColumn.FixedWidth = true;
            this.Contract_OrderAmount.ToolTip = "Göndərilmiş tələbnamələr";
            this.Contract_OrderAmount.Visible = true;
            this.Contract_OrderAmount.VisibleIndex = 9;
            this.Contract_OrderAmount.Width = 100;
            // 
            // Conract_Debt
            // 
            this.Conract_Debt.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Conract_Debt.AppearanceCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Conract_Debt.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Conract_Debt.AppearanceCell.Options.UseBackColor = true;
            this.Conract_Debt.AppearanceCell.Options.UseFont = true;
            this.Conract_Debt.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Conract_Debt.AppearanceHeader.Options.UseFont = true;
            this.Conract_Debt.Caption = "Qalıq";
            this.Conract_Debt.DisplayFormat.FormatString = "n2";
            this.Conract_Debt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Conract_Debt.FieldName = "Conract_Debt";
            this.Conract_Debt.Name = "Conract_Debt";
            this.Conract_Debt.OptionsColumn.FixedWidth = true;
            this.Conract_Debt.UnboundExpression = "[TOTAL_AMOUNT] - [ORDER_AMOUNT]";
            this.Conract_Debt.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.Conract_Debt.Visible = true;
            this.Conract_Debt.VisibleIndex = 10;
            this.Conract_Debt.Width = 100;
            // 
            // Contract_CurrencyCode
            // 
            this.Contract_CurrencyCode.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_CurrencyCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_CurrencyCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_CurrencyCode.Caption = "Valyutası";
            this.Contract_CurrencyCode.FieldName = "CURRENCY_CODE";
            this.Contract_CurrencyCode.Name = "Contract_CurrencyCode";
            this.Contract_CurrencyCode.OptionsColumn.FixedWidth = true;
            this.Contract_CurrencyCode.Visible = true;
            this.Contract_CurrencyCode.VisibleIndex = 11;
            this.Contract_CurrencyCode.Width = 55;
            // 
            // Contract_InsertUser
            // 
            this.Contract_InsertUser.Caption = "Yaradan istifadəçi";
            this.Contract_InsertUser.FieldName = "INSERT_USER";
            this.Contract_InsertUser.Name = "Contract_InsertUser";
            this.Contract_InsertUser.OptionsColumn.FixedWidth = true;
            this.Contract_InsertUser.Visible = true;
            this.Contract_InsertUser.VisibleIndex = 12;
            this.Contract_InsertUser.Width = 180;
            // 
            // Contract_InsertDate
            // 
            this.Contract_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Contract_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Contract_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Contract_InsertDate.Caption = "Yaradılma vaxtı";
            this.Contract_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Contract_InsertDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Contract_InsertDate.FieldName = "INSERT_DATE";
            this.Contract_InsertDate.Name = "Contract_InsertDate";
            this.Contract_InsertDate.OptionsColumn.FixedWidth = true;
            this.Contract_InsertDate.Visible = true;
            this.Contract_InsertDate.VisibleIndex = 13;
            this.Contract_InsertDate.Width = 110;
            // 
            // Conract_BankID
            // 
            this.Conract_BankID.Caption = "BankID";
            this.Conract_BankID.FieldName = "BANK_ID";
            this.Conract_BankID.Name = "Conract_BankID";
            this.Conract_BankID.OptionsColumn.AllowShowHide = false;
            // 
            // Contract_CurrencyID
            // 
            this.Contract_CurrencyID.Caption = "CurrencyID";
            this.Contract_CurrencyID.FieldName = "CURRENCY_ID";
            this.Contract_CurrencyID.Name = "Contract_CurrencyID";
            this.Contract_CurrencyID.OptionsColumn.AllowShowHide = false;
            // 
            // Contract_UsedUserID
            // 
            this.Contract_UsedUserID.Caption = "UsedUserID";
            this.Contract_UsedUserID.FieldName = "USED_USER_ID";
            this.Contract_UsedUserID.Name = "Contract_UsedUserID";
            this.Contract_UsedUserID.OptionsColumn.AllowShowHide = false;
            // 
            // Contract_StatusID
            // 
            this.Contract_StatusID.Caption = "StatusID";
            this.Contract_StatusID.FieldName = "STATUS_ID";
            this.Contract_StatusID.Name = "Contract_StatusID";
            this.Contract_StatusID.OptionsColumn.AllowShowHide = false;
            // 
            // Contract_Amount
            // 
            this.Contract_Amount.Caption = "Müqavilənin dəyəri";
            this.Contract_Amount.DisplayFormat.FormatString = "n2";
            this.Contract_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Contract_Amount.FieldName = "CONTRACT_AMOUNT";
            this.Contract_Amount.Name = "Contract_Amount";
            this.Contract_Amount.OptionsColumn.AllowShowHide = false;
            // 
            // RepositoryItemPictureEdit
            // 
            this.RepositoryItemPictureEdit.InitialImage = global::CRS.Properties.Resources.notes_16;
            this.RepositoryItemPictureEdit.Name = "RepositoryItemPictureEdit";
            this.RepositoryItemPictureEdit.NullText = " ";
            this.RepositoryItemPictureEdit.ZoomAccelerationFactor = 1D;
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.OrderBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.ShowFileBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton);
            this.PopupMenu.ItemLinks.Add(this.barSubItem1);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.ribbon;
            // 
            // FCommons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1710, 758);
            this.Controls.Add(this.ContractsGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "FCommons";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Sərəncamlar";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.FCommons_Activated);
            this.Load += new System.EventHandler(this.FCommons_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContractsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage CommonRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup InfoRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem OrderBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem ShowFileBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup OutRibbonPageGroup;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
        private DevExpress.XtraGrid.GridControl ContractsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ContractsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_CustomerFullName;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Code;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_CommonDate;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_CommonAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_CurrencyCode;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_BankName;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Voen;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_AccountNumber;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_OrderAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Conract_Debt;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_InsertUser;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_InsertDate;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn Conract_BankID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_CurrencyID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_UsedUserID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_StatusID;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_TotalDebt;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_Amount;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarCheckItem ClosedCommonBarCheck;
        private DevExpress.XtraBars.BarCheckItem AktiveCommonBarCheck;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_TempAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Contract_TotalAmount;
    }
}