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
        string PhoneID, StatusID, SeriesID, IssuingID, BirthplaceID, CreditTypeID, CreditNameID, CompanyID, CurrencyID, CarBrandID, CarTypeID, CarColorID, CarModelID, CarBanTypeID, OperationID,
                BankID, AppointmentID, CountryID, FundsSourceID, FundsSourceNameID, FounderID, CashOtherAppointmentID, PositionID, PersonnelID, InsuranceRateID, KindShipID, ContractEvaluateID;

        bool FormStatus = false;
        int topindex, old_row_id, contractEvaluateIsDeleted;

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
                CreditNameTab.Visible = GlobalVariables.CreditName;
                CurrencyTab.Visible = GlobalVariables.Currency;
                HostageTab.Visible = GlobalVariables.Hostage;
                BanksTab.Visible = GlobalVariables.Bank;
                BankOperationTab.Visible = GlobalVariables.BankOperation;
                CountriesTab.Visible = GlobalVariables.Countries;
                PositionTab.Visible = GlobalVariables.Positions;
                InsuranceCompanyTab.Visible = InsuranceRateTab.Visible = GlobalVariables.Insurance;
            }

            BackstageViewControl.SelectedTabIndex = ViewSelectedTabIndex;
            HostageBackstageViewControl.SelectedTabIndex = HostageSelectedTabIndex;
            FundsBackstageViewControl.SelectedTabIndex = FundSelectedTabIndex;
            CashAppointmentBackstageViewControl.SelectedTabIndex = CashAppointmentSelectedTabIndex;
            LoadPhoneDescriptionDataGridView();
            LoadCarColorDataGridView();
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
                        LoadCreditTypeDataGridView();
                        break;
                    case 6:
                        LoadCreditNameDataGridView();
                        break;
                    case 7:
                        LoadCurrencyDataGridView();
                        break;
                    case 10:
                        LoadPersonnelDataGridView();
                        break;
                    case 11:
                        LoadBanksDataGridView();
                        break;
                    case 12:
                        LoadBankAppointmentDataGridView();
                        break;
                    case 13:
                        LoadCountriesDataGridView();
                        break;
                    case 15:
                        LoadFoundersDataGridView();
                        break;
                    case 16:
                        LoadPositionsDataGridView();
                        break;
                    case 17:
                        LoadKindshipRateDataGridView();
                        break;
                    case 18:
                        LoadContractEvaluateDataGridView();
                        break;
                }

                switch (HostageBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCarBrandDataGridView();
                        break;
                    case 1:
                        LoadCarTypeDataGridView();
                        break;
                    case 2:
                        LoadCarColorDataGridView();
                        break;
                    case 3:
                        LoadCarModelDataGridView();
                        break;
                    case 4:
                        LoadCarBanTypeDataGridView();
                        break;
                    case 6:
                        LoadInsuranceCompanyDataGridView();
                        break;
                    case 7:
                        LoadInsuranceRateDataGridView();
                        break;
                }

                switch (FundsBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadFundsSourceDataGridView();
                        break;
                    case 1:
                        LoadFundsSourceNameDataGridView();
                        break;
                }

                switch (CashAppointmentBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCashOtherAppointmentDataGridView();
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
            string s = "SELECT 1 SS,ID,DESCRIPTION_AZ,DESCRIPTION_EN,DESCRIPTION_RU,USED_USER_ID FROM CRS_USER.PHONE_DESCRIPTIONS ORDER BY DESCRIPTION_AZ ASC";

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
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT 1 SS,CT.ID,CN.NAME,TO_CHAR(CT.CALC_DATE,'DD.MM.YYYY') CALC_DATE,CT.CODE,CT.TERM||' ay',CT.INTEREST||' %',CT.NOTE,CT.USED_USER_ID FROM CRS_USER.CREDIT_TYPE CT,CRS_USER.CREDIT_NAMES CN WHERE CT.NAME_ID = CN.ID ORDER BY CN.NAME, CT.CALC_DATE DESC";
                        break;
                    case "EN":
                        s = "SELECT 1 SS,CT.ID,CN.NAME_EN,TO_CHAR(CT.CALC_DATE,'DD.MM.YYYY') CALC_DATE,CT.CODE,CT.TERM||' ay',CT.INTEREST||' %',CT.NOTE,CT.USED_USER_ID FROM CRS_USER.CREDIT_TYPE CT,CRS_USER.CREDIT_NAMES CN WHERE CT.NAME_ID = CN.ID ORDER BY CN.NAME, CT.CALC_DATE DESC";
                        break;
                    case "RU":
                        s = "SELECT 1 SS,CT.ID,CN.NAME_RU,TO_CHAR(CT.CALC_DATE,'DD.MM.YYYY') CALC_DATE,CT.CODE,CT.TERM||' ay',CT.INTEREST||' %',CT.NOTE,CT.USED_USER_ID FROM CRS_USER.CREDIT_TYPE CT,CRS_USER.CREDIT_NAMES CN WHERE CT.NAME_ID = CN.ID ORDER BY CN.NAME, CT.CALC_DATE DESC";
                        break;
                }

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditTypeDataGridView");

                CreditTypeGridControl.DataSource = dt;
                CreditTypeGridView.PopulateColumns();
                CreditTypeGridView.Columns[0].Visible = false;
                CreditTypeGridView.Columns[0].Caption = "S/s";
                CreditTypeGridView.Columns[1].Visible = false;
                CreditTypeGridView.Columns[2].Caption = "Növün adı";
                CreditTypeGridView.Columns[3].Caption = "Tarix";
                CreditTypeGridView.Columns[4].Caption = "Kodu";
                CreditTypeGridView.Columns[5].Caption = "Müddəti";
                CreditTypeGridView.Columns[6].Caption = "İllik faizi";
                CreditTypeGridView.Columns[7].Caption = "Qeyd";
                CreditTypeGridView.Columns[8].Visible = false;

                CreditTypeGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditTypeGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CreditTypeGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditTypeGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CreditTypeGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditTypeGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CreditTypeGridView.Columns[5].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditTypeGridView.Columns[5].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CreditTypeGridView.Columns[6].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditTypeGridView.Columns[6].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                CreditTypeGridView.Columns[2].GroupIndex = 0;

                if (CreditTypeGridView.RowCount > 0)
                {
                    DeleteCreditTypeBarButton.Enabled = true;
                    EditCreditTypeBarButton.Enabled = true;
                    CreditTypeGridView.Columns[3].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                    DeleteCreditTypeBarButton.Enabled = EditCreditTypeBarButton.Enabled = false;
                CreditTypeGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kreditin növləri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCreditNameDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NAME_EN,NAME_RU,NOTE,USED_USER_ID FROM CRS_USER.CREDIT_NAMES ORDER BY NAME ASC";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCreditNameDataGridView");

                CreditNameGridControl.DataSource = dt;
                CreditNameGridView.PopulateColumns();

                CreditNameGridView.Columns[0].Caption = "S/s";
                CreditNameGridView.Columns[1].Visible = false;
                CreditNameGridView.Columns[2].Caption = "Lizinqin adı (AZ)";
                CreditNameGridView.Columns[3].Caption = "Lizinqin adı (EN)";
                CreditNameGridView.Columns[4].Caption = "Lizinqin adı (RU)";
                CreditNameGridView.Columns[5].Caption = "Qeyd";
                CreditNameGridView.Columns[6].Visible = false;

                CreditNameGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CreditNameGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CreditNameGridView.RowCount > 0)
                {
                    DeleteCreditNameBarButton.Enabled = EditCreditNameBarButton.Enabled = true;
                    CreditNameGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                    DeleteCreditNameBarButton.Enabled = EditCreditNameBarButton.Enabled = false;

                CreditNameGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kreditin adları cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCurrencyDataGridView()
        {
            string s = "SELECT 1 SS,ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE,USED_USER_ID FROM CRS_USER.CURRENCY ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCurrencyDataGridView");

                CurrencyGridControl.DataSource = dt;
                CurrencyGridView.PopulateColumns();

                CurrencyGridView.Columns[0].Caption = "S/s";
                CurrencyGridView.Columns[1].Visible = false;
                CurrencyGridView.Columns[2].Caption = "Valyutanın kodu";
                CurrencyGridView.Columns[3].Caption = "Dəyəri";
                CurrencyGridView.Columns[4].Caption = "Valyutanın adı";
                CurrencyGridView.Columns[5].Caption = "Valyutanın qısa adı";
                CurrencyGridView.Columns[6].Caption = "Ən kiçik ölçü vahidi";
                CurrencyGridView.Columns[7].Caption = "Qeyd";
                CurrencyGridView.Columns[8].Visible = false;
                //TextAligment
                CurrencyGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CurrencyGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CurrencyGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CurrencyGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                CurrencyGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CurrencyGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CurrencyGridView.RowCount > 0)
                {
                    DeleteCurrencyBarButton.Enabled = EditCurrencyBarButton.Enabled = true;
                    CurrencyGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CurrencyGridView.FocusedRowHandle == 0)
                        UpCurrencyBarButton.Enabled = false;

                    DownCurrencyBarButton.Enabled = (CurrencyGridView.RowCount > 1);
                }
                else
                    DeleteCurrencyBarButton.Enabled = EditCurrencyBarButton.Enabled = UpCurrencyBarButton.Enabled = DownCurrencyBarButton.Enabled = false;
                CurrencyGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Valyutalar yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
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
                                         S.STATUS_NAME,
                                         B.USED_USER_ID,
                                         B.STATUS_ID,
                                         B.IS_USED
                                    FROM CRS_USER.BANKS B, CRS_USER.STATUS S
                                   WHERE B.STATUS_ID = S.ID
                                ORDER BY ORDER_ID";

            BanksGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBanksDataGridView", "Bankların siyahısı yüklənmədi");

            if (BanksGridView.RowCount > 0)
                DeleteBankBarButton.Enabled = EditBankBarButton.Enabled = true;
            else
                DeleteBankBarButton.Enabled =
                    EditBankBarButton.Enabled =
                    UpBankBarButton.Enabled =
                    DownBankBarButton.Enabled = false;
        }

        private void LoadCarBrandDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BRANDS ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarBrandDataGridView");

                BrandGridControl.DataSource = dt;
                BrandGridView.PopulateColumns();

                BrandGridView.Columns[0].Caption = "S/s";
                BrandGridView.Columns[1].Visible = false;
                BrandGridView.Columns[2].Caption = "Markanın adı";
                BrandGridView.Columns[3].Caption = "Qeyd";
                BrandGridView.Columns[4].Visible = false;

                BrandGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                BrandGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                for (int i = 0; i < BrandGridView.Columns.Count; i++)
                {
                    BrandGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    BrandGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (BrandGridView.RowCount > 0)
                {
                    DeleteCarBrandBarButton.Enabled = EditCarBrandBarButton.Enabled = true;
                    BrandGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (BrandGridView.FocusedRowHandle == 0)
                        UpCarBrandBarButton.Enabled = false;

                    DownCarBrandBarButton.Enabled = (BrandGridView.RowCount > 1);
                }
                else
                    DeleteCarBrandBarButton.Enabled = EditCarBrandBarButton.Enabled = UpCarBrandBarButton.Enabled = DownCarBrandBarButton.Enabled = false;
                BrandGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Markalar cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCarTypeDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_TYPES ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarTypeDataGridView");

                CarTypeGridControl.DataSource = dt;
                CarTypeGridView.PopulateColumns();

                CarTypeGridView.Columns[0].Caption = "S/s";
                CarTypeGridView.Columns[1].Visible = false;
                CarTypeGridView.Columns[2].Caption = "Tipin adı";
                CarTypeGridView.Columns[3].Caption = "Qeyd";
                CarTypeGridView.Columns[4].Visible = false;

                CarTypeGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CarTypeGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CarTypeGridView.RowCount > 0)
                {
                    DeleteCarTypeBarButton.Enabled = true;
                    EditCarTypeBarButton.Enabled = true;
                    CarTypeGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CarTypeGridView.FocusedRowHandle == 0)
                        UpCarTypeBarButton.Enabled = false;

                    DownCarTypeBarButton.Enabled = (CarTypeGridView.RowCount > 1);
                }
                else
                    DeleteCarTypeBarButton.Enabled = EditCarTypeBarButton.Enabled = UpCarTypeBarButton.Enabled = DownCarTypeBarButton.Enabled = false;

                CarTypeGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Avtomonillərin tipləri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCarColorDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_COLORS ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarColorDataGridView");

                CarColorGridControl.DataSource = dt;
                CarColorGridView.PopulateColumns();

                CarColorGridView.Columns[0].Caption = "S/s";
                CarColorGridView.Columns[1].Visible = false;
                CarColorGridView.Columns[2].Caption = "Rəngin adı";
                CarColorGridView.Columns[3].Caption = "Qeyd";
                CarColorGridView.Columns[4].Visible = false;

                CarColorGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CarColorGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CarColorGridView.RowCount > 0)
                {
                    DeleteCarColorBarButton.Enabled = true;
                    EditCarColorBarButton.Enabled = true;
                    CarColorGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CarColorGridView.FocusedRowHandle == 0)
                        UpCarColorBarButton.Enabled = false;
                    if (CarColorGridView.RowCount > 1)
                        DownCarColorBarButton.Enabled = true;
                    else
                        DownCarColorBarButton.Enabled = false;
                }
                else
                    DeleteCarColorBarButton.Enabled = EditCarColorBarButton.Enabled = UpCarColorBarButton.Enabled = DownCarColorBarButton.Enabled = false;

                CarColorGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Avtomonillərin rəngləri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCarModelDataGridView()
        {
            string s = "SELECT 1 SS,M.ID,B.NAME,M.NAME,M.NOTE,M.USED_USER_ID FROM CRS_USER.CAR_BRANDS B,CRS_USER.CAR_MODELS M WHERE B.ID = M.BRAND_ID ORDER BY M.ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarModelDataGridView");

                CarModelGridControl.DataSource = dt;
                CarModelGridView.PopulateColumns();

                CarModelGridView.Columns[0].Caption = "S/s";
                CarModelGridView.Columns[1].Visible = false;
                CarModelGridView.Columns[2].Caption = "Markanın adı";
                CarModelGridView.Columns[3].Caption = "Modelin adı";
                CarModelGridView.Columns[4].Caption = "Qeyd";
                CarModelGridView.Columns[5].Visible = false;

                CarModelGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CarModelGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CarModelGridView.RowCount > 0)
                {
                    DeleteCarModelBarButton.Enabled = true;
                    EditCarModelBarButton.Enabled = true;
                    CarModelGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CarModelGridView.FocusedRowHandle == 0)
                        UpCarModelBarButton.Enabled = false;
                    if (CarModelGridView.RowCount > 1)
                        DownCarModelBarButton.Enabled = true;
                    else
                        DownCarModelBarButton.Enabled = false;
                }
                else
                    DeleteCarModelBarButton.Enabled = EditCarModelBarButton.Enabled = UpCarModelBarButton.Enabled = DownCarModelBarButton.Enabled = false;
                CarModelGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Modellər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCarBanTypeDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_BAN_TYPES ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarBanTypeDataGridView");

                CarBanTypeGridControl.DataSource = dt;
                CarBanTypeGridView.PopulateColumns();

                CarBanTypeGridView.Columns[0].Caption = "S/s";
                CarBanTypeGridView.Columns[1].Visible = false;
                CarBanTypeGridView.Columns[2].Caption = "Ban tipin adı";
                CarBanTypeGridView.Columns[3].Caption = "Qeyd";
                CarBanTypeGridView.Columns[4].Visible = false;

                CarBanTypeGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CarBanTypeGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (CarBanTypeGridView.RowCount > 0)
                {
                    DeleteBanTypeBarButton.Enabled = true;
                    EditBanTypeBarButton.Enabled = true;
                    CarBanTypeGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CarBanTypeGridView.FocusedRowHandle == 0)
                        UpBanTypeBarButton.Enabled = false;
                    DownBanTypeBarButton.Enabled = (CarBanTypeGridView.RowCount > 1);
                }
                else
                    DeleteBanTypeBarButton.Enabled = EditBanTypeBarButton.Enabled = UpBanTypeBarButton.Enabled = DownBanTypeBarButton.Enabled = false;
                CarBanTypeGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Avtomonillərin ban tipləri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadInsuranceCompanyDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,NOTE,USED_USER_ID FROM CRS_USER.INSURANCE_COMPANY ORDER BY NAME";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceCompanyDataGridView");

                InsuranceCompanyGridControl.DataSource = dt;
                InsuranceCompanyGridView.PopulateColumns();

                InsuranceCompanyGridView.Columns[0].Caption = "S/s";
                InsuranceCompanyGridView.Columns[1].Visible = false;
                InsuranceCompanyGridView.Columns[2].Caption = "Şirkətin adı";
                InsuranceCompanyGridView.Columns[3].Caption = "Ünvanı";
                InsuranceCompanyGridView.Columns[4].Caption = "Ünvanın təsviri";
                InsuranceCompanyGridView.Columns[5].Caption = "Telefon";
                InsuranceCompanyGridView.Columns[6].Caption = "Faks";
                InsuranceCompanyGridView.Columns[7].Caption = "Qeyd";
                InsuranceCompanyGridView.Columns[8].Visible = false;

                InsuranceCompanyGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                InsuranceCompanyGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                if (InsuranceCompanyGridView.RowCount > 0)
                {
                    DeleteInsuranceCompanyBarButton.Enabled = EditInsuranceCompanyBarButton.Enabled = true;
                    InsuranceCompanyGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                    DeleteInsuranceCompanyBarButton.Enabled = EditInsuranceCompanyBarButton.Enabled = false;

                InsuranceCompanyGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sığorta şirkətləri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadInsuranceRateDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                     ID,
                                     RATE,
                                     UNCONDITIONAL_AMOUNT,
                                     NOTE,
                                     USED_USER_ID
                                FROM CRS_USER.INSURANCE_RATE
                            ORDER BY ORDER_ID";


            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceRateDataGridView", "Sığortanın faizləri cədvələ yüklənmədi.");

            InsuranceRateGridControl.DataSource = dt;

            if (InsuranceRateGridView.RowCount > 0)
            {
                DeleteInsuranceRateBarButton.Enabled =
                    EditInsuranceRateBarButton.Enabled = true;

                if (InsuranceRateGridView.FocusedRowHandle == 0)
                    UpInsuranceRateBarButton.Enabled = false;

                DownInsuranceRateBarButton.Enabled = (InsuranceRateGridView.RowCount > 1);
            }
            else
                DeleteInsuranceRateBarButton.Enabled = EditInsuranceRateBarButton.Enabled = UpInsuranceRateBarButton.Enabled = DownInsuranceRateBarButton.Enabled = false;
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

        private void LoadBankAppointmentDataGridView()
        {
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = $@"SELECT 1 SS,
                                     BA.ID,         
                                     OT.TYPE_AZ OPERATION_TYPE_NAME,
                                     AT.NAME APPOINTMENT_TYPE_NAME,
                                     BA.NAME APPOINTMENT_NAME,
                                     BA.NOTE,
                                     BA.USED_USER_ID
                                FROM CRS_USER.BANK_APPOINTMENTS BA, CRS_USER.OPERATION_TYPES OT,CRS_USER.APPOINTMENT_TYPE AT
                               WHERE BA.OPERATION_TYPE_ID = OT.ID AND BA.APPOINTMENT_TYPE_ID = AT.ID
                            ORDER BY BA.OPERATION_TYPE_ID, BA.NAME";
                        break;
                    case "EN":
                        s = "SELECT 1 SS,BA.ID,OT.TYPE_EN OPERATION_TYPE_NAME,BA.NAME_EN APPOINTMENT_NAME,BA.NOTE,BA.USED_USER_ID FROM CRS_USER.BANK_APPOINTMENTS BA,CRS_USER.OPERATION_TYPES OT WHERE BA.OPERATION_TYPE_ID = OT.ID ORDER BY NAME ASC";
                        break;
                    case "RU":
                        s = "SELECT 1 SS,BA.ID,OT.TYPE_RU OPERATION_TYPE_NAME,BA.NAME_RU APPOINTMENT_NAME,BA.NOTE,BA.USED_USER_ID FROM CRS_USER.BANK_APPOINTMENTS BA,CRS_USER.OPERATION_TYPES OT WHERE BA.OPERATION_TYPE_ID = OT.ID ORDER BY NAME ASC";
                        break;
                }

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankAppointmentDataGridView");

                AppointmentsGridControl.DataSource = dt;
                AppointmentsGridView.PopulateColumns();

                AppointmentsGridView.Columns[0].Visible = false;
                AppointmentsGridView.Columns[0].Caption = "S/s";
                AppointmentsGridView.Columns["ID"].Visible = false;
                AppointmentsGridView.Columns["OPERATION_TYPE_NAME"].Caption = "Əməliyyatın adı";
                AppointmentsGridView.Columns["APPOINTMENT_TYPE_NAME"].Caption = "Təyinatın növü";
                AppointmentsGridView.Columns["APPOINTMENT_NAME"].Caption = "Təyinatın adı";
                AppointmentsGridView.Columns["NOTE"].Caption = "Qeyd";
                AppointmentsGridView.Columns["USED_USER_ID"].Visible = false;

                AppointmentsGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AppointmentsGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (AppointmentsGridView.RowCount > 0)
                {
                    DeleteAppointmentBarButton.Enabled = true;
                    EditAppointmentBarButton.Enabled = true;
                    AppointmentsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    DeleteAppointmentBarButton.Enabled = false;
                    EditAppointmentBarButton.Enabled = false;
                }

                AppointmentsGridView.Columns["OPERATION_TYPE_NAME"].GroupIndex = 0;
                AppointmentsGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank əməliyyatlarının təyinatları cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadFundsSourceDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.FUNDS_SOURCES ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFundsSourceDataGridView");

                FundsSourcesGridControl.DataSource = dt;
                FundsSourcesGridView.PopulateColumns();

                FundsSourcesGridView.Columns[0].Caption = "S/s";
                FundsSourcesGridView.Columns[1].Visible = false;
                FundsSourcesGridView.Columns[2].Caption = "Mənbə";
                FundsSourcesGridView.Columns[3].Caption = "Qeyd";
                FundsSourcesGridView.Columns[4].Visible = false;

                FundsSourcesGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FundsSourcesGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (FundsSourcesGridView.RowCount > 0)
                {
                    DeleteFundsSourceBarButton.Enabled = true;
                    EditFundsSourceBarButton.Enabled = true;
                    FundsSourcesGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (FundsSourcesGridView.FocusedRowHandle == 0)
                        UpFundsSourceBarButton.Enabled = false;
                    if (FundsSourcesGridView.RowCount > 1)
                        DownFundsSourceBarButton.Enabled = true;
                    else
                        DownFundsSourceBarButton.Enabled = false;
                }
                else
                {
                    DeleteFundsSourceBarButton.Enabled = false;
                    EditFundsSourceBarButton.Enabled = false;
                    UpFundsSourceBarButton.Enabled = false;
                    DownFundsSourceBarButton.Enabled = false;
                }
                FundsSourcesGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Vəsaitlərin mənbələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadFundsSourceNameDataGridView()
        {
            string s = "SELECT 1 SS,SN.ID,S.NAME,SN.NAME,SN.NOTE,SN.USED_USER_ID FROM CRS_USER.FUNDS_SOURCES_NAME SN,CRS_USER.FUNDS_SOURCES S WHERE SN.SOURCE_ID = S.ID ORDER BY SN.ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFundsSourceNameDataGridView");

                FundsSourcesNameGridControl.DataSource = dt;
                FundsSourcesNameGridView.PopulateColumns();

                FundsSourcesNameGridView.Columns[0].Caption = "S/s";
                FundsSourcesNameGridView.Columns[1].Visible = false;
                FundsSourcesNameGridView.Columns[2].Caption = "Mənbə";
                FundsSourcesNameGridView.Columns[3].Caption = "Mənbəyin adı";
                FundsSourcesNameGridView.Columns[4].Caption = "Qeyd";
                FundsSourcesNameGridView.Columns[5].Visible = false;

                FundsSourcesNameGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FundsSourcesNameGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (FundsSourcesNameGridView.RowCount > 0)
                {
                    DeleteFundsSourceNameBarButton.Enabled = true;
                    EditFundsSourceNameBarButton.Enabled = true;
                    FundsSourcesNameGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (FundsSourcesNameGridView.FocusedRowHandle == 0)
                        UpFundsSourceNameBarButton.Enabled = false;
                    if (FundsSourcesNameGridView.RowCount > 1)
                        DownFundsSourceNameBarButton.Enabled = true;
                    else
                        DownFundsSourceNameBarButton.Enabled = false;
                }
                else
                {
                    DeleteFundsSourceNameBarButton.Enabled = false;
                    EditFundsSourceNameBarButton.Enabled = false;
                    UpFundsSourceNameBarButton.Enabled = false;
                    DownFundsSourceNameBarButton.Enabled = false;
                }
                FundsSourcesNameGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Vəsaitlərin mənbələrinin adı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCashOtherAppointmentDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CASH_APPOINTMENTS ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                CashOtherAppointmentGridControl.DataSource = dt;
                CashOtherAppointmentGridView.PopulateColumns();

                CashOtherAppointmentGridView.Columns[0].Caption = "S/s";
                CashOtherAppointmentGridView.Columns[1].Visible = false;
                CashOtherAppointmentGridView.Columns[2].Caption = "Təyinatın adı";
                CashOtherAppointmentGridView.Columns[3].Caption = "Qeyd";
                CashOtherAppointmentGridView.Columns[4].Visible = false;

                CashOtherAppointmentGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CashOtherAppointmentGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                for (int i = 0; i < CashOtherAppointmentGridView.Columns.Count; i++)
                {
                    CashOtherAppointmentGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    CashOtherAppointmentGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (CashOtherAppointmentGridView.RowCount > 0)
                {
                    DeleteCashOtherAppointmentBarButton.Enabled = true;
                    EditCashOtherAppointmentBarButton.Enabled = true;
                    CashOtherAppointmentGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (CashOtherAppointmentGridView.FocusedRowHandle == 0)
                        UpCashOtherAppointmentBarButton.Enabled = false;
                    if (CashOtherAppointmentGridView.RowCount > 1)
                        DownCashOtherAppointmentBarButton.Enabled = true;
                    else
                        DownCashOtherAppointmentBarButton.Enabled = false;
                }
                else
                {
                    DeleteCashOtherAppointmentBarButton.Enabled = false;
                    EditCashOtherAppointmentBarButton.Enabled = false;
                    UpCashOtherAppointmentBarButton.Enabled = false;
                    DownCashOtherAppointmentBarButton.Enabled = false;
                }
                CashOtherAppointmentGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kassanın digər ödənişlərinin təyinatları cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadPersonnelDataGridView()
        {
            string s = null;
            try
            {
                s = $@"SELECT 1 SS,
                             P.ID,
                             P.SURNAME || ' ' || P.NAME || ' ' || P.PATRONYMIC FULLNAME,
                             PO.POSITION_NAME,
                             S.STATUS_NAME,
                             P.NOTE,
                             P.USED_USER_ID,
                             P.STATUS_ID
                        FROM CRS_USER.PERSONNEL P,
                             CRS_USER.V_PERSONNEL_LAST_POSITION PO,
                             CRS_USER.STATUS S
                       WHERE P.STATUS_ID = S.ID AND P.ID = PO.PERSONNEL_ID(+)
                    ORDER BY P.SURNAME";

                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                PersonnelGridControl.DataSource = dt;
                PersonnelGridView.PopulateColumns();

                PersonnelGridView.Columns[0].Caption = "S/s";
                PersonnelGridView.Columns[1].Visible = false;
                PersonnelGridView.Columns[2].Caption = "Soyadı, adı, atasının adı";
                PersonnelGridView.Columns[3].Caption = "Vəzifəsi";
                PersonnelGridView.Columns[4].Caption = "Statusu";
                PersonnelGridView.Columns[5].Caption = "Qeyd";
                PersonnelGridView.Columns[6].Visible = false;
                PersonnelGridView.Columns[7].Visible = false;

                //TextAligment
                PersonnelGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                PersonnelGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;                

                if (PersonnelGridView.RowCount > 0)
                {
                    DeletePersonnelBarButton.Enabled = EditPersonnelBarButton.Enabled = true;
                    PersonnelGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else                
                    DeletePersonnelBarButton.Enabled = EditPersonnelBarButton.Enabled = false;                

                PersonnelGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İşçilərin siyahısı yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadPositionsDataGridView()
        {
            string s = "SELECT 1 SS,ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.POSITIONS ORDER BY ORDER_ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                PositionGridControl.DataSource = dt;
                PositionGridView.PopulateColumns();

                PositionGridView.Columns[0].Caption = "S/s";
                PositionGridView.Columns[1].Visible = false;
                PositionGridView.Columns[2].Caption = "Vəzifənin adı";
                PositionGridView.Columns[3].Caption = "Qeyd";
                PositionGridView.Columns[4].Visible = false;

                PositionGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                PositionGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                for (int i = 0; i < PositionGridView.Columns.Count; i++)
                {
                    PositionGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    PositionGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (PositionGridView.RowCount > 0)
                {
                    DeletePositionBarButton.Enabled = true;
                    EditPositionBarButton.Enabled = true;
                    PositionGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    if (PositionGridView.FocusedRowHandle == 0)
                        UpPositionBarButton.Enabled = false;
                    if (PositionGridView.RowCount > 1)
                        DownPositionBarButton.Enabled = true;
                    else
                        DownPositionBarButton.Enabled = false;
                }
                else
                {
                    DeletePositionBarButton.Enabled = false;
                    EditPositionBarButton.Enabled = false;
                    UpPositionBarButton.Enabled = false;
                    DownPositionBarButton.Enabled = false;
                }
                PositionGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Vəzifələr cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
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
                        LoadCreditTypeDataGridView();
                        break;
                    case 6:
                        LoadCreditNameDataGridView();
                        break;
                    case 7:
                        LoadCurrencyDataGridView();
                        break;
                    case 8:
                        LoadCarBrandDataGridView();
                        break;
                    case 9:
                        LoadCashOtherAppointmentDataGridView();
                        break;
                    case 10:
                        LoadPersonnelDataGridView();
                        break;
                    case 11:
                        LoadBanksDataGridView();
                        break;
                    case 12:
                        LoadBankAppointmentDataGridView();
                        break;
                    case 13:
                        LoadCountriesDataGridView();
                        break;
                    case 14:
                        LoadFundsSourceDataGridView();
                        break;
                    case 15:
                        LoadFoundersDataGridView();
                        break;
                    case 16:
                        LoadPositionsDataGridView();
                        break;
                    case 17:
                        LoadKindshipRateDataGridView();
                        break;
                    case 18:
                        LoadContractEvaluateDataGridView();
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
                if (BackstageViewControl.SelectedTabIndex != 8 && BackstageViewControl.SelectedTabIndex != 9 && BackstageViewControl.SelectedTabIndex != 14)
                    this.RefreshList(BackstageViewControl.SelectedTabIndex);
                else if (BackstageViewControl.SelectedTabIndex == 8)
                    this.RefreshList(HostageBackstageViewControl.SelectedTabIndex);
                else if (BackstageViewControl.SelectedTabIndex == 9)
                    this.RefreshList(CashAppointmentBackstageViewControl.SelectedTabIndex);
                else if (BackstageViewControl.SelectedTabIndex == 14)
                    this.RefreshList(FundsBackstageViewControl.SelectedTabIndex);
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

        private void CreditNameGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CreditNameGridView, CreditNamePopupMenu, e);
        }

        private void CreditNameGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CreditNameGridView.GetFocusedDataRow();
            if (row != null)
                CreditNameID = row["ID"].ToString();
        }

        void RefreshCreditName()
        {
            LoadCreditNameDataGridView();
        }

        private void LoadFCreditNameAddEdit(string transaction, string NameID)
        {
            topindex = CreditNameGridView.TopRowIndex;
            old_row_id = CreditNameGridView.FocusedRowHandle;
            Forms.Dictionaries.FCreditNameAddEdit fse = new Forms.Dictionaries.FCreditNameAddEdit();
            fse.TransactionName = transaction;
            fse.NameID = NameID;
            fse.RefreshCreditNameDataGridView += new Forms.Dictionaries.FCreditNameAddEdit.DoEvent(RefreshCreditName);
            fse.ShowDialog();
            CreditNameGridView.TopRowIndex = topindex;
            CreditNameGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCreditNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditNameAddEdit("INSERT", null);
        }

        private void EditCreditNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCreditNameAddEdit("EDIT", CreditNameID);
        }

        private void RefreshCreditNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCreditNameDataGridView();
        }

        private void CreditNameGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCreditNameBarButton.Enabled)
                LoadFCreditNameAddEdit("EDIT", CreditNameID);
        }

        private void DeleteCreditNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if ((CreditNameID == "1") || (CreditNameID == "5") || (CreditNameID == "6"))
                XtraMessageBox.Show("Proqram seçilmiş lizinq adı əsasında işlədiyi üçün bu adı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CREDIT_TYPE WHERE NAME_ID = " + CreditNameID) == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş kreditin adını silmək istəyirsiniz?", "Kreditin adının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CREDIT_NAMES WHERE ID = " + CreditNameID, "Kreditin adı silinmədi.", this.Name + "/DeleteCreditNameBarButton_ItemClick");
                }
                else
                    XtraMessageBox.Show("Seçilmiş lizinqin adı bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadCreditNameDataGridView();
            }
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

        private void BrandGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BrandGridView, CarBrandPopupMenu, e);
        }

        private void BrandGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(BrandGridView, e);
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

        private void HostageBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (HostageBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCarBrandDataGridView();
                        break;
                    case 1:
                        LoadCarTypeDataGridView();
                        break;
                    case 2:
                        LoadCarColorDataGridView();
                        break;
                    case 3:
                        LoadCarModelDataGridView();
                        break;
                    case 4:
                        LoadCarBanTypeDataGridView();
                        break;
                    case 6:
                        LoadInsuranceCompanyDataGridView();
                        break;
                    case 7:
                        LoadInsuranceRateDataGridView();
                        break;
                }
            }
        }

        private void NewCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBrandAddEdit("INSERT", null);
        }

        private void EditCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCarBrandAddEdit("EDIT", CarBrandID);
        }

        private void RefreshCarBrandBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarBrandDataGridView();
        }

        private void BrandGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarBrandBarButton.Enabled)
                LoadFCarBrandAddEdit("EDIT", CarBrandID);
        }

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

        private void RefreshCarTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarTypeDataGridView();
        }

        private void CarColorGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CarColorGridView, CarColorPopupMenu, e);
        }

        private void CarColorGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarColorGridView, e);
        }

        private void CarTypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarTypeGridView, e);
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

        private void RefreshCarColorBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarColorDataGridView();
        }

        private void CarColorGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCarColorBarButton.Enabled)
                LoadFCarColorAddEdit("EDIT", CarColorID);
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
            GlobalProcedures.GridRowCellStyleForBlock(CreditNameGridView, e);
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

        private void RefreshCarModelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarModelDataGridView();
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

        private void DeleteCreditType()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CREDIT_TYPE_ID = {CreditTypeID}", this.Name + "/DeleteCreditType");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş parametri silmək istəyirsiniz?", "Lizinq növünün parametrinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_CREDIT_TYPE", "P_TYPE_ID", CreditTypeID, "Lizinq növünün parametri silinmədi.");                    
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

        private void RefreshBanTypeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCarBanTypeDataGridView();
        }

        private void CarBanTypeGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CarBanTypeGridView, e);
        }

        private void BanksGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BanksGridView.GetFocusedDataRow();
            if (row != null)
            {
                BankID = row["ID"].ToString();
                UpBankBarButton.Enabled = !(BanksGridView.FocusedRowHandle == 0);
                DownBankBarButton.Enabled = !(BanksGridView.FocusedRowHandle == BanksGridView.RowCount - 1);
            }
            //LoadBranhesDataGridView();
        }

        void RefreshBanks()
        {
            LoadBanksDataGridView();
        }

        private void LoadFBankAddEdit(string transaction, string id)
        {
            topindex = BanksGridView.TopRowIndex;
            old_row_id = BanksGridView.FocusedRowHandle;
            Forms.Dictionaries.FBankAddEdit fb = new Forms.Dictionaries.FBankAddEdit();
            fb.BankID = id;
            fb.TransactionName = transaction;
            fb.RefreshBanksDataGridView += new Forms.Dictionaries.FBankAddEdit.DoEvent(RefreshBanks);
            fb.ShowDialog();
            BanksGridView.TopRowIndex = topindex;
            BanksGridView.FocusedRowHandle = old_row_id;
        }

        private void NewBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBankAddEdit("INSERT", null);
        }

        private void EditBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBankAddEdit("EDIT", BankID);
        }

        private void BanksGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBankBarButton.Enabled)
                LoadFBankAddEdit("EDIT", BankID);
        }

        private void RefreshBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBanksDataGridView();
        }

        private void BanksGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(BanksGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(8, BanksGridView, e);
        }

        private void BanksGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BanksGridView, BanksPopupMenu, e);
        }

        private void LoadFoundersDataGridView()
        {
            string s = null;
            try
            {
                s = "SELECT 1 SS,ID,FULLNAME,NOTE,USED_USER_ID FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID";

                FoundersGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFoundersDataGridView");
                FoundersGridView.PopulateColumns();

                FoundersGridView.Columns[0].Caption = "S/s";
                FoundersGridView.Columns[1].Visible = false;
                FoundersGridView.Columns[2].Caption = "Soyadı, adı, atasının adı";
                FoundersGridView.Columns[3].Caption = "Qeyd";
                FoundersGridView.Columns[4].Visible = false;

                //TextAligment
                FoundersGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FoundersGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                //HeaderAligment
                for (int i = 0; i < FoundersGridView.Columns.Count; i++)
                {
                    FoundersGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    FoundersGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                }

                if (FoundersGridView.RowCount > 0)
                {
                    DeleteFounderBarButton.Enabled =
                        EditFounderBarButton.Enabled = true;
                    FoundersGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                    UpFounderBarButton.Enabled = !(FoundersGridView.FocusedRowHandle == 0);
                    DownFounderBarButton.Enabled = (FoundersGridView.RowCount > 1);
                }
                else
                {
                    DeleteFounderBarButton.Enabled =
                        EditFounderBarButton.Enabled =
                        UpFounderBarButton.Enabled =
                        DownFounderBarButton.Enabled = false;
                }

                FoundersGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçilərin siyahısı yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
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

        private void AppointmentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AppointmentsGridView, AppointmentPopupMenu, e);
        }

        void RefreshAppointment()
        {
            LoadBankAppointmentDataGridView();
        }

        private void LoadFAppointmentAddEdit(string transaction, string appointmentid)
        {
            topindex = AppointmentsGridView.TopRowIndex;
            old_row_id = AppointmentsGridView.FocusedRowHandle;
            Forms.Dictionaries.FAppointmentAddEdit fp = new Forms.Dictionaries.FAppointmentAddEdit();
            fp.TransactionName = transaction;
            fp.AppointmentID = appointmentid;
            fp.RefreshAppointmentsDataGridView += new Forms.Dictionaries.FAppointmentAddEdit.DoEvent(RefreshAppointment);
            fp.ShowDialog();
            AppointmentsGridView.TopRowIndex = topindex;
            AppointmentsGridView.FocusedRowHandle = old_row_id;
        }

        private void NewAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAppointmentAddEdit("INSERT", null);
        }

        private void AppointmentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AppointmentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                AppointmentID = row["ID"].ToString();
            }
        }

        private void EditAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAppointmentAddEdit("EDIT", AppointmentID);
        }

        private void RefreshAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBankAppointmentDataGridView();
        }

        private void AppointmentsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditAppointmentBarButton.Enabled)
                LoadFAppointmentAddEdit("EDIT", AppointmentID);
        }

        private void DeleteAppointment()
        {
            if (Convert.ToInt32(AppointmentID) == 7 || Convert.ToInt32(AppointmentID) == 4 || Convert.ToInt32(AppointmentID) == 3 || Convert.ToInt32(AppointmentID) == 8 || Convert.ToInt32(AppointmentID) == 15 || Convert.ToInt32(AppointmentID) == 18 || Convert.ToInt32(AppointmentID) == 19)
                XtraMessageBox.Show("Sistemin hesablamaları seçilmiş təyinatın üzərində qurulduğu üçün bu təyinatı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int AppointmentUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.BANK_APPOINTMENTS WHERE ID = " + AppointmentID);
                if (AppointmentUsedUserID < 0)
                {
                    int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = " + AppointmentID);
                    if (a == 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təyinatı silmək istəyirsiniz?", "Təyinatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.BANK_APPOINTMENTS WHERE ID = " + AppointmentID, "Təyinat silinmədi.", this.Name + "/DeleteAppointment");
                        }
                    }
                    else
                        XtraMessageBox.Show("Seçilmiş təyinat bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == AppointmentUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş təyinat hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş təyinatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DeleteAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteAppointment();
            LoadBankAppointmentDataGridView();
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
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_BANK", "P_BANK_ID", BankID, "Bank silinmədi.");
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

        private void DeleteBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteBank();
            LoadBanksDataGridView();
        }

        private void AppointmentsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(AppointmentsGridView, e);
        }

        private void CreditTypeGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CALC_DATE", "Center", e);
        }

        private void FundsSourcesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FundsSourcesGridView, FundsSourcesPopupMenu, e);
        }

        private void FundsSourcesNameGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FundsSourcesNameGridView, FundsSourcesNamePopupMenu, e);
        }

        void RefreshFundsSources()
        {
            LoadFundsSourceDataGridView();
        }

        private void LoadFFundsSourceAddEdit(string transaction, string sourceid)
        {
            topindex = FundsSourcesGridView.TopRowIndex;
            old_row_id = FundsSourcesGridView.FocusedRowHandle;
            Dictionaries.FFundsSourceAddEdit fp = new Dictionaries.FFundsSourceAddEdit();
            fp.TransactionName = transaction;
            fp.SourceID = sourceid;
            fp.RefreshSourceDataGridView += new Dictionaries.FFundsSourceAddEdit.DoEvent(RefreshFundsSources);
            fp.ShowDialog();
            FundsSourcesGridView.TopRowIndex = topindex;
            FundsSourcesGridView.FocusedRowHandle = old_row_id;
        }

        private void NewFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundsSourceAddEdit("INSERT", null);
        }

        private void RefreshFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFundsSourceDataGridView();
        }

        private void FundsSourcesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FundsSourcesGridView.GetFocusedDataRow();
            if (row != null)
            {
                FundsSourceID = row["ID"].ToString();
                UpFundsSourceBarButton.Enabled = !(FundsSourcesGridView.FocusedRowHandle == 0);
                DownFundsSourceBarButton.Enabled = !(FundsSourcesGridView.FocusedRowHandle == FundsSourcesGridView.RowCount - 1);
            }
        }

        private void EditFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundsSourceAddEdit("EDIT", FundsSourceID);
        }

        private void FundsSourcesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditFundsSourceBarButton.Enabled)
                LoadFFundsSourceAddEdit("EDIT", FundsSourceID);
        }

        private void UpFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FUNDS_SOURCES", FundsSourceID, "up", out orderid);
            LoadFundsSourceDataGridView();
            FundsSourcesGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FUNDS_SOURCES", FundsSourceID, "down", out orderid);
            LoadFundsSourceDataGridView();
            FundsSourcesGridView.FocusedRowHandle = orderid - 1;
        }

        private void FundsSourcesNameGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FundsSourcesNameGridView.GetFocusedDataRow();
            if (row != null)
            {
                FundsSourceNameID = row["ID"].ToString();
                UpFundsSourceNameBarButton.Enabled = !(FundsSourcesNameGridView.FocusedRowHandle == 0);
                DownFundsSourceNameBarButton.Enabled = !(FundsSourcesNameGridView.FocusedRowHandle == FundsSourcesNameGridView.RowCount - 1);
            }
        }

        void RefreshFundsSourcesName()
        {
            LoadFundsSourceNameDataGridView();
        }

        private void LoadFFundsSourceNameAddEdit(string transaction, string sourceid)
        {
            topindex = FundsSourcesNameGridView.TopRowIndex;
            old_row_id = FundsSourcesNameGridView.FocusedRowHandle;
            Forms.Dictionaries.FFundsSourceNameAddEdit fp = new Forms.Dictionaries.FFundsSourceNameAddEdit();
            fp.TransactionName = transaction;
            fp.SourceID = sourceid;
            fp.RefreshSourceNameDataGridView += new Forms.Dictionaries.FFundsSourceNameAddEdit.DoEvent(RefreshFundsSourcesName);
            fp.ShowDialog();
            FundsSourcesNameGridView.TopRowIndex = topindex;
            FundsSourcesNameGridView.FocusedRowHandle = old_row_id;
        }

        private void NewFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundsSourceNameAddEdit("INSERT", null);
        }

        private void RefreshFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFundsSourceNameDataGridView();
        }

        private void EditFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFundsSourceNameAddEdit("EDIT", FundsSourceNameID);
        }

        private void FundsSourcesNameGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditFundsSourceNameBarButton.Enabled)
                LoadFFundsSourceNameAddEdit("EDIT", FundsSourceNameID);
        }

        private void UpFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FUNDS_SOURCES_NAME", FundsSourceNameID, "up", out orderid);
            LoadFundsSourceNameDataGridView();
            FundsSourcesNameGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FUNDS_SOURCES_NAME", FundsSourceNameID, "down", out orderid);
            LoadFundsSourceNameDataGridView();
            FundsSourcesNameGridView.FocusedRowHandle = orderid - 1;
        }

        private void FundsBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (FundsBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadFundsSourceDataGridView();
                        break;
                    case 1:
                        LoadFundsSourceNameDataGridView();
                        break;
                }
            }
        }

        private void DeleteFundsSource()
        {
            if (Convert.ToInt32(FundsSourceID) == 6 || Convert.ToInt32(FundsSourceID) == 10)
                XtraMessageBox.Show("Sistem seçilmiş mənbə üzərində qurulduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int FundsSourceUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.FUNDS_SOURCES WHERE ID = {FundsSourceID}", this.Name + "/DeleteFundsSource");
                if (FundsSourceUsedUserID <= 0)
                {
                    int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.FUNDS_CONTRACTS WHERE FUNDS_SOURCE_ID = {FundsSourceID}", this.Name + "/DeleteFundsSource");
                    if (a == 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş mənbəni silmək istəyirsiniz?", "Mənbənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.FUNDS_SOURCES WHERE ID = {FundsSourceID}",
                                                             $@"DELETE FROM CRS_USER.FUNDS_SOURCES_NAME WHERE SOURCE_ID = {FundsSourceID}",
                                                                "Mənbə silinmədi.",
                                                                this.Name + "/DeleteFundsSource");
                        }
                    }
                    else
                        XtraMessageBox.Show("Seçilmiş mənbə bazada cəlb olunmuş vəsaitlərdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == FundsSourceUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş mənbə hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş mənbənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DeleteFundsSourceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteFundsSource();
            LoadFundsSourceDataGridView();
        }

        private void DeleteFundsSourceName()
        {
            int FundsSourceNameUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.FUNDS_SOURCES_NAME WHERE ID = {FundsSourceNameID}", this.Name + "/DeleteFundsSourceName");
            if (FundsSourceNameUsedUserID <= 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.FUNDS_CONTRACTS WHERE FUNDS_SOURCE_NAME_ID = {FundsSourceNameID}", this.Name + "/DeleteFundsSourceName");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş mənbəyin adını silmək istəyirsiniz?", "Mənbəyin adının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.FUNDS_SOURCES_NAME WHERE ID = {FundsSourceNameID}",
                                                            "Mənbənin adı silinmədi.",
                                                            this.Name + "/DeleteFundsSourceName");
                    }
                }
                else
                    XtraMessageBox.Show("Seçilmiş mənbəyin adı bazada cəlb olunmuş vəsaitlərdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == FundsSourceNameUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş mənbəyin adı hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş mənbəyin adının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteFundsSourceNameBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteFundsSourceName();
            LoadFundsSourceNameDataGridView();
        }

        private void FoundersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FoundersGridView, FoundersPopupMenu, e);
        }

        void RefreshFounder()
        {
            LoadFoundersDataGridView();
        }

        private void LoadFFounderAddEdit(string transaction, string founderid)
        {
            topindex = FoundersGridView.TopRowIndex;
            old_row_id = FoundersGridView.FocusedRowHandle;
            Dictionaries.FFounderAddEdit ffae = new Dictionaries.FFounderAddEdit();
            ffae.TransactionName = transaction;
            ffae.FounderID = founderid;
            ffae.RefreshFoundersDataGridView += new Dictionaries.FFounderAddEdit.DoEvent(RefreshFounder);
            ffae.ShowDialog();
            FoundersGridView.TopRowIndex = topindex;
            FoundersGridView.FocusedRowHandle = old_row_id;
        }

        private void NewFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFounderAddEdit("INSERT", null);
        }

        private void RefreshFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFoundersDataGridView();
        }

        private void FoundersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FoundersGridView.GetFocusedDataRow();
            if (row != null)
            {
                FounderID = row["ID"].ToString();
                UpFounderBarButton.Enabled = !(FoundersGridView.FocusedRowHandle == 0);
                DownFounderBarButton.Enabled = !(FoundersGridView.FocusedRowHandle == FoundersGridView.RowCount - 1);
            }
        }

        private void EditFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFFounderAddEdit("EDIT", FounderID);
        }

        private void FoundersGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditFounderBarButton.Enabled)
                LoadFFounderAddEdit("EDIT", FounderID);
        }

        private void DeleteFounder()
        {
            int FounderUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.FOUNDERS WHERE ID = {FounderID}", this.Name + "/DeleteFounder");
            if (FounderUsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = {FounderID}", this.Name + "/DeleteFounder");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təsisçini silmək istəyirsiniz?", "Təsisçinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_FOUNDER_DELETE", "P_FOUNDER_ID", int.Parse(FounderID), "Təsisçi silinmədi.");
                }
                else
                    XtraMessageBox.Show("Seçilmiş təsisçi bazada müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == FounderUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş təsisçi hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş təsisçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteFounder();
            LoadFoundersDataGridView();
        }

        private void UpFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FOUNDERS", FounderID, "up", out orderid);
            LoadFoundersDataGridView();
            FoundersGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownFounderBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("FOUNDERS", FounderID, "down", out orderid);
            LoadFoundersDataGridView();
            FoundersGridView.FocusedRowHandle = orderid - 1;
        }

        private void CashOtherAppointmentGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CashOtherAppointmentGridView, CashOtherAppointmentPopupMenu, e);
        }

        private void FoundersGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(FoundersGridView, e);
        }

        private void FundsSourcesGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(FundsSourcesGridView, e);
        }

        private void FundsSourcesNameGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(FundsSourcesNameGridView, e);
        }

        void RefreshCashAppointment()
        {
            LoadCashOtherAppointmentDataGridView();
        }

        private void LoadFCashOtherAppointmentAddEdit(string transaction, string id)
        {
            topindex = CashOtherAppointmentGridView.TopRowIndex;
            old_row_id = CashOtherAppointmentGridView.FocusedRowHandle;
            Dictionaries.FCashOtherAppointmentAddEdit fp = new Dictionaries.FCashOtherAppointmentAddEdit();
            fp.TransactionName = transaction;
            fp.ID = id;
            fp.RefreshAppointmentDataGridView += new Dictionaries.FCashOtherAppointmentAddEdit.DoEvent(RefreshCashAppointment);
            fp.ShowDialog();
            CashOtherAppointmentGridView.TopRowIndex = topindex;
            CashOtherAppointmentGridView.FocusedRowHandle = old_row_id;
        }

        private void NewCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCashOtherAppointmentAddEdit("INSERT", null);
        }

        private void CashAppointmentBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (FormStatus)
            {
                switch (CashAppointmentBackstageViewControl.SelectedTabIndex)
                {
                    case 0:
                        LoadCashOtherAppointmentDataGridView();
                        break;
                }
            }
        }

        private void EditCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCashOtherAppointmentAddEdit("EDIT", CashOtherAppointmentID);
        }

        private void CashOtherAppointmentGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CashOtherAppointmentGridView.GetFocusedDataRow();
            if (row != null)
            {
                CashOtherAppointmentID = row["ID"].ToString();
                UpCashOtherAppointmentBarButton.Enabled = !(CashOtherAppointmentGridView.FocusedRowHandle == 0);
                DownCashOtherAppointmentBarButton.Enabled = !(CashOtherAppointmentGridView.FocusedRowHandle == CashOtherAppointmentGridView.RowCount - 1);
            }
        }

        private void RefreshCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCashOtherAppointmentDataGridView();
        }

        private void UpCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CASH_APPOINTMENTS", CashOtherAppointmentID, "up", out orderid);
            LoadCashOtherAppointmentDataGridView();
            CashOtherAppointmentGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("CASH_APPOINTMENTS", CashOtherAppointmentID, "down", out orderid);
            LoadCashOtherAppointmentDataGridView();
            CashOtherAppointmentGridView.FocusedRowHandle = orderid - 1;
        }

        private void CashOtherAppointmentGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditCashOtherAppointmentBarButton.Enabled)
                LoadFCashOtherAppointmentAddEdit("EDIT", CashOtherAppointmentID);
        }

        private void CashOtherAppointmentGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(CashOtherAppointmentGridView, e);
        }

        private void DeleteCashOtherAppointment()
        {
            int AppointmentUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CASH_APPOINTMENTS WHERE TYPE = 1 AND ID = {CashOtherAppointmentID}");
            if (AppointmentUsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CASH_APPOINTMENT_ID FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT UNION SELECT CASH_APPOINTMENT_ID FROM CRS_USER.CASH_OTHER_PAYMENT) WHERE CASH_APPOINTMENT_ID = " + CashOtherAppointmentID);
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş təyinatı silmək istəyirsiniz?", "Təyinatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CASH_APPOINTMENTS WHERE TYPE = 1 AND ID = {CashOtherAppointmentID}", "Təyinat silinmədi.", this.Name + "/DeleteCashOtherAppointment");
                    }
                }
                else
                    XtraMessageBox.Show("Seçilmiş təyinat bazada kassa əməliyyatlarında istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == AppointmentUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş təyinat hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş təyinatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteCashOtherAppointmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCashOtherAppointment();
            LoadCashOtherAppointmentDataGridView();
        }

        void RefreshPositions()
        {
            LoadPositionsDataGridView();
        }

        private void LoadFPositionAddEdit(string transaction, string id)
        {
            topindex = PositionGridView.TopRowIndex;
            old_row_id = PositionGridView.FocusedRowHandle;
            Dictionaries.FPositionAddEdit fp = new Dictionaries.FPositionAddEdit();
            fp.TransactionName = transaction;
            fp.PositionID = id;
            fp.RefreshPositionDataGridView += new Dictionaries.FPositionAddEdit.DoEvent(RefreshPositions);
            fp.ShowDialog();
            PositionGridView.TopRowIndex = topindex;
            PositionGridView.FocusedRowHandle = old_row_id;
        }

        private void NewPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPositionAddEdit("INSERT", null);
        }

        private void PositionGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PositionGridView.GetFocusedDataRow();
            if (row != null)
            {
                PositionID = row["ID"].ToString();
                UpPositionBarButton.Enabled = !(PositionGridView.FocusedRowHandle == 0);
                DownPositionBarButton.Enabled = !(PositionGridView.FocusedRowHandle == PositionGridView.RowCount - 1);
            }
        }

        private void EditPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPositionAddEdit("EDIT", PositionID);
        }

        private void PositionGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPositionBarButton.Enabled)
                LoadFPositionAddEdit("EDIT", PositionID);
        }

        private void RefreshPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPositionsDataGridView();
        }

        private void UpPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("POSITIONS", PositionID, "up", out orderid);
            LoadPositionsDataGridView();
            PositionGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("POSITIONS", PositionID, "down", out orderid);
            LoadPositionsDataGridView();
            PositionGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeletePosition()
        {
            int PositionUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.POSITIONS WHERE ID = {PositionID}");
            if (PositionUsedUserID < 0)
            {
                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PERSONNEL WHERE POSITION_ID = {PositionID}");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş vəzifəni silmək istəyirsiniz?", "Vəzifənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.POSITIONS WHERE ID = {PositionID}", "Vəzifə silinmədi.", this.Name + "/DeletePosition");
                    }
                }
                else
                    XtraMessageBox.Show("Seçilmiş vəzifə bazada işçilərin siyahısında istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PositionUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş vəzifə hal-hazırda " + used_user_name + " tərəfindən istifadə ediliyi üçün silinə bilməz.", "Seçilmiş vəzifənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeletePositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePosition();
            LoadPositionsDataGridView();
        }

        private void PositionGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PositionGridView, PositionPopupMenu, e);
        }

        private void PositionGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(PositionGridView, e);
        }

        private void PersonnelGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PersonnelGridView, PersonnelPopupMenu, e);
        }

        void RefreshPersonnel()
        {
            LoadPersonnelDataGridView();
        }

        private void LoadFPersonnelAddEdit(string transaction, string personnel_id)
        {
            topindex = PersonnelGridView.TopRowIndex;
            old_row_id = PersonnelGridView.FocusedRowHandle;
            Dictionaries.FPersonnelAddEdit fcae = new Dictionaries.FPersonnelAddEdit();
            fcae.TransactionName = transaction;
            fcae.PersonnelID = personnel_id;
            fcae.RefreshPersonnelDataGridView += new Dictionaries.FPersonnelAddEdit.DoEvent(RefreshPersonnel);
            fcae.ShowDialog();
            PersonnelGridView.TopRowIndex = topindex;
            PersonnelGridView.FocusedRowHandle = old_row_id;
        }

        private void NewPersonnelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelAddEdit("INSERT", null);
        }

        private void PersonnelGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PersonnelGridView.GetFocusedDataRow();
            if (row != null)
                PersonnelID = row["ID"].ToString();
        }

        private void EditPersonnelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelAddEdit("EDIT", PersonnelID);
        }

        private void PersonnelGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPersonnelBarButton.Enabled)
                LoadFPersonnelAddEdit("EDIT", PersonnelID);
        }

        private void RefreshPersonnelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPersonnelDataGridView();
        }

        private void DeletePersonnel()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CASH_SALARY WHERE PERSONNEL_ID = {PersonnelID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş işçinin məlumatlarını silmək istəyirsiniz?", "İşçininnin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_PERSONNEL_DELETE", "P_PERSONNEL_ID", PersonnelID, "İşçinin məlumatları silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş işçiyə əmək haqqı ödənildiyi üçün onun məlumatlarını silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeletePersonnelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int PersonnelUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.PERSONNEL WHERE ID = {PersonnelID}");
            if ((PersonnelUsedUserID == -1) || (GlobalVariables.V_UserID == PersonnelUsedUserID))
                DeletePersonnel();
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PersonnelUsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş işçinin məlumatları " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatlarını silmək olmaz.", "Seçilmiş işçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadPersonnelDataGridView();
        }

        private void PersonnelGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(PersonnelGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(16, PersonnelGridView, e);
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

        private void CreditNameGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteCreditNameBarButton.Enabled = EditCreditNameBarButton.Enabled = (CreditNameGridView.RowCount > 0);
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

        private void PersonnelGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeletePersonnelBarButton.Enabled = EditPersonnelBarButton.Enabled = (PersonnelGridView.RowCount > 0);
        }

        private void UpBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("BANKS", BankID, "up", out orderid);
            LoadBanksDataGridView();
            BanksGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("BANKS", BankID, "down", out orderid);
            LoadBanksDataGridView();
            BanksGridView.FocusedRowHandle = orderid - 1;
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

        private void BanksGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (Convert.ToInt32(BanksGridView.GetListSourceRowCellValue(rowIndex, "IS_USED")) == 1)
                e.Value = Properties.Resources.check_16;
            //else
            //    e.Value = null;
        }

        private void RefreshInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsuranceRateDataGridView();
        }

        void RefreshInsuranceRate()
        {
            LoadInsuranceRateDataGridView();
        }

        private void LoadFInsuranceRateAddEdit(string transaction, string rateID)
        {
            Dictionaries.FInsuranceRateAddEdit fr = new Dictionaries.FInsuranceRateAddEdit();
            fr.TransactionName = transaction;
            fr.RateID = rateID;
            fr.RefreshInsuranceRateDataGridView += new Dictionaries.FInsuranceRateAddEdit.DoEvent(RefreshInsuranceRate);
            fr.ShowDialog();
        }

        private void EditInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceRateAddEdit("EDIT", InsuranceRateID);
        }

        private void InsuranceRateGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InsuranceRateGridView.GetFocusedDataRow();
            if (row != null)
            {
                InsuranceRateID = row["ID"].ToString();
                UpInsuranceRateBarButton.Enabled = !(InsuranceRateGridView.FocusedRowHandle == 0);
                DownInsuranceRateBarButton.Enabled = !(InsuranceRateGridView.FocusedRowHandle == InsuranceRateGridView.RowCount - 1);
            }
        }

        private void UpInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("INSURANCE_RATE", InsuranceRateID, "up", out orderid);
            LoadInsuranceRateDataGridView();
            InsuranceRateGridView.FocusedRowHandle = orderid - 1;
        }

        private void DeleteInsuranceRate()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sığorta dərəcəsini silmək istəyirsiniz?", "Sığorta dərəcəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.INSURANCE_RATE WHERE ID = {InsuranceRateID}", "Model silinmədi.", this.Name + "/DeleteInsuranceRate");
            }
        }

        private void DeleteInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int InsuranceRateUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.INSURANCE_RATE WHERE ID = {InsuranceRateID}");
            if (InsuranceRateUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != InsuranceRateUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == InsuranceRateUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş sığorta dərəcəsi hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş sığorta dərəcəsinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteInsuranceRate();
            }
            else
                DeleteInsuranceRate();
            LoadInsuranceRateDataGridView();
        }

        private void InsuranceRateGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceRateGridView, InsuranceRatePopupMenu, e);
        }

        private void DownInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("INSURANCE_RATE", InsuranceRateID, "down", out orderid);
            LoadInsuranceRateDataGridView();
            InsuranceRateGridView.FocusedRowHandle = orderid - 1;
        }

        private void InsuranceRateGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditInsuranceRateBarButton.Enabled)
                LoadFInsuranceRateAddEdit("EDIT", InsuranceRateID);
        }

        private void InsuranceRateGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(InsuranceRateGridView, e);
        }

        private void InsuranceRateGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (InsuranceRateGridView.RowCount > 0)
            {
                DeleteInsuranceRateBarButton.Enabled =
                    EditInsuranceRateBarButton.Enabled = true;
                if (InsuranceRateGridView.FocusedRowHandle == 0)
                    UpInsuranceRateBarButton.Enabled = false;
                DownInsuranceRateBarButton.Enabled = (InsuranceRateGridView.RowCount > 1);
            }
            else
            {
                DeleteInsuranceRateBarButton.Enabled =
                    EditInsuranceRateBarButton.Enabled =
                    UpInsuranceRateBarButton.Enabled =
                    DownInsuranceRateBarButton.Enabled = false;
            }
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
            if(contractEvaluateIsDeleted == 1)
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

        private void AddInsuranceRateBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceRateAddEdit("INSERT", null);
        }

        private void BanksGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteBankBarButton.Enabled = EditBankBarButton.Enabled = (BanksGridView.RowCount > 0);
        }

        private void AppointmentsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteAppointmentBarButton.Enabled = EditAppointmentBarButton.Enabled = (AppointmentsGridView.RowCount > 0);
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

        private void FoundersGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (FoundersGridView.RowCount > 0)
            {
                DeleteFounderBarButton.Enabled =
                    EditFounderBarButton.Enabled = true;
                if (FoundersGridView.FocusedRowHandle == 0)
                    UpFounderBarButton.Enabled = false;
                DownFounderBarButton.Enabled = (FoundersGridView.RowCount > 1);
            }
            else
            {
                DeleteFounderBarButton.Enabled =
                    EditFounderBarButton.Enabled =
                    UpFounderBarButton.Enabled =
                    DownFounderBarButton.Enabled = false;
            }
        }

        private void PositionGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (PositionGridView.RowCount > 0)
            {
                DeletePositionBarButton.Enabled =
                    EditPositionBarButton.Enabled = true;
                if (PositionGridView.FocusedRowHandle == 0)
                    UpPositionBarButton.Enabled = false;
                DownPositionBarButton.Enabled = (PositionGridView.RowCount > 1);
            }
            else
            {
                DeletePositionBarButton.Enabled =
                    EditPositionBarButton.Enabled =
                    UpPositionBarButton.Enabled =
                    DownPositionBarButton.Enabled = false;
            }
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

        private void FundsSourcesGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (FundsSourcesGridView.RowCount > 0)
            {
                DeleteFundsSourceBarButton.Enabled =
                    EditFundsSourceBarButton.Enabled = true;
                if (FundsSourcesGridView.FocusedRowHandle == 0)
                    UpFundsSourceBarButton.Enabled = false;
                DownFundsSourceBarButton.Enabled = (FundsSourcesGridView.RowCount > 1);
            }
            else
            {
                DeleteFundsSourceBarButton.Enabled =
                    EditFundsSourceBarButton.Enabled =
                    UpFundsSourceBarButton.Enabled =
                    DownFundsSourceBarButton.Enabled = false;
            }
        }

        private void FundsSourcesNameGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (FundsSourcesNameGridView.RowCount > 0)
            {
                DeleteFundsSourceNameBarButton.Enabled =
                    EditFundsSourceNameBarButton.Enabled = true;
                if (FundsSourcesNameGridView.FocusedRowHandle == 0)
                    UpFundsSourceNameBarButton.Enabled = false;
                DownFundsSourceNameBarButton.Enabled = (FundsSourcesNameGridView.RowCount > 1);
            }
            else
            {
                DeleteFundsSourceNameBarButton.Enabled =
                    EditFundsSourceNameBarButton.Enabled =
                    UpFundsSourceNameBarButton.Enabled =
                    DownFundsSourceNameBarButton.Enabled = false;
            }
        }

        private void CashOtherAppointmentGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CashOtherAppointmentGridView.RowCount > 0)
            {
                DeleteCashOtherAppointmentBarButton.Enabled =
                    EditCashOtherAppointmentBarButton.Enabled = true;
                if (CashOtherAppointmentGridView.FocusedRowHandle == 0)
                    UpCashOtherAppointmentBarButton.Enabled = false;
                DownCashOtherAppointmentBarButton.Enabled = (CashOtherAppointmentGridView.RowCount > 1);
            }
            else
                DeleteCashOtherAppointmentBarButton.Enabled = EditCashOtherAppointmentBarButton.Enabled = UpCashOtherAppointmentBarButton.Enabled = DownCashOtherAppointmentBarButton.Enabled = false;
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

        private void CardSeriesTab_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {

        }

        private void RefreshInsuranceCompanyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsuranceCompanyDataGridView();
        }

        void RefreshCompany()
        {
            LoadInsuranceCompanyDataGridView();
        }

        private void LoadFInsuranceCompanyAddEdit(string transaction, string CompanyID)
        {
            topindex = InsuranceCompanyGridView.TopRowIndex;
            old_row_id = InsuranceCompanyGridView.FocusedRowHandle;
            Forms.Dictionaries.FInsuranceCompanyAddEdit fse = new Forms.Dictionaries.FInsuranceCompanyAddEdit();
            fse.TransactionName = transaction;
            fse.CompanyID = CompanyID;
            fse.RefreshInsuranceCompanyDataGridView += new Forms.Dictionaries.FInsuranceCompanyAddEdit.DoEvent(RefreshCompany);
            fse.ShowDialog();
            InsuranceCompanyGridView.TopRowIndex = topindex;
            InsuranceCompanyGridView.FocusedRowHandle = old_row_id;
        }

        private void NewInsuranceCompanybarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceCompanyAddEdit("INSERT", null);
        }

        private void InsuranceCompanyGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InsuranceCompanyGridView.GetFocusedDataRow();
            if (row != null)
            {
                CompanyID = row["ID"].ToString();
            }
        }

        private void EditInsuranceCompanyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceCompanyAddEdit("EDIT", CompanyID);
        }

        private void InsuranceCompanyGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditInsuranceCompanyBarButton.Enabled)
                LoadFInsuranceCompanyAddEdit("EDIT", CompanyID);
        }

        private void InsuranceCompanyGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(InsuranceCompanyGridView, e);
        }

        private void InsuranceCompanyGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteInsuranceCompanyBarButton.Enabled = EditInsuranceCompanyBarButton.Enabled = (InsuranceCompanyGridView.RowCount > 0);
        }

        private void DeleteInsuranceCompany()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT (*) FROM (SELECT COMPANY_ID FROM CRS_USER.INSURANCES WHERE COMPANY_ID = " + CompanyID + " UNION ALL " +
                                                                       "SELECT DISTINCT COMPANY_ID FROM CRS_USER_TEMP.INSURANCES_TEMP WHERE COMPANY_ID = " + CompanyID + ")");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş şirkəti silmək istəyirsiniz?", "Şirkətin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.INSURANCE_COMPANY WHERE ID = " + CompanyID, "Şirkət silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş şirkət bazada müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteInsuranceCompanyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int CompanyUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.INSURANCE_COMPANY WHERE ID = {CompanyID}");
            if (CompanyUsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != CompanyUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CompanyUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş şirkət hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün bu təsviri silmək olmaz.", "Seçilmiş şirkətin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteInsuranceCompany();
            }
            else
                DeleteInsuranceCompany();
            LoadInsuranceCompanyDataGridView();
        }

        private void InsuranceCompanyGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceCompanyGridView, InsuranceCompanyPopupMenu, e);
        }
    }
}