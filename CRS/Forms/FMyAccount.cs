using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FMyAccount : DevExpress.XtraEditors.XtraForm
    {
        public FMyAccount()
        {
            InitializeComponent();
        }
        
        private void FMyAccount_Load(object sender, EventArgs e)
        {
            string s = $@"SELECT CU.SURNAME||' '||CU.NAME||' '||CU.PATRONYMIC,TO_CHAR(CU.BIRTHDAY,'DD.MM.YYYY'),UG.GROUP_NAME,CU.NIKNAME,CU.ADDRESS,(SELECT TO_CHAR(MAX(CONNECT_DATE),'DD.MM.YYYY HH24:MI:SS AM') FROM CRS_USER.USER_CONNECTIONS WHERE USER_ID = CU.ID) MAX_CON_DATE  FROM CRS_USER.CRS_USERS CU,CRS_USER.USER_GROUP UG WHERE CU.GROUP_ID = UG.ID AND CU.ID = {GlobalVariables.V_UserID}";
            try
            {
                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FMyAccount_Load");

                foreach (DataRow dr in dt.Rows)
                {
                    FullNameLabel.Text = dr[0].ToString();
                    BirthdayValueLabel.Text = dr[1].ToString();
                    GroupLabel.Text = dr[2].ToString();
                    AgeValueLabel.Text = GlobalFunctions.CalculationAgeWithYear(GlobalFunctions.ChangeStringToDate(dr[1].ToString(), "ddmmyyyy"), DateTime.Today);
                    NiknameValueLabel.Text = GlobalFunctions.Decrypt(dr[3].ToString());
                    AddressText.Text = dr[4].ToString();
                    MaxConnectionDateLabel.Text = dr[5].ToString();
                }
                
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçinin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }            
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMyAccount));
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.UserPictureBox = new DevExpress.XtraEditors.PictureEdit();
            this.FullNameLabel = new DevExpress.XtraEditors.LabelControl();
            //this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            //this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.GroupNameLabel = new DevExpress.XtraEditors.LabelControl();
            this.BirthdayLabel = new DevExpress.XtraEditors.LabelControl();
            this.AgeLabel = new DevExpress.XtraEditors.LabelControl();
            this.NiknameLabel = new DevExpress.XtraEditors.LabelControl();
            this.GroupLabel = new DevExpress.XtraEditors.LabelControl();
            this.BirthdayValueLabel = new DevExpress.XtraEditors.LabelControl();
            this.AgeValueLabel = new DevExpress.XtraEditors.LabelControl();
            this.NiknameValueLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.HyperlinkLabel = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.AddressLabel = new DevExpress.XtraEditors.LabelControl();
            this.AddressText = new DevExpress.XtraEditors.MemoEdit();
            this.MaxConnectionDateLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.separatorControl3 = new DevExpress.XtraEditors.SeparatorControl();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserPictureBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl3)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 340);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(710, 50);
            this.PanelOption.TabIndex = 58;
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
            this.BCancel.Location = new System.Drawing.Point(621, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // UserPictureBox
            // 
            this.UserPictureBox.Location = new System.Drawing.Point(12, 12);
            this.UserPictureBox.Name = "UserPictureBox";
            this.UserPictureBox.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.UserPictureBox.Properties.ZoomAccelerationFactor = 1D;
            this.UserPictureBox.Size = new System.Drawing.Size(128, 148);
            this.UserPictureBox.TabIndex = 59;
            // 
            // FullNameLabel
            // 
            this.FullNameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.FullNameLabel.Appearance.Options.UseFont = true;
            this.FullNameLabel.Location = new System.Drawing.Point(172, 12);
            this.FullNameLabel.Name = "FullNameLabel";
            this.FullNameLabel.Size = new System.Drawing.Size(64, 23);
            this.FullNameLabel.TabIndex = 60;
            this.FullNameLabel.Text = "Soyadı";
            // 
            // shapeContainer1
            // 
            //this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            //this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            //this.shapeContainer1.Name = "shapeContainer1";
            //this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            //this.lineShape1});
            //this.shapeContainer1.Size = new System.Drawing.Size(710, 390);
            //this.shapeContainer1.TabIndex = 61;
            //this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            //this.lineShape1.Name = "lineShape1";
            //this.lineShape1.X1 = 151;
            //this.lineShape1.X2 = 151;
            //this.lineShape1.Y1 = -4;
            //this.lineShape1.Y2 = 341;
            // 
            // GroupNameLabel
            // 
            this.GroupNameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupNameLabel.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.GroupNameLabel.Appearance.Options.UseFont = true;
            this.GroupNameLabel.Appearance.Options.UseForeColor = true;
            this.GroupNameLabel.Enabled = false;
            this.GroupNameLabel.Location = new System.Drawing.Point(172, 222);
            this.GroupNameLabel.Name = "GroupNameLabel";
            this.GroupNameLabel.Size = new System.Drawing.Size(138, 13);
            this.GroupNameLabel.TabIndex = 62;
            this.GroupNameLabel.Text = "Daxil olduğu istifadəçi qrupu:";
            // 
            // BirthdayLabel
            // 
            this.BirthdayLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BirthdayLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BirthdayLabel.Appearance.Options.UseFont = true;
            this.BirthdayLabel.Appearance.Options.UseForeColor = true;
            this.BirthdayLabel.Enabled = false;
            this.BirthdayLabel.Location = new System.Drawing.Point(172, 70);
            this.BirthdayLabel.Name = "BirthdayLabel";
            this.BirthdayLabel.Size = new System.Drawing.Size(64, 13);
            this.BirthdayLabel.TabIndex = 63;
            this.BirthdayLabel.Text = "Doğum tarixi:";
            // 
            // AgeLabel
            // 
            this.AgeLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgeLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AgeLabel.Appearance.Options.UseFont = true;
            this.AgeLabel.Appearance.Options.UseForeColor = true;
            this.AgeLabel.Enabled = false;
            this.AgeLabel.Location = new System.Drawing.Point(172, 89);
            this.AgeLabel.Name = "AgeLabel";
            this.AgeLabel.Size = new System.Drawing.Size(23, 13);
            this.AgeLabel.TabIndex = 64;
            this.AgeLabel.Text = "Yaşı:";
            // 
            // NiknameLabel
            // 
            this.NiknameLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NiknameLabel.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.NiknameLabel.Appearance.Options.UseFont = true;
            this.NiknameLabel.Appearance.Options.UseForeColor = true;
            this.NiknameLabel.Enabled = false;
            this.NiknameLabel.Location = new System.Drawing.Point(172, 241);
            this.NiknameLabel.Name = "NiknameLabel";
            this.NiknameLabel.Size = new System.Drawing.Size(65, 13);
            this.NiknameLabel.TabIndex = 65;
            this.NiknameLabel.Text = "İstifadəçi adı:";
            // 
            // GroupLabel
            // 
            this.GroupLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GroupLabel.Appearance.Options.UseFont = true;
            this.GroupLabel.Appearance.Options.UseForeColor = true;
            this.GroupLabel.Enabled = false;
            this.GroupLabel.Location = new System.Drawing.Point(355, 222);
            this.GroupLabel.Name = "GroupLabel";
            this.GroupLabel.Size = new System.Drawing.Size(23, 13);
            this.GroupLabel.TabIndex = 66;
            this.GroupLabel.Text = "Yaşı";
            // 
            // BirthdayValueLabel
            // 
            this.BirthdayValueLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BirthdayValueLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BirthdayValueLabel.Appearance.Options.UseFont = true;
            this.BirthdayValueLabel.Appearance.Options.UseForeColor = true;
            this.BirthdayValueLabel.Enabled = false;
            this.BirthdayValueLabel.Location = new System.Drawing.Point(355, 70);
            this.BirthdayValueLabel.Name = "BirthdayValueLabel";
            this.BirthdayValueLabel.Size = new System.Drawing.Size(23, 13);
            this.BirthdayValueLabel.TabIndex = 67;
            this.BirthdayValueLabel.Text = "Yaşı";
            // 
            // AgeValueLabel
            // 
            this.AgeValueLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgeValueLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AgeValueLabel.Appearance.Options.UseFont = true;
            this.AgeValueLabel.Appearance.Options.UseForeColor = true;
            this.AgeValueLabel.Enabled = false;
            this.AgeValueLabel.Location = new System.Drawing.Point(355, 89);
            this.AgeValueLabel.Name = "AgeValueLabel";
            this.AgeValueLabel.Size = new System.Drawing.Size(23, 13);
            this.AgeValueLabel.TabIndex = 68;
            this.AgeValueLabel.Text = "Yaşı";
            // 
            // NiknameValueLabel
            // 
            this.NiknameValueLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NiknameValueLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.NiknameValueLabel.Appearance.Options.UseFont = true;
            this.NiknameValueLabel.Appearance.Options.UseForeColor = true;
            this.NiknameValueLabel.Enabled = false;
            this.NiknameValueLabel.Location = new System.Drawing.Point(355, 241);
            this.NiknameValueLabel.Name = "NiknameValueLabel";
            this.NiknameValueLabel.Size = new System.Drawing.Size(23, 13);
            this.NiknameValueLabel.TabIndex = 69;
            this.NiknameValueLabel.Text = "Yaşı";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Enabled = false;
            this.labelControl8.Location = new System.Drawing.Point(172, 297);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(30, 13);
            this.labelControl8.TabIndex = 70;
            this.labelControl8.Text = "Şifrəni";
            // 
            // HyperlinkLabel
            // 
            this.HyperlinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HyperlinkLabel.Location = new System.Drawing.Point(208, 297);
            this.HyperlinkLabel.Name = "HyperlinkLabel";
            this.HyperlinkLabel.Size = new System.Drawing.Size(40, 13);
            this.HyperlinkLabel.TabIndex = 71;
            this.HyperlinkLabel.Text = "buradan";
            this.HyperlinkLabel.Click += new System.EventHandler(this.HyperlinkLabel_Click);
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Appearance.Options.UseForeColor = true;
            this.labelControl9.Enabled = false;
            this.labelControl9.Location = new System.Drawing.Point(254, 297);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(78, 13);
            this.labelControl9.TabIndex = 72;
            this.labelControl9.Text = "dəyişə bilərsiniz.";
            // 
            // AddressLabel
            // 
            this.AddressLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddressLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AddressLabel.Appearance.Options.UseFont = true;
            this.AddressLabel.Appearance.Options.UseForeColor = true;
            this.AddressLabel.Enabled = false;
            this.AddressLabel.Location = new System.Drawing.Point(172, 108);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(76, 13);
            this.AddressLabel.TabIndex = 73;
            this.AddressLabel.Text = "Yaşadığı ünvan:";
            // 
            // AddressText
            // 
            this.AddressText.Location = new System.Drawing.Point(355, 108);
            this.AddressText.Name = "AddressText";
            this.AddressText.Properties.ReadOnly = true;
            this.AddressText.Size = new System.Drawing.Size(341, 52);
            this.AddressText.TabIndex = 74;
            // 
            // MaxConnectionDateLabel
            // 
            this.MaxConnectionDateLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxConnectionDateLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MaxConnectionDateLabel.Appearance.Options.UseFont = true;
            this.MaxConnectionDateLabel.Appearance.Options.UseForeColor = true;
            this.MaxConnectionDateLabel.Enabled = false;
            this.MaxConnectionDateLabel.Location = new System.Drawing.Point(355, 176);
            this.MaxConnectionDateLabel.Name = "MaxConnectionDateLabel";
            this.MaxConnectionDateLabel.Size = new System.Drawing.Size(23, 13);
            this.MaxConnectionDateLabel.TabIndex = 76;
            this.MaxConnectionDateLabel.Text = "Yaşı";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Appearance.Options.UseForeColor = true;
            this.labelControl10.Enabled = false;
            this.labelControl10.Location = new System.Drawing.Point(172, 176);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(145, 13);
            this.labelControl10.TabIndex = 75;
            this.labelControl10.Text = "Sistemə ən son qoşulma vaxtı:";
            // 
            // separatorControl1
            // 
            this.separatorControl1.Location = new System.Drawing.Point(172, 260);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(524, 23);
            this.separatorControl1.TabIndex = 77;
            // 
            // separatorControl3
            // 
            this.separatorControl3.Location = new System.Drawing.Point(172, 41);
            this.separatorControl3.Name = "separatorControl3";
            this.separatorControl3.Size = new System.Drawing.Size(524, 23);
            this.separatorControl3.TabIndex = 79;
            // 
            // FMyAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(710, 390);
            this.Controls.Add(this.separatorControl3);
            this.Controls.Add(this.separatorControl1);
            this.Controls.Add(this.MaxConnectionDateLabel);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.AddressText);
            this.Controls.Add(this.AddressLabel);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.HyperlinkLabel);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.NiknameValueLabel);
            this.Controls.Add(this.AgeValueLabel);
            this.Controls.Add(this.BirthdayValueLabel);
            this.Controls.Add(this.GroupLabel);
            this.Controls.Add(this.NiknameLabel);
            this.Controls.Add(this.AgeLabel);
            this.Controls.Add(this.BirthdayLabel);
            this.Controls.Add(this.GroupNameLabel);
            this.Controls.Add(this.FullNameLabel);
            this.Controls.Add(this.UserPictureBox);
            this.Controls.Add(this.PanelOption);
            //this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(726, 429);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(726, 429);
            this.Name = "FMyAccount";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mənim hesabım";
            this.Load += new System.EventHandler(this.FMyAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserPictureBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void HyperlinkLabel_Click(object sender, EventArgs e)
        {
            FChangePassword fcp = new FChangePassword();
            fcp.Show();
        }
    }
}