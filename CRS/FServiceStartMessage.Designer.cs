namespace CRS
{
    partial class FServiceStartMessage
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BCancel = new DevExpress.XtraEditors.SimpleButton();
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.ServiceBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.DescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(14, 39);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(524, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Bazanın servisləri sönüb. Servislər avtomatik start edilir. Zəhmət olmasa gözləyi" +
    "n.";
            // 
            // BCancel
            // 
            this.BCancel.Location = new System.Drawing.Point(451, 176);
            this.BCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(87, 28);
            this.BCancel.TabIndex = 1;
            this.BCancel.Text = "İmtina et";
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = 0;
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(14, 89);
            this.marqueeProgressBarControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(524, 22);
            this.marqueeProgressBarControl1.TabIndex = 2;
            // 
            // ServiceBackgroundWorker
            // 
            this.ServiceBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ServiceBackgroundWorker_DoWork);
            this.ServiceBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ServiceBackgroundWorker_RunWorkerCompleted);
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.DescriptionLabel.Appearance.Options.UseFont = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(14, 119);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(4);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(63, 17);
            this.DescriptionLabel.TabIndex = 8;
            this.DescriptionLabel.Text = "Yüklənilir...";
            // 
            // FServiceStartMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 217);
            this.ControlBox = false;
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.Controls.Add(this.BCancel);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FServiceStartMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FServiceStartMessage_FormClosing);
            this.Load += new System.EventHandler(this.FServiceStartMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BCancel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private System.ComponentModel.BackgroundWorker ServiceBackgroundWorker;
        private DevExpress.XtraEditors.LabelControl DescriptionLabel;
    }
}