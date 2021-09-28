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
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using CRS.Class.Tables;
using DevExpress.XtraGrid.Columns;

namespace CRS.Forms.Contracts
{
    public partial class FInsuranceTransfer : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceTransfer()
        {
            InitializeComponent();
        }
        decimal sumTransfer = 0;
        public string ListID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        List<InsuranceTransfer> lstTransfer = new List<InsuranceTransfer>();

        private void FInsuranceTransfer_Load(object sender, EventArgs e)
        {
            SearchDockPanel.Hide();
            LoadInsurance();
        }

        private void LoadInsurance()
        {
            string status = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 6";
            else if (!ClosedCheck.Checked && ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            string s = $@"SELECT I.ID,
                                 I.CONTRACT_ID,
                                 C.CONTRACT_CODE,
                                 IC.NAME COMPANY_NAME,
                                 I.INSURANCE_AMOUNT,
                                 I.INSURANCE_PERIOD ,
                                 I.INSURANCE_INTEREST,
                                 I.AMOUNT INSURANCE_COST,
                                 NVL(SP.PAYED_AMOUNT, 0) PAYED_AMOUNT,
                                 NVL(ST.TRANSFER_AMOUNT, 0) SUM_TRANSFER,
                                 NVL(SP.PAYED_AMOUNT, 0) - NVL(ST.TRANSFER_AMOUNT, 0) - NVL(ST.COMPENSATION, 0) TRANSFER_AMOUNT,
                                 NVL(ST.COMPENSATION, 0) COMPENSATION,
                                 NULL NOTE,
                                 C.STATUS_ID,
                                 SUBSTR(H.HOSTAGE,0,7) CAR_NUMBER,
                                 I.POLICE
                            FROM CRS_USER.INSURANCES I,
                                 CRS_USER.V_CONTRACTS C,
                                 CRS_USER.INSURANCE_COMPANY IC,
                                 CRS_USER.V_SUM_INSURANCE_PAYMENT SP,
                                 CRS_USER.V_SUM_INSURANCE_TRANSFER ST,
                                 CRS_USER.V_HOSTAGE H
                           WHERE     I.CONTRACT_ID = C.CONTRACT_ID
                                 AND I.COMPANY_ID = IC.ID
                                 AND C.CONTRACT_ID = H.CONTRACT_ID
                                 AND I.ID = SP.INSURANCE_ID(+)
                                 AND I.ID = ST.INSURANCE_ID(+)
                                 {(ListID == null? "AND (ROUND(I.INSURANCE_AMOUNT * I.INSURANCE_INTEREST / 100, 2) - NVL(ST.TRANSFER_AMOUNT, 0) - NVL(ST.COMPENSATION, 0)) > 0" : "")}{status}
                        ORDER BY C.CONTRACT_CODE";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsurance", "Sığortaların siyahısı yüklənmədi.");
            InsuranceGridControl.DataSource = dt;

            if (ListID != null)
                InsuranceGridView.ActiveFilter.Add(InsuranceGridView.Columns["ID"],
                    new ColumnFilterInfo($@"[ID] IN ({ListID})", $@"Sığorta in ({ListID})"));

            BOK.Visible = dt.Rows.Count > 0;

            GenerateSumSelectedRowsValue();
            GenerateCountSelectedRows();
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Insurance_SS, e);
        }

        private void InsuranceGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Insurance_SS, "Center", e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsurance();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, PopupMenu, e);
        }

        private void GenerateSumSelectedRowsValue()
        {
            sumTransfer = GlobalFunctions.SumSelectedRow(InsuranceGridView, "TRANSFER_AMOUNT");
            SumSelectedRowsBarStaticItem.Caption = "<color=104,0,0>Seçilmiş köçürmələrin cəmi:</color><b> " + sumTransfer.ToString("n2") + "</b>";
        }

        private void GenerateCountSelectedRows()
        {
            CountSelectedRowsBarStaticItem.Caption = "<color=104,0,0>Seçilmiş sətirlərin sayı:</color><b> " + GlobalFunctions.CountSelectedRow(InsuranceGridView).ToString("n0") + "</b>";
        }

        private void InsuranceGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            GenerateSumSelectedRowsValue();
            GenerateCountSelectedRows();
        }

        private void InsuranceGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GenerateSumSelectedRowsValue();
            GridView view = sender as GridView;
            decimal cost = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, view.Columns["INSURANCE_COST"])),
                    compensation = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, view.Columns["COMPENSATION"])),
                    sumTransfer = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, view.Columns["SUM_TRANSFER"])),
                    transfer = (e.Column == Insurance_TransferAmount) ? Convert.ToDecimal(e.Value) : 0;

            if (transfer > (cost - sumTransfer - compensation) && e.Column == Insurance_TransferAmount)
                GlobalProcedures.ShowErrorMessage(view.GetRowCellValue(e.RowHandle, view.Columns["CONTRACT_CODE"]) + " müqaviləsi üzrə köçürülən məbləğ ən çoxu " + (cost - sumTransfer - compensation).ToString("n2") + " AZN ola bilər");

        }

        private void Transfer(int taskID, DateTime taskDate)
        {
            string sql = null;

            for (int i = 0; i < lstTransfer.Count; i++)
            {
                int insuranceID = lstTransfer[i].insuranceID;
                decimal transfer = lstTransfer[i].amount;
                decimal compensation = lstTransfer[i].compensation;
                string note = lstTransfer[i].note;

                sql = $@"INSERT INTO CRS_USER.INSURANCE_TRANSFER(INSURANCE_ID,TRANSFER_DATE,TRANSFER_AMOUNT,COMPENSATION,PAYMENT_TASK_ID,NOTE,INSERT_USER)
                        VALUES({insuranceID},TO_DATE('{taskDate.ToString("dd.MM.yyyy")}','DD.MM.YYYY'), {transfer.ToString(GlobalVariables.V_CultureInfoEN)},{compensation.ToString(GlobalVariables.V_CultureInfoEN)},{taskID},'{note}',{GlobalVariables.V_UserID})";

                GlobalProcedures.ExecuteQuery(sql, "Köçürmə cədvələ daxil edilmədi.", this.Name + "/RefreshTask");
            }

            this.Close();
        }

        void RefreshTask(int taskID, DateTime taskDate, bool clickOK)
        {
            if (!clickOK)
                return;

            Transfer(taskID, taskDate);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < InsuranceGridView.SelectedRowsCount; i++)
            {
                int rowHandle = InsuranceGridView.GetSelectedRows()[i];
                if (!InsuranceGridView.IsGroupRow(rowHandle))
                    rows.Add(InsuranceGridView.GetDataRow(rowHandle));
            }

            if (rows.Count == 0)
            {
                GlobalProcedures.ShowWarningMessage("Sığortasını köçürmək istədiyiniz müqavilələri seçin.");
                return;
            }

            bool c = true;
            string police = null;
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                decimal cost = Convert.ToDecimal(row["INSURANCE_COST"]),
                    compensation = Convert.ToDecimal(row["COMPENSATION"]),
                    sumTransfer = Convert.ToDecimal(row["SUM_TRANSFER"]),
                    transfer = Convert.ToDecimal(row["TRANSFER_AMOUNT"]);
                int insuranceID = Convert.ToInt32(row["ID"]);
                string note = row["NOTE"].ToString();
                police = police + row["POLICE"].ToString() + ", ";

                if (transfer + compensation + sumTransfer > cost)
                {
                    c = false;
                    break;
                }

                lstTransfer.Add(new InsuranceTransfer { insuranceID = insuranceID, amount = transfer, compensation = 0, note = note });
            }

            police = police.Trim().TrimEnd(',');

            if (!c)
            {
                GlobalProcedures.ShowWarningMessage("Hesabı mənfi (-) olan sığortaları köçürmək olmaz.");
                return;
            }

            if (sumTransfer == 0)
            {
                GlobalProcedures.ShowWarningMessage("Köçürmək istədiyiniz cəmi məbləğ 0 (sıfır) - dan böyük olmalıdır.");
                return;
            }

            if (!PaymentTaskCheck.Checked)
            {
                Transfer(0, DateTime.Today);
                return;                
            }

            PaymentTask.FTaskAddEdit ft = new PaymentTask.FTaskAddEdit();
            ft.TransactionName = "INSERT";
            ft.Amount = sumTransfer;
            ft.TaskType = "Sığorta";
            ft.Reason = police;
            ft.RefreshDataGridView += new PaymentTask.FTaskAddEdit.DoEvent(RefreshTask);
            ft.ShowDialog();
        }

        private void FInsuranceTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void ActiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadInsurance();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void InsuranceGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForClose(6, InsuranceGridView, e);
        }
    }
}