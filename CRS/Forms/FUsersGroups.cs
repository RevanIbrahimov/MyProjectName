using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FUsersGroups : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FUsersGroups()
        {
            InitializeComponent();
        }

        string GroupID, GroupName;
        int topindex, old_row_id;

        private void FUsersGroups_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddUserGroup;

            ShowOrHideUserBarButton.Down = true;
            LoadUsersGroupDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadUsersGroupDataGridView()
        {
            string s = "SELECT 1 SS,ID,GROUP_NAME,GROUP_NAME_EN,GROUP_NAME_RU,NOTE,G.USED_USER_ID FROM CRS_USER.USER_GROUP G";
            try
            {
                GroupGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUsersGroupDataGridView");
                
                if (GroupGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                    {
                        EditBarButton.Enabled = GlobalVariables.EditUserGroup;
                        DeleteBarButton.Enabled = GlobalVariables.DeleteUserGroup;
                        CopyBarButton.Enabled = GlobalVariables.CopyUserGroup;
                    }
                    else
                        EditBarButton.Enabled = DeleteBarButton.Enabled = CopyBarButton.Enabled = true;

                    GroupGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = false;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçi qrupları cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadUsersDataGridView()
        {
            string s = $@"SELECT 1 SS,SURNAME||' '||NAME||' '||PATRONYMIC||' '||DECODE(SEX_ID,1,'oğlu','qızı') FULLNAME,ID,USED_USER_ID,SEX_ID,SESSION_ID FROM CRS_USER.CRS_USERS WHERE STATUS_ID = 1 AND GROUP_ID = {GroupID} ORDER BY 2";
            try
            {
                UserGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUsersDataGridView");                
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Qrupa daxil olan istifadəçilərin siyahısı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void GroupGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void GroupGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(GroupGridView, PopupMenu, e);
        }

        private void ExcelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(GroupGridControl, "mht");
        }

        private void ShowOrHideUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!ShowOrHideUserBarButton.Down)
            {
                UserGridControl.Visible = false;
                GroupGridControl.Dock = DockStyle.Fill;
                ShowOrHideUserBarButton.Caption = "İstifadəçiləri göstər";
            }
            else
            {
                UserGridControl.Visible = true;
                GroupGridControl.Dock = DockStyle.Top;
                ShowOrHideUserBarButton.Caption = "İstifadəçiləri gizlət";
            }
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(GroupGridControl);
        }

        private void GroupGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(GroupGridView, e);
        }

        private void GroupGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = GroupGridView.GetFocusedDataRow();
            if (row != null)
            {
                GroupID = row["ID"].ToString();
                GroupName = row["GROUP_NAME"].ToString();
                LoadUsersDataGridView();
            }
        }

        void RefreshUserGroup()
        {
            LoadUsersGroupDataGridView();
        }

        private void LoadFUserGroupAddEdit(string transaction, string groupID)
        {
            topindex = GroupGridView.TopRowIndex;
            old_row_id = GroupGridView.FocusedRowHandle;
            FUserGroupAddEdit fc = new FUserGroupAddEdit();
            fc.TransactionName = transaction;
            fc.GroupID = groupID;
            fc.RefreshUserGroupDataGridView += new FUserGroupAddEdit.DoEvent(RefreshUserGroup);
            fc.ShowDialog();
            GroupGridView.TopRowIndex = topindex;
            GroupGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFUserGroupAddEdit("INSERT", null);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadUsersGroupDataGridView();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFUserGroupAddEdit("EDIT", GroupID);
        }

        private void DeleteGroup()
        {
            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CRS_USERS WHERE GROUP_ID = {GroupID}") == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qrupu silmək istəyirsiniz?", "Qrupun silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.USER_GROUP WHERE ID = {GroupID}",
                                                     $@"DELETE FROM CRS_USER.ALL_USER_GROUP_ROLE_DETAILS WHERE GROUP_ID = {GroupID}",
                                                     "Seçilmiş qrup bazadan silinmədi.",
                                                     this.Name + "/DeleteGroup");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş qrupa daxil olan istifadəçilər olduğu üçün bu qrupu silmək olmaz. Qrupu silmək üçün əvvəlcə istifadəçiləri bu qrupdan çıxarmaq lazımdır.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteGroup();
            LoadUsersGroupDataGridView();
        }

        private void GroupGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFUserGroupAddEdit("EDIT", GroupID);
        }

        private void CopyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qrupun surətini çıxarmaq istəyirsiniz?", "Qrupun surətinin çıxarılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                int newGroupID = GlobalFunctions.GetOracleSequenceValue("USER_GROUP_SEQUENCE");
                GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.USER_GROUP (ID,
                                                                                     GROUP_NAME,
                                                                                     GROUP_NAME_EN,
                                                                                     GROUP_NAME_RU)
                                                       SELECT {newGroupID},
                                                              'Copy ' || GROUP_NAME GROUP_NAME,
                                                              'Copy ' || GROUP_NAME_EN GROUP_NAME_EN,
                                                              'Copy ' || GROUP_NAME_RU GROUP_NAME_RU
                                                         FROM CRS_USER.USER_GROUP
                                                        WHERE ID = {GroupID}",
                                                 $@"INSERT INTO CRS_USER.ALL_USER_GROUP_ROLE_DETAILS (ID,
                                                                                                      GROUP_ID,
                                                                                                      ROLE_DETAIL_ID)
                                                       SELECT USER_GROUP_PERMISSION_SEQUENCE.NEXTVAL ID, {newGroupID}, ROLE_DETAIL_ID
                                                         FROM CRS_USER.ALL_USER_GROUP_ROLE_DETAILS
                                                        WHERE GROUP_ID = {GroupID}",
                                                 "Seçilmiş qrupun surəti çıxarılmadı.",
                                                 this.Name + "/CopyBarButton_ItemClick");
                LoadUsersGroupDataGridView();
            }
        }

        private void GroupGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (GroupGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditUserGroup;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteUserGroup;
                    CopyBarButton.Enabled = GlobalVariables.CopyUserGroup;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = CopyBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = CopyBarButton.Enabled = false;
        }
    }
}