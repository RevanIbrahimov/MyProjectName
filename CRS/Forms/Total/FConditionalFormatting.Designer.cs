namespace CRS.Forms.Total
{
    partial class FConditionalFormatting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConditionalFormatting));
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
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.FontGridControl = new DevExpress.XtraGrid.GridControl();
            this.FontGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 498);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(742, 62);
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
            this.BCancel.Location = new System.Drawing.Point(638, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 0);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 498);
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
            this.RefreshBarButton});
            this.BarManager.MainMenu = this.Bar;
            this.BarManager.MaxItemId = 4;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton)});
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
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(742, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 560);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(742, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 560);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(742, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 560);
            // 
            // FontGridControl
            // 
            this.FontGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FontGridControl.Location = new System.Drawing.Point(37, 0);
            this.FontGridControl.MainView = this.FontGridView;
            this.FontGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FontGridControl.Name = "FontGridControl";
            this.FontGridControl.Size = new System.Drawing.Size(705, 498);
            this.FontGridControl.TabIndex = 71;
            this.FontGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.FontGridView});
            // 
            // FontGridView
            // 
            this.FontGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.FontGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.FontGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.FontGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.FontGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FontGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.FontGridView.Appearance.GroupFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.FontGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.FontGridView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.FontGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.FontGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FontGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.FontGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.FontGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.FontGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.FontGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FontGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.FontGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.FontGridView.FormatRules.Add(gridFormatRule1);
            this.FontGridView.GridControl = this.FontGridControl;
            this.FontGridView.Name = "FontGridView";
            this.FontGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.FontGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.FontGridView.OptionsBehavior.Editable = false;
            this.FontGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.FontGridView.OptionsFind.FindDelay = 100;
            this.FontGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.FontGridView.OptionsView.ShowFooter = true;
            this.FontGridView.OptionsView.ShowGroupPanel = false;
            this.FontGridView.OptionsView.ShowIndicator = false;
            this.FontGridView.PaintStyleName = "Skin";
            this.FontGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.FontGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.FontGridView_RowCellStyle);
            this.FontGridView.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.FontGridView_RowStyle);
            this.FontGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.FontGridView_FocusedRowObjectChanged);
            this.FontGridView.ColumnFilterChanged += new System.EventHandler(this.FontGridView_ColumnFilterChanged);
            this.FontGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.FormatGridView_CustomColumnDisplayText);
            this.FontGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormatGridView_MouseUp);
            this.FontGridView.DoubleClick += new System.EventHandler(this.FontGridView_DoubleClick);
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
            // FConditionalFormatting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 560);
            this.Controls.Add(this.FontGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FConditionalFormatting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Şərtlərə görə formatlar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FConditionalFormatting_FormClosing);
            this.Load += new System.EventHandler(this.FConditionalFormatting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontGridView)).EndInit();
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
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem NewBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraGrid.GridControl FontGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView FontGridView;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
    }
}