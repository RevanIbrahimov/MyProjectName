namespace CRS.Forms
{
    partial class FPhoneAddEdit
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            ManiXButton.Office2010Blue office2010Blue1 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions5 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions6 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions7 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPhoneAddEdit));
            this.CountryComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DescriptionComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.PhoneText = new DevExpress.XtraEditors.TextEdit();
            this.PhoneLabel = new DevExpress.XtraEditors.LabelControl();
            this.CountryLabel = new DevExpress.XtraEditors.LabelControl();
            this.BOK = new ManiXButton.XButton();
            this.DescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.BCancel = new ManiXButton.XButton();
            this.CodeText = new DevExpress.XtraEditors.TextEdit();
            this.KindShipRateLabel = new DevExpress.XtraEditors.LabelControl();
            this.KindShipNameLabel = new DevExpress.XtraEditors.LabelControl();
            this.KindShipNameText = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.KindShipRateLookUp = new DevExpress.XtraEditors.LookUpEdit();
            this.KindShipNameStartLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.CountryComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodeText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KindShipNameText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KindShipRateLookUp.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CountryComboBox
            // 
            this.CountryComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CountryComboBox.Location = new System.Drawing.Point(177, 15);
            this.CountryComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CountryComboBox.Name = "CountryComboBox";
            this.CountryComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Düzəliş etmək üçün ölkələrin siyahısını aç")});
            this.CountryComboBox.Properties.NullValuePrompt = "Ölkəni seçin";
            this.CountryComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.CountryComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CountryComboBox.Size = new System.Drawing.Size(360, 22);
            this.CountryComboBox.TabIndex = 0;
            this.CountryComboBox.SelectedIndexChanged += new System.EventHandler(this.PrefixComboBox_SelectedIndexChanged);
            this.CountryComboBox.CloseUp += new DevExpress.XtraEditors.Controls.CloseUpEventHandler(this.PrefixComboBox_CloseUp);
            this.CountryComboBox.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CountryComboBox_ButtonClick);
            // 
            // DescriptionComboBox
            // 
            this.DescriptionComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DescriptionComboBox.Location = new System.Drawing.Point(177, 47);
            this.DescriptionComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DescriptionComboBox.Name = "DescriptionComboBox";
            this.DescriptionComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Düzəliş etmək üçün telefon təsvirlərinin siyanını aç")});
            this.DescriptionComboBox.Properties.NullValuePrompt = "Təsviri seçin";
            this.DescriptionComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.DescriptionComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.DescriptionComboBox.Size = new System.Drawing.Size(360, 22);
            this.DescriptionComboBox.TabIndex = 1;
            this.DescriptionComboBox.SelectedIndexChanged += new System.EventHandler(this.DescriptionComboBox_SelectedIndexChanged);
            this.DescriptionComboBox.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.DescriptionComboBox_ButtonClick);
            // 
            // PhoneText
            // 
            this.PhoneText.Location = new System.Drawing.Point(257, 79);
            this.PhoneText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PhoneText.Name = "PhoneText";
            this.PhoneText.Size = new System.Drawing.Size(159, 22);
            this.PhoneText.TabIndex = 2;
            // 
            // PhoneLabel
            // 
            this.PhoneLabel.Location = new System.Drawing.Point(14, 82);
            this.PhoneLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PhoneLabel.Name = "PhoneLabel";
            this.PhoneLabel.Size = new System.Drawing.Size(38, 16);
            this.PhoneLabel.TabIndex = 14;
            this.PhoneLabel.Text = "Nömrə";
            // 
            // CountryLabel
            // 
            this.CountryLabel.Location = new System.Drawing.Point(14, 18);
            this.CountryLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CountryLabel.Name = "CountryLabel";
            this.CountryLabel.Size = new System.Drawing.Size(25, 16);
            this.CountryLabel.TabIndex = 13;
            this.CountryLabel.Text = "Ölkə";
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
            this.BOK.Location = new System.Drawing.Point(355, 16);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(87, 31);
            this.BOK.TabIndex = 4;
            this.BOK.Text = "Yadda saxla";
            this.BOK.Theme = ManiXButton.Theme.MSOffice2010_BLUE;
            this.BOK.UseVisualStyleBackColor = true;
            this.BOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Location = new System.Drawing.Point(14, 50);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(38, 16);
            this.DescriptionLabel.TabIndex = 12;
            this.DescriptionLabel.Text = "Təsviri";
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.labelControl15);
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 173);
            this.PanelOption.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(551, 62);
            this.PanelOption.TabIndex = 11;
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
            this.labelControl15.TabIndex = 140;
            this.labelControl15.Text = "<color=104,0,0>*</color> - lu xanalar mütləq doldurulmalıdır";
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
            this.BCancel.Location = new System.Drawing.Point(450, 16);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 31);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // CodeText
            // 
            this.CodeText.Location = new System.Drawing.Point(177, 78);
            this.CodeText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CodeText.Name = "CodeText";
            this.CodeText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.CodeText.Properties.Appearance.Options.UseFont = true;
            this.CodeText.Properties.Appearance.Options.UseTextOptions = true;
            this.CodeText.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CodeText.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CodeText.Properties.ReadOnly = true;
            this.CodeText.Size = new System.Drawing.Size(72, 24);
            this.CodeText.TabIndex = 17;
            this.CodeText.TabStop = false;
            // 
            // KindShipRateLabel
            // 
            this.KindShipRateLabel.Location = new System.Drawing.Point(14, 113);
            this.KindShipRateLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.KindShipRateLabel.Name = "KindShipRateLabel";
            this.KindShipRateLabel.Size = new System.Drawing.Size(110, 16);
            this.KindShipRateLabel.TabIndex = 19;
            this.KindShipRateLabel.Text = "Qohumluq dərəcəsi";
            // 
            // KindShipNameLabel
            // 
            this.KindShipNameLabel.Location = new System.Drawing.Point(14, 142);
            this.KindShipNameLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.KindShipNameLabel.Name = "KindShipNameLabel";
            this.KindShipNameLabel.Size = new System.Drawing.Size(130, 16);
            this.KindShipNameLabel.TabIndex = 20;
            this.KindShipNameLabel.Text = "Nömrənin sahibinin adı";
            // 
            // KindShipNameText
            // 
            this.KindShipNameText.Enabled = false;
            this.KindShipNameText.Location = new System.Drawing.Point(177, 139);
            this.KindShipNameText.Name = "KindShipNameText";
            this.KindShipNameText.Properties.NullValuePrompt = "Nömrənin sahibinin adını daxil edin";
            this.KindShipNameText.Properties.NullValuePromptShowForEmptyValue = true;
            this.KindShipNameText.Size = new System.Drawing.Size(360, 22);
            this.KindShipNameText.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(163, 18);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 16);
            this.labelControl2.TabIndex = 193;
            this.labelControl2.Text = "*";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(163, 50);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(8, 16);
            this.labelControl1.TabIndex = 194;
            this.labelControl1.Text = "*";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(163, 82);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(8, 16);
            this.labelControl3.TabIndex = 195;
            this.labelControl3.Text = "*";
            // 
            // KindShipRateLookUp
            // 
            this.KindShipRateLookUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KindShipRateLookUp.Location = new System.Drawing.Point(177, 110);
            this.KindShipRateLookUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.KindShipRateLookUp.Name = "KindShipRateLookUp";
            this.KindShipRateLookUp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, editorButtonImageOptions5, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Siyahını aç"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions6, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Düzəliş etmək üçün qohumluq dərəcələrinin siyahısını aç"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, true, false, editorButtonImageOptions7, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), "Seçilmişi sil")});
            this.KindShipRateLookUp.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", 10, "Name")});
            this.KindShipRateLookUp.Properties.DisplayMember = "NAME";
            this.KindShipRateLookUp.Properties.DropDownRows = 10;
            this.KindShipRateLookUp.Properties.NullText = "Qohumluq dərəcəsini seçin";
            this.KindShipRateLookUp.Properties.NullValuePrompt = "Qohumluq dərəcəsini seçin";
            this.KindShipRateLookUp.Properties.NullValuePromptShowForEmptyValue = true;
            this.KindShipRateLookUp.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.KindShipRateLookUp.Properties.ShowHeader = false;
            this.KindShipRateLookUp.Properties.ThrowExceptionOnInvalidLookUpEditValueType = true;
            this.KindShipRateLookUp.Properties.ValidateOnEnterKey = true;
            this.KindShipRateLookUp.Properties.ValueMember = "ID";
            this.KindShipRateLookUp.Size = new System.Drawing.Size(360, 22);
            this.KindShipRateLookUp.TabIndex = 3;
            this.KindShipRateLookUp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.KindShipRateLookUp_ButtonClick);
            this.KindShipRateLookUp.EditValueChanged += new System.EventHandler(this.KindShipRateLookUp_EditValueChanged);
            // 
            // KindShipNameStartLabel
            // 
            this.KindShipNameStartLabel.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.KindShipNameStartLabel.Appearance.Options.UseForeColor = true;
            this.KindShipNameStartLabel.Location = new System.Drawing.Point(163, 142);
            this.KindShipNameStartLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.KindShipNameStartLabel.Name = "KindShipNameStartLabel";
            this.KindShipNameStartLabel.Size = new System.Drawing.Size(8, 16);
            this.KindShipNameStartLabel.TabIndex = 196;
            this.KindShipNameStartLabel.Text = "*";
            this.KindShipNameStartLabel.Visible = false;
            // 
            // FPhoneAddEdit
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(551, 235);
            this.Controls.Add(this.KindShipNameStartLabel);
            this.Controls.Add(this.KindShipRateLookUp);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.KindShipNameText);
            this.Controls.Add(this.KindShipNameLabel);
            this.Controls.Add(this.KindShipRateLabel);
            this.Controls.Add(this.CodeText);
            this.Controls.Add(this.CountryComboBox);
            this.Controls.Add(this.DescriptionComboBox);
            this.Controls.Add(this.PhoneText);
            this.Controls.Add(this.PhoneLabel);
            this.Controls.Add(this.CountryLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.PanelOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPhoneAddEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Telefon nömrəsinin əlavə/düzəliş edilməsi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FPhoneAddEdit_FormClosing);
            this.Load += new System.EventHandler(this.FPhoneAddEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CountryComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhoneText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            this.PanelOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodeText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KindShipNameText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KindShipRateLookUp.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit CountryComboBox;
        private DevExpress.XtraEditors.ComboBoxEdit DescriptionComboBox;
        private DevExpress.XtraEditors.TextEdit PhoneText;
        private DevExpress.XtraEditors.LabelControl PhoneLabel;
        private DevExpress.XtraEditors.LabelControl CountryLabel;
        private ManiXButton.XButton BOK;
        private DevExpress.XtraEditors.LabelControl DescriptionLabel;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.TextEdit CodeText;
        private DevExpress.XtraEditors.LabelControl KindShipRateLabel;
        private DevExpress.XtraEditors.LabelControl KindShipNameLabel;
        private DevExpress.XtraEditors.TextEdit KindShipNameText;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LookUpEdit KindShipRateLookUp;
        private DevExpress.XtraEditors.LabelControl KindShipNameStartLabel;
    }
}