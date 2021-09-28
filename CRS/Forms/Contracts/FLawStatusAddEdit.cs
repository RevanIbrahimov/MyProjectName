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

namespace CRS.Forms.Contracts
{
    public partial class FLawStatusAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FLawStatusAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, StatusID;

        public delegate void DoEvent();
        public event DoEvent RefreshLawStatusDataGridView;

        private void FLawStatusAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")                
                LoadLawStatusDetails();
        }

        private void LoadLawStatusDetails()
        {
            string s = $@"SELECT NAME,NOTE FROM CRS_USER.LAW_STATUS WHERE ID = {StatusID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLawStatusDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["NAME"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();                    
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Status açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlLawStatusDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Statusun adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertLawStatus()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.LAW_STATUS(NAME,NOTE) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}')",
                                                "Status daxil edilmədi.",
                                            this.Name + "/InsertLawStatus");
        }

        private void UpdateLawStatus()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.LAW_STATUS SET NAME ='{NameText.Text.Trim()}',NOTE = '{NoteText.Text.Trim()}'  WHERE ID = {StatusID}",
                                                "Status dəyişdirilmədi.",
                                            this.Name + "/UpdateLawStatus");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlLawStatusDetails())
            {
                if (TransactionName == "INSERT")
                    InsertLawStatus();
                else
                    UpdateLawStatus();
                this.Close();
            }
        }

        private void FLawStatusAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshLawStatusDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}