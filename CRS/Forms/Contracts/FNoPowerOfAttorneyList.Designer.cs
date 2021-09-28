namespace CRS.Forms.Contracts
{
    partial class FNoPowerOfAttorneyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FNoPowerOfAttorneyList));
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExcelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PowerGridControl = new DevExpress.XtraGrid.GridControl();
            this.PowerGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Power_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Fullname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Hostage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.Power_ContractID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DetailsBarButton = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 729);
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
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExcelBarButton,
            this.DetailsBarButton});
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
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DetailsBarButton)});
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
            // ExcelBarButton
            // 
            this.ExcelBarButton.Caption = "İxrac et";
            this.ExcelBarButton.Id = 2;
            this.ExcelBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExcelBarButton.Name = "ExcelBarButton";
            this.ExcelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExcelBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1509, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 729);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1509, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 729);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1509, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 729);
            // 
            // PowerGridControl
            // 
            this.PowerGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PowerGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerGridControl.Location = new System.Drawing.Point(37, 0);
            this.PowerGridControl.MainView = this.PowerGridView;
            this.PowerGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerGridControl.Name = "PowerGridControl";
            this.PowerGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemPictureEdit});
            this.PowerGridControl.Size = new System.Drawing.Size(1472, 729);
            this.PowerGridControl.TabIndex = 3;
            this.PowerGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PowerGridView});
            // 
            // PowerGridView
            // 
            this.PowerGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PowerGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PowerGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PowerGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PowerGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PowerGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Maroon;
            this.PowerGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.PowerGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PowerGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PowerGridView.AppearancePrint.FooterPanel.Options.UseTextOptions = true;
            this.PowerGridView.AppearancePrint.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PowerGridView.AppearancePrint.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Power_SS,
            this.Power_ID,
            this.Power_ContractCode,
            this.Power_Fullname,
            this.Power_Date,
            this.Power_Hostage,
            this.Power_ContractID});
            this.PowerGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.PowerGridView.GridControl = this.PowerGridControl;
            this.PowerGridView.Name = "PowerGridView";
            this.PowerGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PowerGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PowerGridView.OptionsBehavior.Editable = false;
            this.PowerGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PowerGridView.OptionsFind.FindDelay = 100;
            this.PowerGridView.OptionsPrint.RtfPageFooter = resources.GetString("PowerGridView.OptionsPrint.RtfPageFooter");
            this.PowerGridView.OptionsPrint.RtfReportHeader = resources.GetString("PowerGridView.OptionsPrint.RtfReportHeader");
            this.PowerGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PowerGridView.OptionsView.AllowHtmlDrawHeaders = true;
            this.PowerGridView.OptionsView.ColumnAutoWidth = false;
            this.PowerGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PowerGridView.OptionsView.ShowFooter = true;
            this.PowerGridView.OptionsView.ShowGroupPanel = false;
            this.PowerGridView.OptionsView.ShowIndicator = false;
            this.PowerGridView.PaintStyleName = "Skin";
            this.PowerGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PowerGridView.ViewCaption = "Etibarnaməsiz avtomobillər";
            this.PowerGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.PowerGridView_FocusedRowObjectChanged);
            this.PowerGridView.ColumnFilterChanged += new System.EventHandler(this.PowerGridView_ColumnFilterChanged);
            this.PowerGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.PowerGridView_CustomColumnDisplayText);
            this.PowerGridView.PrintInitialize += new DevExpress.XtraGrid.Views.Base.PrintInitializeEventHandler(this.PowerGridView_PrintInitialize);
            this.PowerGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PowerGridView_MouseUp);
            this.PowerGridView.DoubleClick += new System.EventHandler(this.PowerGridView_DoubleClick);
            // 
            // Power_SS
            // 
            this.Power_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Power_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Power_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Power_SS.Caption = "S/s";
            this.Power_SS.FieldName = "SS";
            this.Power_SS.Name = "Power_SS";
            this.Power_SS.OptionsColumn.FixedWidth = true;
            this.Power_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Power_SS.Visible = true;
            this.Power_SS.VisibleIndex = 0;
            this.Power_SS.Width = 60;
            // 
            // Power_ID
            // 
            this.Power_ID.Caption = "ID";
            this.Power_ID.FieldName = "ID";
            this.Power_ID.Name = "Power_ID";
            // 
            // Power_ContractCode
            // 
            this.Power_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Power_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Power_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Power_ContractCode.Caption = "Müqavilə";
            this.Power_ContractCode.FieldName = "CONTRACT_CODE";
            this.Power_ContractCode.Name = "Power_ContractCode";
            this.Power_ContractCode.OptionsColumn.FixedWidth = true;
            this.Power_ContractCode.Visible = true;
            this.Power_ContractCode.VisibleIndex = 1;
            this.Power_ContractCode.Width = 52;
            // 
            // Power_Fullname
            // 
            this.Power_Fullname.Caption = "Sonuncu sürücünün adı";
            this.Power_Fullname.FieldName = "FULLNAME";
            this.Power_Fullname.Name = "Power_Fullname";
            this.Power_Fullname.Visible = true;
            this.Power_Fullname.VisibleIndex = 2;
            this.Power_Fullname.Width = 350;
            // 
            // Power_Date
            // 
            this.Power_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Power_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Power_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Power_Date.Caption = "Bitmə tarixi";
            this.Power_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Power_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Power_Date.FieldName = "POWER_DATE";
            this.Power_Date.Name = "Power_Date";
            this.Power_Date.OptionsColumn.FixedWidth = true;
            this.Power_Date.Visible = true;
            this.Power_Date.VisibleIndex = 3;
            this.Power_Date.Width = 70;
            // 
            // Power_Hostage
            // 
            this.Power_Hostage.Caption = "Avtomobil";
            this.Power_Hostage.FieldName = "HOSTAGE";
            this.Power_Hostage.Name = "Power_Hostage";
            this.Power_Hostage.Visible = true;
            this.Power_Hostage.VisibleIndex = 4;
            this.Power_Hostage.Width = 850;
            // 
            // RepositoryItemPictureEdit
            // 
            this.RepositoryItemPictureEdit.Name = "RepositoryItemPictureEdit";
            this.RepositoryItemPictureEdit.ZoomAccelerationFactor = 1D;
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExcelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DetailsBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // Power_ContractID
            // 
            this.Power_ContractID.Caption = "ContractID";
            this.Power_ContractID.FieldName = "CONTRACT_ID";
            this.Power_ContractID.Name = "Power_ContractID";
            // 
            // DetailsBarButton
            // 
            this.DetailsBarButton.Caption = "Seçilmiş müqavilənin bütün etibarnamələrinin siyahısı";
            this.DetailsBarButton.Id = 3;
            this.DetailsBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.DetailsBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.DetailsBarButton.Name = "DetailsBarButton";
            this.DetailsBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DetailsBarButton_ItemClick);
            // 
            // FNoPowerOfAttorneyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1509, 729);
            this.Controls.Add(this.PowerGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FNoPowerOfAttorneyList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Etibarnaməsiz avtomobillər";
            this.Load += new System.EventHandler(this.FNoPowerOfAttorneyList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl PowerGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PowerGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Power_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Power_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Power_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Fullname;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Hostage;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Date;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem ExcelBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Power_ContractID;
        private DevExpress.XtraBars.BarButtonItem DetailsBarButton;
    }
}