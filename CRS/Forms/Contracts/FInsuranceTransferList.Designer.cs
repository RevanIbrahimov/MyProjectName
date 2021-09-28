namespace CRS.Forms.Contracts
{
    partial class FInsuranceTransferList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInsuranceTransferList));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            this.PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.DeleteBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PrintBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ExportBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.StandaloneBarDockControl = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ClosedCheck = new DevExpress.XtraEditors.CheckEdit();
            this.ActiveCheck = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ToDateValue = new DevExpress.XtraEditors.DateEdit();
            this.FromDateValue = new DevExpress.XtraEditors.DateEdit();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.InsuranceGridControl = new DevExpress.XtraGrid.GridControl();
            this.InsuranceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Insurance_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_ContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_PayedAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_PayDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_TaskCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Note = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Hostage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_CompanyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_StatusID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Cost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Insurance_Police = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PopupMenu
            // 
            this.PopupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.PopupMenu.Manager = this.BarManager;
            this.PopupMenu.Name = "PopupMenu";
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Köçürnəni dəyiş";
            this.EditBarButton.Id = 3;
            this.EditBarButton.ImageOptions.Image = global::CRS.Properties.Resources.edit_16;
            this.EditBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // DeleteBarButton
            // 
            this.DeleteBarButton.Caption = "Köçürməni sil";
            this.DeleteBarButton.Id = 4;
            this.DeleteBarButton.ImageOptions.Image = global::CRS.Properties.Resources.minus_16;
            this.DeleteBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.DeleteBarButton.Name = "DeleteBarButton";
            this.DeleteBarButton.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.True;
            this.DeleteBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DeleteBarButton_ItemClick);
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
            // ExportBarButton
            // 
            this.ExportBarButton.Caption = "İxrac et";
            this.ExportBarButton.Id = 2;
            this.ExportBarButton.ImageOptions.Image = global::CRS.Properties.Resources.excel_16;
            this.ExportBarButton.Name = "ExportBarButton";
            this.ExportBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExportBarButton_ItemClick);
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
            this.ExportBarButton,
            this.EditBarButton,
            this.DeleteBarButton});
            this.BarManager.MainMenu = this.ToolBar;
            this.BarManager.MaxItemId = 5;
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "Main menu";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.DeleteBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RefreshBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.PrintBarButton, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExportBarButton)});
            this.ToolBar.OptionsBar.DrawBorder = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
            this.ToolBar.StandaloneBarDockControl = this.StandaloneBarDockControl;
            this.ToolBar.Text = "Main menu";
            // 
            // StandaloneBarDockControl
            // 
            this.StandaloneBarDockControl.CausesValidation = false;
            this.StandaloneBarDockControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.StandaloneBarDockControl.Location = new System.Drawing.Point(0, 54);
            this.StandaloneBarDockControl.Manager = this.BarManager;
            this.StandaloneBarDockControl.Name = "StandaloneBarDockControl";
            this.StandaloneBarDockControl.Size = new System.Drawing.Size(37, 658);
            this.StandaloneBarDockControl.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1708, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 712);
            this.barDockControlBottom.Manager = this.BarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1708, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.BarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 712);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1708, 0);
            this.barDockControlRight.Manager = this.BarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 712);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ClosedCheck);
            this.panelControl1.Controls.Add(this.ActiveCheck);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.ToDateValue);
            this.panelControl1.Controls.Add(this.FromDateValue);
            this.panelControl1.Controls.Add(this.DateLabel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1708, 54);
            this.panelControl1.TabIndex = 72;
            // 
            // ClosedCheck
            // 
            this.ClosedCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClosedCheck.Location = new System.Drawing.Point(602, 18);
            this.ClosedCheck.MenuManager = this.BarManager;
            this.ClosedCheck.Name = "ClosedCheck";
            this.ClosedCheck.Properties.Caption = "Bağlanmış müqavilələr";
            this.ClosedCheck.Size = new System.Drawing.Size(160, 20);
            this.ClosedCheck.TabIndex = 234;
            this.ClosedCheck.CheckedChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // ActiveCheck
            // 
            this.ActiveCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActiveCheck.EditValue = true;
            this.ActiveCheck.Location = new System.Drawing.Point(436, 18);
            this.ActiveCheck.MenuManager = this.BarManager;
            this.ActiveCheck.Name = "ActiveCheck";
            this.ActiveCheck.Properties.Caption = "Aktiv müqavilələr";
            this.ActiveCheck.Size = new System.Drawing.Size(160, 20);
            this.ActiveCheck.TabIndex = 233;
            this.ActiveCheck.CheckedChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(241, 20);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(5, 16);
            this.labelControl2.TabIndex = 230;
            this.labelControl2.Text = "-";
            // 
            // ToDateValue
            // 
            this.ToDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ToDateValue.EditValue = null;
            this.ToDateValue.Location = new System.Drawing.Point(252, 17);
            this.ToDateValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ToDateValue.Name = "ToDateValue";
            this.ToDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.ToDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.ToDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ToDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.ToDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.ToDateValue.Size = new System.Drawing.Size(99, 22);
            this.ToDateValue.TabIndex = 228;
            this.ToDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // FromDateValue
            // 
            this.FromDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FromDateValue.EditValue = null;
            this.FromDateValue.Location = new System.Drawing.Point(136, 17);
            this.FromDateValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FromDateValue.Name = "FromDateValue";
            this.FromDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.FromDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromDateValue.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.FromDateValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FromDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.FromDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.FromDateValue.Size = new System.Drawing.Size(99, 22);
            this.FromDateValue.TabIndex = 227;
            this.FromDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(38, 20);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(78, 16);
            this.DateLabel.TabIndex = 229;
            this.DateLabel.Text = "Tarix intervalı";
            // 
            // InsuranceGridControl
            // 
            this.InsuranceGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InsuranceGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Location = new System.Drawing.Point(37, 54);
            this.InsuranceGridControl.MainView = this.InsuranceGridView;
            this.InsuranceGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InsuranceGridControl.Name = "InsuranceGridControl";
            this.InsuranceGridControl.Size = new System.Drawing.Size(1671, 658);
            this.InsuranceGridControl.TabIndex = 75;
            this.InsuranceGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.InsuranceGridView});
            // 
            // InsuranceGridView
            // 
            this.InsuranceGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.InsuranceGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.InsuranceGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.InsuranceGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Maroon;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.InsuranceGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.InsuranceGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.InsuranceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Insurance_SS,
            this.Insurance_ContractCode,
            this.Insurance_Police,
            this.Insurance_PayedAmount,
            this.Insurance_PayDate,
            this.Insurance_TaskCode,
            this.Insurance_Note,
            this.Insurance_Hostage,
            this.Insurance_CompanyName,
            this.Insurance_StatusID,
            this.Insurance_ID,
            this.Insurance_Cost});
            this.InsuranceGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.InsuranceGridView.GridControl = this.InsuranceGridControl;
            this.InsuranceGridView.Name = "InsuranceGridView";
            this.InsuranceGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.InsuranceGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.InsuranceGridView.OptionsBehavior.Editable = false;
            this.InsuranceGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.InsuranceGridView.OptionsFind.FindDelay = 100;
            this.InsuranceGridView.OptionsPrint.RtfPageFooter = resources.GetString("InsuranceGridView.OptionsPrint.RtfPageFooter");
            this.InsuranceGridView.OptionsPrint.RtfReportHeader = resources.GetString("InsuranceGridView.OptionsPrint.RtfReportHeader");
            this.InsuranceGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.InsuranceGridView.OptionsView.ColumnAutoWidth = false;
            this.InsuranceGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.InsuranceGridView.OptionsView.ShowFooter = true;
            this.InsuranceGridView.OptionsView.ShowGroupPanel = false;
            this.InsuranceGridView.OptionsView.ShowIndicator = false;
            this.InsuranceGridView.PaintStyleName = "Skin";
            this.InsuranceGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.InsuranceGridView.ViewCaption = "Sığortaların siyahısı";
            this.InsuranceGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.InsuranceGridView_CustomDrawFooterCell);
            this.InsuranceGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.InsuranceGridView_RowCellStyle);
            this.InsuranceGridView.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.InsuranceGridView_FocusedRowObjectChanged);
            this.InsuranceGridView.ColumnFilterChanged += new System.EventHandler(this.InsuranceGridView_ColumnFilterChanged);
            this.InsuranceGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.InsuranceGridView_CustomUnboundColumnData);
            this.InsuranceGridView.PrintInitialize += new DevExpress.XtraGrid.Views.Base.PrintInitializeEventHandler(this.InsuranceGridView_PrintInitialize);
            this.InsuranceGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InsuranceGridView_MouseUp);
            this.InsuranceGridView.DoubleClick += new System.EventHandler(this.InsuranceGridView_DoubleClick);
            // 
            // Insurance_SS
            // 
            this.Insurance_SS.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_SS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_SS.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_SS.Caption = "S/s";
            this.Insurance_SS.FieldName = "Insurance_SS";
            this.Insurance_SS.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_SS.Name = "Insurance_SS";
            this.Insurance_SS.OptionsColumn.FixedWidth = true;
            this.Insurance_SS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Insurance_SS", "{0}")});
            this.Insurance_SS.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.Insurance_SS.Visible = true;
            this.Insurance_SS.VisibleIndex = 0;
            this.Insurance_SS.Width = 40;
            // 
            // Insurance_ContractCode
            // 
            this.Insurance_ContractCode.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_ContractCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_ContractCode.Caption = "Müqavilə";
            this.Insurance_ContractCode.FieldName = "CONTRACT_CODE";
            this.Insurance_ContractCode.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_ContractCode.Name = "Insurance_ContractCode";
            this.Insurance_ContractCode.OptionsColumn.FixedWidth = true;
            this.Insurance_ContractCode.Visible = true;
            this.Insurance_ContractCode.VisibleIndex = 1;
            this.Insurance_ContractCode.Width = 50;
            // 
            // Insurance_PayedAmount
            // 
            this.Insurance_PayedAmount.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Insurance_PayedAmount.AppearanceCell.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_PayedAmount.AppearanceCell.Options.UseBackColor = true;
            this.Insurance_PayedAmount.AppearanceCell.Options.UseFont = true;
            this.Insurance_PayedAmount.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.Insurance_PayedAmount.AppearanceHeader.Options.UseFont = true;
            this.Insurance_PayedAmount.Caption = "Köçürülüb";
            this.Insurance_PayedAmount.DisplayFormat.FormatString = "n2";
            this.Insurance_PayedAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Insurance_PayedAmount.FieldName = "TRANSFER_AMOUNT";
            this.Insurance_PayedAmount.Name = "Insurance_PayedAmount";
            this.Insurance_PayedAmount.OptionsColumn.FixedWidth = true;
            this.Insurance_PayedAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TRANSFER_AMOUNT", "{0:n2}")});
            this.Insurance_PayedAmount.Visible = true;
            this.Insurance_PayedAmount.VisibleIndex = 3;
            this.Insurance_PayedAmount.Width = 110;
            // 
            // Insurance_PayDate
            // 
            this.Insurance_PayDate.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_PayDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_PayDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_PayDate.Caption = "Köçürmə tarixi";
            this.Insurance_PayDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Insurance_PayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Insurance_PayDate.FieldName = "TRANSFER_DATE";
            this.Insurance_PayDate.Name = "Insurance_PayDate";
            this.Insurance_PayDate.OptionsColumn.FixedWidth = true;
            this.Insurance_PayDate.Visible = true;
            this.Insurance_PayDate.VisibleIndex = 4;
            this.Insurance_PayDate.Width = 80;
            // 
            // Insurance_TaskCode
            // 
            this.Insurance_TaskCode.AppearanceCell.Options.UseTextOptions = true;
            this.Insurance_TaskCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Insurance_TaskCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Insurance_TaskCode.Caption = "Tapşırığın nömrəsi";
            this.Insurance_TaskCode.FieldName = "TASK_CODE";
            this.Insurance_TaskCode.Name = "Insurance_TaskCode";
            this.Insurance_TaskCode.OptionsColumn.FixedWidth = true;
            this.Insurance_TaskCode.Visible = true;
            this.Insurance_TaskCode.VisibleIndex = 5;
            this.Insurance_TaskCode.Width = 100;
            // 
            // Insurance_Note
            // 
            this.Insurance_Note.Caption = "Köçürmə qeydləri";
            this.Insurance_Note.FieldName = "NOTE";
            this.Insurance_Note.Name = "Insurance_Note";
            this.Insurance_Note.Visible = true;
            this.Insurance_Note.VisibleIndex = 6;
            this.Insurance_Note.Width = 250;
            // 
            // Insurance_Hostage
            // 
            this.Insurance_Hostage.Caption = "Lizinq predmeti";
            this.Insurance_Hostage.FieldName = "HOSTAGE";
            this.Insurance_Hostage.Name = "Insurance_Hostage";
            this.Insurance_Hostage.OptionsColumn.FixedWidth = true;
            this.Insurance_Hostage.Visible = true;
            this.Insurance_Hostage.VisibleIndex = 7;
            this.Insurance_Hostage.Width = 700;
            // 
            // Insurance_CompanyName
            // 
            this.Insurance_CompanyName.Caption = "Sığorta şirkəti";
            this.Insurance_CompanyName.FieldName = "COMPANY_NAME";
            this.Insurance_CompanyName.Name = "Insurance_CompanyName";
            this.Insurance_CompanyName.Visible = true;
            this.Insurance_CompanyName.VisibleIndex = 8;
            this.Insurance_CompanyName.Width = 305;
            // 
            // Insurance_StatusID
            // 
            this.Insurance_StatusID.Caption = "StatusID";
            this.Insurance_StatusID.FieldName = "STATUS_ID";
            this.Insurance_StatusID.Name = "Insurance_StatusID";
            this.Insurance_StatusID.OptionsColumn.AllowShowHide = false;
            // 
            // Insurance_ID
            // 
            this.Insurance_ID.Caption = "InsuranceID";
            this.Insurance_ID.FieldName = "INSURANCE_ID";
            this.Insurance_ID.Name = "Insurance_ID";
            this.Insurance_ID.OptionsColumn.AllowShowHide = false;
            // 
            // Insurance_Cost
            // 
            this.Insurance_Cost.Caption = "Sığorta haqqı";
            this.Insurance_Cost.FieldName = "INSURANCE_COST";
            this.Insurance_Cost.Name = "Insurance_Cost";
            this.Insurance_Cost.OptionsColumn.AllowShowHide = false;
            // 
            // Insurance_Police
            // 
            this.Insurance_Police.Caption = "Polisdəki qeydiyyatı";
            this.Insurance_Police.FieldName = "POLICE";
            this.Insurance_Police.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.Insurance_Police.Name = "Insurance_Police";
            this.Insurance_Police.OptionsColumn.FixedWidth = true;
            this.Insurance_Police.Visible = true;
            this.Insurance_Police.VisibleIndex = 2;
            this.Insurance_Police.Width = 110;
            // 
            // FInsuranceTransferList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1708, 712);
            this.Controls.Add(this.InsuranceGridControl);
            this.Controls.Add(this.StandaloneBarDockControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MinimizeBox = false;
            this.Name = "FInsuranceTransferList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sığorta köçürmələrinin siyahısı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FInsuranceTransferList_FormClosing);
            this.Load += new System.EventHandler(this.FInsuranceTransferList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosedCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsuranceGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.PopupMenu PopupMenu;
        private DevExpress.XtraBars.BarButtonItem RefreshBarButton;
        private DevExpress.XtraBars.BarButtonItem PrintBarButton;
        private DevExpress.XtraBars.BarButtonItem ExportBarButton;
        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.StandaloneBarDockControl StandaloneBarDockControl;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit ToDateValue;
        private DevExpress.XtraEditors.DateEdit FromDateValue;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraGrid.GridControl InsuranceGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView InsuranceGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_ContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_CompanyName;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_PayedAmount;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_PayDate;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_TaskCode;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Note;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Hostage;
        private DevExpress.XtraEditors.CheckEdit ClosedCheck;
        private DevExpress.XtraEditors.CheckEdit ActiveCheck;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_StatusID;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_ID;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem DeleteBarButton;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Cost;
        private DevExpress.XtraGrid.Columns.GridColumn Insurance_Police;
    }
}