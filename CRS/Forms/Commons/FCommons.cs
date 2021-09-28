using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using CRS.Class;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.Commons
{
    public partial class FCommons : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FCommons()
        {
            InitializeComponent();
        }
        int commonID, topindex, old_row_num, bankID, currencyID, statusID, contractID;
        string contractCode, bankName, customerName, currencyCode, voen, account;
        decimal amount, totalDebt = 0, contractAmount;
        DateTime commonDate;

        private void FCommons_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadCommons()
        {
            string status = null;

            if (ClosedCommonBarCheck.Checked && !AktiveCommonBarCheck.Checked)
                status = " AND CC.STATUS_ID = 19";
            else if (!ClosedCommonBarCheck.Checked && AktiveCommonBarCheck.Checked)
                status = " AND CC.STATUS_ID = 18";
            else 
                status = " AND CC.STATUS_ID IN (18, 19)";

            string sql = $@"SELECT CC.ID,
                                   CC.COMMON_DATE,
                                   C.CONTRACT_CODE,
                                   B.LONG_NAME BANK_NAME,
                                   CC.CUSTOMER_NAME,
                                   CC.VOEN,
                                   CC.ACCOUNT_NUMBER,
                                   CC.AMOUNT,
                                   CC.TEMP_AMOUNT,
                                   CC.AMOUNT + CC.TEMP_AMOUNT TOTAL_AMOUNT,
                                   NVL (SO.AMOUNT, 0) ORDER_AMOUNT,
                                   'AZN' CURRENCY_CODE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS U
                                     WHERE U.ID = CC.INSERT_USER)
                                      INSERT_USER,
                                   CC.INSERT_DATE,
                                   B.ID BANK_ID,
                                   C.CURRENCY_ID,
                                   CC.USED_USER_ID,
                                   CC.STATUS_ID,
                                   C.CREDIT_TYPE_ID,
                                   CC.TOTAL_DEBT,
                                   C.CONTRACT_ID,
                                   C.AMOUNT CONTRACT_AMOUNT
                              FROM CRS_USER.CONTRACT_COMMON CC,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.BANKS B,
                                   CRS_USER.V_SUM_CONTRACT_ORDER SO
                             WHERE     CC.CONTRACT_ID = C.CONTRACT_ID
                                   AND CC.BANK_ID = B.ID
                                   AND CC.ID = SO.CONTRACT_COMMON_ID(+){status}
                            ORDER BY CC.ID";

            ContractsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadCommons", "Sərəncamlar yüklənmədi.");

            OrderBarButton.Enabled =  ShowFileBarButton.Enabled = ContractsGridView.RowCount > 0;
            DeleteBarButton.Enabled = (ContractsGridView.RowCount > 0 && GlobalVariables.DeleteCommonOrder);
        }

        private void ContractsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Contract_SS, e);
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadCommons();
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractsGridControl);
        }

        private void ContractsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void FCommons_Activated(object sender, EventArgs e)
        {
            LoadCommons();
        }

        private void ShowFileBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFile();
        }

        private void LoadFile()
        {
            string fileType = ".docx", directoryPath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\", filePath = null;

            filePath = directoryPath + "Common_" + commonID + fileType;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (File.Exists(filePath))
            {
                Process.Start(filePath);
                return;
            }

            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.COMMON_FILE FROM CRS_USER.CONTRACT_COMMON T WHERE T.ID = {commonID}",
                                                    filePath,
                                                    "COMMON_FILE");            
        }

        private void ContractsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractsGridView.GetFocusedDataRow();
            if (row != null)
            {
                commonID = Convert.ToInt32(row["ID"]);
                contractID = Convert.ToInt32(row["CONTRACT_ID"]);
                customerName = row["CUSTOMER_NAME"].ToString();
                bankName = row["BANK_NAME"].ToString();
                contractCode = row["CONTRACT_CODE"].ToString();
                amount = Convert.ToDecimal(row["TOTAL_AMOUNT"]);
                contractAmount = Convert.ToDecimal(row["CONTRACT_AMOUNT"]);
                currencyCode = row["CURRENCY_CODE"].ToString();
                voen = row["VOEN"].ToString();
                account = row["ACCOUNT_NUMBER"].ToString();
                commonDate = GlobalFunctions.ChangeStringToDate(row["COMMON_DATE"].ToString().Substring(0, 10), "ddmmyyyy");
                bankID = Convert.ToInt16(row["BANK_ID"]);
                currencyID = Convert.ToInt16(row["CURRENCY_ID"]);
                statusID = Convert.ToInt16(row["STATUS_ID"]);
                totalDebt = Convert.ToDecimal(row["TOTAL_DEBT"]);
            }
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "xls");
        }

        private void ClosedCommonBarCheck_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadCommons();
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "html");
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if ((e.RowHandle >= 0) && (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["USED_USER_ID"])) >= 0)
                     && (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["STATUS_ID"])) == 18))
                GlobalProcedures.GridRowCellStyleForBlock(ContractsGridView, e);
            else
                GlobalProcedures.GridRowCellStyleForClose(19, ContractsGridView, e);

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "txt");
        }

        private void ContractsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            OrderBarButton.Enabled = ShowFileBarButton.Enabled = ContractsGridView.RowCount > 0;
            DeleteBarButton.Enabled = (ContractsGridView.RowCount > 0 && GlobalVariables.DeleteCommonOrder);
        }

        private void ContractsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (OrderBarButton.Enabled)
                LoadFOrder();
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sərəncamı silmək istəyirsiniz?", "Sərəncamın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_CONTRACT_COMMON", "P_CONTRACT_COMMON_ID", commonID, "Sərəncam silinmədi.");

                LoadCommons();                
            }
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "csv");
        }

        private void OrderBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFOrder();
        }

        private void LoadFOrder()
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FOrder fo = new FOrder();
            fo.CustomerName = customerName;
            fo.BankName = bankName;
            fo.ContractCode = contractCode;
            fo.Amount = amount;
            fo.CommonID = commonID;
            fo.CurrencyCode = currencyCode;
            fo.CommonDate = commonDate;
            fo.BankID = bankID;
            fo.Voen = voen;
            fo.Account = account;
            fo.CurrencyID = currencyID;
            fo.StatusID = statusID;
            fo.TotalDebt = totalDebt;
            fo.ContractID = contractID;
            fo.ContractAmount = contractAmount;
            fo.RefreshDataGridView += new FOrder.DoEvent(LoadCommons);
            fo.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "mht");
        }

        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractsGridView, PopupMenu, e);
        }
    }
}