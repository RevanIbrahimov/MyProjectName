using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FExchangeCalculator : DevExpress.XtraEditors.XtraForm
    {
        public FExchangeCalculator()
        {
            InitializeComponent();
        }
        public string RateDate;
        string RateID;
        int cur1, cur2;

        private void FExchangeCalculator_Load(object sender, EventArgs e)
        {            
            DateValue.Properties.MaxValue = DateTime.Today;
            if (String.IsNullOrEmpty(RateDate))
                DateValue.EditValue = DateTime.Today;
            else
                DateValue.EditValue = GlobalFunctions.ChangeStringToDate(RateDate, "ddmmyyyy");

            GlobalProcedures.FillComboBoxEdit(Currency1ComboBox, "CURRENCY", "CODE,CODE,CODE", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillComboBoxEdit(Currency2ComboBox, "CURRENCY", "CODE,CODE,CODE", "1 = 1 ORDER BY ORDER_ID");

            LoadExchangesDataGridView();
            RatesGroupControl.Text = DateValue.Text + " tarixinə olan valyuta məzənnələri";
        }

        private void LoadExchangesDataGridView()
        {
            string s = $@"SELECT 1 SS,C.VALUE||' '||C.CODE||' ('||C.NAME||')',E.AMOUNT,E.ID,E.USED_USER_ID FROM CRS_USER.CURRENCY_RATES E,CRS_USER.CURRENCY C WHERE E.CURRENCY_ID = C.ID AND E.RATE_DATE = TO_DATE('{DateValue.Text}','DD/MM/YYYY') ORDER BY C.ORDER_ID";
            try
            {
                ExchangesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
                ExchangesGridView.PopulateColumns();
                ExchangesGridView.Columns[0].Caption = "S/s";
                ExchangesGridView.Columns[1].Caption = "Valyuta";
                ExchangesGridView.Columns[2].Caption = "Məzənnə";
                ExchangesGridView.Columns[3].Visible = false;
                ExchangesGridView.Columns[4].Visible = false;

                ExchangesGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ExchangesGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                
                DeleteBarButton.Enabled = EditBarButton.Enabled = AmountValue.Enabled = (ExchangesGridView.RowCount > 0);                   

                ExchangesGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məzənnələr yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        void RefreshExchange()
        {
            RatesGroupControl.Text = DateValue.Text + " tarixinə olan valyuta məzənnələri";
            LoadExchangesDataGridView();
            ResultText.Text = GlobalFunctions.CalculatedExchange((double)AmountValue.Value, cur1, DateValue.Text, cur2, DateValue.Text).ToString();
        }

        private void LoadFExchangeAddEdit(string transaction, string id)
        {
            Info.FExchangeAddEdit fc = new Info.FExchangeAddEdit();
            fc.TransactionName = transaction;
            fc.RateID = id;
            fc.RefreshExchangesDataGridView += new Info.FExchangeAddEdit.DoEvent(RefreshExchange);
            fc.ShowDialog();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExchangeAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExchangeAddEdit("EDIT", RateID);
        }

        private void ExchangesGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ExchangesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ExchangesGridView.GetFocusedDataRow();
            if (row != null)
                RateID = row["ID"].ToString();
        }

        private void ExchangesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFExchangeAddEdit("EDIT", RateID);
        }

        private void DeleteExchange()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məzənnəni bazadan silmək istəyirsiniz?", "Məzənnənin bazadan silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery("DELETE CRS_USER.CURRENCY_RATES WHERE ID = " + RateID, "Məzənnə bazadan silinmədi.");
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteExchange();
            LoadExchangesDataGridView();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadExchangesDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ExchangesGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ExchangesGridControl, "xls");
        }

        private void ExchangesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ExchangesGridView, PopupMenu, e);
        }

        private void ExchangesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(ExchangesGridView, e);
        }

        private void Currency1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cur1 = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "CODE", "ID", Currency1ComboBox.Text);
            ResultText.Text = GlobalFunctions.CalculatedExchange((double)AmountValue.Value, cur1, DateValue.Text, cur2, DateValue.Text).ToString();
        }

        private void Currency2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cur2 = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "CODE", "ID", Currency2ComboBox.Text);
            ResultText.Text = GlobalFunctions.CalculatedExchange((double)AmountValue.Value, cur1, DateValue.Text, cur2, DateValue.Text).ToString();
        }

        private void AmountValue_EditValueChanged(object sender, EventArgs e)
        {
            ResultText.Text = GlobalFunctions.CalculatedExchange((double)AmountValue.Value, cur1, DateValue.Text, cur2, DateValue.Text).ToString();
        }

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadExchangesDataGridView();
            RatesGroupControl.Text = DateValue.Text + " tarixinə olan valyuta məzənnələri";
            AmountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void UpgradeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ProgressPanel.Show();
            Application.DoEvents();
            GlobalProcedures.LoadCurrencyRateFromCBAR(DateValue.Text);
            LoadExchangesDataGridView();
            ProgressPanel.Hide();
        }

        private void ExchangesGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteBarButton.Enabled = EditBarButton.Enabled = AmountValue.Enabled = (ExchangesGridView.RowCount > 0);
        }

        private void ChangePictureBox_Click(object sender, EventArgs e)
        {
            string cur1text = Currency1ComboBox.Text;
            Currency1ComboBox.EditValue = Currency2ComboBox.Text;
            Currency2ComboBox.EditValue = cur1text;
            cur1 = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "CODE", "ID", Currency1ComboBox.Text);
            cur2 = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "CODE", "ID", Currency2ComboBox.Text);
            ResultText.Text = GlobalFunctions.CalculatedExchange((double)AmountValue.Value, cur1, DateValue.Text, cur2, DateValue.Text).ToString();
        }

        private void RateChartBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Forms.Info.FRateAnalysis fra = new Info.FRateAnalysis();
            fra.ShowDialog();
        }
    }
}