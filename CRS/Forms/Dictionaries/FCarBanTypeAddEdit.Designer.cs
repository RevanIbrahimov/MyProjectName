namespace CRS.Forms.Dictionaries
{
    partial class FCarBanTypeAddEdit
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
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCarBanTypeAddEdit));
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            ManiXButton.Office2010Blue office2010Blue3 = new ManiXButton.Office2010Blue();
            ManiXButton.Office2010Red office2010Red3 = new ManiXButton.Office2010Red();
            this.NoteText = new DevExpress.XtraEditors.TextEdit();
            this.NoteLabel = new DevExpress.XtraEditors.LabelControl();
            this.NameText = new DevExpress.XtraEditors.TextEdit();
            this.NameLabel = new DevExpress.XtraEditors.LabelControl();
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BOK = new ManiXButton.XButton();
            this.BCancel = new ManiXButton.XButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // NoteText
            // 
            this.NoteText.Location = new System.Drawing.Point(66, 38);
            this.NoteText.Name = "NoteText";
            this.NoteText.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoteText.Properties.Appearance.Options.UseFont = true;
            this.NoteText.Size = new System.Drawing.Size(323, 20);
            superToolTip2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem2.Text = "<color=255,0,0>Qeyd</color>";
            toolTipItem2.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem2.Appearance.Options.UseImage = true;
            toolTipItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem2.Image")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Bu xanaya dırnaq işarəsi daxil etmək olmaz. Əgər daxil etsəz, ban tipinin məlumat" +
    "ları <b><color=104,6,6>bazada saxlanılmayacaq</b></color>.";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.NoteText.SuperTip = superToolTip2;
            this.NoteText.TabIndex = 1;
            // 
            // NoteLabel
            // 
            this.NoteLabel.Location = new System.Drawing.Point(12, 41);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(26, 13);
            this.NoteLabel.TabIndex = 89;
            this.NoteLabel.Text = "Qeyd";
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(66, 12);
            this.NameText.Name = "NameText";
            this.NameText.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameText.Properties.Appearance.Options.UseFont = true;
            this.NameText.Size = new System.Drawing.Size(323, 20);
            superToolTip3.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem3.Text = "<color=255,0,0>Ban tipinin adı</color>";
            toolTipItem3.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            toolTipItem3.Appearance.Options.UseImage = true;
            toolTipItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem3.Image")));
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Bu xanaya dırnaq işarəsi daxil etmək olmaz. Əgər daxil etsəz, ban tipinin məlumat" +
    "ları <b><color=104,6,6>bazada saxlanılmayacaq</b></color>.";
            superToolTip3.Items.Add(toolTipTitleItem3);
            superToolTip3.Items.Add(toolTipItem3);
            this.NameText.SuperTip = superToolTip3;
            this.NameText.TabIndex = 0;
            // 
            // NameLabel
            // 
            this.NameLabel.Location = new System.Drawing.Point(12, 15);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(15, 13);
            this.NameLabel.TabIndex = 88;
            this.NameLabel.Text = "Adı";
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.labelControl15);
            this.PanelOption.Controls.Add(this.BOK);
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 69);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(401, 50);
            this.PanelOption.TabIndex = 90;
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
            this.BOK.Location = new System.Drawing.Point(233, 13);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(75, 25);
            this.BOK.TabIndex = 3;
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
            this.BCancel.Location = new System.Drawing.Point(314, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 4;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(54, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(6, 13);
            this.labelControl2.TabIndex = 186;
            this.labelControl2.Text = "*";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(12, 19);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(170, 13);
            this.labelControl15.TabIndex = 139;
            this.labelControl15.Text = "* - lu xanalar mütləq doldurulmalıdır";
            // 
            // FCarBanTypeAddEdit
            // 
            this.AcceptButton = this.BOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(401, 119);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.PanelOption);
            this.Controls.Add(this.NoteText);
            this.Controls.Add(this.NoteLabel);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCarBanTypeAddEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Ban tipinin əlavə/düzəliş edilməsi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FCarBanTypeAddEdit_FormClosing);
            this.Load += new System.EventHandler(this.FCarBanTypeAddEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NoteText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NameText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            this.PanelOption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit NoteText;
        private DevExpress.XtraEditors.LabelControl NoteLabel;
        private DevExpress.XtraEditors.TextEdit NameText;
        private DevExpress.XtraEditors.LabelControl NameLabel;
        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BOK;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl15;
    }
}