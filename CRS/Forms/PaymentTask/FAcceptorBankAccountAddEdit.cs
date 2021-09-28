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

namespace CRS.Forms.PaymentTask
{
    public partial class FAcceptorBankAccountAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAcceptorBankAccountAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, AcceptorID, AcceptorName, AccountID;

        int bank_id,
            branch_id,
            currency_id;

        public delegate void DoEvent();

        private void BankComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bank_id = GlobalFunctions.FindTempComboBoxSelectedValue("TASK_ACCEPTOR_BANKS_TEMP", "NAME", "ID", BankComboBox.Text);
            BranchComboBox.SelectedIndex = -1;            
            GlobalProcedures.FillComboBoxEditWithSqlText(BranchComboBox, "SELECT NAME,NAME,NAME FROM CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP WHERE ACCEPTOR_ID = " + AcceptorID + " AND BANK_ID = " + bank_id + " ORDER BY ORDER_ID");            
        }

        private void BranchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            branch_id = GlobalFunctions.FindTempComboBoxSelectedValue("TASK_ACCEPTOR_BB_TEMP", "NAME", "ID", BranchComboBox.Text);
        }

        private void CurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currency_id = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "CODE", "ID", CurrencyComboBox.Text);
        }

        private bool ControlBankAccountDetails()
        {
            bool b = false;            

            if (String.IsNullOrEmpty(BankComboBox.Text))
            {
                BankComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankComboBox.Focus();
                BankComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(BranchComboBox.Text))
            {
                BranchComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Filial daxil edilməyib.");
                BranchComboBox.Focus();
                BranchComboBox.BackColor = GlobalFunctions.ElementColor();
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

            if (String.IsNullOrEmpty(CurrencyComboBox.Text))
            {
                CurrencyComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta daxil edilməyib.");
                CurrencyComboBox.Focus();
                CurrencyComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            return b;
        }

        private void InsertBankAccount()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.TASK_ACCEPTOR_BANK_ACNTS_TEMP(
                                                                                                        ID,
                                                                                                        ACCEPTOR_ID,
                                                                                                        ACCEPTOR_BANK_ID,
                                                                                                        ACCEPTOR_BANK_BRANCH_ID,
                                                                                                        ACCOUNT,
                                                                                                        CURRENCY_ID,
                                                                                                        NOTE,
                                                                                                        IS_CHANGE,
                                                                                                        USED_USER_ID
                                                                                                    ) 
                                                VALUES(
                                                            TASK_ACCEPTOR_BNK_ACC_SEQUENCE.NEXTVAL,
                                                            {AcceptorID},
                                                            {bank_id},
                                                            {branch_id},
                                                            '{AccountText.Text.Trim()}',
                                                            {currency_id},
                                                            '{NoteText.Text.Trim()}',
                                                            1,
                                                            {GlobalVariables.V_UserID}
                                                       )",
                                                "Bank hesabı daxil edilmədi.");
        }

        private void UpdateBankAccount()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.TASK_ACCEPTOR_BANK_ACNTS_TEMP SET 
                                                                        ACCOUNT = '{AccountText.Text.Trim()}',
                                                                        CURRENCY_ID = {currency_id},
                                                                        NOTE = '{NoteText.Text.Trim()}',
                                                                        ACCEPTOR_BANK_ID = {bank_id},
                                                                        ACCEPTOR_BANK_BRANCH_ID = {branch_id},
                                                                        IS_CHANGE = 1 
                                             WHERE ACCEPTOR_ID = {AcceptorID} AND ID = {AccountID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Bank hesabı dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlBankAccountDetails())
            {
                if (TransactionName == "INSERT")
                    InsertBankAccount();
                else
                    UpdateBankAccount();
                this.Close();
            }
        }

        private void FAcceptorBankAccountAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshAccountsDataGridView();
        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {            
            if (AccountText.Text.Trim().Length == 0)
                NumberLengthLabel.Visible = false;
            else
            {
                NumberLengthLabel.Visible = true;
                NumberLengthLabel.Text = AccountText.Text.Trim().Length.ToString();
            }
        }

        public event DoEvent RefreshAccountsDataGridView;

        private void FAcceptorBankAccountAddEdit_Load(object sender, EventArgs e)
        {
            AcceptorNameText.Text = AcceptorName;
            GlobalProcedures.FillComboBoxEdit(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillComboBoxEditWithSqlText(BankComboBox, $@"SELECT NAME,NAME,NAME FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP WHERE ACCEPTOR_ID = {AcceptorID} ORDER BY ORDER_ID");
            if (TransactionName == "EDIT")
                LoadAccountDetails();
        }

        //private void ComponentEnabled(bool status)
        //{            
        //    BankComboBox.Enabled = !status;
        //    BranchComboBox.Enabled = !status;
        //    CurrencyComboBox.Enabled = !status;
        //    AccountText.Enabled = !status;
        //    NoteText.Enabled = !status;            
        //    BOK.Visible = !status;
        //}

        private void LoadAccountDetails()
        {
            string sql = $@"SELECT B.NAME BANK_NAME,
                                   BB.NAME BRANCH_NAME,
                                   BA.ACCOUNT,
                                   BA.NOTE,
                                   C.CODE CURRENCY_CODE
                              FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANK_ACNTS_TEMP BA,
                                   CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP B,
                                   CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP BB,
                                   CRS_USER.CURRENCY C
                             WHERE     BA.ACCEPTOR_BANK_ID = B.ID
                                   AND BA.ACCEPTOR_BANK_BRANCH_ID = BB.ID
                                   AND B.ID = BB.BANK_ID
                                   AND B.ACCEPTOR_ID = BB.ACCEPTOR_ID
                                   AND BA.CURRENCY_ID = C.ID
                                   AND BA.ACCEPTOR_ID = {AcceptorID}
                                   AND BA.ID = {AccountID}";
            foreach(DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                BankComboBox.EditValue = dr["BANK_NAME"];
                BranchComboBox.EditValue = dr["BRANCH_NAME"];
                AccountText.Text = dr["ACCOUNT"].ToString();
                CurrencyComboBox.EditValue = dr["CURRENCY_CODE"];
                NoteText.Text = dr["NOTE"].ToString();
            }
        }

        private void LoadBankAccountDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT B.NAME BANK_NAME,
                                   BB.NAME BRANCH_NAME,
                                   BA.ACCOUNT,
                                   C.CODE CURRENCY_CODE,
                                   BA.NOTE
                              FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANK_ACNTS_TEMP BA,
                                   CRS_USER.TASK_ACCEPTOR_BANKS B,
                                   CRS_USER.TASK_ACCEPTOR_BANK_BRANCHS BB,
                                   CRS_USER.CURRENCY C
                             WHERE BA.ACCEPTOR_BANK_ID = B.ID
                                   AND BA.ACCEPTOR_BANK_BRANCH_ID = BB.ID
                                   AND B.ID = BB.ACCEPTOR_BANK_ID
                                   AND BA.CURRENCY_ID = C.ID
                                   AND B.ACCEPTOR_ID = BB.ACCEPTOR_ID
                                   AND BA.ID = {AccountID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankAccountDetails");

                foreach (DataRow dr in dt.Rows)
                {                    
                    BankComboBox.EditValue = dr["BANK_NAME"].ToString();
                    BranchComboBox.EditValue = dr["BRANCH_NAME"].ToString();
                    AccountText.Text = dr["ACCOUNT"].ToString();
                    CurrencyComboBox.EditValue = dr["CURRENCY_CODE"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();                    
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
    }
}