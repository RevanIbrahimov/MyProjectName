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
    public partial class FPhonePrefixAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPhonePrefixAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, PrefixID, DescriptionID;

        public delegate void DoEvent();
        public event DoEvent RefreshPrefixDataGridView;

        private void FPhonePrefixAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                LoadPrefixDetails();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPhonePrefixAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPrefixDataGridView();
        }

        public void InsertPhonePrefix()
        {           
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.PHONE_PREFIXS_TEMP(ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,IS_CHANGE,USED_USER_ID)VALUES(PHONE_PREFIX_SEQUENCE.NEXTVAL," + DescriptionID + ",'" + PrefixText.Text.Trim() + "','" + NoteText.Text.Trim() + "',1," + Class.GlobalVariables.V_UserID + ")",
                                                "Prefiks temp cədvələ daxil edilmədi.",
                                                this.Name + "/InsertPhonePrefix");
        }

        public void UpdatePhonePrefix()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PHONE_PREFIXS_TEMP SET IS_CHANGE = 1, PREFIX = '" + PrefixText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + PrefixID,
                                                "Prefiks temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdatePhonePrefix");
        }

        private void LoadPrefixDetails()
        {
            string s = $@"SELECT PREFIX,NOTE FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE ID = {PrefixID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPrefixDetails", "Prefiks açılmadı.");
            if(dt.Rows.Count > 0)
            {
                PrefixText.Text = dt.Rows[0]["PREFIX"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
            }            
        }

        private void PrefixHyperlinkLabel_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link);
        }

        private bool ControlPhonePrefixDetails()
        {
            bool b = false; int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE PHONE_DESCRIPTION_ID = {DescriptionID} AND PREFIX = '{PrefixText.Text}'");

            if (PrefixText.Text.Length < 3)
            {
                PrefixText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Prefiks 3 simvol olmalıdır");                
                PrefixText.Focus();
                PrefixText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((TransactionName == "INSERT") && (a > 0))
            {
                PrefixText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxil etdiyiniz " + PrefixText.Text + " prefiksi artıq bazada mövcuddur.");                
                PrefixText.Focus();
                PrefixText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPhonePrefixDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPhonePrefix();
                else
                    UpdatePhonePrefix();
                this.Close();
            }
        }
    }
}