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
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using System.IO;
using System.Diagnostics;

namespace CRS.Forms.Contracts
{
    public partial class FContractExtend : DevExpress.XtraEditors.XtraForm
    {
        public FContractExtend()
        {
            InitializeComponent();
        }
        public int ContractID, Percent, CurrencyID;
        public string ContractCode, CustomerName, CurrencyCode, PersonType;
        public DateTime ContractStartDate;
        public decimal Debt;
        public bool Commit;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        string CommitmentName, startDate, endDate;
        int version, extendID, isChange, isCommit, topindex, old_row_num, period;
        bool ClickBOK = false;
        DateTime sdate;

        private void FContractExtend_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddExtend;

            ContractCodeText.Text = ContractCode;
            PercentText.Text = Percent + " %";
            ObjectText.Text = GlobalFunctions.GetName($@"SELECT HOSTAGE FROM CRS_USER.V_HOSTAGE WHERE CONTRACT_ID = {ContractID}", this.Name + "/FContractExtend_Load");
            CommitmentName = GlobalFunctions.GetName($@"SELECT COMMITMENT_NAME FROM CRS_USER.V_CONTRACT_COMMITMENTS WHERE CONTRACT_ID = {ContractID}", this.Name + "/FContractExtend_Load");
            if (String.IsNullOrEmpty(CommitmentName))
                CustomerNameText.Text = CustomerName;
            else
                CustomerNameText.Text = CommitmentName;
            LoadData();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void ExtendsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ExtendsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ExtendsGridView, PopupMenu, e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ExtendStatus() && isChange == 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatma təsdiq edildiyi üçün silinə bilməz.");
                return;
            }

