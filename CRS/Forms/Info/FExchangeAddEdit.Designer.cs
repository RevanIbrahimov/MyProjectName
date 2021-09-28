namespace CRS.Forms.Info
{
    partial class FExchangeAddEdit
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FExchangeAddEdit));
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions5 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            ManiXButton.Office2010Blue office2010Blue3 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red3 = new ManiXButton.Office2010Red();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            this.NoteLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.NoteText = new DevExpress.XtraEditors.TextEdit();
            this.AmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.CurrencyComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.RateDate = new DevExpress.XtraEditors.DateEdit();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.RateLabel = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrencyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RateDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RateDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // NoteLabelControl
            // 
            this.NoteLabelControl.Location = new System.Drawing.Point(15, 80);
            this.NoteLabelControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NoteLabelControl.Name = "NoteLabelControl";
            this.NoteLabelControl.Size = new System.Drawing.Size(29, 16);
            this.NoteLabelControl.TabIndex = 31;
            this.NoteLabelControl.Text = "Qeyd";
            // 
            // NoteText
            // 
            this.NoteText.Location = new System.Drawing.Point(99, 77);
            this.NoteText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NoteText.Name = "NoteText";
            this.NoteText.Properties.NullValuePrompt = "Qeydi daxil edin";
            this.NoteText.Properties.NullValuePromptShowForEmptyValue = true;
            this.NoteText.Size = new System.Drawing.Size(338, 22);
            this.NoteText.TabIndex = 3;
            // 
            // AmountValue
            // 
            this.AmountValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AmountValue.Location = new System.Drawing.Point(311, 47);
            this.AmountValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AmountValue.Name = "AmountValue";
            this.AmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalkulyatoru aç")});
            this.AmountValue.Properties.NullValuePrompt = "0.0000";
            this.AmountValue.Properties.NullValuePromptShowForEmptyValue = true;
            this.AmountValue.Properties.Precision = 4;
            this.AmountValue.Size = new System.Drawing.Size(126, 22);
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem1.Text = "<color=255,0,0>Valyutanın AZN qarşılığı</color>";
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Məzənnə sıfırdan böyük olmalı və vergüldən sonra <b><color=104,6,6>4 rəqəm</color" +
    "></b> daxil edilməsi tövsiyyə olunur.";
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = "Qeyd: <i>Əgər vergüldən sonra 4-dən çox rəqəm yazılsa, məzənnə bazaya daxil edilə" +
    "rkən bu ədəd 4 rəqəmə qədər yuvarlaqlaşdırılacaq.</i>";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipSeparatorItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.AmountValue.SuperTip = superToolTip1;
            this.AmountValue.TabIndex = 2;
            // 
            // CurrencyComboBox
            // 
            this.CurrencyComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CurrencyComboBox.Location = new System.Drawing.Point(99, 47);
            this.CurrencyComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CurrencyComboBox.Name = "CurrencyComboBox";
            this.CurrencyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions5, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Düzəliş etmək üçün valyutaların siyahısını aç")});
            this.CurrencyComboBox.Properties.NullValuePrompt = "Valyutanı seçin";
            this.CurrencyComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.CurrencyComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CurrencyComboBox.Size = new System.Drawing.Size(180, 22);
            this.CurrencyComboBox.TabIndex = 1;
            this.CurrencyComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrencyComboBox_SelectedIndexChanged);
            this.CurrencyComboBox.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CurrencyComboBox_ButtonClick);
            // 
            // BOK
            // 
            this.BOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Blue3.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue3.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue3.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue3.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue3.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Blue3.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Blue3.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue3.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue3.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010Blue3.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010Blue3.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue3.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue3.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Blue3.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Blue3.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue3.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue3.TextColor = System.Drawing.Color.White;
            this.BOK.ColorTable = office2010Blue3;
            this.BOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BOK.Location = new System.Drawing.Point(253, 16);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(87, 31);
            this.BOK.TabIndex = 4;
            this.BOK.Text = "Yadda saxla";
            this.BOK.Theme = ManiXButton.Theme.MSOffice2010_BLUE;
            this.BOK.UseVisualStyleBackColor = true;
            this.BOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // BCancel
            // 
            this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            office2010Red3.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Red3.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Red3.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red3.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red3.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Red3.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Red3.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red3.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red3.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(77)))), ((int)(((byte)(45)))));
            office2010Red3.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(148)))), ((int)(((byte)(64)))));
            office2010Red3.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Red3.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Red3.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Red3.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Red3.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red3.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Red3.TextColor = System.Drawing.Color.White;
            this.BCancel.ColorTable = office2010Red3;
            this.BCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(350, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(15, 18);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(30, 16);
            this.DateLabel.TabIndex = 28;
            this.DateLabel.Text = "Tarix";
            // 
            // RateDate
            // 
            this.RateDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateDate.EditValue = null;
            this.RateDate.Location = new System.Drawing.Point(99, 15);
            this.RateDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RateDate.Name = "RateDate";
            this.RateDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Kalendarı aç")});
            this.RateDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RateDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            this.RateDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.RateDate.Properties.EditFormat.FormatString = "dd.MM.yyyy";
            this.RateDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.RateDate.Properties.NullValuePrompt = "dd.mm.yyyy";
            this.RateDate.Properties.NullValuePromptShowForEmptyValue = true;
            this.RateDate.Size = new System.Drawing.Size(117, 22);
            this.RateDate.TabIndex = 0;
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 118);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(454, 62);
            this.PanelOption.TabIndex = 27;
            // 
            // RateLabel
            // 
            this.RateLabel.Location = new System.Drawing.Point(15, 50);
            this.RateLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RateLabel.Name = "RateLabel";
            this.RateLabel.Size = new System.Drawing.Size(51, 16);
            this.RateLabel.TabIndex = 32;
            this.RateLabel.Text = "Məzənnə";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CRS.Properties.Resources.equal_16;
            this.pictureBox1.Location = new System.Drawing.Point(286, 49);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(85, 18);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(8, 16);
            this.labelControl1.TabIndex = 33;
            this.labelControl1.Text = "*";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(85, 50);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 16);
            this.labelControl2.TabIndex = 34;
            this.labelControl2.Text = "*";
            // 
            // FExchangeAddEdit
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(454, 180);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.RateLabel);
            this.Controls.Add(this.NoteLabelControl);
            this.Controls.Add(this.NoteText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.AmountValue);
            this.Controls.Add(this.CurrencyComboBox);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.RateDate);
            this.Controls.Add(this.PanelOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(407, 193);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(407, 193);
            this.Name = "FExchangeAddEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Məzənnənin əlavə/düzəliş edilməsi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FExchangeAddEdit_FormClosing);
            this.Load += new System.EventHandler(this.FExchangeAddEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrencyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RateDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RateDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl NoteLabelControl;
        private DevExpress.XtraEditors.TextEdit NoteText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.CalcEdit AmountValue;
        private DevExpress.XtraEditors.ComboBoxEdit CurrencyComboBox;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraEditors.DateEdit RateDate;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private DevExpress.XtraEditors.LabelControl RateLabel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}