using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;

namespace CRS.Forms.Total
{
    public partial class FPenalty : DevExpress.XtraEditors.XtraForm
    {
        public FPenalty()
        {
            InitializeComponent();
        }
        public string ContractID, CustomerID, ContractCode, Currency;

        string penaltyid, penaltystatus;
        int commit, PaymentUsedUserID;
        double debt = 0, penalty_amount, discount_penalty, payment_penalty, last_debt, PenaltySum = 0;
        bool CurrentStatus = false, PaymentClosed = false, PaymentUsed = false;

        public delegate void DoEvent();
        public event DoEvent RefreshPenaltyValue;

        private void FPenalty_Load(object sender, EventArgs e)
        {
            Class.GlobalProcedures.CalcEditFormat(PenaltyValue);
            Class.GlobalProcedures.CalcEditFormat(CalcPenaltyValue);
            Currency1Label.Text = Currency;
            Currency2Label.Text = Currency;
            this.Text = ContractCode + " saylı lizinq müqaviləsi üzrə cərimə faizləri";

            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT STATUS_ID, USED_USER_ID FROM CRS_USER.CONTRACTS WHERE CUSTOMER_ID = {CustomerID} AND ID = {ContractID}");

            if(dt.Rows.Count > 0)
            {
                PaymentClosed  = Convert.ToInt16(dt.Rows[0]["STATUS_ID"]) == 6;
                PaymentUsedUserID = Convert.ToInt16(dt.Rows[0]["USED_USER_ID"]);
                PaymentUsed = PaymentUsedUserID >= 0;
            }

            if ((PaymentClosed && PaymentUsed) || (PaymentClosed && !PaymentUsed))
                CurrentStatus = true;            
            else if (PaymentUsed && !PaymentClosed)
            {
                if (Class.GlobalVariables.V_UserID != PaymentUsedUserID)
                    CurrentStatus = true;                
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            
            ComponentEnable(CurrentStatus);
            LoadPenaltyDataGridView();
            LoadBalanceDataGridView();
        }

        public void ComponentEnable(bool status)
        {
            CalcStandaloneBarDockControl.Enabled = !status;
            BalanceStandaloneBarDockControl.Enabled = !status;
            BOK.Visible = !status;
            if (status == false)
            {
                CalcPopupMenu.Manager = CalcBarManager;
                BalancePopupMenu.Manager = BalanceBarManager;
            }
            else
            {
                CalcPopupMenu.Manager = null;
                BalancePopupMenu.Manager = null;
            }
        }

        private void LoadPenaltyDataGridView()
        {
            string s = $@"SELECT CP.ID,
                               CP.LAST_PAYMENT_DATE,
                               CP.START_DATE,
                               CP.END_DATE,
                               CP.DAY_COUNT,
                               CP.DEBT,
                               IP.INTEREST,
                               CP.PENALTY_AMOUNT
                        FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES CP,
                             CRS_USER.CONTRACT_INTEREST_PENALTIES IP
                        WHERE CP.IS_BALANCE = 0 AND CP.INTEREST_PENALTIES_ID = IP.ID AND CP.CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPenaltyDataGridView", "Hesablanmış cərimələr cədvələ yüklənmədi.");
            PenaltyGridControl.DataSource = dt;
            PenaltyGridView.BestFitColumns();

            //try
            //{



            //    PenaltyGridView.PopulateColumns();
            //    PenaltyGridView.Columns[0].Caption = "S/s";
            //    PenaltyGridView.Columns[0].Visible = false;
            //    PenaltyGridView.Columns[1].Visible = false;
            //    PenaltyGridView.Columns[2].Caption = "Son ödənişin tarixi";
            //    PenaltyGridView.Columns[3].Caption = "Cərimənin başlama tarixi";
            //    PenaltyGridView.Columns[4].Caption = "Cərimənin bitmə tarixi";
            //    PenaltyGridView.Columns[5].Caption = "Günlərin sayı";
            //    PenaltyGridView.Columns[6].Caption = "Qalıq";
            //    PenaltyGridView.Columns[7].Caption = "Faiz";
            //    PenaltyGridView.Columns[8].Caption = "Cərimə";

            //    for (int i = 0; i < PenaltyGridView.Columns.Count; i++)
            //    { 
            //        PenaltyGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //        PenaltyGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
            //    }

            //    PenaltyGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //    PenaltyGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            //    PenaltyGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //    PenaltyGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            //    PenaltyGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //    PenaltyGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            //    PenaltyGridView.Columns[5].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //    PenaltyGridView.Columns[5].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            //    PenaltyGridView.Columns[7].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            //    PenaltyGridView.Columns[7].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

            //    PenaltyGridView.Columns[6].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
            //    PenaltyGridView.Columns[6].DisplayFormat.FormatType = FormatType.Custom;
            //    PenaltyGridView.Columns[8].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
            //    PenaltyGridView.Columns[8].DisplayFormat.FormatType = FormatType.Custom;

            //    if (PenaltyGridView.RowCount > 0)
            //    {
            //        EditPenaltyBarButton.Enabled = true;
            //        PenaltyGridView.Columns[5].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "");
            //        PenaltyGridView.Columns[8].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:### ### ### ### ### ### ##0.00}");
            //    }
            //    else
            //        EditPenaltyBarButton.Enabled = false;

            //    PenaltyGridView.BestFitColumns();

            //    try
            //    {
            //        PenaltyGridView.BeginUpdate();
            //        for (int i = 0; i < PenaltyGridView.RowCount; i++)
            //        {
            //            PenaltyGridView.SelectRow(i);
            //        }
            //    }
            //    finally
            //    {
            //        PenaltyGridView.EndUpdate();
            //    }
            //}
            //catch (Exception exx)
            //{
            //    Class.GlobalProcedures.LogWrite("Hesablanmış cərimələr cədvələ yüklənmədi.", s, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            //}
        }        

        private void RefreshPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPenaltyDataGridView();
        }

