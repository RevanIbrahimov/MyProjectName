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

namespace CRS.Forms.PaymentTask
{
    public partial class FTemplates : DevExpress.XtraEditors.XtraForm
    {
        public FTemplates()
        {
            InitializeComponent();
        }
        string TemplateID;

        int old_row_num = 0, topindex;

        public delegate void DoEvent();
        public event DoEvent RefreshData;

        private void FTemplates_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddTemplate;

            RefreshTemplates();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TemplateGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TemplateGridView, PopupMenu, e);
        }

        private void LoadTemplateGridView()
        {
            string sql = $@"SELECT 1 SS,
                                     PT.ID TEMPLATE_ID,
                                     PT.TEMPLATE_NAME,
                                     TT.TYPE_NAME,
                                     B.LONG_NAME PAYING_BANK,
                                     AB.ACCEPTOR_NAME,
                                     AB.ACCEPTOR_BANK_NAME ACCEPTOR_BANK,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = PT.INSERT_USER)
                                        INSERT_USER,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = PT.UPDATE_USER)
                                        UPDATE_USER,
                                     PT.INSERT_DATE,
                                     PT.USED_USER_ID
                                FROM CRS_USER.PAYMENT_TASK_TEMPLATES PT,
                                     CRS_USER.TASK_TYPE TT,
                                     CRS_USER.V_ACCEPTOR_BANK AB,
                                     CRS_USER.BANKS B
                               WHERE     PT.TYPE_ID = TT.ID
                                     AND PT.PAYING_BANK_ID = B.ID
                                     AND PT.ACCEPTOR_ID = AB.ACCEPTOR_ID
                                     AND PT.ACCEPTOR_BANK_ID = AB.ACCEPTOR_BANK_ID
                            ORDER BY TT.ORDER_ID, PT.ID";

            TemplateGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);
            EditBarButton.Enabled = DeleteBarButton.Enabled = (TemplateGridView.RowCount > 0);

            if (TemplateGridView.RowCount > 0)
            {
                EditBarButton.Enabled = GlobalVariables.EditTemplate;
                DeleteBarButton.Enabled = GlobalVariables.DeleteTemplate;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = true;
        }

        void RefreshTemplates()
        {
            LoadTemplateGridView();
        }

        private void LoadFTemplateAddEdit(string transaction, string template_id)
        {
            FTemplateAddEdit ft = new FTemplateAddEdit();
            ft.TransactionName = transaction;
            ft.TemplateID = template_id;
            ft.RefreshDataGridView += new FTemplateAddEdit.DoEvent(RefreshTemplates);
            ft.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTemplateAddEdit("INSERT", null);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshTemplates();
        }

        private void TemplateGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void TemplateGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TemplateGridView.GetFocusedDataRow();
            if (row != null)
                TemplateID = row["TEMPLATE_ID"].ToString();
        }

        private void FTemplates_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshData();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTemplateAddEdit("EDIT", TemplateID);
        }

        private void TemplateGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFTemplateAddEdit("EDIT", TemplateID);
        }

        private void TemplateGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(TemplateGridView, e);
        }

        private void TemplateGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            old_row_num = TemplateGridView.FocusedRowHandle;
            topindex = TemplateGridView.TopRowIndex;
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.PAYMENT_TASK_TEMPLATES WHERE ID = {TemplateID}");
            if ((UsedUserID == -1) || (GlobalVariables.V_UserID == UsedUserID))
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş şəblon formanı silmək istəyirsiniz?", "Şablon formanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.PAYMENT_TASK_TEMPLATES WHERE ID = {TemplateID}", "Seçilmiş şablon forma silinmədi.");
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş şablon " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatlarını silmək olmaz.", "Seçilmiş şablon formanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadTemplateGridView();
            TemplateGridView.FocusedRowHandle = old_row_num;
            TemplateGridView.TopRowIndex = topindex;
        }
    }
}