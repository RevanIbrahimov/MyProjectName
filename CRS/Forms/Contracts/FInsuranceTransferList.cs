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
using DevExpress.XtraGrid.Columns;

namespace CRS.Forms.Contracts
{
    public partial class FInsuranceTransferList : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceTransferList()
        {
            InitializeComponent();
        }
        public string ListID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        string contractCode, transferDate, note;
        int topindex, old_row_num, insuranceID, id;
        decimal transferAmount, insuranceCost, compensation;
        DataTable dtTransfer = new DataTable();

        private void FInsuranceTransferList_Load(object sender, EventArgs e)
        {
            LoadTransfer();
            ClosedCheck.Checked = ListID != null;
        }

        private void LoadTransfer()
        {
            string filterDate = null, status = null, id = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 6";
            else if (!ClosedCheck.Checked && ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
                filterDate = $@" AND IP.TRANSFER_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')";

            string sql = $@"SELECT IP.ID,
                                     C.CONTRACT_CODE,
                                     IC.NAME COMPANY_NAME,                                    
                                     IP.TRANSFER_DATE,
                                     IP.TRANSFER_AMOUNT,
                                     PT.CODE TASK_CODE,
                                     IP.NOTE,
                                     H.HOSTAGE,
                                     C.STATUS_ID,
                                     IP.INSURANCE_ID,
                                     ROUND(INSURANCE_AMOUNT * INSURANCE_INTEREST / 100, 2) INSURANCE_COST,
                                     ST.COMPENSATION,
                                     I.POLICE
                                FROM CRS_USER.INSURANCE_TRANSFER IP,
                                     CRS_USER.INSURANCES I,
                                     CRS_USER.V_CONTRACTS C,
                                     CRS_USER.INSURANCE_COMPANY IC,
                                     CRS_USER.V_HOSTAGE H,
                                     CRS_USER.PAYMENT_TASKS PT,
                                     CRS_USER.V_SUM_INSURANCE_TRANSFER ST
                               WHERE     IP.INSURANCE_ID = I.ID
                                     AND I.CONTRACT_ID = C.CONTRACT_ID
                                     AND C.CONTRACT_ID = H.CONTRACT_ID
                                     AND I.COMPANY_ID = IC.ID
                                     AND IP.TRANSFER_AMOUNT > 0
                                     AND IP.INSURANCE_ID = ST.INSURANCE_ID
                                     AND IP.PAYMENT_TASK_ID = PT.ID(+){filterDate}{status}
                            ORDER BY ID";

            dtTransfer = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPayment", "Sığortaların ödənişlərinin siyahısı yüklənmədi.");
            InsuranceGridControl.DataSource = dtTransfer;

            if (ListID != null)
                InsuranceGridView.ActiveFilter.Add(InsuranceGridView.Columns["INSURANCE_ID"],
                    new ColumnFilterInfo($@"[INSURANCE_ID] IN ({ListID})", $@"Sığorta in ({ListID})"));

            if (InsuranceGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditInsuranceTransfer;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteInsuranceTransfer;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Insurance_SS, e);
        }

        private void InsuranceGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Insurance_SS, "Center", e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadTransfer();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadTransfer();
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, PopupMenu, e);
        }

        private void InsuranceGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void InsuranceGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForClose(6, InsuranceGridView, e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş köçürmələni silmək istəyirsiniz? Silinmiş köçürməni geri qaytarmaq olmayacaq", "Seçilmiş köçürmənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DEL_INSURANCE_TRANSFER", "P_ID", id, "Seçilmiş sığortanın köçürmələri silinmədi.");
            }
            LoadTransfer();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void InsuranceGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InsuranceGridView.GetFocusedDataRow();
            if (row != null)
            {
                id = int.Parse(row["ID"].ToString());
                contractCode = row["CONTRACT_CODE"].ToString();
                transferDate = row["TRANSFER_DATE"].ToString().Substring(0, 10);
                note = row["NOTE"].ToString();
                transferAmount = Convert.ToDecimal(row["TRANSFER_AMOUNT"]);
                insuranceCost = Convert.ToDecimal(row["INSURANCE_COST"]);
                compensation = Convert.ToDecimal(row["COMPENSATION"]);
                insuranceID = int.Parse(row["INSURANCE_ID"].ToString());
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceTransferEdit();
        }

        private void LoadFInsuranceTransferEdit()
        {
            DataView dv = new DataView();

            dv = new DataView(dtTransfer);
            dv.RowFilter = $@"ID <> {id} AND INSURANCE_ID = {insuranceID}";
            object objTransfer = dtTransfer.Compute("SUM(TRANSFER_AMOUNT)", dv.RowFilter);

            decimal debt = 0, transfer = 0;

            if (objTransfer != DBNull.Value)
                transfer = Convert.ToDecimal(objTransfer);

            debt = transfer + compensation;

            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            FInsuranceTransferEdit fe = new FInsuranceTransferEdit();
            fe.TransferID = id;
            fe.ContractCode = contractCode;
            fe.TransferDate = transferDate;
            fe.TransferAmount = transferAmount;
            fe.InsuranceCost = insuranceCost;
            fe.Debt = insuranceCost - debt;
            fe.Note = note;
            fe.Compensation = compensation;
            fe.RefreshDataGridView += new FInsuranceTransferEdit.DoEvent(LoadTransfer);
            fe.ShowDialog();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void InsuranceGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFInsuranceTransferEdit();
        }

        private void InsuranceGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (InsuranceGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditInsuranceTransfer;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteInsuranceTransfer;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void FInsuranceTransferList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}