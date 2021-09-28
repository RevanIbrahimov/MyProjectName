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
    public partial class FCountryAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCountryAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CountryID;
        bool CurrentStatus = false, CountryUsed = false;
        int CountryUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCountriesDataGridView;

        private void FCountryAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.COUNTRIES", GlobalVariables.V_UserID, "WHERE ID = " + CountryID + " AND USED_USER_ID = -1");
                LoadCountriesDetails();
                CountryUsed = (CountryUsedUserID >= 0);
                
                if (CountryUsed)
                {
                    if (GlobalVariables.V_UserID != CountryUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CountryUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş ölkə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş ölkənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            AzNameText.Enabled = 
                EnNameText.Enabled = 
                RuNameText.Enabled = 
                NoteText.Enabled = 
                CodeValue.Enabled =
                BOK.Visible = !status;
        }

        private void LoadCountriesDetails()
        {
            string s = $@"SELECT NAME,NAME_EN,NAME_RU,NOTE,CODE,USED_USER_ID FROM CRS_USER.COUNTRIES WHERE ID = {CountryID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCountriesDetails", "Ölkə tapılmadı.");
            if(dt.Rows.Count > 0)
            {
                AzNameText.Text = dt.Rows[0]["NAME"].ToString();
                EnNameText.Text = dt.Rows[0]["NAME_EN"].ToString();
                RuNameText.Text = dt.Rows[0]["NAME_RU"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                CodeValue.Value = Convert.ToInt32(dt.Rows[0]["CODE"]);
                CountryUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlCountryDetails()
        {
            bool b = false;

            if (AzNameText.Text.Length == 0)
            {
                AzNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ölkənin azərbaycan dilində adı daxil edilməyib.");
                AzNameText.Focus();
                AzNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (EnNameText.Text.Length == 0)
            //{
            //    EnNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Ölkənin ingilis dilində adı daxil edilməyib.");
            //    EnNameText.Focus();
            //    EnNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (RuNameText.Text.Length == 0)
            //{
            //    RuNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Ölkənin rus dilində adı daxil edilməyib.");
            //    RuNameText.Focus();
            //    RuNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            if (CodeValue.Value < 0)
            {
                CodeValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ölkənin kodu sıfırdan böyük olmalıdır.");
                CodeValue.Focus();
                CodeValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCountry()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.COUNTRIES") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.COUNTRIES(ID,NAME,NAME_EN,NAME_RU,NOTE,CODE,ORDER_ID) VALUES(COUNTRY_SEQUENCE.NEXTVAL,'" + AzNameText.Text.Trim() + "','" + EnNameText.Text.Trim() + "','" + RuNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + CodeValue.Value + "," + order + ")",
                                                "Ölkə daxil edilmədi.",
                                                this.Name + "/InsertCountry");
        }

        private void UpdateCountry()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.COUNTRIES SET NAME ='" + AzNameText.Text.Trim() + "',NAME_EN = '" + EnNameText.Text.Trim() + "',NAME_RU = '" + RuNameText.Text.Trim() + "',NOTE ='" + NoteText.Text.Trim() + "',CODE = " + CodeValue.Value + " WHERE ID = " + CountryID,
                                                "Ölkə dəyişdirilmədi.",
                                                this.Name + "/UpdateCountry");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCountryDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCountry();
                else
                    UpdateCountry();
                this.Close();
            }
        }

        private void FCountryAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.COUNTRIES", -1, "WHERE ID = " + CountryID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCountriesDataGridView();
        }
    }
}