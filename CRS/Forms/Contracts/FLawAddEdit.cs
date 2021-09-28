using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CRS.Forms.Contracts
{
    public partial class FLawAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FLawAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, LawID;

        public delegate void DoEvent();
        public event DoEvent RefreshLawsDataGridView;

        private void FLawAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                LoadLawDetails();
        }

        private void LoadLawDetails()
        {
            string s = "SELECT NAME,NOTE FROM CRS_USER.LAWS WHERE ID = " + LawID;
            try
            {
                DataTable dt = Class.GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr[0].ToString();
                    if (!String.IsNullOrEmpty(dr[1].ToString()))
                        NoteText.Text = dr[1].ToString();
                    else
                        NoteText.Text = null;
                }
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite("Məhkəmə açılmadı.", s, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlLawDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                XtraMessageBox.Show("Məhkəmənin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertLaw()
        {
            LawID = Class.GlobalFunctions.GetOracleSequenceValue("LAWS_SEQUENCE").ToString();
            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.LAWS(ID,NAME,NOTE) VALUES(" + LawID + ",'" + NameText.Text.Trim() + "','" + NoteText.Text.Trim() + "')",
                                                "Məhkəmə daxil edilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlLawDetails())
            {
                if (TransactionName == "INSERT")
                    InsertLaw();
                else
                    UpdateLaw();
                this.Close();
            }
        }

        private void FLawAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshLawsDataGridView();
        }

        private void UpdateLaw()
        {
            Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.LAWS SET NAME ='" + NameText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "'  WHERE ID = " + LawID,
                                                "Məhkəmə dəyişdirilmədi.");
        }
    }
}