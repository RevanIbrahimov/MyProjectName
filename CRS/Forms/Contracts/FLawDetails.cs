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
using CRS.Class;
using CRS.Class.Views;
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.Contracts
{
    public partial class FLawDetails : DevExpress.XtraEditors.XtraForm
    {
        public FLawDetails()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractCode, ContractStartDate, ContractID, CustomerName;
        int UsedUserID, is_active;
        string DetailID, NextDate, commitment = null, customerfullname = null, ParentID, applicant;
        bool Used = false, CurrentStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshLawsDataGridView;

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLawDetailsDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(DetailsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DetailsGridControl, "xls");
        }

        void RefreshDetails()
        {
            LoadLawDetailsDataGridView();
        }

        private void LoadLawDetails(string transaction, string detailid)
        {
            List<ContractLawsView> lstLaw = ContractLawsViewDAL.SelectContractLaw(Convert.ToInt32(ParentID)).ToList<ContractLawsView>();
            if (lstLaw.Count == 0)
                return;

            var law = lstLaw.First();

            string startdate = law.StartDate.ToString("dd.MM.yyyy"), 
                   customername = law.DefandantName,
                   judge_name = law.JudgeName;
            int lawid = law.LawID; 

            FLawDetailAddEdit fldae = new FLawDetailAddEdit();
            fldae.TransactionName = transaction;
            fldae.ContractID = ContractID;
            fldae.DetailID = detailid;
            fldae.ParentID = ParentID;
            fldae.LawID = lawid;
            fldae.CustomerName = CustomerName;
            fldae.JudgeName = judge_name;
            fldae.Applicant = applicant;
            fldae.DefandantName = customername;
            fldae.StartDate = startdate;
            fldae.RefreshDetailsDataGridView += new FLawDetailAddEdit.DoEvent(RefreshDetails);
            fldae.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled && is_active == 1)
                LoadLawDetails("INSERT", null);
        }

        private void DetailsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled)
                GlobalProcedures.GridMouseUpForPopupMenu(DetailsGridView, PopupMenu, e);
        }

        private void DetailsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled && is_active == 1)
            {
                if (!String.IsNullOrWhiteSpace(NextDate))
                    LoadLawDetails("EDIT", DetailID);
                else
                    LoadProsecute("EDIT", DetailID);
            }
        }

        private void DetailsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = DetailsGridView.GetFocusedDataRow();
            if (row != null)
            {
                DetailID = row["ID"].ToString();
                ParentID = row["PARENT_ID"].ToString();
                NextDate = row["NEXT_DATE"].ToString();
                applicant = row["APPLICANT"].ToString();
                is_active = Convert.ToInt32(row["IS_ACTIVE"].ToString());
                int parent_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_LAWS WHERE PARENT_ID = " + ParentID);
                if (ParentID == DetailID)
                {
                    EditBarButton.Enabled = DeleteBarButton.Enabled = !(parent_count > 0);
                }
                else
                {
                    EditBarButton.Enabled = DeleteBarButton.Enabled = (is_active == 1);
                }
                CloseProsecuteBarButton.Enabled = (GlobalVariables.CloseProsecute && is_active == 1 && parent_count > 0);
                NewBarButton.Enabled = (GlobalVariables.AddLawDetail && is_active == 1);
            }
        }

        private void DetailsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled && StandaloneBarDockControl.Enabled)
            {
                if (!String.IsNullOrWhiteSpace(NextDate))
                    LoadLawDetails("EDIT", DetailID);
                else
                    LoadProsecute("EDIT", DetailID);
            }
        }

        private void ProsecuteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled)
                LoadProsecute("INSERT", null);
        }

        private void DetailsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void LoadProsecute(string transaction, string detailid)
        {
            FProsecute fp = new FProsecute();
            fp.TransactionName = transaction;
            fp.ContractID = ContractID;
            fp.CustomerName = CustomerName;
            fp.DetailID = detailid;
            fp.ContractStartDate = ContractStartDateText.Text;
            fp.RefreshDetailsDataGridView += new FProsecute.DoEvent(RefreshDetails);
            fp.ShowDialog();
        }

        private void CloseProsecuteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled && is_active == 1)
            {
                List<ContractLawsView> lstLaw = ContractLawsViewDAL.SelectContractLaw(Convert.ToInt32(ParentID)).ToList<ContractLawsView>();
                if (lstLaw.Count == 0)
                    return;

                var law = lstLaw.First();
                string startdate = law.StartDate.ToString("dd.MM.yyyy"),
                   law_name = law.LawName,
                   judge_name = law.JudgeName;

                DialogResult dialogResult = XtraMessageBox.Show("<b><color=104,6,6>" + ContractCodeText.Text.Trim() + "</color></b> saylı müqavilənin aşağıda seçilmiş məhkəmə prosesini sonlandırmaq istəyirsiniz?\r\n\r\n   <b>Məhkəməyə verilmə tarixi:</b><color=255,0,0> " + startdate + "</color>\r\n   <b>Məhkəmənin adı:</b> <color=255,0,0>" + law_name + "</color>\r\n   <b>Hakimin adı:</b> <color=255,0,0>" + judge_name + "</color>\r\n\r\n<i>Əgər məhkəmə prosesini sonlandırsanız, bu zaman bu proses üçün </i><u><b><color=104,6,6>növbəti iclasları</color></b></u><i> daxil etmək olmuyacaq.</i>", "Məhkəmə iclasının sonlandırılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question, allowHtmlText: DefaultBoolean.True);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_CONTRACT_LAW_CLOSE", "P_CONTRACT_ID", ContractID, "P_PARENT_ID", ParentID, "Məhkəmə prosesi sonlandırılmadı");

                    //int maxid = GlobalFunctions.GetID($@"SELECT MAX(ID) FROM CRS_USER.CONTRACT_LAWS CL WHERE NVL(CL.PARENT_ID,CL.ID) = {ParentID}");
                    //GlobalProcedures.ExecuteTwoQuery("UPDATE CRS_USER.CONTRACT_LAWS SET LAST_DATE = (SELECT NVL(NEXT_DATE,SYSDATE) FROM CRS_USER.CONTRACT_LAWS WHERE ID = " + maxid + " AND CONTRACT_ID = " + ContractID + ") WHERE ID = " + maxid + " AND CONTRACT_ID = " + ContractID,
                    //                                       "UPDATE CRS_USER.CONTRACT_LAWS SET IS_ACTIVE = 0 WHERE IS_ACTIVE = 1 AND (ID = " + ParentID + " OR PARENT_ID = " + ParentID + ") AND CONTRACT_ID = " + ContractID,
                    //                                       "Məhkəmə prosesi sonlandırılmadı");
                    LoadLawDetailsDataGridView();
                }
            }
        }

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                return;

            string s = $@"SELECT CON.CONTRACT_ID,
                                   TO_CHAR (CON.START_DATE, 'DD.MM.YYYY') CONTRACT_START_DATE,
                                   CUS.CUSTOMER_NAME,
                                   COM.COMMITMENT_NAME
                              FROM CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER.V_COMMITMENTS COM
                            WHERE CON.CUSTOMER_ID = CUS.ID
                               AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                               AND CON.USED_USER_ID = -1
                               AND CON.CONTRACT_CODE = '{ContractCodeText.Text}'";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/ContractCodeText_EditValueChanged");

            if (dt.Rows.Count > 0)
            {
                StandaloneBarDockControl.Enabled = true;
                PopupMenu.Manager = DetailsBarManager;
                foreach (DataRow dr in dt.Rows)
                {
                    ContractID = dr["CONTRACT_ID"].ToString();
                    ContractStartDate = dr["CONTRACT_START_DATE"].ToString();
                    ContractStartDateText.Text = dr["CONTRACT_START_DATE"].ToString();
                    if (String.IsNullOrWhiteSpace(dr["COMMITMENT_NAME"].ToString()))
                        CustomerName = dr["CUSTOMER_NAME"].ToString();
                    else
                        CustomerName = dr["COMMITMENT_NAME"].ToString();
                    CustomerNameText.Text = dr["CUSTOMER_NAME"].ToString();
                    CommitmentNameText.Text = dr["COMMITMENT_NAME"].ToString();
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
                }
                LoadLawDetailsDataGridView();
            }
            else
            {
                DetailsGridControl.DataSource = null;
                StandaloneBarDockControl.Enabled = false;
                PopupMenu.Manager = null;
                ContractStartDateText.Text = CustomerNameText.Text = CommitmentNameText.Text = null;
                if (!String.IsNullOrEmpty(ContractID))
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            }

        }

        private void DetailsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(DetailsGridView.GetRowCellDisplayText(e.RowHandle, DetailsGridView.Columns["LAST_DATE"])) &&
                    String.IsNullOrWhiteSpace(DetailsGridView.GetRowCellDisplayText(e.RowHandle, DetailsGridView.Columns["NEXT_DATE"]))
                    && Convert.ToInt32(DetailsGridView.GetRowCellDisplayText(e.RowHandle, DetailsGridView.Columns["IS_ACTIVE"])) == 1)
            {
                e.Appearance.BackColor = Color.FromArgb(0x99, 0xFF, 0x99);
                e.Appearance.BackColor2 = Color.FromArgb(0x99, 0xFF, 0x99);
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }

            if (Convert.ToInt32(DetailsGridView.GetRowCellDisplayText(e.RowHandle, DetailsGridView.Columns["IS_ACTIVE"])) == 0)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ContractLawCloseColor1); 
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ContractLawCloseColor2); 
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (StandaloneBarDockControl.Enabled && is_active == 1)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş iclası silmək istəyirsiniz?", "Məhkəmə iclasının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CONTRACT_LAWS WHERE ID = {DetailID}", 
                                                        "Məhkəmə iclası silinmədi.",
                                                      this.Name + "/DeleteBarButton_ItemClick");
                    LoadLawDetailsDataGridView();
                }
            }
        }

        private void FLawDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ContractID))
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshLawsDataGridView();
        }

        private void FLawDetails_Load(object sender, EventArgs e)
        {
            //permision
            if (GlobalVariables.V_UserID > 0)
            {
                ProsecuteBarButton.Enabled = GlobalVariables.AddProsecute;
            }

            if (TransactionName == "EDIT")
            {
                CloseProsecuteBarButton.Enabled = false;

                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT COM.COMMITMENT_NAME,
                                                                           (SELECT CUSTOMER_NAME
                                                                              FROM CRS_USER.V_CUSTOMERS
                                                                             WHERE ID = C.CUSTOMER_ID)
                                                                              CUSTOMER_NAME
                                                                      FROM CRS_USER.V_CONTRACTS C, CRS_USER.V_COMMITMENTS COM
                                                                     WHERE C.CONTRACT_ID = COM.CONTRACT_ID(+) AND C.CONTRACT_ID = {ContractID}",
                                                                  this.Name + "/FLawDetails_Load");
                commitment = dt.Rows[0]["COMMITMENT_NAME"].ToString();
                customerfullname = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                
                if (String.IsNullOrWhiteSpace(commitment))
                    CustomerName = customerfullname;
                else
                    CustomerName = commitment;
                
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
                List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
                UsedUserID = lstContract.First().USED_USER_ID;
                Used = (UsedUserID >= 0);

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçdiyiniz müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
                ContractCodeText.TabStop = false;
                ContractCodeText.Properties.ReadOnly = true;
                ContractCodeText.Text = ContractCode.Substring(0,4);
                ContractStartDateText.Text = ContractStartDate;
                LoadLawDetailsDataGridView();
                CustomerNameText.Text = customerfullname;
                CommitmentNameText.Text = commitment;
            }
            else
            {
                ContractCodeText.Properties.ReadOnly = false;
                StandaloneBarDockControl.Enabled = false;
            }
        }

        public void ComponentEnable(bool status)
        {
            StandaloneBarDockControl.Enabled = !status;
            if (status)
                PopupMenu.Manager = null;
            else
                PopupMenu.Manager = DetailsBarManager;
        }

        private void LoadLawDetailsDataGridView()
        {
            string s = $@"SELECT *
                                  FROM (SELECT (CASE
                                                   WHEN CL.PARENT_ID IS NULL
                                                   THEN
                                                      DENSE_RANK () OVER (ORDER BY START_DATE, CL.LAW_ID)
                                                   ELSE
                                                      NULL
                                                END)
                                                  SS,
                                               CL.ID,
                                               CL.APPLICANT,
                                               CL.DEFANDANT_NAME,
                                               L.NAME LAW_NAME,
                                               CL.JUDGE_NAME,
                                               CL.REPRESENTATIVE,
                                               (CASE
                                                   WHEN CL.PARENT_ID IS NULL THEN CL.START_DATE
                                                   ELSE NULL
                                                END)
                                                  START_DATE,
                                               CL.LAST_DATE,
                                               LS.NAME STATUS_NAME,
                                               CL.NEXT_DATE,
                                               CL.NOTE,
                                               CL.CREATED_USER_NAME,
                                               NVL (CL.PARENT_ID, CL.ID) PARENT_ID,
                                               CL.PARENT_ID PID,
                                               CL.IS_ACTIVE
                                          FROM CRS_USER.CONTRACT_LAWS CL,
                                               CRS_USER.LAWS L,
                                               CRS_USER.LAW_STATUS LS
                                         WHERE     CL.LAW_ID = L.ID
                                               AND CL.LAW_STATUS_ID = LS.ID
                                               AND CL.CONTRACT_ID = {ContractID})
                            START WITH PID IS NULL
                            CONNECT BY PRIOR ID = PID";

            try
            {

                DetailsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLawDetailsDataGridView");

                if (DetailsGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                    {
                        NewBarButton.Enabled = GlobalVariables.AddLawDetail;
                        CloseProsecuteBarButton.Enabled = GlobalVariables.CloseProsecute;
                    }
                    else
                        NewBarButton.Enabled = CloseProsecuteBarButton.Enabled = true;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = NewBarButton.Enabled = CloseProsecuteBarButton.Enabled = false;

                DetailsGridView.FocusedRowHandle = DetailsGridView.RowCount - 1;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məhkəmə proseslərinin siyahısı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}