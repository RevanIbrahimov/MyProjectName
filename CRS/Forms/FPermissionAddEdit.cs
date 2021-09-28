using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FPermissionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPermissionAddEdit()
        {
            InitializeComponent();
        }
        public string GroupID, GroupName;

        public delegate void DoEvent();
        public event DoEvent RefreshUserGroupPermissionDataGridView;

        private void FPermissionAddEdit_Load(object sender, EventArgs e)
        {
            this.Name = this.Name + " (" + GroupName + ")";
            LoadAllPermissionDataGridView();
            LoadUsedPermissionDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InsertPermission()
        {            
            ArrayList rows = new ArrayList();
            rows.Clear();
            try
            {
                for (int i = 0; i < AllPermissionGridView.SelectedRowsCount; i++)
                {
                    if (AllPermissionGridView.GetRowLevel(AllPermissionGridView.GetSelectedRows()[i]) == 1)
                        rows.Add(AllPermissionGridView.GetDataRow(AllPermissionGridView.GetSelectedRows()[i]));
                }

                AllPermissionGridView.BeginUpdate();

                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP(ID,GROUP_ID,ROLE_DETAIL_ID) VALUES(USER_GROUP_PERMISSION_SEQUENCE.NEXTVAL," + GroupID + "," + row["ID"].ToString() + ")",
                                                    "İstifadəçi qrupuna hüquq verilmədi.");
                }
            }
            finally
            {
                AllPermissionGridView.EndUpdate();
            }
        }

        private void DeletePermission()
        {
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < UsedPermissionGridView.SelectedRowsCount; i++)
            {
                if (UsedPermissionGridView.GetRowLevel(UsedPermissionGridView.GetSelectedRows()[i]) == 1)
                    rows.Add(UsedPermissionGridView.GetDataRow(UsedPermissionGridView.GetSelectedRows()[i]));
            }

            try
            {
                UsedPermissionGridView.BeginUpdate();
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = " + GroupID + " AND ROLE_DETAIL_ID = " + row["ID"].ToString(),
                                                          "Hüquq alınmadı.");

                }
            }
            finally
            {
                UsedPermissionGridView.EndUpdate();
            }
        }

        private void LoadAllPermissionDataGridView()
        {
            string s = null;

            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    s = $@"SELECT R.ROLE_DESCRIPTION,RD.DETAIL_NAME_AZ,RD.ID FROM CRS_USER.ALL_ROLE_DETAILS RD,CRS_USER.ROLES R WHERE RD.ROLE_ID = R.ROLE_ID AND RD.ID NOT IN (SELECT ROLE_DETAIL_ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = {GroupID}) ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                    break;
                case "EN":
                    s = "SELECT R.ROLE_DESCRIPTION_EN,RD.DETAIL_NAME_EN,RD.ID FROM CRS_USER.ALL_ROLE_DETAILS RD,CRS_USER.ROLES R WHERE RD.ROLE_ID = R.ROLE_ID AND RD.ID NOT IN (SELECT ROLE_DETAIL_ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = " + GroupID + ") ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                    break;
                case "RU":
                    s = "SELECT R.ROLE_DESCRIPTION_RU,RD.DETAIL_NAME_RU,RD.ID FROM CRS_USER.ALL_ROLE_DETAILS RD,CRS_USER.ROLES R WHERE RD.ROLE_ID = R.ROLE_ID AND RD.ID NOT IN (SELECT ROLE_DETAIL_ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = " + GroupID + ") ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                    break;
            }

            AllPermissionGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAllPermissionDataGridView", "Verilməmiş hüquqlar cədvələ yüklənmədi.");
        }

        private void LoadUsedPermissionDataGridView()
        {
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT R.ROLE_DESCRIPTION,RD.DETAIL_NAME_AZ,RD.ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID + " ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                        break;
                    case "EN":
                        s = "SELECT R.ROLE_DESCRIPTION_EN,RD.DETAIL_NAME_EN,RD.ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID + " ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                        break;
                    case "RU":
                        s = "SELECT R.ROLE_DESCRIPTION_RU,RD.DETAIL_NAME_RU,RD.ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID + " ORDER BY RD.ROLE_ID,RD.ORDER_ID";
                        break;
                }

                UsedPermissionGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUsedPermissionDataGridView");
                UsedPermissionGridView.PopulateColumns();
                UsedPermissionGridView.Columns[0].Caption = "Modulun adı";
                UsedPermissionGridView.Columns[1].Caption = "Hüquqların adı";
                UsedPermissionGridView.Columns[2].Visible = false;

                for (int i = 0; i < UsedPermissionGridView.Columns.Count; i++)
                {
                    UsedPermissionGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    UsedPermissionGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                UsedPermissionGridView.Columns[0].GroupIndex = 0;

                UsedPermissionGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Verilmiş hüquqlar cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BAddPermission_Click(object sender, EventArgs e)
        {
            InsertPermission();
            LoadAllPermissionDataGridView();
            LoadUsedPermissionDataGridView();
        }

        private void BDeletePermission_Click(object sender, EventArgs e)
        {
            DeletePermission();
            LoadAllPermissionDataGridView();
            LoadUsedPermissionDataGridView();
        }

        private void BRefresh_Click(object sender, EventArgs e)
        {
            LoadAllPermissionDataGridView();
            LoadUsedPermissionDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPermissionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshUserGroupPermissionDataGridView();
        }
    }
}