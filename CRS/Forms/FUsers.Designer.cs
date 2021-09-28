namespace CRS.Forms
{
    partial class FUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FUsers));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.Ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
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
            this.SendMailBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.OpenUserBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.CloseUserBarCheck = new DevExpress.XtraBars.BarCheckItem();
            this.GroupBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ShowOrHideUserBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.UnlockBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.UsersRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.InfoRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.OutRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.FiltRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ViewRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.UsersGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.UsersGridControl = new DevExpress.XtraGrid.GridControl();
            this.UsersGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.UsersSplitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            this.UsersConnectedGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.ConnectGridControl = new DevExpress.XtraGrid.GridControl();
            this.ConnectGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersGroupControl)).BeginInit();
            this.UsersGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsersGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersSplitContainerControl)).BeginInit();
            this.UsersSplitContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsersConnectedGroupControl)).BeginInit();
            this.UsersConnectedGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // Ribbon
            // 
            this.Ribbon.ExpandCollapseItem.Id = 0;
            this.Ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Ribbon.ExpandCollapseItem,
            this.NewBarButton,
            this.EditBarButton,
            this.DeleteBarButton,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.ExcelBarButton,
            this.SendMailBarButton,
            this.OpenUserBarCheck,
            this.CloseUserBarCheck,
            this.GroupBarButton,
            this.ShowOrHideUserBarButton,
            this.PdfBarButton,
            this.RtfBarButton,
            this.TxtBarButton,
            this.HtmlBarButton,
            this.CsvBarButton,
            this.MhtBarButton,
            this.UnlockBarButton});
            this.Ribbon.Location = new System.Drawing.Point(0, 0);
            this.Ribbon.MaxItemId = 22;
            this.Ribbon.Name = "Ribbon";
            this.Ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.UsersRibbonPage});
            this.Ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.Ribbon.ShowQatLocationSelector = false;
            this.Ribbon.ShowToolbarCustomizeItem = false;
            this.Ribbon.Size = new System.Drawing.Size(971, 143);
            this.Ribbon.Toolbar.ShowCustomizeItem = false;
            this.Ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // NewBarButton
            // 
            this.NewBarButton.Caption = "Yeni";
            this.NewBarButton.CloseRadialMenuOnItemClick = true;
            this.NewBarButton.Id = 1;
            this.NewBarButton.ImageOptions.Image = global::CRS.Properties.Resources.user_add_32;
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.CloseRadialMenuOnItemClick = true;
            this.EditBarButton.Id = 2;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.user_edit_32;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.CloseRadialMenuOnItemClick = true;
            this.DeleteBarButton.Id = 3;
            this.DeleteBarButton.ImageOptions.Image = global::CRS.Properties.Resources.user_delete_32;
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete);
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.CloseRadialMenuOnItemClick = true;
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
            this.PrintBarButton.CloseRadialMenuOnItemClick = true;
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
            this.ExportBarSubItem.Id = 7;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
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
            this.ExportBarSubItem.OptionsMultiColumn.ImageHorizontalAlignment = DevExpress.Utils.Drawing.ItemHorizontalAlignment.Right;
            this.ExportBarSubItem.OptionsMultiColumn.ShowItemText = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "Excel";
            this.ExcelBarButton.CloseRadialMenuOnItemClick = true;
            this.ExcelBarButton.Id = 8;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_32;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // PdfBarButton
            // 
            this.PdfBarButton.Caption = "Pdf";
            this.PdfBarButton.CloseRadialMenuOnItemClick = true;
            this.PdfBarButton.Id = 14;
            this.PdfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.pdf_32;
            this.PdfBarButton.Name = "PdfBarButton";
            this.PdfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PdfBarButton_ItemClick);
            // 
            // RtfBarButton
            // 
            this.RtfBarButton.Caption = "Rtf";
            this.RtfBarButton.CloseRadialMenuOnItemClick = true;
            this.RtfBarButton.Id = 15;
            this.RtfBarButton.ImageOptions.Image = global::CRS.Properties.Resources.rtf_32;
            this.RtfBarButton.Name = "RtfBarButton";
            this.RtfBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RtfBarButton_ItemClick);
            // 
            // HtmlBarButton
            // 
            this.HtmlBarButton.Caption = "Html";
            this.HtmlBarButton.CloseRadialMenuOnItemClick = true;
            this.HtmlBarButton.Id = 17;
            this.HtmlBarButton.ImageOptions.Image = global::CRS.Properties.Resources.html_32;
            this.HtmlBarButton.Name = "HtmlBarButton";
            this.HtmlBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HtmlBarButton_ItemClick);
            // 
            // TxtBarButton
            // 
            this.TxtBarButton.Caption = "Txt";
            this.TxtBarButton.CloseRadialMenuOnItemClick = true;
            this.TxtBarButton.Id = 16;
            this.TxtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.txt_32;
            this.TxtBarButton.Name = "TxtBarButton";
            this.TxtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TxtBarButton_ItemClick);
            // 
            // CsvBarButton
            // 
            this.CsvBarButton.Caption = "Csv";
            this.CsvBarButton.CloseRadialMenuOnItemClick = true;
            this.CsvBarButton.Id = 18;
            this.CsvBarButton.ImageOptions.Image = global::CRS.Properties.Resources.csv_32;
            this.CsvBarButton.Name = "CsvBarButton";
            this.CsvBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CsvBarButton_ItemClick);
            // 
            // MhtBarButton
            // 
            this.MhtBarButton.Caption = "Mht";
            this.MhtBarButton.CloseRadialMenuOnItemClick = true;
            this.MhtBarButton.Id = 19;
            this.MhtBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_32;
            this.MhtBarButton.Name = "MhtBarButton";
            this.MhtBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MhtBarButton_ItemClick);
            // 
            // SendMailBarButton
            // 
            this.SendMailBarButton.Caption = "Mail göndər";
            this.SendMailBarButton.CloseRadialMenuOnItemClick = true;
            this.SendMailBarButton.Id = 9;
            this.SendMailBarButton.ImageOptions.Image = global::CRS.Properties.Resources.email_send_32;
            this.SendMailBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M));
            this.SendMailBarButton.Name = "SendMailBarButton";
            this.SendMailBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.SendMailBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.SendMailBarButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // OpenUserBarCheck
            // 
            this.OpenUserBarCheck.BindableChecked = true;
            this.OpenUserBarCheck.Caption = "barCheckItem1";
            this.OpenUserBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.OpenUserBarCheck.Checked = true;
            this.OpenUserBarCheck.Id = 10;
            this.OpenUserBarCheck.Name = "OpenUserBarCheck";
            this.OpenUserBarCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // CloseUserBarCheck
            // 
            this.CloseUserBarCheck.Caption = "barCheckItem2";
            this.CloseUserBarCheck.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.CloseUserBarCheck.Id = 11;
            this.CloseUserBarCheck.Name = "CloseUserBarCheck";
            this.CloseUserBarCheck.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // GroupBarButton
            // 
            this.GroupBarButton.Caption = "İstifadəçi qrupları";
            this.GroupBarButton.CloseRadialMenuOnItemClick = true;
            this.GroupBarButton.Id = 12;
            this.GroupBarButton.ImageOptions.Image = global::CRS.Properties.Resources.users_groups_32;
            this.GroupBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G));
            this.GroupBarButton.Name = "GroupBarButton";
            this.GroupBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.GroupBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.GroupBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.GroupBarButton_ItemClick);
            // 
            // ShowOrHideUserBarButton
            // 
            this.ShowOrHideUserBarButton.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.ShowOrHideUserBarButton.Caption = "Statistikanı gizlət";
            this.ShowOrHideUserBarButton.Id = 13;
            this.ShowOrHideUserBarButton.ImageOptions.Image = global::CRS.Properties.Resources.split_vertical;
            this.ShowOrHideUserBarButton.Name = "ShowOrHideUserBarButton";
            this.ShowOrHideUserBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.ShowOrHideUserBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowOrHideUserBarButton_ItemClick);
            // 
            // UnlockBarButton
            // 
            this.UnlockBarButton.Caption = "Blokdan çıxar";
            this.UnlockBarButton.Id = 21;
            this.UnlockBarButton.ImageOptions.Image = global::CRS.Properties.Resources.unlock_32;
            this.UnlockBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.B));
            this.UnlockBarButton.Name = "UnlockBarButton";
            this.UnlockBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.UnlockBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.UnlockBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.UnlockBarButton_ItemClick);
            // 
            // UsersRibbonPage
            // 
            this.UsersRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.InfoRibbonPageGroup,
            this.OutRibbonPageGroup,
            this.FiltRibbonPageGroup,
            this.ViewRibbonPageGroup});
            this.UsersRibbonPage.Name = "UsersRibbonPage";
            this.UsersRibbonPage.Text = "İstifadəçilər";
            // 
            // InfoRibbonPageGroup
            // 
            this.InfoRibbonPageGroup.ItemLinks.Add(this.NewBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.EditBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.DeleteBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.RefreshBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.UnlockBarButton);
            this.InfoRibbonPageGroup.ItemLinks.Add(this.GroupBarButton);
            this.InfoRibbonPageGroup.Name = "InfoRibbonPageGroup";
            this.InfoRibbonPageGroup.Text = "Məlumat";
            // 
            // OutRibbonPageGroup
            // 
            this.OutRibbonPageGroup.ItemLinks.Add(this.PrintBarButton);
            this.OutRibbonPageGroup.ItemLinks.Add(this.ExportBarSubItem);
            this.OutRibbonPageGroup.ItemLinks.Add(this.SendMailBarButton);
            this.OutRibbonPageGroup.Name = "OutRibbonPageGroup";
            this.OutRibbonPageGroup.Text = "Çıxış";
            // 
            // FiltRibbonPageGroup
            // 
            this.FiltRibbonPageGroup.ItemLinks.Add(this.OpenUserBarCheck);
            this.FiltRibbonPageGroup.ItemLinks.Add(this.CloseUserBarCheck);
            this.FiltRibbonPageGroup.Name = "FiltRibbonPageGroup";
            this.FiltRibbonPageGroup.Text = "Filtr";
            // 
            // ViewRibbonPageGroup
            // 
            this.ViewRibbonPageGroup.ItemLinks.Add(this.ShowOrHideUserBarButton);
            this.ViewRibbonPageGroup.Name = "ViewRibbonPageGroup";
            this.ViewRibbonPageGroup.Text = "Görünüş";
            // 
            // UsersGroupControl
            // 
            this.UsersGroupControl.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.UsersGroupControl.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UsersGroupControl.AppearanceCaption.Options.UseFont = true;
            this.UsersGroupControl.AppearanceCaption.Options.UseForeColor = true;
            this.UsersGroupControl.Controls.Add(this.UsersGridControl);
            this.UsersGroupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UsersGroupControl.Location = new System.Drawing.Point(0, 0);
            this.UsersGroupControl.Name = "UsersGroupControl";
            this.UsersGroupControl.Size = new System.Drawing.Size(971, 268);
            this.UsersGroupControl.TabIndex = 0;
            this.UsersGroupControl.Text = "İstifadəçilərin siyahısı";
            // 
            // UsersGridControl
            // 
            this.UsersGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.UsersGridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.UsersGridControl.Location = new System.Drawing.Point(2, 20);
            this.UsersGridControl.MainView = this.UsersGridView;
            this.UsersGridControl.MenuManager = this.Ribbon;
            this.UsersGridControl.Name = "UsersGridControl";
            this.UsersGridControl.Size = new System.Drawing.Size(967, 246);
            this.UsersGridControl.TabIndex = 0;
            this.UsersGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.UsersGridView});
            // 
            // UsersGridView
            // 
            this.UsersGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.UsersGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UsersGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.UsersGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.UsersGridView.GridControl = this.UsersGridControl;
            this.UsersGridView.Name = "UsersGridView";
            this.UsersGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.UsersGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.UsersGridView.OptionsBehavior.Editable = false;
            this.UsersGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.UsersGridView.OptionsFind.FindDelay = 100;
            this.UsersGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.UsersGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.UsersGridView.OptionsView.ShowFooter = true;
            this.UsersGridView.OptionsView.ShowGroupPanel = false;
            this.UsersGridView.OptionsView.ShowIndicator = false;
            this.UsersGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.UsersGridView_CustomDrawFooterCell);
            this.UsersGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.UsersGridView_RowCellStyle);
            this.UsersGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.UsersGridView_FocusedRowObjectChanged);
            this.UsersGridView.ColumnFilterChanged += new System.EventHandler(this.UsersGridView_ColumnFilterChanged);
            this.UsersGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.UsersGridView_CustomColumnDisplayText);
            this.UsersGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UsersGridView_MouseUp);
            this.UsersGridView.DoubleClick += new System.EventHandler(this.UsersGridView_DoubleClick);
            // 
            // UsersSplitContainerControl
            // 
            this.UsersSplitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UsersSplitContainerControl.Horizontal = false;
            this.UsersSplitContainerControl.Location = new System.Drawing.Point(0, 143);
            this.UsersSplitContainerControl.Name = "UsersSplitContainerControl";
            this.UsersSplitContainerControl.Panel1.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.UsersSplitContainerControl.Panel1.AppearanceCaption.Options.UseFont = true;
            this.UsersSplitContainerControl.Panel1.Controls.Add(this.UsersGroupControl);
            this.UsersSplitContainerControl.Panel1.Text = "İstifadəçilərin siyahısı";
            this.UsersSplitContainerControl.Panel2.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.UsersSplitContainerControl.Panel2.AppearanceCaption.Options.UseFont = true;
            this.UsersSplitContainerControl.Panel2.Controls.Add(this.UsersConnectedGroupControl);
            this.UsersSplitContainerControl.Panel2.Text = "Sistemə daxil olma statistikası";
            this.UsersSplitContainerControl.ShowCaption = true;
            this.UsersSplitContainerControl.Size = new System.Drawing.Size(971, 418);
            this.UsersSplitContainerControl.SplitterPosition = 268;
            this.UsersSplitContainerControl.TabIndex = 2;
            this.UsersSplitContainerControl.Text = "splitContainerControl1";
            // 
            // UsersConnectedGroupControl
            // 
            this.UsersConnectedGroupControl.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.UsersConnectedGroupControl.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.UsersConnectedGroupControl.AppearanceCaption.Options.UseFont = true;
            this.UsersConnectedGroupControl.AppearanceCaption.Options.UseForeColor = true;
            this.UsersConnectedGroupControl.Controls.Add(this.ConnectGridControl);
            this.UsersConnectedGroupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UsersConnectedGroupControl.Location = new System.Drawing.Point(0, 0);
            this.UsersConnectedGroupControl.Name = "UsersConnectedGroupControl";
            this.UsersConnectedGroupControl.Size = new System.Drawing.Size(971, 145);
            this.UsersConnectedGroupControl.TabIndex = 0;
            this.UsersConnectedGroupControl.Text = "İstifadəçilərin sistemə qoşulma statistikası";
            // 
            // ConnectGridControl
            // 
            this.ConnectGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectGridControl.Location = new System.Drawing.Point(2, 20);
            this.ConnectGridControl.MainView = this.ConnectGridView;
            this.ConnectGridControl.MenuManager = this.Ribbon;
            this.ConnectGridControl.Name = "ConnectGridControl";
            this.ConnectGridControl.Size = new System.Drawing.Size(967, 123);
            this.ConnectGridControl.TabIndex = 1;
            this.ConnectGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ConnectGridView});
            // 
            // ConnectGridView
            // 
            this.ConnectGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.ConnectGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ConnectGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ConnectGridView.Appearance.Row.Options.UseTextOptions = true;
            this.ConnectGridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ConnectGridView.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ConnectGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.ConnectGridView.GridControl = this.ConnectGridControl;
            this.ConnectGridView.Name = "ConnectGridView";
            this.ConnectGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.ConnectGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.ConnectGridView.OptionsBehavior.Editable = false;
            this.ConnectGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.ConnectGridView.OptionsFind.FindDelay = 100;
            this.ConnectGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ConnectGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.ConnectGridView.OptionsView.ShowFooter = true;
            this.ConnectGridView.OptionsView.ShowGroupPanel = false;
            this.ConnectGridView.OptionsView.ShowIndicator = false;
            this.ConnectGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.ConnectGridView_CustomColumnDisplayText);
            // 
            // PopupMenu
            // 
            this.PopupMenu.ItemLinks.Add(this.NewBarButton);
            this.PopupMenu.ItemLinks.Add(this.EditBarButton);
            this.PopupMenu.ItemLinks.Add(this.DeleteBarButton);
            this.PopupMenu.ItemLinks.Add(this.RefreshBarButton);
            this.PopupMenu.ItemLinks.Add(this.UnlockBarButton);
            this.PopupMenu.ItemLinks.Add(this.GroupBarButton);
            this.PopupMenu.ItemLinks.Add(this.PrintBarButton);
            this.PopupMenu.ItemLinks.Add(this.ExportBarSubItem);
            this.PopupMenu.ItemLinks.Add(this.SendMailBarButton);
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Ribbon = this.Ribbon;
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.Ribbon = this.Ribbon;
            // 
            // FUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 561);
            this.Controls.Add(this.UsersSplitContainerControl);
            this.Controls.Add(this.Ribbon);
            this.MinimizeBox = false;
            this.Name = "FUsers";
            this.Ribbon = this.Ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İstifadəçilərin siyahısı";
            this.Activated += new System.EventHandler(this.FUsers_Activated);
            this.Load += new System.EventHandler(this.FUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersGroupControl)).EndInit();
            this.UsersGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UsersGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersSplitContainerControl)).EndInit();
            this.UsersSplitContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UsersConnectedGroupControl)).EndInit();
            this.UsersConnectedGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConnectGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl Ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage UsersRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup InfoRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup OutRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem SendMailBarButton;
        private DevExpress.XtraBars.BarCheckItem OpenUserBarCheck;
        private DevExpress.XtraBars.BarCheckItem CloseUserBarCheck;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup FiltRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem GroupBarButton;
        private DevExpress.XtraEditors.GroupControl UsersGroupControl;
        private DevExpress.XtraGrid.GridControl UsersGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView UsersGridView;
        private DevExpress.XtraEditors.SplitContainerControl UsersSplitContainerControl;
        private DevExpress.XtraEditors.GroupControl UsersConnectedGroupControl;
        private DevExpress.XtraGrid.GridControl ConnectGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ConnectGridView;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ViewRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem ShowOrHideUserBarButton;
        private DevExpress.XtraBars.BarButtonItem PdfBarButton;
        private DevExpress.XtraBars.BarButtonItem RtfBarButton;
        private DevExpress.XtraBars.BarButtonItem TxtBarButton;
        private DevExpress.XtraBars.BarButtonItem HtmlBarButton;
        private DevExpress.XtraBars.BarButtonItem CsvBarButton;
        private DevExpress.XtraBars.BarButtonItem MhtBarButton;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem UnlockBarButton;
    }
}