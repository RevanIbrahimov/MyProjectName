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

namespace CRS.Forms.Bank
{
    public partial class FBankAccountAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBankAccountAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, AccountID;

        int bank_id, 
            branch_id, 
            currency_id, 
            status_id, 
            BankUsedUserID = -1, 
            check = 0;

        bool CurrentStatus = false, 
            BankUsed = false, 
            BankClosed = false;

        public delegate void DoEvent();
        public event DoEvent RefreshAccountsDataGridView;

        private void FBankAccountAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                BankLookUp.Properties.Buttons[1].Visible = GlobalVariables.Bank;
                BranchLookUp.Properties.Buttons[1].Visible = GlobalVariables.Bank;
                CurrencyLookUp.Properties.Buttons[1].Visible = GlobalVariables.Currency;
            }

            DateValue.EditValue = DateTime.Today;            
            GlobalProcedures.FillLookUpEdit(CurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANK_ACCOUNTS", GlobalVariables.V_UserID, "WHERE ID = " + AccountID + " AND USED_USER_ID = -1");

                if (GlobalFunctions.GetID("SELECT STATUS_ID FROM CRS_USER.BANK_ACCOUNTS WHERE ID = " + AccountID) == 12)
                    BankClosed = true;
                else
                {
                    BankClosed = false;
                    BankUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.BANK_ACCOUNTS WHERE ID = " + AccountID);
                    BankUsed = (BankUsedUserID >= 0);
                }

                if (((BankClosed) && (BankUsed)) || ((BankClosed) && (!BankUsed)))
                {
                    XtraMessageBox.Show("Seçilmiş bank hesabı bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else if ((BankUsed) && (!BankClosed))
                {
                    if (GlobalVariables.V_UserID != BankUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BankUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                StatusComboBox.Visible = true;
                StatusLabel.Visible = true;
                ComponentEnabled(CurrentStatus);
                GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (11,12) ORDER BY ID");
                LoadBankAccountDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            DateValue.Enabled = 
                BankLookUp.Enabled =
                BranchLookUp.Enabled =
                CurrencyLookUp.Enabled = 
                AccountText.Enabled = 
                NoteText.Enabled = 
                StatusComboBox.Enabled = 
                IsPaymentCheck.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadBankAccountDetails()
        {
            string s = null;
            try
            {
                s = "SELECT BA.ADATE,B.LONG_NAME,BB.NAME BRANCH_NAME,BA.ACCOUNT,C.CODE CURRENCY_CODE,BA.NOTE,S.STATUS_NAME,BA.IS_PAYMENT FROM CRS_USER.BANK_ACCOUNTS BA,CRS_USER.BANKS B,CRS_USER.BANK_BRANCHES BB,CRS_USER.STATUS S,CRS_USER.CURRENCY C WHERE BA.BANK_ID = B.ID AND BA.BANK_BRANCH_ID = BB.ID AND B.ID = BB.BANK_ID AND BA.STATUS_ID = S.ID AND BA.CURRENCY_ID = C.ID AND BA.ID = " + AccountID;
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankAccountDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    DateValue.EditValue = DateTime.Parse(dr["ADATE"].ToString());
                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["LONG_NAME"].ToString());
                    BranchLookUp.EditValue = BranchLookUp.Properties.GetKeyValueByDisplayText(dr["BRANCH_NAME"].ToString());
                    AccountText.Text = dr["ACCOUNT"].ToString();
                    CurrencyLookUp.EditValue = CurrencyLookUp.Properties.GetKeyValueByDisplayText(dr["CURRENCY_CODE"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                    StatusComboBox.EditValue = dr["STATUS_NAME"].ToString();
                    IsPaymentCheck.Checked = (Convert.ToInt32(dr["IS_PAYMENT"].ToString()) == 1);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank hesabı açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 1:
                    GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (11, 12)");
                    break;
                case 11:
                    {
                        GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                        GlobalProcedures.FillLookUpEdit(BranchLookUp, "BANK_BRANCHES", "ID", "NAME", "STATUS_ID = 9 AND BANK_ID = " + bank_id + " ORDER BY 1");
                    }
                    break;
                case 7:
                    GlobalProcedures.FillLookUpEdit(CurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }

        }

        private void LoadDictionaries(string transaction, int index, string where)
        {
            Forms.FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.StatusWhere = where;
            fc.RefreshList += new Forms.FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private bool ControlBankAccountDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BranchLookUp.EditValue == null)
            {
                BranchLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Filial daxil edilməyib.");
                BranchLookUp.Focus();
                BranchLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AccountText.Text))
            {
                AccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab daxil edilməyib.");
                AccountText.Focus();
                AccountText.BackColor = GlobalFunctions.ElementColor();
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

            if (TransactionName == "EDIT" && String.IsNullOrEmpty(StatusComboBox.Text))
            {
                StatusComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Status daxil edilməyib.");
                StatusComboBox.Focus();
                StatusComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, NumberLengthLabel);
            //if (AccountText.Text.Trim().Length == 0)
            //    NumberLengthLabel.Visible = false;
            //else
            //{
            //    NumberLengthLabel.Visible = true;
            //    NumberLengthLabel.Text = AccountText.Text.Trim().Length.ToString();
            //}
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);
            BranchLookUp.EditValue = null;
            GlobalProcedures.FillLookUpEdit(BranchLookUp, "BANK_BRANCHES", "ID", "NAME", "STATUS_ID = 9 AND BANK_ID = " + bank_id + " ORDER BY 1");            
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11, null);
        }

        private void BranchLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BranchLookUp.EditValue == null)
                return;

            branch_id = Convert.ToInt32(BranchLookUp.EditValue);
        }

        private void CurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CurrencyLookUp.EditValue == null)
                return;

            currency_id = Convert.ToInt32(CurrencyLookUp.EditValue);
        }

        private void CurrencyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7, null);
        }

        private void InsertBankAccount()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.BANK_ACCOUNTS(ID,
                                                                                BANK_ID,
                                                                                BANK_BRANCH_ID,
                                                                                ADATE,
                                                                                ACCOUNT,
                                                                                CURRENCY_ID,
                                                                                NOTE,
                                                                                STATUS_ID,
                                                                                IS_PAYMENT) 
                                                VALUES(BANK_ACCOUNT_SEQUENCE.NEXTVAL,
                                                        {bank_id},
                                                        {branch_id},
                                                        TO_DATE('{DateValue.Text}','DD/MM/YYYY'),
                                                        '{AccountText.Text.Trim()}',
                                                        {currency_id},
                                                        '{NoteText.Text.Trim()}',
                                                        11,
                                                        {check})",
                                                "Bank hesabı daxil edilmədi.");
        }

        private void UpdateBankAccount()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANK_ACCOUNTS SET ADATE = TO_DATE('{DateValue.Text}','DD/MM/YYYY'),
                                                                                                ACCOUNT = '{AccountText.Text.Trim()}',
                                                                                                CURRENCY_ID = {currency_id},
                                                                                                NOTE = '{NoteText.Text.Trim()}',
                                                                                                STATUS_ID = {status_id},
                                                                                                BANK_ID = {bank_id},
                                                                                                BANK_BRANCH_ID = {branch_id},
                                                                                                IS_PAYMENT = {check} 
                                        WHERE ID = {AccountID}",
                                                "Bank hesabı dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlBankAccountDetails())
            {
                if (TransactionName == "INSERT")
                    InsertBankAccount();
                else
                    UpdateBankAccount();
                this.Close();
            }
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (11,12) AND STATUS_NAME", "ID", StatusComboBox.Text);
                    break;
                case "EN":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (11,12) AND STATUS_NAME_EN", "ID", StatusComboBox.Text);
                    break;
                case "RU":
                    status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (11,12) AND STATUS_NAME_RU", "ID", StatusComboBox.Text);
                    break;
            }
        }

        private void FBankAccountAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANK_ACCOUNTS", -1, "WHERE ID = " + AccountID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshAccountsDataGridView();
        }

        private void StatusComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 1, "WHERE ID IN (11, 12)");
        }

        private void IsPaymentCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (IsPaymentCheck.Checked)
                check = 1;
            else
                check = 0;
        }
    }
}