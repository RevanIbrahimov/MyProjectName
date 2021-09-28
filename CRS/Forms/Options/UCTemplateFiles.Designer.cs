namespace CRS.Forms.Options
{
    partial class UCTemplateFiles
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCTemplateFiles));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.GroupControl = new DevExpress.XtraEditors.GroupControl();
            this.DetailGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.FilesGridControl = new DevExpress.XtraGrid.GridControl();
            this.FilesGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Files_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Files_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.File_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Files_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Files_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Files_UpdateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Files_UsedUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.Bar = new DevExpress.XtraBars.Bar();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ShowBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).BeginInit();
            this.GroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailGroupControl)).BeginInit();
            this.DetailGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupControl
            // 
            this.GroupControl.AppearanceCaption.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupControl.AppearanceCaption.ForeColor = System.Drawing.Color.DarkRed;
            this.GroupControl.AppearanceCaption.Options.UseFont = true;
            this.GroupControl.AppearanceCaption.Options.UseForeColor = true;
            this.GroupControl.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("GroupControl.CaptionImageOptions.Image")));
            this.GroupControl.Controls.Add(this.DetailGroupControl);
            this.GroupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupControl.Location = new System.Drawing.Point(0, 0);
            this.GroupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupControl.Name = "GroupControl";
            this.GroupControl.Size = new System.Drawing.Size(941, 623);
            this.GroupControl.TabIndex = 2;
            this.GroupControl.Text = "Şablon fayllar";
            this.GroupControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupControl_Paint);
            // 
            // DetailGroupControl
            // 
            this.DetailGroupControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailGroupControl.Controls.Add(this.FilesGridControl);
            this.DetailGroupControl.Controls.Add(this.StandaloneBarDockControl);
            this.DetailGroupControl.Location = new System.Drawing.Point(17, 37);
            this.DetailGroupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DetailGroupControl.Name = "DetailGroupControl";
            this.DetailGroupControl.Size = new System.Drawing.Size(908, 513);
            this.DetailGroupControl.TabIndex = 4;
            // 
            // FilesGridControl
            // 
            this.FilesGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FilesGridControl.Location = new System.Drawing.Point(39, 25);
            this.FilesGridControl.MainView = this.FilesGridView;
            this.FilesGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FilesGridControl.Name = "FilesGridControl";
            this.FilesGridControl.Size = new System.Drawing.Size(867, 486);
            this.FilesGridControl.TabIndex = 2;
            this.FilesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.FilesGridView});
            // 
            // FilesGridView
            // 
            this.FilesGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.FilesGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.FilesGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.FilesGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.FilesGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FilesGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.FilesGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.FilesGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FilesGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.FilesGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.FilesGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.FilesGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.FilesGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.FilesGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.FilesGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FilesGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.FilesGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Files_ID,
            this.Files_SS,
            this.File_Name,
            this.Files_Note,
            this.Files_InsertDate,
            this.Files_UpdateDate,
            this.Files_UsedUserID});
            this.FilesGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleExpression1.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression1.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule1.Rule = formatConditionRuleExpression1;
            this.FilesGridView.FormatRules.Add(gridFormatRule1);
            this.FilesGridView.GridControl = this.FilesGridControl;
            this.FilesGridView.Name = "FilesGridView";
            this.FilesGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.FilesGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.FilesGridView.OptionsBehavior.Editable = false;
            this.FilesGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.FilesGridView.OptionsFind.FindDelay = 100;
            this.FilesGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.FilesGridView.OptionsView.ShowGroupPanel = false;
            this.FilesGridView.OptionsView.ShowIndicator = false;
            this.FilesGridView.PaintStyleName = "Skin";
            this.FilesGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.FilesGridView.ViewCaption = "11300";
            this.FilesGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.FilesGridView_RowCellStyle);
            this.FilesGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.FilesGridView_FocusedRowObjectChanged);
            this.FilesGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.FilesGridView_CustomColumnDisplayText);
            this.FilesGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FilesGridView_MouseUp);
            this.FilesGridView.DoubleClick += new System.EventHandler(this.FilesGridView_DoubleClick);
            // 
            // Files_ID
            // 
            this.Files_ID.Caption = "ID";
            this.Files_ID.FieldName = "ID";
            this.Files_ID.Name = "Files_ID";
            // 
            // Files_SS
            // 
            this.Files_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Files_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Files_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Files_SS.Caption = "S/s";
            this.Files_SS.FieldName = "SS";
            this.Files_SS.Name = "Files_SS";
            this.Files_SS.OptionsColumn.FixedWidth = true;
            this.Files_SS.Visible = true;
            this.Files_SS.VisibleIndex = 0;
            this.Files_SS.Width = 50;
            // 
            // File_Name
            // 
            this.File_Name.Caption = "Şablonun adı";
            this.File_Name.FieldName = "NAME";
            this.File_Name.Name = "File_Name";
            this.File_Name.OptionsColumn.FixedWidth = true;
            this.File_Name.Visible = true;
            this.File_Name.VisibleIndex = 1;
            this.File_Name.Width = 250;
            // 
            // Files_Note
            // 
            this.Files_Note.Caption = "Qeyd";
            this.Files_Note.FieldName = "NOTE";
            this.Files_Note.Name = "Files_Note";
            this.Files_Note.Visible = true;
            this.Files_Note.VisibleIndex = 2;
            this.Files_Note.Width = 337;
            // 
            // Files_InsertDate
            // 
            this.Files_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Files_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Files_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Files_InsertDate.Caption = "Daxil edilmə vaxtı";
            this.Files_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Files_InsertDate.FieldName = "INSERT_DATE";
            this.Files_InsertDate.Name = "Files_InsertDate";
            this.Files_InsertDate.OptionsColumn.FixedWidth = true;
            this.Files_InsertDate.Visible = true;
            this.Files_InsertDate.VisibleIndex = 3;
            this.Files_InsertDate.Width = 115;
            // 
            // Files_UpdateDate
            // 
            this.Files_UpdateDate.AppearanceCell.Options.UseTextOptions = true;
            this.Files_UpdateDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Files_UpdateDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Files_UpdateDate.Caption = "Dəyişilmə vaxtı";
            this.Files_UpdateDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
            this.Files_UpdateDate.FieldName = "UPDATE_DATE";
            this.Files_UpdateDate.Name = "Files_UpdateDate";
            this.Files_UpdateDate.OptionsColumn.FixedWidth = true;
            this.Files_UpdateDate.Visible = true;
            this.Files_UpdateDate.VisibleIndex = 4;
            this.Files_UpdateDate.Width = 115;
            // 
            // Files_UsedUserID
            // 
            this.Files_UsedUserID.Caption = "UsedUserID";
            this.Files_UsedUserID.FieldName = "USED_USER_ID";
            this.Files_UsedUserID.Name = "Files_UsedUserID";
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(2, 25);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 486);
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
            this.EditBarButton,
            this.RefreshBarButton,
            this.ShowBarButton});
            this.BarManager.MainMenu = this.Bar;
            this.BarManager.MaxItemId = 3;
            // 
            // Bar
            // 
            this.Bar.BarName = "Main menu";
            this.Bar.DockCol = 0;
            this.Bar.DockRow = 0;
            this.Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ShowBarButton)});
            this.Bar.OptionsBar.DrawBorder = false;
            this.Bar.OptionsBar.DrawDragBorder = false;
            this.Bar.OptionsBar.MultiLine = true;
            this.Bar.OptionsBar.UseWholeRow = true;
            this.Bar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.Bar.Text = "Main menu";
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Dəyiş";
            this.EditBarButton.Id = 0;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_16;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 1;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.LargeImage")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // ShowBarButton
            // 
            this.ShowBarButton.Caption = "Fayla bax";
            this.ShowBarButton.Id = 2;
            this.ShowBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ShowBarButton.ImageOptions.Image")));
            this.ShowBarButton.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ShowBarButton.ImageOptions.LargeImage")));
            this.ShowBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.ShowBarButton.Name = "ShowBarButton";
            this.ShowBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.ShowBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowBarButton_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(941, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 623);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(941, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 623);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(941, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 623);
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ShowBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // UCTemplateFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCTemplateFiles";
            this.Size = new System.Drawing.Size(941, 623);
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).EndInit();
            this.GroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailGroupControl)).EndInit();
            this.DetailGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GroupControl;
        private DevExpress.XtraEditors.GroupControl DetailGroupControl;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar Bar;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl FilesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView FilesGridView;
        private DevExpress.XtraGrid.Columns.GridColumn File_Name;
        private DevExpress.XtraGrid.Columns.GridColumn Files_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Files_Note;
        private DevExpress.XtraGrid.Columns.GridColumn Files_InsertDate;
        private DevExpress.XtraGrid.Columns.GridColumn Files_UpdateDate;
        private DevExpress.XtraGrid.Columns.GridColumn Files_SS;
        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem ShowBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Files_UsedUserID;
    }
}
