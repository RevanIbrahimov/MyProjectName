using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using CRS.Forms.Bookkeeping;

namespace CRS.Forms.Total
{
    public partial class FContractGroups : DevExpress.XtraEditors.XtraForm
    {
        public FContractGroups()
        {
            InitializeComponent();
        }
        decimal calc_debt,
            calc_debt_with_rate,
            calc_amount,
            calc_amount_with_rate,
            rateAmount = 1;

        int groupID, old_row_num, topindex;

        string groupName,
                currency_code,
                credit_name,
                status_name,
                contractID;

        List<CurrencyRates> lstRate = null;
        List<Currency> lstCurrency = null;

        private void FContractGroups_Load(object sender, EventArgs e)
        {
            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();
            lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
            SearchDockPanel.Hide();
            LoadTotalsDataGridView();
        }

        private void LoadTotalsDataGridView()
        {
            string s = $@"SELECT CG.NAME GROUP_NAME,
                                 CON.CONTRACT_CODE,
                                 CUS.FULLNAME FULL_CUSTOMER_NAME,
                                 COM.COMMITMENT_NAME,
                                 H.HOSTAGE,
                                 CON.START_DATE,
                                 CON.END_DATE,
                                 CON.AMOUNT,
                                 CON.CURRENCY_CODE,
                                 LT.DEBT,
                                 LT.DAY_COUNT,
                                 NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, CON.MONTHLY_AMOUNT)
                                        MONTHLY_AMOUNT,
                                 LT.REQUIRED,
                                 LT.DELAYS,
                                 LT.NORM_DEBT,
                                 CON.INTEREST,
                                 CON.PERIOD,
                                 CON.CURRENCY_ORDER,
                                 CG.USED_USER_ID,
                                 LT.CONTRACT_ID,
                                 CON.STATUS_ID,
                                 CON.CREDIT_TYPE_ID,
                                 CG.ID GROUP_ID,
                                 S.STATUS_NAME,
                                 CON.CREDIT_NAME,
                                 CE.MONTHLY_AMOUNT EXTEND_MONTHLY_AMOUNT,
                                 (CASE WHEN CON.END_DATE < TRUNC(SYSDATE) THEN 1 ELSE 0 END) IS_OLD
                            FROM CRS_USER.LEASING_TOTAL LT,
                                 CRS_USER.CONTRACT_GROUP CG,
                                 CRS_USER.CONTRACT_GROUP_DETAILS CGD,
                                 CRS_USER.V_CUSTOMERS CUS,
                                 CRS_USER.V_HOSTAGE H,
                                 CRS_USER.V_COMMITMENTS COM,
                                 CRS_USER.V_CONTRACTS CON,
                                 CRS_USER.STATUS S,
                                 CRS_USER.CONTRACT_EXTEND CE
                           WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                 AND LT.CONTRACT_ID = CGD.CONTRACT_ID
                                 AND CG.ID = CGD.GROUP_ID
                                 AND CON.CUSTOMER_ID = CUS.ID
                                 AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                 AND CON.CONTRACT_ID = H.CONTRACT_ID(+)
                                 AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                 AND CON.CONTRACT_ID = CE.CONTRACT_ID(+)
                                 AND CON.IS_COMMIT = 1
                                 AND CON.STATUS_ID = S.ID
                        ORDER BY CON.CONTRACT_CODE DESC";
            TotalsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTotalsDataGridView", "Qruplar üzrə portfel yüklənmədi.");

            EditBarButton.Enabled = DeleteBarButton.Enabled = ShowPaymentBarButton.Enabled = (TotalsGridView.RowCount > 0);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void RefreshContract()
        {
            LoadTotalsDataGridView();
        }

        private void LoadFContractGroupAddEdit(string transaction, int groupID)
        {
            old_row_num = TotalsGridView.FocusedRowHandle;
            topindex = TotalsGridView.TopRowIndex;
            FContractGroupAddEdit fc = new FContractGroupAddEdit();
            fc.TransactionName = transaction;
            fc.GroupID = groupID;
            fc.RefreshDataGridView += new FContractGroupAddEdit.DoEvent(RefreshContract);
            fc.ShowDialog();
            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void TotalsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(TotalsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "xls");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CurBarStatic.Caption = CurrencyRatesDAL.LastRateString();
            LoadTotalsDataGridView();
        }

