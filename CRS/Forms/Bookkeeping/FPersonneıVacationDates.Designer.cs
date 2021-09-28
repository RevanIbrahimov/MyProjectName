namespace CRS.Forms.Bookkeeping
{
    partial class FPersonneıVacationDates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPersonneıVacationDates));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.VacationsBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.DatesGridControl = new DevExpress.XtraGrid.GridControl();
            this.DatesGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Dates_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_PersonnelName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_PositionName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_Period = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_DayCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_UsedVacation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_DebtVacation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_PersonnelStatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_PersonnelID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Dates_EndDate = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.RefreshBarButton,
            this.PrintBarButton,
            this.ExportBarSubItem,
            this.VacationsBarButton});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 5;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1630, 179);
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // RefreshBarButton
            // 
            this.RefreshBarButton.Caption = "Təzələ";
            this.RefreshBarButton.Id = 1;
            this.RefreshBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RefreshBarButton.ImageOptions.Image")));
            this.RefreshBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.RefreshBarButton.Name = "RefreshBarButton";
            this.RefreshBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.RefreshBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.RefreshBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshBarButton_ItemClick);
            // 
            // PrintBarButton
            // 
            this.PrintBarButton.Caption = "Çap et";
            this.PrintBarButton.Id = 2;
            this.PrintBarButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("PrintBarButton.ImageOptions.Image")));
            this.PrintBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.PrintBarButton.Name = "PrintBarButton";
            this.PrintBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.PrintBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            // 
            // ExportBarSubItem
            // 
            this.ExportBarSubItem.Caption = "İxrac et";
            this.ExportBarSubItem.Id = 3;
            this.ExportBarSubItem.ImageOptions.Image = global::CRS.Properties.Resources.table_export_32;
            this.ExportBarSubItem.ItemAppearance.Disabled.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Disabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Hovered.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Normal.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.ItemAppearance.Pressed.Options.UseTextOptions = true;
            this.ExportBarSubItem.ItemAppearance.Pressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.ExportBarSubItem.Name = "ExportBarSubItem";
            this.ExportBarSubItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // VacationsBarButton
            // 
            this.VacationsBarButton.Caption = "Məzuniyyətlər";
            this.VacationsBarButton.Id = 4;
            this.VacationsBarButton.ImageOptions.Image = global::CRS.Properties.Resources.calendar_week_32;
            this.VacationsBarButton.Name = "VacationsBarButton";
            this.VacationsBarButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.VacationsBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.VacationsBarButton_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Məzuniyyətlər";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.VacationsBarButton);
            this.ribbonPageGroup1.ItemLinks.Add(this.RefreshBarButton);
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
            // DatesGridControl
            // 
            this.DatesGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DatesGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DatesGridControl.Location = new System.Drawing.Point(0, 179);
            this.DatesGridControl.MainView = this.DatesGridView;
            this.DatesGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DatesGridControl.Name = "DatesGridControl";
            this.DatesGridControl.Size = new System.Drawing.Size(1630, 618);
            this.DatesGridControl.TabIndex = 95;
            this.DatesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DatesGridView});
            // 
            // DatesGridView
            // 
            this.DatesGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.DatesGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.DatesGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.DatesGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DatesGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DatesGridView.Appearance.GroupFooter.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.DatesGridView.Appearance.GroupFooter.Options.UseFont = true;
            this.DatesGridView.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.DatesGridView.Appearance.GroupFooter.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DatesGridView.Appearance.GroupFooter.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DatesGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.DatesGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DatesGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.DatesGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Dates_SS,
            this.Dates_ID,
            this.Dates_PersonnelName,
            this.Dates_PositionName,
            this.Dates_Period,
            this.Dates_DayCount,
            this.Dates_UsedVacation,
            this.Dates_DebtVacation,
            this.Dates_PersonnelStatusID,
            this.Dates_PersonnelID,
            this.Dates_StartDate,
            this.Dates_EndDate});
            this.DatesGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.DatesGridView.GridControl = this.DatesGridControl;
            this.DatesGridView.GroupCount = 1;
            this.DatesGridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "DAY_COUNT", this.Dates_DayCount, "{0:n0}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "USED_DAY_COUNT", this.Dates_UsedVacation, "{0:n0}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "DEBT_DAY_COUNT", this.Dates_DebtVacation, "{0:n0}")});
            this.DatesGridView.Name = "DatesGridView";
            this.DatesGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.DatesGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.DatesGridView.OptionsBehavior.Editable = false;
            this.DatesGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.DatesGridView.OptionsFind.FindDelay = 100;
            this.DatesGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.DatesGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.DatesGridView.OptionsView.ShowFooter = true;
            this.DatesGridView.OptionsView.ShowGroupPanel = false;
            this.DatesGridView.OptionsView.ShowIndicator = false;
            this.DatesGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Dates_PersonnelName, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.DatesGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.DatesGridView_FocusedRowObjectChanged);
            this.DatesGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.DatesGridView_CustomColumnDisplayText);
            this.DatesGridView.DoubleClick += new System.EventHandler(this.DatesGridView_DoubleClick);
            // 
            // Dates_SS
            // 
            this.Dates_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Dates_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Dates_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Dates_SS.Caption = "S/s";
            this.Dates_SS.FieldName = "SS";
            this.Dates_SS.Name = "Dates_SS";
            this.Dates_SS.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.Dates_SS.OptionsColumn.FixedWidth = true;
            this.Dates_SS.Width = 45;
            // 
            // Dates_ID
            // 
            this.Dates_ID.Caption = "ID";
            this.Dates_ID.FieldName = "ID";
            this.Dates_ID.Name = "Dates_ID";
            // 
            // Dates_PersonnelName
            // 
            this.Dates_PersonnelName.Caption = "İşçinin tam adı";
            this.Dates_PersonnelName.FieldName = "PERSONNEL_NAME";
            this.Dates_PersonnelName.Name = "Dates_PersonnelName";
            this.Dates_PersonnelName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.Dates_PersonnelName.OptionsColumn.FixedWidth = true;
            this.Dates_PersonnelName.Visible = true;
            this.Dates_PersonnelName.VisibleIndex = 1;
            this.Dates_PersonnelName.Width = 100;
            // 
            // Dates_PositionName
            // 
            this.Dates_PositionName.Caption = "Vəzifəsi";
            this.Dates_PositionName.FieldName = "POSITION_NAME";
            this.Dates_PositionName.Name = "Dates_PositionName";
            this.Dates_PositionName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.Dates_PositionName.OptionsColumn.FixedWidth = true;
            this.Dates_PositionName.Width = 120;
            // 
            // Dates_Period
            // 
            this.Dates_Period.AppearanceCell.Options.UseTextOptions = true;
            this.Dates_Period.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Dates_Period.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Dates_Period.Caption = "Məzuniyyət dövrü";
            this.Dates_Period.FieldName = "PERIOD";
            this.Dates_Period.Name = "Dates_Period";
            this.Dates_Period.OptionsColumn.FixedWidth = true;
            this.Dates_Period.Visible = true;
            this.Dates_Period.VisibleIndex = 0;
            this.Dates_Period.Width = 130;
            // 
            // Dates_DayCount
            // 
            this.Dates_DayCount.AppearanceCell.Options.UseTextOptions = true;
            this.Dates_DayCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Dates_DayCount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Dates_DayCount.Caption = "Məzuniyyət günlərinin sayı";
            this.Dates_DayCount.DisplayFormat.FormatString = "n0";
            this.Dates_DayCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Dates_DayCount.FieldName = "DAY_COUNT";
            this.Dates_DayCount.Name = "Dates_DayCount";
            this.Dates_DayCount.OptionsColumn.FixedWidth = true;
            this.Dates_DayCount.Visible = true;
            this.Dates_DayCount.VisibleIndex = 1;
            this.Dates_DayCount.Width = 100;
            // 
            // Dates_UsedVacation
            // 
            this.Dates_UsedVacation.AppearanceCell.Options.UseTextOptions = true;
            this.Dates_UsedVacation.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Dates_UsedVacation.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Dates_UsedVacation.Caption = "İstifadə olunan məzuniyyət";
            this.Dates_UsedVacation.DisplayFormat.FormatString = "n0";
            this.Dates_UsedVacation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Dates_UsedVacation.FieldName = "USED_DAY_COUNT";
            this.Dates_UsedVacation.Name = "Dates_UsedVacation";
            this.Dates_UsedVacation.OptionsColumn.FixedWidth = true;
            this.Dates_UsedVacation.Visible = true;
            this.Dates_UsedVacation.VisibleIndex = 2;
            this.Dates_UsedVacation.Width = 100;
            // 
            // Dates_DebtVacation
            // 
            this.Dates_DebtVacation.AppearanceCell.Options.UseTextOptions = true;
            this.Dates_DebtVacation.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Dates_DebtVacation.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Dates_DebtVacation.Caption = "Qalan məzuniyyət";
            this.Dates_DebtVacation.DisplayFormat.FormatString = "n0";
            this.Dates_DebtVacation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Dates_DebtVacation.FieldName = "DEBT_DAY_COUNT";
            this.Dates_DebtVacation.Name = "Dates_DebtVacation";
            this.Dates_DebtVacation.OptionsColumn.FixedWidth = true;
            this.Dates_DebtVacation.Visible = true;
            this.Dates_DebtVacation.VisibleIndex = 3;
            this.Dates_DebtVacation.Width = 100;
            // 
            // Dates_PersonnelStatusID
            // 
            this.Dates_PersonnelStatusID.Caption = "PersonnelStatusID";
            this.Dates_PersonnelStatusID.FieldName = "PERSONNEL_STATUS_ID";
            this.Dates_PersonnelStatusID.Name = "Dates_PersonnelStatusID";
            // 
            // Dates_PersonnelID
            // 
            this.Dates_PersonnelID.Caption = "PersonnelID";
            this.Dates_PersonnelID.FieldName = "PERSONNEL_ID";
            this.Dates_PersonnelID.Name = "Dates_PersonnelID";
            // 
            // Dates_StartDate
            // 
            this.Dates_StartDate.Caption = "StartDate";
            this.Dates_StartDate.FieldName = "START_DATE";
            this.Dates_StartDate.Name = "Dates_StartDate";
            // 
            // Dates_EndDate
            // 
            this.Dates_EndDate.Caption = "EndDate";
            this.Dates_EndDate.FieldName = "END_DATE";
            this.Dates_EndDate.Name = "Dates_EndDate";
            // 
            // FPersonneıVacationDates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1630, 797);
            this.Controls.Add(this.DatesGridControl);
            this.Controls.Add(this.ribbon);
            this.MinimizeBox = false;
            this.Name = "FPersonneıVacationDates";
            this.Ribbon = this.ribbon;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Məzuniyyətlər";
            this.Load += new System.EventHandler(this.FPersonneıVacationDates_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraGrid.GridControl DatesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DatesGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_PersonnelName;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_Period;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_DayCount;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_PositionName;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_PersonnelStatusID;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_PersonnelID;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_UsedVacation;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_DebtVacation;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarSubItem ExportBarSubItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem VacationsBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn Dates_EndDate;
    }
}