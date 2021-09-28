namespace CRS.Forms.Contracts
{
    partial class FInsurancePaymentAddEdit
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
            ManiXButton.Office2010Blue office2010Blue1 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            this.NoteText = new DevExpress.XtraEditors.TextEdit();
            this.labelControl28 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl32 = new DevExpress.XtraEditors.LabelControl();
            this.AmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl31 = new DevExpress.XtraEditors.LabelControl();
            this.PayedDateValue = new DevExpress.XtraEditors.DateEdit();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl27 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.PayedAmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.LegalCheck = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedAmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LegalCheck.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // NoteText
            // 
            this.NoteText.Location = new System.Drawing.Point(147, 68);
            this.NoteText.Name = "NoteText";
            this.NoteText.Properties.NullValuePrompt = "Qeydi daxil edin";
            this.NoteText.Properties.NullValuePromptShowForEmptyValue = true;
            this.NoteText.Size = new System.Drawing.Size(425, 22);
            this.NoteText.TabIndex = 2;
            // 
            // labelControl28
            // 
            this.labelControl28.Location = new System.Drawing.Point(14, 71);
            this.labelControl28.Name = "labelControl28";
            this.labelControl28.Size = new System.Drawing.Size(29, 16);
            this.labelControl28.TabIndex = 730;
            this.labelControl28.Text = "Qeyd";
            // 
            // labelControl32
            // 
            this.labelControl32.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl32.Appearance.Options.UseForeColor = true;
            this.labelControl32.Location = new System.Drawing.Point(133, 46);
            this.labelControl32.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl32.Name = "labelControl32";
            this.labelControl32.Size = new System.Drawing.Size(8, 16);
            this.labelControl32.TabIndex = 728;
            this.labelControl32.Text = "*";
            // 
            // AmountLabel
            // 
            this.AmountLabel.Location = new System.Drawing.Point(14, 43);
            this.AmountLabel.Name = "AmountLabel";
            this.AmountLabel.Size = new System.Drawing.Size(50, 16);
            this.AmountLabel.TabIndex = 727;
            this.AmountLabel.Text = "Ödənilən";
            // 
            // labelControl31
            // 
            this.labelControl31.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Italic);
            this.labelControl31.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl31.Appearance.Options.UseFont = true;
            this.labelControl31.Appearance.Options.UseForeColor = true;
            this.labelControl31.Location = new System.Drawing.Point(253, 43);
            this.labelControl31.Name = "labelControl31";
            this.labelControl31.Size = new System.Drawing.Size(26, 16);
            this.labelControl31.TabIndex = 726;
            this.labelControl31.Text = "AZN";
            // 
            // PayedDateValue
            // 
            this.PayedDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PayedDateValue.EditValue = null;
            this.PayedDateValue.Location = new System.Drawing.Point(147, 12);
            this.PayedDateValue.Name = "PayedDateValue";
            this.PayedDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PayedDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PayedDateValue.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.PayedDateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.PayedDateValue.Size = new System.Drawing.Size(100, 22);
            this.PayedDateValue.TabIndex = 0;
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(14, 15);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(71, 16);
            this.DateLabel.TabIndex = 724;
            this.DateLabel.Text = "Ödəniş tarixi";
            // 
            // labelControl27
            // 
            this.labelControl27.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl27.Appearance.Options.UseForeColor = true;
            this.labelControl27.Location = new System.Drawing.Point(133, 15);
            this.labelControl27.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl27.Name = "labelControl27";
            this.labelControl27.Size = new System.Drawing.Size(8, 16);
            this.labelControl27.TabIndex = 725;
            this.labelControl27.Text = "*";
            // 
            // labelControl15
            // 
            this.labelControl15.AllowHtmlString = true;
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(14, 19);
            this.labelControl15.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(222, 17);
            this.labelControl15.TabIndex = 137;
            this.labelControl15.Text = "<color=104,0,0>*</color> - lu xanalar mütləq doldurulmalıdır";
            // 
            // PayedAmountValue
            // 
            this.PayedAmountValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PayedAmountValue.Location = new System.Drawing.Point(147, 40);
            this.PayedAmountValue.Name = "PayedAmountValue";
            this.PayedAmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PayedAmountValue.Properties.DisplayFormat.FormatString = "n2";
            this.PayedAmountValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.PayedAmountValue.Properties.EditFormat.FormatString = "n2";
            this.PayedAmountValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.PayedAmountValue.Properties.Mask.EditMask = "n2";
            this.PayedAmountValue.Properties.NullValuePrompt = "0.00";
            this.PayedAmountValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.PayedAmountValue.Properties.Precision = 3;
            this.PayedAmountValue.Size = new System.Drawing.Size(100, 22);
            this.PayedAmountValue.TabIndex = 1;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.labelControl15);
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 105);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(586, 62);
            this.PanelOption.TabIndex = 723;
            // 
            // BOK
            // 
            this.BOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Blue1.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue1.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue1.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue1.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Blue1.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Blue1.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue1.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue1.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010Blue1.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010Blue1.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue1.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue1.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Blue1.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Blue1.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue1.TextColor = System.Drawing.Color.White;
            this.BOK.ColorTable = office2010Blue1;
            this.BOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BOK.Location = new System.Drawing.Point(390, 16);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(87, 31);
            this.BOK.TabIndex = 3;
            this.BOK.Text = "Ödəniş et";
            this.BOK.Theme = ManiXButton.Theme.MSOffice2010_BLUE;
            this.BOK.UseVisualStyleBackColor = true;
            this.BOK.Click += new System.EventHandler(this.BOK_Click);
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
            this.BCancel.Location = new System.Drawing.Point(485, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 4;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // LegalCheck
            // 
            this.LegalCheck.EditValue = true;
            this.LegalCheck.Location = new System.Drawing.Point(470, 39);
            this.LegalCheck.Name = "LegalCheck";
            this.LegalCheck.Properties.Caption = "Hesabda";
            this.LegalCheck.Size = new System.Drawing.Size(102, 20);
            this.LegalCheck.TabIndex = 731;
            // 
            // FInsurancePaymentAddEdit
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(586, 167);
            this.Controls.Add(this.LegalCheck);
            this.Controls.Add(this.NoteText);
            this.Controls.Add(this.labelControl28);
            this.Controls.Add(this.labelControl32);
            this.Controls.Add(this.AmountLabel);
            this.Controls.Add(this.labelControl31);
            this.Controls.Add(this.PayedDateValue);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.labelControl27);
            this.Controls.Add(this.PayedAmountValue);
            this.Controls.Add(this.PanelOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FInsurancePaymentAddEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ödənişin əlavə/düzəliş edilməsi";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FInsurancePaymentAddEdit_FormClosed);
            this.Load += new System.EventHandler(this.FInsurancePaymentAddEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayedAmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            this.PanelOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LegalCheck.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit NoteText;
        private DevExpress.XtraEditors.LabelControl labelControl28;
        private DevExpress.XtraEditors.LabelControl labelControl32;
        private DevExpress.XtraEditors.LabelControl AmountLabel;
        private DevExpress.XtraEditors.LabelControl labelControl31;
        private DevExpress.XtraEditors.DateEdit PayedDateValue;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraEditors.LabelControl labelControl27;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.CalcEdit PayedAmountValue;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private DevExpress.XtraEditors.CheckEdit LegalCheck;
    }
}