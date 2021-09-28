using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using CRS.Forms.Total;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Customer
{
    public partial class FCommitments : DevExpress.XtraEditors.XtraForm
    {
        public FCommitments()
        {
            InitializeComponent();
        }
        public string ContractCode, ContractID;

        string credit_name;

        private void FCommitments_Load(object sender, EventArgs e)
        {
            SearchDockPanel.Hide();
            ToDateValue.Properties.MaxValue = DateTime.Today;
            this.Text = (String.IsNullOrWhiteSpace(ContractCode)) ? this.Text : ContractCode + " saylı lizinq müqaviləsi üzrə öhdəlik götürən şəxslərin siyahısı";
            GlobalProcedures.FillCheckedComboBox(CreditNameComboBox, "CREDIT_NAMES", "NAME,NAME_EN,NAME_RU", null);
        }

        private void LoadTreeList()
        {
            string s = null, contractID = (String.IsNullOrWhiteSpace(ContractID)) ? null : $@"AND CC.CONTRACT_ID = {ContractID}", date = null, power = (PowerOfAttorneyCheck.Checked) ? " AND PA.POWER_CODE IS NOT NULL" : null,
                   status = null;

            if (ActiveContractCheck.Checked && !ClosedContractCheck.Checked)
                status = " AND CON.STATUS_ID = 5";
            else if (!ActiveContractCheck.Checked && ClosedContractCheck.Checked)
                status = " AND CON.STATUS_ID = 6";
            else
                status = null;

            if (!String.IsNullOrWhiteSpace(FromDateValue.Text) && !String.IsNullOrWhiteSpace(ToDateValue.Text))
                date = $@" AND CC.AGREEMENTDATE BETWEEN TO_DATE('{FromDateValue.Text}', 'DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}', 'DD.MM.YYYY')";

            try
            {
                s = $@"SELECT DISTINCT CC.ID,
                         CC.PARENT_ID,
                         CC.COMMITMENT_NAME NAME,
                         CON.CONTRACT_CODE || ' - ' || CON.CREDIT_NAME CONTRACT_NAME,
                         TO_CHAR(CC.AGREEMENTDATE,'DD.MM.YYYY') AGREE_DATE,
                         CC.DEBT,
                         C.CODE,
                         TO_CHAR(CC.PERIOD_DATE,'DD.MM.YYYY'),
                         CC.INTEREST || ' %' INTEREST,
                         CC.ADVANCE_PAYMENT,
                         CC.SERVICE_AMOUNT,
                         S.STATUS_NAME,                         
                         CC.PERSON_TYPE_ID,
                         CON.CONTRACT_CODE,
                         CON.CREDIT_NAME
                    FROM CRS_USER.CONTRACT_ALL_COMMITMENTS CC,
                         CRS_USER.CURRENCY C,
                         CRS_USER.V_CONTRACTS CON,
                         CRS_USER.STATUS S,
                         CRS_USER.V_LAST_POWER_OF_ATTORNEY PA
                   WHERE CC.CURRENCY_ID = C.ID AND CON.STATUS_ID = S.ID AND CC.CONTRACT_ID = CON.CONTRACT_ID AND RTRIM(RTRIM(CC.COMMITMENT_NAME,'oğlu'),'qızı') = PA.FULLNAME_TRIM(+)
                        AND CC.CONTRACT_ID = PA.CONTRACT_ID(+) {contractID} {date} {power} {status} {credit_name}
                ORDER BY CON.CONTRACT_CODE, CC.ID";

                TreeList.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTreeList");
                TreeList.PopulateColumns();
                TreeList.KeyFieldName = "ID";
                //TreeList.ParentFieldName = "CUSTOMER_ID";
                TreeList.ParentFieldName = "PARENT_ID";
                TreeList.Columns[0].Caption = "Öhdəlik götürənin adı";
                TreeList.Columns[1].Caption = "Müqavilə";
                TreeList.Columns[2].Caption = "Razılaşmanın tarixi";
                TreeList.Columns[3].Caption = "Qalıq borc";
                TreeList.Columns[4].Caption = "Valyuta";
                TreeList.Columns[5].Caption = "Lizinq müddəti";
                TreeList.Columns[6].Caption = "Lizinq verənin mükafatı";
                TreeList.Columns[7].Caption = "Avans ödənişi";
                TreeList.Columns[8].Caption = "Xidmət haqqı";
                TreeList.Columns[9].Caption = "Müqavilənin statusu";
                TreeList.Columns[10].Visible = false;
                TreeList.Columns[11].Visible = false;
                TreeList.Columns[12].Visible = false;

                //TextAligment
                TreeList.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                TreeList.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                TreeList.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                TreeList.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                TreeList.Columns[5].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                TreeList.Columns[5].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                TreeList.Columns[6].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                TreeList.Columns[6].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                TreeList.Columns[3].Format.FormatType = FormatType.Custom;
                TreeList.Columns[3].Format.FormatString = "N2";
                TreeList.Columns[7].Format.FormatType = FormatType.Custom;
                TreeList.Columns[7].Format.FormatString = "N2";
                TreeList.Columns[8].Format.FormatType = FormatType.Custom;
                TreeList.Columns[8].Format.FormatString = "N2";

                TreeList.Columns[1].RowFooterSummary = SummaryItemType.Count;
                TreeList.Columns[1].RowFooterSummaryStrFormat = "{0}";

                TreeList.ExpandAll();
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    TreeList.BestFitColumns();
                }));
                //TreeList.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Öhdəlik götürənlərin siyahısı yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadTreeList();
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.TreeListExportToFile(TreeList, "xls");
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowTreeListPreview(TreeList);
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadTreeList();
        }

        private void CreditNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
                credit_name = " AND CON.CREDIT_NAME IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
            else
                credit_name = null;
            LoadTreeList();
        }

        private void PowerOfAttorneyCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
            LoadTreeList();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void TreeList_DoubleClick(object sender, EventArgs e)
        {
            TreeList tree = sender as TreeList;
            TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                object selNode = hi.Node.GetValue(tree.KeyFieldName);
                int CommitmentID = int.Parse(selNode.ToString());
                List<ContractCommitment> lstContractCommitment = CommitmentsDAL.SelectAllCommitmentByID(CommitmentID).ToList<ContractCommitment>();
                int personTypeID = lstContractCommitment.LastOrDefault().PERSON_TYPE_ID;
                if (personTypeID == 1)
                {
                    FCommitmentInfo fci = new FCommitmentInfo();
                    fci.CommitmentID = CommitmentID;
                    fci.ShowDialog();
                }
                else
                {
                    FJuridicalCommitmentInfo fjc = new FJuridicalCommitmentInfo();
                    fjc.CommitmentID = CommitmentID;
                    fjc.ShowDialog();
                }
            }
        }

    }
}