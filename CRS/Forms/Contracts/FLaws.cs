using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Contracts
{
    public partial class FLaws : DevExpress.XtraEditors.XtraForm
    {
        public FLaws()
        {
            InitializeComponent();
        }
        string LawID;

        public delegate void DoEvent();
        public event DoEvent RefreshLawsData;

        private void FLaws_Load(object sender, EventArgs e)
        {
            LoadLawsDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FLaws_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshLawsData();
        }

        void RefreshLaws()
        {
            LoadLawsDataGridView();
        }

        private void LoadLaw(string transaction, string lawid)
        {
            FLawAddEdit flae = new FLawAddEdit();
            flae.TransactionName = transaction;
            flae.LawID = lawid;
            flae.RefreshLawsDataGridView += new FLawAddEdit.DoEvent(RefreshLaws);
            flae.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLaw("INSERT", null);
        }

        private void LoadLawsDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE FROM CRS_USER.LAWS";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                LawsGridControl.DataSource = dt;
                LawsGridView.PopulateColumns();

                LawsGridView.Columns[0].Caption = "S/s";
                LawsGridView.Columns[1].Visible = false;
                LawsGridView.Columns[2].Caption = "Məhkəmənin adı";
                LawsGridView.Columns[3].Caption = "Qeyd";
                //TextAligment
                LawsGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                LawsGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                //HeaderAligment
                for (int i = 0; i < LawsGridView.Columns.Count; i++)
                {
                    LawsGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    LawsGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (LawsGridView.RowCount > 0)
                {
                    DeleteBarButton.Enabled = true;
                    EditBarButton.Enabled = true;
                    LawsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    DeleteBarButton.Enabled = false;
                    EditBarButton.Enabled = false;
                }
                LawsGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məhkəmələrin siyahısı yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLaw("EDIT", LawID);
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

        private void LawsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = LawsGridView.GetFocusedDataRow();
            if (row != null)
            {
                LawID = row["ID"].ToString();
            }
        }

        private void DeleteLaw()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_LAWS WHERE lAW_ID = " + LawID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məhkəməni silmək istəyirsiniz?", "Məhkəmənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.LAWS WHERE ID = " + LawID, "Məhkəmə silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş məhkəmə bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteLaw();
            LoadLawsDataGridView();
        }

        private void LawsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(LawsGridView, PopupMenu, e);
        }

        private void LawsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void LawsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void LawsGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadLaw("EDIT", LawID);
        }
    }
}