        private void PenaltyGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            PenaltySum = 0;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PenaltyGridView.SelectedRowsCount; i++)
            {
                rows.Add(PenaltyGridView.GetDataRow(PenaltyGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                PenaltySum = PenaltySum + Convert.ToDouble(row["PENALTY_AMOUNT"].ToString());
            }
            CalcPenaltyValue.Value = (decimal)PenaltySum;
            PenaltyValue.Value = (decimal)(PenaltySum + last_debt);
            PenaltyGridView.UpdateTotalSummary();
        }

        private void PenaltyGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            Class.GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            Class.GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
        }

        private void LoadBalanceDataGridView()
        {
            string s = "SELECT 1 SS,ID,TO_CHAR(BAL_DATE,'DD.MM.YYYY'),PENALTY_AMOUNT,DISCOUNT_PENALTY,PAYMENT_PENALTY,DEBT_PENALTY,PENALTY_STATUS,IS_COMMIT FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE IN (0,1) AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " ORDER BY ID,BAL_DATE";
            try
            {
                DataTable dt = Class.GlobalFunctions.GenerateDataTable(s);

                BalanceGridControl.DataSource = dt;
                BalanceGridView.PopulateColumns();
                BalanceGridView.Columns[0].Caption = "S/s";
                BalanceGridView.Columns[1].Visible = false;
                BalanceGridView.Columns[2].Caption = "Tarix";
                BalanceGridView.Columns[3].Caption = "Cərimə";
                BalanceGridView.Columns[4].Caption = "Güzəşt olunmuş";
                BalanceGridView.Columns[5].Caption = "Ödənilən";
                BalanceGridView.Columns[6].Caption = "Qalıq";
                BalanceGridView.Columns[7].Caption = "Statusu";
                BalanceGridView.Columns[8].Visible = false;                

                for (int i = 0; i < BalanceGridView.Columns.Count; i++)
                {
                    BalanceGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    BalanceGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }

                BalanceGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                BalanceGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                BalanceGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                BalanceGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                BalanceGridView.Columns[7].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                BalanceGridView.Columns[7].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                BalanceGridView.Columns[3].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                BalanceGridView.Columns[3].DisplayFormat.FormatType = FormatType.Custom;
                BalanceGridView.Columns[4].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                BalanceGridView.Columns[4].DisplayFormat.FormatType = FormatType.Custom;
                BalanceGridView.Columns[5].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                BalanceGridView.Columns[5].DisplayFormat.FormatType = FormatType.Custom;
                BalanceGridView.Columns[6].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                BalanceGridView.Columns[6].DisplayFormat.FormatType = FormatType.Custom;

                if (BalanceGridView.RowCount > 0)
                {
                    EditBalanceBarButton.Enabled = true;
                    DeleteBalanceBarButton.Enabled = true;
                    DeletedBalanceBarButton.Enabled = true;
                    BalanceGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    BalanceGridView.Columns[3].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:### ### ### ### ### ### ##0.00}");
                    BalanceGridView.Columns[4].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:### ### ### ### ### ### ##0.00}");
                    BalanceGridView.Columns[5].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:### ### ### ### ### ### ##0.00}");
                    BalanceGridView.Columns[6].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "{0:### ### ### ### ### ### ##0.00}");
                }
                else
                {
                    EditBalanceBarButton.Enabled = false;
                    DeleteBalanceBarButton.Enabled = false;
                    DeletedBalanceBarButton.Enabled = false;
                }

                BalanceGridView.BestFitColumns();                
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite("Balansa daxil edilmiş cərimələr cədvələ yüklənmədi.", s, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void RefreshBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBalanceDataGridView();
        }

        private void BalanceGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            Class.GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void BalanceGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            Class.GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            Class.GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
            Class.GlobalProcedures.GridCustomDrawFooterCell("DISCOUNT_PENALTY", "Far", e);
            Class.GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_PENALTY", "Far", e);
            Class.GlobalProcedures.GridCustomDrawFooterCell("DEBT_PENALTY", "Far", e);
            if (e.Column.FieldName == "DEBT_PENALTY")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        void RefreshBalance(bool a)
        {
            PenaltyGridControl.Enabled = !a;
            LoadBalanceDataGridView();             
        }

        private void LoadFBalancePenaltyAddEdit(string transaction, string customerid, string contractid, string penaltyid, decimal debt, decimal calcpenalty, string currency)
        {                
            Forms.Total.FBalancePenaltyAddEdit fb = new FBalancePenaltyAddEdit();
            fb.TransactionName = transaction;
            fb.CustomerID = customerid;
            fb.ContractID = contractid;
            fb.PenaltyID = penaltyid;
            fb.Debt = debt;
            fb.CalculatedPenalty = calcpenalty;
            fb.Currency = currency;
            fb.RefreshPenaltyDataGridView += new Forms.Total.FBalancePenaltyAddEdit.DoEvent(RefreshBalance);
            fb.ShowDialog();              
        }

        private void NewBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CalcPenaltyValue.Value > 0)
            {                
                LoadFBalancePenaltyAddEdit("INSERT", CustomerID, ContractID, null, (decimal)last_debt, CalcPenaltyValue.Value, Currency);
            }
            else
            {
                CalcPenaltyValue.BackColor = Color.Red;
                XtraMessageBox.Show("Müştəridən tutulacaq cərimə faizi sıfırdan böyük olmalıdır. Əgər müştəri üçün avtomatik cərimə faizi hesablanıbsa, yuxarıdakı cədvəldən ən azı bir cəriməni seçin.");
                CalcPenaltyValue.BackColor = Class.GlobalFunctions.ElementColor();                
            }
        }

        private void FPenalty_FormClosing(object sender, FormClosingEventArgs e)
        {
            Class.GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_COMMIT = 0 AND IS_CHANGE <> 0 AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + " AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID, "Cərimələr temp cədvəldən silinmədi.");
            this.RefreshPenaltyValue();
        }
                
        private void UpdatePenaltyTemp()
        {
            Class.GlobalProcedures.ExecuteTwoQuery("UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET IS_COMMIT = 1 WHERE IS_COMMIT = 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_BALANCE = 1, IS_COMMIT = 0 WHERE IS_BALANCE = 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND EXISTS (SELECT ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE = 1 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                "Cərimələr temp cədvələ təsdiq edilmədi.");
        }

        private void UpdatePenaltySelected()
        {
            Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_CALCULATED = 0 WHERE IS_BALANCE = 0 AND IS_CALCULATED = 1 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID, "Cərimənin balansa daxil edilib edilməməsi dəyişdirilmədi.");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PenaltyGridView.SelectedRowsCount; i++)
            {
                rows.Add(PenaltyGridView.GetDataRow(PenaltyGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_CALCULATED = 1 WHERE ID = " + row["ID"].ToString(), "Cərimənin balansa daxil edilib edilməməsi üçün olan seçimlər yadda saxlanmadı.");
            }            
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE <> 0 AND IS_COMMIT = 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID) > 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Balansda edilmiş dəyişikliklər yadda saxlanıldıqdan sonra onları dəyişmək mümkün olmayacaq. Buna razısınız?", "Dəyişikliklərin yadda saxlanılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    UpdatePenaltySelected();
                    UpdatePenaltyTemp();               
                    this.Close();
                }
            } 
            else
                this.Close();
        }

        private void PrintBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.ShowGridPreview(BalanceGridControl);
        }

        private void ExportBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.GridExportToFile(BalanceGridControl, "xls");
        }

        private void PrintPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.ShowGridPreview(PenaltyGridControl);
        }

        private void ExportPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.GridExportToFile(PenaltyGridControl, "xls");
        }

        private void PenaltyGridView_MouseUp(object sender, MouseEventArgs e)
        {
            Class.GlobalProcedures.GridMouseUpForPopupMenu(PenaltyGridView, CalcPopupMenu, e);
        }

        private void BalanceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BalanceGridView, BalancePopupMenu, e);
        }

        private void EditBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (commit == 0 && penaltystatus == "Yeni" && BalanceStandaloneBarDockControl.Enabled)                            
                LoadFBalancePenaltyAddEdit("EDIT", CustomerID, ContractID, penaltyid, (decimal)last_debt, 0, Currency);
            else if (commit == 0 && penaltystatus == "Silinib" && BalanceStandaloneBarDockControl.Enabled)
                LoadFDeletePenalty("EDIT", CustomerID, ContractID, penaltyid, Currency, (decimal)last_debt);
            else if (BalanceStandaloneBarDockControl.Enabled)
                XtraMessageBox.Show("Seçdiyiniz tarixə olan cərimə balansa daxil edildiyi üçün dəyişdirilə bilməz.");
        }

        private void NewPenaltyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPenaltyAddEdit("INSERT", null);
        }

        private void LoadFPenaltyAddEdit(string transaction, int? id)
        {
            FPenaltyAddEdit fp = new FPenaltyAddEdit();
            fp.TransactionName = transaction;
            fp.ContractID = ContractID;
            fp.ID = id;
            fp.RefreshDataGridView += new FPenaltyAddEdit.DoEvent(LoadPenaltyDataGridView);
            fp.ShowDialog();
        }

        private void BalanceGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BalanceGridView.GetFocusedDataRow();
            if (row != null)
            {
                penaltyid = row["ID"].ToString();
                penaltystatus = row["PENALTY_STATUS"].ToString();
                commit = Convert.ToInt32(row["IS_COMMIT"].ToString());
            }
        }

        private void DeleteBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (commit == 0 && (penaltystatus == "Yeni" || penaltystatus == "Silinib") && BalanceStandaloneBarDockControl.Enabled)   
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş cərimə faizini balansdan silmək istəyirsiniz?", "Cərimənin balansdan silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Class.GlobalProcedures.ExecuteTwoQuery("UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET IS_CHANGE = 2 WHERE ID = " + penaltyid + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                           "UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_BALANCE = 0 WHERE IS_BALANCE = 1 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND EXISTS (SELECT ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE = 2 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                    "Cərimə silinmədi.");
                    LoadBalanceDataGridView();
                    PenaltyGridControl.Enabled = true;
                }
            }
            else
                XtraMessageBox.Show("Seçdiyiniz tarixə olan cərimə balansa daxil edildiyi üçün bu cəriməni silmək olmaz.");
        }

        private void BalanceGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBalanceBarButton.Enabled && BalanceStandaloneBarDockControl.Enabled)
            {
                if (commit == 0 && (penaltystatus == "Yeni" || penaltystatus == "Silinib"))            
                    LoadFBalancePenaltyAddEdit("EDIT", CustomerID, ContractID, penaltyid, (decimal)last_debt, 0, Currency);                
                else
                    XtraMessageBox.Show("Seçdiyiniz tarixə olan cərimə balansa daxil edildiyi üçün dəyişdirilə bilməz.");
            }
        }

        private void BalanceGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT_PENALTY") == 0) //qaligi hesabliyir
                {
                    penalty_amount = Convert.ToDouble(BalanceGridView.Columns.ColumnByFieldName("PENALTY_AMOUNT").SummaryItem.SummaryValue);
                    discount_penalty = Convert.ToDouble(BalanceGridView.Columns.ColumnByFieldName("DISCOUNT_PENALTY").SummaryItem.SummaryValue);
                    payment_penalty = Convert.ToDouble(BalanceGridView.Columns.ColumnByFieldName("PAYMENT_PENALTY").SummaryItem.SummaryValue);
                    last_debt = penalty_amount - (discount_penalty + payment_penalty);
                    if(PenaltyGridControl.Enabled)
                        PenaltyValue.Value = (decimal)(PenaltySum + last_debt);
                    else
                        PenaltyValue.Value = (decimal)(last_debt);
                    e.TotalValue = last_debt;
                }                
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            Class.GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            Class.GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", Class.GlobalVariables.V_CultureInfoAZ));
        }
        
        void RefreshBalanceForDelete()
        {
            LoadBalanceDataGridView();
        }

        private void LoadFDeletePenalty(string transaction, string customerid, string contractid, string penaltyid, string currency, decimal debt)
        {
            Forms.Total.FDeletePenalty fd = new FDeletePenalty();
            fd.TransactionName = transaction;
            fd.CustomerID = customerid;
            fd.ContractID = contractid;
            fd.PenaltyID = penaltyid;
            fd.Currency = currency;
            fd.Debt = debt;
            fd.RefreshPenaltyDataGridView += new Forms.Total.FDeletePenalty.DoEvent(RefreshBalanceForDelete);
            fd.ShowDialog();
        }

        private void DeletedBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFDeletePenalty("INSERT", CustomerID, ContractID, null, Currency, (decimal)last_debt);
        }

        private void BalanceGridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string payment_status = View.GetRowCellDisplayText(e.RowHandle, View.Columns["PENALTY_STATUS"]);
                if (payment_status == "Silinib")
                {
                    e.Appearance.BackColor = Color.FromArgb(0xFF, 0xFF, 0x66);                    
                }
            }
        }

        private void BalanceGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "DEBT_PENALTY")
            {
                e.Column.AppearanceHeader.FontStyleDelta = FontStyle.Bold;
                e.Column.AppearanceHeader.ForeColor = Color.Red;
                e.Column.AppearanceCell.FontStyleDelta = FontStyle.Bold;                
            }
        }

        private void ShowBalanceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Forms.Total.FBalancePenaltyDetail fbd = new FBalancePenaltyDetail();
            fbd.CustomerID = CustomerID;
            fbd.ContractID = ContractID;
            fbd.Currency = Currency;
            fbd.ShowDialog();
        }
    }
}