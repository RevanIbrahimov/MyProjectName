namespace CRS.Forms.Contracts
{
    partial class FContractPowerOfAttorney
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
            ManiXButton.Office2010Red office2010Red2 = new ManiXButton.Office2010Red();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression2 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.PowerGridControl = new DevExpress.XtraGrid.GridControl();
            this.PowerGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Power_SS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepositoryItemPictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.Power_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Fullname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_InsertDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_InsertUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_Check = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Power_IsRevoke = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 545);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(1200, 62);
            this.PanelOption.TabIndex = 77;
            // 
            // BCancel
            // 
            this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Red2.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Red2.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Red2.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red2.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red2.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Red2.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Red2.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red2.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red2.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red2.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red2.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red2.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red2.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Red2.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Red2.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red2.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red2.TextColor = System.Drawing.Color.White;
            this.BCancel.ColorTable = office2010Red2;
            this.BCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(1099, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 4;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // PowerGridControl
            // 
            this.PowerGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PowerGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerGridControl.Location = new System.Drawing.Point(0, 0);
            this.PowerGridControl.MainView = this.PowerGridView;
            this.PowerGridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerGridControl.Name = "PowerGridControl";
            this.PowerGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemPictureEdit});
            this.PowerGridControl.Size = new System.Drawing.Size(1200, 545);
            this.PowerGridControl.TabIndex = 78;
            this.PowerGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PowerGridView});
            // 
            // PowerGridView
            // 
            this.PowerGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PowerGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PowerGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PowerGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PowerGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.ViewCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PowerGridView.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Indigo;
            this.PowerGridView.Appearance.ViewCaption.Options.UseFont = true;
            this.PowerGridView.Appearance.ViewCaption.Options.UseForeColor = true;
            this.PowerGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PowerGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PowerGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Power_SS,
            this.Power_ID,
            this.Power_Status,
            this.Power_Code,
            this.Power_Fullname,
            this.Power_InsertDate,
            this.Power_Date,
            this.Power_InsertUser,
            this.Power_Check,
            this.Power_IsRevoke});
            this.PowerGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            gridFormatRule2.ApplyToRow = true;
            gridFormatRule2.Name = "Format0";
            formatConditionRuleExpression2.Expression = "[USED_USER_ID] >= 0";
            formatConditionRuleExpression2.PredefinedName = "Yellow Fill, Yellow Text";
            gridFormatRule2.Rule = formatConditionRuleExpression2;
            this.PowerGridView.FormatRules.Add(gridFormatRule2);
            this.PowerGridView.GridControl = this.PowerGridControl;
            this.PowerGridView.Name = "PowerGridView";
            this.PowerGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PowerGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PowerGridView.OptionsBehavior.Editable = false;
            this.PowerGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PowerGridView.OptionsFind.FindDelay = 100;
            this.PowerGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PowerGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PowerGridView.OptionsView.ShowFooter = true;
            this.PowerGridView.OptionsView.ShowGroupPanel = false;
            this.PowerGridView.OptionsView.ShowIndicator = false;
            this.PowerGridView.PaintStyleName = "Skin";
            this.PowerGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PowerGridView.ViewCaption = "Müqaviləyə aid olan etibarnamələr";
            this.PowerGridView.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.PowerGridView_CustomUnboundColumnData);
            this.PowerGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.PowerGridView_CustomColumnDisplayText);
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
            this.Power_SS.Width = 40;
            // 
            // Power_ID
            // 
            this.Power_ID.Caption = "ID";
            this.Power_ID.FieldName = "ID";
            this.Power_ID.Name = "Power_ID";
            // 
            // Power_Status
            // 
            this.Power_Status.Caption = "Statusu";
            this.Power_Status.ColumnEdit = this.RepositoryItemPictureEdit;
            this.Power_Status.FieldName = "Power_Status";
            this.Power_Status.Name = "Power_Status";
            this.Power_Status.OptionsColumn.FixedWidth = true;
            this.Power_Status.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.Power_Status.Visible = true;
            this.Power_Status.VisibleIndex = 1;
            // 
            // RepositoryItemPictureEdit
            // 
            this.RepositoryItemPictureEdit.Name = "RepositoryItemPictureEdit";
            this.RepositoryItemPictureEdit.ZoomAccelerationFactor = 1D;
            // 
            // Power_Code
            // 
            this.Power_Code.AppearanceCell.Options.UseTextOptions = true;
            this.Power_Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Power_Code.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Power_Code.Caption = "Nömrə";
            this.Power_Code.FieldName = "POWER_CODE";
            this.Power_Code.Name = "Power_Code";
            this.Power_Code.OptionsColumn.FixedWidth = true;
            this.Power_Code.Visible = true;
            this.Power_Code.VisibleIndex = 2;
            this.Power_Code.Width = 60;
            // 
            // Power_Fullname
            // 
            this.Power_Fullname.Caption = "Sürücünün adı";
            this.Power_Fullname.FieldName = "FULLNAME";
            this.Power_Fullname.Name = "Power_Fullname";
            this.Power_Fullname.Visible = true;
            this.Power_Fullname.VisibleIndex = 3;
            this.Power_Fullname.Width = 374;
            // 
            // Power_InsertDate
            // 
            this.Power_InsertDate.AppearanceCell.Options.UseTextOptions = true;
            this.Power_InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Power_InsertDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Power_InsertDate.Caption = "Verilmə tarixi";
            this.Power_InsertDate.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.Power_InsertDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.Power_InsertDate.FieldName = "INSERT_DATE";
            this.Power_InsertDate.Name = "Power_InsertDate";
            this.Power_InsertDate.OptionsColumn.FixedWidth = true;
            this.Power_InsertDate.Visible = true;
            this.Power_InsertDate.VisibleIndex = 4;
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
            this.Power_Date.VisibleIndex = 5;
            // 
            // Power_InsertUser
            // 
            this.Power_InsertUser.Caption = "Daxil edən istifadəçi";
            this.Power_InsertUser.FieldName = "INSERT_USER";
            this.Power_InsertUser.Name = "Power_InsertUser";
            this.Power_InsertUser.Visible = true;
            this.Power_InsertUser.VisibleIndex = 6;
            this.Power_InsertUser.Width = 150;
            // 
            // Power_Check
            // 
            this.Power_Check.Caption = "gridColumn1";
            this.Power_Check.FieldName = "FULLNAME_CHECK";
            this.Power_Check.Name = "Power_Check";
            this.Power_Check.Width = 165;
            // 
            // Power_IsRevoke
            // 
            this.Power_IsRevoke.Caption = "IsRevoke";
            this.Power_IsRevoke.FieldName = "IS_REVOKE";
            this.Power_IsRevoke.Name = "Power_IsRevoke";
            // 
            // FContractPowerOfAttorney
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(1200, 607);
            this.Controls.Add(this.PowerGridControl);
            this.Controls.Add(this.PanelOption);
            this.MinimizeBox = false;
            this.Name = "FContractPowerOfAttorney";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "saylı lizinq müqaviləsinə aid olan etibarnamələr";
            this.Load += new System.EventHandler(this.FContractPowerOfAttorney_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemPictureEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraGrid.GridControl PowerGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PowerGridView;
        private DevExpress.XtraGrid.Columns.GridColumn Power_SS;
        private DevExpress.XtraGrid.Columns.GridColumn Power_ID;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Status;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit RepositoryItemPictureEdit;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Code;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Fullname;
        private DevExpress.XtraGrid.Columns.GridColumn Power_InsertDate;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Date;
        private DevExpress.XtraGrid.Columns.GridColumn Power_InsertUser;
        private DevExpress.XtraGrid.Columns.GridColumn Power_Check;
        private DevExpress.XtraGrid.Columns.GridColumn Power_IsRevoke;
    }
}