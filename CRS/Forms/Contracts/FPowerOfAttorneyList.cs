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

namespace CRS.Forms.Contracts
{
    public partial class FPowerOfAttorneyList : DevExpress.XtraEditors.XtraForm
    {
        public FPowerOfAttorneyList()
        {
            InitializeComponent();
        }
        string PowerID, PowerNumber, ContractCode, PowerCode;
        int UsedUserID = -1;

        private void FPowerOfAttorneyList_Load(object sender, EventArgs e)
        {
            ToDateValue.Properties.MaxValue = DateTime.Today;
            LoadPowerOfAttorneyList();
            SearchDockPanel.Hide();
        }

        private void LoadPowerOfAttorneyList()
        {
            string last = (LastCheck.Checked) ? $@" AND PA.ID = (SELECT MAX (ID)
                                                                        FROM CRS_USER.POWER_OF_ATTORNEY
                                                                       WHERE CONTRACT_ID = PA.CONTRACT_ID AND IS_REVOKE = PA.IS_REVOKE)" : null,
                   revoke = (ActiveCheck.Checked && !CloseCheck.Checked) ? $@" AND PA.IS_REVOKE = 0" :
                                                (!ActiveCheck.Checked && CloseCheck.Checked) ? $@" AND PA.IS_REVOKE = 1" : null,
                   status = (ActiveContractCheck.Checked && !CloseCheck.Checked) ? $@" AND C.STATUS_ID = 5" :
                                                (!ActiveContractCheck.Checked && ClosedContractCheck.Checked) ? $@" AND C.STATUS_ID = 6" : null,
                   commitment = (CommitmentCheck.Checked)? " AND (RTRIM (RTRIM (PA.FULLNAME, 'oğlu'), 'qızı'),PA.CONTRACT_ID) IN (SELECT RTRIM (RTRIM (COMMITMENT_NAME, 'oğlu'), 'qızı'),CONTRACT_ID FROM CRS_USER.CONTRACT_ALL_COMMITMENTS)" : null,
                   date = (!String.IsNullOrWhiteSpace(FromDateValue.Text) && !String.IsNullOrWhiteSpace(ToDateValue.Text))? 
                                    $@" AND TRUNC(PA.INSERT_DATE) BETWEEN TO_DATE('{FromDateValue.Text}', 'DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}', 'DD.MM.YYYY')" : null,
                        sql = $@"SELECT PA.ID,
                                   PA.IS_REVOKE,
                                   PA.POWER_CODE,
                                   C.CONTRACT_CODE,                                   
                                   PA.FULLNAME || ' (' || CS.SERIES || ' ' || PA.CARD_NUMBER||')' FULLNAME,
                                   SUBSTR(H.HOSTAGE,1,7) CARNUMBER, 
                                   PA.INSERT_DATE,
                                   PA.POWER_DATE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = PA.INSERT_USER)
                                      INSERT_USER,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = PA.UPDATE_USER)
                                      UPDATE_USER,
                                   PA.UPDATE_DATE,
                                   PA.POWER_NUMBER,
                                   C.USED_USER_ID,
                                   C.STATUS_ID,
                                   PA.IS_RESPONSIBLE
                              FROM CRS_USER.POWER_OF_ATTORNEY PA,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.V_HOSTAGE H
                             WHERE     PA.CONTRACT_ID = C.CONTRACT_ID
                                   AND PA.CONTRACT_ID = H.CONTRACT_ID
                                   AND PA.CARD_SERIES_ID = CS.ID{status}{last}{revoke}{commitment}{date}
                           ORDER BY PA.ID DESC";

            PowerGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPowerOfAttorneyList", "Etibarnamələr yüklənmədi.");

            EnabledButton();
        }

        private void EnabledButton()
        {
            ShowBarButton.Enabled = CancelBarButton.Enabled = (PowerGridView.RowCount > 0);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PowerGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            int isRevoke = Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_REVOKE"));
            if (e.Column == Power_Status && isRevoke == 0)
                e.Value = Properties.Resources.ok_16;
            else if (e.Column == Power_Status && isRevoke != 0)
                e.Value = Properties.Resources.cancel_16;

            int isResponsible = Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_RESPONSIBLE"));
            if (e.Column == Power_Responsibile && isResponsible == 1)
                e.Value = Properties.Resources.status_blue_16;
            else if (e.Column == Power_Responsibile && isResponsible == 2)
                e.Value = Properties.Resources.status_offline_16;

            GlobalProcedures.GenerateAutoRowNumber(sender, Power_SS, e);
        }

        private void PowerGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerOfAttorneyList();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PowerGridControl);
        }

        private void PowerGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e, 8f, 8f, 8f, 8f);
        }

        private void CancelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalVariables.V_UserID != UsedUserID && UsedUserID != -1)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçdiyiniz müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = XtraMessageBox.Show("<b>" + ContractCode + "</b> saylı lizinq müqaviləsinin <color=255,0,0><b>" + PowerCode + "</color></b> nömrəli etibarnaməsini ləğv etmək istəyirsiniz?", "Etibarnamənin ləğv edilməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.POWER_OF_ATTORNEY SET IS_REVOKE = 1 WHERE ID = {PowerID}", "Etibarnamə ləğv edilmədi.", this.Name + "/CancelBarButton_ItemClick");
                LoadPowerOfAttorneyList();
            }
        }

        private void PowerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PowerGridView.GetFocusedDataRow();
            if (row != null)
            {
                PowerID = row["ID"].ToString();
                PowerNumber = row["POWER_NUMBER"].ToString();
                ContractCode = row["CONTRACT_CODE"].ToString();
                PowerCode = row["POWER_CODE"].ToString();
                UsedUserID = Convert.ToInt32(row["USED_USER_ID"].ToString());
            }
        }

        private void PowerGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PowerGridView, PopupMenu, e);
        }

        private void PowerGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(PowerGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(6, PowerGridView, e);
        }

        private void PowerGridView_DoubleClick(object sender, EventArgs e)
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.POWER_FILE FROM CRS_USER.POWER_OF_ATTORNEY T WHERE T.ID = {PowerID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + PowerNumber + ".docx",
                                                "POWER_FILE");
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PowerGridControl, "xls");
        }

        private void PowerGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadPowerOfAttorneyList();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void ActiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadPowerOfAttorneyList();
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
        }

        private void ShowBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.POWER_FILE FROM CRS_USER.POWER_OF_ATTORNEY T WHERE T.ID = {PowerID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + PowerNumber + ".docx",
                                                "POWER_FILE");
        }
    }
}