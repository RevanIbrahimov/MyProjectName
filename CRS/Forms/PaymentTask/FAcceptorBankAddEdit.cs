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

namespace CRS.Forms.PaymentTask
{
    public partial class FAcceptorBankAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAcceptorBankAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BankID, AcceptorID, AcceptorName;
        bool CurrentStatus = false, BankUsed = false;
        int UsedUserID = -1;

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public delegate void DoEvent();
        public event DoEvent RefreshBanksDataGridView;

        private void FAcceptorBankAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_ACCEPTOR", GlobalVariables.V_UserID, "WHERE ID = " + AcceptorID + " AND USED_USER_ID = -1");
                UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.TASK_ACCEPTOR WHERE ID = {AcceptorID}");
                BankUsed = (UsedUserID >= 0);

                if (BankUsed)
                    CurrentStatus = (GlobalVariables.V_UserID != UsedUserID);
                else
                    CurrentStatus = false;

                ComponentEnabled(CurrentStatus);
                LoadBankDetails();
            }
            else
            {
                if(BankID != null)
                    LoadBankDetails();
            }
        }

        private void LoadBankDetails()
        {
            string s = $@"SELECT B.NAME,   
                                   B.CODE,                                
                                   B.CBAR_ACCOUNT,
                                   B.SWIFT,
                                   B.VOEN,
                                   B.ACCEPTOR_ACCOUNT                        
                              FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP B
                             WHERE B.ID = {BankID} AND ACCEPTOR_ID = {AcceptorID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    BankNameText.Text = dr["NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    CBARAccountText.Text = dr["CBAR_ACCOUNT"].ToString();
                    SwiftText.Text = dr["SWIFT"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    AcceptorAccountText.Text = dr["ACCEPTOR_ACCOUNT"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
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

        private void FAcceptorBankAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshBanksDataGridView();
        }

        private void CBARAccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(CBARAccountText, CBARAccountLengthLabel);
        }

        private void AcceptorAccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AcceptorAccountText, AcceptorAccountLengthLabel);
        }

        void LoadSelectedBankDetails(int bankID)
        {
            string s = $@"SELECT B.LONG_NAME,
                                   B.CODE,
                                   B.CBAR_ACCOUNT,
                                   B.SWIFT,
                                   B.VOEN
                              FROM CRS_USER.BANKS B
                             WHERE B.ID = {bankID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadSelectedBankDetails", "Bankın rekvizitləri yüklənmədi.");
            if(dt.Rows.Count > 0)
            {
                BankNameText.Text = dt.Rows[0]["LONG_NAME"].ToString();
                CodeText.Text = dt.Rows[0]["CODE"].ToString();
                CBARAccountText.Text = dt.Rows[0]["CBAR_ACCOUNT"].ToString();
                SwiftText.Text = dt.Rows[0]["SWIFT"].ToString();
                VoenText.Text = dt.Rows[0]["VOEN"].ToString();
            }
        }

        private void BankNameText_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Dictionaries.FBanks fb = new Dictionaries.FBanks();
            fb.GetBanksID += new Dictionaries.FBanks.DoEvent(LoadSelectedBankDetails);
            fb.ShowDialog();
        }

        private void ComponentEnabled(bool status)
        {
            BankNameText.Enabled =
                CodeText.Enabled =
                VoenText.Enabled =
                SwiftText.Enabled = 
                CBARAccountText.Enabled =
                AcceptorAccountText.Enabled = 
                BOK.Visible = !status;
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

            if (AcceptorAccountText.Text.Length == 0)
            {
                AcceptorAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage($@"Alan tərəfin <color=red><b>'{BankNameText.Text}'</b></color> bankında olan hesabı daxil edilməyib.");
                AcceptorAccountText.Focus();
                AcceptorAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertBank()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP") + 1;

            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP(ID,
                                                                                                    ACCEPTOR_ID,
                                                                                                    NAME,
                                                                                                    CODE,
                                                                                                    CBAR_ACCOUNT,
                                                                                                    ACCEPTOR_ACCOUNT,
                                                                                                    SWIFT,
                                                                                                    VOEN,
                                                                                                    ORDER_ID,
                                                                                                    IS_CHANGE,
                                                                                                    USED_USER_ID) 
                                                   VALUES(TASK_ACCEPTOR_BANK_SEQUENCE.NEXTVAL,
                                                                {AcceptorID},
                                                                '{BankNameText.Text.Trim()}',
                                                                '{CodeText.Text.Trim()}',
                                                                '{CBARAccountText.Text.Trim()}',
                                                                '{AcceptorAccountText.Text.Trim()}',
                                                                '{SwiftText.Text.Trim()}',
                                                                '{VoenText.Text.Trim()}',
                                                                {order},
                                                                1,
                                                                {GlobalVariables.V_UserID})",
                                                "Bank daxil edilmədi.",
                                                "InsertBank");

        }

        private void UpdateBank()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP SET 
                                                            NAME ='{BankNameText.Text.Trim()}',    
                                                            CODE = '{CodeText.Text.Trim()}',                                                        
                                                            CBAR_ACCOUNT = '{CBARAccountText.Text.Trim()}',
                                                            ACCEPTOR_ACCOUNT = '{AcceptorAccountText.Text.Trim()}',
                                                            SWIFT = '{SwiftText.Text.Trim()}',
                                                            VOEN = '{VoenText.Text.Trim()}',
                                                            IS_CHANGE = 1                                                                                                                  
                                                      WHERE ID = {BankID} AND ACCEPTOR_ID = {AcceptorID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Bank dəyişdirilmədi.",
                                                "UpdateBank");
        }
    }
}