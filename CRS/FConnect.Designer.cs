namespace CRS
{
    partial class FConnect
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
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConnect));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            ManiXButton.Office2010Red office2010Red2 = new ManiXButton.Office2010Red();
            ManiXButton.Office2010Blue office2010Blue2 = new ManiXButton.Office2010Blue();
            this.AzCheck = new DevExpress.XtraEditors.CheckEdit();
            this.EnCheck = new DevExpress.XtraEditors.CheckEdit();
            this.RuCheck = new DevExpress.XtraEditors.CheckEdit();
            this.UserNameText = new DevExpress.XtraEditors.ButtonEdit();
            this.PasswordText = new DevExpress.XtraEditors.ButtonEdit();
            this.UserNameLabel = new DevExpress.XtraEditors.LabelControl();
            this.PasswordLabel = new DevExpress.XtraEditors.LabelControl();
            this.ForgetPasswordLabel = new DevExpress.XtraEditors.LabelControl();
            this.SaveCheck = new DevExpress.XtraEditors.CheckEdit();
            this.BCancel = new ManiXButton.XButton();
            this.BOK = new ManiXButton.XButton();
            ((System.ComponentModel.ISupportInitialize)(this.AzCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RuCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveCheck.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // AzCheck
            // 
            this.AzCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AzCheck.EditValue = true;
            this.AzCheck.Location = new System.Drawing.Point(413, 338);
            this.AzCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AzCheck.Name = "AzCheck";
            this.AzCheck.Properties.Caption = "AZ";
            this.AzCheck.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.AzCheck.Properties.ImageIndexChecked = 0;
            this.AzCheck.Properties.ImageIndexGrayed = 0;
            this.AzCheck.Properties.ImageIndexUnchecked = 0;
            this.AzCheck.Properties.RadioGroupIndex = 1;
            this.AzCheck.Size = new System.Drawing.Size(45, 20);
            superToolTip4.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem4.Text = "<color=255,0,0>Azərbaycan dili</color>";
            toolTipItem4.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipItem4.Appearance.Options.UseImage = true;
            toolTipItem4.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem4.Image")));
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = "Sistemi Azərbaycan dilinə çevirmək üçün nəzərdə tutulub.";
            superToolTip4.Items.Add(toolTipTitleItem4);
            superToolTip4.Items.Add(toolTipItem4);
            this.AzCheck.SuperTip = superToolTip4;
            this.AzCheck.TabIndex = 5;
            this.AzCheck.Click += new System.EventHandler(this.AzCheck_Click);
            // 
            // EnCheck
            // 
            this.EnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EnCheck.Enabled = false;
            this.EnCheck.Location = new System.Drawing.Point(486, 338);
            this.EnCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EnCheck.Name = "EnCheck";
            this.EnCheck.Properties.Caption = "EN";
            this.EnCheck.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.EnCheck.Properties.RadioGroupIndex = 2;
            this.EnCheck.Size = new System.Drawing.Size(45, 20);
            superToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem1.Text = "<color=255,0,0>İngilis dili</color>";
            toolTipItem1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            toolTipItem1.Appearance.Options.UseImage = true;
            toolTipItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem1.Image")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Sistemi İngilis dilinə çevirmək üçün nəzərdə tutulub.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.EnCheck.SuperTip = superToolTip1;
            this.EnCheck.TabIndex = 6;
            this.EnCheck.TabStop = false;
            this.EnCheck.Click += new System.EventHandler(this.EnCheck_Click);
            // 
            // RuCheck
            // 
            this.RuCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RuCheck.Enabled = false;
            this.RuCheck.Location = new System.Drawing.Point(556, 338);
            this.RuCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RuCheck.Name = "RuCheck";
            this.RuCheck.Properties.Caption = "RU";
            this.RuCheck.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.RuCheck.Properties.RadioGroupIndex = 3;
            this.RuCheck.Size = new System.Drawing.Size(45, 20);
            superToolTip2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem2.Text = "<color=255,0,0>Rus dili</color>";
            toolTipItem2.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            toolTipItem2.Appearance.Options.UseImage = true;
            toolTipItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolTipItem2.Image")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Sistemi Rus dilinə çevirmək üçün nəzərdə tutulub.";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.RuCheck.SuperTip = superToolTip2;
            this.RuCheck.TabIndex = 7;
            this.RuCheck.TabStop = false;
            this.RuCheck.Click += new System.EventHandler(this.RuCheck_Click);
            // 
            // UserNameText
            // 
            this.UserNameText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserNameText.EditValue = "";
            this.UserNameText.Location = new System.Drawing.Point(325, 86);
            this.UserNameText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UserNameText.Name = "UserNameText";
            this.UserNameText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.UserNameText.Properties.Appearance.Options.UseFont = true;
            this.UserNameText.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            editorButtonImageOptions3.Image = global::CRS.Properties.Resources.User_icon;
            this.UserNameText.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, true, editorButtonImageOptions3)});
            this.UserNameText.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.UserNameText.Properties.NullText = "df";
            this.UserNameText.Properties.NullValuePrompt = "İstifadəçi adını daxil edin";
            this.UserNameText.Properties.NullValuePromptShowForEmptyValue = true;
            this.UserNameText.Size = new System.Drawing.Size(361, 24);
            this.UserNameText.TabIndex = 0;
            this.UserNameText.TextChanged += new System.EventHandler(this.UserNameText_TextChanged);
            // 
            // PasswordText
            // 
            this.PasswordText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PasswordText.EditValue = "";
            this.PasswordText.Location = new System.Drawing.Point(325, 158);
            this.PasswordText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.PasswordText.Properties.Appearance.Options.UseFont = true;
            this.PasswordText.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            editorButtonImageOptions1.Image = global::CRS.Properties.Resources.password_icon;
            this.PasswordText.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, true, editorButtonImageOptions1)});
            this.PasswordText.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.PasswordText.Properties.NullValuePrompt = "Şifrəni daxil edin";
            this.PasswordText.Properties.NullValuePromptShowForEmptyValue = true;
            this.PasswordText.Properties.PasswordChar = '*';
            this.PasswordText.Size = new System.Drawing.Size(361, 24);
            this.PasswordText.TabIndex = 1;
            this.PasswordText.TextChanged += new System.EventHandler(this.UserNameText_TextChanged);
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.UserNameLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UserNameLabel.Appearance.Options.UseFont = true;
            this.UserNameLabel.Appearance.Options.UseForeColor = true;
            this.UserNameLabel.Location = new System.Drawing.Point(324, 63);
            this.UserNameLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(87, 17);
            this.UserNameLabel.TabIndex = 10;
            this.UserNameLabel.Text = "İstifadəçi adı";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.PasswordLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PasswordLabel.Appearance.Options.UseFont = true;
            this.PasswordLabel.Appearance.Options.UseForeColor = true;
            this.PasswordLabel.Location = new System.Drawing.Point(325, 134);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(31, 17);
            this.PasswordLabel.TabIndex = 11;
            this.PasswordLabel.Text = "Şifrə";
            // 
            // ForgetPasswordLabel
            // 
            this.ForgetPasswordLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ForgetPasswordLabel.Appearance.ForeColor = System.Drawing.Color.DarkBlue;
            this.ForgetPasswordLabel.Appearance.Options.UseFont = true;
            this.ForgetPasswordLabel.Appearance.Options.UseForeColor = true;
            this.ForgetPasswordLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForgetPasswordLabel.Location = new System.Drawing.Point(550, 201);
            this.ForgetPasswordLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ForgetPasswordLabel.Name = "ForgetPasswordLabel";
            this.ForgetPasswordLabel.Size = new System.Drawing.Size(136, 17);
            this.ForgetPasswordLabel.TabIndex = 12;
            this.ForgetPasswordLabel.Text = "Şifrəni unutmusuz?";
            this.ForgetPasswordLabel.Click += new System.EventHandler(this.ForgetPasswordLabel_Click);
            // 
            // SaveCheck
            // 
            this.SaveCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveCheck.Location = new System.Drawing.Point(325, 197);
            this.SaveCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveCheck.Name = "SaveCheck";
            this.SaveCheck.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveCheck.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SaveCheck.Properties.Appearance.Options.UseFont = true;
            this.SaveCheck.Properties.Appearance.Options.UseForeColor = true;
            this.SaveCheck.Properties.Caption = "Yadda saxla";
            this.SaveCheck.Size = new System.Drawing.Size(120, 21);
            this.SaveCheck.TabIndex = 13;
            this.SaveCheck.TabStop = false;
            // 
            // BCancel
            // 
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
            this.BCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BCancel.Location = new System.Drawing.Point(511, 250);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(175, 47);
            this.BCancel.TabIndex = 14;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // BOK
            // 
            office2010Blue2.BorderColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue2.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue2.ButtonMouseOverColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue2.ButtonMouseOverColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue2.ButtonMouseOverColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(137)))));
            office2010Blue2.ButtonMouseOverColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            office2010Blue2.ButtonNormalColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(72)))), ((int)(((byte)(161)))));
            office2010Blue2.ButtonNormalColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(135)))), ((int)(((byte)(228)))));
            office2010Blue2.ButtonNormalColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(97)))), ((int)(((byte)(181)))));
            office2010Blue2.ButtonNormalColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(125)))), ((int)(((byte)(219)))));
            office2010Blue2.ButtonSelectedColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(199)))), ((int)(((byte)(87)))));
            office2010Blue2.ButtonSelectedColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(215)))));
            office2010Blue2.ButtonSelectedColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(117)))));
            office2010Blue2.ButtonSelectedColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            office2010Blue2.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue2.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            office2010Blue2.TextColor = System.Drawing.Color.White;
            this.BOK.ColorTable = office2010Blue2;
            this.BOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BOK.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BOK.Location = new System.Drawing.Point(327, 250);
            this.BOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(175, 47);
            this.BOK.TabIndex = 4;
            this.BOK.Text = "Daxil ol";
            this.BOK.Theme = ManiXButton.Theme.MSOffice2010_BLUE;
            this.BOK.UseVisualStyleBackColor = true;
            this.BOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // FConnect
            // 
            this.AcceptButton = this.BOK;
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(715, 389);
            this.ControlBox = false;
            this.Controls.Add(this.BCancel);
            this.Controls.Add(this.RuCheck);
            this.Controls.Add(this.AzCheck);
            this.Controls.Add(this.EnCheck);
            this.Controls.Add(this.SaveCheck);
            this.Controls.Add(this.ForgetPasswordLabel);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UserNameLabel);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.UserNameText);
            this.Controls.Add(this.BOK);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(733, 436);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(733, 436);
            this.Name = "FConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LRS - Leasing Registry System";
            this.Load += new System.EventHandler(this.FConnect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AzCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RuCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveCheck.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ManiXButton.XButton BOK;
        private DevExpress.XtraEditors.CheckEdit AzCheck;
        private DevExpress.XtraEditors.CheckEdit EnCheck;
        private DevExpress.XtraEditors.CheckEdit RuCheck;
        private DevExpress.XtraEditors.ButtonEdit UserNameText;
        private DevExpress.XtraEditors.ButtonEdit PasswordText;
        private DevExpress.XtraEditors.LabelControl UserNameLabel;
        private DevExpress.XtraEditors.LabelControl PasswordLabel;
        private DevExpress.XtraEditors.LabelControl ForgetPasswordLabel;
        private DevExpress.XtraEditors.CheckEdit SaveCheck;
        private ManiXButton.XButton BCancel;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;

    }
}

