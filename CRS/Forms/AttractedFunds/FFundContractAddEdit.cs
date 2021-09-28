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
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.Utils;

namespace CRS.Forms.AttractedFunds
{
    public partial class FFundContractAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FFundContractAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID;

        bool CurrentStatus = false,
            ContractUsed = false,
            FormStatus = false,
            ContractClosed = false;

        int ContractUsedUserID = -1,
            source_id,
            source_name_id,
            period = 14,
            currency_id,
            check_end_date = 0,
            bank_id,
            founder_id,
            founder_card_id,
            old_source_id,
            status_id,
            pay_count,
            percentID;

        double cur = 1;

        public delegate void DoEvent();
        public event DoEvent RefreshContractsDataGridView;

        private void FFundContractAddEdit_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddFundPercent;

            CurrencyRateValue.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            CurrencyRateValue.Properties.DisplayFormat.FormatString = "### ##0.0000";

            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                BankComboBox.Properties.Buttons[1].Visible = GlobalVariables.Bank;
                CurrencyLookUp.Properties.Buttons[1].Visible = GlobalVariables.Currency;
                FounderComboBox.Properties.Buttons[1].Visible = GlobalVariables.Founders;
            }

            ContractStartDate.Properties.MaxValue = DateTime.Today;
            GlobalProcedures.CalcEditFormat(AmountValue);
            RefreshDictionaries(7);
            RefreshFundsDictionaries(0);
            FundsSourceLookUp.ItemIndex = 0;

            if (TransactionName == "INSERT")
            {
                ContractStartDate.EditValue = DateTime.Today;
                ContractID = GlobalFunctions.GetOracleSequenceValue("FUNDS_CONTRACTS_SEQUENCE").ToString();
                LoadPercent();
            }
            else
            {
                StatusComboBox.Visible = StatusLabel.Visible = true;
                InsertPercentTemp();
                LoadPercent();
                GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (13, 14)");
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
                var fundContracts = FundContractsDAL.SelectFundContractByID(int.Parse(ContractID)).ToList<FundContracts>().First();
                ContractUsedUserID = fundContracts.USED_USER_ID;
                ContractUsed = (ContractUsedUserID > 0);
                ContractClosed = (fundContracts.STATUS_ID == 14);
                if (ContractClosed)
                    NewBarButton.Enabled = EditBarButton.Enabled = DeleteBarButton.Enabled = false;

                if (((ContractClosed) && (ContractUsed)) || ((ContractClosed) && (!ContractUsed)))
                {
                    XtraMessageBox.Show("Müqavilə sistemdə bağlanılıb. Bu müqavilənin məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else if ((!ContractClosed) && (ContractUsed))
                {
                    if (GlobalVariables.V_UserID != ContractUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ContractUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;

                if (!CurrentStatus)
                {
                    pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT CONTRACT_ID FROM CRS_USER.FUNDS_PAYMENTS UNION SELECT CONTRACT_ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP) WHERE CONTRACT_ID = " + ContractID);
                    if (pay_count == 0)
                        CurrentStatus = false;
                    else
                    {
                        XtraMessageBox.Show("Seçilmiş müqavilə üzrə ödənişlər olduğu üçün bu müqavilənin məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                }

                ComponentEnabled(CurrentStatus);
                LoadContractDetails();
            }
            FormStatus = true;
        }

        private void InsertPercentTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_FUNDS_PERCENT_TEMP", "P_CONTRACT_ID", ContractID, "İllik faizlər temp cədvələ daxil edilmədi.");
        }

        private void ComponentEnabled(bool status)
        {
            ContractNumberText.Enabled =
            RegistrationNumberText.Enabled =
            FundsSourceLookUp.Enabled =
            FundsSourceNameComboBox.Enabled =
            PeriodValue.Enabled =
            ContractStartDate.Enabled =
            ContractEndDate.Enabled =
            EndDateCheckEdit.Enabled =
            CurrencyRateValue.Enabled =
            AmountValue.Enabled =
            CurrencyLookUp.Enabled =
            NoteText.Enabled =
            StatusComboBox.Enabled =
            CloseDateValue.Enabled = !status;
        }

        private void LoadContractDetails()
        {
            string s = null, s1 = null, s2 = null, s3 = null;
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID FROM CRS_USER.FUNDS_PAYMENTS UNION SELECT CONTRACT_ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP) WHERE CONTRACT_ID = {ContractID}");

            FundsSourceLookUp.Enabled = (a == 0);

            try
            {
                s = $@"SELECT FC.CONTRACT_NUMBER,
                               FC.REGISTRATION_NUMBER,
                               FS.NAME SOURCE,
                               FC.INTEREST,
                               FC.PERIOD,
                               TO_CHAR (FC.START_DATE, 'DD/MM/YYYY'),
                               TO_CHAR (FC.END_DATE, 'DD/MM/YYYY'),
                               FC.AMOUNT,
                               C.CODE,
                               FC.NOTE,
                               FC.CHECK_END_DATE,
                               FC.FUNDS_SOURCE_ID,
                               FC.ID,
                               FC.FUNDS_SOURCE_NAME_ID,
                               S.STATUS_NAME,
                               S.STATUS_NAME_EN,
                               S.STATUS_NAME_RU,
                               FC.STATUS_ID,
                               TO_CHAR (FC.CLOSED_DATE, 'DD/MM/YYYY'),
                               FC.CURRENCY_RATE
                          FROM CRS_USER.FUNDS_CONTRACTS FC,
                               CRS_USER.FUNDS_SOURCES FS,
                               CRS_USER.CURRENCY C,
                               CRS_USER.STATUS S
                         WHERE     FC.STATUS_ID = S.ID
                               AND FC.CURRENCY_ID = C.ID
                               AND FC.FUNDS_SOURCE_ID = FS.ID
                               AND FC.ID = {ContractID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ContractNumberText.Text = dr[0].ToString();
                    RegistrationNumberText.Text = dr[1].ToString();
                    FundsSourceLookUp.EditValue = FundsSourceLookUp.Properties.GetKeyValueByDisplayText(dr[2].ToString());
                    old_source_id = Convert.ToInt32(dr[11].ToString());
                    if (old_source_id == 6)
                    {
                        s1 = $@"SELECT B.LONG_NAME FROM CRS_USER.BANKS B,CRS_USER.BANK_CONTRACTS BC WHERE BC.BANK_ID = B.ID AND BC.FUNDS_CONTRACT_ID = {dr[12].ToString()}";

                        BankComboBox.EditValue = GlobalFunctions.GenerateDataTable(s1, this.Name + "/LoadContractDetails").Rows[0]["LONG_NAME"];
                        BankComboBox.Enabled = !(a > 0);
                    }
                    else if (old_source_id == 10)
                    {
                        s2 = "SELECT F.FULLNAME,CS.SERIES||': '||FCR.CARD_NUMBER||', '||TO_CHAR(FCR.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib.' CARD FROM CRS_USER.FOUNDER_CONTRACTS FC,CRS_USER.FOUNDERS F,CRS_USER.FOUNDER_CARDS FCR,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.FOUNDER_ID = F.ID AND FC.FOUNDER_CARD_ID = FCR.ID AND FCR.CARD_SERIES_ID = CS.ID AND FCR.CARD_ISSUING_ID = CI.ID AND FCR.FOUNDER_ID = F.ID AND FC.FUNDS_CONTRACT_ID = " + dr[12].ToString();
                        DataTable dt2 = GlobalFunctions.GenerateDataTable(s2, this.Name + "/LoadContractDetails");
                        FounderComboBox.EditValue = dt2.Rows[0]["FULLNAME"];
                        CardText.EditValue = dt2.Rows[0]["CARD"];
                        FounderComboBox.Enabled = !(a > 0);
                    }
                    else
                    {
                        s3 = $@"SELECT NAME FROM CRS_USER.FUNDS_SOURCES_NAME FS WHERE ID = {dr["FUNDS_SOURCE_NAME_ID"]}";

                        FundsSourceNameComboBox.EditValue = GlobalFunctions.GenerateDataTable(s3, this.Name + "/LoadContractDetails").Rows[0]["NAME"];

                        FundsSourceNameComboBox.Enabled = !(a > 0);
                    }

                    if (Convert.ToInt32(dr[10].ToString()) == 1)
                    {
                        EndDateCheckEdit.Checked = true;
                        ContractEndDate.Enabled = !CurrentStatus;
                    }
                    else
                    {
                        EndDateCheckEdit.Checked = false;
                        ContractEndDate.Enabled = false;
                    }
                    PeriodValue.Value = Convert.ToInt32(dr[4].ToString());
                    period = Convert.ToInt32(dr[4].ToString());
                    ContractStartDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[5].ToString(), "ddmmyyyy");
                    ContractEndDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[6].ToString(), "ddmmyyyy");
                    AmountValue.Value = Convert.ToDecimal(dr[7].ToString());
                    CurrencyLookUp.EditValue = CurrencyLookUp.Properties.GetKeyValueByDisplayText(dr[8].ToString());
                    CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"].ToString());
                    NoteText.Text = dr[9].ToString();
                    status_id = Convert.ToInt32(dr[17].ToString());
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            StatusComboBox.EditValue = dr[14].ToString();
                            break;
                        case "EN":
                            StatusComboBox.EditValue = dr[15].ToString();
                            break;
                        case "RU":
                            StatusComboBox.EditValue = dr[16].ToString();
                            break;
                    }

                    if (status_id == 14)
                        StatusComboBox.Enabled = CloseDateValue.Enabled = BOK.Enabled = NoteText.Enabled = false;


                    if (!String.IsNullOrEmpty(dr[18].ToString()))
                        CloseDateValue.EditValue = GlobalFunctions.ChangeStringToDate(dr[18].ToString(), "ddmmyyyy");
                    else
                        CloseDateValue.EditValue = DateTime.Today;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FFundContractAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP WHERE USED_USER_ID = { GlobalVariables.V_UserID}", "Faizlər temp cədvəldən silinmədi.", this.Name + "/FFundContractAddEdit_FormClosing");
            this.RefreshContractsDataGridView();
        }

        private void FundsSourceNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            source_name_id = GlobalFunctions.FindComboBoxSelectedValue("FUNDS_SOURCES_NAME", "NAME", "ID", FundsSourceNameComboBox.Text);
        }

        private bool ControlContractDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(ContractNumberText.Text))
            {
                ContractNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin nömrəsi daxil edilməyib.");
                ContractNumberText.Focus();
                ContractNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FUNDS_CONTRACTS WHERE CONTRACT_NUMBER = '" + ContractNumberText.Text.Trim() + "'") > 0)
            {
                ContractNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractNumberText.Text + " növrəli müqavilə artıq daxil edilib. Müqavilənin nömrəsi təkrar ola bilməz.");
                ContractNumberText.Focus();
                ContractNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(RegistrationNumberText.Text))
            {
                RegistrationNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyat nömrəsi daxil edilməyib.");
                RegistrationNumberText.Focus();
                RegistrationNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (FundsSourceLookUp.EditValue == null)
            {
                FundsSourceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mənbə daxil edilməyib.");
                FundsSourceLookUp.Focus();
                FundsSourceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (source_id == 6 && String.IsNullOrWhiteSpace(BankComboBox.Text))
            {
                BankComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankComboBox.Focus();
                BankComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(FundsSourceNameComboBox.Text) && source_id != 6 && source_id != 10)
            {
                FundsSourceNameComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mənbəyin adı daxil edilməyib.");
                FundsSourceNameComboBox.Focus();
                FundsSourceNameComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(ContractStartDate.Text))
            {
                ContractStartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Başlama tarixi daxil edilməyib.");
                ContractStartDate.Focus();
                ContractStartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(ContractEndDate.Text))
            {
                ContractEndDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bitmə tarixi daxil edilməyib.");
                ContractEndDate.Focus();
                ContractEndDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PercentGridView.RowCount == 0)
            {
                GlobalProcedures.ShowErrorMessage("İllik faiz daxil edilməyib.");
                return false;
            }
            else
                b = true;

            if (CurrencyLookUp.EditValue == null)
            {
                CurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta daxil edilməyib.");
                CurrencyLookUp.Focus();
                CurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void ContractStartDate_EditValueChanged(object sender, EventArgs e)
        {
            CurrencyRate();
            if (!EndDateCheckEdit.Checked)
            {
                ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
                ContractEndDate.Properties.MinValue = ContractStartDate.DateTime;
            }
        }

        private void ContractEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ContractStartDate.Text) && !String.IsNullOrEmpty(ContractEndDate.Text) && FormStatus && EndDateCheckEdit.Checked)
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                PeriodValue.Value = diff_month;
            }
        }

        private void EndDateCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (EndDateCheckEdit.Checked)
            {
                check_end_date = 1;
                ContractEndDate.Enabled = !CurrentStatus;
            }
            else
            {
                check_end_date = 0;
                PeriodValue.Value = period;
                ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
                ContractEndDate.Enabled = false;
            }
            PeriodValue.Enabled = !EndDateCheckEdit.Checked;
        }

        void RefreshFundsDictionaries(int index)
        {
            switch (index)
            {
                case 0:
                    GlobalProcedures.FillLookUpEdit(FundsSourceLookUp, "FUNDS_SOURCES", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 1:
                    GlobalProcedures.FillComboBoxEditWithSqlText(FundsSourceNameComboBox, $@"SELECT NAME,NAME,NAME FROM CRS_USER.FUNDS_SOURCES_NAME WHERE SOURCE_ID = {source_id} ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index, int funds_index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.FundSelectedTabIndex = funds_index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshFundsDictionaries);
            fc.ShowDialog();
        }

        private void FundsSourceComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 14, 0);
        }

        private void FundsSourceNameComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 14, 1);
        }

        private void FundsSourceLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 14, 0);
        }

        private void FundsSourceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (FundsSourceLookUp.EditValue == null)
                return;

            source_id = Convert.ToInt32(FundsSourceLookUp.EditValue);

            if (source_id == 6)
            {
                GlobalProcedures.FillComboBoxEdit(BankComboBox, "BANKS", "LONG_NAME,LONG_NAME,LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                FundsSourceNameComboBox.Visible = FundsSourceNameLabel.Visible = false;

                BankNameLabel.Visible = BankComboBox.Visible = true;

                FounderComboBox.Visible = false;
                FounderLabel.Visible = false;
                CardLabel.Visible = false;
                CardText.Visible = false;
                BankComboBox.Location = new Point(155, 87);
                BankNameLabel.Location = new Point(12, 90);

            }
            else if (source_id == 10)
            {
                GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID");
                BankNameLabel.Visible = false;
                BankComboBox.Visible = false;

                FundsSourceNameComboBox.Visible = false;
                FundsSourceNameLabel.Visible = false;
                FounderComboBox.Visible = true;
                FounderLabel.Visible = true;
                CardLabel.Visible = true;
                CardText.Visible = true;
                FounderComboBox.Location = new Point(155, 87);
                FounderLabel.Location = new Point(12, 90);
                CardText.Location = new Point(155, 113);
                CardLabel.Location = new Point(12, 116);
            }
            else
            {
                GlobalProcedures.FillComboBoxEditWithSqlText(FundsSourceNameComboBox, "SELECT NAME,NAME,NAME FROM CRS_USER.FUNDS_SOURCES_NAME WHERE SOURCE_ID = " + source_id + " ORDER BY ORDER_ID");
                FundsSourceNameComboBox.Visible = true;
                FundsSourceNameLabel.Visible = true;
                BankNameLabel.Visible = false;
                BankComboBox.Visible = false;

                FounderComboBox.Visible = false;
                FounderLabel.Visible = false;
                CardLabel.Visible = false;
                CardText.Visible = false;
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void CurrencyRate()
        {
            if (currency_id > 1)
            {
                cur = GlobalFunctions.CurrencyLastRate(currency_id, ContractStartDate.Text);
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                CurrencyRateLabel.Text = "1 " + CurrencyLookUp.Text + " = ";
            }
            else
            {
                cur = 1;
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = false;
            }

            CurrencyRateValue.Value = (decimal)cur;
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(ContractStartDate.Text);
            CurrencyRate();
        }

        private void LoadPercent()
        {
            string sql = $@"SELECT 1 SS,
                                   ID,
                                   PDATE,
                                   PERCENT_VALUE || ' %' PERCENT_VALUE,
                                   PERCENT_VALUE INT_PERCENT_VALUE,
                                   NOTE
                              FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP
                             WHERE IS_CHANGE != 2 AND FUNDS_CONTRACTS_ID = {ContractID}
                            ORDER BY ID";

            PercentGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPercent", "Faizlər açılmadı.");
            if (PercentGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditFundPercent;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteFundPercent;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPercent();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPercentAddEdit("INSERT", null);
        }

        private void PercentGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PercentGridView.GetFocusedDataRow();
            if (row != null)
                percentID = int.Parse(row["ID"].ToString());
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPercentAddEdit("EDIT", percentID);
        }

        private void PercentGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFPercentAddEdit("EDIT", percentID);
        }

        private void PercentGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş faizi silmək istəyirsiniz?", "Faizin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP SET IS_CHANGE = 2 WHERE ID = {percentID}", "Faiz silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
            }
            LoadPercent();
        }

        private void PercentGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PercentGridView, PopupMenu, e);
        }

        private void PercentGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (PercentGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditFundPercent;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteFundPercent;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void LoadFPercentAddEdit(string transaction, int? id)
        {
            FPercentAddEdit fp = new FPercentAddEdit();
            fp.TransactionName = transaction;
            fp.ContractID = int.Parse(ContractID);
            fp.PercentID = id;
            fp.ContractStartDate = ContractStartDate.DateTime;
            fp.ContractEndDate = ContractEndDate.DateTime;
            fp.RefreshDataGridView += new FPercentAddEdit.DoEvent(LoadPercent);
            fp.ShowDialog();
        }

        private void CurrencyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7, null);
        }

        private void CurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CurrencyLookUp.EditValue == null)
                return;

            currency_id = Convert.ToInt32(CurrencyLookUp.EditValue);
            CurrencyRate();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 1:
                    GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (13, 14)");
                    break;
                case 7:
                    GlobalProcedures.FillLookUpEdit(CurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 11:
                    GlobalProcedures.FillComboBoxEdit(BankComboBox, "BANKS", "LONG_NAME,LONG_NAME,LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                    break;
                case 15:
                    GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index, string where)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.StatusWhere = where;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void InsertContract()
        {
            if (source_id == 6 || source_id == 10)
                source_name_id = 0;

            if (source_id == 6)
                GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.FUNDS_CONTRACTS(ID,FUNDS_SOURCE_ID,FUNDS_SOURCE_NAME_ID,CONTRACT_NUMBER,REGISTRATION_NUMBER,PERIOD,START_DATE,END_DATE,AMOUNT,CURRENCY_ID,CHECK_END_DATE,NOTE,STATUS_ID,CURRENCY_RATE)VALUES(" + ContractID + "," + source_id + "," + source_name_id + ",'" + ContractNumberText.Text.Trim() + "','" + RegistrationNumberText.Text.Trim() + "'," + PeriodValue.Value + ",TO_DATE('" + ContractStartDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ContractEndDate.Text + "','DD/MM/YYYY')," + AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + currency_id + "," + check_end_date + ",'" + NoteText.Text.Trim() + "',13," + CurrencyRateValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ")",
                                                 $@"INSERT INTO CRS_USER.BANK_CONTRACTS(BANK_ID,FUNDS_CONTRACT_ID)VALUES({bank_id},{ContractID})",
                                                    "Bankın müqaviləsi bazaya daxil edilmədi.");
            else if (source_id == 10)
                GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.FUNDS_CONTRACTS(ID,FUNDS_SOURCE_ID,FUNDS_SOURCE_NAME_ID,CONTRACT_NUMBER,REGISTRATION_NUMBER,PERIOD,START_DATE,END_DATE,AMOUNT,CURRENCY_ID,CHECK_END_DATE,NOTE,STATUS_ID,CURRENCY_RATE)VALUES(" + ContractID + "," + source_id + "," + source_name_id + ",'" + ContractNumberText.Text.Trim() + "','" + RegistrationNumberText.Text.Trim() + "'," + PeriodValue.Value + ",TO_DATE('" + ContractStartDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ContractEndDate.Text + "','DD/MM/YYYY')," + AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + currency_id + "," + check_end_date + ",'" + NoteText.Text.Trim() + "',13," + CurrencyRateValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ")",
                                                 $@"INSERT INTO CRS_USER.FOUNDER_CONTRACTS(FOUNDER_ID,FUNDS_CONTRACT_ID,FOUNDER_CARD_ID)VALUES(" + founder_id + "," + ContractID + "," + founder_card_id + ")",
                                                    "Təsisçinin müqaviləsi bazaya daxil edilmədi.");
            else
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.FUNDS_CONTRACTS(ID,FUNDS_SOURCE_ID,FUNDS_SOURCE_NAME_ID,CONTRACT_NUMBER,REGISTRATION_NUMBER,PERIOD,START_DATE,END_DATE,AMOUNT,CURRENCY_ID,CHECK_END_DATE,NOTE,STATUS_ID,CURRENCY_RATE)VALUES(" + ContractID + "," + source_id + "," + source_name_id + ",'" + ContractNumberText.Text.Trim() + "','" + RegistrationNumberText.Text.Trim() + "'," + PeriodValue.Value + ",TO_DATE('" + ContractStartDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ContractEndDate.Text + "','DD/MM/YYYY')," + AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + currency_id + "," + check_end_date + ",'" + NoteText.Text.Trim() + "',13," + CurrencyRateValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ")",
                                                    "Müqavilə bazaya daxil edilmədi.");
            GlobalProcedures.CalculatedAttractedFundsTotal(int.Parse(ContractID));
        }

        private void UpdateContract()
        {
            if (source_id == 6 || source_id == 10)
                source_name_id = 0;
            if (status_id == 13)
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.FUNDS_CONTRACTS SET FUNDS_SOURCE_ID = " + source_id + ",FUNDS_SOURCE_NAME_ID = " + source_name_id + ",CONTRACT_NUMBER = '" + ContractNumberText.Text.Trim() + "',REGISTRATION_NUMBER = '" + RegistrationNumberText.Text.Trim() + "',PERIOD = " + PeriodValue.Value + ",START_DATE = TO_DATE('" + ContractStartDate.Text + "','DD/MM/YYYY'),END_DATE = TO_DATE('" + ContractEndDate.Text + "','DD/MM/YYYY'),AMOUNT = " + AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",CURRENCY_ID = " + currency_id + ",CHECK_END_DATE = " + check_end_date + ",NOTE = '" + NoteText.Text.Trim() + "',STATUS_ID = " + status_id + ",CURRENCY_RATE = " + CurrencyRateValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + ContractID,
                                                "Müqavilə bazada dəyişdirilmədi.",
                                                this.Name + "/UpdateContract");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.FUNDS_CONTRACTS SET NOTE = '{NoteText.Text.Trim()}',STATUS_ID = {status_id},CLOSED_DATE = TO_DATE('{CloseDateValue.Text}','DD/MM/YYYY') WHERE ID = {ContractID}",
                                                "Müqavilə bazada dəyişdirilmədi.",
                                                this.Name + "/UpdateContract");

            if (source_id == 6)
            {
                if (source_id == old_source_id)
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANK_CONTRACTS SET BANK_ID = {bank_id} WHERE FUNDS_CONTRACT_ID = {ContractID}",
                                                        "Bankın müqaviləsi bazada dəyişdirilmədi.",
                                                        this.Name + "/UpdateContract");
                else
                    GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}",
                                                    $@"INSERT INTO CRS_USER.BANK_CONTRACTS(BANK_ID,FUNDS_CONTRACT_ID)VALUES({bank_id},{ContractID})",
                                                    "Bankın müqaviləsi bazaya daxil edilmədi.",
                                                    this.Name + "/UpdateContract");
            }
            else if (source_id == 10)
            {
                if (source_id == old_source_id)
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.FOUNDER_CONTRACTS SET FOUNDER_ID = {founder_id},FOUNDER_CARD_ID = {founder_card_id} WHERE FUNDS_CONTRACT_ID = {ContractID}", "Təsisçinin müqaviləsi bazada dəyişdirilmədi.");
                else
                    GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.BANK_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}",
                                                     $@"INSERT INTO CRS_USER.FOUNDER_CONTRACTS(FOUNDER_ID,FUNDS_CONTRACT_ID,FOUNDER_CARD_ID)VALUES({founder_id},{ContractID},{founder_card_id})",
                                                        "Təsisçinin müqaviləsi bazaya daxil edilmədi.");

            }

            GlobalProcedures.CalculatedAttractedFundsTotal(int.Parse(ContractID));
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlContractDetails())
            {
                if (TransactionName == "INSERT")
                    InsertContract();
                else
                    UpdateContract();
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_FUNDS_PERCENT", "P_CONTRACT_ID", ContractID, "İllik faiz əsas cədvələ daxil edilmədi.");
                this.Close();
            }
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void BankComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", BankComboBox.Text);
        }

        private void BankComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11, null);
        }

        private void FounderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            founder_id = GlobalFunctions.FindComboBoxSelectedValue("FOUNDERS", "FULLNAME", "ID", FounderComboBox.Text);
            FindFounderCard(founder_id);
        }

        private void FindFounderCard(int founderid)
        {
            string s = $@"SELECT CS.SERIES||': '||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib',FC.ID FROM CRS_USER.FOUNDER_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = (SELECT MAX(ID) FROM CRS_USER.FOUNDER_CARDS WHERE FOUNDER_ID = FC.FOUNDER_ID) AND FC.FOUNDER_ID = {founderid}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FindFounderCard");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CardText.Text = dr[0].ToString();
                        founder_card_id = int.Parse(dr[1].ToString());
                    }
                }
                else
                {
                    CardText.Text = null;
                    founder_card_id = 0;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçinin şəxsiyyətini təsdiq edən sənəd tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FounderComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 15, null);
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (13,14) AND STATUS_NAME", "ID", StatusComboBox.Text);
                    break;
                case "EN":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (13,14) AND STATUS_NAME_EN", "ID", StatusComboBox.Text);
                    break;
                case "RU":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (13,14) AND STATUS_NAME_RU", "ID", StatusComboBox.Text);
                    break;
            }

            if (status_id == 14)
            {
                CloseDateLabel.Visible = true;
                CloseDateValue.Visible = true;
                CloseDateValue.EditValue = DateTime.Today;
                CloseDateValue.Properties.MinValue = ContractStartDate.DateTime;
                ContractNumberText.Enabled =
                    RegistrationNumberText.Enabled =
                    FundsSourceLookUp.Enabled =
                    FundsSourceNameComboBox.Enabled =
                    BankComboBox.Enabled =
                    PeriodValue.Enabled =
                    ContractStartDate.Enabled =
                    EndDateCheckEdit.Enabled =
                    ContractEndDate.Enabled =
                    AmountValue.Enabled =
                    CurrencyLookUp.Enabled = false;
            }
            else
            {
                CloseDateLabel.Visible = false;
                CloseDateValue.Visible = false;
                ContractNumberText.Enabled =
                    RegistrationNumberText.Enabled =
                    FundsSourceLookUp.Enabled =
                    FundsSourceNameComboBox.Enabled =
                    BankComboBox.Enabled =
                    PeriodValue.Enabled =
                    ContractStartDate.Enabled =
                    EndDateCheckEdit.Enabled =
                    ContractEndDate.Enabled =
                    AmountValue.Enabled =
                    CurrencyLookUp.Enabled = !CurrentStatus;
            }
        }

        private void StatusComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 1, "WHERE ID IN (13, 14)");
        }
    }
}