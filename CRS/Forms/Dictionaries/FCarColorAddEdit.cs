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
    public partial class FCarColorAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCarColorAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ColorID;
        bool CurrentStatus = false, ColorUsed = false;
        int ColorUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCarColorDataGridView;

        private void FCarColorAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_COLORS", GlobalVariables.V_UserID, "WHERE ID = " + ColorID + " AND USED_USER_ID = -1");
                LoadCarColorDetails();
                ColorUsed = (ColorUsedUserID > 0);
                
                if (ColorUsed)
                {
                    if (GlobalVariables.V_UserID != ColorUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ColorUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş rəng hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş rəngin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);                
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void LoadCarColorDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_COLORS WHERE ID = {ColorID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarColorDetails");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                ColorUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlCarColorDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Rəng daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCarColor()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CAR_COLORS") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CAR_COLORS(ID,NAME,NOTE,ORDER_ID) VALUES(CAR_COLOR_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order})",
                                                "Rəng daxil edilmədi.",
                                            this.Name + "/InsertCarColor");
        }

        private void UpdateCarColor()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CAR_COLORS SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {ColorID}",
                                                "Rəng dəyişdirilmədi.",
                                                this.Name + "/UpdateCarColor");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCarColorAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_COLORS", -1, "WHERE ID = " + ColorID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCarColorDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCarColorDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCarColor();
                else
                    UpdateCarColor();
                this.Close();
            }
        }
    }
}