        private void TotalsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TotalsGridView, PopupMenu, e);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractGroupAddEdit("INSERT", 0);
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
                GlobalProcedures.FillCheckedComboBox(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (5,6) ORDER BY ID", null);
                GlobalProcedures.FillCheckedComboBox(CreditNameComboBox, "CREDIT_NAMES", "NAME,NAME_EN,NAME_RU", null);
                SearchDockPanel.Show();
            }
            else
                SearchDockPanel.Hide();
        }

        private void FilterContracts()
        {
            ColumnView view = TotalsGridView;

            //Currency
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                    new ColumnFilterInfo(currency_code, ""));
            else
                view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);

            //Status
            if (!String.IsNullOrEmpty(StatusComboBox.Text))
                view.ActiveFilter.Add(view.Columns["STATUS_NAME"],
                    new ColumnFilterInfo(status_name, ""));
            else
                view.ActiveFilter.Remove(view.Columns["STATUS_NAME"]);

            //CreditName
            if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
                view.ActiveFilter.Add(view.Columns["CREDIT_NAME"],
                    new ColumnFilterInfo(credit_name, ""));
            else
                view.ActiveFilter.Remove(view.Columns["CREDIT_NAME"]);
        }

        private void TotalsGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_CODE", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);

            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_code = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void StatusComboBox_EditValueChanged(object sender, EventArgs e)
        {
            status_name = " [STATUS_NAME] IN ('" + StatusComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void ShowPaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contractID;
            fsp.ShowDialog();
        }

        private void CreditNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            credit_name = " [CREDIT_NAME] IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void TotalsGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "FULL_CUSTOMER_NAME")
            {
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.FieldName == "DEBT")
                e.Appearance.ForeColor = Color.Red;

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                e.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractGroupAddEdit("EDIT", groupID);
        }

        private void TotalsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFContractGroupAddEdit("EDIT", groupID);
        }

        private void TotalsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TotalsGridView.GetFocusedDataRow();
            if (row != null)
            {
                groupID = Convert.ToInt32(row["GROUP_ID"]);
                groupName = row["GROUP_NAME"].ToString();
                contractID = row["CONTRACT_ID"].ToString();
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş " + groupName + " adlı qrupu silmək istəyirsiniz?", "Qrupun silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_CONTRACT_GROUP", "P_GROUP_ID", groupID, "Müqavilələrin qrupu silinmədi");
            }
            LoadTotalsDataGridView();
        }

        private void TotalsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            decimal monthly_amount = 0, delya_value = 0, res = 0;

            GlobalProcedures.GridRowCellStyleForBlock(TotalsGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(6, TotalsGridView, e);

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }

            if (e.Column.FieldName == "DELAYS")
            {
                monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
                delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DELAYS"));
                if (monthly_amount != 0)
                    res = Math.Round((delya_value / monthly_amount) * 100, 2);
                GlobalProcedures.FindFontDetails(res, e);
            }

            if (e.Column.FieldName == "START_DATE" || e.Column.FieldName == "END_DATE")
            {
                int isOld = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "IS_OLD"));
                if (isOld == 1)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.OrangeRed;
            }
        }

        private void TotalsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                calc_debt =
                    calc_debt_with_rate =
                    calc_amount =
                    calc_amount_with_rate = 0;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_amount_with_rate += Convert.ToDecimal(e.FieldValue) * rateAmount;
                    calc_amount += Convert.ToDecimal(e.FieldValue);
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_debt_with_rate += Convert.ToDecimal(e.FieldValue) * rateAmount;
                    calc_debt += Convert.ToDecimal(e.FieldValue);
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //verilen   
                {
                    if (e.GroupLevel == 1)
                        e.TotalValue = calc_amount;
                    if (e.GroupLevel == 0 || e.IsTotalSummary)
                        e.TotalValue = calc_amount_with_rate;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaliq
                {
                    if (e.GroupLevel == 1)
                        e.TotalValue = calc_debt;
                    if (e.GroupLevel == 0 || e.IsTotalSummary)
                        e.TotalValue = calc_debt_with_rate;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("FULL_CUSTOMER_NAME") == 0 && TotalsGridView.RowCount > 0) //cemi
                {
                    if (e.GroupLevel == 0)
                        e.TotalValue = e.GetValue("GROUP_NAME").ToString() + " qrupu üzrə AZN ilə cəmi";
                    if (e.GroupLevel == 1)
                        e.TotalValue = e.GetValue("CURRENCY_CODE").ToString() + " üzrə cəmi";
                    if (e.IsTotalSummary)
                        e.TotalValue = "YEKUN (AZN - ilə)";
                }
            }
        }
    }
}