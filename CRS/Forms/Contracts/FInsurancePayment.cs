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
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.Contracts
{
    public partial class FInsurancePayment : DevExpress.XtraEditors.XtraForm
    {
        public FInsurancePayment()
        {
            InitializeComponent();
        }
        public int InsuranceID, TypeID, FromCustomerPayment = 1;        

        decimal calc_amount = 0;
        int payedID, topindex, old_row_num, UsedUserID = -1;
        bool CurrentStatus = false;
        DataTable dtPayed = new DataTable();

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PayedGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PayedGridControl, "xls");
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsurancePaymentAddEdit("INSERT", null);
        }

        private void LoadFInsurancePaymentAddEdit(string transaction, int? id)
        {
            DataView dv = new DataView();

            dv = new DataView(dtPayed);
            if (id != null)
                dv.RowFilter = $@"ID <> {id}";

            object objCompute = dtPayed.Compute("SUM(PAYED_AMOUNT)", dv.RowFilter);
            decimal debt = 0;

            if (objCompute != DBNull.Value)
                debt = Convert.ToDecimal(objCompute);

            decimal maxCompensation = Math.Round(InsuranceCostValue.Value - debt - TransferAmountValue.Value, 2);
            maxCompensation = maxCompensation == 0 ? debt : maxCompensation;
            topindex = PayedGridView.TopRowIndex;
            old_row_num = PayedGridView.FocusedRowHandle;
            FInsurancePaymentAddEdit fra = new FInsurancePaymentAddEdit();
            fra.TransactionName = transaction;
            fra.ID = id;
            fra.InsuranceID = InsuranceID;
            fra.StartDate = StartDateText.Text;
            fra.TypeID = TypeID;
            fra.InsuranceAmount = InsuranceCostValue.Value;
            fra.CurrentDebt = TypeID == 1 ? Math.Round(InsuranceCostValue.Value - debt, 2) : maxCompensation;
            fra.PayedCount = PayedGridView.RowCount;
            fra.RefreshDataGridView += new FInsurancePaymentAddEdit.DoEvent(LoadPayed);
            fra.ShowDialog();
            PayedGridView.TopRowIndex = topindex;
            PayedGridView.FocusedRowHandle = old_row_num;
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsurancePaymentAddEdit("EDIT", payedID);
        }

        private void PayedGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFInsurancePaymentAddEdit("EDIT", payedID);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PayedGridView.TopRowIndex;
            old_row_num = PayedGridView.FocusedRowHandle;
            if (TypeID == 1)
            {
                int customerPaymentID = Convert.ToInt32(GlobalFunctions.GridGetRowCellValue(PayedGridView, "CUSTOMER_PAYMENT_ID"));

                if (customerPaymentID > 0)
                {
                    GlobalProcedures.ShowWarningMessage("Bu ödəniş lizinq ödənişindən avtomatik ödənildiyindən silinə bilməz.");
                    return;
                }

                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş ödənişi silmək istəyirsiniz?", "Ödənişin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP SET IS_CHANGE = 2 WHERE ID = {payedID}",
                                                     "Ödəniş silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
                }
            }
            else
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş əvəzləşməni silmək istəyirsiniz?", "Əvəzləşmənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCE_TRANSFER_TEMP SET IS_CHANGE = 2 WHERE ID = {payedID}",
                                                     "Əvəzləşmə silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
                }
            }
            LoadPayed();
            PayedGridView.TopRowIndex = topindex;
            PayedGridView.FocusedRowHandle = old_row_num;
        }

        private void FInsurancePayment_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TypeID == 1)            
                GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_DELETE_INSURANCE_PAY_TEMP", "P_INSURANCE_ID", InsuranceID, "P_TYPE", FromCustomerPayment, "Ödənişlər temp cədvəldən silinmədi.");            
            else
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_DELETE_INSURANCE_TRN_TEMP", "P_INSURANCE_ID", InsuranceID, "Əvəzləşmələr temp cədvəldən silinmədi.");
            this.RefreshDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (calc_amount > InsuranceCostValue.Value)
            {
                GlobalProcedures.ShowErrorMessage((TypeID == 1 ? "Ödənişlərin" : "Əvəzləşmələrin") + " cəmi " + InsuranceCostValue.Value.ToString("n2") + " AZN olmalıdır");
                return;
            }

            if (TypeID == 1)
                GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_INSERT_INSURANCE_PAYMENT", "P_INSURANCE_ID", InsuranceID, "P_TYPE", 0, "Ödənişlər əsas cədvələ daxil edilmədi.");
            else
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_INSURANCE_TRANSFER", "P_INSURANCE_ID", InsuranceID, "Əvəzləşmələr əsas cədvələ daxil edilmədi.");
            this.Close();
        }

        private void PayedGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PayedGridView, PopupMenu, e);
        }

        private void PayedGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
            {
                calc_amount = 0;
                return;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Start)
                calc_amount = 0;

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYED_AMOUNT") == 0)
                    calc_amount += Convert.ToDecimal(e.FieldValue);
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYED_AMOUNT") == 0)
                    e.TotalValue = calc_amount;
            }
        }

        private void PayedGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Payed_SS, e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPayed();
        }

        private void PayedGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PayedGridView.GetFocusedDataRow();
            if (row != null)
                payedID = Convert.ToInt32(row["ID"]);
        }

        private void FInsurancePayment_Load(object sender, EventArgs e)
        {
            this.Text = TypeID == 1 ? "Sığortanın ödənişləri" : "Sığorta üçün əvəzləşmələr";
            PayGroupControl.Text = TypeID == 1 ? "Ödənişlər" : "Əvəzləşmələr";
            DebtLabel.Text = TypeID == 1 ? "Borc" : "Hesabda";
            LoadDetails();
            InsertTemp();
            LoadPayed();

            if (GlobalVariables.V_UserID != UsedUserID && UsedUserID != -1)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Buna görə də sığorta ödənişləri dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş sığortanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CurrentStatus = true;
            }
            else
                CurrentStatus = false;

            ComponentEnable(CurrentStatus);
        }

        private void ComponentEnable(bool status)
        {
            BOK.Visible = !status;
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT C.CONTRACT_CODE,
                                     I.INSURANCE_AMOUNT,
                                     I.INSURANCE_PERIOD,
                                     I.INSURANCE_INTEREST,
                                     ROUND (I.INSURANCE_AMOUNT * I.INSURANCE_INTEREST / 100, 2) COST,
                                     I.AMOUNT,
                                     TO_CHAR (I.START_DATE, 'DD.MM.YYYY') START_DATE,
                                     TO_CHAR (I.END_DATE, 'DD.MM.YYYY') END_DATE,
                                     NVL (ST.TRANSFER_AMOUNT, 0) TRANSFER_AMOUNT,
                                     NVL (ST.COMPENSATION, 0) COMPENSATION,
                                     I.POLICE,
                                     C.USED_USER_ID
                                FROM CRS_USER.INSURANCES I,
                                     CRS_USER.V_CONTRACTS C,
                                     CRS_USER.V_SUM_INSURANCE_TRANSFER ST
                               WHERE I.CONTRACT_ID = C.CONTRACT_ID AND I.ID = ST.INSURANCE_ID(+)
                                 AND I.ID = {InsuranceID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Sığortanın detalları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                ContractCodeText.Text = dt.Rows[0]["CONTRACT_CODE"].ToString();
                InsuranceAmountValue.EditValue = Convert.ToDecimal(dt.Rows[0]["INSURANCE_AMOUNT"]);
                InsuranceCostValue.EditValue = Convert.ToDecimal(dt.Rows[0]["AMOUNT"]);
                StartDateText.Text = dt.Rows[0]["START_DATE"].ToString();
                EndDateText.Text = dt.Rows[0]["END_DATE"].ToString();
                InsurancePeriodValue.EditValue = Convert.ToInt16(dt.Rows[0]["INSURANCE_PERIOD"]);
                TransferAmountValue.EditValue = dt.Rows[0]["TRANSFER_AMOUNT"];
                PoliceText.EditValue = dt.Rows[0]["POLICE"];
                UsedUserID = Convert.ToInt16(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void PayedGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Payed_SS, "Center", e);
        }

        private void InsertTemp()
        {
            if (TypeID == 1)
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_INSURANCE_PAY_TEMP", "P_INSURANCE_ID", InsuranceID, "Ödənişlər temp cədvələ daxil edilmədi.");
            else
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_INSURANCE_TRN_TEMP", "P_INSURANCE_ID", InsuranceID, "Əvəzləşmələr temp cədvələ daxil edilmədi.");
        }

        private void LoadPayed()
        {
            string sql = null;
            if (TypeID == 1)
                sql = $@"SELECT ID,
                                   PAY_DATE,
                                   PAYED_AMOUNT,                                   
                                   NOTE,
                                   IS_LEGAL,
                                   CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {InsuranceID}
                            ORDER BY ID";
            else
                sql = $@"SELECT ID,
                                 TRANSFER_DATE PAY_DATE,
                                 COMPENSATION PAYED_AMOUNT,
                                 NOTE,
                                 0 IS_LEGAL,
                                 0 CUSTOMER_PAYMENT_ID
                            FROM CRS_USER_TEMP.INSURANCE_TRANSFER_TEMP
                         WHERE IS_CHANGE != 2 AND COMPENSATION > 0 AND INSURANCE_ID = {InsuranceID}
                        ORDER BY ID";
            dtPayed = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPayed", "Sığortanın ödənişləri açılmadı.");
            PayedGridControl.DataSource = dtPayed;
            EditBarButton.Enabled = DeleteBarButton.Enabled = PayedGridView.RowCount > 0 && !CurrentStatus;
            NewBarButton.Enabled = calc_amount < (InsuranceCostValue.Value - ((TypeID == 2) ? TransferAmountValue.Value : 0)) && !CurrentStatus;
            DebtAmount.Value = InsuranceCostValue.Value - calc_amount - (TypeID == 2 ? TransferAmountValue.Value : 0);
            DebtAmount.Value = (DebtAmount.Value >= 0) ? DebtAmount.Value : 0;
            Payed_Legal.Visible = TypeID == 1;
        }


    }
}