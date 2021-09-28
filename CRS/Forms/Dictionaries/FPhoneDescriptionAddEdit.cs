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
    public partial class FPhoneDescriptionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPhoneDescriptionAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, DescriptionID;
        bool CurrentStatus = false, PhoneUsed = false;
        string PrefixID;
        int PhoneUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshPhoneDescriptionDataGridView;

        private void FPhoneDescriptionAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                DescriptionID = GlobalFunctions.GetOracleSequenceValue("PHONE_DESCRIPTION_SEQUENCE").ToString();
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PHONE_DESCRIPTIONS", GlobalVariables.V_UserID, "WHERE ID = " + DescriptionID + " AND USED_USER_ID = -1");
                LoadDescriptionDetails();
                PhoneUsed = (PhoneUsedUserID > 0);                

                if (PhoneUsed)
                {
                    if (GlobalVariables.V_UserID != PhoneUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PhoneUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş təsvir hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş təsvirin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);                
                InsertpPrefixTemp();
                LoadPrefixDataGridView();
            }
        }

        private void ComponentEnabled(bool status)
        {
            AzNameText.Enabled = 
            EnNameText.Enabled = 
            RuNameText.Enabled = 
            BOK.Visible = 
            StandaloneBarDockControl.Enabled = !status;
            if (status)
                PopupMenu.Manager = null;
            else
                PopupMenu.Manager = BarManager;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadDescriptionDetails()
        {
            string s = $@"SELECT DESCRIPTION_AZ,DESCRIPTION_EN,DESCRIPTION_RU,USED_USER_ID FROM CRS_USER.PHONE_DESCRIPTIONS WHERE ID = {DescriptionID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDescriptionDetails", "Telefon nömrəsinin təsviri açılmadı.");
            if(dt.Rows.Count > 0)
            {
                AzNameText.Text = dt.Rows[0]["DESCRIPTION_AZ"].ToString();
                EnNameText.Text = dt.Rows[0]["DESCRIPTION_EN"].ToString();
                RuNameText.Text = dt.Rows[0]["DESCRIPTION_RU"].ToString();
                PhoneUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlDescriptionDetails()
        {
            bool b = false;

            if (AzNameText.Text.Length == 0)
            {
                AzNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Azərbaycan dilində telefon nömrəsinin təsviri daxil edilməyib.");                
                AzNameText.Focus();
                AzNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (EnNameText.Text.Length == 0)
            //{
            //    EnNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("İngilis dilində telefon nömrəsinin təsviri daxil edilməyib.");                
            //    EnNameText.Focus();
            //    EnNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (RuNameText.Text.Length == 0)
            //{
            //    RuNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Rus dilində telefon nömrəsinin təsviri daxil edilməyib.");                
            //    RuNameText.Focus();
            //    RuNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void LoadPrefixDataGridView()
        {
            string s = $@"SELECT 1 SS,ID,PREFIX,NOTE FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE IS_CHANGE <> 2 AND PHONE_DESCRIPTION_ID = {DescriptionID}";

            PrefixGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            EditPrefixBarButton.Enabled = DeletePrefixBarButton.Enabled = (PrefixGridView.RowCount > 0);
        }

        private void InsertDescription()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.PHONE_DESCRIPTIONS(ID,DESCRIPTION_AZ,DESCRIPTION_EN,DESCRIPTION_RU) VALUES(" + DescriptionID + ",'" + AzNameText.Text.Trim() + "','" + EnNameText.Text.Trim() + "','" + RuNameText.Text.Trim() + "')",
                                                "Telefon nömrəsinin təsviri daxil edilmədi.",
                                                this.Name + "/InsertDescription");
        }

        private void UpdateDescription()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.PHONE_DESCRIPTIONS SET DESCRIPTION_AZ ='" + AzNameText.Text.Trim() + "',DESCRIPTION_EN='" + EnNameText.Text.Trim() + "',DESCRIPTION_RU='" + RuNameText.Text.Trim() + "' WHERE ID = " + DescriptionID,
                                                "Telefon nömrəsinin təsviri dəyişdirilmədi.",
                                                this.Name + "/UpdateDescription");
        }

        private void DeletePrefixTemp()
        {
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE PHONE_DESCRIPTION_ID = {DescriptionID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                "Prefikslər temp cədvəldən silinmədi.",
                                this.Name + "/DeletePrefixTemp");
        }

        private void FPhoneDescriptionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PHONE_DESCRIPTIONS", -1, "WHERE ID = " + DescriptionID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            DeletePrefixTemp();
            this.RefreshPhoneDescriptionDataGridView();
        }

        private void InsertpPrefix()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONE_PREFIXS WHERE PHONE_DESCRIPTION_ID = {DescriptionID}",
                                             $@"INSERT INTO CRS_USER.PHONE_PREFIXS(ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE)SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE PHONE_DESCRIPTION_ID = {DescriptionID}",
                                                    "Prefikslər əsas cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertpPrefix");
        }

        private void InsertpPrefixTemp()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER_TEMP.PHONE_PREFIXS_TEMP WHERE PHONE_DESCRIPTION_ID = {DescriptionID}",
                                             $@"INSERT INTO CRS_USER_TEMP.PHONE_PREFIXS_TEMP(ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,USED_USER_ID)SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,{GlobalVariables.V_UserID} FROM CRS_USER.PHONE_PREFIXS WHERE PHONE_DESCRIPTION_ID = {DescriptionID}",
                                                   "Prefikslər temp cədvələ daxil edilmədi.",
                                                   this.Name + "/InsertpPrefixTemp");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDescriptionDetails())
            {
                if (TransactionName == "INSERT")
                    InsertDescription();
                else
                    UpdateDescription();
                InsertpPrefix();
                this.Close();
            }
        }

        void RefreshPrefix()
        {
            LoadPrefixDataGridView();
        }

        private void LoadFPhonePrefixAddEdit(string transactionname, string prefixid, string descriptionid)
        {
            FPhonePrefixAddEdit fgcs = new FPhonePrefixAddEdit();
            fgcs.TransactionName = transactionname;
            fgcs.DescriptionID = descriptionid;
            fgcs.PrefixID = prefixid;
            fgcs.RefreshPrefixDataGridView += new FPhonePrefixAddEdit.DoEvent(RefreshPrefix);
            fgcs.ShowDialog();
        }

        private void NewPrefixBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhonePrefixAddEdit("INSERT", null, DescriptionID);
        }

        private void EditPrefixBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhonePrefixAddEdit("EDIT", PrefixID, DescriptionID);
        }

        private void PrefixGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
        
        private void DeletePhonePrefix()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT (*) FROM (SELECT PHONE_PREFIX_ID FROM CRS_USER.PHONES WHERE PHONE_PREFIX_ID = {PrefixID} UNION ALL 
                                                                       SELECT DISTINCT PHONE_PREFIX_ID FROM CRS_USER_TEMP.PHONES_TEMP WHERE PHONE_PREFIX_ID = {PrefixID})");

            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş prefiksi silmək istəyirsiniz?", "Prefiksin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONE_PREFIXS_TEMP SET IS_CHANGE = 2 WHERE ID = {PrefixID}", "Prefiks silinmədi.", this.Name + "/DeletePhonePrefix");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş prefiks bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeletePrefixBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhonePrefix();
            LoadPrefixDataGridView();
        }

        private void RefreshPrefixBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPrefixDataGridView();
        }

        private void PrefixGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPrefixBarButton.Enabled && StandaloneBarDockControl.Enabled)
                LoadFPhonePrefixAddEdit("EDIT", PrefixID, DescriptionID);
        }

        private void PrefixGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PrefixGridView.GetFocusedDataRow();
            if (row != null)
                PrefixID = row["ID"].ToString();
        }

        private void PrefixGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PrefixGridView, PopupMenu, e);
        }
    }
}