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
    public partial class FPositionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPositionAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, PositionID;
        bool CurrentStatus = false, PositionUsed = false;
        int PositionUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshPositionDataGridView;

        private void FOperationAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                PositionID = Class.GlobalFunctions.GetOracleSequenceValue("POSITION_SEQUENCE").ToString();
            else
            {
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.POSITIONS", Class.GlobalVariables.V_UserID, "WHERE ID = " + PositionID + " AND USED_USER_ID = -1");
                if (Class.GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.POSITIONS WHERE ID = " + PositionID) > 0)
                {
                    PositionUsedUserID = Class.GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.POSITIONS WHERE ID = " + PositionID);
                    PositionUsed = true;
                }
                else
                    PositionUsed = false;

                if (PositionUsed)
                {
                    if (Class.GlobalVariables.V_UserID != PositionUsedUserID)
                    {
                        string used_user_name = Class.GlobalVariables.lstUsers.Find(u => u.ID == PositionUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş tip hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş tipin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadOperationTypeDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            AzNameText.Enabled = !status;
            EnNameText.Enabled = !status;
            RuNameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void LoadOperationTypeDetails()
        {
            string s = "SELECT NAME,NAME_EN,NAME_RU,NOTE FROM CRS_USER.POSITIONS WHERE ID = " + PositionID;
            try
            {
                DataTable dt = Class.GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    AzNameText.Text = dr[0].ToString();
                    EnNameText.Text = dr[1].ToString();
                    RuNameText.Text = dr[2].ToString();
                    NoteText.Text = dr[3].ToString();                    
                } 
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Vəzifə tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlOperationTypeDetails()
        {
            bool b = false;

            if (AzNameText.Text.Length == 0)
            {
                AzNameText.BackColor = Color.Red;
                XtraMessageBox.Show("Vəzifənin azərbaycan dilində adı daxil edilməyib.");
                AzNameText.Focus();
                AzNameText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (EnNameText.Text.Length == 0)
            {
                EnNameText.BackColor = Color.Red;
                XtraMessageBox.Show("Vəzifənin ingilis dilində adı daxil edilməyib.");
                EnNameText.Focus();
                EnNameText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RuNameText.Text.Length == 0)
            {
                RuNameText.BackColor = Color.Red;
                XtraMessageBox.Show("Vəzifənin rus dilində adı daxil edilməyib.");
                RuNameText.Focus();
                RuNameText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPosition()
        {
            int order = Class.GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.POSITIONS") + 1;
            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.POSITIONS(ID,NAME,NAME_EN,NAME_RU,NOTE,ORDER_ID) VALUES(" + PositionID + ",'" + AzNameText.Text.Trim() + "','" + EnNameText.Text.Trim() + "','" + RuNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + order + ")",
                                                "Vəzifə daxil edilmədi.");
        }

        private void UpdatePosition()
        {
            Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.POSITIONS SET NAME ='" + AzNameText.Text.Trim() + "',NAME_EN = '" + EnNameText.Text.Trim() + "',NAME_RU = '" + RuNameText.Text.Trim() + "',NOTE ='" + NoteText.Text.Trim() + "' WHERE ID = " + PositionID,
                                                "Vəzifə dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FOperationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.POSITIONS", -1, "WHERE ID = " + PositionID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            this.RefreshPositionDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlOperationTypeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPosition();
                else
                    UpdatePosition();
                this.Close();
            }
        }
    }
}