using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FPhoneAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPhoneAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OwnerID, OwnerType, PhoneID;

        int DescriptionID, CountryID, CountryCode;
        int? KindShipRateID = null;

        public delegate void DoEvent();
        public event DoEvent RefreshPhonesDataGridView;

        private void FPhoneAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                CountryComboBox.Properties.Buttons[1].Visible = GlobalVariables.Countries;
                DescriptionComboBox.Properties.Buttons[1].Visible = GlobalVariables.PhoneDescription;
            }
            
            GlobalProcedures.FillComboBoxEdit(CountryComboBox, "COUNTRIES", "NAME,NAME_EN,NAME_RU", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillComboBoxEdit(DescriptionComboBox, "PHONE_DESCRIPTIONS", "DESCRIPTION_AZ,DESCRIPTION_EN,DESCRIPTION_RU", null);
            GlobalProcedures.FillLookUpEdit(KindShipRateLookUp, "KINDSHIP_RATE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            CountryComboBox.SelectedIndex = 0;
            this.ActiveControl = DescriptionComboBox;
            if (TransactionName == "EDIT")
                LoadPhoneDetails();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void InsertPhone()
        {
            string kindShipRateID = (KindShipRateID == null) ? "null" : KindShipRateID.ToString();
            int order = GlobalFunctions.GetMax($@"SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_ID = {OwnerID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND OWNER_TYPE = '{OwnerType}'", this.Name + "/InsertPhone") + 1;            
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.PHONES_TEMP(ID,PHONE_DESCRIPTION_ID,COUNTRY_ID,PHONE_NUMBER,OWNER_ID,OWNER_TYPE,USED_USER_ID,IS_CHANGE,ORDER_ID,KINDSHIP_RATE_ID,KINDSHIP_NAME)VALUES(PHONES_SEQUENCE.NEXTVAL," + DescriptionID + "," + CountryID + ",'" + PhoneText.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "") + "'," + OwnerID + ",'" + OwnerType + "'," + GlobalVariables.V_UserID + ",1," + order + "," + kindShipRateID + ",'" + KindShipNameText.Text.Trim() + "')",
                                                "Telefon temp cədvələ daxil edilmədi.",
                                           this.Name + "/InsertPhone");
        }

        private void UpdatePhone()
        {
            string kindShipRateID = (KindShipRateID == null) ? "null" : KindShipRateID.ToString();
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET PHONE_DESCRIPTION_ID = {DescriptionID},PHONE_NUMBER = '{PhoneText.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "")}',COUNTRY_ID = {CountryID},IS_CHANGE = 1,KINDSHIP_RATE_ID = {kindShipRateID}, KINDSHIP_NAME = '{KindShipNameText.Text.Trim()}' WHERE USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {PhoneID}",
                                                "Telefon temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdatePhone");
        }

        private void LoadPhoneDetails()
        {
            string s = null;

            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": s = $@"SELECT PD.DESCRIPTION_AZ,P.PHONE_NUMBER,PP.NAME,PP.CODE,KR.NAME KINDSHIP_RATE_NAME,P.KINDSHIP_NAME FROM CRS_USER_TEMP.PHONES_TEMP P,CRS_USER.PHONE_DESCRIPTIONS PD,CRS_USER.COUNTRIES PP,CRS_USER.KINDSHIP_RATE KR WHERE P.PHONE_DESCRIPTION_ID = PD.ID AND P.COUNTRY_ID = PP.ID AND P.KINDSHIP_RATE_ID = KR.ID(+) AND P.USED_USER_ID = {GlobalVariables.V_UserID} AND P.ID = {PhoneID}";
                    break;
                case "EN": s = $@"SELECT PD.DESCRIPTION_EN,P.PHONE_NUMBER,PP.NAME_EN,PP.CODE,KR.NAME KINDSHIP_RATE_NAME,P.KINDSHIP_NAME FROM CRS_USER_TEMP.PHONES_TEMP P,CRS_USER.PHONE_DESCRIPTIONS PD,CRS_USER.COUNTRIES PP,CRS_USER.KINDSHIP_RATE KR WHERE P.PHONE_DESCRIPTION_ID = PD.ID AND P.COUNTRY_ID = PP.ID AND P.KINDSHIP_RATE_ID = KR.ID(+) AND P.USED_USER_ID = {GlobalVariables.V_UserID} AND P.ID = {PhoneID}";
                    break;
                case "RU": s = $@"SELECT PD.DESCRIPTION_RU,P.PHONE_NUMBER,PP.NAME_RU,PP.CODE,KR.NAME KINDSHIP_RATE_NAME,P.KINDSHIP_NAME FROM CRS_USER_TEMP.PHONES_TEMP P,CRS_USER.PHONE_DESCRIPTIONS PD,CRS_USER.COUNTRIES PP,CRS_USER.KINDSHIP_RATE KR WHERE P.PHONE_DESCRIPTION_ID = PD.ID AND P.COUNTRY_ID = PP.ID AND P.KINDSHIP_RATE_ID = KR.ID(+) AND P.USED_USER_ID = {GlobalVariables.V_UserID} AND P.ID = {PhoneID}";
                    break;
            }

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    DescriptionComboBox.EditValue = dr[0].ToString();
                    PhoneText.Text = dr[1].ToString();
                    CountryComboBox.EditValue = dr[2].ToString();
                    CodeText.Text = "+" + dr[3];
                    KindShipRateLookUp.EditValue = KindShipRateLookUp.Properties.GetKeyValueByDisplayText(dr["KINDSHIP_RATE_NAME"].ToString());
                    KindShipNameText.Text = dr["KINDSHIP_NAME"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefonunun detalları açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlPhoneDetails()
        {
            bool b = false;           

            if (String.IsNullOrEmpty(CountryComboBox.Text))
            {
                CountryComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ölkə seçilməyib.");               
                CountryComboBox.Focus();
                CountryComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DescriptionComboBox.Text))
            {
                DescriptionComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təsvir seçilməyib.");                
                DescriptionComboBox.Focus();
                DescriptionComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((CountryCode == 994) && ((TransactionName == "INSERT" && PhoneText.Text.Length < 13) || (TransactionName == "EDIT" && PhoneText.Text.Length < 9) || (PhoneText.Text == "") || (PhoneText.Text == "(__)___-__-__") || (PhoneText.Text[0] == '0')))
            {
                PhoneText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Nömrə düz deyil.");                
                PhoneText.Focus();
                PhoneText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CountryCode == 994)
            {
                string phone_prefix = null, phone = PhoneText.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "");
                phone_prefix = "0" + phone.Substring(0, 2);
                List<PhonePrefixs> lstPrefix = PhonePrefixsDAL.SelectPrefixByDescription(DescriptionID).ToList<PhonePrefixs>();
                var prefix = lstPrefix.Find(p => p.PREFIX.Trim() == phone_prefix);
                if (prefix == null)
                {
                    PhoneText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Nömrənin prefix-i təsvir ilə uyğun deyil və ya bazada yoxdur. Zəhmət olmasa nömrənin mobil və ya şəhər nömrəsi olduğunu düzgün seçin.");                    
                    PhoneText.Focus();
                    PhoneText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;
            }

            if(KindShipRateID != null && KindShipNameText.Text.Length == 0)
            {
                KindShipNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Nömrənin sahibinin adı daxil edilməyib.");
                KindShipNameText.Focus();
                KindShipNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPhoneDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPhone();
                else
                    UpdatePhone();
                this.Close();
            }
        }

        private void FPhoneAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPhonesDataGridView();
        }

        private void FindDescriptionID()
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": DescriptionID = GlobalFunctions.FindComboBoxSelectedValue("PHONE_DESCRIPTIONS", "DESCRIPTION_AZ", "ID", DescriptionComboBox.Text);
                    break;
                case "EN": DescriptionID = GlobalFunctions.FindComboBoxSelectedValue("PHONE_DESCRIPTIONS", "DESCRIPTION_EN", "ID", DescriptionComboBox.Text);
                    break;
                case "RU": DescriptionID = GlobalFunctions.FindComboBoxSelectedValue("PHONE_DESCRIPTIONS", "DESCRIPTION_RU", "ID", DescriptionComboBox.Text);
                    break;
            }
        }

        private void DescriptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindDescriptionID();
            PhoneText.Focus();
        }

        private void PrefixComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Countries> lstCountries = CountriesDAL.SelectCountryByName(CountryComboBox.Text).ToList<Countries>();
            var country = lstCountries.First();
            CountryID = country.ID;
            CountryCode = country.CODE;
            CodeText.Text = "+" + CountryCode.ToString();

            if (country.CODE == 994)
            {
                PhoneText.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                PhoneText.Properties.Mask.EditMask = "(00)000-00-00";
            }
            else
            {
                PhoneText.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                PhoneText.Properties.Mask.EditMask = null;
            }
        }

        private void KindShipRateLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (KindShipRateLookUp.EditValue == null)
            {
                KindShipRateID = null;
                KindShipNameText.Enabled = KindShipNameStartLabel.Visible = false;
                return;
            }
            else
                KindShipNameText.Enabled = KindShipNameStartLabel.Visible = true;

            KindShipRateID = Convert.ToInt32(KindShipRateLookUp.EditValue);
        }

        private void KindShipRateLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionariesForDescription("E", 17);
            else if(e.Button.Index == 2)
            {
                GlobalProcedures.DeleteLookUpEditSelectedValue(KindShipRateLookUp);
                KindShipNameText.Text = null;                
            }
        }

        private void PrefixComboBox_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            PhoneText.Focus();
        }

        void RefreshDescription(int index)
        {
            switch (index)
            {
                case 0: GlobalProcedures.FillComboBoxEdit(DescriptionComboBox, "PHONE_DESCRIPTIONS", "DESCRIPTION_AZ,DESCRIPTION_EN,DESCRIPTION_RU", null);                    
                    break;
                case 13: GlobalProcedures.FillComboBoxEdit(CountryComboBox, "COUNTRIES", "NAME,NAME_EN,NAME_RU", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 17: GlobalProcedures.FillLookUpEdit(KindShipRateLookUp, "KINDSHIP_RATE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
            
            FindDescriptionID();
        }

        private void LoadDictionariesForDescription(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDescription);
            fc.ShowDialog();
        }

        private void DescriptionComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionariesForDescription("E", 0);
        }

        private void CountryComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionariesForDescription("E", 13);
        }

    }
}