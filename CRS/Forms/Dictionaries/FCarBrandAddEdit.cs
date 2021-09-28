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
    public partial class FCarBrandAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCarBrandAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, BrandID;
        bool CurrentStatus = false, BrandUsed = false;
        int BrandUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCarBrandDataGridView;

        private void FCarBrandAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")                            
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_BRANDS", GlobalVariables.V_UserID, "WHERE ID = " + BrandID + " AND USED_USER_ID = -1");
                LoadCarBrandDetails();                
                BrandUsed = (BrandUsedUserID >= 0);
                
                if (BrandUsed)
                {
                    if (GlobalVariables.V_UserID != BrandUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BrandUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş marka hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş markanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadCarBrandDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BRANDS WHERE ID = {BrandID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarBrandDetails");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                BrandUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlCarBrandDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Markanın adı daxil edilməyib.");                
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCarBrand()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CAR_BRANDS") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CAR_BRANDS(ID,
                                                                                NAME,
                                                                                NOTE,
                                                                                ORDER_ID)
                                                VALUES(CAR_BRAND_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order})",
                                            "Marka daxil edilmədi.",
                                            this.Name + "/InsertCarBrand");
        }

        private void UpdateCarBrand()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CAR_BRANDS SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {BrandID}",
                                                "Marka dəyişdirilmədi.",
                                                this.Name + "/UpdateCarBrand");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCarBrandAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_BRANDS", -1, "WHERE ID = " + BrandID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCarBrandDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCarBrandDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCarBrand();
                else
                    UpdateCarBrand();
                this.Close();
            }
        }
    }
}