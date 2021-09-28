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
    public partial class FAcceptorBankBranchAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAcceptorBankBranchAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BranchID, BankID, AcceptorID;

        public delegate void DoEvent();
        public event DoEvent RefreshBranchesDataGridView;

        private void FAcceptorBankBranchAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                LoadBranchDetails();
        }

        private void LoadBranchDetails()
        {
            string s = $@"SELECT NAME,CODE,NOTE FROM CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP WHERE ACCEPTOR_ID = {AcceptorID} AND ID = {BranchID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Filial açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private bool ControlBranchDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Filialın adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Filialın kodu daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertBranch()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP(ID,
                                                                                                ACCEPTOR_ID,
                                                                                                BANK_ID,
                                                                                                NAME,
                                                                                                CODE,
                                                                                                NOTE,
                                                                                                ORDER_ID,
                                                                                                IS_CHANGE,
                                                                                                USED_USER_ID) 
                                                VALUES(TASK_ACCEPTOR_BB_SEQUENCE.NEXTVAL,
                                                        {AcceptorID},
                                                        {BankID},
                                                        '{NameText.Text.Trim()}',
                                                        '{CodeText.Text.Trim()}',
                                                        '{NoteText.Text.Trim()}',
                                                        {order},
                                                        1,
                                                        {GlobalVariables.V_UserID})",
                                                "Filial daxil edilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlBranchDetails())
            {
                if (TransactionName == "INSERT")
                    InsertBranch();
                else
                    UpdateBranch();
                this.Close();
            }
        }

        private void FAcceptorBankBranchAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshBranchesDataGridView();
        }

        private void UpdateBranch()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.TASK_ACCEPTOR_BB_TEMP SET IS_CHANGE = 1, 
                                                                                    NAME ='{NameText.Text.Trim()}',
                                                                                    CODE = '{CodeText.Text.Trim()}', 
                                                                                    NOTE = '{NoteText.Text.Trim()}'
                                                                                    WHERE ACCEPTOR_ID = {AcceptorID} AND 
                                                                                            ID = {BranchID} AND 
                                                                                            USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Filial dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}