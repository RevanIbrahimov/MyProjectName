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
    public partial class FChangeCode : DevExpress.XtraEditors.XtraForm
    {
        public FChangeCode()
        {
            InitializeComponent();
        }
        public string TypeName, TypeCode;
        public int TypeID;
        public DateTime TaskDate = DateTime.Today;

        bool b = false;        

        public delegate void DoEvent(string code, int task_number, bool close);
        public event DoEvent RefreshCode;

        private void FChangeCode_Load(object sender, EventArgs e)
        {
            TypeNameText.Text = TypeName;
            int max_task_number = GlobalFunctions.GetID($@"SELECT NVL(MAX(TASK_NUMBER),0) FROM CRS_USER.PAYMENT_TASKS WHERE TYPE_ID = {TypeID}");
            DescriptionLabel.Text = "Ən son daxil edilmiş qeydiyyat nömrəsi: " + max_task_number;
            DescriptionLabel.Visible = (max_task_number > 0);
            NumberValue.Properties.MinValue = max_task_number + 1;
            LoadOperationsDataGridView();
            NumberValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void LoadOperationsDataGridView()
        {
            string s = null;
            try
            {
                s = $@"SELECT 1 SS,CU.USER_FULLNAME,CC.CODE FROM CRS_USER_TEMP.TASK_CODE_TEMP CC,CRS_USER.V_USERS CU WHERE CC.TASK_TYPE_ID = {TypeID} AND CC.USED_USER_ID = CU.ID";

                CodeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Nömrələr cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            b = false;
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            b = (GlobalFunctions.ExecuteQuery($@"UPDATE CRS_USER_TEMP.TASK_CODE_TEMP SET TASK_NUMBER = {NumberValue.Value}, CODE = '{CodeLabel.Text}' WHERE TASK_TYPE_ID = {TypeID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                 "Nömrəs dəyişdirilmədi.") > 0);

            this.Close();
        }

        private void NumberValue_EditValueChanged(object sender, EventArgs e)
        {
            CodeLabel.Text = TypeCode + NumberValue.Value + "-" + TaskDate.Year.ToString().Substring(2, 2);
        }

        private void FChangeCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshCode(CodeLabel.Text, (int)NumberValue.Value, b);
        }

        private void CodeGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
    }
}