namespace CRS.Forms.Info
{
    partial class FRateAnalysis
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraCharts.StackedLineSeriesView stackedLineSeriesView1 = new DevExpress.XtraCharts.StackedLineSeriesView();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRateAnalysis));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ShowLabelCheck = new DevExpress.XtraEditors.CheckEdit();
            this.CurrencyLabel = new DevExpress.XtraEditors.LabelControl();
            this.CurrencyComboBox = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ToDateValue = new DevExpress.XtraEditors.DateEdit();
            this.FromDateValue = new DevExpress.XtraEditors.DateEdit();
            this.DateLabel = new DevExpress.XtraEditors.LabelControl();
            this.chart = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowLabelCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrencyComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stackedLineSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ShowLabelCheck);
            this.panelControl1.Controls.Add(this.CurrencyLabel);
            this.panelControl1.Controls.Add(this.CurrencyComboBox);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.ToDateValue);
            this.panelControl1.Controls.Add(this.FromDateValue);
            this.panelControl1.Controls.Add(this.DateLabel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1230, 44);
            this.panelControl1.TabIndex = 1;
            // 
            // ShowLabelCheck
            // 
            this.ShowLabelCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowLabelCheck.EditValue = true;
            this.ShowLabelCheck.Location = new System.Drawing.Point(965, 12);
            this.ShowLabelCheck.Name = "ShowLabelCheck";
            this.ShowLabelCheck.Properties.Caption = "Xəttin üzərində qiyməti göstər";
            this.ShowLabelCheck.Size = new System.Drawing.Size(195, 19);
            this.ShowLabelCheck.TabIndex = 229;
            this.ShowLabelCheck.CheckedChanged += new System.EventHandler(this.ToDateValue_EditValueChanged);
            // 
            // CurrencyLabel
            // 
            this.CurrencyLabel.Location = new System.Drawing.Point(280, 15);
            this.CurrencyLabel.Name = "CurrencyLabel";
            this.CurrencyLabel.Size = new System.Drawing.Size(36, 13);
            this.CurrencyLabel.TabIndex = 228;
            this.CurrencyLabel.Text = "Valyuta";
            // 
            // CurrencyComboBox
            // 
            this.CurrencyComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CurrencyComboBox.Location = new System.Drawing.Point(337, 12);
            this.CurrencyComboBox.Name = "CurrencyComboBox";
            this.CurrencyComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "Siyahını aç", null, null, true)});
            this.CurrencyComboBox.Properties.NullValuePrompt = "Valyutaları seçin";
            this.CurrencyComboBox.Properties.NullValuePromptShowForEmptyValue = true;
            this.CurrencyComboBox.Size = new System.Drawing.Size(233, 20);
            this.CurrencyComboBox.TabIndex = 227;
            this.CurrencyComboBox.EditValueChanged += new System.EventHandler(this.CurrencyComboBox_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(792, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(4, 13);
            this.labelControl2.TabIndex = 226;
            this.labelControl2.Text = "-";
            // 
            // ToDateValue
            // 
            this.ToDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ToDateValue.EditValue = null;
            this.ToDateValue.Location = new System.Drawing.Point(804, 12);
            this.ToDateValue.Name = "ToDateValue";
            this.ToDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Kalendarı aç", null, null, true)});
            this.ToDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ToDateValue.Size = new System.Drawing.Size(85, 20);
            this.ToDateValue.TabIndex = 224;
            this.ToDateValue.EditValueChanged += new System.EventHandler(this.ToDateValue_EditValueChanged);
            // 
            // FromDateValue
            // 
            this.FromDateValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FromDateValue.EditValue = null;
            this.FromDateValue.Location = new System.Drawing.Point(701, 12);
            this.FromDateValue.Name = "FromDateValue";
            this.FromDateValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "Kalendarı aç", null, null, true)});
            this.FromDateValue.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FromDateValue.Size = new System.Drawing.Size(85, 20);
            this.FromDateValue.TabIndex = 223;
            this.FromDateValue.EditValueChanged += new System.EventHandler(this.FromDateValue_EditValueChanged);
            // 
            // DateLabel
            // 
            this.DateLabel.Location = new System.Drawing.Point(599, 15);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(65, 13);
            this.DateLabel.TabIndex = 225;
            this.DateLabel.Text = "Tarix intervalı";
            // 
            // chart
            // 
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 44);
            this.chart.Name = "chart";
            this.chart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chart.SeriesTemplate.View = stackedLineSeriesView1;
            this.chart.Size = new System.Drawing.Size(1230, 459);
            this.chart.TabIndex = 2;
            // 
            // FRateAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 503);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "FRateAnalysis";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Məzənnələrin qrafik təsviri";
            this.Load += new System.EventHandler(this.FRateAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowLabelCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrencyComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDateValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stackedLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit ToDateValue;
        private DevExpress.XtraEditors.DateEdit FromDateValue;
        private DevExpress.XtraEditors.LabelControl DateLabel;
        private DevExpress.XtraCharts.ChartControl chart;
        private DevExpress.XtraEditors.LabelControl CurrencyLabel;
        private DevExpress.XtraEditors.CheckedComboBoxEdit CurrencyComboBox;
        private DevExpress.XtraEditors.CheckEdit ShowLabelCheck;
    }
}