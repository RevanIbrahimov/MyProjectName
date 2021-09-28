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
    public partial class FCarModelAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCarModelAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ModelID;
        bool CurrentStatus = false, ModelUsed = false;
        int ModelUsedUserID = -1, brand_id;

        public delegate void DoEvent();
        public event DoEvent RefreshCarModelDataGridView;

        private void FCarModelAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillComboBoxEdit(BrandComboBox, "CAR_BRANDS", "NAME,NAME,NAME", null);
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_MODELS", Class.GlobalVariables.V_UserID, "WHERE ID = " + ModelID + " AND USED_USER_ID = -1");
                LoadCarModelDetails();
                ModelUsed = (ModelUsedUserID >= 0);                

                if (ModelUsed)
                {
                    if (GlobalVariables.V_UserID != ModelUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ModelUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş model hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş modelin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            BrandComboBox.Enabled = !status;
            NameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void LoadCarModelDetails()
        {
            string s = $@"SELECT M.NAME MODEL_NAME,M.NOTE,B.NAME BRAND_NAME,M.USED_USER_ID FROM CRS_USER.CAR_BRANDS B,CRS_USER.CAR_MODELS M WHERE B.ID = M.BRAND_ID AND M.ID = {ModelID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarModelDetails");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["MODEL_NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                BrandComboBox.EditValue = dt.Rows[0]["BRAND_NAME"].ToString();
                ModelUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }                       
        }

        private bool ControlCarModelDetails()
        {
            bool b = false;

            if (BrandComboBox.Text.Length == 0)
            {
                BrandComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Marka daxil edilməyib.");
                BrandComboBox.Focus();
                BrandComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Model daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCarModel()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CAR_MODELS") + 1;
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CAR_MODELS(ID,NAME,NOTE,BRAND_ID,ORDER_ID) VALUES(CAR_MODEL_SEQUENCE.NEXTVAL,'" + NameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + brand_id + "," + order + ")",
                                                "Model daxil edilmədi.",
                                                this.Name + "/InsertCarModel");
        }

        private void UpdateCarModel()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CAR_MODELS SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}',BRAND_ID = {brand_id} WHERE ID = {ModelID}",
                                                "Model dəyişdirilmədi.",
                                                 this.Name + "/UpdateCarModel");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BrandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            brand_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_BRANDS", "NAME", "ID", BrandComboBox.Text);
        }

        private void FCarModelAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_MODELS", -1, "WHERE ID = " + ModelID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCarModelDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCarModelDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCarModel();
                else
                    UpdateCarModel();
                this.Close();
            }
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillComboBoxEdit(BrandComboBox, "CAR_BRANDS", "NAME,NAME,NAME", null);            
        }

        private void LoadDictionaries(string transaction, int index, int hostage_index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.HostageSelectedTabIndex = hostage_index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void BrandComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 8, 0);
        }
    }
}