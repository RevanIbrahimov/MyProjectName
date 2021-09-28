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

namespace CRS.Forms.Dictionaries
{
    public partial class FBanks : DevExpress.XtraEditors.XtraForm
    {
        public FBanks()
        {
            InitializeComponent();
        }
        int BankID;

        public delegate void DoEvent(int bankID);
        public event DoEvent GetBanksID;

        private void FBanks_Load(object sender, EventArgs e)
        {
            LoadBanksDataGridView();
        }

        private void LoadBanksDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 B.ID,
                                 B.LONG_NAME,
                                 B.CODE,
                                 B.SHORT_NAME,
                                 B.SWIFT,
                                 B.VOEN,
                                 B.CBAR_ACCOUNT,
                                 S.STATUS_NAME,
                                 B.USED_USER_ID,
                                 B.STATUS_ID,
                                 B.IS_USED
                            FROM CRS_USER.BANKS B, CRS_USER.STATUS S
                           WHERE B.STATUS_ID = S.ID
                        ORDER BY ORDER_ID";

            BanksGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);

            BOK.Visible = (BanksGridView.RowCount > 0);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BanksGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            this.GetBanksID(BankID);
            this.Close();
        }

        private void BanksGridView_DoubleClick(object sender, EventArgs e)
        {
            if (BOK.Visible)
                BOK_Click(sender, EventArgs.Empty);
        }

        private void BanksGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BanksGridView.GetFocusedDataRow();
            if (row != null)
                BankID = Convert.ToInt32(row["ID"]);
        }

        private void BanksGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForClose(8, BanksGridView, e);
        }
    }
}