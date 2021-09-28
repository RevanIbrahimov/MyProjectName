namespace CRS.Forms.Total
{
    partial class FContractImages
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
            this.ContractImageSlider = new DevExpress.XtraEditors.Controls.ImageSlider();
            ((System.ComponentModel.ISupportInitialize)(this.ContractImageSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // ContractImageSlider
            // 
            this.ContractImageSlider.AllowDrop = true;
            this.ContractImageSlider.AllowLooping = true;
            this.ContractImageSlider.AnimationTime = 500;
            this.ContractImageSlider.ContextButtonOptions.BottomPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ContractImageSlider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContractImageSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContractImageSlider.LayoutMode = DevExpress.Utils.Drawing.ImageLayoutMode.MiddleCenter;
            this.ContractImageSlider.Location = new System.Drawing.Point(0, 0);
            this.ContractImageSlider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ContractImageSlider.Name = "ContractImageSlider";
            this.ContractImageSlider.Size = new System.Drawing.Size(1697, 729);
            this.ContractImageSlider.TabIndex = 158;
            this.ContractImageSlider.TabStop = false;
            this.ContractImageSlider.Text = "imageSlider1";
            // 
            // FContractImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 729);
            this.Controls.Add(this.ContractImageSlider);
            this.MinimizeBox = false;
            this.Name = "FContractImages";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Müqaviləyə aid olan şəkillər";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FContractImages_FormClosing);
            this.Load += new System.EventHandler(this.FContractImages_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ContractImageSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Controls.ImageSlider ContractImageSlider;
    }
}