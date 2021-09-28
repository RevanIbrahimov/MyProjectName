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
    public partial class FNoPowerOfAttorneyList : DevExpress.XtraEditors.XtraForm
    {
        public FNoPowerOfAttorneyList()
        {
            InitializeComponent();
        }
        int contractID;
        string contractCode;

        private void FNoPowerOfAttorneyList_Load(object sender, EventArgs e)
        {
            LoadPowerOfAttorneyList();
        }

        private void LoadPowerOfAttorneyList()
        {
            string sql = $@"SELECT 1 SS,
                                 C.CONTRACT_CODE,
                                 PA.FULLNAME || ' (' || CS.SERIES || ' ' || PA.CARD_NUMBER || ')'
                                    FULLNAME,
                                 PA.POWER_DATE,
                                 H.HOSTAGE,
                                 C.CONTRACT_ID
                            FROM CRS_USER.POWER_OF_ATTORNEY PA,
                                 CRS_USER.V_CONTRACTS C,
                                 CRS_USER.V_HOSTAGE H,
                                 CRS_USER.CARD_SERIES CS
                           WHERE     (PA.ID, PA.CONTRACT_ID, PA.POWER_DATE) IN (  SELECT MAX (ID),
                                                                                         CONTRACT_ID,
                                                                                         MAX (POWER_DATE)
                                                                                    FROM CRS_USER.POWER_OF_ATTORNEY
                                                                                GROUP BY CONTRACT_ID)
                                 AND POWER_DATE < TRUNC (SYSDATE)
                                 AND PA.CONTRACT_ID = C.CONTRACT_ID
                                 AND C.CONTRACT_ID = H.CONTRACT_ID
                                 AND PA.CARD_SERIES_ID = CS.ID
                                 AND C.STATUS_ID = 5
                        ORDER BY C.CONTRACT_CODE DESC, PA.POWER_DATE DESC";

            PowerGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPowerOfAttorneyList", "Etibarnamələr yüklənmədi.");

            DetailsBarButton.Enabled = (PowerGridView.RowCount > 0);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerOfAttorneyList();
        }

        private void PowerGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PowerGridControl);
        }

        private void PowerGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PowerGridView, PopupMenu, e);
        }

        private void PowerGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ExcelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PowerGridControl, "xls");
        }

        private void PowerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PowerGridView.GetFocusedDataRow();
            if (row != null)
            {
                contractID = Convert.ToInt32(row["CONTRACT_ID"].ToString());
                contractCode = row["CONTRACT_CODE"].ToString();
            }
        }

        private void DetailsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractPowerOfAttorney();
        }

        private void LoadContractPowerOfAttorney()
        {
            FContractPowerOfAttorney fc = new FContractPowerOfAttorney();
            fc.ContractID = contractID;
            fc.ContractCode = contractCode;
            fc.ShowDialog();
        }

        private void PowerGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DetailsBarButton.Enabled = (PowerGridView.RowCount > 0);
        }

        private void PowerGridView_DoubleClick(object sender, EventArgs e)
        {
            if (DetailsBarButton.Enabled)
                LoadContractPowerOfAttorney();
        }
    }
}