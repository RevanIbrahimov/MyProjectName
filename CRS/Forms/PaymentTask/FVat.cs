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
    public partial class FVat : DevExpress.XtraEditors.XtraForm
    {
        public FVat()
        {
            InitializeComponent();
        }
        bool CurrentStatus = false, BankUsed = false;
        int UsedUserID = -1;

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FVat_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_VAT_BANKS", GlobalVariables.V_UserID, "WHERE USED_USER_ID = -1");
            UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.TASK_VAT_BANKS");
            BankUsed = (UsedUserID >= 0);

            if (BankUsed)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş bankın məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Bankın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void ComponentEnabled(bool status)
        {
            BankNameText.Enabled =
                CodeText.Enabled =
                VoenText.Enabled =
                SwiftText.Enabled =
                CBARAccountText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadBankDetails()
        {
            string s = $@"SELECT B.NAME,   
                                   B.CODE,                                
                                   B.CBAR_ACCOUNT,
                                   B.SWIFT,
                                   B.VOEN,
                                   B.NOTE                       
                              FROM CRS_USER.TASK_VAT_BANKS B";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    BankNameText.Text = dr["NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    CBARAccountText.Text = dr["CBAR_ACCOUNT"].ToString();
                    SwiftText.Text = dr["SWIFT"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("ƏDV Bank açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CBARAccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(CBARAccountText, CBARAccountLengthLabel);
        }

        private void UpdateBank()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.TASK_VAT_BANKS SET 
                                                            NAME ='{BankNameText.Text.Trim()}',    
                                                            CODE = '{CodeText.Text.Trim()}',                                                        
                                                            CBAR_ACCOUNT = '{CBARAccountText.Text.Trim()}',                                                            
                                                            SWIFT = '{SwiftText.Text.Trim()}',
                                                            VOEN = '{VoenText.Text.Trim()}',
                                                            NOTE = '{NoteText.Text.Trim()}' WHERE USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "ƏDV bank dəyişdirilmədi.",
                                                "FVat - UpdateBank");
        }

        private bool ControlBankDetails()
        {
            bool b = false;

            if (BankNameText.Text.Length == 0)
            {
                BankNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bankın tam adı daxil edilməyib.");
                BankNameText.Focus();
                BankNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bankın kodu daxil edilməyib.");
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

            return b;
        }

        private void FVat_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_VAT_BANKS", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlBankDetails())
            {
                UpdateBank();
                this.Close();
            }
        }
    }
}