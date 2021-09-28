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
using System.Collections;

namespace CRS.Forms.Options
{
    public partial class FOtherUserColors : DevExpress.XtraEditors.XtraForm
    {
        public FOtherUserColors()
        {
            InitializeComponent();
        }
        int userID;

        public delegate void DoEvent();
        public event DoEvent RefreshColorsDataGridView;

        private void FOtherUserColors_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillLookUpEdit(UserLookUp, "V_USERS", "ID", "USER_FULLNAME", $@"STATUS_ID = 1 AND ID <> {GlobalVariables.V_UserID}");
        }

        private void FOtherUserColors_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshColorsDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (UserLookUp.EditValue == null)
                return;

            userID = Convert.ToInt32(UserLookUp.EditValue);

            string s = null;
            try
            {
                s = $@"SELECT 1 SS,
                               LC.ID,
                               LCT.NAME,
                               LC.COLOR_VALUE_1,
                               LC.COLOR_VALUE_2,
                               LC.COLOR_TYPE_ID
                          FROM CRS_USER.LRS_COLORS LC, CRS_USER.LRS_COLOR_TYPE LCT
                         WHERE LC.COLOR_TYPE_ID = LCT.ID AND LC.USER_ID = {userID}
                        ORDER BY LC.COLOR_TYPE_ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                ColorsGridControl.DataSource = dt;

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Rənglər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ColorsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ColorsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            int value1, value2;

            if (e.Column.Name == "Color1")
            {
                value1 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_1"));
                e.Appearance.BackColor = Color.FromArgb(value1);
            }

            if (e.Column.Name == "Color2")
            {
                value2 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_2"));
                e.Appearance.BackColor = Color.FromArgb(value2);
            }

            if (e.Column.Name == "Color3")
            {
                value1 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_1"));
                value2 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_2"));
                e.Appearance.BackColor = Color.FromArgb(value1);
                e.Appearance.BackColor2 = Color.FromArgb(value2);
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ColorsGridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("Rəngləri əvəz etmək üçün ən azı bir rəng seçilməlidir.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < ColorsGridView.SelectedRowsCount; i++)
            {
                rows.Add(ColorsGridView.GetDataRow(ColorsGridView.GetSelectedRows()[i]));
            }

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.LRS_COLORS_TEMP WHERE USER_ID = {GlobalVariables.V_UserID}") > 0)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.LRS_COLORS_TEMP SET COLOR_VALUE_1 = {row["COLOR_VALUE_1"]}, COLOR_VALUE_2 = {row["COLOR_VALUE_2"]} WHERE USER_ID = {GlobalVariables.V_UserID} AND COLOR_TYPE_ID = {row["COLOR_TYPE_ID"]}",
                        "Rəng dəyişdirilmədi.",
                        this.Name + "/BOK_Click");
                }
            }
            else
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.LRS_COLORS_TEMP (ID,
                                                                                             USER_ID,
                                                                                             COLOR_TYPE_ID,
                                                                                             COLOR_VALUE_1,
                                                                                             COLOR_VALUE_2,
                                                                                             USED_USER_ID)
                                                       SELECT LRS_COLOR_SEQUENCE.NEXTVAL,
                                                              {GlobalVariables.V_UserID} USER_ID,
                                                              COLOR_TYPE_ID,
                                                              COLOR_VALUE_1,
                                                              COLOR_VALUE_2,
                                                              {GlobalVariables.V_UserID}
                                                         FROM CRS_USER.LRS_COLORS
                                                        WHERE USER_ID = {userID}", 
                                                "Rəng dəyişdirilmədi.", 
                                                this.Name + "/BOK_Click");

            this.Close();
        }
    }
}