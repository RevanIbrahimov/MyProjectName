namespace CRS.Forms
{
    partial class FDateCalculator
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDateCalculator));
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.TypeRadioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.EndDate = new DevExpress.XtraEditors.DateEdit();
            this.StartDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DayResLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.MonthResLabel = new DevExpress.XtraEditors.LabelControl();
            this.YearResLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TypeRadioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 222);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(395, 62);
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
            this.BCancel.Location = new System.Drawing.Point(292, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // TypeRadioGroup
            // 
            this.TypeRadioGroup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TypeRadioGroup.Location = new System.Drawing.Point(14, 15);
            this.TypeRadioGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TypeRadioGroup.Name = "TypeRadioGroup";
            this.TypeRadioGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.TypeRadioGroup.Properties.Appearance.Options.UseBackColor = true;
            this.TypeRadioGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.TypeRadioGroup.Properties.Columns = 2;
            this.TypeRadioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "360 gün"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Normal")});
            this.TypeRadioGroup.Size = new System.Drawing.Size(365, 38);
            this.TypeRadioGroup.TabIndex = 52;
            this.TypeRadioGroup.TabStop = false;
            this.TypeRadioGroup.SelectedIndexChanged += new System.EventHandler(this.TypeRadioGroup_SelectedIndexChanged);
            this.TypeRadioGroup.EditValueChanged += new System.EventHandler(this.EndDate_EditValueChanged);
            // 
            // EndDate
            // 
            this.EndDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EndDate.EditValue = null;
            this.EndDate.Location = new System.Drawing.Point(280, 80);
            this.EndDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EndDate.Name = "EndDate";
            this.EndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.EndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.EndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EndDate.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.EndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.EndDate.Size = new System.Drawing.Size(99, 22);
            this.EndDate.TabIndex = 1;
            this.EndDate.EditValueChanged += new System.EventHandler(this.EndDate_EditValueChanged);
            // 
            // StartDate
            // 
            this.StartDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartDate.EditValue = null;
            this.StartDate.Location = new System.Drawing.Point(101, 80);
            this.StartDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StartDate.Name = "StartDate";
            this.StartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.StartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.StartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.StartDate.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.StartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.StartDate.Size = new System.Drawing.Size(99, 22);
            this.StartDate.TabIndex = 0;
            this.StartDate.EditValueChanged += new System.EventHandler(this.EndDate_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(220, 84);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 16);
            this.labelControl1.TabIndex = 55;
            this.labelControl1.Text = "Son tarix";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 84);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(83, 16);
            this.labelControl2.TabIndex = 56;
            this.labelControl2.Text = "Başlanğıc tarix";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(208, 84);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(6, 17);
            this.labelControl3.TabIndex = 57;
            this.labelControl3.Text = "-";
            // 
            // DayResLabel
            // 
            this.DayResLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.DayResLabel.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.DayResLabel.Appearance.Options.UseFont = true;
            this.DayResLabel.Appearance.Options.UseForeColor = true;
            this.DayResLabel.Location = new System.Drawing.Point(133, 146);
            this.DayResLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DayResLabel.Name = "DayResLabel";
            this.DayResLabel.Size = new System.Drawing.Size(62, 17);
            this.DayResLabel.TabIndex = 71;
            this.DayResLabel.Text = "Son tarix";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(14, 146);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 17);
            this.labelControl4.TabIndex = 72;
            this.labelControl4.Text = "Günlərin sayı :";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(14, 170);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(86, 17);
            this.labelControl5.TabIndex = 73;
            this.labelControl5.Text = "Ayların sayı :";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(14, 193);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(78, 17);
            this.labelControl6.TabIndex = 74;
            this.labelControl6.Text = "İllərin sayı :";
            // 
            // MonthResLabel
            // 
            this.MonthResLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.MonthResLabel.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.MonthResLabel.Appearance.Options.UseFont = true;
            this.MonthResLabel.Appearance.Options.UseForeColor = true;
            this.MonthResLabel.Location = new System.Drawing.Point(133, 170);
            this.MonthResLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MonthResLabel.Name = "MonthResLabel";
            this.MonthResLabel.Size = new System.Drawing.Size(62, 17);
            this.MonthResLabel.TabIndex = 75;
            this.MonthResLabel.Text = "Son tarix";
            // 
            // YearResLabel
            // 
            this.YearResLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.YearResLabel.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.YearResLabel.Appearance.Options.UseFont = true;
            this.YearResLabel.Appearance.Options.UseForeColor = true;
            this.YearResLabel.Location = new System.Drawing.Point(133, 193);
            this.YearResLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.YearResLabel.Name = "YearResLabel";
            this.YearResLabel.Size = new System.Drawing.Size(62, 17);
            this.YearResLabel.TabIndex = 76;
            this.YearResLabel.Text = "Son tarix";
            // 
            // FDateCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(395, 284);
            this.Controls.Add(this.YearResLabel);
            this.Controls.Add(this.MonthResLabel);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.DayResLabel);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.StartDate);
            this.Controls.Add(this.EndDate);
            this.Controls.Add(this.TypeRadioGroup);
            this.Controls.Add(this.PanelOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 278);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(357, 278);
            this.Name = "FDateCalculator";
            this.Text = "Tarix kalkulyatoru";
            this.Load += new System.EventHandler(this.FDateCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TypeRadioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.RadioGroup TypeRadioGroup;
        private DevExpress.XtraEditors.DateEdit EndDate;
        private DevExpress.XtraEditors.DateEdit StartDate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl DayResLabel;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl MonthResLabel;
        private DevExpress.XtraEditors.LabelControl YearResLabel;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
    }
}