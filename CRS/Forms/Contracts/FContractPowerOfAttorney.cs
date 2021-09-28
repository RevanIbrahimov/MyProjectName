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
    public partial class FContractPowerOfAttorney : DevExpress.XtraEditors.XtraForm
    {
        public FContractPowerOfAttorney()
        {
            InitializeComponent();
        }
        public int ContractID;
        public string ContractCode;

        private void FContractPowerOfAttorney_Load(object sender, EventArgs e)
        {
            this.Text = ContractCode + " " + this.Text;

            string s = $@"SELECT 1 SS,
                                   PA.ID,
                                   PA.POWER_CODE,
                                   PA.FULLNAME||' ('||CS.SERIES || ' ' || PA.CARD_NUMBER||')' FULLNAME,
                                   PA.INSERT_DATE,
                                   PA.POWER_DATE, 
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = PA.INSERT_USER)
                                      INSERT_USER,
                                   PA.FULLNAME_CHECK,
                                   PA.IS_REVOKE
                              FROM CRS_USER.POWER_OF_ATTORNEY PA, CRS_USER.CARD_SERIES CS
                             WHERE   PA.CARD_SERIES_ID = CS.ID
                                   AND CONTRACT_ID = {ContractID}
                            ORDER BY PA.ID";

            PowerGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/FContractPowerOfAttorney_Load");
        }

        private void PowerGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_REVOKE")) == 0)
                e.Value = Properties.Resources.ok_16;
            else
                e.Value = Properties.Resources.cancel_16;
        }

        private void PowerGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
    }
}