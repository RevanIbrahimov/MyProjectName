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
    public partial class FCurrencyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCurrencyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CurrencyID;
        bool CurrentStatus = false, CurrencyUsed = false;
        int CurrencyUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCurrencyDataGridView;

        private void FCurrencyAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")                
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CURRENCY", GlobalVariables.V_UserID, "WHERE ID = " + CurrencyID + " AND USED_USER_ID = -1");
                LoadCurrencyDetails();
                CurrencyUsed = (CurrencyUsedUserID >= 0);

                if (CurrencyUsed)
                {
                    if (GlobalVariables.V_UserID != CurrencyUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CurrencyUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş valyuta hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş valyutanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadCurrencyDetails()
        {
            string s = $@"SELECT CODE,NOTE,VALUE,NAME,SHORT_NAME,SMALL_NAME,USED_USER_ID FROM CRS_USER.CURRENCY WHERE ID = {CurrencyID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCurrencyDetails", "Valyuta tapılmadı.");
            if (dt.Rows.Count > 0)
            {
                CodeText.Text = dt.Rows[0]["CODE"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                CurrencyValue.Value = Convert.ToInt32(dt.Rows[0]["VALUE"]);
                LongNameText.Text = dt.Rows[0]["NAME"].ToString();
                ShortNameText.Text = dt.Rows[0]["SHORT_NAME"].ToString();
                SmallNameText.Text = dt.Rows[0]["SMALL_NAME"].ToString();
                CurrencyUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private bool ControlCurrencyDetails()
        {
            bool b = false;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyutanın kodu daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((CurrencyValue.Value == 0) || (String.IsNullOrEmpty(CurrencyValue.Text)))
            {
                CurrencyValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Dəyər daxil edilməyib.");
                CurrencyValue.Focus();
                CurrencyValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LongNameText.Text.Length == 0)
            {
                LongNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyutanın adı daxil edilməyib.");
                LongNameText.Focus();
                LongNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ShortNameText.Text.Length == 0)
            {
                ShortNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyutanın qısa adı daxil edilməyib.");
                ShortNameText.Focus();
                ShortNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SmallNameText.Text.Length == 0)
            {
                SmallNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyutanın ən kiçik ölçü vahidi daxil edilməyib.");
                SmallNameText.Focus();
                SmallNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void ComponentEnabled(bool status)
        {
            CodeText.Enabled = 
            CurrencyValue.Enabled = 
            LongNameText.Enabled = 
            ShortNameText.Enabled = 
            SmallNameText.Enabled = 
            NoteText.Enabled = 
            BOK.Visible = !status;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InsertCurrency()
        {
            int order = GlobalFunctions.GetMax("SELECT MAX(ORDER_ID) FROM CRS_USER.CURRENCY") + 1;
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CURRENCY(ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE,ORDER_ID) VALUES(CURRENCY_SEQUENCE.NEXTVAL,'" + CodeText.Text.Trim() + "'," + CurrencyValue.Value + ",'" + LongNameText.Text.Trim() + "','" + ShortNameText.Text.Trim() + "','" + SmallNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + order + ")",
                                                "Valyuta bazaya daxil edilmədi.",
                                                this.Name + "/InsertCurrency");
        }

        private void UpdateCurrency()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CURRENCY SET CODE = '" + CodeText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',VALUE = " + CurrencyValue.Value + ",NAME = '" + LongNameText.Text.Trim() + "',SHORT_NAME = '" + ShortNameText.Text.Trim() + "',SMALL_NAME = '" + SmallNameText.Text.Trim() + "' WHERE ID = " + CurrencyID,
                                                "Valyuta dəyişdirilmədi.",
                                                this.Name + "/UpdateCurrency");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCurrencyDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCurrency();
                else
                    UpdateCurrency();
                this.Close();
            }
        }

        private void FCurrencyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CURRENCY", -1, "WHERE ID = " + CurrencyID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCurrencyDataGridView();
        }
    }
}