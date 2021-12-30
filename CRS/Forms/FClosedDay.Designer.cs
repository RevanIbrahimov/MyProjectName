namespace CRS.Forms
{
    partial class FClosedDay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FClosedDay));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ClosedDayBarButton = new DevExpress.XtraBars.BarButtonItem();
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
            this.ClosedDayRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.DaysGridControl = new DevExpress.XtraGrid.GridControl();
            this.DaysGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Days_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Days_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Days_ClosedDay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Days_InsertUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Days_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DaysGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DaysGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ClosedDayBarButton,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.ExcelBarButton,
            this.PdfBarButton,
            this.TxtBarButton,
            this.HtmlBarButton,
            this.CsvBarButton,
            this.MhtBarButton,
            this.RtfBarButton,
            this.DeleteBarButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 13;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ClosedDayRibbonPage});
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(559, 179);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // ClosedDayBarButton
            // 
            this.ClosedDayBarButton.Caption = "Günü bağla";
            this.ClosedDayBarButton.Id = 1;
            this.ClosedDayBarButton.ImageOptions.Image = global::CRS.Properties.Resources.calendar__2_;
            this.ClosedDayBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.ClosedDayBarButton.Name = "ClosedDayBarButton";
            this.ClosedDayBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.ClosedDayBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ClosedDayBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ClosedDayBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 2;
            this.RefreshBarButton.ImageOptions.Image = global::CRS.Properties.Resources.Refresh_32;
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 3;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarSubItem
            // 
            this.ExportBarSubItem.Caption = "İxrac et";
            this.ExportBarSubItem.Id = 4;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.ExportBarSubItem.ItemAppearance.Disabled.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Disabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Hovered.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Pressed.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Pressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PdfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RtfBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.HtmlBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.TxtBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.CsvBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.MhtBarButton)});
            this.ExportBarSubItem.MultiColumn = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarSubItem.Name = "ExportBarSubItem";
            this.ExportBarSubItem.OptionsMultiColumn.ShowItemText = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "Excel";
            this.ExcelBarButton.Id = 5;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButton
            // 
            this.PdfBarButton.Caption = "Pdf";
            this.PdfBarButton.Id = 6;
            this.PdfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButton.Name = "PdfBarButton";
            this.PdfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButton_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.Id = 11;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.Id = 8;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.Id = 7;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.Id = 9;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.Id = 10;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // ClosedDayRibbonPage
            // 
            this.ClosedDayRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ClosedDayRibbonPage.Name = "ClosedDayRibbonPage";
            this.ClosedDayRibbonPage.Text = "Günlər";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.ClosedDayBarButton);
            this.ribbonPageGroup1.ItemLinks.Add(this.RefreshBarButton);
            this.ribbonPageGroup1.ItemLinks.Add(this.DeleteBarButton);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Məlumat";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.PrintBarButton);
            this.ribbonPageGroup2.ItemLinks.Add(this.ExportBarSubItem);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Çıxış";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 708);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(559, 40);
            // 
            // DaysGridControl
            // 
            this.DaysGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaysGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DaysGridControl.Location = new System.Drawing.Point(0, 179);
            this.DaysGridControl.MainView = this.DaysGridView;
            this.DaysGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DaysGridControl.MenuManager = this.ribbon;
            this.DaysGridControl.Name = "DaysGridControl";
            this.DaysGridControl.Size = new System.Drawing.Size(559, 529);
            this.DaysGridControl.TabIndex = 2;
            this.DaysGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DaysGridView});
            // 
            // DaysGridView
            // 
            this.DaysGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.DaysGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DaysGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DaysGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.DaysGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DaysGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DaysGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Days_SS,
            this.Days_ID,
            this.Days_ClosedDay,
            this.Days_InsertUser,
            this.Days_InsertDate});
            this.DaysGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.DaysGridView.GridControl = this.DaysGridControl;
            this.DaysGridView.Name = "DaysGridView";
            this.DaysGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.DaysGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.DaysGridView.OptionsBehavior.Editable = false;
            this.DaysGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.DaysGridView.OptionsFind.FindDelay = 100;
            this.DaysGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.DaysGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.DaysGridView.OptionsView.ShowFooter = true;
            this.DaysGridView.OptionsView.ShowGroupPanel = false;
            this.DaysGridView.OptionsView.ShowIndicator = false;
            this.DaysGridView.ViewCaption = "Bağlanılmış günlər";
            this.DaysGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.DaysGridView_FocusedRowObjectChanged);
            this.DaysGridView.ColumnFilterChanged += new System.EventHandler(this.DaysGridView_ColumnFilterChanged);
            this.DaysGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.DaysGridView_CustomUnboundColumnData);
            this.DaysGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DaysGridView_MouseUp);
            // 
            // Days_SS
            // 
            this.Days_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Days_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Days_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Days_SS.Caption = "S/s";
            this.Days_SS.FieldName = "User_SS";
            this.Days_SS.Name = "Days_SS";
            this.Days_SS.OptionsColumn.FixedWidth = true;
            this.Days_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "User_SS", "{0}")});
            this.Days_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Days_SS.Visible = true;
            this.Days_SS.VisibleIndex = 0;
            this.Days_SS.Width = 45;
            // 
            // Days_ID
            // 
            this.Days_ID.Caption = "ID";
            this.Days_ID.FieldName = "ID";
            this.Days_ID.Name = "Days_ID";
            this.Days_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Days_ClosedDay
            // 
            this.Days_ClosedDay.Caption = "Bağlanılmış gün";
            this.Days_ClosedDay.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Days_ClosedDay.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Days_ClosedDay.FieldName = "CLOSED_DAY";
            this.Days_ClosedDay.Name = "Days_ClosedDay";
            this.Days_ClosedDay.OptionsColumn.FixedWidth = true;
            this.Days_ClosedDay.Visible = true;
            this.Days_ClosedDay.VisibleIndex = 1;
            this.Days_ClosedDay.Width = 90;
            // 
            // Days_InsertUser
            // 
            this.Days_InsertUser.Caption = "Günü bağlıyan istifadəçi";
            this.Days_InsertUser.FieldName = "INSERT_USER";
            this.Days_InsertUser.Name = "Days_InsertUser";
            this.Days_InsertUser.Visible = true;
            this.Days_InsertUser.VisibleIndex = 2;
            this.Days_InsertUser.Width = 797;
            // 
            // Days_InsertDate
            // 
            this.Days_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Days_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Days_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Days_InsertDate.Caption = "Bağlanılma vaxtı";
            this.Days_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Days_InsertDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Days_InsertDate.FieldName = "INSERT_DATE";
            this.Days_InsertDate.Name = "Days_InsertDate";
            this.Days_InsertDate.OptionsColumn.FixedWidth = true;
            this.Days_InsertDate.Visible = true;
            this.Days_InsertDate.VisibleIndex = 3;
            this.Days_InsertDate.Width = 115;
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.ClosedDayBarButton);
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.ribbon;
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 12;
            this.DeleteBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // FClosedDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 748);
            this.Controls.Add(this.DaysGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FClosedDay";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Bağlanılmış günlər";
            this.Load += new System.EventHandler(this.FClosedDay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DaysGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DaysGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ClosedDayRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem ClosedDayBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraGrid.GridControl DaysGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DaysGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Days_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Days_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Days_ClosedDay;
        private DevExpress.XtraGrid.Columns.GridColumn Days_InsertUser;
        private DevExpress.XtraGrid.Columns.GridColumn Days_InsertDate;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
    }
}