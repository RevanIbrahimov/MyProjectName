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

namespace CRS.Forms.Contracts
{
    public partial class FFinishedContracts : DevExpress.XtraEditors.XtraForm
    {
        public FFinishedContracts()
        {
            InitializeComponent();
        }

        private void FFinishedContracts_Load(object sender, EventArgs e)
        {
            LoadContractDataGridView();
        }

        private void LoadContractDataGridView()
        {
            string sql = $@"SELECT 1 SS,
                                   C.CONTRACT_CODE,
                                   CUS.CUSTOMER_NAME,
                                   COM.COMMITMENT_NAME,
                                   C.AMOUNT,
                                   C.CURRENCY_CODE,
                                   C.INTEREST,
                                   C.PERIOD,
                                   C.START_DATE,
                                   C.END_DATE,
                                   P.PAYMENT_DATE CLOSED_DATE
                              FROM V_CUSTOMER_LAST_PAYMENT P,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.V_COMMITMENTS COM,
                                   CRS_USER.V_CUSTOMERS CUS
                             WHERE     P.CONTRACT_ID = C.CONTRACT_ID
                                   AND C.CONTRACT_ID = COM.CONTRACT_ID(+)
                                   AND C.CUSTOMER_ID = CUS.ID
                                   AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND C.STATUS_ID = 6";

            ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }

        private void ContractGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ContractGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractGridControl);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractDataGridView();
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractGridControl, "xls");
        }
    }
}