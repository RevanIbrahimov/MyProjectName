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
    public partial class FBankBranchAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBankBranchAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BranchID, BankID;
        int status_id;

        public delegate void DoEvent();
        public event DoEvent RefreshBranchesDataGridView;

        private void FBankBranchAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if(GlobalVariables.V_UserID > 0)
                StatusLookUp.Properties.Buttons[1].Visible = GlobalVariables.Status;                      
            
            if (TransactionName == "EDIT")
            {
                StatusLookUp.Visible = StatusLabel.Visible = true;
                RefreshDictionaries(1);
                LoadBranchDetails();
            }
        }

        private void LoadBranchDetails()
        {
            string s = $@"SELECT BB.NAME,BB.CODE, BB.NOTE, S.STATUS_NAME
                              FROM CRS_USER_TEMP.BANK_BRANCHES_TEMP BB, CRS_USER.STATUS S
                             WHERE BB.STATUS_ID = S.ID AND BB.ID = {BranchID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                    StatusLookUp.EditValue = StatusLookUp.Properties.GetKeyValueByDisplayText(dr["STATUS_NAME"].ToString());                    
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
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.BANK_BRANCHES_TEMP(ID,
                                                                                            BANK_ID,
                                                                                            NAME,
                                                                                            CODE,
                                                                                            NOTE,
                                                                                            IS_CHANGE,
                                                                                            STATUS_ID,
                                                                                            USED_USER_ID)
                                                        VALUES(BANK_BRANCH_SEQUENCE.NEXTVAL,
                                                                {BankID},
                                                                '{NameText.Text.Trim()}',
                                                                '{CodeText.Text.Trim()}',
                                                                '{NoteText.Text.Trim()}',
                                                                1,
                                                                9,
                                                                {GlobalVariables.V_UserID})",
                                                "Filial daxil edilmədi.");
        }

        private void UpdateBranch()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_BRANCHES_TEMP SET IS_CHANGE = 1, 
                                                                                            NAME ='{NameText.Text.Trim()}',
                                                                                            CODE = '{CodeText.Text.Trim()}',
                                                                                            NOTE = '{NoteText.Text.Trim()}',
                                                                                            STATUS_ID = {StatusLookUp.EditValue} 
                                                                                            WHERE ID = {BranchID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Filial dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void FBankBranchAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {            
            this.RefreshBranchesDataGridView();
        }
        

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(StatusLookUp, "STATUS", "ID", "STATUS_NAME", "ID IN (9,10) ORDER BY ID");
        }

        private void StatusLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionariesForStatus("E", 1, "WHERE ID IN (9, 10)");
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