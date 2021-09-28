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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Contracts
{
    public partial class FNote : DevExpress.XtraEditors.XtraForm
    {
        public FNote()
        {
            InitializeComponent();
        }
        public string CustomerName, ContractCode, ContractID, CustomerID;

        public delegate void DoEvent(string contract_id);
        public event DoEvent RefreshContractDataGridView;
        
        bool CurrentStatus = false;

        private void FNote_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
            List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();
            if (GlobalVariables.V_UserID != contract.USED_USER_ID)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == contract.USED_USER_ID).FULLNAME;
                XtraMessageBox.Show(ContractCode + " saylı lizinq müqaviləsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun üçün qeyd yazıla bilməz. Siz yalnız qeydlərə baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CurrentStatus = true;
            }
            else
                CurrentStatus = false;

            CustomerNameText.Text = CustomerName;
            ContractCodeText.Text = ContractCode;
            LoadNoteDetails();
            ComponentEnable(CurrentStatus);
        }

        private void LoadNoteDetails()
        {
            string s = $@"SELECT NOTE FROM CRS_USER.CONTRACTS WHERE ID = {ContractID}";
            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadNoteDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NoteText.Text = dr[0].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Qeyd açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        public void ComponentEnable(bool status)
        {
            if (GlobalVariables.V_UserID > 0 && !status)
            {
                NoteText.Enabled = true;
            }
            else if (status)
                NoteText.Enabled = false;

            BOK.Visible = !status;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NoteText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                BOK_Click(sender, EventArgs.Empty);
            }
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {            
            if (NoteText.Text.Length <= 1400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (1400 - NoteText.Text.Length).ToString();
        }

        private void FNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshContractDataGridView(ContractID);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET NOTE = '{NoteText.Text.Trim()}',NOTE_CHANGE_USER = '{GlobalVariables.V_UserName}',NOTE_CHANGE_DATE = SYSDATE WHERE ID = {ContractID}",
                                                "Qeyd dəyişdirilmədi.",
                                          this.Name + "/BOK_Click");            
            this.Close();
        }
    }
}