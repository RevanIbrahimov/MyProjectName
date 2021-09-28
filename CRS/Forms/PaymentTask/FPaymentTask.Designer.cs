namespace CRS.Forms.PaymentTask
{
    partial class FPaymentTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPaymentTask));
            this.PaymentTaskRibbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.ExcelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PdfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RtfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.HtmlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TxtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CsvBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.MhtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TemplatesBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.TaskTypeBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.AcceptorBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.VatBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PaymentTaskRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.OutRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.TaskGridControl = new DevExpress.XtraGrid.GridControl();
            this.TaskGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Task_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_TypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_PayingBank = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_AcceptorName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_InsertUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_UpdateUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_UpdateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Task_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTaskRibbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PaymentTaskRibbon
            // 
            this.PaymentTaskRibbon.ExpandCollapseItem.Id = 0;
            this.PaymentTaskRibbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.PaymentTaskRibbon.ExpandCollapseItem,
            this.NewBarButton,
            this.EditBarButton,
            this.DeleteBarButton,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.ExcelBarButton,
            this.PdfBarButton,
            this.RtfBarButton,
            this.HtmlBarButton,
            this.TxtBarButton,
            this.CsvBarButton,
            this.MhtBarButton,
            this.TemplatesBarButton,
            this.barSubItem1,
            this.TaskTypeBarButton,
            this.AcceptorBarButton,
            this.VatBarButton});
            this.PaymentTaskRibbon.Location = new System.Drawing.Point(0, 0);
            this.PaymentTaskRibbon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PaymentTaskRibbon.MaxItemId = 5;
            this.PaymentTaskRibbon.Name = "PaymentTaskRibbon";
            this.PaymentTaskRibbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.PaymentTaskRibbonPage});
            this.PaymentTaskRibbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.PaymentTaskRibbon.ShowQatLocationSelector = false;
            this.PaymentTaskRibbon.ShowToolbarCustomizeItem = false;
            this.PaymentTaskRibbon.Size = new System.Drawing.Size(1746, 179);
            this.PaymentTaskRibbon.StatusBar = this.ribbonStatusBar;
            this.PaymentTaskRibbon.Toolbar.ShowCustomizeItem = false;
            this.PaymentTaskRibbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // NewBarButton
            // 
            this.NewBarButton.Caption = "Yeni";
            this.NewBarButton.Id = 1;
            this.NewBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("NewBarButton.ImageOptions.Image")));
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Id = 2;
            this.EditBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("EditBarButton.ImageOptions.Image")));
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 3;
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
            this.RefreshBarButton.Id = 4;
            this.RefreshBarButton.ImageOptions.Image = global::CRS.Properties.Resources.Refresh_32;
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap";
            this.PrintBarButton.Id = 5;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarSubItem
            // 
            this.ExportBarSubItem.Caption = "İxrac";
            this.ExportBarSubItem.Id = 6;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.ExportBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PdfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RtfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.HtmlBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.TxtBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.CsvBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.MhtBarButton)});
            this.ExportBarSubItem.MultiColumn = DevExpress.Utils.DefaultBoolean.False;
            this.ExportBarSubItem.Name = "ExportBarSubItem";
            this.ExportBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
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
            this.MhtBarButton.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // TemplatesBarButton
            // 
            this.TemplatesBarButton.Caption = "Şablon formaları";
            this.TemplatesBarButton.Id = 1;
            this.TemplatesBarButton.ImageOptions.Image = global::CRS.Properties.Resources.template_32;
            this.TemplatesBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T));
            this.TemplatesBarButton.Name = "TemplatesBarButton";
            this.TemplatesBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.TemplatesBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.TemplatesBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TemplatesBarButton_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Soraqçalar";
            this.barSubItem1.Id = 1;
            this.barSubItem1.ImageOptions.Image = global::CRS.Properties.Resources.dictionary_32;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.TaskTypeBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.AcceptorBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.VatBarButton)});
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // TaskTypeBarButton
            // 
            this.TaskTypeBarButton.Caption = "Tapşırığın növləri";
            this.TaskTypeBarButton.Id = 2;
            this.TaskTypeBarButton.ImageOptions.Image = global::CRS.Properties.Resources.type_32;
            this.TaskTypeBarButton.Name = "TaskTypeBarButton";
            this.TaskTypeBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TaskTypeBarButton_ItemClick);
            // 
            // AcceptorBarButton
            // 
            this.AcceptorBarButton.Caption = "Alan tərəflər";
            this.AcceptorBarButton.Id = 3;
            this.AcceptorBarButton.ImageOptions.Image = global::CRS.Properties.Resources.coins_in_hand_32;
            this.AcceptorBarButton.Name = "AcceptorBarButton";
            this.AcceptorBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AcceptorBarButton_ItemClick);
            // 
            // VatBarButton
            // 
            this.VatBarButton.Caption = "ƏDV-ni alan bank";
            this.VatBarButton.Id = 4;
            this.VatBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("VatBarButton.ImageOptions.Image")));
            this.VatBarButton.Name = "VatBarButton";
            this.VatBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.VatBarButton_ItemClick);
            // 
            // PaymentTaskRibbonPage
            // 
            this.PaymentTaskRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.OutRibbonPageGroup});
            this.PaymentTaskRibbonPage.Name = "PaymentTaskRibbonPage";
            this.PaymentTaskRibbonPage.Text = "Ödəniş tapşırıqları";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.NewBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.EditBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.TemplatesBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.barSubItem1);
            this.InfoRibbonPageGroup.Name = "InfoRibbonPageGroup";
            this.InfoRibbonPageGroup.Text = "Məlumat";
            // 
            // OutRibbonPageGroup
            // 
            this.OutRibbonPageGroup.ItemLinks.Add(this.PrintBarButton);
            this.OutRibbonPageGroup.ItemLinks.Add(this.ExportBarSubItem);
            this.OutRibbonPageGroup.Name = "OutRibbonPageGroup";
            this.OutRibbonPageGroup.Text = "Çıxış";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 652);
            this.ribbonStatusBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.PaymentTaskRibbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1746, 40);
            // 
            // TaskGridControl
            // 
            this.TaskGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TaskGridControl.Location = new System.Drawing.Point(0, 179);
            this.TaskGridControl.MainView = this.TaskGridView;
            this.TaskGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TaskGridControl.Name = "TaskGridControl";
            this.TaskGridControl.Size = new System.Drawing.Size(1746, 473);
            this.TaskGridControl.TabIndex = 79;
            this.TaskGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.TaskGridView});
            // 
            // TaskGridView
            // 
            this.TaskGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.TaskGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.TaskGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.TaskGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TaskGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TaskGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.TaskGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TaskGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TaskGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.TaskGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.TaskGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.TaskGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.TaskGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.TaskGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.TaskGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.TaskGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TaskGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.TaskGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Task_SS,
            this.Task_ID,
            this.Task_Code,
            this.Task_TypeName,
            this.Task_Date,
            this.Task_Amount,
            this.Task_CurrencyCode,
            this.Task_PayingBank,
            this.Task_AcceptorName,
            this.Task_InsertUser,
            this.Task_InsertDate,
            this.Task_UpdateUser,
            this.Task_UpdateDate,
            this.Task_UsedUserID});
            this.TaskGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.TaskGridView.GridControl = this.TaskGridControl;
            this.TaskGridView.Name = "TaskGridView";
            this.TaskGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.TaskGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.TaskGridView.OptionsBehavior.Editable = false;
            this.TaskGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.TaskGridView.OptionsFind.FindDelay = 100;
            this.TaskGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.TaskGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.TaskGridView.OptionsView.ShowFooter = true;
            this.TaskGridView.OptionsView.ShowGroupPanel = false;
            this.TaskGridView.OptionsView.ShowIndicator = false;
            this.TaskGridView.PaintStyleName = "Skin";
            this.TaskGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.TaskGridView.ViewCaption = "Ödəniş tapşırıqları";
            this.TaskGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.TaskGridView_RowCellStyle);
            this.TaskGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.TaskGridView_FocusedRowObjectChanged);
            this.TaskGridView.ColumnFilterChanged += new System.EventHandler(this.TaskGridView_ColumnFilterChanged);
            this.TaskGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.TaskGridView_CustomUnboundColumnData);
            this.TaskGridView.PrintInitialize += new DevExpress.XtraGrid.Views.Base.PrintInitializeEventHandler(this.TaskGridView_PrintInitialize);
            this.TaskGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TaskGridView_MouseUp);
            this.TaskGridView.DoubleClick += new System.EventHandler(this.TaskGridView_DoubleClick);
            // 
            // Task_SS
            // 
            this.Task_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Task_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Task_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Task_SS.Caption = "S/s";
            this.Task_SS.FieldName = "Task_SS";
            this.Task_SS.Name = "Task_SS";
            this.Task_SS.OptionsColumn.FixedWidth = true;
            this.Task_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Task_SS", "{0}")});
            this.Task_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Task_SS.Visible = true;
            this.Task_SS.VisibleIndex = 0;
            this.Task_SS.Width = 40;
            // 
            // Task_ID
            // 
            this.Task_ID.Caption = "ID";
            this.Task_ID.FieldName = "TASK_ID";
            this.Task_ID.Name = "Task_ID";
            this.Task_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Task_Code
            // 
            this.Task_Code.Caption = "Kodu";
            this.Task_Code.FieldName = "CODE";
            this.Task_Code.Name = "Task_Code";
            this.Task_Code.OptionsColumn.FixedWidth = true;
            this.Task_Code.Visible = true;
            this.Task_Code.VisibleIndex = 1;
            this.Task_Code.Width = 70;
            // 
            // Task_TypeName
            // 
            this.Task_TypeName.Caption = "Tapşırığın növü";
            this.Task_TypeName.FieldName = "TYPE_NAME";
            this.Task_TypeName.Name = "Task_TypeName";
            this.Task_TypeName.OptionsColumn.FixedWidth = true;
            this.Task_TypeName.Visible = true;
            this.Task_TypeName.VisibleIndex = 2;
            this.Task_TypeName.Width = 170;
            // 
            // Task_Date
            // 
            this.Task_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Task_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Task_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Task_Date.Caption = "Tarix";
            this.Task_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Task_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Task_Date.FieldName = "TDATE";
            this.Task_Date.Name = "Task_Date";
            this.Task_Date.OptionsColumn.FixedWidth = true;
            this.Task_Date.Visible = true;
            this.Task_Date.VisibleIndex = 3;
            this.Task_Date.Width = 65;
            // 
            // Task_Amount
            // 
            this.Task_Amount.Caption = "Məbləğ";
            this.Task_Amount.DisplayFormat.FormatString = "n2";
            this.Task_Amount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Task_Amount.FieldName = "AMOUNT";
            this.Task_Amount.Name = "Task_Amount";
            this.Task_Amount.OptionsColumn.FixedWidth = true;
            this.Task_Amount.Visible = true;
            this.Task_Amount.VisibleIndex = 4;
            this.Task_Amount.Width = 100;
            // 
            // Task_CurrencyCode
            // 
            this.Task_CurrencyCode.AppearanceCell.Options.UseTextOptions = true;
            this.Task_CurrencyCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Task_CurrencyCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Task_CurrencyCode.Caption = "Valyuta";
            this.Task_CurrencyCode.FieldName = "CURRENCY_CODE";
            this.Task_CurrencyCode.Name = "Task_CurrencyCode";
            this.Task_CurrencyCode.OptionsColumn.FixedWidth = true;
            this.Task_CurrencyCode.Visible = true;
            this.Task_CurrencyCode.VisibleIndex = 5;
            this.Task_CurrencyCode.Width = 45;
            // 
            // Task_PayingBank
            // 
            this.Task_PayingBank.Caption = "Ödəyən bank";
            this.Task_PayingBank.FieldName = "PAYING_BANK";
            this.Task_PayingBank.Name = "Task_PayingBank";
            this.Task_PayingBank.Visible = true;
            this.Task_PayingBank.VisibleIndex = 6;
            this.Task_PayingBank.Width = 200;
            // 
            // Task_AcceptorName
            // 
            this.Task_AcceptorName.Caption = "Alan tərəf";
            this.Task_AcceptorName.FieldName = "ACCEPTOR_NAME";
            this.Task_AcceptorName.Name = "Task_AcceptorName";
            this.Task_AcceptorName.Visible = true;
            this.Task_AcceptorName.VisibleIndex = 7;
            this.Task_AcceptorName.Width = 200;
            // 
            // Task_InsertUser
            // 
            this.Task_InsertUser.Caption = "Daxil edən";
            this.Task_InsertUser.FieldName = "INSERT_USER";
            this.Task_InsertUser.Name = "Task_InsertUser";
            this.Task_InsertUser.OptionsColumn.FixedWidth = true;
            this.Task_InsertUser.Visible = true;
            this.Task_InsertUser.VisibleIndex = 8;
            this.Task_InsertUser.Width = 150;
            // 
            // Task_InsertDate
            // 
            this.Task_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Task_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Task_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Task_InsertDate.Caption = "Daxil olma tarixi";
            this.Task_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Task_InsertDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Task_InsertDate.FieldName = "INSERT_DATE";
            this.Task_InsertDate.Name = "Task_InsertDate";
            this.Task_InsertDate.OptionsColumn.FixedWidth = true;
            this.Task_InsertDate.Visible = true;
            this.Task_InsertDate.VisibleIndex = 9;
            this.Task_InsertDate.Width = 115;
            // 
            // Task_UpdateUser
            // 
            this.Task_UpdateUser.Caption = "Dəyişiklik edən";
            this.Task_UpdateUser.FieldName = "UPDATE_USER";
            this.Task_UpdateUser.Name = "Task_UpdateUser";
            this.Task_UpdateUser.OptionsColumn.FixedWidth = true;
            this.Task_UpdateUser.Visible = true;
            this.Task_UpdateUser.VisibleIndex = 10;
            this.Task_UpdateUser.Width = 150;
            // 
            // Task_UpdateDate
            // 
            this.Task_UpdateDate.AppearanceCell.Options.UseTextOptions = true;
            this.Task_UpdateDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Task_UpdateDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Task_UpdateDate.Caption = "Dəyişmə vaxtı";
            this.Task_UpdateDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Task_UpdateDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Task_UpdateDate.FieldName = "UPDATE_DATE";
            this.Task_UpdateDate.Name = "Task_UpdateDate";
            this.Task_UpdateDate.OptionsColumn.FixedWidth = true;
            this.Task_UpdateDate.Visible = true;
            this.Task_UpdateDate.VisibleIndex = 11;
            this.Task_UpdateDate.Width = 115;
            // 
            // Task_UsedUserID
            // 
            this.Task_UsedUserID.Caption = "UsedUserID";
            this.Task_UsedUserID.FieldName = "USED_USER_ID";
            this.Task_UsedUserID.Name = "Task_UsedUserID";
            this.Task_UsedUserID.OptionsColumn.AllowShowHide = false;
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.NewBarButton);
            this.PopupMenu.ItemLinks.Add(this.EditBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.TemplatesBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.PaymentTaskRibbon;
            // 
            // FPaymentTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1746, 692);
            this.Controls.Add(this.TaskGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.PaymentTaskRibbon);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FPaymentTask";
            this.Ribbon = this.PaymentTaskRibbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Ödəniş tapşırıqları";
            this.Activated += new System.EventHandler(this.FPaymentTask_Activated);
            this.Load += new System.EventHandler(this.FPaymentTask_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTaskRibbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaskGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl PaymentTaskRibbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage PaymentTaskRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup InfoRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraGrid.GridControl TaskGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView TaskGridView;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup OutRibbonPageGroup;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Task_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Task_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Task_Code;
        private DevExpress.XtraGrid.Columns.GridColumn Task_TypeName;
        private DevExpress.XtraGrid.Columns.GridColumn Task_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Task_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn Task_CurrencyCode;
        private DevExpress.XtraGrid.Columns.GridColumn Task_UpdateDate;
        private DevExpress.XtraGrid.Columns.GridColumn Task_AcceptorName;
        private DevExpress.XtraGrid.Columns.GridColumn Task_InsertUser;
        private DevExpress.XtraGrid.Columns.GridColumn Task_UpdateUser;
        private DevExpress.XtraGrid.Columns.GridColumn Task_UsedUserID;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraGrid.Columns.GridColumn Task_InsertDate;
        private DevExpress.XtraBars.BarButtonItem TemplatesBarButton;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem TaskTypeBarButton;
        private DevExpress.XtraBars.BarButtonItem AcceptorBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Task_PayingBank;
        private DevExpress.XtraBars.BarButtonItem VatBarButton;
    }
}