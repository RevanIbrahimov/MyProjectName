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

namespace CRS.Forms.Total
{
    public partial class FCommitmentInfo : DevExpress.XtraEditors.XtraForm
    {
        public FCommitmentInfo()
        {
            InitializeComponent();
        }
        public int CommitmentID;

        string PowerID, PowerCode;

        private void FCommitmentInfo_Load(object sender, EventArgs e)
        {
            LoadCommitmentDetails();
            LoadPhoneDataGridView();
            LoadPowerDataGridView();
        }

        private void LoadCommitmentDetails()
        {
            string s = $@"SELECT CC.COMMITMENT_NAME,
                               CS.SERIES || ' ' || CC.CARD_NUMBER CARD_NUMBER,
                               CC.CARD_PINCODE,
                               TO_CHAR (CC.CARD_ISSUE_DATE, 'DD.MM.YYYY') ISSUEDATE,
                               TO_CHAR (CC.CARD_RELIABLE_DATE, 'DD.MM.YYYY') RELIABLEDATE,
                               CI.NAME ISSUING_NAME,
                               CC.ADDRESS,
                               C.CONTRACT_CODE,
                               CC.VOEN,
                               CC.ACCOUNT_NUMBER
                          FROM CRS_USER.CONTRACT_COMMITMENTS CC,
                               CRS_USER.CARD_SERIES CS,
                               CRS_USER.CARD_ISSUING CI,
                               CRS_USER.V_CONTRACTS C
                         WHERE     CC.CARD_SERIES_ID = CS.ID
                               AND CC.CARD_ISSUING_ID = CI.ID
                               AND CC.CONTRACT_ID = C.CONTRACT_ID
                               AND CC.ID = {CommitmentID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDetails").Rows)
                {
                    NameText.Text = dr["COMMITMENT_NAME"].ToString();
                    NumberText.Text = dr["CARD_NUMBER"].ToString();
                    PinCodeText.Text = dr["CARD_PINCODE"].ToString();
                    StartDateText.Text = dr["ISSUEDATE"].ToString();
                    EndDateText.Text = dr["RELIABLEDATE"].ToString();
                    IssuingText.Text = dr["ISSUING_NAME"].ToString();
                    AddressText.Text = dr["ADDRESS"].ToString();
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    AccountText.EditValue = dr["ACCOUNT_NUMBER"];
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Öhdəliyin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 P.ID,
                                 PD.DESCRIPTION_AZ DESCRIPTION,
                                 '+' || C.CODE || P.PHONE_NUMBER PHONENUMBER,
                                 P.IS_SEND_SMS
                            FROM CRS_USER.PHONES P,
                                 CRS_USER.PHONE_DESCRIPTIONS PD,
                                 CRS_USER.COUNTRIES C
                           WHERE     P.PHONE_DESCRIPTION_ID = PD.ID
                                 AND P.COUNTRY_ID = C.ID
                                 AND P.OWNER_TYPE = 'CC'
                                 AND P.OWNER_ID = {CommitmentID}
                        ORDER BY P.ORDER_ID";

            PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
        }


        private void LoadPowerDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                   PA.ID,
                                   PA.POWER_CODE,                                   
                                   PA.INSERT_DATE,
                                   PA.POWER_DATE,
                                   PA.IS_REVOKE
                              FROM CRS_USER.POWER_OF_ATTORNEY PA
                             WHERE  PA.FULLNAME = '{NameText.Text.Trim()}'
                            ORDER BY PA.ID";

            PowerGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPowerDataGridView");
            ViewPowerFileBarButton.Enabled = (PowerGridView.RowCount > 0);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void RefreshPowerBarButtonI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerDataGridView();
        }

        private void LoadPowerFile()
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.POWER_FILE FROM CRS_USER.POWER_OF_ATTORNEY T WHERE T.ID = {PowerID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text + "_Etibarname.docx",
                                                "POWER_FILE");
        }

        private void ViewPowerFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerFile();
        }

        private void PowerGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_REVOKE")) == 0)
                e.Value = Properties.Resources.ok_16;
            else
                e.Value = Properties.Resources.cancel_16;
        }

        private void PowerGridView_DoubleClick(object sender, EventArgs e)
        {
            if (ViewPowerFileBarButton.Enabled)
                LoadPowerFile();
        }

        private void PowerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PowerGridView.GetFocusedDataRow();
            if (row != null)
            {
                PowerID = row["ID"].ToString();
                PowerCode = row["POWER_CODE"].ToString();
            }
        }
    }
}