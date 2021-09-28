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
    public partial class FCarBanTypeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCarBanTypeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TypeID;
        bool CurrentStatus = false, TypeUsed = false;
        int TypeUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCarBanTypeDataGridView;

        private void FCarBanTypeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")                
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_BAN_TYPES", GlobalVariables.V_UserID, "WHERE ID = " + TypeID + " AND USED_USER_ID = -1");
                LoadCarTypeDetails();
                TypeUsed = (TypeUsedUserID >= 0);                

                if (TypeUsed)
                {
                    if (GlobalVariables.V_UserID != TypeUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TypeUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş ban tipi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş ban tipinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            NameText.Enabled = 
            NoteText.Enabled = 
            BOK.Visible = !status;
        }

        private void LoadCarTypeDetails()
        {
            string s = "SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BAN_TYPES WHERE ID = " + TypeID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarTypeDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["NAME"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                    TypeUsedUserID = Convert.ToInt32(dr["USED_USER_ID"]);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ban tipi tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlCarBanTypeDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ban tipin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCarBanType()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CAR_BAN_TYPES") + 1;
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CAR_BAN_TYPES(ID,NAME,NOTE,ORDER_ID) VALUES(CAR_BAN_TYPE_SEQUENCE.NEXTVAL,'" + NameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + order + ")",
                                                "Ban tipi daxil edilmədi.");
        }

        private void UpdateCarBanType()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CAR_BAN_TYPES SET NAME ='" + NameText.Text.Trim() + "',NOTE ='" + NoteText.Text.Trim() + "' WHERE ID = " + TypeID,
                                                "Ban tipi dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCarBanTypeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCarBanType();
                else
                    UpdateCarBanType();
                this.Close();
            }
        }

        private void FCarBanTypeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_BAN_TYPES", -1, "WHERE ID = " + TypeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCarBanTypeDataGridView();
        }
    }
}