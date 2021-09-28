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
    public partial class FTaskType : DevExpress.XtraEditors.XtraForm
    {
        public FTaskType()
        {
            InitializeComponent();
        }
        string TypeID;

        public delegate void DoEvent();
        public event DoEvent RefreshTypeDataGridView;

        private void FTaskType_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddTaskType;

            LoadTypeDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FTaskType_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshTypeDataGridView();
        }

        private void LoadTypeDataGridView()
        {
            string sql = "SELECT 1 SS,ID,TYPE_NAME,CODE,USED_USER_ID FROM CRS_USER.TASK_TYPE ORDER BY ORDER_ID";
            TypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);

            if (TypeGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditTaskType;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteTaskType;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = true;
            }
            else
                DeleteBarButton.Enabled =
                    EditBarButton.Enabled =
                    UpBarButton.Enabled =
                    DownBarButton.Enabled = false;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadTypeDataGridView();
        }

        private void TypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                TypeID = row["ID"].ToString();
                UpBarButton.Enabled = !(TypeGridView.FocusedRowHandle == 0);
                DownBarButton.Enabled = !(TypeGridView.FocusedRowHandle == TypeGridView.RowCount - 1);
            }
        }

        void RefreshType()
        {
            LoadTypeDataGridView();
        }

        private void LoadFTaskTypeAddEdit(string transaction, string type_id)
        {
            FTaskTypeAddEdit ft = new FTaskTypeAddEdit();
            ft.TransactionName = transaction;
            ft.TypeID = type_id;
            ft.RefreshDataGridView += new FTaskTypeAddEdit.DoEvent(RefreshType);
            ft.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTaskTypeAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTaskTypeAddEdit("EDIT", TypeID);
        }

        private void TypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFTaskTypeAddEdit("EDIT", TypeID);
        }

        private void TypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(TypeGridView, e);
        }

        private void TypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TypeGridView, PopupMenu, e);
        }

        private void UpBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TASK_TYPE", TypeID, "up", out orderid);
            LoadTypeDataGridView();
            TypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TASK_TYPE", TypeID, "down", out orderid);
            LoadTypeDataGridView();
            TypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteType()
        {
            int UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.TASK_TYPE WHERE ID = " + TypeID);
            if (UsedUserID <= 0)
            {
                int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.PAYMENT_TASKS WHERE TYPE_ID = " + TypeID);
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş tapşırıq növünü silmək istəyirsiniz?", "Tapşırıq növünün silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.TASK_TYPE WHERE ID = " + TypeID, "Tapşırığın növü silinmədi.");
                    }
                    LoadTypeDataGridView();
                }
                else
                    XtraMessageBox.Show("Seçilmiş tapşırıq növü bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş tapşırığın növü hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş tapşırıq növünün hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteType();
        }

        private void TypeGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
    }
}