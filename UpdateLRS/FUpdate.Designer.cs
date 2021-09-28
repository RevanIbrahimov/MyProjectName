namespace UpdateLRS
{
    partial class FUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FUpdate));
            this.MarqueeProgressBarControl = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.UpdateBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.VersionLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.MarqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MarqueeProgressBarControl
            // 
            this.MarqueeProgressBarControl.EditValue = "update...";
            this.MarqueeProgressBarControl.Location = new System.Drawing.Point(13, 351);
            this.MarqueeProgressBarControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MarqueeProgressBarControl.Name = "MarqueeProgressBarControl";
            this.MarqueeProgressBarControl.Properties.EditValueChangedDelay = 1;
            this.MarqueeProgressBarControl.Properties.EndColor = System.Drawing.Color.Blue;
            this.MarqueeProgressBarControl.Properties.NullText = "fgh";
            this.MarqueeProgressBarControl.Properties.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.Cycle;
            this.MarqueeProgressBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.MarqueeProgressBarControl.Properties.ShowTitle = true;
            this.MarqueeProgressBarControl.Properties.StartColor = System.Drawing.Color.DarkKhaki;
            this.MarqueeProgressBarControl.Size = new System.Drawing.Size(576, 22);
            this.MarqueeProgressBarControl.TabIndex = 15;
            // 
            // UpdateBackgroundWorker
            // 
            this.UpdateBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UpdateBackgroundWorker_DoWork);
            this.UpdateBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.UpdateBackgroundWorker_RunWorkerCompleted);
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(13, 13);
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
            this.pictureEdit2.Size = new System.Drawing.Size(576, 279);
            this.pictureEdit2.TabIndex = 17;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AllowHtmlString = true;
            this.VersionLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.VersionLabel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.VersionLabel.Appearance.Options.UseFont = true;
            this.VersionLabel.Appearance.Options.UseForeColor = true;
            this.VersionLabel.Location = new System.Drawing.Point(13, 314);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(4);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(148, 17);
            this.VersionLabel.TabIndex = 16;
            this.VersionLabel.Text = "Yeni versiya: v1.0.2.0";
            // 
            // FUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 400);
            this.Controls.Add(this.MarqueeProgressBarControl);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.VersionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUpdate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update LRS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MarqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl MarqueeProgressBarControl;
        private System.ComponentModel.BackgroundWorker UpdateBackgroundWorker;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.LabelControl VersionLabel;
    }
}

