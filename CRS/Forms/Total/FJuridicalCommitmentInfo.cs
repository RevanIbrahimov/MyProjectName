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

namespace CRS.Forms.Total
{
    public partial class FJuridicalCommitmentInfo : DevExpress.XtraEditors.XtraForm
    {
        public FJuridicalCommitmentInfo()
        {
            InitializeComponent();
        }
        public int CommitmentID;

        private void FJuridicalCommitmentInfo_Load(object sender, EventArgs e)
        {
            LoadCommitmentDetails();
            LoadPhoneDataGridView();
        }

        private void LoadCommitmentDetails()
        {
            string s = $@"SELECT CC.COMMITMENT_NAME,
                                   CC.LEADING_NAME,
                                   CC.ADDRESS,
                                   C.CONTRACT_CODE,
                                   CC.VOEN
                              FROM CRS_USER.CONTRACT_JURIDICAL_COMMITMENTS CC, CRS_USER.V_CONTRACTS C
                             WHERE CC.CONTRACT_ID = C.CONTRACT_ID AND CC.ID = {CommitmentID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDetails").Rows)
                {
                    NameText.Text = dr["COMMITMENT_NAME"].ToString();
                    LeadingNameText.Text = dr["LEADING_NAME"].ToString();                    
                    AddressText.Text = dr["ADDRESS"].ToString();
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
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
    }
}