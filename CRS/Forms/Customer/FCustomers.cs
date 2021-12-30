using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;

namespace CRS.Forms.Customer
{
    public partial class FCustomers : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public delegate void DoEvent(string a);
        public string TransactionName;
        public event DoEvent RefreshDataGridView;

        public FCustomers()
        {
            InitializeComponent();
        }
        string CustomerFullName, CustomerName, CustomerID, CustomerCode, phone = null, mail = null, toolTip = null, ToolTipCustomerID = null, TitleText = null, ContractID, SellerID, birthplacename, sexname;
        bool FormStatus = false;
        int old_row_num = 0, 
            topindex, 
            row_num, 
            person_type_id,
            commit;

        private void FCustomers_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                NewCustomerBarSubItem.Enabled = GlobalVariables.AddCustomer;                
                PrintBarButton.Enabled = GlobalVariables.PrintCustomer;
                ExportBarSubItem.Enabled = GlobalVariables.ExportCustomer;
                SendMailBarButton.Enabled = GlobalVariables.SendMailCustomer;
                SendSmsBarButton.Enabled = GlobalVariables.SendSmsCustomer;
                VoenCalcBarButton.Enabled = GlobalVariables.ShowVoenList;
            }

            ShowOrHideContractBarButton.Down = true;
            SearchDockPanel.Hide();
            CustomersGridControl.Height = this.Height - 180;
            FormStatus = true;
        }

        private void ShowOrHideContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!ShowOrHideContractBarButton.Down)
            {
                ContractsGridControl.Visible = false;
                CustomersGridControl.Dock = DockStyle.Fill;
                CustomerSplitter.Visible = false;
                ShowOrHideContractBarButton.Caption = "Müqavilələri göstər";
            }
            else
            {
                ContractsGridControl.Visible = true;
                CustomersGridControl.Dock = DockStyle.Top;
                CustomerSplitter.Visible = true;
                ShowOrHideContractBarButton.Caption = "Müqavilələri gizlət";
            }
        }

        void RefreshCustomers(string code)
        {
            LoadCustomersDataGridView();
        }

        private void LoadFCustomerAddEdit(string transaction, string customer_id, string fullname)
        {
            if (transaction == "INSERT")
                old_row_num = topindex = CustomersGridView.RowCount;
            else
            {
                old_row_num = CustomersGridView.FocusedRowHandle;
                topindex = CustomersGridView.TopRowIndex;
            }

            L2FCustomerAddEdit fcae = new L2FCustomerAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerID = customer_id;
            fcae.CustomerFullName = fullname;
            fcae.RefreshCustomersDataGridView += new L2FCustomerAddEdit.DoEvent(RefreshCustomers);
            fcae.ShowDialog();

            CustomersGridView.FocusedRowHandle = old_row_num;
            CustomersGridView.TopRowIndex = topindex;
        }
        
        private void CustomersGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Customer_SS, "Center", e);
        }

        private void CustomersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CustomersGridView, PopupMenu, e);
        }

        private void LoadCustomersDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 C.ID CUSTOMER_ID,
                                 C.CODE CUSTOMER_CODE,
                                 PT.NAME CUSTOMER_TYPE,
                                 C.FULLNAME,
                                 P.PHONE,
                                 C.ADDRESS,
                                 C.REGISTRATION_ADDRESS,
                                 C.NOTE,
                                 C.USED_USER_ID,
                                 ROW_NUMBER () OVER (ORDER BY 3, 4) ROW_NUM,
                                 C.PERSON_TYPE_ID,
                                 C.VOEN,
                                 C.CUSTOMER_NAME
                            FROM CRS_USER.V_CUSTOMERS C,
                                 CRS_USER.PERSON_TYPE PT,         
                                 (  SELECT P.OWNER_ID,
                                           LISTAGG (
                                                 0
                                              || SUBSTR (P.PHONE_NUMBER, 1, 2)
                                              || '-'
                                              || SUBSTR (P.PHONE_NUMBER, 3, 3)
                                              || '-'
                                              || SUBSTR (P.PHONE_NUMBER, 6, 2)
                                              || '-'
                                              || SUBSTR (P.PHONE_NUMBER, 8, 2),
                                              ';')
                                           WITHIN GROUP (ORDER BY ORDER_ID)
                                              PHONE,
                                           DECODE (P.OWNER_TYPE,  'C', 1,  'JP', 2) PERSON_TYPE_ID
                                      FROM CRS_USER.PHONES P
                                     WHERE P.OWNER_TYPE IN ('C', 'JP')
                                  GROUP BY P.OWNER_ID, P.OWNER_TYPE) P
                           WHERE     C.ID = P.OWNER_ID(+)
                                 AND C.PERSON_TYPE_ID = P.PERSON_TYPE_ID(+)
                                 AND C.PERSON_TYPE_ID = PT.ID
                        ORDER BY 3, 5";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomersDataGridView", "Müştərilərin siyahısı yüklənmədi.");

            if (dt == null)
                return;

            CustomersGridControl.DataSource = dt;
            if (CustomersGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteCustomerBarButton.Enabled = GlobalVariables.DeleteCustomer;
                    EditCustomerBarButton.Enabled = GlobalVariables.EditCustomer;
                    SendMailBarButton.Enabled = GlobalVariables.SendMailCustomer;
                    SendSmsBarButton.Enabled = GlobalVariables.SendSmsCustomer;
                }
                else
                    DeleteCustomerBarButton.Enabled = EditCustomerBarButton.Enabled = SendMailBarButton.Enabled = SendSmsBarButton.Enabled = true;
            }
            else
                DeleteCustomerBarButton.Enabled = EditCustomerBarButton.Enabled = SendMailBarButton.Enabled = SendSmsBarButton.Enabled = false;
        }

        private void CustomersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CustomersGridView.GetFocusedDataRow();
            if (row != null)
            {
                CustomerID = row["CUSTOMER_ID"].ToString();
                CustomerFullName = row["FULLNAME"].ToString();
                CustomerName = row["CUSTOMER_NAME"].ToString();
                CustomerCode = row["CUSTOMER_CODE"].ToString();
                row_num = Convert.ToInt32(row["ROW_NUM"].ToString());
                person_type_id = Convert.ToInt32(row["PERSON_TYPE_ID"].ToString());
                LoadContractsDataGridView();
                ContractsGridView.ViewCaption = "Seçilmiş şəxsə aid olan müqavilələr";
            }
        }

        private void LoadContractsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 C.ID,
                                 CT.CODE || C.CODE CONTRACT_CODE,
                                 ST.STATUS_NAME,
                                 CN.NAME CREDIT_NAME,
                                 COM.COMMITMENT_NAME COMMITMENT,
                                 H.HOSTAGE,
                                    TO_CHAR (C.START_DATE, 'DD.MM.YYYY')
                                 || ' - '
                                 || TO_CHAR (C.END_DATE, 'DD.MM.YYYY')
                                    CONTRACT_DATE,
                                 (CASE
                                     WHEN C.CHECK_PERIOD > 0 THEN C.CHECK_PERIOD || ' ay'
                                     ELSE CT.TERM || ' ay'
                                  END)
                                    PERIOD,
                                 (CASE
                                     WHEN C.CHECK_INTEREST > -1 THEN C.CHECK_INTEREST || ' %'
                                     ELSE CT.INTEREST || ' %'
                                  END)
                                    INTEREST,
                                 C.GRACE_PERIOD || ' ay' GRACE,
                                 C.AMOUNT||' '||CUR.CODE AMOUNT,         
                                 C.SELLER_ID,         
                                 C.STATUS_ID,         
                                 C.SELLER_TYPE_ID,
                                 C.CUSTOMER_TYPE_ID,
                                 C.IS_COMMIT
                            FROM CRS_USER.CONTRACTS C,
                                 CRS_USER.CREDIT_TYPE CT,
                                 CRS_USER.CREDIT_NAMES CN,
                                 CRS_USER.CURRENCY CUR,
                                 CRS_USER.STATUS ST,
                                 CRS_USER.V_COMMITMENTS COM,
                                 CRS_USER.V_HOSTAGE H
                           WHERE     C.CREDIT_TYPE_ID = CT.ID
                                 AND C.ID = COM.CONTRACT_ID(+)
                                 AND CT.NAME_ID = CN.ID
                                 AND C.CURRENCY_ID = CUR.ID
                                 AND C.STATUS_ID = ST.ID
                                 AND C.ID = H.CONTRACT_ID
                                 AND C.CUSTOMER_ID = {CustomerID}
                                 AND C.CUSTOMER_TYPE_ID = {person_type_id}
                        ORDER BY CT.CODE || C.CODE DESC";

            ContractsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
        }

        void RefreshContracts(string contract_id)
        {
            LoadContractsDataGridView();
        }

        private void LoadFContractAddEdit(string transaction, string contract_id, string customer_id, string seller_id)
        {
            Contracts.FCarOrObjectContractAddEdit fcae = new Contracts.FCarOrObjectContractAddEdit();
            fcae.TransactionName = transaction;
            fcae.ContractID = contract_id;
            fcae.CustomerID = customer_id;
            fcae.Commit = commit;
            fcae.SellerID = seller_id;
            fcae.RefreshContractsDataGridView += new Contracts.FCarOrObjectContractAddEdit.DoEvent(RefreshContracts);
            fcae.ShowDialog();
        }

        private void EditCustomerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (person_type_id == 1)
                LoadFCustomerAddEdit("EDIT", CustomerID, CustomerFullName);
            else
                LoadFJuridicalPersonAddEdit("EDIT", CustomerID);
        }

        private void CustomersGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCustomerBarButton.Enabled)
            {
                if (person_type_id == 1)
                    LoadFCustomerAddEdit("EDIT", CustomerID, CustomerFullName);
                else
                    LoadFJuridicalPersonAddEdit("EDIT", CustomerID);
            }
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CustomersGridControl);
        }

        private void CustomerToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            bool validColumn = false;
            phone = null; mail = null;
            if (e.SelectedControl != CustomersGridControl)
                return;
            DataTable dt = null;
            GridHitInfo hitInfo = CustomersGridView.CalcHitInfo(e.ControlMousePosition);

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            if (hitInfo.Column.FieldName == "FULLNAME")
                validColumn = true;

            if (!validColumn)
                return;

            SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            toolTipArgs.AllowHtmlText = DefaultBoolean.True;
            DataRow drCurrentRow = CustomersGridView.GetDataRow(hitInfo.RowHandle);
            if (drCurrentRow != null)
            {
                TitleText = drCurrentRow["FULLNAME"].ToString();
                ToolTipCustomerID = drCurrentRow["CUSTOMER_ID"].ToString();
                toolTipArgs.Title.Text = "<color=255,0,0>" + TitleText + "</color>";
            }

            dt = GlobalFunctions.GenerateDataTable("SELECT T.IMAGE FROM CRS_USER.CUSTOMER_IMAGE T WHERE T.CUSTOMER_ID = " + ToolTipCustomerID);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        toolTipArgs.Contents.Image = Image.FromStream(stmBLOBData);
                    }
                }
            }

            dt = GlobalFunctions.GenerateDataTable("SELECT MAIL A FROM CRS_USER.MAILS WHERE OWNER_TYPE = 'C' AND OWNER_ID = " + ToolTipCustomerID);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mail = mail + "\n" + dr[0];
                }
            }

            toolTip = null;
            if (!String.IsNullOrEmpty(mail))
                toolTip = toolTip + "\n<b><color=255, 0, 0>Mail</color></b> " + " " + mail;

            toolTipArgs.Footer.Text = toolTip;
            toolTipArgs.ShowFooterSeparator = true;
            e.Info = new ToolTipControlInfo();
            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
            e.Info.ToolTipType = ToolTipType.SuperTip;
            e.Info.SuperTip = new SuperToolTip();
            e.Info.SuperTip.Setup(toolTipArgs);
        }

        private void CustomersGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CustomersGridView, e);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "mht");
        }

        private void RefreshCustomerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadCustomersDataGridView();
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CustomerNameText.Text = 
                BirthplaceComboBox.Text = 
                SexComboBox.Text = 
                AddressText.Text = 
                NoteText.Text = 
                RegistrationAddressText.Text = null;
            CustomersGridView.ClearColumnsFilter();
        }

        void RefreshContract(string contract_id)
        {
            LoadCustomersDataGridView();
        }

        private void LoadFContractAddEdit(string transaction, string customer_code)
        {
            topindex = CustomersGridView.TopRowIndex;
            old_row_num = CustomersGridView.FocusedRowHandle;
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractShowWait));
            Contracts.FCarOrObjectContractAddEdit fcae = new Contracts.FCarOrObjectContractAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerCode = customer_code;
            fcae.RefreshContractsDataGridView += new Contracts.FCarOrObjectContractAddEdit.DoEvent(RefreshContract);
            GlobalProcedures.SplashScreenClose();
            fcae.ShowDialog();
            CustomersGridView.TopRowIndex = topindex;
            CustomersGridView.FocusedRowHandle = old_row_num;
        }

        private void NewContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {                      
            LoadFContractAddEdit("INSERT", CustomerCode);       
        }

        private void ContractsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractsGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractID = row["ID"].ToString();
                SellerID = row["SELLER_ID"].ToString();
                commit = Convert.ToInt32(row["IS_COMMIT"].ToString());
            }
        }

        private void CustomersGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ContractsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (ContractsGridView.RowCount > 0)
                LoadFContractAddEdit("EDIT", ContractID, CustomerID, SellerID);
        }

        private void IndividualPersonAddBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFCustomerAddEdit("INSERT", null, null);
        }

        private void VoenCalcBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FVoenPaymentCalc fv = new FVoenPaymentCalc();
            fv.ShowDialog();
        }

        private void CustomersGridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Customer_SS, e);
        }

        private void ContractsGridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, CustomerContract_SS, e);
        }

        private void CustomersGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(CustomersGridView, CustomerRribbonPage.Text);
        }

        private void ContractsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(ContractsGridView, "Müştərinin müqavilələri");
        }

        private void SendSmsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FSendSms sms = new FSendSms();
            sms.OwnerID = int.Parse(CustomerID);
            sms.PersonTypeID = person_type_id;
            sms.OwnerName = CustomerName;
            sms.OwnerType = (person_type_id == 1) ? "C" : "JP";
            sms.ShowDialog();
        }

        private void LoadFJuridicalPersonAddEdit(string transaction, string person_id)
        {
            if (transaction == "INSERT")
                old_row_num = topindex = CustomersGridView.RowCount;
            else
            {
                old_row_num = CustomersGridView.FocusedRowHandle;
                topindex = CustomersGridView.TopRowIndex;
            }

            FJuridicalPersonAddEdit fj = new FJuridicalPersonAddEdit();
            fj.TransactionName = transaction;
            fj.CustomerID = person_id;
            fj.RefreshCustomersDataGridView += new FJuridicalPersonAddEdit.DoEvent(RefreshCustomers);
            fj.ShowDialog();

            CustomersGridView.FocusedRowHandle = old_row_num;
            CustomersGridView.TopRowIndex = topindex;
        }

        private void JuridicalPersonAddBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFJuridicalPersonAddEdit("INSERT", null);
        }

        private void ContractsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(CustomerContract_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT", "Far", e);
        }

        private void DeleteCustomer()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CUSTOMER_ID = " + CustomerID + " AND CUSTOMER_TYPE_ID = " + person_type_id);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş şəxsin məlumatlarını silmək istəyirsiniz?", "Müştərinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (person_type_id == 1)
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CUSTOMER_DELETE", "P_CUSTOMER_ID", CustomerID, "Seçilmiş şəxsin məlumatları silinmədi.");
                    else
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_JURIDICAL_PERSON_DELETE", "P_CUSTOMER_ID", CustomerID, "Seçilmiş şəxsin məlumatları silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş şəxsin adına müqavilə olduğu üçün onun məlumatlarını silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCustomerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = CustomersGridView.FocusedRowHandle;
            topindex = CustomersGridView.TopRowIndex;
            int CustomerUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.V_CUSTOMERS WHERE ID = " + CustomerID);
            if ((CustomerUsedUserID == -1) || (GlobalVariables.V_UserID == CustomerUsedUserID))
                DeleteCustomer();
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CustomerUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş şəxsin məlumatları " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatlarını silmək olmaz.", "Seçilmiş müştərinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadCustomersDataGridView();
            CustomersGridView.FocusedRowHandle = old_row_num;
            CustomersGridView.TopRowIndex = topindex;
        }

        private void ContractsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForClose(6, ContractsGridView, e);
        }

        private void CommitmentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                FCommitments fc = new FCommitments();
                fc.ShowDialog();
            }
            catch { }
        }

        private void FCustomers_Activated(object sender, EventArgs e)
        {
            LoadCustomersDataGridView();
            CustomersGridView.FocusedRowHandle = CustomersGridView.TopRowIndex = CustomersGridView.RowCount - 1;

            GlobalProcedures.GridRestoreLayout(CustomersGridView, CustomerRribbonPage.Text);
            GlobalProcedures.GridRestoreLayout(ContractsGridView, "Müştərinin müqavilələri");
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                SearchDockPanel.Show();
                GlobalProcedures.FillCheckedComboBox(SexComboBox, "SEX", "NAME,NAME_EN,NAME_RU", null);
                GlobalProcedures.FillCheckedComboBox(BirthplaceComboBox, "BIRTHPLACE", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
            }
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FilterCustomers()
        {
            if (FormStatus)
            {
                ColumnView view = CustomersGridView;

                //CustomerName
                if (!String.IsNullOrEmpty(CustomerNameText.Text))
                    view.ActiveFilter.Add(view.Columns["FULLNAME"],
                  new ColumnFilterInfo("[FULLNAME] Like '%" + CustomerNameText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["FULLNAME"]);

                //Address
                if (!String.IsNullOrEmpty(AddressText.Text))
                    view.ActiveFilter.Add(view.Columns["ADDRESS"],
                  new ColumnFilterInfo("[ADDRESS] Like '%" + AddressText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["ADDRESS"]);

                //RegistrationAddress
                if (!String.IsNullOrEmpty(RegistrationAddressText.Text))
                    view.ActiveFilter.Add(view.Columns["REGISTRATION_ADDRESS"],
                  new ColumnFilterInfo("[REGISTRATION_ADDRESS] Like '%" + RegistrationAddressText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["REGISTRATION_ADDRESS"]);

                //Birthplace
                if (!String.IsNullOrEmpty(BirthplaceComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["BIRTHPLACE"],
                        new ColumnFilterInfo(birthplacename, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["BIRTHPLACE"]);

                //Sex
                if (!String.IsNullOrEmpty(SexComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["SEX_NAME"],
                        new ColumnFilterInfo(sexname, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["SEX_NAME"]);

                //Phone
                if (!String.IsNullOrEmpty(PhoneText.Text))
                    view.ActiveFilter.Add(view.Columns["PHONE"],
                  new ColumnFilterInfo("[PHONE] Like '%" + PhoneText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["PHONE"]);

                //VOEN
                if (!String.IsNullOrEmpty(VoenText.Text))
                    view.ActiveFilter.Add(view.Columns["VOEN"],
                  new ColumnFilterInfo("[VOEN] Like '%" + VoenText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["VOEN"]);

                //Note
                if (!String.IsNullOrEmpty(NoteText.Text))
                    view.ActiveFilter.Add(view.Columns["NOTE"],
                  new ColumnFilterInfo("[NOTE] Like '%" + NoteText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["NOTE"]);
            }
        }

        private void BirthplaceComboBox_EditValueChanged(object sender, EventArgs e)
        {
            birthplacename = " [BIRTHPLACE] IN ('" + BirthplaceComboBox.Text.Replace("; ", "','") + "')";
            FilterCustomers();
        }

        private void SexComboBox_EditValueChanged(object sender, EventArgs e)
        {
            sexname = " [SEX_NAME] IN ('" + SexComboBox.Text.Replace("; ", "','") + "')";
            FilterCustomers();
        }

        private void CustomerNameText_EditValueChanged(object sender, EventArgs e)
        {
            FilterCustomers();
        }

        private void CustomersGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CustomersGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteCustomerBarButton.Enabled = GlobalVariables.DeleteCustomer;
                    EditCustomerBarButton.Enabled = GlobalVariables.EditCustomer;
                    SendMailBarButton.Enabled = GlobalVariables.SendMailCustomer;
                    SendSmsBarButton.Enabled = GlobalVariables.SendSmsCustomer;
                }
                else
                    DeleteCustomerBarButton.Enabled = EditCustomerBarButton.Enabled = SendMailBarButton.Enabled = SendSmsBarButton.Enabled = true;
            }
            else
                DeleteCustomerBarButton.Enabled = EditCustomerBarButton.Enabled = SendMailBarButton.Enabled = SendSmsBarButton.Enabled = false;
        }
    }
}