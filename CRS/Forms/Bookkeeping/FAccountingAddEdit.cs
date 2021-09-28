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

namespace CRS.Forms.Bookkeeping
{
    public partial class FAccountingAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAccountingAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, AccountID;

        bool CurrentStatus = false, AccountUsed = false, IsImportant = false;
        int AccountUsedUserID = -1, type_id, bank_id;
        string old_sub_account;

        public delegate void DoEvent();
        public event DoEvent RefreshAccountDataGridView;

        private void FAccountingAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                BankLookUp.Properties.Buttons[1].Visible = GlobalVariables.Bank;

            GlobalProcedures.FillLookUpEdit(TypeLookUp, "ACCOUNT_TYPE", "ID", "NAME", null);
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.ACCOUNTING_PLAN", GlobalVariables.V_UserID, "WHERE ID = " + AccountID + " AND USED_USER_ID = -1");
                LoadAccountDetails();
                AccountUsed = (AccountUsedUserID >= 0);

                if (AccountUsed)
                {
                    if (GlobalVariables.V_UserID != AccountUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == AccountUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş hesab hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş hesabın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
            }
            else
                CategoryRadioGroup_SelectedIndexChanged(sender, EventArgs.Empty);
        }

        private void ComponentEnabled(bool status)
        {
            TypeLookUp.Enabled =
                NumberValue.Enabled =
                SUBNameText.Enabled =
                NoteText.Enabled =
                BankLookUp.Enabled =
                BOK.Visible = !status;
        }

