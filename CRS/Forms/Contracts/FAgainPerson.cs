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

namespace CRS.Forms.Contracts
{
    public partial class FAgainPerson : DevExpress.XtraEditors.XtraForm
    {
        public FAgainPerson()
        {
            InitializeComponent();
        }
        public int PersonTypeID;

        int personID, categoryID;
        string ownerType;

        public delegate void DoEvent(int personid, int categoryid, string ownerType);
        public event DoEvent SelectedPerson;

        private void FAgainPerson_Load(object sender, EventArgs e)
        {
            LoadPersonsDataGridView();
        }

        private void LoadPersonsDataGridView()
        {
            string sql = $@"SELECT *
                                FROM (  SELECT MAX (S.ID) ID,
                                               S.FULLNAME,
                                               SC.SELLER_CARD CARD,
                                               NVL (SC.ADDRESS, SC.REGISTRATION_ADDRESS) REGISTRATION_ADDRESS,
                                               S.PERSON_TYPE_ID,
                                               1 CATEGORY_ID,
                                               'Satıcı' CATEGORY_NAME,
                                               DECODE (S.PERSON_TYPE_ID, 1, 'S', 'JP') OWNER_TYPE
                                          FROM CRS_USER.V_SELLER_CARDS SC, CRS_USER.V_SELLERS S
                                         WHERE SC.SELLER_ID = S.ID AND SC.PERSON_TYPE_ID = S.PERSON_TYPE_ID
                                      GROUP BY S.FULLNAME,
                                               SC.SELLER_CARD,
                                               SC.ADDRESS,
                                               SC.REGISTRATION_ADDRESS,
                                               S.PERSON_TYPE_ID
                                      UNION ALL
                                        SELECT MAX (C.ID) ID,
                                               C.COMMITMENT_NAME FULLNAME,
                                               CC.CARD,
                                               CC.REGISTRATION_ADDRESS,
                                               1 PERSON_TYPE_ID,
                                               2 CATEGORY_ID,
                                               'Öhdəlik ötürən' CATEGORY_NAME,
                                               'CC' OWNER_TYPE
                                          FROM CRS_USER.V_COMMITMENT_CARDS CC,
                                               CRS_USER.CONTRACT_COMMITMENTS C
                                         WHERE CC.COMMITMENT_ID = C.ID
                                      GROUP BY C.COMMITMENT_NAME, CC.CARD, CC.REGISTRATION_ADDRESS
                                      UNION ALL
                                        SELECT MAX (C.ID),
                                               C.CUSTOMER_NAME,
                                                  CS.NAME
                                               || ':'
                                               || CS.SERIES
                                               || ', №: '
                                               || CC.CARD_NUMBER
                                               || ', '
                                               || TO_CHAR (CC.ISSUE_DATE, 'DD.MM.YYYY')
                                               || ' tarixində '
                                               || CI.NAME
                                               || ' tərəfindən verilib',
                                               CC.REGISTRATION_ADDRESS,
                                               C.PERSON_TYPE_ID,
                                               3 CATEGORY_ID,
                                               'Müştəri' CATEGORY_NAME,
                                               DECODE (C.PERSON_TYPE_ID, 1, 'C', 'JP') OWNER_TYPE
                                          FROM CRS_USER.V_CUSTOMER_CARDS CC,
                                               CRS_USER.V_CUSTOMERS C,
                                               CRS_USER.CARD_SERIES CS,
                                               CRS_USER.CARD_ISSUING CI
                                         WHERE     CC.CUSTOMER_ID = C.ID
                                               AND CC.CARD_SERIES_ID = CS.ID
                                               AND CC.CARD_ISSUING_ID = CI.ID
                                      GROUP BY C.CUSTOMER_NAME,
                                               CS.NAME,
                                               CS.SERIES,
                                               CI.NAME,
                                               CC.REGISTRATION_ADDRESS,
                                               CC.ISSUE_DATE,
                                               C.PERSON_TYPE_ID,
                                               CC.CARD_NUMBER)
                               WHERE PERSON_TYPE_ID = {PersonTypeID}
                            ORDER BY FULLNAME";

            PersonsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPersonsDataGridView");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPersonsDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PersonsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PersonsGridView.GetFocusedDataRow();
            if (row != null)
            {
                personID = int.Parse(row["ID"].ToString());
                categoryID = int.Parse(row["CATEGORY_ID"].ToString());
                ownerType = row["OWNER_TYPE"].ToString();
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            this.SelectedPerson(personID, categoryID, ownerType);
            this.Close();
        }        
    }
}