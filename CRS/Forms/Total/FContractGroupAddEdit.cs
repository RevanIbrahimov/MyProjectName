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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.Total
{
    public partial class FContractGroupAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FContractGroupAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int GroupID;

        string currency_code,
            credit_name,
            status_name,
            group_name,
            group_note,
            contracts;

        int UsedUserID = -1;

        bool CurrentStatus = false,
           ContractUsed = false,
            FormStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        List<long> firstContractList = new List<long>();

        private void FContractGroupAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            GlobalProcedures.FillCheckedComboBox(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (5,6) ORDER BY ID", "Aktiv");
            GlobalProcedures.FillCheckedComboBox(CreditNameComboBox, "CREDIT_NAMES", "NAME,NAME_EN,NAME_RU", null);

            LoadContracts();
            ShowPanel();
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_GROUP", GlobalVariables.V_UserID, "WHERE ID = " + GroupID + " AND USED_USER_ID = -1");
                LoadGroup();
                ContractUsed = (UsedUserID >= 0);

                if (ContractUsed)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş qrupun məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş qrupun hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
                OnlySelectedCheck_CheckedChanged(sender, EventArgs.Empty);
            }
            else
                FilterContracts();

            FormStatus = true;
        }

        public void ComponentEnable(bool status)
        {
            NameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadContracts()
        {
            string sql = $@"WITH C
                                 AS (SELECT 1 SS,
                                            C.CONTRACT_ID,
                                            C.CONTRACT_CODE,
                                            H.HOSTAGE,
                                            S.STATUS_NAME,
                                            CN.NAME CREDIT_NAME,
                                            C.USED_USER_ID,
                                            C.STATUS_ID,
                                            C.CURRENCY_CODE,
                                            NVL(COM.COMMITMENT_NAME, CUS.CUSTOMER_NAME) CUSTOMER_NAME
                                       FROM CRS_USER.V_CONTRACTS C,
                                            CRS_USER.V_HOSTAGE H,
                                            CRS_USER.STATUS S,
                                            CRS_USER.CREDIT_NAMES CN,
                                            CRS_USER.V_COMMITMENTS COM,
                                            CRS_USER.V_CUSTOMERS CUS
                                      WHERE     C.CONTRACT_ID = H.CONTRACT_ID
                                            AND CUS.ID = C.CUSTOMER_ID
                                            AND CUS.PERSON_TYPE_ID = C.CUSTOMER_TYPE_ID
                                            AND C.CONTRACT_ID = COM.CONTRACT_ID(+)
                                            AND C.STATUS_ID = S.ID
                                            AND C.CREDIT_NAME_ID = CN.ID),
                                 G
                                 AS (SELECT CONTRACT_ID
                                       FROM CRS_USER.CONTRACT_GROUP_DETAILS
                                      WHERE GROUP_ID = {GroupID})
                              SELECT C.*, NVL2 (G.CONTRACT_ID, 1, 0) CONTROL
                                FROM C LEFT JOIN G ON C.CONTRACT_ID = G.CONTRACT_ID
                            ORDER BY C.CONTRACT_CODE DESC";

            ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContracts", "Müqavilələrin siyahısı yüklənmədi.");
            BOK.Enabled = (ContractGridView.RowCount > 0);

            try
            {
                ContractGridView.BeginUpdate();
                for (int i = 0; i < ContractGridView.RowCount; i++)
                {
                    DataRow row = ContractGridView.GetDataRow(ContractGridView.GetVisibleRowHandle(i));
                    if (Convert.ToInt32(row["CONTROL"].ToString()) == 1)
                    {
                        ContractGridView.SelectRow(i);
                        firstContractList.Add(Convert.ToInt64(row["CONTRACT_ID"]));
                    }
                }
            }
            finally
            {
                ContractGridView.EndUpdate();
            }
        }

        private void LoadGroup()
        {
            string sql = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CONTRACT_GROUP WHERE ID = {GroupID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadGroup", "Müqavilə qrupunun detalları açılmadı.");
            if (dt.Rows.Count > 0)
            {
                group_name = dt.Rows[0]["NAME"].ToString();
                group_note = dt.Rows[0]["NOTE"].ToString();
                NameText.Text = group_name;
                NoteText.Text = group_note;
                UsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPanel();
        }

        void ShowPanel()
        {
            SearchPanel.Visible = (SearchBarButton.Down);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContracts();
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }

        private void ContractGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void StatusComboBox_EditValueChanged(object sender, EventArgs e)
        {
            status_name = " [STATUS_NAME] IN ('" + StatusComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void CreditNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            credit_name = " [CREDIT_NAME] IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private bool ControlGroupDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qrupun adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlGroupDetails())
            {
                if (TransactionName == "INSERT")
                    InsertGroup();
                else
                    UpdateGroup();
                this.Close();
            }
        }

        private void ContractGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (!FormStatus)
                return;

            if (e.Action == CollectionChangeAction.Add)
            {
                DataRow row = ContractGridView.GetDataRow(e.ControllerRow);
                if (firstContractList.IndexOf(Convert.ToInt64(row["CONTRACT_ID"])) == -1)
                    firstContractList.Add(Convert.ToInt64(row["CONTRACT_ID"]));
            }
            else if (e.Action == CollectionChangeAction.Remove)
            {
                DataRow row = ContractGridView.GetDataRow(e.ControllerRow);
                firstContractList.Remove(Convert.ToInt64(row["CONTRACT_ID"]));
            }
        }

        private void ContractGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            BOK.Enabled = (ContractGridView.RowCount > 0);

            if (!FormStatus)
                return;

            SelectedRow();
        }

        void SelectedRow()
        {
            try
            {
                FormStatus = false;
                ContractGridView.BeginUpdate();
                for (int i = 0; i < ContractGridView.RowCount; i++)
                {
                    DataRow row = ContractGridView.GetDataRow(ContractGridView.GetVisibleRowHandle(i));
                    for (int j = 0; j < firstContractList.Count; j++)
                    {
                        if (Convert.ToInt32(row["CONTRACT_ID"].ToString()) == firstContractList[j])
                        {
                            ContractGridView.SelectRow(i);
                        }
                    }
                }
            }
            finally
            {
                FormStatus = true;
                ContractGridView.EndUpdate();
            }
        }

        private void OnlySelectedCheck_CheckedChanged(object sender, EventArgs e)
        {
            contracts = null;
            if (FormStatus)
                SelectedRow();
            if (firstContractList.Count > 0 && OnlySelectedCheck.Checked)
            {
                for (int i = 0; i < firstContractList.Count; i++)
                {
                    contracts = contracts + firstContractList[i].ToString() + ",";
                }
                contracts = " [CONTRACT_ID] IN (" + contracts.TrimEnd(',') + ")";
                OnlySelectedCheck.Font = new Font(OnlySelectedCheck.Font.FontFamily, OnlySelectedCheck.Font.Size, FontStyle.Bold);
            }
            else
                OnlySelectedCheck.Font = new Font(OnlySelectedCheck.Font.FontFamily, OnlySelectedCheck.Font.Size, FontStyle.Regular);
            FilterContracts();
        }

        private void InsertGroup()
        {
            GroupID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER.CONTRACT_GROUP(NAME,NOTE)VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}') RETURNING ID INTO :ID",
                                                    "Müqavilələrin qrupu yaradılmadı.",
                                                    "ID",
                                                    this.Name + "/InsertGroup");
            if (GroupID == 0)
                return;

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < ContractGridView.SelectedRowsCount; i++)
            {
                rows.Add(ContractGridView.GetDataRow(ContractGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACT_GROUP_DETAILS(GROUP_ID,CONTRACT_ID,CREATOR_USER)VALUES({GroupID},{row["CONTRACT_ID"]},{GlobalVariables.V_UserID})",
                                                    "Müqavilələrin qrupu yaradılmadı.",
                                              this.Name + "/InsertGroup");
            }
        }

        private void UpdateGroup()
        {
            if (group_name != NameText.Text.Trim() || group_note != NoteText.Text.Trim())
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_GROUP SET NAME = '{NameText.Text.Trim()}',NOTE = '{NoteText.Text.Trim()}' WHERE ID = {GroupID}",
                                                        "Müqavilələrin qrupu dəyişdirilmədi.",
                                                        this.Name + "/UpdateGroup");


            GlobalProcedures.ExecuteQuery($@"DELETE CRS_USER.CONTRACT_GROUP_DETAILS WHERE GROUP_ID = {GroupID}",
                                                    "Qrupa daxil olan müqavilələr dəyişdirilmədi.",
                                                    this.Name + "/UpdateGroup");

            for (int i = 0; i < firstContractList.Count; i++)
            {
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACT_GROUP_DETAILS(GROUP_ID,CONTRACT_ID,CREATOR_USER)VALUES({GroupID},{firstContractList[i]},{GlobalVariables.V_UserID})",
                                                    "Müqavilələrin qrupu yaradılmadı.",
                                              this.Name + "/InsertGroup");
            }
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_code = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            FilterContracts();
        }

        private void FContractGroupAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_GROUP", -1, "WHERE ID = " + GroupID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void FilterContracts()
        {
            ColumnView view = ContractGridView;

            //Currency
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                    new ColumnFilterInfo(currency_code, ""));
            else
                view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);

            //Status
            if (!String.IsNullOrEmpty(StatusComboBox.Text))
                view.ActiveFilter.Add(view.Columns["STATUS_NAME"],
                    new ColumnFilterInfo(status_name, ""));
            else
                view.ActiveFilter.Remove(view.Columns["STATUS_NAME"]);

            //CreditName
            if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
                view.ActiveFilter.Add(view.Columns["CREDIT_NAME"],
                    new ColumnFilterInfo(credit_name, ""));
            else
                view.ActiveFilter.Remove(view.Columns["CREDIT_NAME"]);

            //Contract
            if (!String.IsNullOrWhiteSpace(contracts))
                view.ActiveFilter.Add(view.Columns["CONTRACT_ID"],
                    new ColumnFilterInfo(contracts, ""));
            else
                view.ActiveFilter.Remove(view.Columns["CONTRACT_ID"]);
        }


        private void ContractGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(ContractGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(6, ContractGridView, e);
        }
    }
}