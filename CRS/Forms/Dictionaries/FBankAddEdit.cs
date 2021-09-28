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

namespace CRS.Forms.Dictionaries
{
    public partial class FBankAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBankAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BankID;
        bool CurrentStatus = false, BankUsed = false;
        int BankUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshBanksDataGridView;

        private void FBankAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                StatusLookUp.Properties.Buttons[1].Visible = GlobalVariables.Status;

            if (TransactionName == "INSERT")
                BankID = GlobalFunctions.GetOracleSequenceValue("BANK_SEQUENCE").ToString();
            else
            {
                StatusLookUp.Visible = StatusLabel.Visible = true;
                RefreshDictionaries(1);
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANKS", Class.GlobalVariables.V_UserID, "WHERE ID = " + BankID + " AND USED_USER_ID = -1");
                BankUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.BANKS WHERE ID = " + BankID);
                BankUsed = (BankUsedUserID > 0);

                if (BankUsed)
                {
                    if (GlobalVariables.V_UserID != BankUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BankUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş bank hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bankın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadBankDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            LongNameText.Enabled = 
                ShortNameText.Enabled =   
                CodeText.Enabled = 
                SwiftText.Enabled =
                VoenText.Enabled = 
                AccountText.Enabled =
                CBARAccountText.Enabled =          
                StatusLookUp.Enabled =    
                BOK.Visible = !status;
        }

        private void LoadBankDetails()
        {
            string s = $@"SELECT B.LONG_NAME,
                                   B.SHORT_NAME,
                                   B.CODE,                                   
                                   B.SWIFT,
                                   B.VOEN,
                                   B.ACCOUNT,
                                   B.CBAR_ACCOUNT,
                                   S.STATUS_NAME,
                                   B.IS_USED
                              FROM CRS_USER.BANKS B, CRS_USER.STATUS S
                             WHERE B.STATUS_ID = S.ID AND B.ID = {BankID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    LongNameText.Text = dr["LONG_NAME"].ToString();
                    ShortNameText.Text = dr["SHORT_NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    SwiftText.Text = dr["SWIFT"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    AccountText.Text = dr["ACCOUNT"].ToString();
                    CBARAccountText.Text = dr["CBAR_ACCOUNT"].ToString();
                    StatusLookUp.EditValue = StatusLookUp.Properties.GetKeyValueByDisplayText(dr["STATUS_NAME"].ToString());
                    IsUsedCheck.Checked = (Convert.ToInt32(dr["IS_USED"].ToString()) == 1);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlBankDetails()
        {
            bool b = false;

            if (LongNameText.Text.Length == 0)
            {
                LongNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bankın tam adı daxil edilməyib.");               
                LongNameText.Focus();
                LongNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ShortNameText.Text.Length == 0)
            {
                ShortNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bankın qısa adı daxil edilməyib.");                
                ShortNameText.Focus();
                ShortNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kod daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SwiftText.Text.Length == 0)
            {
                SwiftText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Swift daxil edilməyib.");
                SwiftText.Focus();
                SwiftText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length == 0)
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vöen daxil edilməyib.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AccountText.Text.Length == 0)
            {
                AccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin bankda olan hesabı daxil edilməyib.");
                AccountText.Focus();
                AccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CBARAccountText.Text.Length == 0)
            {
                CBARAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bankın Mərkəzi bankda olan hesabı daxil edilməyib.");
                CBARAccountText.Focus();
                CBARAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "EDIT" && StatusLookUp.Text.Length == 0)
            {
                StatusLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Status daxil edilməyib.");
                StatusLookUp.Focus();
                StatusLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertBank()
        {
            int is_used = (IsUsedCheck.Checked) ? 1 : 0;
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.BANKS") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.BANKS(ID,
                                                                        LONG_NAME,
                                                                        SHORT_NAME, 
                                                                        CODE,                                                                               
                                                                        SWIFT,
                                                                        VOEN,
                                                                        ACCOUNT,
                                                                        CBAR_ACCOUNT,
                                                                        STATUS_ID,
                                                                        IS_USED,
                                                                        ORDER_ID) 
                                                   VALUES({BankID},
                                                           '{LongNameText.Text.Trim()}',
                                                           '{ShortNameText.Text.Trim()}',    
                                                           '{CodeText.Text.Trim()}',                                                        
                                                           '{SwiftText.Text.Trim()}',
                                                           '{VoenText.Text.Trim()}',
                                                           '{AccountText.Text.Trim()}',
                                                           '{CBARAccountText.Text.Trim()}',                                                           
                                                           7,
                                                           {is_used},
                                                           {order})",
                                                "Bank daxil edilmədi.");
        }

        private void UpdateBank()
        {
            int is_used = (IsUsedCheck.Checked) ? 1 : 0;
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANKS SET 
                                                            LONG_NAME ='{LongNameText.Text.Trim()}',
                                                            SHORT_NAME = '{ShortNameText.Text.Trim()}', 
                                                            CODE = '{CodeText.Text.Trim()}',
                                                            SWIFT = '{SwiftText.Text.Trim()}',
                                                            VOEN = '{VoenText.Text.Trim()}',
                                                            ACCOUNT = '{AccountText.Text.Trim()}',
                                                            CBAR_ACCOUNT = '{CBARAccountText.Text.Trim()}',
                                                            STATUS_ID = {StatusLookUp.EditValue},
                                                            IS_USED = {is_used} 
                                                      WHERE ID = {BankID}",
                                                "Bank dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlBankDetails())
            {
                if (TransactionName == "INSERT")
                    InsertBank();
                else
                    UpdateBank();
                this.Close();
            }
        }

        private void FBankAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANKS", -1, "WHERE ID = " + BankID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);            
            this.RefreshBanksDataGridView();
        }
        
        
        private void CBARAccountText_EditValueChanged(object sender, EventArgs e)
        {
            if (CBARAccountText.Text.Trim().Length == 0)
                CbarAccountLengthLabel.Visible = false;
            else
            {
                CbarAccountLengthLabel.Visible = true;
                CbarAccountLengthLabel.Text = CBARAccountText.Text.Trim().Length.ToString();
            }
        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, AccountLengthLabel);
        }

        private void StatusLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionariesForStatus("E", 1, "WHERE ID IN (7, 8)");
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(StatusLookUp, "STATUS", "ID", "STATUS_NAME", "ID IN (7,8) ORDER BY ID");
        }

        private void LoadDictionariesForStatus(string transaction, int index, string where)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.StatusWhere = where;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }
        
    }
}