            int pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER.CUSTOMER_PAYMENTS UNION ALL SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP) WHERE PAYMENT_DATE >= TO_DATE('{startDate}','DD.MM.YYYY') AND CONTRACT_ID = {ContractID}");
            if (pay_count > 0)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatma ilə " + startDate + " tarixindən sonra ödəniş olduğu üçün bu müddət uzatmanı silmək olmaz.");
                return;
            }

            List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(1, ContractID).ToList<ContractExtend>();
            if (lstContractExtend.Count > 0)
            {
                var extend = lstContractExtend.Where(item => item.ID > extendID).ToList<ContractExtend>();
                if (extend.Count > 0)
                {
                    GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatmadan sonra başqa müddət uzatmalar olduğu üçün onu silmək olmaz.");
                    return;
                }
            }

            DialogResult dialogResult = XtraMessageBox.Show(ContractCode + " saylı lizinq müqaviləsinin seçilmiş uzadılmasını silmək istəyirsiniz?", "Uzadılmanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithThreeParametrAndUser("CRS_USER_TEMP.PROC_DELETE_CONTRACT_EXTEND", "P_ID", extendID, "P_CONTRACT_ID", ContractID, "P_VERSION", version, ContractCode + " saylı lizinq müqaviləsinin " + version + " uzadılması silinmədi.");
            }
            LoadData();
        }

        private void PaymentsLabel_Click(object sender, EventArgs e)
        {
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = ContractID.ToString();
            fsp.ShowDialog();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            ClickBOK = true;
            this.Close();
        }

        private void ShowFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = ExtendsGridView.TopRowIndex;
            old_row_num = ExtendsGridView.FocusedRowHandle;
            GenerateFile();
            ExtendsGridView.TopRowIndex = topindex;
            ExtendsGridView.FocusedRowHandle = old_row_num;
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExtendTime("EDIT", extendID);
        }

        private void ExtendsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Extend_CheckInterestDebt)
            {
                if (Convert.ToInt32(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CHECK_INTEREST_DEBT").ToString()) == 1)
                    e.Value = "Bəli";
                else
                    e.Value = "Xeyr";
            }

            if (e.Column == Extend_Debt)
            {
                decimal current = Convert.ToDecimal(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CURRENT_DEBT").ToString());
                decimal interest = Convert.ToDecimal(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "INTEREST_DEBT").ToString());
                int check = Convert.ToInt32(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CHECK_INTEREST_DEBT").ToString());
                e.Value = (check == 1) ? current + interest : current;
            }

            GlobalProcedures.GenerateAutoRowNumber(sender, Extend_SS, e);
        }

        private void ExtendsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (ExtendsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditExtend;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteExtend;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void ExtendsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFExtendTime("EDIT", extendID);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFExtendTime("INSERT", null);
        }

        private bool ExtendStatus()
        {
            bool commit = false;
            if (isCommit == 0)
                commit = false;
            else if (isCommit == 1 && !GlobalVariables.CommitContract)
                commit = true;
            else if (isCommit == 1 && GlobalVariables.CommitContract)
                commit = false;

            return commit;
        }

        private void LoadFExtendTime(string transaction, int? extendID)
        {
            topindex = ExtendsGridView.TopRowIndex;
            old_row_num = ExtendsGridView.FocusedRowHandle;
            FExtendTime fe = new FExtendTime();
            fe.TransactionName = transaction;
            fe.Debt = Debt;
            fe.ExtendID = extendID;
            fe.ContractCode = ContractCodeText.Text;
            fe.CurrencyCode = CurrencyCode;
            fe.ContractStartDate = ContractStartDate;
            fe.ContractID = ContractID;
            fe.Interest = (decimal)Percent;
            fe.CurrencyID = CurrencyID;
            fe.Commit = ExtendStatus();
            fe.CustomerName = CustomerNameText.Text;
            fe.PersonType = PersonType;
            fe.RefreshDataGridView += new FExtendTime.DoEvent(LoadData);
            fe.ShowDialog();
            ExtendsGridView.TopRowIndex = topindex;
            ExtendsGridView.FocusedRowHandle = old_row_num;
        }

        private void ExtendsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ExtendsGridView.GetFocusedDataRow();
            if (row != null)
            {
                extendID = int.Parse(row["ID"].ToString());
                version = int.Parse(row["VERSION"].ToString());
                isChange = int.Parse(row["IS_CHANGE"].ToString());
                isCommit = int.Parse(row["IS_COMMIT"].ToString());
                startDate = row["START_DATE"].ToString().Substring(0, 10);
                endDate = row["END_DATE"].ToString().Substring(0, 10);
                sdate = Convert.ToDateTime(row["START_DATE"]);
                period = Convert.ToInt32(row["MONTH_COUNT"]);
            }
        }

        private void FContractExtend_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ClickBOK)
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_EXTEND_TEMP", "P_CONTRACT_ID", ContractID, "Uzadılmalar yenidən temp cədvələ yüklənmədi.");

            this.RefreshDataGridView();
        }

        private void LoadData()
        {
            string sql = $@"SELECT ID,
                                   START_DATE,
                                   END_DATE,
                                   MONTH_COUNT,
                                   INTEREST,
                                   DEBT,
                                   CURRENT_DEBT,
                                   INTEREST_DEBT,
                                   CHECK_INTEREST_DEBT,
                                   MONTHLY_AMOUNT,
                                   VERSION,
                                   NOTE,
                                   IS_CHANGE,
                                   IS_COMMIT
                              FROM CRS_USER_TEMP.CONTRACT_EXTEND_TEMP
                             WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID}
                           ORDER BY ID";

            ExtendsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadData", "Müqavilənin uzatmaları açılmadı.");

            if (ExtendsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditExtend;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteExtend;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void GenerateFile()
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = null, saveAs;
            string currency_name;

            fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müddət uzadılma.docx");
            saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Uzadılma_" + ContractCode.Replace(@"/", null) + ".docx");

            List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(CurrencyID).ToList<Currency>();
            currency_name = lstCurrency.First().NAME;

            string date = GlobalFunctions.DateWithDayMonthYear(sdate);
            string month = period + " (" + GlobalFunctions.IntegerToWritten((double)period) + ")";
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;

            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;
            try
            {
                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                aDoc.Activate();

                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCode);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerName);
                GlobalProcedures.FindAndReplace(wordApp, "[$persontype]", PersonType.ToLower());
                GlobalProcedures.FindAndReplace(wordApp, "[$enddate]", endDate);
                GlobalProcedures.FindAndReplace(wordApp, "[$month]", month);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                if (File.Exists((string)saveAs))
                    Process.Start((string)saveAs);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Uzadılma_" + ContractCode.Replace(@"/", null) + ".docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            finally
            {
                aDoc.Close(ref missing, ref missing, ref missing);
                wordApp.Quit();
                GlobalProcedures.SplashScreenClose();
            }
        }
    }
}