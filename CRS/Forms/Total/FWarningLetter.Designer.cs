namespace CRS.Forms.Total
{
    partial class FWarningLetter
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWarningLetter));
            ManiXButton.Office2010Blue office2010Blue1 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            this.LetterCodeLabel = new DevExpress.XtraEditors.LabelControl();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.DateValue = new DevExpress.XtraEditors.DateEdit();
            this.LetterCodeText = new DevExpress.XtraEditors.TextEdit();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LetterCodeText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // LetterCodeLabel
            // 
            this.LetterCodeLabel.Location = new System.Drawing.Point(14, 19);
            this.LetterCodeLabel.Name = "LetterCodeLabel";
            this.LetterCodeLabel.Size = new System.Drawing.Size(105, 16);
            this.LetterCodeLabel.TabIndex = 53;
            this.LetterCodeLabel.Text = "Məktubun nömrəsi";
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(14, 49);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(30, 16);
            this.DateLabel.TabIndex = 54;
            this.DateLabel.Text = "Tarix";
            // 
            // DateValue
            // 
            this.DateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DateValue.EditValue = null;
            this.DateValue.Location = new System.Drawing.Point(171, 46);
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
            this.DateValue.Properties.NullValuePrompt = "dd.MM.yyyy";
            this.DateValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.DateValue.Size = new System.Drawing.Size(117, 22);
            this.DateValue.TabIndex = 1;
            // 
            // LetterCodeText
            // 
            this.LetterCodeText.Location = new System.Drawing.Point(171, 16);
            this.LetterCodeText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LetterCodeText.Name = "LetterCodeText";
            this.LetterCodeText.Properties.NullValuePrompt = "Məktubun nömrəsini daxil edin";
            this.LetterCodeText.Properties.NullValuePromptShowForEmptyValue = true;
            this.LetterCodeText.Size = new System.Drawing.Size(290, 22);
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            superToolTip1.Items.Add(toolTipItem1);
            this.LetterCodeText.SuperTip = superToolTip1;
            this.LetterCodeText.TabIndex = 0;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.labelControl15);
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 83);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(477, 62);
            this.PanelOption.TabIndex = 57;
            // 
            // labelControl15
            // 
            this.labelControl15.AllowHtmlString = true;
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(14, 23);
            this.labelControl15.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(222, 17);
            this.labelControl15.TabIndex = 135;
            this.labelControl15.Text = "<color=104,0,0>*</color> - lu xanalar mütləq doldurulmalıdır";
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
            this.BOK.Location = new System.Drawing.Point(279, 16);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(87, 31);
            this.BOK.TabIndex = 4;
            this.BOK.TabStop = false;
            this.BOK.Text = "Yadda saxla";
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
            this.BCancel.Location = new System.Drawing.Point(374, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.TabStop = false;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl6.Appearance.Options.UseForeColor = true;
            this.labelControl6.Location = new System.Drawing.Point(157, 19);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(8, 16);
            this.labelControl6.TabIndex = 178;
            this.labelControl6.Text = "*";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(157, 49);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(8, 16);
            this.labelControl1.TabIndex = 179;
            this.labelControl1.Text = "*";
            // 
            // FWarningLetter
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(477, 145);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.LetterCodeText);
            this.Controls.Add(this.DateValue);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.LetterCodeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FWarningLetter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xəbərdarlıq məktubu";
            this.Load += new System.EventHandler(this.FWarningLetter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LetterCodeText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            this.PanelOption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl LetterCodeLabel;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraEditors.DateEdit DateValue;
        private DevExpress.XtraEditors.TextEdit LetterCodeText;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}