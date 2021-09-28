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

namespace CRS.Forms.Bookkeeping
{
    public partial class FJurnalChangeDate : DevExpress.XtraEditors.XtraForm
    {
        public FJurnalChangeDate()
        {
            InitializeComponent();
        }
        public string OperationID, Date;

        public delegate void DoEvent();
        public event DoEvent RefreshOperationsDataGridView;

        private void FJurnalChangeDate_Load(object sender, EventArgs e)
        {
            OperationDate.DateTime = GlobalFunctions.ChangeStringToDate(Date, "ddmmyyyy");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            Update();
            this.Close();
        }

        private void FJurnalChangeDate_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshOperationsDataGridView();
        }

        private void Update()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.OPERATION_JOURNAL SET OPERATION_DATE = TO_DATE('{OperationDate.Text}','DD/MM/YYYY'),                                                            
                                                            YR_MNTH_DY = {GlobalFunctions.ConvertDateToInteger(OperationDate.Text, "ddmmyyyy")} 
                                                WHERE ID = {OperationID}", "Əməliyyatın tarixi dəyişdirilmədi.", this.Name + "/Update");
        }
    }
}