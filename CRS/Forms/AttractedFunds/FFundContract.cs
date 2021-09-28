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

namespace CRS.Forms.AttractedFunds
{
    public partial class FFundContract : DevExpress.XtraEditors.XtraForm
    {
        public FFundContract()
        {
            InitializeComponent();
        }
        string ContractID, ContractNumber;
        int SourceID, topindex, old_row_num;

        public delegate void DoEvent();
        public event DoEvent RefreshFundsDataGridView;

        private void FFundContract_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddFundContract;

            LoadContractsDataGridView();
        }

        private void LoadContractsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 FC.ID,
                                 FS.NAME,
                                 (CASE
                                     WHEN FS.ID = 6
                                     THEN
                                        (SELECT B.LONG_NAME
                                           FROM CRS_USER.BANKS B
                                          WHERE (B.ID) IN (SELECT BANK_ID
                                                             FROM CRS_USER.BANK_CONTRACTS
                                                            WHERE FUNDS_CONTRACT_ID = FC.ID))
                                     WHEN FS.ID = 10
                                     THEN
                                        (SELECT FULLNAME
                                           FROM CRS_USER.FOUNDERS
                                          WHERE ID = (SELECT FOUNDER_ID
                                                        FROM CRS_USER.FOUNDER_CONTRACTS
                                                       WHERE FUNDS_CONTRACT_ID = FC.ID))
                                     ELSE
                                        (SELECT NAME
                                           FROM CRS_USER.FUNDS_SOURCES_NAME
                                          WHERE     ID = FC.FUNDS_SOURCE_NAME_ID
                                                AND SOURCE_ID = FC.FUNDS_SOURCE_ID)
                                  END)
                                    SOURCE_NAME,
                                 FC.CONTRACT_NUMBER,
                                 FC.REGISTRATION_NUMBER,
                                 FP.PERCENT_VALUE||' %' INTEREST,
                                 FC.PERIOD || ' ay' PERIOD,
                                 TO_CHAR (FC.START_DATE, 'DD.MM.YYYY') S_DATE,
                                 TO_CHAR (FC.END_DATE, 'DD.MM.YYYY') E_DATE,
                                 FC.AMOUNT,
                                 C.CODE,
                                 S.STATUS_NAME,
                                 FC.USED_USER_ID,
                                 FC.FUNDS_SOURCE_ID,
                                 FC.STATUS_ID
                            FROM CRS_USER.FUNDS_CONTRACTS FC,
                                 CRS_USER.FUNDS_SOURCES FS,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.STATUS S,
                                 CRS_USER.V_LAST_FUNDS_PERCENT FP
                           WHERE     FC.STATUS_ID = S.ID
                                 AND FC.FUNDS_SOURCE_ID = FS.ID
                                 AND FC.CURRENCY_ID = C.ID
                                 AND FC.ID = FP.FUNDS_CONTRACTS_ID
                        ORDER BY FS.NAME, FC.START_DATE, FC.ID";

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractsDataGridView");
                ContractsGridControl.DataSource = dt;
                ContractsGridView.PopulateColumns();

                ContractsGridView.Columns[0].Caption = "S/s";
                ContractsGridView.Columns[1].Visible = false;
                ContractsGridView.Columns[2].Caption = "Mənbə";
                ContractsGridView.Columns[3].Caption = "Mənbəyin adı";
                ContractsGridView.Columns[4].Caption = "Müqavilənin nömrəsi";
                ContractsGridView.Columns[5].Caption = "Qeydiyyat nömrəsi";
                ContractsGridView.Columns[6].Caption = "İllik faiz";
                ContractsGridView.Columns[7].Caption = "Müddət";
                ContractsGridView.Columns[8].Caption = "Başlama tarixi";
                ContractsGridView.Columns[9].Caption = "Bitmə tarixi";
                ContractsGridView.Columns[10].Caption = "Məbləğ";
                ContractsGridView.Columns[11].Caption = "Valyuta";
                ContractsGridView.Columns[12].Caption = "Statusu";
                ContractsGridView.Columns[13].Visible = false;
                ContractsGridView.Columns[14].Visible = false;
                ContractsGridView.Columns[15].Visible = false;


                ContractsGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[5].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[5].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[6].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[6].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[7].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[7].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[8].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[8].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[9].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[9].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractsGridView.Columns[11].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractsGridView.Columns[11].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                ContractsGridView.Columns[10].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                ContractsGridView.Columns[10].DisplayFormat.FormatType = FormatType.Custom;
                if (ContractsGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                    {
                        DeleteBarButton.Enabled = GlobalVariables.DeleteFundContract;
                        EditBarButton.Enabled = (GlobalVariables.EditFundContract || GlobalVariables.EditFundPercent);
                    }
                    else
                        DeleteBarButton.Enabled = EditBarButton.Enabled = true;

                    ContractsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = false;

                ContractsGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Cəlb olunmuş vəsaitlərin müqavilələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContractsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
        
        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractsGridView, PopupMenu, e);
        }

        void RefreshContracts()
        {
            LoadContractsDataGridView();
        }

        private void LoadFFundContractAddEdit(string transaction, string contract_id)
        {
            topindex = ContractsGridView.TopRowIndex;
            old_row_num = ContractsGridView.FocusedRowHandle;
            FFundContractAddEdit fcae = new FFundContractAddEdit();
            fcae.TransactionName = transaction;
            fcae.ContractID = contract_id;
            fcae.RefreshContractsDataGridView += new FFundContractAddEdit.DoEvent(RefreshContracts);
            fcae.ShowDialog();
            ContractsGridView.TopRowIndex = topindex;
            ContractsGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundContractAddEdit("INSERT", null);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractsDataGridView();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundContractAddEdit("EDIT", ContractID);
        }

        private void ContractsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractsGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractID = row["ID"].ToString();
                ContractNumber = row["CONTRACT_NUMBER"].ToString();
                SourceID = Convert.ToInt32(row["FUNDS_SOURCE_ID"].ToString());
                OpenBarButton.Enabled = (Convert.ToInt32(row["STATUS_ID"].ToString()) == 14 && GlobalVariables.OpenFundContract);
                CloseBarButton.Enabled = (Convert.ToInt32(row["STATUS_ID"].ToString()) == 13 && GlobalVariables.CloseFundContract);
            }
        }

        private void ContractsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFFundContractAddEdit("EDIT", ContractID);
        }

        private void DeleteContract()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT CONTRACT_ID FROM CRS_USER.FUNDS_PAYMENTS UNION SELECT CONTRACT_ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP) WHERE CONTRACT_ID = " + ContractID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş müqaviləni silmək istəyirsiniz?", "Müqavilənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_FUND_CONTRACT_DELETE", "P_CONTRACT_ID", ContractID, "P_SOURCE_ID", SourceID, "Müqavilənin məlumatları silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Silmək istədiyiniz müqavilənin ödənişləri olduğu üçün bu müqaviləni silmək olmaz. ", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int ContractUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = " + ContractID);
            if ((ContractUsedUserID == -1) || (GlobalVariables.V_UserID == ContractUsedUserID))
                DeleteContract();
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ContractUsedUserID).FULLNAME;
                XtraMessageBox.Show("Silmək istədiyiniz müqavilənin məlumatları " + used_user_name + " tərəfindən istifadə edilir. Bu müqavilənin məlumatlarını silmək olmaz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadContractsDataGridView();
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(ContractsGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(14, ContractsGridView, e);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractsGridControl, "xls");
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractsGridControl);
        }

        private void CloseBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş <color=red><b>" + ContractNumber + "</b></color> nömrəli müqaviləni bağlamaq istəyirsiniz?", "Müqavilənin bağlanılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_FUND_CONTRACT_STATUS", "P_CONTRACT_ID", int.Parse(ContractID), "P_STATUS_ID", 14, "Açıq müqavilə bağlanmadı");
            LoadContractsDataGridView();
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void OpenBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            old_row_num = ContractsGridView.FocusedRowHandle;
            topindex = ContractsGridView.TopRowIndex;
            
            DialogResult dialogResult = XtraMessageBox.Show("Bağlanmış <color=red><b>" + ContractNumber + "</b></color> nömrəli müqaviləni açmaq istəyirsiniz?", "Müqavilənin açılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_FUND_CONTRACT_STATUS", "P_CONTRACT_ID", int.Parse(ContractID), "P_STATUS_ID", 13, "Bağlanmış müqavilə açılmadı");
            LoadContractsDataGridView();
            ContractsGridView.FocusedRowHandle = old_row_num;
            ContractsGridView.TopRowIndex = topindex;
        }

        private void FFundContract_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshFundsDataGridView();
        }

        private void ContractsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (ContractsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteBarButton.Enabled = GlobalVariables.DeleteFundContract;
                    EditBarButton.Enabled = GlobalVariables.EditFundContract;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = true;
            }
            else
                DeleteBarButton.Enabled = EditBarButton.Enabled = false;
        }
    }
}