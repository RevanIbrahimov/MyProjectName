namespace CRS.Forms
{
    partial class FExchangeCalculator
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
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FExchangeCalculator));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.RatesGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.ProgressPanel = new DevExpress.XtraWaitForm.ProgressPanel();
            this.ExchangesGridControl = new DevExpress.XtraGrid.GridControl();
            this.ExchangesGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.Bar = new DevExpress.XtraBars.Bar();
            this.NewBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.UpgradeBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RateChartBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.AmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.Currency1ComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ExchangeGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.ResultText = new DevExpress.XtraEditors.TextEdit();
            this.Currency2ComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ChangePictureBox = new System.Windows.Forms.PictureBox();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.DateValue = new DevExpress.XtraEditors.DateEdit();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RatesGroupControl)).BeginInit();
            this.RatesGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExchangesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExchangesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Currency1ComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExchangeGroupControl)).BeginInit();
            this.ExchangeGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Currency2ComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 456);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(629, 62);
            this.PanelOption.TabIndex = 55;
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
            this.BCancel.Location = new System.Drawing.Point(527, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 4;
            this.BCancel.TabStop = false;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // RatesGroupControl
            // 
            this.RatesGroupControl.Controls.Add(this.ProgressPanel);
            this.RatesGroupControl.Controls.Add(this.ExchangesGridControl);
            this.RatesGroupControl.Controls.Add(this.StandaloneBarDockControl);
            this.RatesGroupControl.Location = new System.Drawing.Point(17, 166);
            this.RatesGroupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RatesGroupControl.Name = "RatesGroupControl";
            this.RatesGroupControl.Size = new System.Drawing.Size(597, 283);
            this.RatesGroupControl.TabIndex = 56;
            this.RatesGroupControl.Text = "Valyuta məzənnələri";
            // 
            // ProgressPanel
            // 
            this.ProgressPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ProgressPanel.Appearance.Options.UseBackColor = true;
            this.ProgressPanel.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ProgressPanel.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.ProgressPanel.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ProgressPanel.AppearanceCaption.Options.UseFont = true;
            this.ProgressPanel.AppearanceCaption.Options.UseForeColor = true;
            this.ProgressPanel.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.ProgressPanel.AppearanceDescription.FontStyleDelta = System.Drawing.FontStyle.Italic;
            this.ProgressPanel.AppearanceDescription.Options.UseFont = true;
            this.ProgressPanel.BarAnimationElementThickness = 2;
            this.ProgressPanel.Caption = "Gözləyin...";
            this.ProgressPanel.Description = "Məzənnələr yüklənilir ...";
            this.ProgressPanel.Location = new System.Drawing.Point(154, 26);
            this.ProgressPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ProgressPanel.Name = "ProgressPanel";
            this.ProgressPanel.Size = new System.Drawing.Size(323, 81);
            this.ProgressPanel.TabIndex = 67;
            this.ProgressPanel.Text = "progressPanel1";
            this.ProgressPanel.Visible = false;
            // 
            // ExchangesGridControl
            // 
            this.ExchangesGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExchangesGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExchangesGridControl.Location = new System.Drawing.Point(39, 25);
            this.ExchangesGridControl.MainView = this.ExchangesGridView;
            this.ExchangesGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExchangesGridControl.Name = "ExchangesGridControl";
            this.ExchangesGridControl.Size = new System.Drawing.Size(556, 256);
            this.ExchangesGridControl.TabIndex = 56;
            this.ExchangesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ExchangesGridView});
            // 
            // ExchangesGridView
            // 
            this.ExchangesGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.ExchangesGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.ExchangesGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.ExchangesGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ExchangesGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ExchangesGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.ExchangesGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ExchangesGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ExchangesGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.ExchangesGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ExchangesGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ExchangesGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ExchangesGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.ExchangesGridView.FormatRules.Add(gridFormatRule1);
            this.ExchangesGridView.GridControl = this.ExchangesGridControl;
            this.ExchangesGridView.Name = "ExchangesGridView";
            this.ExchangesGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.ExchangesGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.ExchangesGridView.OptionsBehavior.Editable = false;
            this.ExchangesGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.ExchangesGridView.OptionsFind.FindDelay = 100;
            this.ExchangesGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.ExchangesGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.ExchangesGridView.OptionsView.ShowGroupPanel = false;
            this.ExchangesGridView.OptionsView.ShowIndicator = false;
            this.ExchangesGridView.PaintStyleName = "Skin";
            this.ExchangesGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.ExchangesGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.ExchangesGridView_RowCellStyle);
            this.ExchangesGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.ExchangesGridView_FocusedRowObjectChanged);
            this.ExchangesGridView.ColumnFilterChanged += new System.EventHandler(this.ExchangesGridView_ColumnFilterChanged);
            this.ExchangesGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.ExchangesGridView_CustomColumnDisplayText);
            this.ExchangesGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExchangesGridView_MouseUp);
            this.ExchangesGridView.DoubleClick += new System.EventHandler(this.ExchangesGridView_DoubleClick);
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(2, 25);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 256);
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
            this.ExportBarButton,
            this.UpgradeBarButton,
            this.RateChartBarButton});
            this.BarManager.MainMenu = this.Bar;
            this.BarManager.MaxItemId = 8;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.UpgradeBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RateChartBarButton)});
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
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 4;
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
            this.ExportBarButton.Id = 5;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Insert));
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // UpgradeBarButton
            // 
            this.UpgradeBarButton.Caption = "Mərkəzi Bankın rəsmi məzənnələrini yüklə";
            this.UpgradeBarButton.Id = 6;
            this.UpgradeBarButton.ImageOptions.Image = global::CRS.Properties.Resources.explorer_16;
            this.UpgradeBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M));
            this.UpgradeBarButton.Name = "UpgradeBarButton";
            this.UpgradeBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem1.Text = "<color=255,0,0>Mərkəzi Bankın rəsmi məzənnələrini yüklə</color>";
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Seçilmiş tarix üçün bazada olan valyutaların məzənnələrini <b><color=104,6,6>Mərk" +
    "əzi Bank</color></b>ın bazasından avtomatik yükləmək üçün nəzərdə tutulub.";
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = resources.GetString("toolTipTitleItem2.Text");
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipSeparatorItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.UpgradeBarButton.SuperTip = superToolTip1;
            this.UpgradeBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.UpgradeBarButton_ItemClick);
            // 
            // RateChartBarButton
            // 
            this.RateChartBarButton.Caption = "Məzənnələrin qrafik təsviri";
            this.RateChartBarButton.Id = 7;
            this.RateChartBarButton.ImageOptions.Image = global::CRS.Properties.Resources.chart_161;
            this.RateChartBarButton.Name = "RateChartBarButton";
            this.RateChartBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RateChartBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(629, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 518);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(629, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 518);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(629, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 518);
            // 
            // AmountValue
            // 
            this.AmountValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AmountValue.Location = new System.Drawing.Point(21, 41);
            this.AmountValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AmountValue.Name = "AmountValue";
            this.AmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.AmountValue.Properties.NullValuePrompt = "Məbləği daxil edin";
            this.AmountValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.AmountValue.Properties.Precision = 10;
            this.AmountValue.Size = new System.Drawing.Size(177, 22);
            this.AmountValue.TabIndex = 0;
            this.AmountValue.EditValueChanged += new System.EventHandler(this.AmountValue_EditValueChanged);
            // 
            // Currency1ComboBox
            // 
            this.Currency1ComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Currency1ComboBox.Location = new System.Drawing.Point(205, 41);
            this.Currency1ComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Currency1ComboBox.Name = "Currency1ComboBox";
            this.Currency1ComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç")});
            this.Currency1ComboBox.Properties.NullValuePrompt = "Seçin";
            this.Currency1ComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.Currency1ComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Currency1ComboBox.Size = new System.Drawing.Size(68, 22);
            this.Currency1ComboBox.TabIndex = 1;
            this.Currency1ComboBox.SelectedIndexChanged += new System.EventHandler(this.Currency1ComboBox_SelectedIndexChanged);
            // 
            // ExchangeGroupControl
            // 
            this.ExchangeGroupControl.Controls.Add(this.ResultText);
            this.ExchangeGroupControl.Controls.Add(this.Currency2ComboBox);
            this.ExchangeGroupControl.Controls.Add(this.labelControl1);
            this.ExchangeGroupControl.Controls.Add(this.ChangePictureBox);
            this.ExchangeGroupControl.Controls.Add(this.AmountValue);
            this.ExchangeGroupControl.Controls.Add(this.Currency1ComboBox);
            this.ExchangeGroupControl.Location = new System.Drawing.Point(17, 47);
            this.ExchangeGroupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExchangeGroupControl.Name = "ExchangeGroupControl";
            this.ExchangeGroupControl.Size = new System.Drawing.Size(597, 112);
            this.ExchangeGroupControl.TabIndex = 69;
            this.ExchangeGroupControl.Text = "Çevirici";
            // 
            // ResultText
            // 
            this.ResultText.Location = new System.Drawing.Point(306, 41);
            this.ResultText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ResultText.MenuManager = this.BarManager;
            this.ResultText.Name = "ResultText";
            this.ResultText.Properties.ReadOnly = true;
            this.ResultText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ResultText.Size = new System.Drawing.Size(197, 22);
            this.ResultText.TabIndex = 72;
            // 
            // Currency2ComboBox
            // 
            this.Currency2ComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Currency2ComboBox.Location = new System.Drawing.Point(510, 41);
            this.Currency2ComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Currency2ComboBox.Name = "Currency2ComboBox";
            this.Currency2ComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç")});
            this.Currency2ComboBox.Properties.NullValuePrompt = "Seçin";
            this.Currency2ComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.Currency2ComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.Currency2ComboBox.Size = new System.Drawing.Size(68, 22);
            this.Currency2ComboBox.TabIndex = 2;
            this.Currency2ComboBox.SelectedIndexChanged += new System.EventHandler(this.Currency2ComboBox_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(364, 86);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(229, 17);
            this.labelControl1.TabIndex = 70;
            this.labelControl1.Text = "Hər bir valyuta AZN-ə qarşı hesablanır";
            // 
            // ChangePictureBox
            // 
            this.ChangePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ChangePictureBox.Image = global::CRS.Properties.Resources.swap_horiz_16;
            this.ChangePictureBox.Location = new System.Drawing.Point(280, 47);
            this.ChangePictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChangePictureBox.Name = "ChangePictureBox";
            this.ChangePictureBox.Size = new System.Drawing.Size(16, 12);
            this.ChangePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ChangePictureBox.TabIndex = 69;
            this.ChangePictureBox.TabStop = false;
            this.ChangePictureBox.Click += new System.EventHandler(this.ChangePictureBox_Click);
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.UpgradeBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RateChartBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // DateValue
            // 
            this.DateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DateValue.EditValue = null;
            this.DateValue.Location = new System.Drawing.Point(286, 15);
            this.DateValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateValue.Name = "DateValue";
            this.DateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.DateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.DateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.DateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DateValue.Size = new System.Drawing.Size(107, 22);
            this.DateValue.TabIndex = 115;
            this.DateValue.TabStop = false;
            this.DateValue.EditValueChanged += new System.EventHandler(this.DateValue_EditValueChanged);
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(240, 18);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(30, 16);
            this.DateLabel.TabIndex = 116;
            this.DateLabel.Text = "Tarix";
            // 
            // FExchangeCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(629, 518);
            this.Controls.Add(this.DateValue);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.ExchangeGroupControl);
            this.Controls.Add(this.RatesGroupControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 468);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(557, 468);
            this.Name = "FExchangeCalculator";
            this.ShowInTaskbar = false;
            this.Text = "Valyuta çevirici";
            this.Load += new System.EventHandler(this.FExchangeCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RatesGroupControl)).EndInit();
            this.RatesGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExchangesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExchangesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Currency1ComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExchangeGroupControl)).EndInit();
            this.ExchangeGroupControl.ResumeLayout(false);
            this.ExchangeGroupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Currency2ComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChangePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.GroupControl RatesGroupControl;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar Bar;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl ExchangesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ExchangesGridView;
        private DevExpress.XtraEditors.CalcEdit AmountValue;
        private DevExpress.XtraEditors.ComboBoxEdit Currency1ComboBox;
        private DevExpress.XtraEditors.GroupControl ExchangeGroupControl;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.PictureBox ChangePictureBox;
        private DevExpress.XtraEditors.TextEdit ResultText;
        private DevExpress.XtraEditors.ComboBoxEdit Currency2ComboBox;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraEditors.DateEdit DateValue;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraBars.BarButtonItem UpgradeBarButton;
        private DevExpress.XtraWaitForm.ProgressPanel ProgressPanel;
        private DevExpress.XtraBars.BarButtonItem RateChartBarButton;
    }
}