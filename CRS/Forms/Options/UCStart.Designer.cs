namespace CRS.Forms.Options
{
    partial class UCStart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GroupControl = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.LanguageRadioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.MenuRadioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DateSortRadioGroup = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).BeginInit();
            this.GroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LanguageRadioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuRadioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateSortRadioGroup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupControl
            // 
            this.GroupControl.AppearanceCaption.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupControl.AppearanceCaption.ForeColor = System.Drawing.Color.DarkRed;
            this.GroupControl.AppearanceCaption.Options.UseFont = true;
            this.GroupControl.AppearanceCaption.Options.UseForeColor = true;
            this.GroupControl.CaptionImageOptions.Image = global::CRS.Properties.Resources.start_16;
            this.GroupControl.Controls.Add(this.labelControl3);
            this.GroupControl.Controls.Add(this.DateSortRadioGroup);
            this.GroupControl.Controls.Add(this.labelControl2);
            this.GroupControl.Controls.Add(this.LanguageRadioGroup);
            this.GroupControl.Controls.Add(this.labelControl1);
            this.GroupControl.Controls.Add(this.MenuRadioGroup);
            this.GroupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupControl.Location = new System.Drawing.Point(0, 0);
            this.GroupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupControl.Name = "GroupControl";
            this.GroupControl.Size = new System.Drawing.Size(958, 614);
            this.GroupControl.TabIndex = 1;
            this.GroupControl.Text = "Sistemin açılma parametrləri";
            this.GroupControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupControl_Paint);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 370);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(240, 16);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Proqram açılanda birinci hansı dil seçilsin?";
            // 
            // LanguageRadioGroup
            // 
            this.LanguageRadioGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguageRadioGroup.Location = new System.Drawing.Point(17, 405);
            this.LanguageRadioGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LanguageRadioGroup.Name = "LanguageRadioGroup";
            this.LanguageRadioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Azərbaycan dili"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "İngilis dili", false),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Rus dili", false)});
            this.LanguageRadioGroup.Size = new System.Drawing.Size(924, 96);
            this.LanguageRadioGroup.TabIndex = 4;
            this.LanguageRadioGroup.SelectedIndexChanged += new System.EventHandler(this.LanguageRadioGroup_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 50);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(259, 16);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Proqram açılanda birinci hansı menyu açılsın?";
            // 
            // MenuRadioGroup
            // 
            this.MenuRadioGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuRadioGroup.Location = new System.Drawing.Point(17, 85);
            this.MenuRadioGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MenuRadioGroup.Name = "MenuRadioGroup";
            this.MenuRadioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Məlumat üçün"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Müştərilər"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Müqavilələr"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Lizinq Portfeli"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Kassa əməliyyatları"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Bank əməliyyatları"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Cəlb olunmuş vəsaitlər"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Mühasibatlıq"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Ödəniş tapşırıqları"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Smslər")});
            this.MenuRadioGroup.Size = new System.Drawing.Size(924, 258);
            this.MenuRadioGroup.TabIndex = 2;
            this.MenuRadioGroup.SelectedIndexChanged += new System.EventHandler(this.MenuRadioGroup_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(17, 521);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(342, 16);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Portfeldə ödənişlər açılanda tarix hansı ardıcıllıqla düzülsün?";
            // 
            // DateSortRadioGroup
            // 
            this.DateSortRadioGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DateSortRadioGroup.Location = new System.Drawing.Point(17, 556);
            this.DateSortRadioGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateSortRadioGroup.Name = "DateSortRadioGroup";
            this.DateSortRadioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Artan sıra ilə"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Azalan sıra ilə")});
            this.DateSortRadioGroup.Size = new System.Drawing.Size(924, 34);
            this.DateSortRadioGroup.TabIndex = 6;
            this.DateSortRadioGroup.SelectedIndexChanged += new System.EventHandler(this.DateSortRadioGroup_SelectedIndexChanged);
            // 
            // UCStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupControl);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCStart";
            this.Size = new System.Drawing.Size(958, 614);
            this.Load += new System.EventHandler(this.UCStart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).EndInit();
            this.GroupControl.ResumeLayout(false);
            this.GroupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LanguageRadioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuRadioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateSortRadioGroup.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GroupControl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.RadioGroup LanguageRadioGroup;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.RadioGroup MenuRadioGroup;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.RadioGroup DateSortRadioGroup;
    }
}
