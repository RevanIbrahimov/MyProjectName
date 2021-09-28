using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System.Threading;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.XtraSplashScreen;
using CRS.Class.Views;

namespace CRS.Forms.Contracts
{
    public partial class FContracts : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FContracts()
        {
            InitializeComponent();
        }
        string ContractID,
            CustomerID,
            SellerID,
            ContractCode,
            CustomerFullName,
            CommitmentName,
            toolTip = null,
            ToolTipCustomerID = null,
            TitleText = null,
            currency_name,
            credit_name,
            CurrencyCode,
            s_date,
            e_date,
            customerCode,
            customerName;

        int Commit,
            credit_name_id,
            status_id,
            credit_type_id,
            old_row_num,
            topindex,
            row_num,
            seller_type_id,
            customer_type_id,
            commitmentPersonTypeID = 0,
            interest,
            period,
            commitmentID = 0,
            isSpecialAttention = 0;

        double amount;

        bool FormStatus = false, IsInsert = false, isExtend;

        private void FContracts_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                NewContractBarButton.Enabled = GlobalVariables.AddContract;
                PrintBarButton.Enabled = GlobalVariables.PrintContract;
                ExportBarSubItem.Enabled = GlobalVariables.ExportContract;
                PaymentBarButton.Enabled = GlobalVariables.AddPayment;
            }

            SearchDockPanel.Hide();
            ShowOrHideSellerBarButton.Down = true;
            ContractsGridControl.Height = this.Height - 130;
            FormStatus = true;
        }

        private void LoadContractsDataGridView()
        {
            string s = null, is_commit = null, status = null, agreement = null, note = null, commitment = null;

            if ((NotCommitBarCheck.Checked) && (!CommitBarCheck.Checked))
                is_commit = is_commit + " AND C.IS_COMMIT = 0";
            else if ((!NotCommitBarCheck.Checked) && (CommitBarCheck.Checked))
                is_commit = is_commit + " AND C.IS_COMMIT = 1";
            else
                is_commit = null;

            if (AgreementBarCheck.Checked)
                agreement = agreement + " AND C.PARENT_ID IS NOT NULL";
            else
                agreement = null;

            if (NoteBarCheck.Checked)
                note = note + " AND C.NOTE IS NOT NULL";
            else
                note = null;

            if (CloseBarCheck.Checked && !CancelCheck.Checked)
                status = " AND C.STATUS_ID IN (5, 6)";
            else if (!CloseBarCheck.Checked && CancelCheck.Checked)
                status = " AND C.STATUS_ID = 17";
            else if (!CloseBarCheck.Checked && !CancelCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            if (IsCommitmentBarCheck.Checked)
                commitment = " AND COM.COMMITMENT_NAME IS NOT NULL";
            else
                commitment = null;

            s = $@"SELECT 1 SS,
                             C.ID,
                             CU.FULLNAME CUSTOMERFULLNAME,
                             CT.CODE || C.CODE CONTRACT_CODE,
                             CN.NAME CREDIT_NAME,
                             COM.COMMITMENT_NAME COMMITMENT,
                             C.START_DATE,
                             C.END_DATE,
                             (CASE
                                 WHEN C.CHECK_PERIOD > 0 THEN C.CHECK_PERIOD || ' ay'
                                 ELSE CT.TERM || ' ay'
                              END)
                                PERIOD,
                             (CASE
                                 WHEN C.CHECK_INTEREST > -1 THEN C.CHECK_INTEREST || ' %'
                                 ELSE CT.INTEREST || ' %'
                              END)
                                INTEREST,
                             C.GRACE_PERIOD || ' ay' GRACE,
                             C.AMOUNT,
                             CUR.CODE CURRENCY_CODE,
                             C.USED_USER_ID,
                             C.CUSTOMER_ID,
                             C.SELLER_ID,
                             C.IS_COMMIT,
                             C.STATUS_ID,
                             ST.STATUS_NAME,
                             CN.ID CREDIT_NAME_ID,
                             TRIM (C.NOTE) CONTRACT_NOTE,
                             C.NOTE_CHANGE_USER,
                             TO_CHAR (C.NOTE_CHANGE_DATE, 'DD.MM.YYYY HH24:MI:SS AM')
                                NOTE_CHANGE_DATE,
                             (CASE
                                 WHEN C.CHECK_INTEREST > -1 THEN C.CHECK_INTEREST
                                 ELSE CT.INTEREST
                              END)
                                INT_INTEREST,
                             (CASE WHEN C.CHECK_PERIOD > 0 THEN C.CHECK_PERIOD ELSE CT.TERM END)
                                INT_PERIOD,
                             C.NOTE NOTES,
                             CT.ID CREDIT_TYPE_ID,
                             ROW_NUMBER () OVER (ORDER BY CT.CODE || C.CODE DESC) ROW_NUM,
                             (CASE WHEN C.LIQUID_TYPE = 0 THEN C.IS_EXPENSES ELSE 1 END)
                                IS_EXPENSES,
                             C.SELLER_TYPE_ID,
                             C.CUSTOMER_TYPE_ID,
                             C.PARENT_ID,
                             COM.PERSON_TYPE_ID COMMITMENT_PERSON_TYPE_ID,
                             CU.CODE CUSTOMER_CODE,
                             CU.CUSTOMER_NAME,
                             NVL(COM.ID,0) COMMITMENT_ID,
                             H.CAR_NUMBER,
                             NVL2(CE.MONTH_COUNT, CE.MONTH_COUNT||' ay',  NULL) EXTEND_MOUNT_COUNT,
                             NVL2(CE.MONTH_COUNT, CE.MONTH_COUNT,  0) INT_EXTEND_MOUNT_COUNT,
                             C.IS_SPECIAL_ATTENTION
                        FROM CRS_USER.CONTRACTS C,
                             CRS_USER.CREDIT_TYPE CT,
                             CRS_USER.CREDIT_NAMES CN,
                             CRS_USER.CURRENCY CUR,
                             CRS_USER.STATUS ST,
                             CRS_USER.V_CUSTOMERS CU,
                             CRS_USER.V_CONTRACT_COMMITMENTS COM,
                             CRS_USER.HOSTAGE_CAR H,
                             CRS_USER.V_LAST_CONTRACT_EXTEND CE
                       WHERE     C.CREDIT_TYPE_ID = CT.ID
                             AND C.CUSTOMER_ID = CU.ID
                             AND C.CUSTOMER_TYPE_ID = CU.PERSON_TYPE_ID
                             AND C.ID = COM.CONTRACT_ID(+)
                             AND CT.NAME_ID = CN.ID
                             AND C.CURRENCY_ID = CUR.ID
                             AND C.STATUS_ID = ST.ID
                             AND C.ID = H.CONTRACT_ID(+)
                             AND C.ID = CE.CONTRACT_ID(+)
                             {status}{is_commit}{agreement}{note}{commitment}
                    ORDER BY CT.CODE || C.CODE DESC";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractsDataGridView");

            if (dt == null)
                return;

            ContractsGridControl.DataSource = dt;
            if (ContractsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteContractBarButton.Enabled = GlobalVariables.DeleteContract;
                    EditContractBarButton.Enabled = GlobalVariables.EditContract;
                    AgreementBarButton.Enabled = GlobalVariables.AddAgreement;
                    CancelBarButton.Enabled = GlobalVariables.CancelContract;
                }
                else
                    DeleteContractBarButton.Enabled = EditContractBarButton.Enabled = AgreementBarButton.Enabled = CancelBarButton.Enabled = true;

                NoteBarButton.Enabled = SpecialInfoExportBarButton.Enabled = PaymentBarButton.Enabled = true;
            }
            else
                DeleteContractBarButton.Enabled =
                    EditContractBarButton.Enabled =
                    NoteBarButton.Enabled =
                    SpecialInfoExportBarButton.Enabled =
                    OpenContractBarButton.Enabled =
                    CloseContractBarButton.Enabled =
                    AgreementBarButton.Enabled =
                    CancelBarButton.Enabled =
                    PaymentBarButton.Enabled = false;
        }

        void RefreshContracts(string contract_id)
        {
            string status = null;
            if (IsInsert)
            {
                if (CloseBarCheck.Checked)
                    status = " AND C.STATUS_ID IN (5,6)";
                else
                    status = " AND C.STATUS_ID = 5";
                old_row_num = GlobalFunctions.GetID($@"SELECT ROW_NUM - 1
                                                                      FROM (  SELECT ROW_NUMBER () OVER (ORDER BY CT.CODE || C.CODE DESC) ROW_NUM,
                                                                                     C.ID
                                                                                FROM CRS_USER.CONTRACTS C, CRS_USER.CREDIT_TYPE CT
                                                                               WHERE C.CREDIT_TYPE_ID = CT.ID {status}
                                                                            ORDER BY CT.CODE || C.CODE DESC)
                                                                     WHERE ID = {contract_id}");
            }
            LoadContractsDataGridView();
        }

        private void LoadFContractAddEdit(string transaction, string contract_id, string customer_id, string seller_id, int commit)
        {
            if (!IsInsert)
            {
                old_row_num = ContractsGridView.FocusedRowHandle;
                topindex = ContractsGridView.TopRowIndex;
            }
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractShowWait));

            FContractAddEdit fc = new FContractAddEdit();
            fc.TransactionName = transaction;
            fc.ContractID = contract_id;
            fc.CustomerID = customer_id;
            fc.SellerID = seller_id;
            fc.IsExtend = isExtend;
            fc.Commit = commit;
            if (transaction == "EDIT")
                fc.IsSpecialAttention = isSpecialAttention;
            fc.RefreshContractsDataGridView += new FContractAddEdit.DoEvent(RefreshContracts);
            GlobalProcedures.SplashScreenClose();
            fc.ShowDialog();

            ContractsGridView.FocusedRowHandle = old_row_num;
            if (!IsInsert)
                ContractsGridView.TopRowIndex = topindex;

        }

        private void NewContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            IsInsert = true;
            LoadFContractAddEdit("INSERT", null, null, null, 0);
        }

        private void ShowOrHideSellerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ShowOrHideSellerBarButton.Down)
            {
                HostageGridControl.Visible = false;
                ContractsGridControl.Dock = DockStyle.Fill;
                CustomerSplitter.Visible = false;
                ShowOrHideSellerBarButton.Caption = "Girovu göstər";
            }
            else
            {
                HostageGridControl.Visible = true;
                ContractsGridControl.Dock = DockStyle.Top;
                CustomerSplitter.Visible = true;
                ShowOrHideSellerBarButton.Caption = "Girovu gizlət";
            }
        }

        private void ContractsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
            if (e.Column.FieldName == "IS_COMMITMENT")
                if (Convert.ToDecimal(e.Value) == 0)
                    e.DisplayText = "";
        }

        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractsGridView, PopupMenu, e);
        }

        private void ContractsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractsGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractID = row["ID"].ToString();
                CustomerID = row["CUSTOMER_ID"].ToString();
                SellerID = row["SELLER_ID"].ToString();
                ContractCode = row["CONTRACT_CODE"].ToString();
                CustomerFullName = row["CUSTOMERFULLNAME"].ToString();
                customerName = row["CUSTOMER_NAME"].ToString();
                customerCode = row["CUSTOMER_CODE"].ToString();
                CommitmentName = row["COMMITMENT"].ToString();
                CurrencyCode = row["CURRENCY_CODE"].ToString();
                Commit = Convert.ToInt32(row["IS_COMMIT"].ToString());
                status_id = Convert.ToInt32(row["STATUS_ID"].ToString());
                row_num = Convert.ToInt32(row["ROW_NUM"].ToString());
                credit_name_id = Convert.ToInt32(row["CREDIT_NAME_ID"].ToString());
                seller_type_id = Convert.ToInt32(row["SELLER_TYPE_ID"].ToString());
                customer_type_id = Convert.ToInt32(row["CUSTOMER_TYPE_ID"].ToString());
                amount = Convert.ToDouble(row["AMOUNT"].ToString());
                interest = Convert.ToInt32(row["INT_INTEREST"].ToString());
                period = Convert.ToInt32(row["INT_PERIOD"].ToString());
                commitmentID = Convert.ToInt32(row["COMMITMENT_ID"].ToString());
                if (!String.IsNullOrWhiteSpace(row["COMMITMENT_PERSON_TYPE_ID"].ToString()))
                    commitmentPersonTypeID = Convert.ToInt32(row["COMMITMENT_PERSON_TYPE_ID"].ToString());
                else
                    commitmentPersonTypeID = 0;
                s_date = row["START_DATE"].ToString().Substring(0, 10);
                e_date = row["END_DATE"].ToString().Substring(0, 10);

                isExtend = !(String.IsNullOrWhiteSpace(row["EXTEND_MOUNT_COUNT"].ToString()));

                LoadHostageDataGridView();
                HostageGridView.ViewCaption = ContractCode + " nömrəli müqaviləyə aid olan girov haqqında məlumatlar";
                OpenContractBarButton.Enabled = (status_id == 6 && GlobalVariables.OpenContract);
                CloseContractBarButton.Enabled = (status_id == 5 && GlobalVariables.CloseContract);
                isSpecialAttention = Convert.ToInt32(row["IS_SPECIAL_ATTENTION"].ToString());
            }
        }

        private void EditContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            IsInsert = false;
            LoadFContractAddEdit("EDIT", ContractID, CustomerID, SellerID, Commit);
        }

        private void ContractsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditContractBarButton.Enabled)
            {
                IsInsert = false;
                LoadFContractAddEdit("EDIT", ContractID, CustomerID, SellerID, Commit);
            }
        }

        private void RefreshContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadContractsDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractsGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "xls");
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

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "mht");
        }

        private void LoadHostageDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                   H.ID,
                                   S.FULLNAME SELLER_NAME,
                                   HOSTAGE,
                                   LIQUID_AMOUNT,
                                   FIRST_PAYMENT,
                                   CURRENCY_CODE
                              FROM CRS_USER.V_HOSTAGE H, CRS_USER.CONTRACTS C, CRS_USER.V_SELLERS S
                             WHERE     H.CONTRACT_ID = C.ID
                                   AND C.SELLER_ID = S.ID
                                   AND C.SELLER_TYPE_ID = S.PERSON_TYPE_ID
                                   AND H.CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadHostageDataGridView");
            HostageGridControl.DataSource = dt;
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if ((e.RowHandle >= 0) && (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["USED_USER_ID"])) < 0)
                     && (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["STATUS_ID"])) == 5))
            {
                GlobalProcedures.GridRowCellStyleForNotCommit(ContractsGridView, e);

                int is_expenses = int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["IS_EXPENSES"]));
                if (is_expenses == 0)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ContractNotExpensesColor1);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ContractNotExpensesColor2);
                }
            }
            else if (int.Parse(ContractsGridView.GetRowCellDisplayText(e.RowHandle, ContractsGridView.Columns["USED_USER_ID"])) >= 0)
                GlobalProcedures.GridRowCellStyleForBlock(ContractsGridView, e);
            else
                GlobalProcedures.GridRowCellStyleForClose(6, ContractsGridView, e);

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void NotCommitBarCheck_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadContractsDataGridView();
        }

        private void BClear_Click(object sender, EventArgs e)
        {
            CustomerNameText.Text = ContractCodeText.Text = NoteText.Text = CommitmentText.Text = null;
            InterestValue.Value = PeriodValue.Value = 0;
            CurrencyComboBox.Text = CreditNameComboBox.Text = null;
            FromDateValue.Text = ToDateValue.Text = null;
            FromAmountValue.Text = ToAmountValue.Text = null;
            FilterContracts();
        }

        private void ContractsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void NorPowerOfAttorneyBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FNoPowerOfAttorneyList fnpa = new FNoPowerOfAttorneyList();
            fnpa.ShowDialog();
        }

        private void CancelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS UNION ALL SELECT CONTRACT_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP UNION ALL SELECT CONTRACT_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT) WHERE CONTRACT_ID = {ContractID}", this.Name + "/DeleteContract");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("<color=red><b>" + ContractCode + "</b></color> saylı Lizinq Müqaviləsini ləğv etmək istəyirsiniz? Ləğv edilmiş müqaviləni geri qaytarmaq olmayacaq.", "Müqavilənin ləğv edilməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_CHANGE_CONTRACT_STATUS", "P_CONTRACT_ID", ContractID, "P_STATUS_ID", 17, "Müqavilənin statusu dəyişdirilmədi.");
                LoadContractsDataGridView();
            }
            else
                GlobalProcedures.ShowWarningMessage("Seçilmiş " + ContractCode + " saylı Lizinq Müqaviləsinin ödənişləri və ya Lizinq müqaviləsi üzrə məxaric olduğu üçün bu müqaviləni ləğv etmək olmaz.");
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void CancelCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadContractsDataGridView();
        }

        private void ContractsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(ContractsGridView, ContractRibbonPage.Text);
        }

        private void HostageGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(HostageGridView, "Satıcı");
        }

        private void FromEntendMountCount_EditValueChanged(object sender, EventArgs e)
        {
            ToEntendMountCount.Properties.MinValue = FromEntendMountCount.Value;
            FilterContracts();
        }

        private void ToEntendMountCount_EditValueChanged(object sender, EventArgs e)
        {
            FromEntendMountCount.Properties.MinValue = 1;
            FilterContracts();
        }

        private void PowerOfAttorneyBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FPowerOfAttorneyList fpa = new FPowerOfAttorneyList();
            fpa.ShowDialog();
        }

        private void SellerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FSeller fs = new FSeller();
            fs.ShowDialog();
        }

        private void FinishedContractsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FFinishedContracts ff = new FFinishedContracts();
            ff.ShowDialog();
        }

        private void OpenContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            //XtraMessageBox.AllowHtmlText = true;
            DialogResult dialogResult = XtraMessageBox.Show("Bağlanmış <color=red><b>" + ContractCode + "</b></color> saylı Lizinq Müqaviləsini açmaq istəyirsiniz?", "Müqavilənin açılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_CHANGE_CONTRACT_STATUS", "P_CONTRACT_ID", ContractID, "P_STATUS_ID", 5, "Bağlanmış müqavilə açılmadı");
            LoadContractsDataGridView();
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void BStatisticBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FStatistic fs = new FStatistic();
            fs.ShowDialog();
        }

        private void InsuranceBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FInsurance fi = new FInsurance();
            fi.ShowDialog();
        }

        private void AgreementBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();

            int ContractUsedUserID = contract.USED_USER_ID;
            if ((ContractUsedUserID == -1) || (GlobalVariables.V_UserID == ContractUsedUserID))
            {
                double percent = GlobalFunctions.FindContractResidualPercent(int.Parse(ContractID));
                if (percent > 0)
                    XtraMessageBox.Show("Seçilmiş <color=red><b>" + ContractCode + "</b></color> saylı Lizinq Müqaviləsinin bu günə qədər <b>" + percent.ToString("N2") + " " + CurrencyCode + "</b> qalıq faizi olduğu üçün bu müqavilənin razılaşmasını yaratmaq olmaz.\r\n\r\n<b>1. Razılaşdırılmış müqavilə üçün əvvəlcə köhnə müqavilənin bütün qalıq faizləri ödənilməlidir.\r\n2. Qalıq borc digər ödənişlərə daxil edilməlidir.</b>",
                                           "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    LoadFContractAddEdit("AGREEMENT", ContractID, CustomerID, SellerID, Commit);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ContractUsedUserID).FULLNAME;
                XtraMessageBox.Show(ContractCode + " saylı Lizinq Müqaviləsinin məlumatları " + used_user_name + " tərəfindən istifadə edilir. Bu müqavilə üçün razılaşdırma yaratmaq olmaz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadFPayment(string contract_id, string contract_code, int interest, int period, string s_date, string e_date, string lizinq, string customer_code, string customer_name, string customer_id, double amount, string currency, string commitment_name, int commitment_id)
        {
            Total.FPayment fp = new Total.FPayment();
            fp.ContractID = contract_id;
            fp.ContractCode = contract_code;
            fp.Interest = interest;
            fp.Period = period;
            fp.SDate = s_date;
            fp.EDate = e_date;
            fp.Lizinq = lizinq;
            fp.CustomerID = customer_id;
            fp.CustomerCode = customer_code;
            fp.CustomerName = customer_name;
            fp.CommitmentName = commitment_name;
            fp.CommitmentID = commitment_id;
            fp.CommitmentPersonTypeID = commitmentPersonTypeID;
            fp.Amount = amount;
            fp.Currency = currency;
            fp.CustomerTypeID = customer_type_id;
            fp.IsExtend = isExtend;
            fp.DebtDate = null;
            fp.CreditNameID = credit_name_id;
            fp.RefreshTotalsDataGridView += new Total.FPayment.DoEvent(LoadContractsDataGridView);
            fp.ShowDialog();
        }

        private void PaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!IsInsert)
            {
                old_row_num = ContractsGridView.FocusedRowHandle;
                topindex = ContractsGridView.TopRowIndex;
            }

            var leasingDetail = HostagesViewDAL.SelectHostage(int.Parse(ContractID)).ToList<HostagesView>().LastOrDefault();

            LoadFPayment(ContractID, ContractCode, interest, period, s_date, e_date, leasingDetail.HOSTAGE, customerCode, customerName, CustomerID, amount, CurrencyCode, CommitmentName, commitmentID);

            ContractsGridView.FocusedRowHandle = old_row_num;
            if (!IsInsert)
                ContractsGridView.TopRowIndex = topindex;
        }

        private void CloseContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            double debt = GlobalFunctions.GetAmount($@"SELECT DEBT
                                                              FROM CRS_USER.CUSTOMER_PAYMENTS P
                                                             WHERE     CONTRACT_ID = {ContractID}
                                                                   AND ID = (SELECT MAX(ID)
                                                                               FROM CRS_USER.CUSTOMER_PAYMENTS
                                                                              WHERE CONTRACT_ID = P.CONTRACT_ID)", this.Name + "/CloseContractBarButton_ItemClick");

            //XtraMessageBox.AllowHtmlText = true;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş <color=red><b>" + ContractCode + "</b></color> saylı Lizinq Müqaviləsinin <b>" + debt.ToString("N2") + " " + CurrencyCode + "</b> borcu var. Müqaviləni bağlamaq istəyirsiniz?", "Müqavilənin bağlanılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_CHANGE_CONTRACT_STATUS", "P_CONTRACT_ID", ContractID, "P_STATUS_ID", 6, "Açıq müqavilə bağlanmadı");
            LoadContractsDataGridView();
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void ContractLawsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FContractLaws fcl = new FContractLaws();
            fcl.ShowDialog();
        }

        private void DeleteContract()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS UNION ALL SELECT CONTRACT_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP UNION ALL SELECT CONTRACT_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT) WHERE CONTRACT_ID = {ContractID}", this.Name + "/DeleteContract");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş " + ContractCode + " saylı Lizinq Müqaviləsini silmək istəyirsiniz?", "Müqavilənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CASH_ADVANCE_DELETE", "P_CONTRACT_ID", int.Parse(ContractID), "Avans məbləği kassadan silinmədi.");
                    GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CONTRACT_DELETE", "P_CONTRACT_ID", ContractID, "Müqavilə bazadan silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş " + ContractCode + " saylı Lizinq Müqaviləsinin ödənişləri və ya Lizinq müqaviləsi üzrə məxaric olduğu üçün bu müqavilənin məlumatlarını silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();
            if (contract.STATUS_ID == 6)
                XtraMessageBox.Show("Seçilmiş " + ContractCode + " saylı Lizinq Müqaviləsi bağlanılıb. Statistika üçün bu müqavilənin məlumatlarını bazadan silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int ContractUsedUserID = contract.USED_USER_ID;
                if ((ContractUsedUserID == -1) || (GlobalVariables.V_UserID == ContractUsedUserID))
                    DeleteContract();
                else
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ContractUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Silmək istədiyiniz " + ContractCode + " saylı Lizinq Müqaviləsinin məlumatları " + used_user_name + " tərəfindən istifadə edilir. Bu müqavilənin məlumatlarını silmək olmaz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            LoadContractsDataGridView();
            LoadHostageDataGridView();
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void FContracts_Activated(object sender, EventArgs e)
        {
            LoadContractsDataGridView();

            GlobalProcedures.GridRestoreLayout(ContractsGridView, ContractRibbonPage.Text);
            GlobalProcedures.GridRestoreLayout(HostageGridView, "Satıcı");
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
                GlobalProcedures.FillCheckedComboBox(CreditNameComboBox, "CREDIT_NAMES", "NAME,NAME_EN,NAME_RU", null);
                GlobalProcedures.CalcEditFormat(FromAmountValue);
                GlobalProcedures.CalcEditFormat(ToAmountValue);
                SearchDockPanel.Show();
            }
            else
                SearchDockPanel.Hide();
        }

        private void ContractsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Contract_Notes)
            {
                if (ContractsGridView.GetListSourceRowCellValue(rowIndex, "CONTRACT_NOTE").ToString().Length > 0)
                    e.Value = Properties.Resources.notes_16;
                else
                    e.Value = null;
            }

            if (e.Column == Contract_SpecialAttention)
            {
                if (int.Parse(ContractsGridView.GetListSourceRowCellValue(rowIndex, "IS_SPECIAL_ATTENTION").ToString()) == 1)
                    e.Value = Properties.Resources.attention_161;
                else
                    e.Value = null;
            }
        }

        private void ToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            GridHitInfo hitInfo = ContractsGridView.CalcHitInfo(e.ControlMousePosition);
            DataRow drCurrentRow = ContractsGridView.GetDataRow(hitInfo.RowHandle);
            if (drCurrentRow == null)
                return;

            if (String.IsNullOrEmpty(drCurrentRow["CONTRACT_NOTE"].ToString()))
                return;

            if (e.SelectedControl != ContractsGridControl)
                return;

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            if (hitInfo.Column != Contract_Notes)
                return;

            SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            toolTipArgs.AllowHtmlText = DefaultBoolean.True;

            toolTip = null;

            TitleText = drCurrentRow["CUSTOMERFULLNAME"].ToString();
            ToolTipCustomerID = drCurrentRow["ID"].ToString();
            toolTipArgs.Title.Text = "<color=255,0,0>" + TitleText + "</color>";
            toolTipArgs.Contents.Text = drCurrentRow["CONTRACT_NOTE"].ToString();
            toolTipArgs.Contents.Image = Properties.Resources.notes_32;

            toolTip = "<i>Ən son dəyişiklik etmiş istifadəçi</i> : <b><color=104,6,6>" + drCurrentRow["NOTE_CHANGE_USER"].ToString() + "</color></b>\n" +
                        "<i>Dəyişilmə vaxtı</i> : <b><color=104,6,6>" + drCurrentRow["NOTE_CHANGE_DATE"].ToString() + "</color></b>";
            toolTipArgs.Footer.Text = toolTip;
            toolTipArgs.ShowFooterSeparator = true;


            e.Info = new ToolTipControlInfo();
            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
            e.Info.ToolTipType = ToolTipType.SuperTip;
            e.Info.SuperTip = new SuperToolTip();
            e.Info.SuperTip.Setup(toolTipArgs);
        }

        private void NoteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (status_id == 5)
            {
                old_row_num = ContractsGridView.FocusedRowHandle;
                topindex = ContractsGridView.TopRowIndex;
                Forms.Contracts.FNote fn = new FNote();
                fn.CustomerName = CustomerFullName;
                fn.ContractCode = ContractCode;
                fn.CustomerID = CustomerID;
                fn.ContractID = ContractID;
                fn.RefreshContractDataGridView += new Forms.Contracts.FNote.DoEvent(RefreshContracts);
                fn.ShowDialog();
                ContractsGridView.FocusedRowHandle = old_row_num;
                ContractsGridView.TopRowIndex = topindex;
            }
            else
                XtraMessageBox.Show(ContractCode + " saylı lizinq müqaviləsi bağlanıldığı üçün bu müqaviləyə qeyd yazmaq olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FilterContracts()
        {
            if (FormStatus)
            {
                ColumnView view = ContractsGridView;
                //CustomerName
                if (!String.IsNullOrEmpty(CustomerNameText.Text))
                    view.ActiveFilter.Add(view.Columns["CUSTOMERFULLNAME"],
                  new ColumnFilterInfo("[CUSTOMERFULLNAME] Like '%" + CustomerNameText.Text.Trim() + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CUSTOMERFULLNAME"]);

                //ContractCode
                if (!String.IsNullOrEmpty(ContractCodeText.Text))
                    view.ActiveFilter.Add(view.Columns["CONTRACT_CODE"],
                        new ColumnFilterInfo("[CONTRACT_CODE] Like '%" + ContractCodeText.Text.Trim() + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_CODE"]);

                //Interest
                if (InterestValue.Value > 0)
                    view.ActiveFilter.Add(view.Columns["INT_INTEREST"],
                        new ColumnFilterInfo("[INT_INTEREST] = " + InterestValue.Value, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INT_INTEREST"]);

                //Period
                if (PeriodValue.Value > 0)
                    view.ActiveFilter.Add(view.Columns["INT_PERIOD"],
                        new ColumnFilterInfo("[INT_PERIOD] = " + PeriodValue.Value, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INT_PERIOD"]);

                //Currency
                if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                        new ColumnFilterInfo(currency_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);

                //CreditName
                if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CREDIT_NAME"],
                        new ColumnFilterInfo(credit_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CREDIT_NAME"]);

                //Amount
                if (!String.IsNullOrEmpty(FromAmountValue.Text) && !String.IsNullOrEmpty(ToAmountValue.Text))
                    view.ActiveFilter.Add(view.Columns["AMOUNT"],
                        new ColumnFilterInfo("[AMOUNT] >= " + FromAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + " AND [AMOUNT] <=" + ToAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN), ""));
                else
                    view.ActiveFilter.Remove(view.Columns["AMOUNT"]);

                //Note
                if (!String.IsNullOrEmpty(NoteText.Text))
                    view.ActiveFilter.Add(view.Columns["NOTES"],
                  new ColumnFilterInfo("[NOTES] Like '%" + NoteText.Text.Trim() + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["NOTES"]);

                //Commitment
                if (!String.IsNullOrEmpty(CommitmentText.Text))
                    view.ActiveFilter.Add(view.Columns["COMMITMENT"],
                  new ColumnFilterInfo("[COMMITMENT] Like '%" + CommitmentText.Text.Trim() + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["COMMITMENT"]);

                //StartDate
                if (!String.IsNullOrEmpty(FromDateValue.Text) && !String.IsNullOrEmpty(ToDateValue.Text))
                    view.ActiveFilter.Add(view.Columns["START_DATE"],
                  new ColumnFilterInfo("[START_DATE] >= '" + FromDateValue.DateTime.ToString("yyyyMMdd") + "' AND [START_DATE] <= '" + ToDateValue.DateTime.ToString("yyyyMMdd") + "'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["START_DATE"]);

                //CarNumber
                if (!String.IsNullOrEmpty(CarNumberText.Text))
                    view.ActiveFilter.Add(view.Columns["CAR_NUMBER"],
                  new ColumnFilterInfo("[CAR_NUMBER] Like '%" + CarNumberText.Text.Trim() + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CAR_NUMBER"]);

                //Entend Period
                if (FromEntendMountCount.Value >= 0 && ToEntendMountCount.Value > 0)
                    view.ActiveFilter.Add(view.Columns["INT_EXTEND_MOUNT_COUNT"],
                        new ColumnFilterInfo("[INT_EXTEND_MOUNT_COUNT] >= " + FromEntendMountCount.Value + " AND [INT_EXTEND_MOUNT_COUNT] <= " + ToEntendMountCount.Value, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INT_EXTEND_MOUNT_COUNT"]);

                //Nezaret
                if (SpecialAttentionCheck.Checked && !NotSpecialAttentionCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_SPECIAL_ATTENTION"],
                  new ColumnFilterInfo("[IS_SPECIAL_ATTENTION] = 1", ""));
                else if (!SpecialAttentionCheck.Checked && NotSpecialAttentionCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_SPECIAL_ATTENTION"],
                  new ColumnFilterInfo("[IS_SPECIAL_ATTENTION] = 0", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["IS_SPECIAL_ATTENTION"]);
            }
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void CustomerNameText_EditValueChanged(object sender, EventArgs e)
        {
            FilterContracts();
        }

        private void CreditNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            credit_name = " [CREDIT_NAME] IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            FilterContracts();
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            InterestValue.Value =
                PeriodValue.Value = 0;
            CustomerNameText.Text =
                ContractCodeText.Text =
                CurrencyComboBox.Text =
                CreditNameComboBox.Text =
                FromDateValue.Text =
                ToDateValue.Text =
                FromAmountValue.Text =
                ToAmountValue.Text =
                NoteText.Text =
                CommitmentText.Text =
                CarNumberText.Text = null;
            ContractsGridView.ClearColumnsFilter();
        }

        private void ContractsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (ContractsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteContractBarButton.Enabled = GlobalVariables.DeleteContract;
                    EditContractBarButton.Enabled = GlobalVariables.EditContract;
                    AgreementBarButton.Enabled = GlobalVariables.AddAgreement;
                }
                else
                    DeleteContractBarButton.Enabled = EditContractBarButton.Enabled = AgreementBarButton.Enabled = true;

                NoteBarButton.Enabled = SpecialInfoExportBarButton.Enabled = PaymentBarButton.Enabled = true;
            }
            else
                DeleteContractBarButton.Enabled =
                    EditContractBarButton.Enabled =
                    NoteBarButton.Enabled =
                    SpecialInfoExportBarButton.Enabled =
                    OpenContractBarButton.Enabled =
                    CloseContractBarButton.Enabled =
                    AgreementBarButton.Enabled =
                    PaymentBarButton.Enabled = false;
        }

        private void SpecialInfoExportBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            int cell_number = 0, power = 0, insurance = 0;
            string sql = null;
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractShowWait));

            try
            {
                Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    XtraMessageBox.Show("Excel yüklənilməyib!!");
                    return;
                }


                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.get_Range("A1", "B1").Merge(true);
                xlWorkSheet.get_Range("A1", "A1").Font.Bold = true;
                xlWorkSheet.get_Range("A1", "A1").Font.Color = Color.Red;
                xlWorkSheet.get_Range("A1", "A1").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A1", "A1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("A4", "A45").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A4", "A45").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                xlWorkSheet.get_Range("B4", "B300").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B4", "B300").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                xlWorkSheet.get_Range("B4", "B300").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Lizinqalan";
                xlWorkSheet.get_Range("A3", "B3").Merge(true);
                xlWorkSheet.get_Range("A3", "A3").Font.Bold = true;
                xlWorkSheet.get_Range("A3", "A3").Font.Color = Color.Blue;
                xlWorkSheet.get_Range("A3", "A3").Interior.Color = Color.Yellow;
                xlWorkSheet.get_Range("A3", "A3").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "A3").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[11, 1] = "Satıcı";
                xlWorkSheet.get_Range("A11", "B11").Merge(true);
                xlWorkSheet.get_Range("A11", "A11").Font.Bold = true;
                xlWorkSheet.get_Range("A11", "A11").Font.Color = Color.Blue;
                xlWorkSheet.get_Range("A11", "A11").Interior.Color = Color.Yellow;
                xlWorkSheet.get_Range("A11", "A11").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A11", "A11").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[19, 1] = "Müqavilənin rekvizitləri";
                xlWorkSheet.get_Range("A19", "B19").Merge(true);
                xlWorkSheet.get_Range("A19", "A19").Font.Bold = true;
                xlWorkSheet.get_Range("A19", "A19").Font.Color = Color.Blue;
                xlWorkSheet.get_Range("A19", "A19").Interior.Color = Color.Yellow;
                xlWorkSheet.get_Range("A19", "A19").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A19", "A19").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[33, 1] = "Lizinqin predmeti";
                xlWorkSheet.get_Range("A33", "B33").Merge(true);
                xlWorkSheet.get_Range("A33", "A33").Font.Bold = true;
                xlWorkSheet.get_Range("A33", "A33").Font.Color = Color.Blue;
                xlWorkSheet.get_Range("A33", "A33").Interior.Color = Color.Yellow;
                xlWorkSheet.get_Range("A33", "A33").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A33", "A33").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                //muqavile
                sql = $@"SELECT    CT.CODE
                                   || C.CODE
                                   || ' - '
                                   || CUS.CUSTOMER_NAME
                                   || ' - '
                                   || CN.NAME
                                      FULLNAME,
                                   CUS.FULLNAME CUS_NAME,
                                   CC.CARD_DESCRIPTION CARD,
                                   TO_CHAR (CC.ISSUE_DATE, 'DD.MM.YYYY'),
                                   CC.CARD_ISSUING_NAME,
                                   CC.REGISTRATION_ADDRESS,
                                   CC.ADDRESS,
                                   PH.PHONE,
                                   CT.CODE || C.CODE CONTRACT_CODE,
                                   CN.NAME CREDIT_NAME,
                                   TO_CHAR (C.START_DATE, 'DD.MM.YYYY') S_DATE,
                                   TO_CHAR (C.END_DATE, 'DD.MM.YYYY') E_DATE,
                                   CT.TERM,
                                   CT.INTEREST,
                                   C.GRACE_PERIOD,
                                   C.CUSTOMER_ACCOUNT,
                                   DECODE (C.PAYMENT_TYPE, 0, 'Annutet', 'Fərdi') PAYMENT_TYPE,
                                   C.AMOUNT,
                                   CUR.CODE,
                                   C.CURRENCY_RATE,                                   
                                   C.MONTHLY_AMOUNT || ' ' || CUR.CODE
                              FROM CRS_USER.CONTRACTS C,
                                   CRS_USER.CREDIT_TYPE CT,
                                   CRS_USER.CREDIT_NAMES CN,
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER.CURRENCY CUR,
                                   CRS_USER.V_CUSTOMER_CARDS_DETAILS CC,       
                                   (SELECT * FROM CRS_USER.V_PHONE WHERE OWNER_TYPE IN ('C', 'JP')) PH
                             WHERE     C.CUSTOMER_ID = CUS.ID
                                   AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND C.CUSTOMER_CARDS_ID = CC.CARD_ID
                                   AND CUS.ID = CC.CUSTOMER_ID 
                                   AND CUS.PERSON_TYPE_ID = CC.PERSON_TYPE_ID      
                                   AND C.CREDIT_TYPE_ID = CT.ID
                                   AND CT.NAME_ID = CN.ID
                                   AND CUS.ID = PH.OWNER_ID(+)  
                                   AND DECODE(C.CUSTOMER_TYPE_ID,1,'C',2,'JP') = PH.OWNER_TYPE                                 
                                   AND C.CURRENCY_ID = CUR.ID
                                   AND C.ID = {ContractID}";
                DataTable dt_contract = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                foreach (DataRow dr in dt_contract.Rows)
                {
                    xlWorkSheet.Cells[1, 1] = dr[0];
                    xlWorkSheet.Cells[4, 1] = "Tam adı";
                    xlWorkSheet.Cells[4, 2] = dr[1];
                    xlWorkSheet.Cells[5, 1] = "Sənəd";
                    xlWorkSheet.Cells[5, 2] = dr[2];
                    xlWorkSheet.Cells[6, 1] = "Verilmə tarixi";
                    xlWorkSheet.Cells[6, 2] = dr[3];
                    xlWorkSheet.Cells[7, 1] = "Verən orqan";
                    xlWorkSheet.Cells[7, 2] = dr[4];
                    xlWorkSheet.Cells[8, 1] = "Qeydiyyat ünvanı";
                    xlWorkSheet.Cells[8, 2] = dr[5];
                    xlWorkSheet.Cells[9, 1] = "Hal-hazırki ünvanı";
                    xlWorkSheet.Cells[9, 2] = dr[6];
                    xlWorkSheet.Cells[10, 1] = "Telefonlar";
                    xlWorkSheet.Cells[10, 2] = dr[7];
                    xlWorkSheet.Cells[10, 2].NumberFormat = "#";
                    xlWorkSheet.Cells[20, 1] = "Müqavilə nömrəsi";
                    xlWorkSheet.Cells[20, 2] = dr[8];
                    xlWorkSheet.Cells[21, 1] = "Lizinqin növü";
                    xlWorkSheet.Cells[21, 2] = dr[9];
                    xlWorkSheet.Cells[22, 1] = "Başlanma tarixi";
                    xlWorkSheet.Cells[22, 2] = dr[10];
                    xlWorkSheet.Cells[23, 1] = "Bitmə tarixi";
                    xlWorkSheet.Cells[23, 2] = dr[11];
                    xlWorkSheet.Cells[24, 1] = "Müddəti";
                    xlWorkSheet.Cells[24, 2] = dr[12];
                    xlWorkSheet.Cells[25, 1] = "İllik faiz";
                    xlWorkSheet.Cells[25, 2] = dr[13];
                    xlWorkSheet.Cells[26, 1] = "Güzəşt müddəti";
                    xlWorkSheet.Cells[26, 2] = dr[14];
                    xlWorkSheet.Cells[27, 1] = "Müştəri hesabı";
                    xlWorkSheet.Cells[27, 2] = dr[15];
                    xlWorkSheet.Cells[28, 1] = "Ödəniş növü";
                    xlWorkSheet.Cells[28, 2] = dr[16];
                    xlWorkSheet.Cells[29, 1] = "Məbləğ";
                    xlWorkSheet.Cells[29, 2] = dr[17];
                    xlWorkSheet.Cells[30, 1] = "Valuta";
                    xlWorkSheet.Cells[30, 2] = dr[18];
                    xlWorkSheet.Cells[31, 1] = "Kurs";
                    xlWorkSheet.Cells[31, 2] = dr[19];
                    xlWorkSheet.Cells[32, 1] = "Aylıq ödəniş";
                    xlWorkSheet.Cells[32, 2] = dr[20];
                }
                //satici
                if (seller_type_id == 1)
                    sql = $@"SELECT S.SURNAME || ' ' || S.NAME || ' ' || S.PATRONYMIC,
                                   CS.SERIES || ' ' || S.CARD_NUMBER,
                                   TO_CHAR (S.CARD_ISSUE_DATE, 'DD.MM.YYYY'),
                                   CI.NAME,
                                   S.REGISTRATION_ADDRESS,
                                   S.ADDRESS,
                                   PH.PHONE
                              FROM CRS_USER.SELLERS S,
                                   CRS_USER.CONTRACTS C,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI,
                                   (SELECT * FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = 'S') PH
                             WHERE     S.CARD_SERIES_ID = CS.ID
                                   AND S.CARD_ISSUING_ID = CI.ID
                                   AND S.ID = PH.OWNER_ID(+)                                   
                                   AND S.ID = C.SELLER_ID
                                   AND C.SELLER_TYPE_ID = 1
                                   AND C.ID = {ContractID}";
                else
                    sql = $@"SELECT J.NAME,
                                   'Vöen: ' || J.VOEN,
                                   NULL ISSUE_DATE,
                                   NULL ISSUE_NAME,
                                   NULL REGISTRATION_ADDRESS,
                                   J.ADDRESS,
                                   PH.PHONE
                              FROM CRS_USER.JURIDICAL_PERSONS J,
                                   (SELECT * FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = 'JP') PH,
                                   CRS_USER.CONTRACTS C
                             WHERE     J.ID = PH.OWNER_ID(+)                                  
                                   AND J.ID = C.SELLER_ID
                                   AND C.SELLER_TYPE_ID = 2
                                   AND C.ID = {ContractID}";
                DataTable dt_seller = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                foreach (DataRow dr in dt_seller.Rows)
                {
                    xlWorkSheet.Cells[12, 1] = "Tam adı";
                    xlWorkSheet.Cells[12, 2] = dr[0];
                    xlWorkSheet.Cells[13, 1] = "Sənəd";
                    xlWorkSheet.Cells[13, 2] = dr[1];
                    xlWorkSheet.Cells[14, 1] = "Verilmə tarixi";
                    xlWorkSheet.Cells[14, 2] = dr[2];
                    xlWorkSheet.Cells[15, 1] = "Verən orqan";
                    xlWorkSheet.Cells[15, 2] = dr[3];
                    xlWorkSheet.Cells[16, 1] = "Qeydiyyat ünvanı";
                    xlWorkSheet.Cells[16, 2] = dr[4];
                    xlWorkSheet.Cells[17, 1] = "Yaşadığı ünvan";
                    xlWorkSheet.Cells[17, 2] = dr[5];
                    xlWorkSheet.Cells[18, 1] = "Telefonlar";
                    xlWorkSheet.Cells[18, 2] = dr[6];
                    xlWorkSheet.Cells[18, 2].NumberFormat = "#";
                }

                if (credit_name_id == 5)
                {
                    sql = "SELECT HO.ADDRESS,HO.EXCERPT,HO.AREA,HO.LIQUID_AMOUNT||' '||CUR.CODE,HO.FIRST_PAYMENT||' '||CUR.CODE FROM CRS_USER.HOSTAGE_OBJECT HO,CRS_USER.CURRENCY CUR WHERE HO.CURRENCY_ID = CUR.ID AND HO.CONTRACT_ID = " + ContractID;
                    DataTable dt_home = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                    foreach (DataRow dr in dt_home.Rows)
                    {
                        xlWorkSheet.Cells[34, 1] = "Ünvanı";
                        xlWorkSheet.Cells[34, 2] = dr[0];
                        xlWorkSheet.Cells[35, 1] = "Çıxarış";
                        xlWorkSheet.Cells[35, 2] = dr[1];
                        xlWorkSheet.Cells[36, 1] = "Sahəsi";
                        xlWorkSheet.Cells[36, 2] = dr[2] + " m²";
                        xlWorkSheet.Cells[37, 1] = "Likvid dəyəri";
                        xlWorkSheet.Cells[37, 2] = dr[3];
                        xlWorkSheet.Cells[38, 1] = "İlkin ödənişi";
                        xlWorkSheet.Cells[38, 2] = dr[4];
                    }
                }
                else if (credit_name_id == 1)
                {
                    sql = "SELECT CB.NAME,CM.NAME,CT.NAME,CBT.NAME,HC.YEAR,CC.NAME,HC.BAN,HC.CAR_NUMBER,HC.MILAGE,HC.ENGINE,HC.ENGINE_NUMBER,HC.CHASSIS_NUMBER,HC.LIQUID_AMOUNT||' '||CUR.CODE,HC.FIRST_PAYMENT||' '||CUR.CODE FROM CRS_USER.HOSTAGE_CAR HC,CRS_USER.CAR_BRANDS CB,CRS_USER.CAR_MODELS CM,CRS_USER.CAR_TYPES CT,CRS_USER.CAR_COLORS CC,CRS_USER.CAR_BAN_TYPES CBT,CRS_USER.CURRENCY CUR WHERE HC.BRAND_ID = CB.ID AND HC.MODEL_ID = CM.ID AND HC.TYPE_ID = CT.ID AND HC.BAN_TYPE_ID = CBT.ID AND HC.COLOR_ID = CC.ID AND HC.CURRENCY_ID = CUR.ID AND HC.CONTRACT_ID = " + ContractID;
                    DataTable dt_car = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                    foreach (DataRow dr in dt_car.Rows)
                    {
                        xlWorkSheet.Cells[34, 1] = "Marka";
                        xlWorkSheet.Cells[34, 2] = dr[0];
                        xlWorkSheet.Cells[35, 1] = "Model";
                        xlWorkSheet.Cells[35, 2] = dr[1];
                        xlWorkSheet.Cells[36, 1] = "Tip";
                        xlWorkSheet.Cells[36, 2] = dr[2];
                        xlWorkSheet.Cells[37, 1] = "Ban tipi";
                        xlWorkSheet.Cells[37, 2] = dr[3];
                        xlWorkSheet.Cells[38, 1] = "İl";
                        xlWorkSheet.Cells[38, 2] = dr[4];
                        xlWorkSheet.Cells[39, 1] = "Rəngi";
                        xlWorkSheet.Cells[39, 2] = dr[5];
                        xlWorkSheet.Cells[40, 1] = "Ban";
                        xlWorkSheet.Cells[40, 2] = dr[6];
                        xlWorkSheet.Cells[41, 1] = "Qeydiyyat nişanı";
                        xlWorkSheet.Cells[41, 2] = dr[7];
                        xlWorkSheet.Cells[42, 1] = "Yürüşü";
                        xlWorkSheet.Cells[42, 2] = dr[8] + " km";
                        xlWorkSheet.Cells[43, 1] = "Mühərrikin həcmi";
                        xlWorkSheet.Cells[43, 2] = dr[9] + " sm³";
                        xlWorkSheet.Cells[44, 1] = "Mühərrikin nömrəsi";
                        xlWorkSheet.Cells[44, 2] = dr[10];
                        xlWorkSheet.Cells[44, 2].NumberFormat = "#";
                        xlWorkSheet.Cells[45, 1] = "Şassi nömrəsi";
                        xlWorkSheet.Cells[45, 2] = dr[11];
                        xlWorkSheet.Cells[46, 1] = "Likvid dəyəri";
                        xlWorkSheet.Cells[46, 2] = dr[12];
                        xlWorkSheet.Cells[47, 1] = "İlkin ödənişi";
                        xlWorkSheet.Cells[47, 2] = dr[13];
                    }
                }

                //OHDELIKLER
                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_COMMITMENTS WHERE CONTRACT_ID = " + ContractID) > 0)
                {
                    if (credit_name_id == 1)
                    {
                        xlWorkSheet.Cells[48, 1] = "Öhdəlik götürənlər";
                        xlWorkSheet.get_Range("A48", "B48").Merge(true);
                        xlWorkSheet.get_Range("A48", "A48").Font.Bold = true;
                        xlWorkSheet.get_Range("A48", "A48").Font.Color = Color.Blue;
                        xlWorkSheet.get_Range("A48", "A48").Interior.Color = Color.Yellow;
                        xlWorkSheet.get_Range("A48", "A48").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        xlWorkSheet.get_Range("A48", "A48").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        cell_number = 49;
                    }
                    else if (credit_name_id == 5)
                    {
                        xlWorkSheet.Cells[39, 1] = "Öhdəlik götürənlər";
                        xlWorkSheet.get_Range("A39", "B39").Merge(true);
                        xlWorkSheet.get_Range("A39", "A39").Font.Bold = true;
                        xlWorkSheet.get_Range("A39", "A39").Font.Color = Color.Blue;
                        xlWorkSheet.get_Range("A39", "A39").Interior.Color = Color.Yellow;
                        xlWorkSheet.get_Range("A39", "A39").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        xlWorkSheet.get_Range("A39", "A39").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        cell_number = 40;
                    }

                    sql = $@"SELECT CC.COMMITMENT_NAME,
                                     CS.SERIES || ' ' || CC.CARD_NUMBER CARD,
                                     TO_CHAR (CC.CARD_ISSUE_DATE, 'DD.MM.YYYY') CARD_ISSUE_DATE,
                                     CI.NAME,
                                     CC.ADDRESS,
                                     PH.PHONE,
                                     TO_CHAR (CC.AGREEMENTDATE, 'DD.MM.YYYY') AGREEMENTDATE,
                                     CC.DEBT || ' ' || CUR.CODE DEBT,
                                     TO_CHAR (CC.PERIOD_DATE, 'DD.MM.YYYY') PERIOD_DATE,
                                     CC.INTEREST,
                                     CC.ADVANCE_PAYMENT || ' ' || CUR.CODE ADVANCE_PAYMENT,
                                     CC.SERVICE_AMOUNT || ' ' || CUR.CODE SERVICE_AMOUNT
                                FROM CRS_USER.CURRENCY CUR,
                                     CRS_USER.CONTRACT_COMMITMENTS CC,
                                     CRS_USER.CARD_SERIES CS,
                                     CRS_USER.CARD_ISSUING CI,
                                     (SELECT *
                                        FROM CRS_USER.V_PHONE
                                       WHERE OWNER_TYPE = 'CC') PH
                               WHERE     CC.CARD_SERIES_ID = CS.ID
                                     AND CC.CARD_ISSUING_ID = CI.ID
                                     AND CC.ID = PH.OWNER_ID(+)
                                     AND CC.CURRENCY_ID = CUR.ID
                                     AND CC.CONTRACT_ID = {ContractID}
                            ORDER BY CC.ID";
                    DataTable dt_commitment = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                    foreach (DataRow dr in dt_commitment.Rows)
                    {
                        xlWorkSheet.Cells[cell_number, 1] = "Soyadı, adı və atasının adı";
                        xlWorkSheet.Cells[cell_number, 2] = dr[0];
                        xlWorkSheet.Cells[cell_number + 1, 1] = "Sənəd";
                        xlWorkSheet.Cells[cell_number + 1, 2] = dr[1];
                        xlWorkSheet.Cells[cell_number + 2, 1] = "Verilmə tarixi";
                        xlWorkSheet.Cells[cell_number + 2, 2] = dr[2];
                        xlWorkSheet.Cells[cell_number + 3, 1] = "Verən orqan";
                        xlWorkSheet.Cells[cell_number + 3, 2] = dr[3];
                        xlWorkSheet.Cells[cell_number + 4, 1] = "Qeydiyyat ünvanı";
                        xlWorkSheet.Cells[cell_number + 4, 2] = dr[4];
                        xlWorkSheet.Cells[cell_number + 5, 1] = "Telefonlar";
                        xlWorkSheet.Cells[cell_number + 5, 2] = dr[5];
                        xlWorkSheet.Cells[cell_number + 5, 2].NumberFormat = "#";
                        xlWorkSheet.Cells[cell_number + 6, 1] = "Razılaşmanın tarixi";
                        xlWorkSheet.Cells[cell_number + 6, 2] = dr[6];
                        xlWorkSheet.Cells[cell_number + 7, 1] = "Qalıq borc";
                        xlWorkSheet.Cells[cell_number + 7, 2] = dr[7];
                        xlWorkSheet.Cells[cell_number + 8, 1] = "Lizinq müddəti";
                        xlWorkSheet.Cells[cell_number + 8, 2] = dr[8];
                        xlWorkSheet.Cells[cell_number + 9, 1] = "Lizinq verənin mükafatı";
                        xlWorkSheet.Cells[cell_number + 9, 2] = dr[9];
                        xlWorkSheet.Cells[cell_number + 10, 1] = "Avans ödənişi";
                        xlWorkSheet.Cells[cell_number + 10, 2] = dr[10];
                        xlWorkSheet.Cells[cell_number + 11, 1] = "Xidmət haqqı";
                        xlWorkSheet.Cells[cell_number + 11, 2] = dr[11];
                        cell_number = cell_number + 13;
                    }

                }
                else
                    cell_number = 49;

                //ETIBARNAMELER
                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.POWER_OF_ATTORNEY WHERE CONTRACT_ID = " + ContractID) > 0)
                {
                    if (credit_name_id == 1 && cell_number > 0)
                    {
                        string A = "A" + (cell_number - 1).ToString(), B = "B" + (cell_number - 1).ToString();
                        xlWorkSheet.Cells[cell_number - 1, 1] = "Etibarnamələr";
                        xlWorkSheet.get_Range(A, B).Merge(true);
                        xlWorkSheet.get_Range(A, A).Font.Bold = true;
                        xlWorkSheet.get_Range(A, A).Font.Color = Color.Blue;
                        xlWorkSheet.get_Range(A, A).Interior.Color = Color.Yellow;
                        xlWorkSheet.get_Range(A, A).VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        xlWorkSheet.get_Range(A, A).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        power = cell_number;
                    }
                    //else if (credit_name_id == 1 && cell_number == 0)
                    //{
                    //    xlWorkSheet.Cells[46, 1] = "Etibarnamələr";
                    //    xlWorkSheet.get_Range("A46", "B46").Merge(true);
                    //    xlWorkSheet.get_Range("A46", "A46").Font.Bold = true;
                    //    xlWorkSheet.get_Range("A46", "A46").Font.Color = Color.Blue;
                    //    xlWorkSheet.get_Range("A46", "A46").Interior.Color = Color.Yellow;
                    //    xlWorkSheet.get_Range("A46", "A46").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    //    xlWorkSheet.get_Range("A46", "A46").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //    power = 47;
                    //}

                    sql = "SELECT P.FULLNAME,CS.SERIES||' '||P.CARD_NUMBER,TO_CHAR(P.CARD_ISSUE_DATE,'DD.MM.YYYY'),CI.NAME,TO_CHAR(P.POWER_DATE,'DD.MM.YYYY') FROM CRS_USER.POWER_OF_ATTORNEY P,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE P.CARD_SERIES_ID = CS.ID AND P.CARD_ISSUING_ID = CI.ID AND P.CONTRACT_ID = " + ContractID + " ORDER BY P.ID";
                    DataTable dt_power = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                    foreach (DataRow dr in dt_power.Rows)
                    {
                        xlWorkSheet.Cells[power, 1] = "Soyadı, adı və atasının adı";
                        xlWorkSheet.Cells[power, 2] = dr[0];
                        xlWorkSheet.Cells[power + 1, 1] = "Sənəd";
                        xlWorkSheet.Cells[power + 1, 2] = dr[1];
                        xlWorkSheet.Cells[power + 2, 1] = "Verilmə tarixi";
                        xlWorkSheet.Cells[power + 2, 2] = dr[2];
                        xlWorkSheet.Cells[power + 3, 1] = "Verən orqan";
                        xlWorkSheet.Cells[power + 3, 2] = dr[3];
                        xlWorkSheet.Cells[power + 4, 1] = "Qüvvədə olma müddəti";
                        xlWorkSheet.Cells[power + 4, 2] = dr[4];
                        power = power + 6;
                    }
                }
                else
                    power = cell_number;

                //SIGORTA
                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.INSURANCES WHERE CONTRACT_ID = " + ContractID) > 0)
                {
                    if (credit_name_id == 1 && power > 0)
                    {
                        string A = "A" + (power - 1).ToString(), B = "B" + (power - 1).ToString();
                        xlWorkSheet.Cells[power - 1, 1] = "Sığorta";
                        xlWorkSheet.get_Range(A, B).Merge(true);
                        xlWorkSheet.get_Range(A, A).Font.Bold = true;
                        xlWorkSheet.get_Range(A, A).Font.Color = Color.Blue;
                        xlWorkSheet.get_Range(A, A).Interior.Color = Color.Yellow;
                        xlWorkSheet.get_Range(A, A).VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        xlWorkSheet.get_Range(A, A).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        insurance = power;
                    }
                    else if (credit_name_id == 1 && power == 0)
                    {
                        xlWorkSheet.Cells[46, 1] = "Sığorta";
                        xlWorkSheet.get_Range("A46", "B46").Merge(true);
                        xlWorkSheet.get_Range("A46", "A46").Font.Bold = true;
                        xlWorkSheet.get_Range("A46", "A46").Font.Color = Color.Blue;
                        xlWorkSheet.get_Range("A46", "A46").Interior.Color = Color.Yellow;
                        xlWorkSheet.get_Range("A46", "A46").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        xlWorkSheet.get_Range("A46", "A46").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        insurance = 47;
                    }

                    sql = "SELECT IC.NAME COMPANY_NAME,I.INSURANCE_AMOUNT||' '||C.CODE, I.INSURANCE_PERIOD||' ay',I.INSURANCE_INTEREST,I.UNCONDITIONAL_AMOUNT||' '||C.CODE,TO_CHAR(I.START_DATE,'DD.MM.YYYY'),TO_CHAR(I.END_DATE,'DD.MM.YYYY') FROM CRS_USER.INSURANCES I,CRS_USER.CURRENCY C,CRS_USER.INSURANCE_COMPANY IC WHERE I.COMPANY_ID = IC.ID AND I.CURRENCY_ID = C.ID AND I.CONTRACT_ID = " + ContractID;
                    DataTable dt_insurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SpecialInfoExportBarButton_ItemClick");

                    foreach (DataRow dr in dt_insurance.Rows)
                    {
                        xlWorkSheet.Cells[insurance, 1] = "Sığorta şirkəti";
                        xlWorkSheet.Cells[insurance, 2] = dr[0];
                        xlWorkSheet.Cells[insurance + 1, 1] = "Sığorta dəyəri";
                        xlWorkSheet.Cells[insurance + 1, 2] = dr[1];
                        xlWorkSheet.Cells[insurance + 2, 1] = "Sığorta müddəti";
                        xlWorkSheet.Cells[insurance + 2, 2] = dr[2];
                        xlWorkSheet.Cells[insurance + 3, 1] = "Sığorta dərəcəsi";
                        xlWorkSheet.Cells[insurance + 3, 2] = dr[3];
                        xlWorkSheet.Cells[insurance + 4, 1] = "Şərtsiz azadolma";
                        xlWorkSheet.Cells[insurance + 4, 2] = dr[4];
                        xlWorkSheet.Cells[insurance + 5, 1] = "Başlama tarixi";
                        xlWorkSheet.Cells[insurance + 5, 2] = dr[5];
                        xlWorkSheet.Cells[insurance + 6, 1] = "Bitmə tarixi";
                        xlWorkSheet.Cells[insurance + 6, 2] = dr[6];
                    }
                }

                xlWorkSheet.Columns.AutoFit();

                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls"))
                {
                    FileInfo info = new FileInfo(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls");
                    if (!GlobalFunctions.IsFileLocked(info))
                        File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls");
                    else
                        XtraMessageBox.Show(ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls faylı artıq yaradılıb və açıqdır.");
                }

                xlWorkBook.SaveAs(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                GlobalProcedures.SplashScreenClose();
                Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + " saylı müqaviləsinin xüsusi məlumatları.xls");


                GlobalProcedures.ReleaseObject(xlWorkSheet);
                GlobalProcedures.ReleaseObject(xlWorkBook);
                GlobalProcedures.ReleaseObject(xlApp);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.LogWrite("Müqavilənin xüsusi məlumatları excelə ixrac edilmədi.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LawShedulesBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FLawSchedules fls = new FLawSchedules();
            fls.ShowDialog();
        }
    }
}