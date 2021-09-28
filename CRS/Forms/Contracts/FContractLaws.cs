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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace CRS.Forms.Contracts
{
    public partial class FContractLaws : DevExpress.XtraEditors.XtraForm
    {
        public FContractLaws()
        {
            InitializeComponent();
        }
        string ContractID, ContractCode, ContractStartDate;

        private void FContractLaws_Load(object sender, EventArgs e)
        {
            LoadLawsDataGridView();
        }

        private void LoadLawsDataGridView()
        {
            string s = @"SELECT 1 SS,
                                 CL.ID,
                                 CL.CONTRACT_ID,
                                 C.CONTRACT_CODE || ' - ' || CUS.CUSTOMER_NAME CONTRACT_CODE,
                                 CL.APPLICANT,
                                 CL.DEFANDANT_NAME,
                                 L.NAME LAW_NAME,
                                 CL.JUDGE_NAME,
                                 CL.REPRESENTATIVE,
                                 CL.START_DATE LAW_START_DATE,
                                 LS.NAME STATUS_NAME,
                                 (SELECT CLS.LAST_DATE
                                    FROM CRS_USER.CONTRACT_LAWS CLS
                                   WHERE     CLS.CONTRACT_ID = CL.CONTRACT_ID
                                         AND CLS.ID =
                                                (SELECT MAX (ID)
                                                   FROM CRS_USER.CONTRACT_LAWS
                                                  WHERE CONTRACT_ID = CLS.CONTRACT_ID AND ID < CL.ID))
                                    LAST_DATE,
                                 CL.NEXT_DATE,
                                 CL.NOTE,
                                 CL.CREATED_USER_NAME,
                                 C.USED_USER_ID,
                                 C.START_DATE CONTRACT_START_DATE,
                                 CL.IS_ACTIVE
                            FROM CRS_USER.CONTRACT_LAWS CL,
                                 CRS_USER.V_CONTRACTS C,
                                 CRS_USER.V_CUSTOMERS CUS,
                                 CRS_USER.LAWS L,
                                 CRS_USER.LAW_STATUS LS,
                                 (  SELECT MAX (ID) ID, LAW_ID, CONTRACT_ID, IS_ACTIVE, START_DATE
                                      FROM CRS_USER.CONTRACT_LAWS CL
                                  GROUP BY LAW_ID, CONTRACT_ID, IS_ACTIVE, START_DATE) AL
                           WHERE     CL.CONTRACT_ID = C.CONTRACT_ID
                                 AND CL.LAW_ID = L.ID
                                 AND CL.LAW_STATUS_ID = LS.ID
                                 AND CL.ID = AL.ID
                                 AND CL.CONTRACT_ID = AL.CONTRACT_ID
                                 AND C.CUSTOMER_ID = CUS.ID
                        ORDER BY CL.IS_ACTIVE DESC, CL.NEXT_DATE DESC, CL.START_DATE DESC";
            LawsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);

            if (LawsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    NewBarButton.Enabled = (GlobalVariables.AddLawDetail || GlobalVariables.AddProsecute);
                else
                    NewBarButton.Enabled = true;
                EditBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = false;            
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LawsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawsDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(LawsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(LawsGridControl, "xls");
        }

        void RefreshLaws()
        {
            LoadLawsDataGridView();
        }

        private void LoadLawDetails(string transaction, string contractid)
        {
            FLawDetails fld = new FLawDetails();
            fld.TransactionName = transaction;
            fld.ContractCode = ContractCode;
            fld.ContractStartDate = ContractStartDate;
            fld.ContractID = contractid;
            fld.RefreshLawsDataGridView += new FLawDetails.DoEvent(RefreshLaws);
            fld.ShowDialog();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawDetails("EDIT", ContractID);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawDetails("INSERT", null);
        }

        private void LawsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(LawsGridView, PopupMenu, e);
        }

        private void LawsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (int.Parse(LawsGridView.GetRowCellDisplayText(e.RowHandle, LawsGridView.Columns["USED_USER_ID"])) >= 0)
                GlobalProcedures.GridRowCellStyleForBlock(LawsGridView, e);

            if (e.Column.FieldName == "NEXT_DATE")
            {
                e.Appearance.BackColor = Color.Yellow;
                e.Appearance.BackColor2 = Color.Yellow;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        private void LawsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if ( Convert.ToInt32(LawsGridView.GetListSourceRowCellValue(rowIndex, "IS_ACTIVE")) == 1)
                e.Value = Properties.Resources.ok_16;
            else
                e.Value = Properties.Resources.cancel_16;
        }

        private void LawsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e, 8f, 8f, 8f, 8f);
        }        

        private void FilterLaws()
        {
            ColumnView view = LawsGridView;

            if (ActiveBarCheck.Checked && !CanceledBarCheck.Checked)
                view.ActiveFilter.Add(view.Columns["IS_ACTIVE"],
                    new ColumnFilterInfo("[IS_ACTIVE] = 1", ""));
            else if (!ActiveBarCheck.Checked && CanceledBarCheck.Checked)
                view.ActiveFilter.Add(view.Columns["IS_ACTIVE"],
                    new ColumnFilterInfo("[IS_ACTIVE] = 0", ""));
            else
                view.ActiveFilter.Remove(view.Columns["IS_ACTIVE"]);
        }

        private void ActiveBarCheck_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FilterLaws();
        }

        private void LawsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadLawDetails("EDIT", ContractID);
        }

        private void LawsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = LawsGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractID = row["CONTRACT_ID"].ToString();
                ContractCode = row["CONTRACT_CODE"].ToString();
                ContractStartDate = row["CONTRACT_START_DATE"].ToString().Substring(0,10);
            }
        }
    }
}