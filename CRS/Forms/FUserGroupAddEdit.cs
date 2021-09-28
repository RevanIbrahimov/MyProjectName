﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FUserGroupAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FUserGroupAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, GroupID;
        int GroupUsedUserID = -1, topindex, old_row_id;
        bool GroupUsed = false, CurrentStatus = false;
        string UserID;

        public delegate void DoEvent();
        public event DoEvent RefreshUserGroupDataGridView;

        private void FUserGroupAddEdit_Load(object sender, EventArgs e)
        {
            //permision
            if (Class.GlobalVariables.V_UserID > 0)
            {
                NewUserBarButton.Enabled = Class.GlobalVariables.AddUser;
                EditUserBarButton.Enabled = Class.GlobalVariables.EditUser;
            }

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.USER_GROUP", Class.GlobalVariables.V_UserID, "WHERE ID = " + GroupID + " AND USED_USER_ID = -1");
                InsertUserGroupPermissionTemp();
                GroupUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.USER_GROUP WHERE ID = " + GroupID);
                GroupUsed = (GroupUsedUserID >= 0);
                
                if (GroupUsed)
                {
                    if (GlobalVariables.V_UserID != GroupUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == GroupUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Qrupun məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş qrupun hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
                LoadGroupDetails();
                LoadPermissionDataGridView();
            }
            else
                GroupID = GlobalFunctions.GetOracleSequenceValue("USER_GROUP_SEQUENCE").ToString();
        }

        private void ComponentEnable(bool status)
        {
            AzGroupNameText.Enabled = EnGroupNameText.Enabled = RuGroupNameText.Enabled = StandaloneBarDockControl.Enabled = 
                UsersStandaloneBarDockControl.Enabled = BOK.Visible = !status;

            if (status == false)
            {
                PermissionPopupMenu.Manager = PermissionBarManager;
                UserPopupMenu.Manager = UserBarManager;
            }
            else            
                PermissionPopupMenu.Manager = UserPopupMenu.Manager = null;            
        }

        private void LoadGroupDetails()
        {
            string s = "SELECT GROUP_NAME,GROUP_NAME_EN,GROUP_NAME_RU,NOTE FROM CRS_USER.USER_GROUP WHERE ID = " + GroupID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);
                foreach (DataRow dr in dt.Rows)
                {
                    AzGroupNameText.Text = dr["GROUP_NAME"].ToString();
                    EnGroupNameText.Text = dr["GROUP_NAME_EN"].ToString();
                    RuGroupNameText.Text = dr["GROUP_NAME_RU"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçi qrupunun parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void LoadPermissionDataGridView()
        {
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT R.ROLE_DESCRIPTION,RD.DETAIL_NAME_AZ FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID;
                        break;
                    case "EN":
                        s = "SELECT R.ROLE_DESCRIPTION,RD.DETAIL_NAME_AZ FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID;
                        break;
                    case "RU":
                        s = "SELECT R.ROLE_DESCRIPTION,RD.DETAIL_NAME_AZ FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP RDT,CRS_USER.ROLES R, CRS_USER.ALL_ROLE_DETAILS RD WHERE RD.ID = RDT.ROLE_DETAIL_ID AND R.ROLE_ID = RD.ROLE_ID AND RDT.GROUP_ID = " + GroupID;
                        break;
                }

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPermissionDataGridView");
                
                PermissionGridControl.DataSource = dt;
                PermissionGridView.PopulateColumns();
                PermissionGridView.Columns[0].Caption = "Modulun adı";
                PermissionGridView.Columns[1].Caption = "Hüququn adı";

                for (int i = 0; i < PermissionGridView.Columns.Count; i++)
                {
                    PermissionGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    PermissionGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                PermissionGridView.BeginSort();
                try
                {
                    PermissionGridView.Columns[0].GroupIndex = 0;
                }
                finally
                {
                    PermissionGridView.EndSort();
                }

                PermissionGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Hüquqlar cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void LoadUsersDataGridView()
        {
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ": s = "SELECT 1 SS,E.SURNAME||' '||E.NAME||' '||E.PATRONYMIC||' '||DECODE(E.SEX_ID,1,'oğlu','qızı') FULLNAME,S.STATUS_NAME,E.SESSION_ID,E.ID FROM CRS_USER.CRS_USERS E,CRS_USER.STATUS S WHERE E.STATUS_ID = S.ID AND E.GROUP_ID = " + GroupID + " ORDER BY 2";
                        break;
                    case "EN": s = "SELECT 1 SS,E.SURNAME||' '||E.NAME||' '||E.PATRONYMIC||' '||DECODE(E.SEX_ID,1,'oğlu','qızı') FULLNAME,S.STATUS_NAME_EN,E.SESSION_ID,E.ID FROM CRS_USER.CRS_USERS E,CRS_USER.STATUS S WHERE E.STATUS_ID = S.ID AND E.GROUP_ID = " + GroupID + " ORDER BY 2";
                        break;
                    case "RU": s = "SELECT 1 SS,E.SURNAME||' '||E.NAME||' '||E.PATRONYMIC||' '||DECODE(E.SEX_ID,1,'oğlu','qızı') FULLNAME,S.STATUS_NAME_RU,E.SESSION_ID,E.ID FROM CRS_USER.CRS_USERS E,CRS_USER.STATUS S WHERE E.STATUS_ID = S.ID AND E.GROUP_ID = " + GroupID + " ORDER BY 2";
                        break;
                }

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUsersDataGridView");


                UsersGridControl.DataSource = dt;
                UsersGridView.PopulateColumns();

                UsersGridView.Columns[0].Caption = "S/s";
                UsersGridView.Columns[1].Caption = "İstifadəçilərin adı";
                UsersGridView.Columns[2].Caption = "Statusu";
                UsersGridView.Columns[3].Visible = false;
                UsersGridView.Columns[4].Visible = false;

                UsersGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UsersGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (UsersGridView.RowCount > 0)
                {
                    UsersGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (GlobalVariables.V_UserID > 0)
                        EditUserBarButton.Enabled = GlobalVariables.EditUser;
                    else
                        EditUserBarButton.Enabled = true;
                    DeleteUserBarButton.Enabled = true;
                }
                else                
                    EditUserBarButton.Enabled = DeleteUserBarButton.Enabled = false;
                UsersGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Qrupa daxil olan istifadəçilərin siyahısı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlGroupDetails()
        {
            bool b = false;

            if (AzGroupNameText.Text.Length == 0)
            {
                AzGroupNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Azərbaycan dilində qrupun adı daxil edilməyib.");                
                AzGroupNameText.BackColor = GlobalFunctions.ElementColor();
                AzGroupNameText.Focus();
                return false;
            }
            else
                b = true;

            if (EnGroupNameText.Text.Length == 0)
            {
                EnGroupNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İngilis dilində qrupun adı daxil edilməyib.");                
                EnGroupNameText.BackColor = GlobalFunctions.ElementColor();
                EnGroupNameText.Focus();
                return false;
            }
            else
                b = true;

            if (RuGroupNameText.Text.Length == 0)
            {
                RuGroupNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Rus dilində qrupun adı daxil edilməyib.");               
                RuGroupNameText.BackColor = GlobalFunctions.ElementColor();
                RuGroupNameText.Focus();
                return false;
            }
            else
                b = true;

            if (PermissionGridView.RowCount == 0)
            {
                XtraMessageBox.Show("Qrupa hüquq verilməyib.");
                return false;
            }
            else
                b = true;
            return b;
        }

        private void InsertUserGroup()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.USER_GROUP(ID,GROUP_NAME,GROUP_NAME_EN,GROUP_NAME_RU,NOTE)VALUES(" + GroupID + ",'" + AzGroupNameText.Text.Trim() + "','" + EnGroupNameText.Text.Trim() + "','" + RuGroupNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "')",
                                                "İstifadəçi qrupu daxil edilmədi.",
                                                this.Name + "/InsertUserGroup");
        }

        private void UpdateUserGroup()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.USER_GROUP SET GROUP_NAME = '" + AzGroupNameText.Text.Trim() + "',GROUP_NAME_EN = '" + EnGroupNameText.Text.Trim() + "',GROUP_NAME_RU = '" + RuGroupNameText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + GroupID,
                                                "İstifadəçi qrupu dəyişdirilmədi.",
                                                this.Name + "/InsertUserGroup");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlGroupDetails())
            {
                if (TransactionName == "INSERT")
                    InsertUserGroup();
                else
                    UpdateUserGroup();
                InsertUserGroupPermission();
                this.Close();
            }
        }

        void RefreshUserGroupPermission()
        {
            LoadPermissionDataGridView();
        }

        private void LoadFPermissionAddEdit(string groupID)
        {
            FPermissionAddEdit fc = new FPermissionAddEdit();
            fc.GroupID = groupID;
            fc.GroupName = AzGroupNameText.Text.Trim();
            fc.RefreshUserGroupPermissionDataGridView += new FPermissionAddEdit.DoEvent(RefreshUserGroupPermission);
            fc.ShowDialog();
        }


        private void PermissionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPermissionAddEdit(GroupID);
        }

        private void RefreshPermissionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPermissionDataGridView();
        }

        private void InsertUserGroupPermission()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.ALL_USER_GROUP_ROLE_DETAILS WHERE GROUP_ID = {GroupID}",
                                             $@"INSERT INTO CRS_USER.ALL_USER_GROUP_ROLE_DETAILS(ID,GROUP_ID,ROLE_DETAIL_ID)SELECT ID,GROUP_ID,ROLE_DETAIL_ID FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = {GroupID}",
                                                    "İstifadəçi qrupunun hüquqları əsas cədvələ daxil edilmədi.",
                                             this.Name + "/InsertUserGroupPermission");
        }

        private void DeleteUserGroupPermissionTemp()
        {
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = {GroupID}", 
                                              "İstifadəçi qrupunun hüquqları üçün olan temp məlumatlar silinmədi.",
                                              this.Name + "/DeleteUserGroupPermissionTemp");
        }

        private void InsertUserGroupPermissionTemp()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP WHERE GROUP_ID = {GroupID}",
                                             $@"INSERT INTO CRS_USER_TEMP.USER_GROUP_ROLE_DETAILS_TEMP(ID,GROUP_ID,ROLE_DETAIL_ID)SELECT ID,GROUP_ID,ROLE_DETAIL_ID FROM CRS_USER.ALL_USER_GROUP_ROLE_DETAILS WHERE GROUP_ID = {GroupID}",
                                                    "İstifadəçi qrupunun hüquqları temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertUserGroupPermissionTemp");
        }

        private void FUserGroupAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.USER_GROUP", -1, "WHERE ID = " + GroupID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            DeleteUserGroupPermissionTemp();
            this.RefreshUserGroupDataGridView();
        }

        private void PermissionGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PermissionGridView, PermissionPopupMenu, e);
        }

        private void UsersGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void UsersGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void UsersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(UsersGridView, UserPopupMenu, e);
        }

        void RefreshUser()
        {
            LoadUsersDataGridView();
        }

        private void LoadFUserAddEdit(string transaction, string UserID, string group_id)
        {
            FUserAddEdit fc = new FUserAddEdit();
            fc.TransactionName = transaction;
            fc.UserID = UserID;
            fc.GroupID = group_id;
            fc.RefreshUserDataGridView += new FUserAddEdit.DoEvent(RefreshUser);
            fc.ShowDialog();
        }

        private void NewUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFUserAddEdit("INSERT", null, GroupID);
        }

        private void EditUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFUserAddEdit("EDIT", UserID, GroupID);
        }

        private void UsersGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditUserBarButton.Enabled && UsersStandaloneBarDockControl.Enabled)
                LoadFUserAddEdit("EDIT", UserID, GroupID);
        }

        private void UsersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = UsersGridView.GetFocusedDataRow();
            if (row != null)
                UserID = row["ID"].ToString();
        }

        private void UsersGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForConnect(UsersGridView, e);
        }

        private void DeleteUser()
        {
            if (EditUserBarButton.Enabled)
            {
                if (GlobalFunctions.GetID($@"SELECT SESSION_ID FROM CRS_USER.CRS_USERS WHERE ID = {UserID}") != 0)
                    XtraMessageBox.Show("İstifadəçi sistemə daxil olduğu üçün onu bu qrupdan silmək olmaz. İstifadəçini qrupdan silmək üçün onu sistemdən çıxmaq lazımdır.", "Seçilmiş istifadəçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş istifadəçini qrupdan silmək üçün bu istifadəçinin qrupunu dəyişmək lazımdır. Buna razısınız?", "İstifadəçinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        LoadFUserAddEdit("EDIT", UserID, GroupID);
                }
            }
        }

        private void DeleteUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteUser();
            RefreshUser();
        }

        private void LoadFListOfAutomaticAdd(string transaction, string GroupID, string GroupName)
        {
            FListOfAutomaticAdd fc = new FListOfAutomaticAdd();
            fc.TransactionType = transaction;
            fc.GroupID = GroupID;
            fc.GroupName = GroupName;
            fc.RefreshListDataGridView += new FListOfAutomaticAdd.DoEvent(RefreshUser);
            fc.ShowDialog();
        }

        private void AddUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": LoadFListOfAutomaticAdd("U", GroupID, AzGroupNameText.Text);
                    break;
                case "EN": LoadFListOfAutomaticAdd("U", GroupID, EnGroupNameText.Text);
                    break;
                case "RU": LoadFListOfAutomaticAdd("U", GroupID, RuGroupNameText.Text);
                    break;
            }
        }

        private void OtherInfoXtraTabControl_Click(object sender, EventArgs e)
        {
            switch (OtherInfoXtraTabControl.SelectedTabPageIndex)
            {
                case 0: LoadPermissionDataGridView();
                    break;
                case 1: LoadUsersDataGridView();
                    break;
            }
        }

        private void RefreshUserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadUsersDataGridView();
        }

        private void UsersGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (UsersGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    EditUserBarButton.Enabled = GlobalVariables.EditUser;
                else
                    EditUserBarButton.Enabled = true;
                DeleteUserBarButton.Enabled = true;
            }
            else            
                EditUserBarButton.Enabled = DeleteUserBarButton.Enabled = false;
        }
    }
}