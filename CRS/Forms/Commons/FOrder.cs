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

namespace CRS.Forms.Commons
{
    public partial class FOrder : DevExpress.XtraEditors.XtraForm
    {
        public FOrder()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string ContractCode, CustomerName, BankName, CurrencyCode, Voen, Account;
        public decimal Amount, TotalDebt, ContractAmount;
        public DateTime CommonDate;
        public int CommonID, BankID, CurrencyID, StatusID, ContractID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        DataTable dtOrder = new DataTable();
        decimal calc_amount = 0;
        int orderID, topindex, old_row_num, UsedUserID;
        bool OrderUsed = false, CurrentStatus = false;

        private void FOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_DELETE_CONTRACT_ORDER", "P_COMMON_ID", CommonID, "Tələbnamələr temp cədvəldən silinmədi.");
            this.RefreshDataGridView();
        }

        private void OrderGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OrderGridView, PopupMenu, e);
        }

        private void OrderGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Order_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell(Order_Amount, "Far", e);
        }

        private void FOrder_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_COMMON", GlobalVariables.V_UserID, "WHERE ID = " + CommonID + " AND USED_USER_ID = -1");
            ContractCodeText.Text = ContractCode;
            CustomerNameText.Text = CustomerName;
            BankNameText.Text = BankName;
            OrderGridView.ViewCaption = "Sərəncam : " + Amount.ToString("n2") + " AZN";
            TotalDebtValue.EditValue = TotalDebt;
            UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CONTRACT_COMMON WHERE ID = " + CommonID);
            OrderUsed = (UsedUserID >= 0);
            
            if (OrderUsed)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş sərəncam hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş sərəncamın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;

            if (!CurrentStatus)
                CurrentStatus = StatusID == 19;

            ComponentEnable(CurrentStatus);
            InsertTemp();
            PayedAmountValue.EditValue = GlobalFunctions.GetAmount($@"SELECT NVL(ROUND(SUM (PAYMENT_AMOUNT_AZN), 2), 0) PAYMENT_AMOUNT
                                                                         FROM CRS_USER.CUSTOMER_PAYMENTS
                                                                        WHERE     CONTRACT_ID = {ContractID}");

            CommonPayedAmountValue.EditValue = GlobalFunctions.GetAmount($@"SELECT NVL(ROUND(SUM (PAYMENT_AMOUNT_AZN), 2), 0) PAYMENT_AMOUNT
                                                                                 FROM CRS_USER.CUSTOMER_PAYMENTS
                                                                                WHERE  CONTRACT_ID = {ContractID} AND PAYMENT_DATE >= TO_DATE('{CommonDate.ToString("dd.MM.yyyy")}','DD.MM.YYYY')");

            LoadOrder();
        }

        private void ComponentEnable(bool status)
        {
            NewBarButton.Enabled = GlobalVariables.AddCommonOrder && !status;
            BOK.Visible = !status;
        }

        private void InsertTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_CON_ORDER_TEMP", "P_COMMON_ID", CommonID, "Tələbnamələr temp cədvələ daxil edilmədi.");
        }

        private void OrderGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
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
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0)
                    calc_amount += Convert.ToDecimal(e.FieldValue);
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0)
                    e.TotalValue = calc_amount;
            }
        }

        private void CommonPayedAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            CommonDebtValue.EditValue = Amount - CommonPayedAmountValue.Value;
        }

        private void PayedLabel_Click(object sender, EventArgs e)
        {
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = ContractID.ToString();
            fsp.ShowDialog();
        }

        private void PaymentScheduleLabel_Click(object sender, EventArgs e)
        {
            Total.FPaymentSchedules fps = new Total.FPaymentSchedules();
            fps.ContractID = ContractID.ToString();
            fps.Amount = ContractAmount.ToString("N2") + " " + CurrencyCode;
            fps.ContractCode = ContractCode;
            fps.ShowDialog();
        }

        private void PayedAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            ContractDebtValue.EditValue = TotalDebtValue.Value - PayedAmountValue.Value;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadOrder();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFOrderAddEdit("EDIT", orderID);
        }

        private void OrderGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFOrderAddEdit("EDIT", orderID);
        }

        private void OrderGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (OrderGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditCommonOrder && !CurrentStatus;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteCommonOrder && !CurrentStatus;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = !CurrentStatus;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_CONTRACT_ORDERS", "P_COMMON_ID", CommonID, "Tələbnamələr əsas cədvələ daxil olmadı.");
            this.Close();
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = OrderGridView.TopRowIndex;
            old_row_num = OrderGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş tələbnaməni silmək istəyirsiniz?", "Tələbnamənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CONTRACT_ORDER_TEMP SET IS_CHANGE = 2 WHERE ID = {orderID}",
                                                 "Tələbnamə silinmədi.", this.Name + "/DeleteBarButton_ItemClick");

                LoadOrder();
            }
            OrderGridView.TopRowIndex = topindex;
            OrderGridView.FocusedRowHandle = old_row_num;
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OrderGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OrderGridControl, "xls");
        }

        private void OrderGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = OrderGridView.GetFocusedDataRow();
            if (row != null)
                orderID = Convert.ToInt32(row["ID"]);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFOrderAddEdit("INSERT", null);
        }

        private void LoadFOrderAddEdit(string transaction, int? id)
        {
            DataView dv = new DataView();

            dv = new DataView(dtOrder);
            //if (id != null)
            dv.RowFilter = $@"IS_CHANGE = 1";

            object objCompute = dtOrder.Compute("SUM(AMOUNT)", dv.RowFilter);
            decimal debt = 0;

            if (objCompute != DBNull.Value)
                debt = Convert.ToDecimal(objCompute);

            topindex = OrderGridView.TopRowIndex;
            old_row_num = OrderGridView.FocusedRowHandle;
            FOrderAddEdit fo = new FOrderAddEdit();
            fo.TransactionName = transaction;
            fo.ID = id;
            fo.CurrencyCode = CurrencyCode;
            fo.CommonID = CommonID;
            fo.ContractID = ContractID;
            fo.CommonDate = CommonDate;
            fo.BankID = BankID;
            fo.ContractCode = ContractCodeText.Text.Trim();
            fo.CustomerName = CustomerNameText.Text.Trim();
            fo.CustomerVoen = Voen;
            fo.CustomerAccount = Account;
            fo.CurrencyID = CurrencyID;
            fo.CommonAmount = Amount;
            fo.TotalDebt = TotalDebtValue.Value;
            fo.OrderCount = OrderGridView.RowCount;
            fo.RefreshDataGridView += new FOrderAddEdit.DoEvent(LoadOrder);
            fo.ShowDialog();
            OrderGridView.TopRowIndex = topindex;
            OrderGridView.FocusedRowHandle = old_row_num;
        }

        private void OrderGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Order_SS, e);
        }

        private void LoadOrder()
        {
            string sql = $@"SELECT ID,
                                   ORDER_DATE,
                                   ORDER_NUMBER,
                                   AMOUNT,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS U
                                     WHERE U.ID = C.INSERT_USER)
                                      INSERT_USER,
                                   C.INSERT_DATE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS U
                                     WHERE U.ID = C.UPDATE_USER)
                                      UPDATE_USER,
                                   UPDATE_DATE,
                                   C.IS_CHANGE
                              FROM CRS_USER_TEMP.CONTRACT_ORDER_TEMP C
                             WHERE IS_CHANGE != 2 AND CONTRACT_COMMON_ID = {CommonID}
                            ORDER BY ORDER_DATE, ID";
            dtOrder = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadOrder", "Tələbnamələr yüklənmədi.");
            OrderGridControl.DataSource = dtOrder;
            if (OrderGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditCommonOrder && !CurrentStatus;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteCommonOrder && !CurrentStatus;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = !CurrentStatus;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
            NewBarButton.Enabled = calc_amount < Amount && GlobalVariables.AddCommonOrder && !CurrentStatus;
            //CommonDebtValue.Value = AmountValue.Value - calc_amount;
            //CommonDebtValue.Value = (CommonDebtValue.Value >= 0) ? CommonDebtValue.Value : 0;
        }
    }
}