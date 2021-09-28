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

namespace CRS.Forms.Info
{
    public partial class FExchanges : DevExpress.XtraEditors.XtraForm
    {
        public FExchanges()
        {
            InitializeComponent();
        }
        string RateID, currency_name = null;

        public delegate void DoEvent();
        public event DoEvent RefreshSlideLabel;

        private void FExchanges_Load(object sender, EventArgs e)
        {         
            FromDateValue.EditValue = DateTime.Today;
            ToDateValue.EditValue = DateTime.Today;
            GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", "1 = 1 ORDER BY ORDER_ID");
            SearchDockPanel.Hide();
            LoadExchangesDataGridView();
        }

        private void LoadExchangesDataGridView()
        {
            string s = null;
            try
            {
                s = "SELECT 1 SS,TO_CHAR(E.RATE_DATE,'DD/MM/YYYY'),C.VALUE||' '||C.CODE,E.AMOUNT,E.NOTE,E.ID,E.USED_USER_ID FROM CRS_USER.CURRENCY_RATES E,CRS_USER.CURRENCY C WHERE E.CURRENCY_ID = C.ID AND E.RATE_DATE BETWEEN TO_DATE('" + FromDateValue.Text + "','DD/MM/YYYY') AND TO_DATE('" + ToDateValue.Text + "','DD/MM/YYYY')" + currency_name + " ORDER BY E.RATE_DATE,C.ORDER_ID";

                ExchangesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadExchangesDataGridView");
                ExchangesGridView.PopulateColumns();
                ExchangesGridView.Columns[0].Caption = "S/s";
                ExchangesGridView.Columns[0].Visible = false;
                ExchangesGridView.Columns[1].Caption = "Tarix";
                ExchangesGridView.Columns[2].Caption = "Valyuta";
                ExchangesGridView.Columns[3].Caption = "Məzənnə (AZN ilə)";
                ExchangesGridView.Columns[4].Caption = "Qeyd";
                ExchangesGridView.Columns[5].Visible = false;
                ExchangesGridView.Columns[6].Visible = false;

                ExchangesGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ExchangesGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                ExchangesGridView.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ExchangesGridView.Columns[1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                ExchangesGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ExchangesGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;                            

                if (ExchangesGridView.RowCount > 0)
                {
                    DeleteBarButton.Enabled = true;
                    EditBarButton.Enabled = true;
                    ExchangesGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    DeleteBarButton.Enabled = false;
                    EditBarButton.Enabled = false;
                }

                ExchangesGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məzənnələr yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExchangesGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
        
        private void ExchangesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(ExchangesGridView, e);
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

        void RefreshExchange()
        {
            LoadExchangesDataGridView();
        }

        private void LoadFExchangeAddEdit(string transaction, string id)
        {
            FExchangeAddEdit fc = new FExchangeAddEdit();
            fc.TransactionName = transaction;
            fc.RateID = id;
            fc.RefreshExchangesDataGridView += new FExchangeAddEdit.DoEvent(RefreshExchange);
            fc.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExchangeAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExchangeAddEdit("EDIT", RateID);
        }

        private void ExchangesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ExchangesGridView.GetFocusedDataRow();
            if (row != null)
                RateID = row["ID"].ToString();            
        }

        private void ExchangesGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFExchangeAddEdit("EDIT", RateID);
        }

        private void DeleteExchange()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məzənnəni bazadan silmək istəyirsiniz?", "Məzənnənin bazadan silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)            
                GlobalProcedures.ExecuteQuery($@"DELETE CRS_USER.CURRENCY_RATES WHERE ID = {RateID}", "Məzənnə bazadan silinmədi.", this.Name + "/DeleteExchange");            
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteExchange();
            LoadExchangesDataGridView();
        }

        private void FExchanges_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshSlideLabel();
        }

        private void UpgradeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.LoadCurrencyRateFromCBAR(FromDateValue.Text);
            LoadExchangesDataGridView();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                currency_name = " AND C.CODE IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            else
                currency_name = null;
            LoadExchangesDataGridView();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 7: GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", "1 = 1 ORDER BY ORDER_ID");           
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index)
        {
            Forms.FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new Forms.FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void CurrencyComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7);
        }

        private void RateChartBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FRateAnalysis fra = new FRateAnalysis();
            fra.ShowDialog();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadExchangesDataGridView();
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadExchangesDataGridView();
        }
    }
}