        private void LoadAccountDetails()
        {
            string s = $@"SELECT P.ACCOUNT_NAME,
                               P.NOTE,
                               T.NAME TYPE_NAME,
                               P.ACCOUNT_NUMBER,
                               P.SUB_ACCOUNT,
                               P.ACCOUNT_CATEGORY,
                               B.LONG_NAME BANK_NAME,
                               P.SUB_ACCOUNT_NAME,
                               P.USED_USER_ID,
                               P.IS_IMPORTANT
                          FROM CRS_USER.ACCOUNTING_PLAN P, CRS_USER.ACCOUNT_TYPE T, CRS_USER.BANKS B
                         WHERE T.ID = P.ACCOUNT_TYPE_ID AND P.BANK_ID = B.ID(+) AND P.ID = {AccountID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    NameText.Text = dr["ACCOUNT_NAME"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                    TypeLookUp.EditValue = TypeLookUp.Properties.GetKeyValueByDisplayText(dr["TYPE_NAME"].ToString());
                    NumberValue.Value = Convert.ToInt32(dr["ACCOUNT_NUMBER"].ToString());

                    //if (NumberValue.Value != 223 && NumberValue.Value != 221 && NumberValue.Value != 543 && NumberValue.Value != 216 && NumberValue.Value != 631 && NumberValue.Value != 174 && NumberValue.Value != 612 && NumberValue.Value != 712 && NumberValue.Value != 243 && NumberValue.Value != 751)
                    //{
                    //    NumberValue.Enabled = SUBAccountText.Enabled = true;
                    //}
                    //else if (NumberValue.Value == 221 || NumberValue.Value == 223 || NumberValue.Value == 631 || NumberValue.Value == 612 || NumberValue.Value == 712 || NumberValue.Value == 751)
                    //    NumberValue.Enabled = false;
                    //else
                    //{
                    //    NumberValue.Enabled = SUBAccountText.Enabled = false;
                    //}

                    SUBAccountText.Text = dr["SUB_ACCOUNT"].ToString();
                    old_sub_account = dr["SUB_ACCOUNT"].ToString();
                    CategoryRadioGroup.SelectedIndex = Convert.ToInt32(dr["ACCOUNT_CATEGORY"].ToString());
                    if (Convert.ToInt32(dr["ACCOUNT_CATEGORY"].ToString()) == 1)
                        BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    else
                    {
                        BankLookUp.Visible =
                            BankNameLabel.Visible =
                            labelControl5.Visible = false;
                    }
                    SUBNameText.Text = dr["SUB_ACCOUNT_NAME"].ToString();
                    AccountUsedUserID = Convert.ToInt32(dr["USED_USER_ID"].ToString());
                    IsImportant = Convert.ToInt32(dr["IS_IMPORTANT"]) == 1;
                    SUBAccountText.ReadOnly = NumberValue.ReadOnly = IsImportant;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Hesab tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private bool ControlAccountDetails()
        {
            bool b = false;

            if (TypeLookUp.EditValue == null)
            {
                TypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesabın tipi daxil edilməyib.");
                TypeLookUp.Focus();
                TypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesabın adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NumberValue.Value != 543 && NumberValue.Value != 216 && NumberValue.Value != 174 && NumberValue.Value != 243 && SUBAccountText.Text.Length == 0)
            {
                SUBAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sub hesab daxil edilməyib.");
                SUBAccountText.Focus();
                SUBAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SUBAccountText.Text.Length != 12)
            {
                SUBAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sub hesabın uzunluğu 12 simvol olmalıdır.");
                SUBAccountText.Focus();
                SUBAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NumberValue.Value != 543 && NumberValue.Value != 216 && NumberValue.Value != 174 && NumberValue.Value != 243 && SUBAccountText.Text.Length != 0 && SUBNameText.Text.Length == 0)
            {
                SUBNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sub hesabın adı daxil edilməyib.");
                SUBNameText.Focus();
                SUBNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.ACCOUNTING_PLAN WHERE SUB_ACCOUNT = '{SUBAccountText.Text.Trim()}'") > 0)
            {
                SUBAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(SUBAccountText.Text.Trim() + " sub hesabı artıq bazaya daxil edilib.");
                SUBAccountText.Focus();
                SUBAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CategoryRadioGroup.SelectedIndex == 1 && BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertAccount()
        {
            if (CategoryRadioGroup.SelectedIndex == 0)
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.ACCOUNTING_PLAN(ID,ACCOUNT_NUMBER,SUB_ACCOUNT,ACCOUNT_NAME,SUB_ACCOUNT_NAME,NOTE,ACCOUNT_TYPE_ID,ACCOUNT_CATEGORY) VALUES(ACCOUNTING_PLAN_SEQUENCE.NEXTVAL," + NumberValue.Value + ",'" + SUBAccountText.Text.Trim() + "','" + NameText.Text.Trim() + "','" + SUBNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + type_id + "," + CategoryRadioGroup.SelectedIndex + ")",
                                                "Hesab daxil edilmədi.");
            else
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.ACCOUNTING_PLAN(ID,ACCOUNT_NUMBER,SUB_ACCOUNT,ACCOUNT_NAME,SUB_ACCOUNT_NAME,NOTE,ACCOUNT_TYPE_ID,ACCOUNT_CATEGORY,BANK_ID) VALUES(ACCOUNTING_PLAN_SEQUENCE.NEXTVAL," + NumberValue.Value + ",'" + SUBAccountText.Text.Trim() + "','" + NameText.Text.Trim() + "','" + SUBNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + type_id + "," + CategoryRadioGroup.SelectedIndex + "," + bank_id + ")",
                                                "Hesab daxil edilmədi.");
        }

        private void UpdateAccount()
        {
            if (NumberValue.Value != 543 && NumberValue.Value != 216 && NumberValue.Value != 174 && NumberValue.Value != 243)
            {
                GlobalProcedures.ExecuteThreeQuery($@"UPDATE CRS_USER.OPERATION_DEBT SET ACCOUNT = '{SUBAccountText.Text.Trim()}' WHERE ACCOUNT = '{old_sub_account}'",
                                                   $@"UPDATE CRS_USER.OPERATION_JOURNAL SET DEBIT_ACCOUNT = '{SUBAccountText.Text.Trim()}' WHERE DEBIT_ACCOUNT = '{old_sub_account}'",
                                                   $@"UPDATE CRS_USER.OPERATION_JOURNAL SET CREDIT_ACCOUNT = '{SUBAccountText.Text.Trim()}' WHERE CREDIT_ACCOUNT = '{old_sub_account}'",
                                                    "Hesab dəyişdirilmədi.");
            }

            if (CategoryRadioGroup.SelectedIndex == 0)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.ACCOUNTING_PLAN SET ACCOUNT_NAME ='{NameText.Text.Trim()}',SUB_ACCOUNT_NAME ='{SUBNameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}',ACCOUNT_TYPE_ID = {type_id},ACCOUNT_NUMBER = {NumberValue.Value},SUB_ACCOUNT = '{SUBAccountText.Text.Trim()}' WHERE ID = {AccountID}",
                                                "Hesab dəyişdirilmədi.");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.ACCOUNTING_PLAN SET ACCOUNT_NAME ='{NameText.Text.Trim()}',SUB_ACCOUNT_NAME ='{SUBNameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}',ACCOUNT_TYPE_ID = {type_id},ACCOUNT_NUMBER = {NumberValue.Value},SUB_ACCOUNT = '{SUBAccountText.Text.Trim()}',BANK_ID = {bank_id} WHERE ID = {AccountID}",
                                                "Hesab dəyişdirilmədi.");
        }

        private void FAccountingAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.ACCOUNTING_PLAN", -1, "WHERE ID = " + AccountID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshAccountDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlAccountDetails())
            {
                if (TransactionName == "INSERT")
                    InsertAccount();
                else
                    UpdateAccount();
                this.Close();
            }
        }

        private void CategoryRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CategoryRadioGroup.SelectedIndex == 0)
            {
                BankLookUp.Visible = false;
                BankNameLabel.Visible = false;
                labelControl5.Visible = false;
                this.MaximumSize = new Size(741, 300);
                this.MinimumSize = new Size(741, 300);
            }
            else
            {
                BankLookUp.Visible = true;
                BankNameLabel.Visible = true;
                labelControl5.Visible = true;
                this.MaximumSize = new Size(741, 350);
                this.MinimumSize = new Size(741, 350);
                GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
            }
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11, null);
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);
        }

        private void SUBAccountText_EditValueChanged(object sender, EventArgs e)
        {
            if (SUBAccountText.Text.Trim().Length == 0)
                SubAccountLengthLabel.Visible = false;
            else
            {
                SubAccountLengthLabel.Visible = true;
                SubAccountLengthLabel.Text = SUBAccountText.Text.Trim().Length.ToString();
            }
        }

        private void TypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeLookUp.EditValue == null)
                return;

            type_id = Convert.ToInt32(TypeLookUp.EditValue);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 11:
                    GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
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

        private void NumberValue_EditValueChanged(object sender, EventArgs e)
        {
            NameText.Text = GlobalFunctions.GetName($@"SELECT DISTINCT ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = {NumberValue.Value}");
        }
    }
}