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
    public partial class FLawStatus : DevExpress.XtraEditors.XtraForm
    {
        public FLawStatus()
        {
            InitializeComponent();
        }
        string StatusID;

        public delegate void DoEvent();
        public event DoEvent RefreshLawStatusData;

        private void FLawStatus_Load(object sender, EventArgs e)
        {
            LoadLawStatusDataGridView();
        }

        private void LoadLawStatusDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE FROM CRS_USER.LAW_STATUS";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLawStatusDataGridView");

                LawStatusGridControl.DataSource = dt;
                LawStatusGridView.PopulateColumns();

                LawStatusGridView.Columns[0].Caption = "S/s";
                LawStatusGridView.Columns[1].Visible = false;
                LawStatusGridView.Columns[2].Caption = "Statusun adı";
                LawStatusGridView.Columns[3].Caption = "Qeyd";
                //TextAligment
                LawStatusGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                LawStatusGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                //HeaderAligment
                for (int i = 0; i < LawStatusGridView.Columns.Count; i++)
                {
                    LawStatusGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    LawStatusGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (LawStatusGridView.RowCount > 0)
                {
                    DeleteBarButton.Enabled = true;
                    EditBarButton.Enabled = true;
                    LawStatusGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else                
                    DeleteBarButton.Enabled = EditBarButton.Enabled = false;
                                    
                LawStatusGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite("Məhkəmələrin siyahısı yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FLawStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshLawStatusData();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawStatusDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.ShowGridPreview(LawStatusGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Class.GlobalProcedures.GridExportToFile(LawStatusGridControl, "xls");
        }

        private void LawStatusGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = LawStatusGridView.GetFocusedDataRow();
            if (row != null)
            {
                StatusID = row["ID"].ToString();
            }
        }

        void RefreshLawStatus()
        {
            LoadLawStatusDataGridView();
        }

        private void LoadLawStatus(string transaction, string statusid)
        {
            FLawStatusAddEdit flae = new FLawStatusAddEdit();
            flae.TransactionName = transaction;
            flae.StatusID = statusid;
            flae.RefreshLawStatusDataGridView += new FLawStatusAddEdit.DoEvent(RefreshLawStatus);
            flae.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawStatus("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawStatus("EDIT", StatusID);
        }

        private void DeleteLawStatus()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACT_LAWS WHERE lAW_STATUS_ID = {StatusID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş statusu silmək istəyirsiniz?", "Statusun silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.LAW_STATUS WHERE ID = {StatusID}", "Status silinmədi.", this.Name + "/DeleteLawStatus");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş status bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteLawStatus();
            LoadLawStatusDataGridView();
        }

        private void LawStatusGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(LawStatusGridView, PopupMenu, e);
        }
        
        private void LawStatusGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void LawStatusGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadLawStatus("EDIT", StatusID);
        }
    }
}