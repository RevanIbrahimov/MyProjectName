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

namespace CRS.Forms.Contracts
{
    public partial class FSeller : DevExpress.XtraEditors.XtraForm
    {
        public FSeller()
        {
            InitializeComponent();
        }

        private void FSeller_Load(object sender, EventArgs e)
        {
            LoadSellerDataGridView();
        }

        private void LoadSellerDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 C.CONTRACT_CODE,
                                 S.FULLNAME SELLER_NAME,
                                 H.HOSTAGE,
                                 LIQUID_AMOUNT,
                                 FIRST_PAYMENT,
                                 H.CURRENCY_CODE,
                                 C.SELLER_ID,
                                 C.SELLER_TYPE_ID,
                                 C.CREDIT_TYPE_ID,
                                 SC.SELLER_CARD,
                                 SC.REGISTRATION_ADDRESS,
                                 SC.ADDRESS,
                                 P.PHONE
                            FROM CRS_USER.V_HOSTAGE H,
                                 CRS_USER.V_CONTRACTS C,
                                 CRS_USER.V_SELLERS S,
                                 CRS_USER.V_SELLER_CARDS SC,
                                 (SELECT *
                                    FROM CRS_USER.V_PHONE
                                   WHERE OWNER_TYPE IN ('S', 'JP')) P
                           WHERE     H.CONTRACT_ID = C.CONTRACT_ID
                                 AND C.SELLER_ID = S.ID
                                 AND C.SELLER_TYPE_ID = S.PERSON_TYPE_ID
                                 AND S.ID = SC.SELLER_ID
                                 AND S.PERSON_TYPE_ID = SC.PERSON_TYPE_ID
                                 AND S.ID = P.OWNER_ID(+)
                        ORDER BY C.CONTRACT_CODE DESC ";
            DataTable dt = GlobalFunctions.GenerateDataTable(s);
            SellerGridControl.DataSource = dt;
        }        

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadSellerDataGridView();
        }

        private void SellerGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void SellerGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void SellerGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SellerGridControl, "xls");
        }

        private void SellerGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(SellerGridView, PopupMenu, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(SellerGridControl);
        }
    }
}