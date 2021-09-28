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
    public partial class FBirthplaceAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBirthplaceAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BirthplaceID;
        bool CurrentStatus = false, BirthplaceUsed = false;
        int BirthplaceUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshBirthplaceDataGridView;

        private void FBirthplaceAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")               
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BIRTHPLACE", Class.GlobalVariables.V_UserID, "WHERE ID = " + BirthplaceID + " AND USED_USER_ID = -1");
                BirthplaceUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.BIRTHPLACE WHERE ID = {BirthplaceID}");
                BirthplaceUsed = (BirthplaceUsedUserID >= 0); 

                if (BirthplaceUsed)
                {
                    if (GlobalVariables.V_UserID != BirthplaceUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BirthplaceUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş doğum yeri hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş doğum yerinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadSeriesDetails();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = 
                NoteText.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadSeriesDetails()
        {
            string s = $@"SELECT NAME,NOTE FROM CRS_USER.BIRTHPLACE WHERE ID = {BirthplaceID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["NAME"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Doğum yeri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlBirthplaceDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Doğum yerinin adı daxil edilməyib.");               
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertBirthplace()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.BIRTHPLACE") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.BIRTHPLACE(ID,NAME,NOTE,ORDER_ID) VALUES(BIRTHPLACE_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order})",
                                                "Seriya daxil edilmədi.");
        }

        private void UpdateBirthplace()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BIRTHPLACE SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {BirthplaceID}",
                                                "Seriya dəyişdirilmədi.");
        }

        private void FBirthplaceAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BIRTHPLACE", -1, "WHERE ID = " + BirthplaceID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshBirthplaceDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlBirthplaceDetails())
            {
                if (TransactionName == "INSERT")
                    InsertBirthplace();
                else
                    UpdateBirthplace();
                this.Close();
            }
        }
    }
}