namespace CRS.Forms.Bank
{
    partial class FBankAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBankAccounts));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.BCancel = new ManiXButton.XButton();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.Bar = new DevExpress.XtraBars.Bar();
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.AccountsGridControl = new DevExpress.XtraGrid.GridControl();
            this.AccountsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Account_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Account_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Acceptor_BankFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Acceptor_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Acceptor_Account = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Acceptor_CurrencyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Acceptor_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Account_StatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Account_IsPayment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Account_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.labelControl15);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 396);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(1192, 50);
            this.PanelOption.TabIndex = 50;
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.labelControl15.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Appearance.Options.UseForeColor = true;
            this.labelControl15.Location = new System.Drawing.Point(32, 19);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(415, 13);
            this.labelControl15.TabIndex = 136;
            this.labelControl15.Text = "Hesabın sənədlərdə istifadə olunması üçün həmin hesabı seçmək lazımdır";
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
            this.BCancel.Location = new System.Drawing.Point(1103, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.TabStop = false;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(32, 396);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.Bar});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.DockControls.Add(this.StandaloneBarDockControl);
            this.BarManager.Form = this;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.NewBarButton,
            this.EditBarButton,
            this.DeleteBarButton,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarButton});
            this.BarManager.MainMenu = this.Bar;
            this.BarManager.MaxItemId = 6;
            // 
            // Bar
            // 
            this.Bar.BarName = "Main menu";
            this.Bar.DockCol = 0;
            this.Bar.DockRow = 0;
            this.Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.Bar.OptionsBar.DrawBorder = false;
            this.Bar.OptionsBar.DrawDragBorder = false;
            this.Bar.OptionsBar.MultiLine = true;
            this.Bar.OptionsBar.UseWholeRow = true;
            this.Bar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.Bar.Text = "Main menu";
            // 
            // NewBarButton
            // 
            this.NewBarButton.Caption = "Yeni";
            this.NewBarButton.Id = 0;
            this.NewBarButton.ImageOptions.Image = global::CRS.Properties.Resources.plus_16;
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Id = 1;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_16;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Id = 2;
            this.DeleteBarButton.ImageOptions.Image = global::CRS.Properties.Resources.minus_16;
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete);
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 3;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.LargeImage")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap";
            this.PrintBarButton.Id = 4;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.LargeImage")));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarButton
            // 
            this.ExportBarButton.Caption = "İxrac";
            this.ExportBarButton.Id = 5;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1192, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 446);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1192, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 446);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1192, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 446);
            // 
            // AccountsGridControl
            // 
            this.AccountsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccountsGridControl.Location = new System.Drawing.Point(32, 0);
            this.AccountsGridControl.MainView = this.AccountsGridView;
            this.AccountsGridControl.Name = "AccountsGridControl";
            this.AccountsGridControl.Size = new System.Drawing.Size(1160, 396);
            this.AccountsGridControl.TabIndex = 70;
            this.AccountsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.AccountsGridView});
            // 
            // AccountsGridView
            // 
            this.AccountsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.AccountsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.AccountsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.AccountsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AccountsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.AccountsGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.AccountsGridView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.AccountsGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.AccountsGridView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.AccountsGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.AccountsGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AccountsGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.AccountsGridView.Appearance.SelectedRow.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.AccountsGridView.Appearance.SelectedRow.Options.UseFont = true;
            this.AccountsGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.AccountsGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AccountsGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.AccountsGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.AccountsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.AccountsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.AccountsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.AccountsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AccountsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Account_SS,
            this.Account_ID,
            this.Acceptor_BankFullName,
            this.Acceptor_Date,
            this.Acceptor_Account,
            this.Acceptor_CurrencyCode,
            this.Acceptor_Note,
            this.Account_StatusID,
            this.Account_IsPayment,
            this.Account_UsedUserID});
            this.AccountsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.AccountsGridView.FormatRules.Add(gridFormatRule1);
            this.AccountsGridView.GridControl = this.AccountsGridControl;
            this.AccountsGridView.Name = "AccountsGridView";
            this.AccountsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.AccountsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.AccountsGridView.OptionsBehavior.Editable = false;
            this.AccountsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.AccountsGridView.OptionsFind.FindDelay = 100;
            this.AccountsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.AccountsGridView.OptionsSelection.MultiSelect = true;
            this.AccountsGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.AccountsGridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.AccountsGridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.AccountsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.AccountsGridView.OptionsView.ShowFooter = true;
            this.AccountsGridView.OptionsView.ShowGroupPanel = false;
            this.AccountsGridView.OptionsView.ShowIndicator = false;
            this.AccountsGridView.PaintStyleName = "Skin";
            this.AccountsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.AccountsGridView.ViewCaption = "Bank hesablarının siyahısı";
            this.AccountsGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.AccountsGridView_RowCellStyle);
            this.AccountsGridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.AccountsGridView_ShowingEditor);
            this.AccountsGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.AccountsGridView_FocusedRowObjectChanged);
            this.AccountsGridView.ColumnFilterChanged += new System.EventHandler(this.AccountsGridView_ColumnFilterChanged);
            this.AccountsGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.AccountsGridView_CustomColumnDisplayText);
            this.AccountsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AccountsGridView_MouseUp);
            this.AccountsGridView.DoubleClick += new System.EventHandler(this.AccountsGridView_DoubleClick);
            // 
            // Account_SS
            // 
            this.Account_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Account_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Account_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Account_SS.Caption = "S/s";
            this.Account_SS.FieldName = "SS";
            this.Account_SS.Name = "Account_SS";
            this.Account_SS.OptionsColumn.FixedWidth = true;
            this.Account_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SS", "{0}")});
            this.Account_SS.Visible = true;
            this.Account_SS.VisibleIndex = 1;
            this.Account_SS.Width = 40;
            // 
            // Account_ID
            // 
            this.Account_ID.Caption = "ID";
            this.Account_ID.FieldName = "ID";
            this.Account_ID.Name = "Account_ID";
            // 
            // Acceptor_BankFullName
            // 
            this.Acceptor_BankFullName.Caption = "Bank";
            this.Acceptor_BankFullName.FieldName = "BANK_FULLNAME";
            this.Acceptor_BankFullName.Name = "Acceptor_BankFullName";
            this.Acceptor_BankFullName.Visible = true;
            this.Acceptor_BankFullName.VisibleIndex = 2;
            this.Acceptor_BankFullName.Width = 893;
            // 
            // Acceptor_Date
            // 
            this.Acceptor_Date.AppearanceCell.Options.UseTextOptions = true;
            this.Acceptor_Date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Acceptor_Date.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Acceptor_Date.Caption = "Tarix";
            this.Acceptor_Date.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Acceptor_Date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Acceptor_Date.FieldName = "ADATE";
            this.Acceptor_Date.Name = "Acceptor_Date";
            this.Acceptor_Date.OptionsColumn.FixedWidth = true;
            this.Acceptor_Date.Visible = true;
            this.Acceptor_Date.VisibleIndex = 3;
            // 
            // Acceptor_Account
            // 
            this.Acceptor_Account.AppearanceCell.Options.UseTextOptions = true;
            this.Acceptor_Account.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Acceptor_Account.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Acceptor_Account.Caption = "Hesab";
            this.Acceptor_Account.FieldName = "ACCOUNT";
            this.Acceptor_Account.Name = "Acceptor_Account";
            this.Acceptor_Account.OptionsColumn.FixedWidth = true;
            this.Acceptor_Account.Visible = true;
            this.Acceptor_Account.VisibleIndex = 4;
            this.Acceptor_Account.Width = 190;
            // 
            // Acceptor_CurrencyCode
            // 
            this.Acceptor_CurrencyCode.Caption = "Valyuta";
            this.Acceptor_CurrencyCode.FieldName = "CURRENCY_CODE";
            this.Acceptor_CurrencyCode.Name = "Acceptor_CurrencyCode";
            this.Acceptor_CurrencyCode.OptionsColumn.FixedWidth = true;
            this.Acceptor_CurrencyCode.Visible = true;
            this.Acceptor_CurrencyCode.VisibleIndex = 5;
            // 
            // Acceptor_Note
            // 
            this.Acceptor_Note.Caption = "Qeyd";
            this.Acceptor_Note.FieldName = "NOTE";
            this.Acceptor_Note.Name = "Acceptor_Note";
            this.Acceptor_Note.Visible = true;
            this.Acceptor_Note.VisibleIndex = 6;
            // 
            // Account_StatusID
            // 
            this.Account_StatusID.Caption = "StatusID";
            this.Account_StatusID.FieldName = "STATUS_ID";
            this.Account_StatusID.Name = "Account_StatusID";
            // 
            // Account_IsPayment
            // 
            this.Account_IsPayment.Caption = "IsPayment";
            this.Account_IsPayment.FieldName = "IS_PAYMENT";
            this.Account_IsPayment.Name = "Account_IsPayment";
            // 
            // Account_UsedUserID
            // 
            this.Account_UsedUserID.Caption = "UsedUserID";
            this.Account_UsedUserID.FieldName = "USED_USER_ID";
            this.Account_UsedUserID.Name = "Account_UsedUserID";
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // FBankAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(1192, 446);
            this.Controls.Add(this.AccountsGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FBankAccounts";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank hesabları";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FBankAccounts_FormClosing);
            this.Load += new System.EventHandler(this.FBankAccounts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            this.PanelOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccountsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar Bar;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl AccountsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView AccountsGridView;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Account_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Account_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Acceptor_BankFullName;
        private DevExpress.XtraGrid.Columns.GridColumn Acceptor_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Acceptor_Account;
        private DevExpress.XtraGrid.Columns.GridColumn Acceptor_CurrencyCode;
        private DevExpress.XtraGrid.Columns.GridColumn Acceptor_Note;
        private DevExpress.XtraGrid.Columns.GridColumn Account_StatusID;
        private DevExpress.XtraGrid.Columns.GridColumn Account_IsPayment;
        private DevExpress.XtraGrid.Columns.GridColumn Account_UsedUserID;
    }
}