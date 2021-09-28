namespace CRS.Forms.Total
{
    partial class FBalancePenaltyDetail
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
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.PenaltyGridControl = new DevExpress.XtraGrid.GridControl();
            this.PenaltyGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PenaltyValue = new DevExpress.XtraEditors.CalcEdit();
            this.CurrencyLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 427);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(853, 50);
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
            this.BCancel.Location = new System.Drawing.Point(764, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // PenaltyGridControl
            // 
            this.PenaltyGridControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.PenaltyGridControl.Location = new System.Drawing.Point(0, 0);
            this.PenaltyGridControl.MainView = this.PenaltyGridView;
            this.PenaltyGridControl.Name = "PenaltyGridControl";
            this.PenaltyGridControl.Size = new System.Drawing.Size(853, 395);
            this.PenaltyGridControl.TabIndex = 61;
            this.PenaltyGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PenaltyGridView});
            // 
            // PenaltyGridView
            // 
            this.PenaltyGridView.Appearance.FooterPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PenaltyGridView.Appearance.FooterPanel.Options.UseFont = true;
            this.PenaltyGridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.PenaltyGridView.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PenaltyGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PenaltyGridView.Appearance.ViewCaption.Options.UseBackColor = true;
            this.PenaltyGridView.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.PenaltyGridView.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.PenaltyGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.PenaltyGridView.GridControl = this.PenaltyGridControl;
            this.PenaltyGridView.Name = "PenaltyGridView";
            this.PenaltyGridView.OptionsBehavior.AutoExpandAllGroups = true;
            this.PenaltyGridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.PenaltyGridView.OptionsBehavior.Editable = false;
            this.PenaltyGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.PenaltyGridView.OptionsFind.FindDelay = 100;
            this.PenaltyGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.PenaltyGridView.OptionsSelection.MultiSelect = true;
            this.PenaltyGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.PenaltyGridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.PenaltyGridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.PenaltyGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.PenaltyGridView.OptionsView.ShowFooter = true;
            this.PenaltyGridView.OptionsView.ShowGroupPanel = false;
            this.PenaltyGridView.OptionsView.ShowIndicator = false;
            this.PenaltyGridView.PaintStyleName = "Skin";
            this.PenaltyGridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.PenaltyGridView.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.PenaltyGridView_CustomDrawFooterCell);
            this.PenaltyGridView.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.PenaltyGridView_SelectionChanged);
            // 
            // PenaltyValue
            // 
            this.PenaltyValue.Location = new System.Drawing.Point(621, 401);
            this.PenaltyValue.Name = "PenaltyValue";
            this.PenaltyValue.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.PenaltyValue.Properties.Appearance.Options.UseBackColor = true;
            this.PenaltyValue.Properties.ReadOnly = true;
            this.PenaltyValue.Size = new System.Drawing.Size(192, 20);
            this.PenaltyValue.TabIndex = 97;
            // 
            // CurrencyLabel
            // 
            this.CurrencyLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CurrencyLabel.Appearance.Options.UseForeColor = true;
            this.CurrencyLabel.Location = new System.Drawing.Point(819, 404);
            this.CurrencyLabel.Name = "CurrencyLabel";
            this.CurrencyLabel.Size = new System.Drawing.Size(20, 13);
            this.CurrencyLabel.TabIndex = 96;
            this.CurrencyLabel.Text = "AZN";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Purple;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(377, 404);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(238, 13);
            this.labelControl1.TabIndex = 95;
            this.labelControl1.Text = "Müştəridən tutulan cərimə faizlərinin cəmi";
            // 
            // FBalancePenaltyDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(853, 477);
            this.Controls.Add(this.PenaltyValue);
            this.Controls.Add(this.CurrencyLabel);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.PenaltyGridControl);
            this.Controls.Add(this.PanelOption);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(869, 516);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(869, 516);
            this.Name = "FBalancePenaltyDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balansa daxil edilmiş cərimə faizlərinin statistikası";
            this.Load += new System.EventHandler(this.FBalancePenaltyDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenaltyValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraGrid.GridControl PenaltyGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView PenaltyGridView;
        private DevExpress.XtraEditors.CalcEdit PenaltyValue;
        private DevExpress.XtraEditors.LabelControl CurrencyLabel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}