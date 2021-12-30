using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FDictionaries : DevExpress.XtraEditors.XtraForm
    {
        public FDictionaries()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public int ViewSelectedTabIndex = 0, HostageSelectedTabIndex = 0, FundSelectedTabIndex = 0, CashAppointmentSelectedTabIndex;
        public string TransactionType, StatusWhere = null;
        string PhoneID,
            StatusID,
            SeriesID,
            IssuingID,
            BirthplaceID,
            CreditTypeID,
            PawnTypeID,
            CurrencyID,
            BankID,
            AppointmentID,
            CountryID,
            goldTypeID,
            CashOtherAppointmentID,
            KindShipID,
            ContractEvaluateID,
            AppraiserID,
            EyebrowsID,
            FundsSourceID,
            LoanOfficerID,
            CarBrandID,
            CarBanTypeID,
            CarColorID,
            CarTypeID,
            CarModelID,
            Credit1ClassID,
            creditClassID,
            creditStatusID,
            creditPurposeID,
            collateralTypeID,
            typeCreditID,
            typeDocumentID
            ;

        bool FormStatus = false;
        int topindex, old_row_id, contractEvaluateIsDeleted, old_row_num;

        public delegate void DoEvent(int index);
        public event DoEvent RefreshList;

        private void FDictionaries_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                PhoneDescriptionTab.Visible = GlobalVariables.PhoneDescription;
                StatusTab.Visible = GlobalVariables.Status;
                CardSeriesTab.Visible = GlobalVariables.CardSeries;
                CardIssuingTab.Visible = GlobalVariables.CardIssuing;
                BirthplaceTab.Visible = GlobalVariables.Birthplace;
                BirthplaceTab.Visible = GlobalVariables.Birthplace;
                CreditTypeTab.Visible = GlobalVariables.CreditType;
                CurrencyTab.Visible = GlobalVariables.Currency;
                CountriesTab.Visible = GlobalVariables.Countries;
            }

            BackstageViewControl.SelectedTabIndex = ViewSelectedTabIndex;
            LoadPhoneDescriptionDataGridView();
            FormStatus = true;
            if (TransactionType == "E")
            {
                switch (BackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadPhoneDescriptionDataGridView();
                        break;
                    case 1:
                        LoadStatusDataGridView();
                        break;
                    case 2:
                        LoadCardSeriesDataGridView();
                        break;
                    case 3:
                        LoadCardIssuingDataGridView();
                        break;
                    case 4:
                        LoadBirthplaceDataGridView();
                        break;
                    case 5:
                        LoadCreditStatus();
                        break;
                    
                    case 7:
                        LoadCurrencyDataGridView();
                        break;
                    
                    case 8:
                        LoadCountriesDataGridView();
                        break;
                    case 9:
                        LoadKindshipRateDataGridView();
                        break;
                    case 10:
                        LoadContractEvaluateDataGridView();
                        break;
                    case 11:
                        LoadDocumentType();
                        break;
                    case 12:
                        LoadLoanOfficerDataGridView();
                        break;
                    case 13:
                        LoadEyebrowsDataGridView();
                        break;
                    case 14:
                        LoadPawnTypeDataGridView();
                        break;
                    //case 15:
                    //    LoadAppointmentDataGridView();
                    //    break;
                    case 15:
                        LoadCarColorDataGridView();
                        break;
                    
                    case 16:
                        LoadCarModelDataGridView();
                        break;
                    case 17:
                        LoadCarTypeDataGridView();
                        break;
                    case 18:
                        LoadCarBrandDataGridView();
                        break;
                    case 19:
                        LoadCarBanTypeDataGridView();
                        break;
                }

                switch (CreditParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCreditTypeDataGridView();
                        break;
                    case 1:
                        LoadTypeCredit();
                        break;
                    case 2:
                        LoadCreditPurpose();
                        break;
                    case 3:
                        LoadCreditClass();
                        break;
                    case 4:
                        LoadCreditStatus();
                        break;
                    case 5:
                        LoadCollateralType();
                        break;
                }


                switch (PawnParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadEyebrowsDataGridView();
                        break;
                    case 1:
                        LoadGoldType();
                        break;
                    case 2:
                        LoadPawnTypeDataGridView();
                        break;

                }

                switch (LizingParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCarBrandDataGridView();
                        break;
                    case 1:
                        LoadCarModelDataGridView();
                        break;
                    case 2:
                        LoadCarTypeDataGridView();
                        break;
                    case 3:
                        LoadCarColorDataGridView();
                        break;
                    case 4:
                        LoadCarBanTypeDataGridView();
                        break;
                }
            }
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PhoneGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void StatusGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(StatusGridView, StatusPopupMenu, e);
        }

        private void LoadPhoneDescriptionDataGridView()
        {
            string s = "SELECT 1 SS,ID,DESCRIPTION_AZ,USED_USER_ID FROM CRS_USER.PHONE_DESCRIPTIONS ORDER BY DESCRIPTION_AZ ASC";

            PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            DeletePhoneBarButton.Enabled = EditPhoneBarButton.Enabled = (PhoneGridView.RowCount > 0);
        }

        private void LoadStatusDataGridView()
        {
            string s = $@"SELECT 1 SS,ID,OWNER_FORM,STATUS_DESCRIPTION,STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU,USED_USER_ID FROM CRS_USER.STATUS {StatusWhere} ORDER BY ID";
            StatusGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            EditStatusBarButton.Enabled = (StatusGridView.RowCount > 0);
        }

        private void LoadCardSeriesDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,SERIES,NOTE,USED_USER_ID FROM CRS_USER.CARD_SERIES ORDER BY ORDER_ID";

            CardSeriesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);

            if (CardSeriesGridView.RowCount > 0)
            {
                DeleteCardSeriesBarButton.Enabled =
                    EditCardSeriesBarButton.Enabled =
                    SortCardSeriesBarButton.Enabled = true;

                if (CardSeriesGridView.FocusedRowHandle == 0)
                    UpCardSeriesBarButton.Enabled = false;
                DownCardSeriesBarButton.Enabled = (CardSeriesGridView.RowCount > 1);
            }
            else

                DeleteCardSeriesBarButton.Enabled =
                    EditCardSeriesBarButton.Enabled =
                    UpCardSeriesBarButton.Enabled =
                    DownCardSeriesBarButton.Enabled =
                    SortCardSeriesBarButton.Enabled = false;
        }

        private void LoadBirthplaceDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.BIRTHPLACE ORDER BY ORDER_ID";

            BirthplaceGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);

            if (BirthplaceGridView.RowCount > 0)
            {
                DeleteBirthplaceBarButton.Enabled = EditBirthplaceBarButton.Enabled = BirthplaceSortBarButton.Enabled = true;

                if (BirthplaceGridView.FocusedRowHandle == 0)
                    UpBirthplaceBarButton.Enabled = false;
                DownBirthplaceBarButton.Enabled = (BirthplaceGridView.RowCount > 1);
            }
            else
                DeleteBirthplaceBarButton.Enabled =
                    EditBirthplaceBarButton.Enabled =
                    UpBirthplaceBarButton.Enabled =
                    DownBirthplaceBarButton.Enabled =
                    BirthplaceSortBarButton.Enabled = false;
        }

        private void LoadCardIssuingDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CARD_ISSUING ORDER BY ORDER_ID";

            CardIssuingGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);

            if (CardIssuingGridView.RowCount > 0)
            {
                DeleteCardIssuingBarButton.Enabled =
                    EditCardIssuingBarButton.Enabled =
                    SortCardIssuingButton.Enabled = true;

                if (CardIssuingGridView.FocusedRowHandle == 0)
                    UpCardIssuingBarButton.Enabled = false;
                DownCardIssuingBarButton.Enabled = (CardIssuingGridView.RowCount > 1);
            }
            else
                DeleteCardIssuingBarButton.Enabled =
                    EditCardIssuingBarButton.Enabled =
                    UpCardIssuingBarButton.Enabled =
                    DownCardIssuingBarButton.Enabled =
                    SortCardIssuingButton.Enabled = false;
        }

        private void LoadCreditTypeDataGridView()
        {
            string s = $@"SELECT CT.ID,
                                 CN.NAME,
                                 CT.CALC_DATE,
                                 CT.TERM,
                                 CT.INTEREST,
                                 CT.NOTE,
                                 CT.USED_USER_ID,
                                 CC.NAME CATEGORY_NAME,
                                 CT.PENALTY_PERCENT,
                                 CT.COMMISSION
                            FROM CRS_USER.CREDIT_TYPE CT, CRS_USER.CREDIT_NAMES CN, CRS_USER.CREDIT_CATEGORY CC
                           WHERE CT.NAME_ID = CN.ID 
                             AND CN.CREDIT_CATEGORY_ID = CC.ID
                        ORDER BY CN.NAME, CT.CALC_DATE DESC";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditTypeDataGridView", "Kreditin rekvizitləri yüklənmədi.");

            CreditTypeGridControl.DataSource = dt;

            DeleteCreditTypeBarButton.Enabled = EditCreditTypeBarButton.Enabled = (CreditTypeGridView.RowCount > 0);
        }

        private void LoadPawnTypeDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.PAWN_TYPE ORDER BY NAME";
            PawnTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPawnTypeDataGridView", "Girova qoyulan əşyaların siyahısı yüklənmədi.");

            DeletePawnTypeBarButton.Enabled = EditPawnTypeBarButton.Enabled = PawnTypeGridView.RowCount > 0;
        }

        private void LoadCurrencyDataGridView()
        {
            string s = "SELECT 1 SS,ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE,USED_USER_ID FROM CRS_USER.CURRENCY ORDER BY ORDER_ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCurrencyDataGridView", "Valyutalar yüklənmədi.");

            CurrencyGridControl.DataSource = dt;

            if (CurrencyGridView.RowCount > 0)
            {
                DeleteCurrencyBarButton.Enabled = EditCurrencyBarButton.Enabled = true;
                if (CurrencyGridView.FocusedRowHandle == 0)
                    UpCurrencyBarButton.Enabled = false;

                DownCurrencyBarButton.Enabled = (CurrencyGridView.RowCount > 1);
            }
            else
                DeleteCurrencyBarButton.Enabled = EditCurrencyBarButton.Enabled = UpCurrencyBarButton.Enabled = DownCurrencyBarButton.Enabled = false;
        }

        private void LoadCountriesDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,CODE,NOTE,USED_USER_ID FROM CRS_USER.COUNTRIES ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCountriesDataGridView");

                CountriesGridControl.DataSource = dt;
                CountriesGridView.PopulateColumns();

                CountriesGridView.Columns[0].Caption = "S/s";
                CountriesGridView.Columns[1].Visible = false;
                CountriesGridView.Columns[2].Caption = "Ölkənin adı";
                CountriesGridView.Columns[3].Caption = "Kodu";
                CountriesGridView.Columns[4].Caption = "Qeyd";
                CountriesGridView.Columns[5].Visible = false;

                CountriesGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CountriesGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CountriesGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CountriesGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CountriesGridView.RowCount > 0)
                {
                    DeleteCountryBarButton.Enabled = true;
                    EditCountryBarButton.Enabled = true;
                    CountriesGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CountriesGridView.FocusedRowHandle == 0)
                        UpCountryBarButton.Enabled = false;
                    DownCountryBarButton.Enabled = (CountriesGridView.RowCount > 1);
                }
                else
                {
                    DeleteCountryBarButton.Enabled =
                        EditCountryBarButton.Enabled =
                        UpCountryBarButton.Enabled =
                        DownCountryBarButton.Enabled = false;
                }
                CountriesGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ölkələr cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadKindshipRateDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.KINDSHIP_RATE ORDER BY ORDER_ID";

            KindshipRateGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            DeleteKindshipRateBarButton.Enabled = EditKindshipRateBarButton.Enabled = (KindshipRateGridView.RowCount > 0);
        }

        private void LoadContractEvaluateDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID,IS_DELETED FROM CRS_USER.CONTRACT_EVALUATE ORDER BY ORDER_ID";

            ContractEvaluateGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            DeleteContractEvaluateBarButton.Enabled = EditContractEvaluateBarButton.Enabled = (ContractEvaluateGridView.RowCount > 0);
        }

        private void LoadAppraiserDataGridView()
        {
            string s = "SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.APPRAISER ORDER BY NAME";

            AppraiserGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAppraiserDataGridView", "Qiymətləndiricilərin siyahısı yüklənmədi.");
            DeleteAppraiserBarButton.Enabled = EditAppraiserBarButton.Enabled = (AppraiserGridView.RowCount > 0);
        }

        private void LoadEyebrowsDataGridView()
        {
            string s = "SELECT ID,NAME,NOTE,DECODE(IS_DEDUCTION, 1, 'Bəli', 'Xeyr') DEDUCTION,USED_USER_ID FROM CRS_USER.EYEBROWS_TYPE ORDER BY NAME";

            EyebrowsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadEyebrowsDataGridView", "Qaşın növlərinin siyahısı yüklənmədi.");
            DeleteEyebrowsBarButton.Enabled = EditEyebrowsBarButton.Enabled = (EyebrowsGridView.RowCount > 0);
        }

        private void LoadLoanOfficerDataGridView()
        {
            string s = "SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.LOAN_OFFICER ORDER BY NAME";

            LoanOfficerGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLoanOfficerDataGridView", "Kredit mütəxəssislərinin siyahısı yüklənmədi.");
            DeleteLoanOfficerBarButton.Enabled = EditLoanOfficerBarButton.Enabled = (LoanOfficerGridView.RowCount > 0);
        }

        private void LoadCarBrandDataGridView()
        {
            string s = "SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BRANDS ORDER BY ORDER_ID";

            BrandGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarBrandDataGridView", "Markalar cədvələ yüklənmədi.");

            if (BrandGridView.RowCount > 0)
            {
                DeleteCarBrandBarButton.Enabled = EditCarBrandBarButton.Enabled = true;
                if (BrandGridView.FocusedRowHandle == 0)
                    UpCarBrandBarButton.Enabled = false;

                DownCarBrandBarButton.Enabled = (BrandGridView.RowCount > 1);
            }
            else
                DeleteCarBrandBarButton.Enabled = EditCarBrandBarButton.Enabled = UpCarBrandBarButton.Enabled = DownCarBrandBarButton.Enabled = false;
        }


        private void LoadCarModelDataGridView()
        {
            string s = "SELECT 1 SS,M.ID,B.NAME BNAME,M.NAME MNAME,M.NOTE,M.USED_USER_ID FROM CRS_USER.CAR_BRANDS B,CRS_USER.CAR_MODELS M WHERE B.ID = M.BRAND_ID ORDER BY M.ORDER_ID";

            CarModelGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarModelDataGridView", "Modellər cədvələ yüklənmədi.");


            if (CarModelGridView.RowCount > 0)
            {
                DeleteCarModelBarButton.Enabled = true;
                EditCarModelBarButton.Enabled = true;

                if (CarModelGridView.FocusedRowHandle == 0)
                    UpCarModelBarButton.Enabled = false;
                if (CarModelGridView.RowCount > 1)
                    DownCarModelBarButton.Enabled = true;
                else
                    DownCarModelBarButton.Enabled = false;
            }
            else
                DeleteCarModelBarButton.Enabled = EditCarModelBarButton.Enabled = UpCarModelBarButton.Enabled = DownCarModelBarButton.Enabled = false;

        }

        private void LoadCarTypeDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_TYPES ORDER BY ORDER_ID";

            CarTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarTypeDataGridView", "Avtomonillərin tipləri cədvələ yüklənmədi.");



            if (CarTypeGridView.RowCount > 0)
            {
                DeleteCarTypeBarButton.Enabled = true;
                EditCarTypeBarButton.Enabled = true;
                if (CarTypeGridView.FocusedRowHandle == 0)
                    UpCarTypeBarButton.Enabled = false;

                DownCarTypeBarButton.Enabled = (CarTypeGridView.RowCount > 1);
            }
            else
                DeleteCarTypeBarButton.Enabled = EditCarTypeBarButton.Enabled = UpCarTypeBarButton.Enabled = DownCarTypeBarButton.Enabled = false;



        }

        private void LoadCarColorDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_COLORS ORDER BY ORDER_ID";

            CarColorGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarColorDataGridView", "Avtomonillərin rəngləri cədvələ yüklənmədi.");


            if (CarColorGridView.RowCount > 0)
            {
                DeleteCarColorBarButton.Enabled = true;
                EditCarColorBarButton.Enabled = true;
                if (CarColorGridView.FocusedRowHandle == 0)
                    UpCarColorBarButton.Enabled = false;
                if (CarColorGridView.RowCount > 1)
                    DownCarColorBarButton.Enabled = true;
                else
                    DownCarColorBarButton.Enabled = false;
            }
            else
                DeleteCarColorBarButton.Enabled = EditCarColorBarButton.Enabled = UpCarColorBarButton.Enabled = DownCarColorBarButton.Enabled = false;


        }

        private void LoadCarBanTypeDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BAN_TYPES ORDER BY ORDER_ID";

            CarBanTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarBanTypeDataGridView");


            if (CarBanTypeGridView.RowCount > 0)
            {
                DeleteBanTypeBarButton.Enabled = true;
                EditBanTypeBarButton.Enabled = true;
                if (CarBanTypeGridView.FocusedRowHandle == 0)
                    UpBanTypeBarButton.Enabled = false;
                DownBanTypeBarButton.Enabled = (CarBanTypeGridView.RowCount > 1);
            }
            else
                DeleteBanTypeBarButton.Enabled = EditBanTypeBarButton.Enabled = UpBanTypeBarButton.Enabled = DownBanTypeBarButton.Enabled = false;

        }

        //private void LoadAppointmentDataGridView()
        //{
        //    string s = $@"SELECT CA.ID,
        //                       CA.NAME,
        //                       OT.TYPE_AZ OPERATION_TYPE_NAME,
        //                       CA.ORDER_ID,
        //                       CA.NOTE,
        //                       CA.USED_USER_ID
        //                  FROM CRS_USER.CASH_APPOINTMENTS CA, CRS_USER.OPERATION_TYPES OT
        //                 WHERE CA.OPERATION_TYPE_ID = OT.ID
        //                ORDER BY CA.NAME";

        //    AppointmentGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAppointmentDataGridView", "Kassa təyinatlarının siyahısı yüklənmədi.");
        //    DeleteAppointmentBarButton.Enabled = EditAppointmentBarButton.Enabled = (AppointmentGridView.RowCount > 0);
        //}

        private void BackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (BackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadPhoneDescriptionDataGridView();
                        break;
                    case 1:
                        LoadStatusDataGridView();
                        break;
                    case 2:
                        LoadCardSeriesDataGridView();
                        break;
                    case 3:
                        LoadCardIssuingDataGridView();
                        break;
                    case 4:
                        LoadBirthplaceDataGridView();
                        break;
                    case 5:
                        LoadCreditStatus();
                        break;
                    //case 6:
                    //    LoadPawnTypeDataGridView();
                    //    break;
                    case 7:
                        LoadCurrencyDataGridView();
                        break;
                    
                    case 8:
                        LoadCountriesDataGridView();
                        break;
                    case 9:
                        LoadKindshipRateDataGridView();
                        break;
                    case 10:
                        LoadContractEvaluateDataGridView();
                        break;
                    case 11:
                        LoadDocumentType();
                        break;
                    case 12:
                        LoadLoanOfficerDataGridView();
                        break;
                    case 13:
                        LoadEyebrowsDataGridView();
                        break;
                    case 14:
                        LoadCarColorDataGridView();
                        break;
                    
                    case 15:
                        LoadCarModelDataGridView();
                        break;
                        
                        //case 15:
                    //    LoadAppointmentDataGridView();
                    //    break;
                    case 16:
                        LoadCarTypeDataGridView();
                        break;
                    
                    case 17:
                        LoadCarBanTypeDataGridView();
                        break;
                }
                switch (CreditParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCreditTypeDataGridView();
                        break;
                }

                switch (PawnParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadEyebrowsDataGridView();
                        break;
                }
                switch (LizingParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCarBrandDataGridView();
                        break;

                }
            }
        }

        void RefreshPhoneDescription()
        {
            LoadPhoneDescriptionDataGridView();
        }

        private void LoadFPhoneDescriptionAddEdit(string transaction, string descriptionID)
        {
            topindex = PhoneGridView.TopRowIndex;
            old_row_id = PhoneGridView.FocusedRowHandle;
            Forms.Dictionaries.FPhoneDescriptionAddEdit fp = new Forms.Dictionaries.FPhoneDescriptionAddEdit();
            fp.TransactionName = transaction;
            fp.DescriptionID = descriptionID;
            fp.RefreshPhoneDescriptionDataGridView += new Forms.Dictionaries.FPhoneDescriptionAddEdit.DoEvent(RefreshPhoneDescription);
            fp.ShowDialog();
            PhoneGridView.TopRowIndex = topindex;
            PhoneGridView.FocusedRowHandle = old_row_id;
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneDescriptionAddEdit("INSERT", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneDescriptionAddEdit("EDIT", PhoneID);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled)
                LoadFPhoneDescriptionAddEdit("EDIT", PhoneID);
        }

        private void DeletePhoneDescription()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT (*) FROM (SELECT PHONE_DESCRIPTION_ID FROM CRS_USER.PHONES WHERE PHONE_DESCRIPTION_ID = " + PhoneID + " UNION ALL " +
                                                                       "SELECT DISTINCT PHONE_DESCRIPTION_ID FROM CRS_USER_TEMP.PHONES_TEMP WHERE PHONE_DESCRIPTION_ID = " + PhoneID + ")");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təsviri silmək istəyirsiniz?", "Təsvirin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.PHONE_DESCRIPTIONS WHERE ID = " + PhoneID, "Təsvir silinmədi.", this.Name + "/DeletePhoneDescription");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş təsvir bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int PhoneUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.PHONE_DESCRIPTIONS WHERE ID = " + PhoneID);
            if (PhoneUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != PhoneUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PhoneUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş təsvir hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün bu təsviri silmək olmaz.", "Seçilmiş təsvirin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeletePhoneDescription();
            }
            else
                DeletePhoneDescription();
            LoadPhoneDescriptionDataGridView();
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
                PhoneID = row["ID"].ToString();
        }

        void RefreshStatus()
        {
            LoadStatusDataGridView();
        }

        private void LoadFStatusEdit(string StatusID)
        {
            topindex = StatusGridView.TopRowIndex;
            old_row_id = StatusGridView.FocusedRowHandle;
            Forms.Dictionaries.FStatusEdit fse = new Forms.Dictionaries.FStatusEdit();
            fse.StatusID = StatusID;
            fse.RefreshStatusDataGridView += new Forms.Dictionaries.FStatusEdit.DoEvent(RefreshStatus);
            fse.ShowDialog();
            StatusGridView.TopRowIndex = topindex;
            StatusGridView.FocusedRowHandle = old_row_id;
        }

        private void EditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFStatusEdit(StatusID);
        }

        private void StatusGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = StatusGridView.GetFocusedDataRow();
            if (row != null)
                StatusID = row["ID"].ToString();
        }

        private void RefreshStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadStatusDataGridView();
        }

        private void StatusGridView_DoubleClick(object sender, EventArgs e)
        {
            LoadFStatusEdit(StatusID);
        }

        private void CardSeriesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CardSeriesGridView, CardSeriesPopupMenu, e);
        }

        private void CardSeriesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CardSeriesGridView.GetFocusedDataRow();
            if (row != null)
            {
                SeriesID = row["ID"].ToString();
                UpCardSeriesBarButton.Enabled = !(CardSeriesGridView.FocusedRowHandle == 0);
                DownCardSeriesBarButton.Enabled = !(CardSeriesGridView.FocusedRowHandle == CardSeriesGridView.RowCount - 1);
            }
        }

        void RefreshSeries()
        {
            LoadCardSeriesDataGridView();
        }

        private void LoadFCardSeriesAddEdit(string transaction, string SeriedID)
        {
            topindex = CardSeriesGridView.TopRowIndex;
            old_row_id = CardSeriesGridView.FocusedRowHandle;
            Forms.Dictionaries.FCardSeriesAddEdit fse = new Forms.Dictionaries.FCardSeriesAddEdit();
            fse.TransactionName = transaction;
            fse.SeriesID = SeriedID;
            fse.RefreshCardSeriesDataGridView += new Forms.Dictionaries.FCardSeriesAddEdit.DoEvent(RefreshSeries);
            fse.ShowDialog();
            CardSeriesGridView.TopRowIndex = topindex;
            CardSeriesGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardSeriesAddEdit("INSERT", null);
        }

        private void EditCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardSeriesAddEdit("EDIT", SeriesID);
        }

        private void CardSeriesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCardSeriesBarButton.Enabled)
                LoadFCardSeriesAddEdit("EDIT", SeriesID);
        }

        private void RefreshCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCardSeriesDataGridView();
        }

        private void PhoneGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(PhoneGridView, e);
        }

        private void StatusGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(StatusGridView, e);
        }

        private void CardSeriesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CardSeriesGridView, e);
        }

        private void CardIssuingGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CardIssuingGridView, CardIssuingPopupMenu, e);
        }

        private void CardIssuingGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CardIssuingGridView.GetFocusedDataRow();
            if (row != null)
            {
                IssuingID = row["ID"].ToString();
                UpCardIssuingBarButton.Enabled = !(CardIssuingGridView.FocusedRowHandle == 0);
                DownCardIssuingBarButton.Enabled = !(CardIssuingGridView.FocusedRowHandle == CardIssuingGridView.RowCount - 1);
            }
        }

        void RefreshIssuing()
        {
            LoadCardIssuingDataGridView();
        }

        private void LoadFCardIssuingAddEdit(string transaction, string IssuingID)
        {
            topindex = CardIssuingGridView.TopRowIndex;
            old_row_id = CardIssuingGridView.FocusedRowHandle;
            Forms.Dictionaries.FCardIssuingAddEdit fse = new Forms.Dictionaries.FCardIssuingAddEdit();
            fse.TransactionName = transaction;
            fse.IssuingID = IssuingID;
            fse.RefreshCardIssuingDataGridView += new Forms.Dictionaries.FCardIssuingAddEdit.DoEvent(RefreshIssuing);
            fse.ShowDialog();
            CardIssuingGridView.TopRowIndex = topindex;
            CardIssuingGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardIssuingAddEdit("INSERT", null);
        }

        private void EditCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardIssuingAddEdit("EDIT", IssuingID);
        }

        private void CardIssuingGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCardIssuingBarButton.Enabled)
                LoadFCardIssuingAddEdit("EDIT", IssuingID);
        }

        private void RefreshCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCardIssuingDataGridView();
        }

        private void FDictionaries_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionType == "E")
            {
                this.RefreshList(BackstageViewControl.SelectedTabIndex);
            }
        }

        private void BirthplaceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BirthplaceGridView, BirthplacePopupMenu, e);
        }

        private void BirthplaceGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BirthplaceGridView.GetFocusedDataRow();
            if (row != null)
            {
                BirthplaceID = row["ID"].ToString();
                UpBirthplaceBarButton.Enabled = !(BirthplaceGridView.FocusedRowHandle == 0);
                DownBirthplaceBarButton.Enabled = !(BirthplaceGridView.FocusedRowHandle == BirthplaceGridView.RowCount - 1);
            }
        }

        void RefreshBirthplace()
        {
            LoadBirthplaceDataGridView();
        }

        private void LoadFBirthplaceAddEdit(string transaction, string BirthplaceID)
        {
            topindex = BirthplaceGridView.TopRowIndex;
            old_row_id = BirthplaceGridView.FocusedRowHandle;
            Forms.Dictionaries.FBirthplaceAddEdit fse = new Forms.Dictionaries.FBirthplaceAddEdit();
            fse.TransactionName = transaction;
            fse.BirthplaceID = BirthplaceID;
            fse.RefreshBirthplaceDataGridView += new Forms.Dictionaries.FBirthplaceAddEdit.DoEvent(RefreshBirthplace);
            fse.ShowDialog();
            BirthplaceGridView.TopRowIndex = topindex;
            BirthplaceGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBirthplaceAddEdit("INSERT", null);
        }

        private void EditBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBirthplaceAddEdit("EDIT", BirthplaceID);
        }

        private void RefreshBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBirthplaceDataGridView();
        }

        private void BirthplaceGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBirthplaceBarButton.Enabled)
                LoadFBirthplaceAddEdit("EDIT", BirthplaceID);
        }

        private void CreditTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CreditTypeGridView, CreditTypePopupMenu, e);
        }

        void RefreshCreditType()
        {
            LoadCreditTypeDataGridView();
        }

        private void LoadFCreditTypeAddEdit(string transaction, string TypeID)
        {
            topindex = CreditTypeGridView.TopRowIndex;
            old_row_id = CreditTypeGridView.FocusedRowHandle;
            Forms.Dictionaries.FCreditTypeAddEdit fse = new Forms.Dictionaries.FCreditTypeAddEdit();
            fse.TransactionName = transaction;
            fse.TypeID = TypeID;
            fse.RefreshCreditTypeDataGridView += new Forms.Dictionaries.FCreditTypeAddEdit.DoEvent(RefreshCreditType);
            fse.ShowDialog();
            CreditTypeGridView.TopRowIndex = topindex;
            CreditTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCreditTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditTypeAddEdit("INSERT", null);
        }

        private void CreditTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CreditTypeGridView.GetFocusedDataRow();
            if (row != null)
                CreditTypeID = row["ID"].ToString();
        }

        private void EditCreditTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditTypeAddEdit("EDIT", CreditTypeID);
        }

        private void CreditTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCreditTypeBarButton.Enabled)
                LoadFCreditTypeAddEdit("EDIT", CreditTypeID);
        }

        private void RefreshCreditTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCreditTypeDataGridView();
        }

        private void CreditTypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
            GlobalProcedures.GridRowCellStyleForBlock(CreditTypeGridView, e);
        }

        private void CreditNameGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {

        }


        private void LoadFPawnTypeAddEdit(string transaction, string NameID)
        {
            topindex = PawnTypeGridView.TopRowIndex;
            old_row_id = PawnTypeGridView.FocusedRowHandle;
            Dictionaries.FPawnTypeAddEdit fse = new Dictionaries.FPawnTypeAddEdit();
            fse.TransactionName = transaction;
            fse.PawnTypeID = NameID;
            fse.RefreshDataGridView += new Dictionaries.FPawnTypeAddEdit.DoEvent(LoadPawnTypeDataGridView);
            fse.ShowDialog();
            PawnTypeGridView.TopRowIndex = topindex;
            PawnTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void CurrencyGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CurrencyGridView, CurrencyPopupMenu, e);
        }

        private void CurrencyGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CurrencyGridView, e);
        }

        private void CurrencyGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CurrencyGridView.GetFocusedDataRow();
            if (row != null)
            {
                CurrencyID = row["ID"].ToString();
                UpCurrencyBarButton.Enabled = !(CurrencyGridView.FocusedRowHandle == 0);
                DownCurrencyBarButton.Enabled = !(CurrencyGridView.FocusedRowHandle == CurrencyGridView.RowCount - 1);
            }
        }

        void RefreshCurrency()
        {
            LoadCurrencyDataGridView();
        }

        private void LoadFCurrencyAddEdit(string transaction, string CurrencyID)
        {
            topindex = CurrencyGridView.TopRowIndex;
            old_row_id = CurrencyGridView.FocusedRowHandle;
            Forms.Dictionaries.FCurrencyAddEdit fp = new Forms.Dictionaries.FCurrencyAddEdit();
            fp.TransactionName = transaction;
            fp.CurrencyID = CurrencyID;
            fp.RefreshCurrencyDataGridView += new Forms.Dictionaries.FCurrencyAddEdit.DoEvent(RefreshCurrency);
            fp.ShowDialog();
            CurrencyGridView.TopRowIndex = topindex;
            CurrencyGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCurrencyAddEdit("INSERT", null);
        }

        private void EditCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCurrencyAddEdit("EDIT", CurrencyID);
        }

        private void RefreshCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCurrencyDataGridView();
        }

        private void CurrencyGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCurrencyBarButton.Enabled)
                LoadFCurrencyAddEdit("EDIT", CurrencyID);
        }

        private void DeleteCardSeries()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT (*) FROM (SELECT DISTINCT CARD_SERIES_ID FROM CRS_USER.CUSTOMER_CARDS WHERE CARD_SERIES_ID = " + SeriesID + " UNION ALL " +
                                                                       "SELECT DISTINCT CARD_SERIES_ID FROM CRS_USER.SELLERS WHERE CARD_SERIES_ID = " + SeriesID + ")");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş seriyanı silmək istəyirsiniz?", "Seriyanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CARD_SERIES WHERE ID = " + SeriesID, "Şəxsiyyəti təsdiq edən sənədin seriyası silinmədi.", this.Name + "/DeleteCardSeries");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş seriya bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int SeriesUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CARD_SERIES WHERE ID = " + SeriesID);
            if (SeriesUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != SeriesUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == SeriesUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş seriya hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün bu seriyanı silmək olmaz.", "Seçilmiş seriyanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteCardSeries();
            }
            else
                DeleteCardSeries();
            LoadCardSeriesDataGridView();
        }


        private void DeleteCardIssuing()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT (*) FROM (SELECT DISTINCT CARD_ISSUING_ID FROM CRS_USER.CUSTOMER_CARDS WHERE CARD_ISSUING_ID = " + IssuingID + " UNION ALL " +
                                                                       "SELECT DISTINCT CARD_ISSUING_ID FROM CRS_USER.SELLERS WHERE CARD_ISSUING_ID = " + IssuingID + ")");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş orqanı silmək istəyirsiniz?", "Orqanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CARD_ISSUING WHERE ID = " + IssuingID, "Şəxsiyyəti təsdiq edən sənədi verən orqan silinmədi.", this.Name + "/DeleteCardIssuing");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş orqan bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int IssuingUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CARD_ISSUING WHERE ID = " + IssuingID);
            if (IssuingUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != IssuingUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == IssuingUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş orqan hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün bu orqanı silmək olmaz.", "Seçilmiş orqanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteCardIssuing();
            }
            else
                DeleteCardIssuing();
            LoadCardIssuingDataGridView();
        }

        private void CardIssuingGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CardIssuingGridView, e);
        }

        private void BirthplaceGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(BirthplaceGridView, e);
        }

        private void CreditNameGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(PawnTypeGridView, e);
        }

        private void DeleteBirthplace()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT (*) FROM (SELECT DISTINCT BIRTHPLACE_ID FROM CRS_USER.CUSTOMERS WHERE BIRTHPLACE_ID = " + BirthplaceID + " UNION ALL " +
                                                                       "SELECT DISTINCT BIRTHPLACE_ID FROM CRS_USER.SELLERS WHERE BIRTHPLACE_ID = " + BirthplaceID + ")");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş doğum yerini silmək istəyirsiniz?", "Doğum yerinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.BIRTHPLACE WHERE ID = " + BirthplaceID, "Doğum yeri silinmədi.", this.Name + "/DeleteBirthplace");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş doğum yeri bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int BirthplaceUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.BIRTHPLACE WHERE ID = " + BirthplaceID);
            if (BirthplaceUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != BirthplaceUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BirthplaceUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş doğum yeri hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş doğum yerinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteBirthplace();
            }
            else
                DeleteBirthplace();
            LoadBirthplaceDataGridView();
        }

        private void DeleteCurrency()
        {
            if (Convert.ToInt32(CurrencyID) != 1)
            {
                int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT CURRENCY_ID FROM CRS_USER.CONTRACTS UNION ALL SELECT DISTINCT CURRENCY_ID FROM CRS_USER.CURRENCY_RATES) WHERE CURRENCY_ID = " + CurrencyID);
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş valyutanı silmək istəyirsiniz?", "Valyutanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CURRENCY WHERE ID = " + CurrencyID, "Valyuta silinmədi.", this.Name + "/DeleteCurrency");
                }
                else
                    XtraMessageBox.Show("Seçilmiş valyuta müqavilələrdə və ya valyuta məzənnələrində istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                XtraMessageBox.Show("AZN baza valyuta olduğu üçün bu valyutanı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int CurrencyUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CURRENCY WHERE ID = " + CurrencyID);
            if (CurrencyUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != CurrencyUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CurrencyUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş valyuta hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş valyutanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteCurrency();
            }
            else
                DeleteCurrency();
            LoadCurrencyDataGridView();
        }

        private void DeleteCreditType()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CREDIT_TYPE_ID = {CreditTypeID}", this.Name + "/DeleteCreditType");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş parametri silmək istəyirsiniz?", "Lizinq növünün parametrinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CREDIT_TYPE WHERE ID = {CreditTypeID}", "Lizinq növünün parametri silinmədi.", this.Name + "/DeleteCreditType");
            }
            else
                XtraMessageBox.Show("Seçilmiş parametr bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCreditTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int CreditTypeUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CREDIT_TYPE WHERE ID = {CreditTypeID}", this.Name + "/DeleteCreditTypeBarButton_ItemClick");
            if (CreditTypeUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != CreditTypeUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CreditTypeUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş parametr hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş parametrin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteCreditType();
            }
            else
                DeleteCreditType();
            LoadCreditTypeDataGridView();
        }

        private void LoadCreditClass()
        {
            string s = "SELECT ID,NAME,CODE,USED_USER_ID,ORDER_ID FROM CRS_USER.CREDIT_CLASS ORDER BY ORDER_ID";

            CreditClassGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditClass", "Kreditin təsnifatının siyahısı yüklənmədi.");

            if (CreditClassGridView.RowCount > 0)
            {
                DeleteCreditClassBarButton.Enabled =
                    EditCreditClassBarButton.Enabled = true;
                UpCreditClassBarButton.Enabled = !(CreditClassGridView.FocusedRowHandle == 0);
                DownCreditClassBarButton.Enabled = (CreditClassGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditClassBarButton.Enabled =
                    EditCreditClassBarButton.Enabled =
                    UpCreditClassBarButton.Enabled =
                    DownCreditClassBarButton.Enabled = false;
            }
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDescriptionDataGridView();
        }

        private void UpCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CARD_ISSUING", IssuingID, "up", out orderid);
            LoadCardIssuingDataGridView();
            CardIssuingGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCardIssuingBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CARD_ISSUING", IssuingID, "down", out orderid);
            LoadCardIssuingDataGridView();
            CardIssuingGridView.FocusedRowHandle = orderid - 1;
        }

        private void UpCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CARD_SERIES", SeriesID, "up", out orderid);
            LoadCardSeriesDataGridView();
            CardSeriesGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CARD_SERIES", SeriesID, "down", out orderid);
            LoadCardSeriesDataGridView();
            CardSeriesGridView.FocusedRowHandle = orderid - 1;
        }

        private void CountriesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CountriesGridView, CountriesPopupMenu, e);
        }

        private void CountriesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CountriesGridView.GetFocusedDataRow();
            if (row != null)
            {
                CountryID = row["ID"].ToString();
                UpCountryBarButton.Enabled = !(CountriesGridView.FocusedRowHandle == 0);
                DownCountryBarButton.Enabled = !(CountriesGridView.FocusedRowHandle == CountriesGridView.RowCount - 1);
            }
        }

        void RefreshCountries()
        {
            LoadCountriesDataGridView();
        }

        private void LoadFCountryAddEdit(string transaction, string countryid)
        {
            topindex = CountriesGridView.TopRowIndex;
            old_row_id = CountriesGridView.FocusedRowHandle;
            Forms.Dictionaries.FCountryAddEdit fp = new Forms.Dictionaries.FCountryAddEdit();
            fp.TransactionName = transaction;
            fp.CountryID = countryid;
            fp.RefreshCountriesDataGridView += new Forms.Dictionaries.FCountryAddEdit.DoEvent(RefreshCountries);
            fp.ShowDialog();
            CountriesGridView.TopRowIndex = topindex;
            CountriesGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCountryAddEdit("INSERT", null);
        }

        private void EditCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCountryAddEdit("EDIT", CountryID);
        }

        private void RefreshCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCountriesDataGridView();
        }

        private void CountriesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCountryBarButton.Enabled)
                LoadFCountryAddEdit("EDIT", CountryID);
        }

        private void UpCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("COUNTRIES", CountryID, "up", out orderid);
            LoadCountriesDataGridView();
            CountriesGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("COUNTRIES", CountryID, "down", out orderid);
            LoadCountriesDataGridView();
            CountriesGridView.FocusedRowHandle = orderid - 1;
        }

        private void CountriesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CountriesGridView, e);
        }

        private void DeleteCountry()
        {
            if (Convert.ToInt32(CountryID) == 4)
                XtraMessageBox.Show("Sistem Azərbaycanın üzərində qurulduğu üçün bu ölkəni silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int CountryUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.COUNTRIES WHERE ID = " + CountryID);
                if (CountryUsedUserID <= 0)
                {
                    int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT COUNTRY_ID FROM CRS_USER.PHONES UNION ALL SELECT COUNTRY_ID FROM CRS_USER_TEMP.PHONES_TEMP) WHERE COUNTRY_ID = " + CountryID);
                    if (a == 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş ölkəni silmək istəyirsiniz?", "Ölkənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.COUNTRIES WHERE ID = " + CountryID, "Ölkə silinmədi.", this.Name + "/DeleteCountry");
                        }
                    }
                    else
                        XtraMessageBox.Show("Seçilmiş ölkə bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CountryUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş ölkə hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş ölkənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DeleteCountryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCountry();
            LoadCountriesDataGridView();
        }

        private void DownBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("BIRTHPLACE", BirthplaceID, "down", out orderid);
            LoadBirthplaceDataGridView();
            BirthplaceGridView.FocusedRowHandle = orderid - 1;
        }

        private void UpBirthplaceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("BIRTHPLACE", BirthplaceID, "up", out orderid);
            LoadBirthplaceDataGridView();
            BirthplaceGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteBank()
        {
            int BankUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.BANKS WHERE ID = {BankID}", this.Name + "/DeleteBank");
            if (BankUsedUserID <= 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.BANK_OPERATIONS WHERE BANK_ID = {BankID}", this.Name + "/DeleteBank");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş bankı silmək istəyirsiniz?", "Bankın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANKS WHERE ID = {BankID}", "Bank silinmədi.", this.Name + "/DeleteBank");
                    }
                }
                else
                    XtraMessageBox.Show("Seçilmiş bank bazada istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BankUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş bank hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş bankın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreditTypeGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CALC_DATE", "Center", e);
        }

        private void DeleteCreditClass()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CREDIT_CLASS WHERE ID = {creditClassID}", this.Name + "/DeleteCreditClass");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {creditClassID}", this.Name + "/DeleteCreditClass");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kredit təsnifatını silmək istəyirsiniz?", "Kredit təsnifatının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CREDIT_CLASS WHERE ID = {creditClassID}", "Kredit təsnifatı silinmədi.", this.Name + "/DeleteCreditClass");
                }
                else
                    XtraMessageBox.Show("Seçilmiş kredit təsnifatı bazada istifadə olunduğu üçün bu təsnifatı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş kredit təsnifatı hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş kredit təsnifatının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PhoneGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeletePhoneBarButton.Enabled = EditPhoneBarButton.Enabled = (PhoneGridView.RowCount > 0);
        }

        private void StatusGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditStatusBarButton.Enabled = (StatusGridView.RowCount > 0);
        }

        private void CardSeriesGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CardSeriesGridView.RowCount > 0)
            {
                DeleteCardSeriesBarButton.Enabled =
                    EditCardSeriesBarButton.Enabled = true;
                if (CardSeriesGridView.FocusedRowHandle == 0)
                    UpCardSeriesBarButton.Enabled = false;
                DownCardSeriesBarButton.Enabled = (CardSeriesGridView.RowCount > 1);
            }
            else
            {
                DeleteCardSeriesBarButton.Enabled =
                    EditCardSeriesBarButton.Enabled =
                    UpCardSeriesBarButton.Enabled =
                    DownCardSeriesBarButton.Enabled = false;
            }
        }

        private void CardIssuingGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CardIssuingGridView.RowCount > 0)
            {
                DeleteCardIssuingBarButton.Enabled =
                    EditCardIssuingBarButton.Enabled = true;
                if (CardIssuingGridView.FocusedRowHandle == 0)
                    UpCardIssuingBarButton.Enabled = false;
                DownCardIssuingBarButton.Enabled = (CardIssuingGridView.RowCount > 1);
            }
            else
            {
                DeleteCardIssuingBarButton.Enabled =
                    EditCardIssuingBarButton.Enabled =
                    UpCardIssuingBarButton.Enabled =
                    DownCardIssuingBarButton.Enabled = false;
            }
        }

        private void BirthplaceGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (BirthplaceGridView.RowCount > 0)
            {
                DeleteBirthplaceBarButton.Enabled =
                    EditBirthplaceBarButton.Enabled = true;
                if (BirthplaceGridView.FocusedRowHandle == 0)
                    UpBirthplaceBarButton.Enabled = false;
                DownBirthplaceBarButton.Enabled = (BirthplaceGridView.RowCount > 1);

            }
            else
            {
                DeleteBirthplaceBarButton.Enabled =
                    EditBirthplaceBarButton.Enabled =
                    UpBirthplaceBarButton.Enabled =
                    DownBirthplaceBarButton.Enabled = false;
            }
        }

        private void CreditTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteCreditTypeBarButton.Enabled = EditCreditTypeBarButton.Enabled = (CreditTypeGridView.RowCount > 0);
        }

        private void CurrencyGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CurrencyGridView.RowCount > 0)
            {
                DeleteCurrencyBarButton.Enabled =
                    EditCurrencyBarButton.Enabled = true;
                if (CurrencyGridView.FocusedRowHandle == 0)
                    UpCurrencyBarButton.Enabled = false;
                DownCurrencyBarButton.Enabled = (CurrencyGridView.RowCount > 1);
            }
            else
            {
                DeleteCurrencyBarButton.Enabled =
                    EditCurrencyBarButton.Enabled =
                    UpCurrencyBarButton.Enabled =
                    DownCurrencyBarButton.Enabled = false;
            }
        }

        private void BirthplaceSortBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.SortTableByName("CRS_USER.BIRTHPLACE");
            LoadBirthplaceDataGridView();
        }

        private void SortCardIssuingButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.SortTableByName("CRS_USER.CARD_ISSUING");
            LoadCardIssuingDataGridView();
        }

        private void SortCardSeriesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.SortTableByName("CRS_USER.CARD_SERIES");
            LoadCardSeriesDataGridView();
        }

        private void KindshipRateGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(KindshipRateGridView, KindshipRatePopupMenu, e);
        }

        private void RefreshKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadKindshipRateDataGridView();
        }

        private void RefreshContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractEvaluateDataGridView();
        }

        void RefreshContractEvaluate()
        {
            LoadContractEvaluateDataGridView();
        }

        private void LoadFContractEvaluateAddEdit(string transaction, string contractEvaluateID)
        {
            topindex = ContractEvaluateGridView.TopRowIndex;
            old_row_id = ContractEvaluateGridView.FocusedRowHandle;
            Dictionaries.FContractEvaluateAddEdit fp = new Dictionaries.FContractEvaluateAddEdit();
            fp.TransactionName = transaction;
            fp.ContractEvaluateID = contractEvaluateID;
            fp.RefreshContractEvaluateDataGridView += new Dictionaries.FContractEvaluateAddEdit.DoEvent(RefreshContractEvaluate);
            fp.ShowDialog();
            ContractEvaluateGridView.TopRowIndex = topindex;
            ContractEvaluateGridView.FocusedRowHandle = old_row_id;
        }

        private void NewContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractEvaluateAddEdit("INSERT", null);
        }

        private void ContractEvaluateGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractEvaluateGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractEvaluateID = row["ID"].ToString();
                contractEvaluateIsDeleted = int.Parse(row["IS_DELETED"].ToString());
                UpContractEvaluateBarButton.Enabled = !(ContractEvaluateGridView.FocusedRowHandle == 0);
                DownContractEvaluateBarButton.Enabled = !(ContractEvaluateGridView.FocusedRowHandle == ContractEvaluateGridView.RowCount - 1);
            }
        }

        private void ContractEvaluateGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractEvaluateGridView, ContractEvaluatePopupMenu, e);
        }

        private void DeleteContractEvaluate()
        {
            if (contractEvaluateIsDeleted == 1)
            {
                GlobalProcedures.ShowWarningMessage("Seçilmiş qiymətləndirmə baza əhəmiyyətli olduğu üçün silinə bilməz.");
                return;
            }

            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CONTRACT_EVALUATE_ID = {ContractEvaluateID}");
            if (a == 0 || int.Parse(ContractEvaluateID) != 1)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qiymətləndirməni silmək istəyirsiniz?", "Qiymətləndirmənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CONTRACT_EVALUATE WHERE ID = {ContractEvaluateID}", "Qiymətləndirmə silinmədi.", this.Name + "/DeleteContractEvaluate");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş qiymətləndirmə bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int EvaluateUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CONTRACT_EVALUATE WHERE ID = {ContractEvaluateID}");
            if (EvaluateUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != EvaluateUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == EvaluateUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş qiymətləndirmə hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş qiymətləndirmənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteContractEvaluate();
            }
            else
                DeleteContractEvaluate();
            LoadContractEvaluateDataGridView();
        }

        private void UpContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CONTRACT_EVALUATE", ContractEvaluateID, "up", out orderid);
            LoadContractEvaluateDataGridView();
            ContractEvaluateGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CONTRACT_EVALUATE", ContractEvaluateID, "down", out orderid);
            LoadContractEvaluateDataGridView();
            ContractEvaluateGridView.FocusedRowHandle = orderid - 1;
        }

        private void ContractEvaluateGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditContractEvaluateBarButton.Enabled)
                LoadFContractEvaluateAddEdit("EDIT", ContractEvaluateID);
        }

        private void ContractEvaluateGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (ContractEvaluateGridView.RowCount > 0)
            {
                DeleteContractEvaluateBarButton.Enabled =
                    EditContractEvaluateBarButton.Enabled = true;
                if (ContractEvaluateGridView.FocusedRowHandle == 0)
                    UpContractEvaluateBarButton.Enabled = false;
                DownContractEvaluateBarButton.Enabled = (ContractEvaluateGridView.RowCount > 1);

            }
            else
            {
                DeleteContractEvaluateBarButton.Enabled =
                    EditContractEvaluateBarButton.Enabled =
                    UpContractEvaluateBarButton.Enabled =
                    DownContractEvaluateBarButton.Enabled = false;
            }
        }

        void RefreshKindShip()
        {
            LoadKindshipRateDataGridView();
        }

        private void LoadFKindShipAddEdit(string transaction, string kindShipID)
        {
            topindex = KindshipRateGridView.TopRowIndex;
            old_row_id = KindshipRateGridView.FocusedRowHandle;
            Dictionaries.FKindShipAddEdit fp = new Dictionaries.FKindShipAddEdit();
            fp.TransactionName = transaction;
            fp.KindShipID = kindShipID;
            fp.RefreshKindShipDataGridView += new Dictionaries.FKindShipAddEdit.DoEvent(RefreshKindShip);
            fp.ShowDialog();
            KindshipRateGridView.TopRowIndex = topindex;
            KindshipRateGridView.FocusedRowHandle = old_row_id;
        }

        private void NewKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFKindShipAddEdit("INSERT", null);
        }

        private void KindshipRateGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = KindshipRateGridView.GetFocusedDataRow();
            if (row != null)
            {
                KindShipID = row["ID"].ToString();
                UpKindshipRateBarButton.Enabled = !(KindshipRateGridView.FocusedRowHandle == 0);
                DownKindshipRateBarButton.Enabled = !(KindshipRateGridView.FocusedRowHandle == KindshipRateGridView.RowCount - 1);
            }
        }

        private void DeleteKindShip()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT KINDSHIP_RATE_ID FROM CRS_USER.PHONES UNION ALL SELECT KINDSHIP_RATE_ID FROM CRS_USER_TEMP.PHONES_TEMP) WHERE KINDSHIP_RATE_ID = {KindShipID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qohumluq dərəcəsini silmək istəyirsiniz?", "Qohumluq dərəcəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.KINDSHIP_RATE WHERE ID = {KindShipID}", "Qohumluq dərəcəsi silinmədi.", this.Name + "/DeleteKindShip");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş qohumluq dərəcəsi bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DownKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("KINDSHIP_RATE", KindShipID, "down", out orderid);
            LoadKindshipRateDataGridView();
            KindshipRateGridView.FocusedRowHandle = orderid - 1;
        }

        private void UpKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("KINDSHIP_RATE", KindShipID, "up", out orderid);
            LoadKindshipRateDataGridView();
            KindshipRateGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int KindShipUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.KINDSHIP_RATE WHERE ID = {KindShipID}");
            if (KindShipUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != KindShipUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == KindShipUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş qohumluq dərəcəsi hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş qohumluq dərəcəsinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteKindShip();
            }
            else
                DeleteKindShip();
            LoadKindshipRateDataGridView();
        }

        private void PawnTypeGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, PawnType_SS, e);
        }

        private void NewPawnTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPawnTypeAddEdit("INSERT", null);
        }

        private void EditPawnTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPawnTypeAddEdit("EDIT", PawnTypeID);
        }

        private void DeletePawnTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.PAWN WHERE PAWN_TYPE_ID = " + PawnTypeID) == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş əşyanı silmək istəyirsiniz?", "Əşyanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.PAWN_TYPE WHERE ID = " + PawnTypeID, "Əşya silinmədi.", this.Name + "/DeletePawnTypeBarButton_ItemClick");
            }
            else
                XtraMessageBox.Show("Seçilmiş əşya girovlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LoadPawnTypeDataGridView();
        }

        private void RefreshPawnTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPawnTypeDataGridView();
        }

        private void PawnTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPawnTypeBarButton.Enabled)
                LoadFPawnTypeAddEdit("EDIT", PawnTypeID);
        }

        private void PawnTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PawnTypeGridView, PawnTypePopupMenu, e);
        }

        private void PawnTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeletePawnTypeBarButton.Enabled = EditPawnTypeBarButton.Enabled = (PawnTypeGridView.RowCount > 0);
        }

        private void PawnTypeGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(PawnType_SS, "Center", e);
        }

        //private void CreditClassGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        //{
        //    GlobalProcedures.GenerateAutoRowNumber(sender, SS, e);
        //}

        private void CreditClassGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CreditClassGridView.GetFocusedDataRow();
            if (row != null)
            {
                creditClassID = row["ID"].ToString();
                UpCreditClassBarButton.Enabled = !(CreditClassGridView.FocusedRowHandle == 0);
                DownCreditClassBarButton.Enabled = !(CreditClassGridView.FocusedRowHandle == CreditClassGridView.RowCount - 1);
            }
        }

        private void CreditClassGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CreditClassGridView, CreditClassPopupMenu, e);
        }

        private void NewAppraiserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAppraiserAddEdit("INSERT", null);
        }

        private void EditAppraiserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAppraiserAddEdit("EDIT", AppraiserID);
        }

        private void AppraiserGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditAppraiserBarButton.Enabled)
                LoadFAppraiserAddEdit("EDIT", AppraiserID);
        }

        private void RefreshAppraiserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAppraiserDataGridView();
        }

        private void AppraiserGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Appraiser_SS, e);
        }

        private void DeleteAppraiserBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.APPRAISER WHERE ID = {AppraiserID}");
            if (UsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş qiymətləndirici hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş qiymətləndirici hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteAppraiser();
            }
            else
                DeleteAppraiser();
            LoadAppraiserDataGridView();
        }

        void DeleteAppraiser()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE APPRAISER_ID = {AppraiserID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qiymətləndiricini silmək istəyirsiniz?", "Qiymətləndiricinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.APPRAISER WHERE ID = {AppraiserID}", "Qiymətləndirici silinmədi.", this.Name + "/DeleteAppraiser");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş qiymətləndirici müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AppraiserGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AppraiserGridView, AppraiserPopupMenu, e);
        }

        private void AppraiserGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AppraiserGridView.GetFocusedDataRow();
            if (row != null)
                AppraiserID = row["ID"].ToString();
        }

        private void AppraiserGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteAppraiserBarButton.Enabled = EditAppraiserBarButton.Enabled = (AppraiserGridView.RowCount > 0);
        }

        private void EyebrowsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Eyebrows_SS, e);
        }

        private void EyebrowsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(EyebrowsGridView, EyebrowsPopupMenu, e);
        }

        private void RefreshEyebrowsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadEyebrowsDataGridView();
        }

        private void NewEyebrowsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFEyebrowsAddEdit("INSERT", null);
        }

        private void EyebrowsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = EyebrowsGridView.GetFocusedDataRow();
            if (row != null)
                EyebrowsID = row["ID"].ToString();
        }

        private void EditEyebrowsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFEyebrowsAddEdit("EDIT", EyebrowsID);
        }

        private void EyebrowsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditEyebrowsBarButton.Enabled)
                LoadFEyebrowsAddEdit("EDIT", EyebrowsID);
        }

        private void LoanOfficerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = LoanOfficerGridView.GetFocusedDataRow();
            if (row != null)
                LoanOfficerID = row["ID"].ToString();
        }

        private void NewLoanOfficerBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFLoanOfficerAddEdit("INSERT", null);
        }

        private void LoadFLoanOfficerAddEdit(string transaction, string id)
        {
            topindex = LoanOfficerGridView.TopRowIndex;
            old_row_id = LoanOfficerGridView.FocusedRowHandle;
            Dictionaries.FLoanOfficerAddEdit fa = new Dictionaries.FLoanOfficerAddEdit();
            fa.TransactionName = transaction;
            fa.ID = id;
            fa.RefreshDataGridView += new Dictionaries.FLoanOfficerAddEdit.DoEvent(LoadLoanOfficerDataGridView);
            fa.ShowDialog();
            LoanOfficerGridView.TopRowIndex = topindex;
            LoanOfficerGridView.FocusedRowHandle = old_row_id;
        }

        private void EditLoanOfficerBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFLoanOfficerAddEdit("EDIT", LoanOfficerID);
        }

        private void LoanOfficerGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditLoanOfficerBarButton.Enabled)
                LoadFLoanOfficerAddEdit("EDIT", LoanOfficerID);
        }

        private void LoanOfficerGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, LoanOfficer_SS, e);
        }

        private void LoanOfficerGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(LoanOfficerGridView, LoanOfficerPopupMenu, e);
        }

        private void DeleteLoanOfficerBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.LOAN_OFFICER WHERE ID = {LoanOfficerID}");
            if (UsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş mütəxəssiz hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş mütəxəssisin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteLoanOfficer();
            }
            else
                DeleteLoanOfficer();
            LoadLoanOfficerDataGridView();
        }

        void DeleteLoanOfficer()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE LOAN_OFFICER_ID = {LoanOfficerID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kredit mütəxəssisini silmək istəyirsiniz?", "Kredit mütəxəssisinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.LOAN_OFFICER WHERE ID = {LoanOfficerID}", "Kredit mütəxəssisi silinmədi.", this.Name + "/DeleteLoanOfficer");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş kredit mütəxəssisi müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //private void NewAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadFAppointmentAddEdit("INSERT", null);
        //}

        //private void LoadFAppointmentAddEdit(string transaction, string id)
        //{
        //    topindex = AppointmentGridView.TopRowIndex;
        //    old_row_id = AppointmentGridView.FocusedRowHandle;
        //    Dictionaries.FAppointmentAddEdit fap = new Dictionaries.FAppointmentAddEdit();
        //    fap.TransactionName = transaction;
        //    fap.ID = id;
        //    fap.RefreshDataGridView += new Dictionaries.FAppointmentAddEdit.DoEvent(LoadAppointmentDataGridView);
        //    fap.ShowDialog();
        //    AppointmentGridView.TopRowIndex = topindex;
        //    AppointmentGridView.FocusedRowHandle = old_row_id;
        //}

        //private void EditAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadFAppointmentAddEdit("EDIT", AppointmentID);
        //}

        //private void RefreshAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadAppointmentDataGridView();
        //}

        //private void AppointmentGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        //{
        //    DataRow row = AppointmentGridView.GetFocusedDataRow();
        //    if (row != null)
        //        AppointmentID = row["ID"].ToString();
        //}

        //private void AppointmentGridView_MouseUp(object sender, MouseEventArgs e)
        //{
        //    GlobalProcedures.GridMouseUpForPopupMenu(AppointmentGridView, AppointmentPopupMenu, e);
        //}

        //private void AppointmentGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        //{
        //    GlobalProcedures.GenerateAutoRowNumber(sender, Appointment_SS, e);
        //}

        //private void DeleteAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CASH_APPOINTMENTS WHERE ID = {AppointmentID}");
        //    if (UsedUserID >= 0)
        //    {
        //        if (GlobalVariables.V_UserID != UsedUserID)
        //        {
        //            string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
        //            XtraMessageBox.Show("Seçilmiş təyinat hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş təyinatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //            DeleteAppointment();
        //    }
        //    else
        //        DeleteAppointment();
        //    LoadAppointmentDataGridView();
        //}

        void DeleteAppointment()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CASH_OPERATIONS WHERE CASH_APPOİNTMENT_ID = {AppointmentID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təyinatı silmək istəyirsiniz?", "Təyinatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CASH_APPOINTMENTS WHERE ID = {AppointmentID}", "Tətinatın silinmədi.", this.Name + "/DeleteAppointment");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş təyinat kassada istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void CreditParametrBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (CreditParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCreditTypeDataGridView();
                        break;
                    case 1:
                        LoadTypeCredit();
                        break;
                    case 2:
                        LoadCreditPurpose();
                        break;
                    case 3:
                        LoadCreditClass();
                        break;
                    case 4:
                        LoadCreditStatus();
                        break;
                    case 5:
                        LoadCollateralType();
                        break;
                }
}
}
    private void PawnParametrBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if(FormStatus)
            {
                switch (PawnParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadEyebrowsDataGridView();
                        break;
                    case 1:
                        LoadGoldType();
                        break;
                    case 2:
                        LoadPawnTypeDataGridView();
                        break;
                    case 3:
                        LoadAppraiserDataGridView();
                        break;
                }
            }
        }
        private void LizingParametrBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (LizingParametrBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCarBrandDataGridView();
                        break;
                    case 1:
                        LoadCarModelDataGridView();
                        break;
                    case 2:
                        LoadCarTypeDataGridView();
                        break;
                    case 3:
                        LoadCarColorDataGridView();
                        break;
                    case 4:
                        LoadCarBanTypeDataGridView();
                        break;
                }
            }
        }

        

    private void BrandGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BrandGridView, CarBrandPopupMenu, e);
        }

        private void RefreshCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarBrandDataGridView();
        }

        private void RefreshCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarModelDataGridView();
        }

        private void RefreshCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarTypeDataGridView();
        }

        private void RefreshBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarBanTypeDataGridView();
        }

        private void RefreshCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarColorDataGridView();
        }
        private void backstageViewTabItem12_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {

        }

        private void backstageViewClientControl18_Load(object sender, EventArgs e)
        {

        }

        private void BrandGridControl_Click(object sender, EventArgs e)
        {

        }

        private void CarModelGridControl_Click(object sender, EventArgs e)
        {

        }

        //private void AppointmentGridView_DoubleClick(object sender, EventArgs e)
        //{
        //    if (EditAppointmentBarButton.Enabled)
        //        LoadFAppointmentAddEdit("EDIT", AppointmentID);
        //}               

        private void RefreshLoanOfficerBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLoanOfficerDataGridView();
        }

        private void EyebrowsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {

        }

        private void DeleteEyebrowsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.EYEBROWS_TYPE WHERE ID = {EyebrowsID}");
            if (UsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş qaşın növü hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş qaşın növünün hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteEyebrows();
            }
            else
                DeleteEyebrows();
            LoadEyebrowsDataGridView();
        }

        private void CurrencyGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Currency_SS, e);
        }        

        void DeleteEyebrows()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE EYEBROWS_TYPE_ID = {EyebrowsID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş qaşın növünü silmək istəyirsiniz?", "Qaşın növünün silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.EYEBROWS_TYPE WHERE ID = {EyebrowsID}", "Qaşın növü silinmədi.", this.Name + "/DeleteEyebrows");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş qaşın növü müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LoadFEyebrowsAddEdit(string transaction, string id)
        {
            topindex = EyebrowsGridView.TopRowIndex;
            old_row_id = EyebrowsGridView.FocusedRowHandle;
            Dictionaries.FEyebrowsAddEdit fa = new Dictionaries.FEyebrowsAddEdit();
            fa.TransactionName = transaction;
            fa.ID = id;
            fa.RefreshDataGridView += new Dictionaries.FEyebrowsAddEdit.DoEvent(LoadEyebrowsDataGridView);
            fa.ShowDialog();
            EyebrowsGridView.TopRowIndex = topindex;
            EyebrowsGridView.FocusedRowHandle = old_row_id;
        }

        private void LoadFAppraiserAddEdit(string transaction, string id)
        {
            topindex = AppraiserGridView.TopRowIndex;
            old_row_id = AppraiserGridView.FocusedRowHandle;
            Dictionaries.FAppraiserAddEdit fa = new Dictionaries.FAppraiserAddEdit();
            fa.TransactionName = transaction;
            fa.ID = id;
            fa.RefreshDataGridView += new Dictionaries.FAppraiserAddEdit.DoEvent(LoadAppraiserDataGridView);
            fa.ShowDialog();
            AppraiserGridView.TopRowIndex = topindex;
            AppraiserGridView.FocusedRowHandle = old_row_id;
        }

        private void CreditClassGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CreditClassGridView.RowCount > 0)
            {
                DeleteCreditClassBarButton.Enabled =
                    EditCreditClassBarButton.Enabled = true;
                if (CreditClassGridView.FocusedRowHandle == 0)
                    UpCreditClassBarButton.Enabled = false;
                DownCreditClassBarButton.Enabled = (CreditClassGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditClassBarButton.Enabled =
                    EditCreditClassBarButton.Enabled =
                    UpCreditClassBarButton.Enabled =
                    DownCreditClassBarButton.Enabled = false;
            }
        }

        private void CreditClassGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CreditClassGridView, e);
        }

        private void NewCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditClassAddEdit("INSERT", null);
        }

        private void LoadFCreditClassAddEdit(string transaction, string id)
        {
            topindex = CreditClassGridView.TopRowIndex;
            old_row_id = CreditClassGridView.FocusedRowHandle;
            Dictionaries.FCreditClassAddEdit fg = new Dictionaries.FCreditClassAddEdit();
            fg.TransactionName = transaction;
            fg.CreditClassID = id;
            fg.RefreshDataGridView += new Dictionaries.FCreditClassAddEdit.DoEvent(LoadCreditClass);
            fg.ShowDialog();
            CreditClassGridView.TopRowIndex = topindex;
            CreditClassGridView.FocusedRowHandle = old_row_id;
        }

        private void EditCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditClassAddEdit("EDIT", creditClassID);
        }

        private void CreditClassGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCreditClassBarButton.Enabled)
                LoadFCreditClassAddEdit("EDIT", creditClassID);
        }

        private void RefreshCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCreditClass();
        }

        private void UpCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CREDIT_CLASS", creditClassID, "up", out orderid);
            LoadCreditClass();
            CreditClassGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CREDIT_CLASS", creditClassID, "down", out orderid);
            LoadCreditClass();
            CreditClassGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteCreditClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCreditClass();
            LoadCreditClass();
        }

        private void PawnTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PawnTypeGridView.GetFocusedDataRow();
            if (row != null)
                PawnTypeID = row["ID"].ToString();
        }

        private void KindshipRateGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditKindshipRateBarButton.Enabled)
                LoadFKindShipAddEdit("EDIT", KindShipID);
        }

        private void EditKindshipRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFKindShipAddEdit("EDIT", KindShipID);
        }

        private void EditContractEvaluateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFContractEvaluateAddEdit("EDIT", ContractEvaluateID);
        }

        private void CountriesGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CountriesGridView.RowCount > 0)
            {
                DeleteCountryBarButton.Enabled =
                    EditCountryBarButton.Enabled = true;
                if (CountriesGridView.FocusedRowHandle == 0)
                    UpCountryBarButton.Enabled = false;
                DownCountryBarButton.Enabled = (CountriesGridView.RowCount > 1);
            }
            else
            {
                DeleteCountryBarButton.Enabled =
                    EditCountryBarButton.Enabled =
                    UpCountryBarButton.Enabled =
                    DownCountryBarButton.Enabled = false;
            }
        }

        private void UpCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CURRENCY", CurrencyID, "up", out orderid);
            LoadCurrencyDataGridView();
            CurrencyGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCurrencyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CURRENCY", CurrencyID, "down", out orderid);
            LoadCurrencyDataGridView();
            CurrencyGridView.FocusedRowHandle = orderid - 1;
        }

        private void BrandGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BrandGridView.GetFocusedDataRow();
            if (row != null)
            {
                CarBrandID = row["ID"].ToString();
                UpCarBrandBarButton.Enabled = !(BrandGridView.FocusedRowHandle == 0);
                DownCarBrandBarButton.Enabled = !(BrandGridView.FocusedRowHandle == BrandGridView.RowCount - 1);
            }
        }


        void RefreshCarBrand()
        {
            LoadCarBrandDataGridView();
        }

        private void LoadFCarBrandAddEdit(string transaction, string BrandID)
        {
            topindex = BrandGridView.TopRowIndex;
            old_row_id = BrandGridView.FocusedRowHandle;
            Forms.Dictionaries.FCarBrandAddEdit fp = new Forms.Dictionaries.FCarBrandAddEdit();
            fp.TransactionName = transaction;
            fp.BrandID = BrandID;
            fp.RefreshCarBrandDataGridView += new Forms.Dictionaries.FCarBrandAddEdit.DoEvent(RefreshCarBrand);
            fp.ShowDialog();
            BrandGridView.TopRowIndex = topindex;
            BrandGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBrandAddEdit("INSERT", null);
        }

        private void EditCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBrandAddEdit("EDIT", CarBrandID);
        }

       

        private void BrandGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarBrandBarButton.Enabled)
                LoadFCarBrandAddEdit("EDIT", CarBrandID);
        }


        private void DeleteBrand()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE BRAND_ID = " + CarBrandID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş markanı silmək istəyirsiniz?", "Markanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CAR_BRANDS WHERE ID = " + CarBrandID, "Marka silinmədi.", this.Name + "/DeleteBrand");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş marka bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int BrandUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CAR_BRANDS WHERE ID = " + CarBrandID);
            if (BrandUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != BrandUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == BrandUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş marka hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş markanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteBrand();
            }
            else
                DeleteBrand();
            LoadCarBrandDataGridView();
        }

        private void DownCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_BRANDS", CarBrandID, "down", out orderid);
            LoadCarBrandDataGridView();
            BrandGridView.FocusedRowHandle = orderid - 1;
        }

        private void UpCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_BRANDS", CarBrandID, "up", out orderid);
            LoadCarBrandDataGridView();
            BrandGridView.FocusedRowHandle = orderid - 1;
        }

        private void BrandGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (BrandGridView.RowCount > 0)
            {
                DeleteCarBrandBarButton.Enabled =
                    EditCarBrandBarButton.Enabled = true;
                if (BrandGridView.FocusedRowHandle == 0)
                    UpCarBrandBarButton.Enabled = false;
                DownCarBrandBarButton.Enabled = (BrandGridView.RowCount > 1);
            }
            else
            {
                DeleteCarBrandBarButton.Enabled =
                    EditCarBrandBarButton.Enabled =
                    UpCarBrandBarButton.Enabled =
                    DownCarBrandBarButton.Enabled = false;
            }
        }

        //////////
        private void CarTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CarTypeGridView, CarTypePopupMenu, e);
        }

        private void CarTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CarTypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                CarTypeID = row["ID"].ToString();
                UpCarTypeBarButton.Enabled = !(CarTypeGridView.FocusedRowHandle == 0);
                DownCarTypeBarButton.Enabled = !(CarTypeGridView.FocusedRowHandle == CarTypeGridView.RowCount - 1);
            }
        }

        void RefreshCarType()
        {
            LoadCarTypeDataGridView();
        }

        private void LoadFCarTypeAddEdit(string transaction, string TypeID)
        {
            topindex = CarTypeGridView.TopRowIndex;
            old_row_id = CarTypeGridView.FocusedRowHandle;
            Forms.Dictionaries.FCarTypeAddEdit fp = new Forms.Dictionaries.FCarTypeAddEdit();
            fp.TransactionName = transaction;
            fp.TypeID = TypeID;
            fp.RefreshCarTypeDataGridView += new Forms.Dictionaries.FCarTypeAddEdit.DoEvent(RefreshCarType);
            fp.ShowDialog();
            CarTypeGridView.TopRowIndex = topindex;
            CarTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarTypeAddEdit("INSERT", null);
        }

        private void EditCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarTypeAddEdit("EDIT", CarTypeID);
        }

        private void CarTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarTypeBarButton.Enabled)
                LoadFCarTypeAddEdit("EDIT", CarTypeID);
        }

       

        private void CarTypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarTypeGridView, e);
        }

        private void DeleteType()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE TYPE_ID = " + CarTypeID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş tipi silmək istəyirsiniz?", "Tipin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CAR_TYPES WHERE ID = " + CarTypeID, "Tip silinmədi.", this.Name + "/DeleteType");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş tip bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int TypeUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CAR_TYPES WHERE ID = " + CarTypeID);
            if (TypeUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != TypeUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TypeUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş tip hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş tipin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteType();
            }
            else
                DeleteType();
            LoadCarTypeDataGridView();
        }

        private void UpCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_TYPES", CarTypeID, "up", out orderid);
            LoadCarTypeDataGridView();
            CarTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_TYPES", CarTypeID, "down", out orderid);
            LoadCarTypeDataGridView();
            CarTypeGridView.FocusedRowHandle = orderid - 1;
        }
        private void CarTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CarTypeGridView.RowCount > 0)
            {
                DeleteCarTypeBarButton.Enabled =
                    EditCarTypeBarButton.Enabled = true;
                if (CarTypeGridView.FocusedRowHandle == 0)
                    UpCarTypeBarButton.Enabled = false;
                DownCarTypeBarButton.Enabled = (CarTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteCarTypeBarButton.Enabled =
                    EditCarTypeBarButton.Enabled =
                    UpCarTypeBarButton.Enabled =
                    DownCarTypeBarButton.Enabled = false;
            }
        }
        //////////////////////////////
        private void CarColorGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CarColorGridView, CarColorPopupMenu, e);
        }

        private void CarColorGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarColorGridView, e);
        }

        private void CarColorGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CarColorGridView.GetFocusedDataRow();
            if (row != null)
            {
                CarColorID = row["ID"].ToString();
                UpCarColorBarButton.Enabled = !(CarColorGridView.FocusedRowHandle == 0);
                DownCarColorBarButton.Enabled = !(CarColorGridView.FocusedRowHandle == CarColorGridView.RowCount - 1);
            }
        }

        void RefreshCarColor()
        {
            LoadCarColorDataGridView();
        }

        private void LoadFCarColorAddEdit(string transaction, string ColorID)
        {
            topindex = CarColorGridView.TopRowIndex;
            old_row_id = CarColorGridView.FocusedRowHandle;
            Forms.Dictionaries.FCarColorAddEdit fp = new Forms.Dictionaries.FCarColorAddEdit();
            fp.TransactionName = transaction;
            fp.ColorID = ColorID;
            fp.RefreshCarColorDataGridView += new Forms.Dictionaries.FCarColorAddEdit.DoEvent(RefreshCarColor);
            fp.ShowDialog();
            CarColorGridView.TopRowIndex = topindex;
            CarColorGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarColorAddEdit("INSERT", null);
        }

        private void EditCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarColorAddEdit("EDIT", CarColorID);
        }

       

        private void CarColorGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarColorBarButton.Enabled)
                LoadFCarColorAddEdit("EDIT", CarColorID);
        }



        private void DeleteColor()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE COLOR_ID = " + CarColorID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş rəngi silmək istəyirsiniz?", "Rəngin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CAR_COLORS WHERE ID = " + CarColorID, "Rəng silinmədi.", this.Name + "/DeleteColor");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş rəng bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int ColorUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CAR_COLORS WHERE ID = " + CarColorID);
            if (ColorUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != ColorUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ColorUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş rəng hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş rəngin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteColor();
            }
            else
                DeleteColor();
            LoadCarColorDataGridView();
        }

        private void CarTypeGridControl_Click(object sender, EventArgs e)
        {

        }

        private void UpCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_COLORS", CarColorID, "up", out orderid);
            LoadCarColorDataGridView();
            CarColorGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_COLORS", CarColorID, "down", out orderid);
            LoadCarColorDataGridView();
            CarColorGridView.FocusedRowHandle = orderid - 1;
        }

        private void CarColorGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CarColorGridView.RowCount > 0)
            {
                DeleteCarColorBarButton.Enabled =
                    EditCarColorBarButton.Enabled = true;
                if (CarColorGridView.FocusedRowHandle == 0)
                    UpCarColorBarButton.Enabled = false;
                DownCarColorBarButton.Enabled = (CarColorGridView.RowCount > 1);
            }
            else
            {
                DeleteCarColorBarButton.Enabled =
                    EditCarColorBarButton.Enabled =
                    UpCarColorBarButton.Enabled =
                    DownCarColorBarButton.Enabled = false;
            }
        }
        ////////////////////////
        private void CarModelGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CarModelGridView, CarModelPopupMenu, e);
        }

        private void CarModelGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarModelGridView, e);
        }

        private void CarModelGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CarModelGridView.GetFocusedDataRow();
            if (row != null)
            {
                CarModelID = row["ID"].ToString();
                UpCarModelBarButton.Enabled = !(CarModelGridView.FocusedRowHandle == 0);
                DownCarModelBarButton.Enabled = !(CarModelGridView.FocusedRowHandle == CarModelGridView.RowCount - 1);
            }
        }

        void RefreshCarModel()
        {
            LoadCarModelDataGridView();
        }

        private void LoadFCarModelAddEdit(string transaction, string ModelID)
        {
            topindex = CarModelGridView.TopRowIndex;
            old_row_id = CarModelGridView.FocusedRowHandle;
            Forms.Dictionaries.FCarModelAddEdit fp = new Forms.Dictionaries.FCarModelAddEdit();
            fp.TransactionName = transaction;
            fp.ModelID = ModelID;
            fp.RefreshCarModelDataGridView += new Forms.Dictionaries.FCarModelAddEdit.DoEvent(RefreshCarModel);
            fp.ShowDialog();
            CarModelGridView.TopRowIndex = topindex;
            CarModelGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarModelAddEdit("INSERT", null);
        }

        private void EditCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarModelAddEdit("EDIT", CarModelID);
        }

        private void CarModelGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarModelBarButton.Enabled)
                LoadFCarModelAddEdit("EDIT", CarModelID);
        }

       

        private void DeleteModel()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE MODEL_ID = " + CarModelID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş modeli silmək istəyirsiniz?", "Modelin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CAR_MODELS WHERE ID = " + CarModelID, "Model silinmədi.", this.Name + "/DeleteModel");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş model bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int ModelUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CAR_MODELS WHERE ID = " + CarModelID);
            if (ModelUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != ModelUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ModelUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş model hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş modelin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteModel();
            }
            else
                DeleteModel();
            LoadCarModelDataGridView();
        }

        private void UpCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_MODELS", CarModelID, "up", out orderid);
            LoadCarModelDataGridView();
            CarModelGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_MODELS", CarModelID, "down", out orderid);
            LoadCarModelDataGridView();
            CarModelGridView.FocusedRowHandle = orderid - 1;
        }


        private void CarModelGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CarModelGridView.RowCount > 0)
            {
                DeleteCarModelBarButton.Enabled =
                    EditCarModelBarButton.Enabled = true;
                if (CarModelGridView.FocusedRowHandle == 0)
                    UpCarModelBarButton.Enabled = false;
                DownCarModelBarButton.Enabled = (CarModelGridView.RowCount > 1);
            }
            else
            {
                DeleteCarModelBarButton.Enabled =
                    EditCarModelBarButton.Enabled =
                    UpCarModelBarButton.Enabled =
                    DownCarModelBarButton.Enabled = false;
            }
        }

        ///////////////////////
        private void BanTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CarBanTypeGridView, BanTypePopupMenu, e);
        }

        private void CarBanTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CarBanTypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                CarBanTypeID = row["ID"].ToString();
                UpBanTypeBarButton.Enabled = !(CarBanTypeGridView.FocusedRowHandle == 0);
                DownBanTypeBarButton.Enabled = !(CarBanTypeGridView.FocusedRowHandle == CarBanTypeGridView.RowCount - 1);
            }
        }

        void RefreshCarBanType()
        {
            LoadCarBanTypeDataGridView();
        }

        private void LoadFCarBanTypeAddEdit(string transaction, string TypeID)
        {
            topindex = CarBanTypeGridView.TopRowIndex;
            old_row_id = CarBanTypeGridView.FocusedRowHandle;
            Forms.Dictionaries.FCarBanTypeAddEdit fp = new Forms.Dictionaries.FCarBanTypeAddEdit();
            fp.TransactionName = transaction;
            fp.TypeID = TypeID;
            fp.RefreshCarBanTypeDataGridView += new Forms.Dictionaries.FCarBanTypeAddEdit.DoEvent(RefreshCarBanType);
            fp.ShowDialog();
            CarBanTypeGridView.TopRowIndex = topindex;
            CarBanTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBanTypeAddEdit("INSERT", null);
        }

        private void EditBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBanTypeAddEdit("EDIT", CarBanTypeID);
        }

        private void CarBanTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBanTypeBarButton.Enabled)
                LoadFCarBanTypeAddEdit("EDIT", CarBanTypeID);
        }

        private void DeleteCarBanType()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE BAN_TYPE_ID = {CarBanTypeID}", this.Name + "/DeleteCarBanType");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş ban tipini silmək istəyirsiniz?", "Ban tipinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CAR_BAN_TYPES WHERE ID = {CarBanTypeID}", "Ban tipi silinmədi.", this.Name + "/DeleteCarBanType");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş ban tipi bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int CarBanTypeUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CAR_BAN_TYPES WHERE ID = {CarBanTypeID}", this.Name + "/DeleteBanTypeBarButton_ItemClick");
            if (CarBanTypeUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != CarBanTypeUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CarBanTypeUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş ban tipi hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş ban tipinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteCarBanType();
            }
            else
                DeleteCarBanType();
            LoadCarBanTypeDataGridView();
        }

       

        private void CarBanTypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarBanTypeGridView, e);
        }



        private void UpBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_BAN_TYPES", CarBanTypeID, "up", out orderid);
            LoadCarBanTypeDataGridView();
            CarBanTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CAR_BAN_TYPES", CarBanTypeID, "down", out orderid);
            LoadCarBanTypeDataGridView();
            CarBanTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void CarBanTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CarBanTypeGridView.RowCount > 0)
            {
                DeleteBanTypeBarButton.Enabled =
                    EditBanTypeBarButton.Enabled = true;
                if (CarBanTypeGridView.FocusedRowHandle == 0)
                    UpBanTypeBarButton.Enabled = false;
                DownBanTypeBarButton.Enabled = (CarBanTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteBanTypeBarButton.Enabled =
                    EditBanTypeBarButton.Enabled =
                    UpBanTypeBarButton.Enabled =
                    DownBanTypeBarButton.Enabled = false;
            }
        }
        ///////
        ///
        //private void NewCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    OpenFCreditPurposeAddEdit("INSERT", null);
        //}
        //private void OpenFCreditPurposeAddEdit(string transaction, int? purposeID)
        //{
        //    old_row_num = CreditPurposeGridView.FocusedRowHandle;
        //    topindex = CreditPurposeGridView.TopRowIndex;
        //    FCreditPurposeAddEdit fc = new FCreditPurposeAddEdit();
        //    fc.TransactionName = transaction;
        //    fc.PurposeID = purposeID;
        //    fc.RefreshList += new FCreditPurposeAddEdit.DoEvent(LoadCreditPurpose);
        //    fc.ShowDialog();
        //    CreditPurposeGridView.FocusedRowHandle = old_row_num;
        //    CreditPurposeGridView.TopRowIndex = topindex;
        //}
        //private void LoadCreditPurpose()
        //{
        //    List<CreditPurpose> lstPurpose = SqliteDataAccess.LoadCreditPurpose(null);
        //    CreditPurposeGridControl.DataSource = lstPurpose;
        //    EditCreditPurposeBarButton.Enabled = DeleteCreditPurposeBarButton.Enabled = (lstPurpose.Count > 0);
        //}

        ///////////////////
        ///

        //void RefreshCredit1Class()
        //{
        //    LoadCredit1ClassDataGridView();
        //}


        //private void Credit1ClassGridView_ColumnFilterChanged(object sender, EventArgs e)
        //{
        //    if (Credit1ClassGridView.RowCount > 0)
        //    {
        //        DeleteCredit1ClassBarButton.Enabled =
        //            EditCredit1ClassBarButton.Enabled = true;
        //        if (Credit1ClassGridView.FocusedRowHandle == 0)
        //            UpCredit1ClassBarButton.Enabled = false;
        //        DownCredit1ClassBarButton.Enabled = (Credit1ClassGridView.RowCount > 1);
        //    }
        //    else
        //    {
        //        DeleteCredit1ClassBarButton.Enabled =
        //            EditCredit1ClassBarButton.Enabled =
        //            UpCredit1ClassBarButton.Enabled =
        //            DownCredit1ClassBarButton.Enabled = false;
        //    }
        //}


        //private void Credit1ClassGridView_DoubleClick(object sender, EventArgs e)
        //{
        //    if (EditCredit1ClassBarButton.Enabled)
        //        LoadFCredit1ClassAddEdit("EDIT", Credit1ClassID);
        //}




        private void NewCredit1ClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCredit1ClassAddEdit("INSERT", null);
        }

        private void EditCredit1ClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCredit1ClassAddEdit("EDIT", Credit1ClassID);
        }



        //private void DeleteCredit1ClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    int Credit1ClassUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CAR_COLORS WHERE ID = " + Credit1ClassID);
        //    if (Credit1ClassUsedUserID >= 0)
        //    {
        //        if (GlobalVariables.V_UserID != Credit1ClassUsedUserID)
        //        {
        //            string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == Credit1ClassUsedUserID).FULLNAME;
        //            XtraMessageBox.Show("Seçilmiş rəng hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş rəngin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //            DeleteCredit1Class();
        //    }
        //    else
        //        DeleteCredit1Class();
        //    LoadCredit1ClassDataGridView();
        //}



        //private void RefreshCredit1ClassBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    LoadCredit1ClassDataGridView();
        //}




        //private void Credit1ClassGridView_MouseUp(object sender, MouseEventArgs e)
        //{
        //    GlobalProcedures.GridMouseUpForPopupMenu(Credit1ClassGridView, Credit1ClassPopupMenu, e);
        //}










        private void LoadFCredit1ClassAddEdit(string transaction, string Credit1ClassID)
        {
            //topindex = Credit1ClassGridView.TopRowIndex;
            //old_row_id = Credit1ClassGridView.FocusedRowHandle;
            //Forms.Dictionaries.FCredit1ClassAddEdit fp = new Forms.Dictionaries.FCredit1ClassAddEdit();
            //fp.TransactionName = transaction;
            //fp.Credit1ClassID = Credit1ClassID;
            //fp.RefreshCredit1ClassDataGridView += new Forms.Dictionaries.FCredit1ClassAddEdit.DoEvent(RefreshCredit1Class);
            //fp.ShowDialog();
            //Credit1ClassGridView.TopRowIndex = topindex;
            //Credit1ClassGridView.FocusedRowHandle = old_row_id;
        }



        //private void Credit1ClassGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        //{
        //    DataRow row = Credit1ClassGridView.GetFocusedDataRow();
        //    if (row != null)
        //    {
        //        Credit1ClassID = row["ID"].ToString();
        //        UpCredit1ClassBarButton.Enabled = !(Credit1ClassGridView.FocusedRowHandle == 0);
        //        DownCredit1ClassBarButton.Enabled = !(Credit1ClassGridView.FocusedRowHandle == Credit1ClassGridView.RowCount - 1);
        //    }
        //}

            

        //private void LoadCredit1ClassDataGridView()
        //{
        //    string s = "SELECT 1 SS,ID,NAME,CODE FROM CRS_USER.CREDIT_CLASS ORDER BY ORDER_ID";

        //    Credit1ClassGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCredit1ClassDataGridView", " Kreditlərin təsnifatı cədvələ yüklənmədi.");


        //    if (Credit1ClassGridView.RowCount > 0)
        //    {
        //        DeleteCredit1ClassBarButton.Enabled = true;
        //        EditCredit1ClassBarButton.Enabled = true;
        //        if (Credit1ClassGridView.FocusedRowHandle == 0)
        //            UpCredit1ClassBarButton.Enabled = false;
        //        if (Credit1ClassGridView.RowCount > 1)
        //            DownCredit1ClassBarButton.Enabled = true;
        //        else
        //            DownCredit1ClassBarButton.Enabled = false;
        //    }
        //    else
        //        DeleteCredit1ClassBarButton.Enabled = EditCredit1ClassBarButton.Enabled = UpCredit1ClassBarButton.Enabled = DownCredit1ClassBarButton.Enabled = false;


        //}



        /////////////////////////
        private void DeleteCredit1Class()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.HOSTAGE_CAR WHERE COLOR_ID = " + Credit1ClassID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş rəngi silmək istəyirsiniz?", "Rəngin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CAR_COLORS WHERE ID = " + Credit1ClassID, "Rəng silinmədi.", this.Name + "/DeleteCredit1Class");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş rəng bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /////////////////////////////////////
        /////////////////////////////////
        ///////////////////////////
        /////GoldType
        ///

        private void LoadGoldType()
        {
            string s = "SELECT ID,NAME,NOTE,COEFFICIENT,AMOUNT,USED_USER_ID,ORDER_ID FROM CRS_USER.GOLD_TYPE ORDER BY ORDER_ID";

            GoldTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadGoldType", "Qızılın əyyarlarının siyahısı yüklənmədi.");

            if (GoldTypeGridView.RowCount > 0)
            {
                DeleteGoldTypeBarButton.Enabled =
                    EditGoldTypeBarButton.Enabled = true;
                UpGoldTypeBarButton.Enabled = !(GoldTypeGridView.FocusedRowHandle == 0);
                DownGoldTypeBarButton.Enabled = (GoldTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteGoldTypeBarButton.Enabled =
                    EditGoldTypeBarButton.Enabled =
                    UpGoldTypeBarButton.Enabled =
                    DownGoldTypeBarButton.Enabled = false;
            }
        }


        private void DeleteGoldType()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.GOLD_TYPE WHERE ID = {goldTypeID}", this.Name + "/DeleteGoldType");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {goldTypeID}", this.Name + "/DeleteGoldType");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş əyyarı silmək istəyirsiniz?", "Əyyarın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.GOLD_TYPE WHERE ID = {goldTypeID}", "Əyyar silinmədi.", this.Name + "/DeleteGoldType");
                }
                else
                    XtraMessageBox.Show("Seçilmiş əyyar girovlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş əyyar hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş əyyarın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void GoldTypeGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, GoldType_SS, e);
        }

        private void GoldTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = GoldTypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                goldTypeID = row["ID"].ToString();
                UpGoldTypeBarButton.Enabled = !(GoldTypeGridView.FocusedRowHandle == 0);
                DownGoldTypeBarButton.Enabled = !(GoldTypeGridView.FocusedRowHandle == GoldTypeGridView.RowCount - 1);
            }
        }

        private void GoldTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(GoldTypeGridView, GoldTypePopupMenu, e);
        }



        private void GoldTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (GoldTypeGridView.RowCount > 0)
            {
                DeleteGoldTypeBarButton.Enabled =
                    EditGoldTypeBarButton.Enabled = true;
                if (GoldTypeGridView.FocusedRowHandle == 0)
                    UpGoldTypeBarButton.Enabled = false;
                DownGoldTypeBarButton.Enabled = (GoldTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteGoldTypeBarButton.Enabled =
                    EditGoldTypeBarButton.Enabled =
                    UpGoldTypeBarButton.Enabled =
                    DownGoldTypeBarButton.Enabled = false;
            }
        }


        private void GoldTypeGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(GoldTypeGridView, e);
        }

        private void NewGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFGoldTypeAddEdit("INSERT", null);
        }

        private void LoadFGoldTypeAddEdit(string transaction, string id)
        {
            topindex = GoldTypeGridView.TopRowIndex;
            old_row_id = GoldTypeGridView.FocusedRowHandle;
            Dictionaries.FGoldTypeAddEdit fg = new Dictionaries.FGoldTypeAddEdit();
            fg.TransactionName = transaction;
            fg.GoldTypeID = id;
            fg.RefreshDataGridView += new Dictionaries.FGoldTypeAddEdit.DoEvent(LoadGoldType);
            fg.ShowDialog();
            GoldTypeGridView.TopRowIndex = topindex;
            GoldTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void EditGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFGoldTypeAddEdit("EDIT", goldTypeID);
        }

        private void GoldTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditGoldTypeBarButton.Enabled)
                LoadFGoldTypeAddEdit("EDIT", goldTypeID);
        }

        private void RefreshGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadGoldType();
        }

        private void UpGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("GOLD_TYPE", goldTypeID, "up", out orderid);
            LoadGoldType();
            GoldTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("GOLD_TYPE", goldTypeID, "down", out orderid);
            LoadGoldType();
            GoldTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteGoldTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteGoldType();
            LoadGoldType();
        }

        ////////////////////////////////////
        ///CreditPurporse
        ///

        private void LoadCreditPurpose()
        {
            string s = "SELECT ID,NAME,CODE,USED_USER_ID,ORDER_ID FROM CRS_USER.CREDIT_PURPOSE ORDER BY ORDER_ID";

            CreditPurposeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditPurpose", "Kreditin təyinatının siyahısı yüklənmədi.");

            if (CreditPurposeGridView.RowCount > 0)
            {
                DeleteCreditPurposeBarButton.Enabled =
                    EditCreditPurposeBarButton.Enabled = true;
                UpCreditPurposeBarButton.Enabled = !(CreditPurposeGridView.FocusedRowHandle == 0);
                DownCreditPurposeBarButton.Enabled = (CreditPurposeGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditPurposeBarButton.Enabled =
                    EditCreditPurposeBarButton.Enabled =
                    UpCreditPurposeBarButton.Enabled =
                    DownCreditPurposeBarButton.Enabled = false;
            }
        }

        private void DeleteCreditPurpose()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CREDIT_PURPOSE WHERE ID = {creditPurposeID}", this.Name + "/DeleteCreditPurpose");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {creditPurposeID}", this.Name + "/DeleteCreditPurpose");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kredit təyinatını silmək istəyirsiniz?", "Kredit təyinatının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CREDIT_PURPOSE WHERE ID = {creditPurposeID}", "Kredit təyinatı silinmədi.", this.Name + "/DeleteCreditPurpose");
                }
                else
                    XtraMessageBox.Show("Seçilmiş kredit təyinatı bazada istifadə olunduğu üçün bu təyinatı silmək olmaz..", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş kredit təyinatı hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş kredit təyinatının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void CreditPurposeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CreditPurposeGridView.GetFocusedDataRow();
            if (row != null)
            {
                creditPurposeID = row["ID"].ToString();
                UpCreditPurposeBarButton.Enabled = !(CreditPurposeGridView.FocusedRowHandle == 0);
                DownCreditPurposeBarButton.Enabled = !(CreditPurposeGridView.FocusedRowHandle == CreditPurposeGridView.RowCount - 1);
            }
        }

        private void CreditPurposeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CreditPurposeGridView, CreditPurposePopupMenu, e);
        }
        private void CreditPurposeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CreditPurposeGridView.RowCount > 0)
            {
                DeleteCreditPurposeBarButton.Enabled =
                    EditCreditPurposeBarButton.Enabled = true;
                if (CreditPurposeGridView.FocusedRowHandle == 0)
                    UpCreditPurposeBarButton.Enabled = false;
                DownCreditPurposeBarButton.Enabled = (CreditPurposeGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditPurposeBarButton.Enabled =
                    EditCreditPurposeBarButton.Enabled =
                    UpCreditPurposeBarButton.Enabled =
                    DownCreditPurposeBarButton.Enabled = false;
            }
        }

        private void CreditPurposeGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CreditPurposeGridView, e);
        }

        private void NewCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditPurposeAddEdit("INSERT", null);
        }

        private void LoadFCreditPurposeAddEdit(string transaction, string id)
        {
            topindex = CreditPurposeGridView.TopRowIndex;
            old_row_id = CreditPurposeGridView.FocusedRowHandle;
            Dictionaries.FCreditPurposeAddEdit fg = new Dictionaries.FCreditPurposeAddEdit();
            fg.TransactionName = transaction;
            fg.CreditPurposeID = id;
            fg.RefreshDataGridView += new Dictionaries.FCreditPurposeAddEdit.DoEvent(LoadCreditPurpose);
            fg.ShowDialog();
            CreditPurposeGridView.TopRowIndex = topindex;
            CreditPurposeGridView.FocusedRowHandle = old_row_id;
        }

        private void EditCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditPurposeAddEdit("EDIT", creditPurposeID);
        }

        private void CreditPurposeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCreditPurposeBarButton.Enabled)
                LoadFCreditPurposeAddEdit("EDIT", creditPurposeID);
        }

        private void RefreshCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCreditPurpose();
        }

        private void UpCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CREDIT_PURPOSE", creditPurposeID, "up", out orderid);
            LoadCreditPurpose();
            CreditPurposeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CREDIT_PURPOSE", creditPurposeID, "down", out orderid);
            LoadCreditPurpose();
            CreditPurposeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteCreditPurposeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCreditPurpose();
            LoadCreditPurpose();
        }


        ////////////////////////
        ///CreditStatus
        ///
        private void LoadCreditStatus()
        {
            string s = "SELECT ID,NAME,CODE,USED_USER_ID,ORDER_ID FROM CRS_USER.CREDIT_STATUS ORDER BY ORDER_ID";

            CreditStatusGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditStatus", "Kredit statusunun siyahısı yüklənmədi.");

            if (CreditStatusGridView.RowCount > 0)
            {
                DeleteCreditStatusBarButton.Enabled =
                    EditCreditStatusBarButton.Enabled = true;
                UpCreditStatusBarButton.Enabled = !(CreditStatusGridView.FocusedRowHandle == 0);
                DownCreditStatusBarButton.Enabled = (CreditStatusGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditStatusBarButton.Enabled =
                    EditCreditStatusBarButton.Enabled =
                    UpCreditStatusBarButton.Enabled =
                    DownCreditStatusBarButton.Enabled = false;
            }
        }

        private void DeleteCreditStatus()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CREDIT_STATUS WHERE ID = {creditStatusID}", this.Name + "/DeleteCreditStatus");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {creditStatusID}", this.Name + "/DeleteCreditStatus");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kredit statusunu silmək istəyirsiniz?", "Kredit statusu silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CREDIT_STATUS WHERE ID = {creditStatusID}", "Kredit statusu silinmədi.", this.Name + "/DeleteCreditStatus");
                }
                else
                    XtraMessageBox.Show("Seçilmiş kredit statusu bazada istifadə olunduğu üçün bu statusu silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş kredit statusu hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş kredit statusunun hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void CreditStatusGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CreditStatusGridView.GetFocusedDataRow();
            if (row != null)
            {
                creditStatusID = row["ID"].ToString();
                UpCreditStatusBarButton.Enabled = !(CreditStatusGridView.FocusedRowHandle == 0);
                DownCreditStatusBarButton.Enabled = !(CreditStatusGridView.FocusedRowHandle == CreditStatusGridView.RowCount - 1);
            }
        }

        private void CreditStatusGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CreditStatusGridView, CreditStatusPopupMenu, e);
        }
        private void CreditStatusGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CreditStatusGridView.RowCount > 0)
            {
                DeleteCreditStatusBarButton.Enabled =
                    EditCreditStatusBarButton.Enabled = true;
                if (CreditStatusGridView.FocusedRowHandle == 0)
                    UpCreditStatusBarButton.Enabled = false;
                DownCreditStatusBarButton.Enabled = (CreditStatusGridView.RowCount > 1);
            }
            else
            {
                DeleteCreditStatusBarButton.Enabled =
                    EditCreditStatusBarButton.Enabled =
                    UpCreditStatusBarButton.Enabled =
                    DownCreditStatusBarButton.Enabled = false;
            }
        }

        private void CreditStatusGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CreditStatusGridView, e);
        }

        private void NewCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditStatusAddEdit("INSERT", null);
        }

        private void LoadFCreditStatusAddEdit(string transaction, string id)
        {
            topindex = CreditStatusGridView.TopRowIndex;
            old_row_id = CreditStatusGridView.FocusedRowHandle;
            Dictionaries.FCreditStatusAddEdit fg = new Dictionaries.FCreditStatusAddEdit();
            fg.TransactionName = transaction;
            fg.CreditStatusID = id;
            fg.RefreshDataGridView += new Dictionaries.FCreditStatusAddEdit.DoEvent(LoadCreditStatus);
            fg.ShowDialog();
            CreditStatusGridView.TopRowIndex = topindex;
            CreditStatusGridView.FocusedRowHandle = old_row_id;
        }

        private void EditCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditStatusAddEdit("EDIT", creditStatusID);
        }

        private void CreditStatusGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCreditStatusBarButton.Enabled)
                LoadFCreditStatusAddEdit("EDIT", creditStatusID);
        }

        private void RefreshCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCreditStatus();
        }

        private void UpCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void DownCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CREDIT_STATUS", creditStatusID, "down", out orderid);
            LoadCreditStatus();
            CreditStatusGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteCreditStatusBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCreditStatus();
            LoadCreditStatus();
        }

        ///////////////////////
        ///CollateralType
        ///
        /// 

        private void LoadCollateralType()
        {
            string s = "SELECT ID,NAME,CODE,USED_USER_ID,ORDER_ID FROM CRS_USER.COLLATERALL_TYPE ORDER BY ORDER_ID";

            CollateralTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCollateralType", "Seçilmiş təminatın siyahısı yüklənmədi.");

            if (CollateralTypeGridView.RowCount > 0)
            {
                DeleteCollateralTypeBarButton.Enabled =
                    EditCollateralTypeBarButton.Enabled = true;
                UpCollateralTypeBarButton.Enabled = !(CollateralTypeGridView.FocusedRowHandle == 0);
                DownCollateralTypeBarButton.Enabled = (CollateralTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteCollateralTypeBarButton.Enabled =
                    EditCollateralTypeBarButton.Enabled =
                    UpCollateralTypeBarButton.Enabled =
                    DownCollateralTypeBarButton.Enabled = false;
            }
        }

        private void DeleteCollateralType()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.COLLATERALL_TYPE WHERE ID = {collateralTypeID}", this.Name + "/DeleteCollateralType");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {collateralTypeID}", this.Name + "/DeleteCollateralType");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təminatı silmək istəyirsiniz?", "Təminatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.COLLATERALL_TYPE WHERE ID = {collateralTypeID}", "Təminat silinmədi.", this.Name + "/DeleteCollateralType");
                }
                else
                    XtraMessageBox.Show("Seçilmiş təminat bazada istifadə olunduğu üçün bu təminatı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş təminat " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş təminatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void CollateralTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CollateralTypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                collateralTypeID = row["ID"].ToString();
                UpCollateralTypeBarButton.Enabled = !(CollateralTypeGridView.FocusedRowHandle == 0);
                DownCollateralTypeBarButton.Enabled = !(CollateralTypeGridView.FocusedRowHandle == CollateralTypeGridView.RowCount - 1);
            }
        }

        private void CollateralTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CollateralTypeGridView, CollateralTypePopupMenu, e);
        }
        private void CollateralTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CollateralTypeGridView.RowCount > 0)
            {
                DeleteCollateralTypeBarButton.Enabled =
                    EditCollateralTypeBarButton.Enabled = true;
                if (CollateralTypeGridView.FocusedRowHandle == 0)
                    UpCollateralTypeBarButton.Enabled = false;
                DownCollateralTypeBarButton.Enabled = (CollateralTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteCollateralTypeBarButton.Enabled =
                    EditCollateralTypeBarButton.Enabled =
                    UpCollateralTypeBarButton.Enabled =
                    DownCollateralTypeBarButton.Enabled = false;
            }
        }

        private void CollateralTypeGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CollateralTypeGridView, e);
        }

        private void NewCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCollateralTypeAddEdit("INSERT", null);
        }

        private void LoadFCollateralTypeAddEdit(string transaction, string id)
        {
            topindex = CollateralTypeGridView.TopRowIndex;
            old_row_id = CollateralTypeGridView.FocusedRowHandle;
            Dictionaries.FCollateralTypeAddEdit fg = new Dictionaries.FCollateralTypeAddEdit();
            fg.TransactionName = transaction;
            fg.CollateralTypeID = id;
            fg.RefreshDataGridView += new Dictionaries.FCollateralTypeAddEdit.DoEvent(LoadCollateralType);
            fg.ShowDialog();
            CollateralTypeGridView.TopRowIndex = topindex;
            CollateralTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void EditCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCollateralTypeAddEdit("EDIT", collateralTypeID);
        }

        private void CollateralTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCollateralTypeBarButton.Enabled)
                LoadFCollateralTypeAddEdit("EDIT", collateralTypeID);
        }

        private void RefreshCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCollateralType();
        }

        private void UpCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("COLLATERALL_TYPE", collateralTypeID, "up", out orderid);
            LoadCollateralType();
            CollateralTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("COLLATERALL_TYPE", collateralTypeID, "down", out orderid);
            LoadCollateralType();
            CollateralTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteCollateralTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCollateralType();
            LoadCollateralType();
        }

        ///////////////////////
        ///TypeCredit
        ///

        private void LoadTypeCredit()
        {
            string s = "SELECT ID,NAME,CODE,USED_USER_ID,ORDER_ID FROM CRS_USER.TYPE_CREDIT ORDER BY ORDER_ID";

            TypeCreditGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTypeCredit", "Kreditin siyahısı yüklənmədi.");

            if (TypeCreditGridView.RowCount > 0)
            {
                DeleteTypeCreditBarButton.Enabled =
                    EditTypeCreditBarButton.Enabled = true;
                UpTypeCreditBarButton.Enabled = !(TypeCreditGridView.FocusedRowHandle == 0);
                DownTypeCreditBarButton.Enabled = (TypeCreditGridView.RowCount > 1);
            }
            else
            {
                DeleteTypeCreditBarButton.Enabled =
                    EditTypeCreditBarButton.Enabled =
                    UpTypeCreditBarButton.Enabled =
                    DownTypeCreditBarButton.Enabled = false;
            }
        }

        private void DeleteTypeCredit()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.TYPE_CREDIT WHERE ID = {typeCreditID}", this.Name + "/DeleteTypeCredit");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {typeCreditID}", this.Name + "/DeleteTypeCredit");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kredit tipini silmək istəyirsiniz?", "Kredit tipinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.TYPE_CREDIT WHERE ID = {typeCreditID}", "Kredit tipi silinmədi.", this.Name + "/DeleteTypeCredit");
                }
                else
                    XtraMessageBox.Show("Seçilmiş kredit tipi bazada istifadə olunduğu üçün bu tipi silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş kredit tipi hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş kredit tipinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void TypeCreditGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TypeCreditGridView.GetFocusedDataRow();
            if (row != null)
            {
                typeCreditID = row["ID"].ToString();
                UpTypeCreditBarButton.Enabled = !(TypeCreditGridView.FocusedRowHandle == 0);
                DownTypeCreditBarButton.Enabled = !(TypeCreditGridView.FocusedRowHandle == TypeCreditGridView.RowCount - 1);
            }
        }

        private void TypeCreditGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TypeCreditGridView, TypeCreditPopupMenu, e);
        }
        private void TypeCreditGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (TypeCreditGridView.RowCount > 0)
            {
                DeleteTypeCreditBarButton.Enabled =
                    EditTypeCreditBarButton.Enabled = true;
                if (TypeCreditGridView.FocusedRowHandle == 0)
                    UpTypeCreditBarButton.Enabled = false;
                DownTypeCreditBarButton.Enabled = (TypeCreditGridView.RowCount > 1);
            }
            else
            {
                DeleteTypeCreditBarButton.Enabled =
                    EditTypeCreditBarButton.Enabled =
                    UpTypeCreditBarButton.Enabled =
                    DownTypeCreditBarButton.Enabled = false;
            }
        }

        private void TypeCreditGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(TypeCreditGridView, e);
        }

        private void NewTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTypeCreditAddEdit("INSERT", null);
        }

        private void LoadFTypeCreditAddEdit(string transaction, string id)
        {
            topindex = TypeCreditGridView.TopRowIndex;
            old_row_id = TypeCreditGridView.FocusedRowHandle;
            Dictionaries.FTypeCreditAddEdit fg = new Dictionaries.FTypeCreditAddEdit();
            fg.TransactionName = transaction;
            fg.TypeCreditID = id;
            fg.RefreshDataGridView += new Dictionaries.FTypeCreditAddEdit.DoEvent(LoadTypeCredit);
            fg.ShowDialog();
            TypeCreditGridView.TopRowIndex = topindex;
            TypeCreditGridView.FocusedRowHandle = old_row_id;
        }

        private void EditTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTypeCreditAddEdit("EDIT", typeCreditID);
        }

        private void TypeCreditGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditTypeCreditBarButton.Enabled)
                LoadFTypeCreditAddEdit("EDIT", typeCreditID);
        }

        private void RefreshTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadTypeCredit();
        }

        private void UpTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TYPE_CREDIT", typeCreditID, "up", out orderid);
            LoadTypeCredit();
            TypeCreditGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TYPE_CREDIT", typeCreditID, "down", out orderid);
            LoadTypeCredit();
            TypeCreditGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteTypeCreditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteTypeCredit();
            LoadTypeCredit();
        }

        ///////////////////////////
        ///DocumentType
        ///

        private void LoadDocumentType()
        {
            string s = "SELECT ID,NAME,PTTRN,USED_USER_ID,ORDER_ID FROM CRS_USER.TYPE_DOCUMENT ORDER BY ORDER_ID";

            DocumentTypeGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDocumentType", "Sənədin növlərinin siyahısı yüklənmədi.");

            if (DocumentTypeGridView.RowCount > 0)
            {
                DeleteDocumentTypeBarButton.Enabled =
                    EditDocumentTypeBarButton.Enabled = true;
                UpDocumentTypeBarButton.Enabled = !(DocumentTypeGridView.FocusedRowHandle == 0);
                DownDocumentTypeBarButton.Enabled = (DocumentTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteDocumentTypeBarButton.Enabled =
                    EditDocumentTypeBarButton.Enabled =
                    UpDocumentTypeBarButton.Enabled =
                    DownDocumentTypeBarButton.Enabled = false;
            }
        }

        private void DeleteDocumentType()
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.TYPE_DOCUMENT WHERE ID = {typeDocumentID}", this.Name + "/DeleteDocumentType");
            if (UsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAWN WHERE GOLD_TYPE_ID = {typeDocumentID}", this.Name + "/DeleteDocumentType");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sənəd növünü silmək istəyirsiniz?", "Sənəd növünün silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.TYPE_DOCUMENT WHERE ID = {typeDocumentID}", "Sənəd növü silinmədi.", this.Name + "/DeleteDocumentType");
                }
                else
                    XtraMessageBox.Show("Seçilmiş sənəd növü bazada istifadə olunduğu üçün bu təminatı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş sənəd növü hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş sənəd növünün hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void DocumentTypeGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = DocumentTypeGridView.GetFocusedDataRow();
            if (row != null)
            {
                typeDocumentID = row["ID"].ToString();
                UpDocumentTypeBarButton.Enabled = !(DocumentTypeGridView.FocusedRowHandle == 0);
                DownDocumentTypeBarButton.Enabled = !(DocumentTypeGridView.FocusedRowHandle == DocumentTypeGridView.RowCount - 1);
            }
        }

        private void DocumentTypeGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(DocumentTypeGridView, DocumentTypePopupMenu, e);
        }
        private void DocumentTypeGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (DocumentTypeGridView.RowCount > 0)
            {
                DeleteDocumentTypeBarButton.Enabled =
                    EditDocumentTypeBarButton.Enabled = true;
                if (DocumentTypeGridView.FocusedRowHandle == 0)
                    UpDocumentTypeBarButton.Enabled = false;
                DownDocumentTypeBarButton.Enabled = (DocumentTypeGridView.RowCount > 1);
            }
            else
            {
                DeleteDocumentTypeBarButton.Enabled =
                    EditDocumentTypeBarButton.Enabled =
                    UpDocumentTypeBarButton.Enabled =
                    DownDocumentTypeBarButton.Enabled = false;
            }
        }

        private void DocumentTypeGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(DocumentTypeGridView, e);
        }

        private void NewDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFDocumentTypeAddEdit("INSERT", null);
        }

        private void LoadFDocumentTypeAddEdit(string transaction, string id)
        {
            topindex = DocumentTypeGridView.TopRowIndex;
            old_row_id = DocumentTypeGridView.FocusedRowHandle;
            Dictionaries.FDocumentTypeAddEdit fg = new Dictionaries.FDocumentTypeAddEdit();
            fg.TransactionName = transaction;
            fg.DocumentTypeID = id;
            fg.RefreshDataGridView += new Dictionaries.FDocumentTypeAddEdit.DoEvent(LoadDocumentType);
            fg.ShowDialog();
            DocumentTypeGridView.TopRowIndex = topindex;
            DocumentTypeGridView.FocusedRowHandle = old_row_id;
        }

        private void EditDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFDocumentTypeAddEdit("EDIT", typeDocumentID);
        }

        private void DocumentTypeGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditDocumentTypeBarButton.Enabled)
                LoadFDocumentTypeAddEdit("EDIT", typeDocumentID);
        }

        private void RefreshDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDocumentType();
        }

        private void UpDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TYPE_DOCUMENT", typeDocumentID, "up", out orderid);
            LoadDocumentType();
            DocumentTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TYPE_DOCUMENT", typeDocumentID, "down", out orderid);
            LoadDocumentType();
            DocumentTypeGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteDocumentTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteDocumentType();
            LoadDocumentType();
        }




    }
}