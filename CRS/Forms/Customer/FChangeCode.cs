using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using CRS.Class;

namespace CRS.Forms.Customer
{
    public partial class FChangeCode : DevExpress.XtraEditors.XtraForm
    {
        public FChangeCode()
        {
            InitializeComponent();
        }
        public int type, name_id;
        bool b = false;

        public delegate void DoEvent(string a, bool close);
        public event DoEvent RefreshCode;

        private void FChangeCode_Load(object sender, EventArgs e)
        {
            int max_number = 0;
            switch (type)
            {
                case 1://musteri
                    {
                        max_number = GlobalFunctions.GetID("SELECT TO_NUMBER(NVL(MAX(CODE),'0000')) FROM CRS_USER.V_CUSTOMERS", this.Name + "/FChangeCode_Load");
                        DescriptionLabel.Text = "Ən son daxil edilmiş müştərinin qeydiyyat nömrəsi: " + max_number.ToString().PadLeft(4, '0');
                        NumberValue.Properties.MinValue = max_number + 1;
                    }
                    break;
                case 2://muqavile
                    {
                        max_number = GlobalFunctions.GetID($@"SELECT TO_NUMBER(NVL(MAX(C.CODE),'000')) FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND CT.NAME_ID = {name_id}", this.Name + " / FChangeCode_Load");
                        DescriptionLabel.Text = "Ən son daxil edilmiş müqavilənin nömrəsi: " + max_number.ToString().PadLeft(3, '0');
                        NumberValue.Properties.MinValue = max_number + 1;
                    }
                    break;
                case 3://etibarname
                    {
                        max_number = GlobalFunctions.GetID($@"SELECT TO_NUMBER (NVL (MAX (POWER_NUMBER), 0))
                                                                  FROM (SELECT POWER_NUMBER FROM CRS_USER.POWER_OF_ATTORNEY
                                                                        UNION ALL
                                                                        SELECT POWER_NUMBER FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP
                                                                                        WHERE IS_CHANGE <> 2)", this.Name + "/FChangeCode_Load");
                        DescriptionLabel.Text = "Ən son daxil edilmiş etibarnamənin nömrəsi: " + max_number.ToString().PadLeft(4, '0');
                        NumberValue.Properties.MinValue = max_number + 1;
                    }
                    break;
            }

            LoadOperationsDataGridView();
            spinEdit1_EditValueChanged(sender, e);
        }

        private void LoadOperationsDataGridView()
        {
            string s = null;

            switch (type)
            {
                case 1:
                    s = $@"SELECT 1 SS,
                                   U.USER_FULLNAME,
                                   CC.CODE
                              FROM CRS_USER_TEMP.CUSTOMER_CODE_TEMP CC, CRS_USER.V_USERS U
                             WHERE CC.USED_USER_ID = U.ID";
                    break;
                case 2:
                    s = $@"SELECT 1 SS,
                                   U.USER_FULLNAME,
                                   CC.CODE
                              FROM CRS_USER_TEMP.CONTRACT_CODE_TEMP CC, CRS_USER.V_USERS U
                             WHERE CC.USED_USER_ID = U.ID";
                    break;
                case 3:
                    s = $@"SELECT 1 SS,
                                       U.USER_FULLNAME,
                                       CC.CODE
                                  FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP CC, CRS_USER.V_USERS U
                                 WHERE CC.USED_USER_ID = U.ID";
                    break;
            }

            CodeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperationsDataGridView", "Nömrələr cədvələ yüklənmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            b = false;
            this.Close();
        }

        private void FChangeCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshCode(CodeLabel.Text, b);
        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            switch (type)
            {
                case 1:
                    CodeLabel.Text = NumberValue.Value.ToString().PadLeft(4, '0').Trim();
                    break;
                case 2:
                    CodeLabel.Text = NumberValue.Value.ToString().PadLeft(3, '0').Trim();
                    break;
                case 3:
                    CodeLabel.Text = "E" + NumberValue.Value.ToString().PadLeft(4, '0').Trim() + "/" + DateTime.Today.Year.ToString().Substring(2, 2);
                    break;
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            switch (type)
            {
                case 1:
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_CODE_TEMP SET CODE_NUMBER = {NumberValue.Value},CODE = '{CodeLabel.Text.Trim()}' WHERE USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Nömrə dəyişdirilmədi.",
                                                    this.Name  + "/BOK_Click");
                    break;
                case 2:
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CONTRACT_CODE_TEMP SET CODE_NUMBER = {NumberValue.Value},CODE = '{CodeLabel.Text.Trim()}' WHERE USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Nömrə dəyişdirilmədi.",
                                                    this.Name + "/BOK_Click");
                    break;
                case 3:
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP SET CODE_NUMBER = {NumberValue.Value},CODE = '{CodeLabel.Text.Trim()}' WHERE USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Nömrə dəyişdirilmədi.",
                                                    this.Name + "/BOK_Click");
                    break;
            }

            b = true;
            this.Close();
        }

        private void CodeGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
    }
}