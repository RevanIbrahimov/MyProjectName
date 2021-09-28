using System;
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
using DevExpress.XtraEditors.Controls;

namespace CRS.Forms
{
    public partial class FSendSms : DevExpress.XtraEditors.XtraForm
    {
        public FSendSms()
        {
            InitializeComponent();
        }
        public int OwnerID, PersonTypeID;
        public string OwnerType, OwnerName;

        private void FSendSms_Load(object sender, EventArgs e)
        {
            string selectedNumber = null,
                   sqltext = $@"SELECT C.CODE || P.PHONE_NUMBER PHONE_NUMBER, P.IS_SEND_SMS
                                      FROM CRS_USER.PHONES P, CRS_USER.COUNTRIES C
                                     WHERE     P.COUNTRY_ID = C.ID
                                           AND P.PHONE_DESCRIPTION_ID = 5
                                           AND P.OWNER_ID = {OwnerID}
                                           AND P.OWNER_TYPE = '{OwnerType}'
                                ORDER BY P.ORDER_ID";

            NumberCheckedComboBox.Properties.Items.Clear();
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sqltext, this.Name + "/FSendSms_Load", "Telefon nömrələri açılmadı.").Rows)
            {
                int isSendSms = Convert.ToInt32(dr["IS_SEND_SMS"]);
                string phoneNumber = dr["PHONE_NUMBER"].ToString();
                if (!String.IsNullOrEmpty(phoneNumber))
                {
                    NumberCheckedComboBox.Properties.Items.Add(phoneNumber, CheckState.Unchecked, true);
                    NumberCheckedComboBox.Properties.SeparatorChar = ';';

                    if (isSendSms == 1)
                        selectedNumber = selectedNumber + "; " + phoneNumber;                    
                }                
            }

            if (selectedNumber != null)
            {
                selectedNumber = selectedNumber.Trim(';').Trim();
                NumberCheckedComboBox.SetEditValue(selectedNumber);
            }
        }

        private void SmsBodyText_EditValueChanged(object sender, EventArgs e)
        {
            if (SmsBodyText.Text.Length <= 160)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (160 - SmsBodyText.Text.Length).ToString();
        }

        private bool ControlSmsDetails()
        {
            bool b = false;

            if (NumberCheckedComboBox.Text.Length == 0)
            {
                NumberCheckedComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sms göndərmək üçün heç bir mobil nömrə seçilməyib.");
                NumberCheckedComboBox.Focus();
                NumberCheckedComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SmsBodyText.Text.Length == 0)
            {
                SmsBodyText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sms-in mətni daxil edilməyib.");
                SmsBodyText.Focus();
                SmsBodyText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SmsBodyText.Text.Length > 160)
            {
                SmsBodyText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sms-in mətni maksimum 160 simvol ola bilər.");
                SmsBodyText.Focus();
                SmsBodyText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertSms()
        {
            string sql = null;

            foreach (CheckedListBoxItem item in NumberCheckedComboBox.Properties.GetItems())
                if (item.CheckState == CheckState.Checked)
                {
                    sql = $@"INSERT INTO CRS_USER.SMS_DETAILS (SMS_TYPE_ID,
                                  OWNER_ID,
                                  OWNER_TYPE,
                                  OWNER_NAME,
                                  PERSON_TYPE_ID,
                                  PHONE_NUMBER,
                                  MESSAGE_BODY,
                                  IS_AUTOMATIC,
                                  SMS_STATUS_ID,
                                  INSERT_USER)
                            VALUES (4,
                                    {OwnerID},
                                    '{OwnerType}',
                                    '{OwnerName}',
                                    {PersonTypeID},
                                    '{item.Value}',
                                    '{SmsBodyText.Text.Trim()}',
                                    0,
                                    107,
                                    {GlobalVariables.V_UserID})";
                    GlobalProcedures.ExecuteQuery(sql, item.Value +  " nömrəsi üçün sms mətni bazaya daxil edilmədi.", this.Name + "/InsertSms");
                }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlSmsDetails())
            {
                InsertSms();
                this.Close();
            }
        }
    }
}