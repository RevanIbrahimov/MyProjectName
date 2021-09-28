namespace CRS.Forms
{
    partial class LRSSplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LRSSplashScreen));
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.DescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.LRSBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.VersionLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = 0;
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(31, 284);
            this.marqueeProgressBarControl1.Margin = new System.Windows.Forms.Padding(4);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(539, 15);
            this.marqueeProgressBarControl1.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl1.Location = new System.Drawing.Point(31, 354);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(102, 16);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "Copyright © 2021";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.DescriptionLabel.Appearance.Options.UseFont = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(31, 254);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(4);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(63, 17);
            this.DescriptionLabel.TabIndex = 7;
            this.DescriptionLabel.Text = "Yüklənilir...";
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureEdit3.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit3.EditValue = global::CRS.Properties.Resources.ProLizinqLogo;
            this.pictureEdit3.Location = new System.Drawing.Point(335, 325);
            this.pictureEdit3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.AllowFocused = false;
            this.pictureEdit3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit3.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit3.Properties.ShowMenu = false;
            this.pictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit3.Properties.ZoomAccelerationFactor = 1D;
            this.pictureEdit3.Size = new System.Drawing.Size(249, 76);
            this.pictureEdit3.TabIndex = 11;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(16, 15);
            this.pictureEdit2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.AllowFocused = false;
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.Image;
            this.pictureEdit2.Properties.ShowMenu = false;
            this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit2.Properties.ZoomAccelerationFactor = 1D;
            this.pictureEdit2.Size = new System.Drawing.Size(568, 222);
            this.pictureEdit2.TabIndex = 9;
            // 
            // LRSBackgroundWorker
            // 
            this.LRSBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LRSBackgroundWorker_DoWork);
            this.LRSBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LRSBackgroundWorker_RunWorkerCompleted);
            // 
            // VersionLabel
            // 
            this.VersionLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.VersionLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.VersionLabel.Appearance.Options.UseFont = true;
            this.VersionLabel.Appearance.Options.UseForeColor = true;
            this.VersionLabel.Location = new System.Drawing.Point(511, 254);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(4);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(56, 17);
            this.VersionLabel.TabIndex = 12;
            this.VersionLabel.Text = "v1.0.2.0";
            // 
            // LRSSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 414);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.pictureEdit3);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LRSSplashScreen";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LRSSplashScreen_FormClosed);
            this.Load += new System.EventHandler(this.LRSSplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl DescriptionLabel;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private System.ComponentModel.BackgroundWorker LRSBackgroundWorker;
        private DevExpress.XtraEditors.LabelControl VersionLabel;
    }
}
