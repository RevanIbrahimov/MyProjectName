namespace CRS.Forms.Bookkeeping
{
    partial class FJournalLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FJournalLog));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue2 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.Journal_AmountCur = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_AmountAZN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BackBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.ExcelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PdfBarButtonI = new DevExpress.XtraBars.BarButtonItem();
            this.RtfBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.HtmlBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TxtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CsvBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.MhtBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.LogRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.OperationsGridControl = new DevExpress.XtraGrid.GridControl();
            this.OperationsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Journal_LogDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_LogUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_LogTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_UserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_Appointment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_DebitAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_CreditAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_CurrencyRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_UserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_YRMNTHDY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_IsManual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_AccountOperationTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_ContractID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_LogType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Journal_LogID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // Journal_AmountCur
            // 
            this.Journal_AmountCur.Caption = "Məbləğ (xarici valyutada)";
            this.Journal_AmountCur.DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
            this.Journal_AmountCur.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Journal_AmountCur.FieldName = "AMOUNT_CUR";
            this.Journal_AmountCur.Name = "Journal_AmountCur";
            this.Journal_AmountCur.OptionsColumn.FixedWidth = true;
            this.Journal_AmountCur.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMOUNT_CUR", "{0:### ### ### ### ### ### ##0.00}")});
            this.Journal_AmountCur.Visible = true;
            this.Journal_AmountCur.VisibleIndex = 10;
            this.Journal_AmountCur.Width = 130;
            // 
            // Journal_AmountAZN
            // 
            this.Journal_AmountAZN.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Journal_AmountAZN.AppearanceHeader.Options.UseFont = true;
            this.Journal_AmountAZN.Caption = "Məbləğ (AZN-ilə)";
            this.Journal_AmountAZN.DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
            this.Journal_AmountAZN.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Journal_AmountAZN.FieldName = "AMOUNT_AZN";
            this.Journal_AmountAZN.Name = "Journal_AmountAZN";
            this.Journal_AmountAZN.OptionsColumn.FixedWidth = true;
            this.Journal_AmountAZN.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMOUNT_AZN", "{0:### ### ### ### ### ### ##0.00}")});
            this.Journal_AmountAZN.Visible = true;
            this.Journal_AmountAZN.VisibleIndex = 11;
            this.Journal_AmountAZN.Width = 130;
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.RefreshBarButton,
            this.BackBarButton,
            this.DeleteBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.ExcelBarButton,
            this.PdfBarButtonI,
            this.RtfBarButton,
            this.HtmlBarButton,
            this.TxtBarButton,
            this.CsvBarButton,
            this.MhtBarButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 13;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.LogRibbonPage});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1774, 179);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 1;
            this.RefreshBarButton.ImageOptions.Image = global::CRS.Properties.Resources.Refresh_32;
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // BackBarButton
            // 
            this.BackBarButton.Caption = "Geri qaytar";
            this.BackBarButton.Id = 2;
            this.BackBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BackBarButton.ImageOptions.Image")));
            this.BackBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G));
            this.BackBarButton.Name = "BackBarButton";
            this.BackBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.BackBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem1.Text = "<color=255,0,0>Manual silinən əməliyyatları geri qaytar</color>";
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Jurnaldan manual silinən əməliyyatları geri qaytarmaq üçün nəzərdə tutulub.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.BackBarButton.SuperTip = superToolTip1;
            this.BackBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BackBarButton_ItemClick);
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
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 4;
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
            this.ExportBarSubItem.Id = 5;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.ExportBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PdfBarButtonI),
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
            this.ExcelBarButton.Id = 6;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButtonI
            // 
            this.PdfBarButtonI.Caption = "Pdf";
            this.PdfBarButtonI.Id = 7;
            this.PdfBarButtonI.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButtonI.Name = "PdfBarButtonI";
            this.PdfBarButtonI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButtonI_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.Id = 8;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.Id = 9;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.Id = 10;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.Id = 11;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.Id = 12;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // LogRibbonPage
            // 
            this.LogRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.ribbonPageGroup1});
            this.LogRibbonPage.Name = "LogRibbonPage";
            this.LogRibbonPage.Text = "Əməliyyatların tarixçəsi";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.BackBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.Name = "InfoRibbonPageGroup";
            this.InfoRibbonPageGroup.Text = "Məlumat";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.PrintBarButton);
            this.ribbonPageGroup1.ItemLinks.Add(this.ExportBarSubItem);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Çıxış";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 747);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1774, 40);
            // 
            // OperationsGridControl
            // 
            this.OperationsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationsGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OperationsGridControl.Location = new System.Drawing.Point(0, 179);
            this.OperationsGridControl.MainView = this.OperationsGridView;
            this.OperationsGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OperationsGridControl.Name = "OperationsGridControl";
            this.OperationsGridControl.Size = new System.Drawing.Size(1774, 568);
            this.OperationsGridControl.TabIndex = 81;
            this.OperationsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.OperationsGridView});
            // 
            // OperationsGridView
            // 
            this.OperationsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.OperationsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.OperationsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.OperationsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.OperationsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.OperationsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.OperationsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OperationsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.OperationsGridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.OperationsGridView.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.OperationsGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.OperationsGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.OperationsGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.OperationsGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.OperationsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.OperationsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.OperationsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.OperationsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.OperationsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Journal_LogDate,
            this.Journal_LogUserName,
            this.Journal_LogTypeName,
            this.Journal_Date,
            this.Journal_UserName,
            this.Journal_Appointment,
            this.Journal_DebitAccount,
            this.Journal_CreditAccount,
            this.Journal_CurrencyRate,
            this.Journal_AmountCur,
            this.Journal_AmountAZN,
            this.Journal_ID,
            this.Journal_UserID,
            this.Journal_YRMNTHDY,
            this.Journal_IsManual,
            this.Journal_AccountOperationTypeID,
            this.Journal_ContractID,
            this.Journal_LogType,
            this.Journal_UsedUserID,
            this.Journal_LogID});
            this.OperationsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Column = this.Journal_AmountCur;
            gridFormatRule1.Name = "Format_AmountCur_LessZero";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Less;
            formatConditionRuleValue1.PredefinedName = "Red Text";
            formatConditionRuleValue1.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            gridFormatRule1.Rule = formatConditionRuleValue1;
            gridFormatRule2.Column = this.Journal_AmountAZN;
            gridFormatRule2.Name = "Format_AmountAZN_LessZero";
            formatConditionRuleValue2.Condition = DevExpress.XtraEditors.FormatCondition.Less;
            formatConditionRuleValue2.PredefinedName = "Red Text";
            formatConditionRuleValue2.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            gridFormatRule2.Rule = formatConditionRuleValue2;
            this.OperationsGridView.FormatRules.Add(gridFormatRule1);
            this.OperationsGridView.FormatRules.Add(gridFormatRule2);
            this.OperationsGridView.GridControl = this.OperationsGridControl;
            this.OperationsGridView.Name = "OperationsGridView";
            this.OperationsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.OperationsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.OperationsGridView.OptionsBehavior.Editable = false;
            this.OperationsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.OperationsGridView.OptionsFind.FindDelay = 100;
            this.OperationsGridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.OperationsGridView.OptionsNavigation.AutoFocusNewRow = true;
            this.OperationsGridView.OptionsNavigation.EnterMoveNextColumn = true;
            this.OperationsGridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.OperationsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.OperationsGridView.OptionsSelection.MultiSelect = true;
            this.OperationsGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.OperationsGridView.OptionsView.ColumnAutoWidth = false;
            this.OperationsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.OperationsGridView.OptionsView.ShowFooter = true;
            this.OperationsGridView.OptionsView.ShowGroupPanel = false;
            this.OperationsGridView.OptionsView.ShowIndicator = false;
            this.OperationsGridView.PaintStyleName = "Skin";
            this.OperationsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.OperationsGridView.ViewCaption = "Jurnal";
            this.OperationsGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.OperationsGridView_RowCellStyle);
            this.OperationsGridView.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.OperationsGridView_SelectionChanged);
            this.OperationsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OperationsGridView_MouseUp);
            // 
            // Journal_LogDate
            // 
            this.Journal_LogDate.AppearanceCell.Options.UseTextOptions = true;
            this.Journal_LogDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Journal_LogDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Journal_LogDate.Caption = "İcra tarixi";
            this.Journal_LogDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Journal_LogDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Journal_LogDate.FieldName = "LOG_DATE";
            this.Journal_LogDate.Name = "Journal_LogDate";
            this.Journal_LogDate.OptionsColumn.FixedWidth = true;
            this.Journal_LogDate.Visible = true;
            this.Journal_LogDate.VisibleIndex = 1;
            this.Journal_LogDate.Width = 115;
            // 
            // Journal_LogUserName
            // 
            this.Journal_LogUserName.Caption = "İcra edən istifadəçi";
            this.Journal_LogUserName.FieldName = "LOG_USER_NAME";
            this.Journal_LogUserName.Name = "Journal_LogUserName";
            this.Journal_LogUserName.OptionsColumn.FixedWidth = true;
            this.Journal_LogUserName.Visible = true;
            this.Journal_LogUserName.VisibleIndex = 2;
            this.Journal_LogUserName.Width = 103;
            // 
            // Journal_LogTypeName
            // 
            this.Journal_LogTypeName.Caption = "İcranın növü";
            this.Journal_LogTypeName.FieldName = "LOG_TYPE_NAME";
            this.Journal_LogTypeName.Name = "Journal_LogTypeName";
            this.Journal_LogTypeName.OptionsColumn.FixedWidth = true;
            this.Journal_LogTypeName.Visible = true;
            this.Journal_LogTypeName.VisibleIndex = 3;
            // 
            // Journal_Date
            // 
            this.Journal_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Journal_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Journal_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Journal_Date.Caption = "Tarix";
            this.Journal_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Journal_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Journal_Date.FieldName = "ODATE";
            this.Journal_Date.Name = "Journal_Date";
            this.Journal_Date.OptionsColumn.FixedWidth = true;
            this.Journal_Date.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ODATE", "{0}")});
            this.Journal_Date.ToolTip = "Əməliyyatın tarixi";
            this.Journal_Date.Visible = true;
            this.Journal_Date.VisibleIndex = 4;
            this.Journal_Date.Width = 65;
            // 
            // Journal_UserName
            // 
            this.Journal_UserName.AppearanceCell.Options.UseTextOptions = true;
            this.Journal_UserName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Journal_UserName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Journal_UserName.Caption = "Daxil edən istifadəçi";
            this.Journal_UserName.FieldName = "USERNAME";
            this.Journal_UserName.Name = "Journal_UserName";
            this.Journal_UserName.OptionsColumn.FixedWidth = true;
            this.Journal_UserName.ToolTip = "Jurnala daxil edən istifadəçi";
            this.Journal_UserName.Visible = true;
            this.Journal_UserName.VisibleIndex = 5;
            this.Journal_UserName.Width = 105;
            // 
            // Journal_Appointment
            // 
            this.Journal_Appointment.Caption = "Təyinat";
            this.Journal_Appointment.FieldName = "APPOINTMENT";
            this.Journal_Appointment.Name = "Journal_Appointment";
            this.Journal_Appointment.OptionsColumn.FixedWidth = true;
            this.Journal_Appointment.Visible = true;
            this.Journal_Appointment.VisibleIndex = 6;
            this.Journal_Appointment.Width = 450;
            // 
            // Journal_DebitAccount
            // 
            this.Journal_DebitAccount.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Journal_DebitAccount.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.Journal_DebitAccount.AppearanceHeader.Options.UseFont = true;
            this.Journal_DebitAccount.AppearanceHeader.Options.UseForeColor = true;
            this.Journal_DebitAccount.Caption = "Debit hesabı";
            this.Journal_DebitAccount.FieldName = "DEBIT_ACCOUNT";
            this.Journal_DebitAccount.Name = "Journal_DebitAccount";
            this.Journal_DebitAccount.OptionsColumn.FixedWidth = true;
            this.Journal_DebitAccount.Visible = true;
            this.Journal_DebitAccount.VisibleIndex = 7;
            this.Journal_DebitAccount.Width = 120;
            // 
            // Journal_CreditAccount
            // 
            this.Journal_CreditAccount.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Journal_CreditAccount.AppearanceHeader.ForeColor = System.Drawing.Color.Blue;
            this.Journal_CreditAccount.AppearanceHeader.Options.UseFont = true;
            this.Journal_CreditAccount.AppearanceHeader.Options.UseForeColor = true;
            this.Journal_CreditAccount.Caption = "Kredit hesabı";
            this.Journal_CreditAccount.FieldName = "CREDIT_ACCOUNT";
            this.Journal_CreditAccount.Name = "Journal_CreditAccount";
            this.Journal_CreditAccount.OptionsColumn.FixedWidth = true;
            this.Journal_CreditAccount.Visible = true;
            this.Journal_CreditAccount.VisibleIndex = 8;
            this.Journal_CreditAccount.Width = 120;
            // 
            // Journal_CurrencyRate
            // 
            this.Journal_CurrencyRate.Caption = "Məzənnə";
            this.Journal_CurrencyRate.DisplayFormat.FormatString = "#0.0000";
            this.Journal_CurrencyRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Journal_CurrencyRate.FieldName = "CURRENCY_RATE";
            this.Journal_CurrencyRate.Name = "Journal_CurrencyRate";
            this.Journal_CurrencyRate.OptionsColumn.FixedWidth = true;
            this.Journal_CurrencyRate.Visible = true;
            this.Journal_CurrencyRate.VisibleIndex = 9;
            this.Journal_CurrencyRate.Width = 80;
            // 
            // Journal_ID
            // 
            this.Journal_ID.Caption = "ID";
            this.Journal_ID.FieldName = "ID";
            this.Journal_ID.Name = "Journal_ID";
            this.Journal_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_UserID
            // 
            this.Journal_UserID.Caption = "USED_USER_ID";
            this.Journal_UserID.FieldName = "USER_ID";
            this.Journal_UserID.Name = "Journal_UserID";
            this.Journal_UserID.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_YRMNTHDY
            // 
            this.Journal_YRMNTHDY.Caption = "Tarix";
            this.Journal_YRMNTHDY.FieldName = "YR_MNTH_DY";
            this.Journal_YRMNTHDY.Name = "Journal_YRMNTHDY";
            this.Journal_YRMNTHDY.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_IsManual
            // 
            this.Journal_IsManual.Caption = "IS_MANUAL";
            this.Journal_IsManual.FieldName = "IS_MANUAL";
            this.Journal_IsManual.Name = "Journal_IsManual";
            this.Journal_IsManual.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_AccountOperationTypeID
            // 
            this.Journal_AccountOperationTypeID.Caption = "Ödənişin tipi";
            this.Journal_AccountOperationTypeID.FieldName = "ACCOUNT_OPERATION_TYPE_ID";
            this.Journal_AccountOperationTypeID.Name = "Journal_AccountOperationTypeID";
            this.Journal_AccountOperationTypeID.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_ContractID
            // 
            this.Journal_ContractID.Caption = "CONTRACT_ID";
            this.Journal_ContractID.FieldName = "CONTRACT_ID";
            this.Journal_ContractID.Name = "Journal_ContractID";
            this.Journal_ContractID.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_LogType
            // 
            this.Journal_LogType.Caption = "LogType";
            this.Journal_LogType.FieldName = "LOG_TYPE";
            this.Journal_LogType.Name = "Journal_LogType";
            this.Journal_LogType.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_UsedUserID
            // 
            this.Journal_UsedUserID.Caption = "UsedUserID";
            this.Journal_UsedUserID.FieldName = "USED_USER_ID";
            this.Journal_UsedUserID.Name = "Journal_UsedUserID";
            this.Journal_UsedUserID.OptionsColumn.AllowShowHide = false;
            // 
            // Journal_LogID
            // 
            this.Journal_LogID.Caption = "LogID";
            this.Journal_LogID.FieldName = "LOG_ID";
            this.Journal_LogID.Name = "Journal_LogID";
            this.Journal_LogID.OptionsColumn.AllowShowHide = false;
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.BackBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.ribbon;
            // 
            // FJournalLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1774, 787);
            this.Controls.Add(this.OperationsGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.MinimizeBox = false;
            this.Name = "FJournalLog";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Əməliyyatların tarixçəsi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FJournalLog_FormClosing);
            this.Load += new System.EventHandler(this.FJournalLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OperationsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage LogRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup InfoRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraGrid.GridControl OperationsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView OperationsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_UserName;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_Appointment;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_DebitAccount;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_CreditAccount;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_CurrencyRate;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_AmountCur;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_AmountAZN;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_UserID;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_YRMNTHDY;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_IsManual;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_AccountOperationTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_ContractID;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_LogDate;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_LogUserName;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_LogTypeName;
        private DevExpress.XtraBars.BarButtonItem BackBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_LogType;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButtonI;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_UsedUserID;
        private DevExpress.XtraGrid.Columns.GridColumn Journal_LogID;
    }
}