﻿using System;
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

namespace CRS.Forms.Dictionaries
{
    public partial class FGoldTypeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FGoldTypeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, GoldTypeID;
        bool CurrentStatus = false, NameUsed = false;
        int NameUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FGoldTypeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.GOLD_TYPE", GlobalVariables.V_UserID, "WHERE ID = " + GoldTypeID + " AND USED_USER_ID = -1");
                LoadDetails();
                NameUsed = (NameUsedUserID >= 0);

                if (NameUsed)
                {
                    if (GlobalVariables.V_UserID != NameUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == NameUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş əyyar hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş əyyarın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadDetails()
        {
            string s = $@"SELECT NAME,NOTE,COEFFICIENT,AMOUNT,USED_USER_ID FROM CRS_USER.GOLD_TYPE WHERE ID = {GoldTypeID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Əyyarın adı açılmadı.");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                CoeffecientValue.EditValue = dt.Rows[0]["COEFFICIENT"];
                AmountValue.EditValue = dt.Rows[0]["AMOUNT"];
                NameUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private bool ControlNameDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əyyarın adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CoeffecientValue.Value <= 0)
            {
                CoeffecientValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əyyarın əmsalı sıfırdan böyük olmalıdır.");
                CoeffecientValue.Focus();
                CoeffecientValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlNameDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void FGoldTypeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.GOLD_TYPE", -1, "WHERE ID = " + GoldTypeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void Insert()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.GOLD_TYPE(NAME,NOTE,COEFFICIENT,AMOUNT,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{Math.Round(CoeffecientValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},{Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},{GlobalVariables.V_UserID})",
                                                 "Əyyar daxil edilmədi.",
                                                 this.Name + "/Insert");
        }

        private void Update()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.GOLD_TYPE SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}',COEFFICIENT = {Math.Round(CoeffecientValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},AMOUNT={Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)}, UPDATE_USER = {GlobalVariables.V_UserID}, UPDATE_DATE = SYSDATE WHERE ID = {GoldTypeID}",
                                                 "Əyyar dəyişdirilmədi.",
                                                 this.Name + "/Update");
        }
    }
}