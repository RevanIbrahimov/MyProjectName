namespace CRS.Forms
{
    partial class FCreditCalculator
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
            ManiXButton.Office2010Red office2010Red1 = new ManiXButton.Office2010Red();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCreditCalculator));
            this.PanelOption = new DevExpress.XtraEditors.PanelControl();
            this.BCancel = new ManiXButton.XButton();
            this.CreditAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.PeriodLabel = new DevExpress.XtraEditors.LabelControl();
            this.InterestLabel = new DevExpress.XtraEditors.LabelControl();
            this.MonthlyAmountLabel = new DevExpress.XtraEditors.LabelControl();
            this.CreditAmountValue = new DevExpress.XtraEditors.CalcEdit();
            this.PeriodValue = new DevExpress.XtraEditors.CalcEdit();
            this.InterestValue = new DevExpress.XtraEditors.CalcEdit();
            //this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            //this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.MonthlyPaymentText = new DevExpress.XtraEditors.TextEdit();
            this.SumPaymentText = new DevExpress.XtraEditors.TextEdit();
            this.SumPaymentLabel = new DevExpress.XtraEditors.LabelControl();
            this.InterestAmountText = new DevExpress.XtraEditors.TextEdit();
            this.InterestAmountLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).BeginInit();
            this.PanelOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CreditAmountValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthlyPaymentText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SumPaymentText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestAmountText.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelOption
            // 
            this.PanelOption.Controls.Add(this.BCancel);
            this.PanelOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelOption.Location = new System.Drawing.Point(0, 191);
            this.PanelOption.Name = "PanelOption";
            this.PanelOption.Size = new System.Drawing.Size(284, 50);
            this.PanelOption.TabIndex = 50;
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
            this.BCancel.Location = new System.Drawing.Point(195, 13);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 25);
            this.BCancel.TabIndex = 5;
            this.BCancel.Text = "Bağla";
            this.BCancel.Theme = ManiXButton.Theme.MSOffice2010_RED;
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // CreditAmountLabel
            // 
            this.CreditAmountLabel.Location = new System.Drawing.Point(12, 15);
            this.CreditAmountLabel.Name = "CreditAmountLabel";
            this.CreditAmountLabel.Size = new System.Drawing.Size(75, 13);
            this.CreditAmountLabel.TabIndex = 51;
            this.CreditAmountLabel.Text = "Kreditin məbləği";
            // 
            // PeriodLabel
            // 
            this.PeriodLabel.Location = new System.Drawing.Point(12, 41);
            this.PeriodLabel.Name = "PeriodLabel";
            this.PeriodLabel.Size = new System.Drawing.Size(61, 13);
            this.PeriodLabel.TabIndex = 52;
            this.PeriodLabel.Text = "Müddəti (ay)";
            // 
            // InterestLabel
            // 
            this.InterestLabel.Location = new System.Drawing.Point(12, 67);
            this.InterestLabel.Name = "InterestLabel";
            this.InterestLabel.Size = new System.Drawing.Size(59, 13);
            this.InterestLabel.TabIndex = 53;
            this.InterestLabel.Text = "İllik faizi (%)";
            // 
            // MonthlyAmountLabel
            // 
            this.MonthlyAmountLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.MonthlyAmountLabel.Location = new System.Drawing.Point(12, 111);
            this.MonthlyAmountLabel.Name = "MonthlyAmountLabel";
            this.MonthlyAmountLabel.Size = new System.Drawing.Size(68, 13);
            this.MonthlyAmountLabel.TabIndex = 54;
            this.MonthlyAmountLabel.Text = "Aylıq ödəniş";
            // 
            // CreditAmountValue
            // 
            this.CreditAmountValue.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CreditAmountValue.Location = new System.Drawing.Point(127, 12);
            this.CreditAmountValue.Name = "CreditAmountValue";
            this.CreditAmountValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "Kalkulyatoru aç", null, null, true)});
            this.CreditAmountValue.Properties.Mask.EditMask = "n2";
            this.CreditAmountValue.Properties.Precision = 2;
            this.CreditAmountValue.Size = new System.Drawing.Size(143, 20);
            this.CreditAmountValue.TabIndex = 0;
            this.CreditAmountValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // PeriodValue
            // 
            this.PeriodValue.EditValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.PeriodValue.Location = new System.Drawing.Point(127, 38);
            this.PeriodValue.Name = "PeriodValue";
            this.PeriodValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Kalkulyatoru aç", null, null, true)});
            this.PeriodValue.Properties.Mask.EditMask = "n0";
            this.PeriodValue.Properties.Precision = 0;
            this.PeriodValue.Size = new System.Drawing.Size(143, 20);
            this.PeriodValue.TabIndex = 1;
            this.PeriodValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // InterestValue
            // 
            this.InterestValue.EditValue = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.InterestValue.Location = new System.Drawing.Point(127, 64);
            this.InterestValue.Name = "InterestValue";
            this.InterestValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "Kalkulyatoru aç", null, null, true)});
            this.InterestValue.Properties.Mask.EditMask = "n2";
            this.InterestValue.Properties.Precision = 2;
            this.InterestValue.Size = new System.Drawing.Size(143, 20);
            this.InterestValue.TabIndex = 2;
            this.InterestValue.EditValueChanged += new System.EventHandler(this.CreditAmountValue_EditValueChanged);
            // 
            // shapeContainer1
            // 
            //this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            //this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            //this.shapeContainer1.Name = "shapeContainer1";
            //this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            //this.lineShape1});
            //this.shapeContainer1.Size = new System.Drawing.Size(284, 241);
            //this.shapeContainer1.TabIndex = 60;
            //this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            //this.lineShape1.Name = "lineShape1";
            //this.lineShape1.X1 = 13;
            //this.lineShape1.X2 = 268;
            //this.lineShape1.Y1 = 95;
            //this.lineShape1.Y2 = 95;
            // 
            // MonthlyPaymentText
            // 
            this.MonthlyPaymentText.Location = new System.Drawing.Point(127, 108);
            this.MonthlyPaymentText.Name = "MonthlyPaymentText";
            this.MonthlyPaymentText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MonthlyPaymentText.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.MonthlyPaymentText.Properties.Appearance.Options.UseFont = true;
            this.MonthlyPaymentText.Properties.Appearance.Options.UseForeColor = true;
            this.MonthlyPaymentText.Properties.ReadOnly = true;
            this.MonthlyPaymentText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MonthlyPaymentText.Size = new System.Drawing.Size(143, 20);
            this.MonthlyPaymentText.TabIndex = 61;
            // 
            // SumPaymentText
            // 
            this.SumPaymentText.Location = new System.Drawing.Point(127, 133);
            this.SumPaymentText.Name = "SumPaymentText";
            this.SumPaymentText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SumPaymentText.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.SumPaymentText.Properties.Appearance.Options.UseFont = true;
            this.SumPaymentText.Properties.Appearance.Options.UseForeColor = true;
            this.SumPaymentText.Properties.ReadOnly = true;
            this.SumPaymentText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SumPaymentText.Size = new System.Drawing.Size(143, 20);
            this.SumPaymentText.TabIndex = 63;
            // 
            // SumPaymentLabel
            // 
            this.SumPaymentLabel.Location = new System.Drawing.Point(12, 137);
            this.SumPaymentLabel.Name = "SumPaymentLabel";
            this.SumPaymentLabel.Size = new System.Drawing.Size(57, 13);
            this.SumPaymentLabel.TabIndex = 62;
            this.SumPaymentLabel.Text = "Cəmi ödəniş";
            // 
            // InterestAmountText
            // 
            this.InterestAmountText.Location = new System.Drawing.Point(127, 160);
            this.InterestAmountText.Name = "InterestAmountText";
            this.InterestAmountText.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InterestAmountText.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.InterestAmountText.Properties.Appearance.Options.UseFont = true;
            this.InterestAmountText.Properties.Appearance.Options.UseForeColor = true;
            this.InterestAmountText.Properties.ReadOnly = true;
            this.InterestAmountText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.InterestAmountText.Size = new System.Drawing.Size(143, 20);
            this.InterestAmountText.TabIndex = 65;
            // 
            // InterestAmountLabel
            // 
            this.InterestAmountLabel.Location = new System.Drawing.Point(12, 163);
            this.InterestAmountLabel.Name = "InterestAmountLabel";
            this.InterestAmountLabel.Size = new System.Drawing.Size(58, 13);
            this.InterestAmountLabel.TabIndex = 64;
            this.InterestAmountLabel.Text = "Faiz məbləği";
            // 
            // FCreditCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(284, 241);
            this.Controls.Add(this.InterestAmountText);
            this.Controls.Add(this.InterestAmountLabel);
            this.Controls.Add(this.SumPaymentText);
            this.Controls.Add(this.SumPaymentLabel);
            this.Controls.Add(this.MonthlyPaymentText);
            this.Controls.Add(this.InterestValue);
            this.Controls.Add(this.PeriodValue);
            this.Controls.Add(this.CreditAmountValue);
            this.Controls.Add(this.MonthlyAmountLabel);
            this.Controls.Add(this.InterestLabel);
            this.Controls.Add(this.PeriodLabel);
            this.Controls.Add(this.CreditAmountLabel);
            this.Controls.Add(this.PanelOption);
            //this.Controls.Add(this.shapeContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 280);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 280);
            this.Name = "FCreditCalculator";
            this.ShowInTaskbar = false;
            this.Text = "Kredit kalkulyatoru";
            this.Load += new System.EventHandler(this.FCreditCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PanelOption)).EndInit();
            this.PanelOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CreditAmountValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthlyPaymentText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SumPaymentText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InterestAmountText.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PanelOption;
        private ManiXButton.XButton BCancel;
        private DevExpress.XtraEditors.LabelControl CreditAmountLabel;
        private DevExpress.XtraEditors.LabelControl PeriodLabel;
        private DevExpress.XtraEditors.LabelControl InterestLabel;
        private DevExpress.XtraEditors.LabelControl MonthlyAmountLabel;
        private DevExpress.XtraEditors.CalcEdit CreditAmountValue;
        private DevExpress.XtraEditors.CalcEdit PeriodValue;
        private DevExpress.XtraEditors.CalcEdit InterestValue;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private DevExpress.XtraEditors.TextEdit MonthlyPaymentText;
        private DevExpress.XtraEditors.TextEdit SumPaymentText;
        private DevExpress.XtraEditors.LabelControl SumPaymentLabel;
        private DevExpress.XtraEditors.TextEdit InterestAmountText;
        private DevExpress.XtraEditors.LabelControl InterestAmountLabel;
    }
}