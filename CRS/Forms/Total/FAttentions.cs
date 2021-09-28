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
using CRS.Class.DataAccess;

namespace CRS.Forms.Total
{
    public partial class FAttentions : DevExpress.XtraEditors.XtraForm
    {
        public FAttentions()
        {
            InitializeComponent();
        }
        public string ContractCode, CustomerName, ContractID;

        string NoteID;
        int InsertUserID;        
        bool CurrentStatus = false;

        private void FAttentions_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
            List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();
            if (GlobalVariables.V_UserID != contract.USED_USER_ID)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == contract.USED_USER_ID).FULLNAME;
                XtraMessageBox.Show(ContractCode + " saylı lizinq müqaviləsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun üçün qeyd yazıla bilməz. Siz yalnız qeydlərə baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CurrentStatus = true;
            }
            else
                CurrentStatus = false;

            CustomerNameText.Text = CustomerName;
            ContractCodeText.Text = ContractCode;
            LoadAttentionDataGridView();
            ComponentEnable(CurrentStatus);
        }

        private void LoadAttentionDataGridView()
        {
            string sql = $@"SELECT 1 SS,
                                   PN.ID,
                                   PN.NOTE,
                                   U.USER_FULLNAME,
                                   PN.INSERT_DATE,
                                   PN.CHANGE_DATE,
                                   PN.INSERT_USER_ID
                              FROM CRS_USER.PAYMENT_NOTES PN, CRS_USER.V_USERS U
                             WHERE PN.INSERT_USER_ID = U.ID AND PN.CONTRACT_ID = {ContractID}
                            ORDER BY PN.ID ";

            AttentionGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractDataGridView");

            EditBarButton.Enabled = DeleteBarButton.Enabled = (AttentionGridView.RowCount > 0);
        }

        public void ComponentEnable(bool status)
        {
            if (GlobalVariables.V_UserID > 0 && !status)
            {
                NewBarButton.Enabled = EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else if (status)
                NewBarButton.Enabled = EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAttentionDataGridView();
        }

        private void AttentionGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void AttentionGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(AttentionGridControl);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAttentionAddEdit("INSERT", null);
        }

        void RefresfAttentions()
        {
            LoadAttentionDataGridView();
        }

        private void LoadFAttentionAddEdit(string transaction, string noteID)
        {
            FAttentionAddEdit fa = new FAttentionAddEdit();
            fa.TransactionName = transaction;
            fa.ContractID = ContractID;
            fa.NoteID = noteID;
            fa.RefreshAttentionDataGridView += new FAttentionAddEdit.DoEvent(RefresfAttentions);
            fa.ShowDialog();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAttentionAddEdit("EDIT", NoteID);
        }

        private void AttentionGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFAttentionAddEdit("EDIT", NoteID);
        }

        private void AttentionGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AttentionGridView.GetFocusedDataRow();
            if (row != null)
            {
                NoteID = row["ID"].ToString();
                InsertUserID = Convert.ToInt32(row["INSERT_USER_ID"].ToString());
                EditBarButton.Enabled = DeleteBarButton.Enabled = (InsertUserID == GlobalVariables.V_UserID);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qeydi silmək istəyirsiniz?", "Qeydin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.PAYMENT_NOTES WHERE ID = " + NoteID, "Qeyd silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
                LoadAttentionDataGridView();
            }
        }

        private void FAttentions_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AttentionGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AttentionGridView, PopupMenu, e);
        }
    }
}