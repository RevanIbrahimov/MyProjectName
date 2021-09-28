namespace CRS.Forms.Contracts
{
    partial class FLaws
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLaws));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
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
            this.LawsGridControl = new DevExpress.XtraGrid.GridControl();
            this.LawsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LawsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LawsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 299);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(743, 50);
            this.PanelOption.TabIndex = 51;
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
            this.BCancel.Location = new System.Drawing.Point(654, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
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
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(32, 299);
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
            this.NewBarButton.Glyph = global::CRS.Properties.Resources.plus_16;
            this.NewBarButton.Id = 0;
            this.NewBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.NewBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Glyph = global::CRS.Properties.Resources.edit_16;
            this.EditBarButton.Id = 1;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Sil";
            this.DeleteBarButton.Glyph = global::CRS.Properties.Resources.minus_16;
            this.DeleteBarButton.Id = 2;
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete);
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Glyph = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.Glyph")));
            this.RefreshBarButton.Id = 3;
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.LargeGlyph")));
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap";
            this.PrintBarButton.Glyph = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.Glyph")));
            this.PrintBarButton.Id = 4;
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.LargeGlyph")));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.PrintBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintBarButton_ItemClick);
            // 
            // ExportBarButton
            // 
            this.ExportBarButton.Caption = "İxrac";
            this.ExportBarButton.Glyph = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Id = 5;
            this.ExportBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Insert));
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(743, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 349);
            this.barDockControlBottom.Size = new System.Drawing.Size(743, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 349);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(743, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 349);
            // 
            // LawsGridControl
            // 
            this.LawsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LawsGridControl.Location = new System.Drawing.Point(32, 0);
            this.LawsGridControl.MainView = this.LawsGridView;
            this.LawsGridControl.Name = "LawsGridControl";
            this.LawsGridControl.Size = new System.Drawing.Size(711, 299);
            this.LawsGridControl.TabIndex = 61;
            this.LawsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.LawsGridView});
            // 
            // LawsGridView
            // 
            this.LawsGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.LawsGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.LawsGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.LawsGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.LawsGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LawsGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.LawsGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.LawsGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.LawsGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.LawsGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.LawsGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LawsGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LawsGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.LawsGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.LawsGridView.FormatRules.Add(gridFormatRule1);
            this.LawsGridView.GridControl = this.LawsGridControl;
            this.LawsGridView.Name = "LawsGridView";
            this.LawsGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.LawsGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.LawsGridView.OptionsBehavior.Editable = false;
            this.LawsGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.LawsGridView.OptionsFind.FindDelay = 100;
            this.LawsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.LawsGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.LawsGridView.OptionsView.ShowFooter = true;
            this.LawsGridView.OptionsView.ShowGroupPanel = false;
            this.LawsGridView.OptionsView.ShowIndicator = false;
            this.LawsGridView.PaintStyleName = "Skin";
            this.LawsGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.LawsGridView.ViewCaption = "11300";
            this.LawsGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.LawsGridView_CustomDrawFooterCell);
            this.LawsGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.LawsGridView_FocusedRowObjectChanged);
            this.LawsGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.LawsGridView_CustomColumnDisplayText);
            this.LawsGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LawsGridView_MouseUp);
            this.LawsGridView.DoubleClick += new System.EventHandler(this.LawsGridView_DoubleClick);
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.NewBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // FLaws
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 349);
            this.Controls.Add(this.LawsGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FLaws";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Məhkəmələrin siyahısı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FLaws_FormClosing);
            this.Load += new System.EventHandler(this.FLaws_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LawsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LawsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);

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
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl LawsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView LawsGridView;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
    }
}