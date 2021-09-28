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
using CRS.Class.Tables;

namespace CRS.Forms.PaymentTask
{
    public partial class FAcceptors : DevExpress.XtraEditors.XtraForm
    {
        public FAcceptors()
        {
            InitializeComponent();
        }
        string AcceptorID;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshAcceptorDataGridView;

        private void FAcceptors_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
                NewBarButton.Enabled = GlobalVariables.AddAcceptor;

            LoadAcceptorGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadAcceptorGridView()
        {
            string sql = $@"SELECT 1 SS,
                                     ID,
                                     ACCEPTOR_NAME,
                                     ACCEPTOR_ACCOUNT,
                                     ACCEPTOR_VOEN,                                     
                                     USED_USER_ID
                                FROM CRS_USER.TASK_ACCEPTOR
                            ORDER BY ID";
            AcceptorGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);

            if (AcceptorGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditAcceptor;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteAcceptor;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAcceptorGridView();
        }

        private void FAcceptors_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshAcceptorDataGridView();
        }

        void RefreshAcceptor()
        {
            LoadAcceptorGridView();
        }

        private void LoadFAcceptorAddEdit(string transaction, string acceptor_id)
        {
            FAcceptorAddEdit fa = new FAcceptorAddEdit();
            fa.TransactionName = transaction;
            fa.AcceptorID = acceptor_id;
            fa.RefreshDataGridView += new FAcceptorAddEdit.DoEvent(RefreshAcceptor);
            fa.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAcceptorAddEdit("INSERT", null);
        }

        private void AcceptorGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AcceptorGridView.GetFocusedDataRow();
            if (row != null)
            {
                AcceptorID = row["ID"].ToString();
                UsedUserID = Convert.ToInt32(row["USED_USER_ID"].ToString());
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAcceptorAddEdit("EDIT", AcceptorID);
        }

        private void AcceptorGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFAcceptorAddEdit("EDIT", AcceptorID);
        }

        private void AcceptorGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(AcceptorGridView, e);
        }

        private void AcceptorGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AcceptorGridView, PopupMenu, e);
        }

        private void AcceptorGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if ((UsedUserID == -1) || (GlobalVariables.V_UserID == UsedUserID))
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAYMENT_TASKS WHERE ACCEPTOR_ID = {AcceptorID}");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş müştərinin məlumatlarını silmək istəyirsiniz?", "Alan tərəfin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.TASK_ACCEPTOR_BANKS WHERE ACCEPTOR_ID = {AcceptorID}",
                                                         $@"DELETE FROM CRS_USER.TASK_ACCEPTOR WHERE ID = {AcceptorID}",
                                                         "Seçilmiş ödənişi alan tərəfin məlumatları silinmədi.",
                                                         "FAcceptors - DeleteBarButton_ItemClick");
                }
                else
                    XtraMessageBox.Show("Seçilmiş müştəri ödəniş tapşırıqlarında istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş məlumatlar " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatlarını silmək olmaz.", "Seçilmiş ödənişi alan tərəfin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadAcceptorGridView();
        }
    }
}