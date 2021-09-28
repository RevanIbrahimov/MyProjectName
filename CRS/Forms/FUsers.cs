using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FUsers : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FUsers()
        {
            InitializeComponent();
        }
        string UserID;
        int topindex, old_row_id, session_id = 0, used_user_id = -1;

        private void FUsers_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                NewBarButton.Enabled = GlobalVariables.AddUser;
                //EditBarButton.Enabled = GlobalVariables.EditUser;
                //DeleteBarButton.Enabled = GlobalVariables.DeleteUser;
                GroupBarButton.Enabled = GlobalVariables.UsersGroup;
                UnlockBarButton.Enabled = GlobalVariables.UnlockUser;
            }
            ShowOrHideUserBarButton.Down = true;
            LoadUsersDataGridView();
        }

        private void LoadUsersDataGridView()
        {
            string status_id = null, s = null;
            if ((OpenUserBarCheck.Checked) && (!CloseUserBarCheck.Checked))
                status_id = status_id + " AND U.STATUS_ID = 1";
            else if ((!OpenUserBarCheck.Checked) && (CloseUserBarCheck.Checked))
                status_id = status_id + " AND U.STATUS_ID = 2";
            else
                status_id = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT 1 SS,U.ID,U.SURNAME||' '||U.NAME||' '||U.PATRONYMIC FULLNAME,S.NAME SEX_NAME,TO_CHAR(U.BIRTHDAY,'DD/MM/YYYY') USER_BIRTHDAY,U.ADDRESS,ST.STATUS_NAME STATUS,G.GROUP_NAME,U.NOTE,U.USED_USER_ID,U.STATUS_ID,U.SESSION_ID FROM CRS_USER.CRS_USERS U,CRS_USER.SEX S,CRS_USER.STATUS ST,CRS_USER.USER_GROUP G WHERE S.ID = U.SEX_ID AND U.GROUP_ID = G.ID(+) AND U.STATUS_ID = ST.ID" + status_id + " ORDER BY 1, 2, 3";
                        break;
                    case "EN":
                        s = "SELECT 1 SS,U.ID,U.SURNAME||' '||U.NAME||' '||U.PATRONYMIC FULLNAME,S.NAME_EN SEX_NAME,TO_CHAR(U.BIRTHDAY,'DD/MM/YYYY') USER_BIRTHDAY,U.ADDRESS,ST.STATUS_NAME_EN STATUS,G.GROUP_NAME_EN GROUP_NAME,U.NOTE,U.USED_USER_ID,U.STATUS_ID,U.SESSION_ID FROM CRS_USER.CRS_USERS U,CRS_USER.SEX S,CRS_USER.STATUS ST,CRS_USER.USER_GROUP G WHERE S.ID = U.SEX_ID AND U.GROUP_ID = G.ID(+) AND U.STATUS_ID = ST.ID" + status_id + " ORDER BY 1, 2, 3";
                        break;
                    case "RU":
                        s = "SELECT 1 SS,U.ID,U.SURNAME||' '||U.NAME||' '||U.PATRONYMIC FULLNAME,S.NAME_RU SEX_NAME,TO_CHAR(U.BIRTHDAY,'DD/MM/YYYY') USER_BIRTHDAY,U.ADDRESS,ST.STATUS_NAME_RU STATUS,G.GROUP_NAME_RU GROUP_NAME,U.NOTE,U.USED_USER_ID,U.STATUS_ID,U.SESSION_ID FROM CRS_USER.CRS_USERS U,CRS_USER.SEX S,CRS_USER.STATUS ST,CRS_USER.USER_GROUP G WHERE S.ID = U.SEX_ID AND U.GROUP_ID = G.ID(+) AND U.STATUS_ID = ST.ID" + status_id + " ORDER BY 1, 2, 3";
                        break;
                }

                UsersGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUsersDataGridView");
                UsersGridView.PopulateColumns();
                UsersGridView.Columns[0].Caption = "S/s";
                UsersGridView.Columns[1].Caption = "Qeydiyyat nömrəsi";
                UsersGridView.Columns[2].Caption = "Soyadı, adı, atasının adı";
                UsersGridView.Columns[3].Caption = "Cinsi";
                UsersGridView.Columns[4].Caption = "Doğum tarixi";
                UsersGridView.Columns[5].Caption = "Ünvanı";
                UsersGridView.Columns[6].Caption = "Statusu";
                UsersGridView.Columns[7].Caption = "Qrupu";
                UsersGridView.Columns[8].Caption = "Qeyd";
                UsersGridView.Columns[9].Visible = false;
                UsersGridView.Columns[10].Visible = false;
                UsersGridView.Columns[11].Visible = false;

                //TextAligment
                UsersGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                UsersGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                UsersGridView.Columns[1].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                UsersGridView.Columns[1].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                UsersGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                UsersGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                if (UsersGridView.RowCount > 0)
                {
                    UsersGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (GlobalVariables.V_UserID > 0)
                    {
                        EditBarButton.Enabled = GlobalVariables.EditUser;
                        DeleteBarButton.Enabled = GlobalVariables.DeleteUser;
                    }
                    else
                        DeleteBarButton.Enabled = EditBarButton.Enabled = true;

                    SendMailBarButton.Enabled = true;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = SendMailBarButton.Enabled = false;
                UsersGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçilərin siyahısı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        void RefreshUser()
        {
            LoadUsersDataGridView();
        }

        private void LoadFUserAddEdit(string transaction, string UserID)
        {
            topindex = UsersGridView.TopRowIndex;
            old_row_id = UsersGridView.FocusedRowHandle;
            FUserAddEdit fc = new FUserAddEdit();
            fc.TransactionName = transaction;
            fc.UserID = UserID;
            fc.RefreshUserDataGridView += new FUserAddEdit.DoEvent(RefreshUser);
            fc.ShowDialog();
            UsersGridView.TopRowIndex = topindex;
            UsersGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFUserAddEdit("INSERT", null);
        }

        private void GroupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FUsersGroups fug = new FUsersGroups();
            fug.ShowDialog();
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUsersDataGridView();
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFUserAddEdit("EDIT", UserID);
        }

        private void UsersGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void UsersGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
            {
                LoadFUserAddEdit("EDIT", UserID);
            }
        }

        private void DeleteUser()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş istifadəçini silmək istəyirsiniz?", "İstifadəçinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_USER_DELETE", "P_USER_ID", UserID, "Seçilmiş istifadəçi bazadan silinmədi.");                
            }
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(UsersGridControl);
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (session_id != 0)
                XtraMessageBox.Show("İstifadəçi hal-hazırda sistemə qoşulduğu üçün onu bazadan silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (used_user_id >= 0)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == used_user_id).FULLNAME;
                XtraMessageBox.Show("İstifadəçinin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edildiyi üçün onu bazadan silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DeleteUser();
                LoadUsersDataGridView();
            }
        }

        private void LoadUserConnectDataGridView()
        {
            int userid = 0;
            string s = s = $@"SELECT 1 SS,IPADDRESS,MACADDRESS,TO_CHAR(CONNECT_DATE,'DD.MM.YYYY HH24:MI:SS AM'),TO_CHAR(DISCONNECT_DATE,'DD.MM.YYYY HH24:MI:SS AM') FROM CRS_USER.USER_CONNECTIONS WHERE USER_ID = {UserID} ORDER BY CONNECT_DATE DESC";
            if (UsersGridView.RowCount > 0)
            {
                userid = int.Parse(UserID);
                try
                {
                    ConnectGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUserConnectDataGridView");
                    ConnectGridView.PopulateColumns();
                    ConnectGridView.Columns[0].Caption = "S/s";
                    ConnectGridView.Columns[1].Caption = "IP ünvanı";
                    ConnectGridView.Columns[2].Caption = "MAC ünvanı";
                    ConnectGridView.Columns[3].Caption = "Sistemə qoşulma vaxtı";
                    ConnectGridView.Columns[4].Caption = "Sistemdən çıxma vaxtı";
                    
                    ConnectGridView.BestFitColumns();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("İstifadəçinin sistemə daxil olma statistikasının cədvəli yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void UsersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = UsersGridView.GetFocusedDataRow();
            if (row != null)
            {
                UserID = row["ID"].ToString();
                session_id = Convert.ToInt32(row["SESSION_ID"].ToString());
                used_user_id = Convert.ToInt32(row["USED_USER_ID"].ToString());
                LoadUserConnectDataGridView();
            }
            if (Convert.ToInt32(UserID) == GlobalVariables.V_UserID)
                UnlockBarButton.Enabled = false;
            else
                UnlockBarButton.Enabled = true;
        }

        private void ShowOrHideUserBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ShowOrHideUserBarButton.Down)
            {
                UsersSplitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
                ShowOrHideUserBarButton.Caption = "Statistikanı göstər";
            }
            else
            {
                UsersSplitContainerControl.PanelVisibility = SplitPanelVisibility.Both;
                ShowOrHideUserBarButton.Caption = "Statistikanı gizlət";
            }
        }

        private void FUsers_Activated(object sender, EventArgs e)
        {
            string s = "SELECT ID,STATUS_NAME FROM CRS_USER.STATUS WHERE ID IN (1,2)";
            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FUsers_Activated");
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["ID"].ToString()) == 1)
                        OpenUserBarCheck.Caption = dr["STATUS_NAME"].ToString();
                    else
                        CloseUserBarCheck.Caption = dr["STATUS_NAME"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçilərin statusları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(UsersGridControl, "mht");
        }

        private void UsersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(UsersGridView, PopupMenu, e);
        }

        private void UsersGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(UsersGridView, e);
            GlobalProcedures.GridRowCellStyleForConnect(UsersGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(2, UsersGridView, e);
        }

        private void ConnectGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void UsersGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void UsersGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (UsersGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditUser;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteUser;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = true;

                SendMailBarButton.Enabled = true;
            }
            else
                DeleteBarButton.Enabled = EditBarButton.Enabled = SendMailBarButton.Enabled = false;
        }

        private void UnlockBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(GlobalVariables.V_UserID == int.Parse(UserID))
            {
                XtraMessageBox.Show("İstifadəçi özü özünü bazada blokdan çıxara bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if(used_user_id >= 0)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == used_user_id).FULLNAME;
                XtraMessageBox.Show("İstifadəçinin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edildiyi üçün onu bazada blokdan çıxarmaq olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş istifadəçini blokdan çıxarmaq istəyirsiniz? Əgər razısınızsa həmin istifadəçinin kompyuterində LRS sisteminin bağlı olduğundan əmin olun. Əks halda bazada problemlər baş verə bilər.", "İstifadəçinin blokdan çıxarılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_UNLOCK_USER", "P_USER_ID", UserID, "İstifadəçi sistemdən çıxarılmadı."); 
                LoadUsersDataGridView();
            }
        }
    }
}