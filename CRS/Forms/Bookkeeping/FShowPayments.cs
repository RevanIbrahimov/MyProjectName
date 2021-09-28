using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.Utils;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Forms.Bookkeeping
{
    public partial class FShowPayments : DevExpress.XtraEditors.XtraForm
    {
        public FShowPayments()
        {
            InitializeComponent();
        }
        public string ContractID;
        string Currency, CustomerID, rate;
        double total, basic_amount, interest_amount, payment_interest_amount, calc_debt = 0, interest_debt = 0, Amount;

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void PaymentsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            int calculated = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CLEARING_CALCULATED"));
            if (calculated == 0)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor2);
            }
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("dd.MM.yyyy"));
        }

        private void PaymentsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void FShowPayments_Load(object sender, EventArgs e)
        {
            LoadContractDetails();
            if (Currency == "AZN")
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency;
            else
            {
                rate = Class.GlobalFunctions.GetName("SELECT CUR.VALUE||' '||CUR.CODE||' = '||C.CURRENCY_RATE||' AZN' FROM CRS_USER.CONTRACTS C,CRS_USER.CURRENCY CUR WHERE C.CURRENCY_ID = CUR.ID AND C.ID = " + ContractID);
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency + "   (" + rate + ")";
            }
            LoadPayments(Currency);
            
            List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
            if (lstContractExtend.Count > 0)
            {
                ExtendLabel.Visible = ExtendStartDateText.Visible = ExtendEndDateText.Visible = ExtendSimvolLabel.Visible = true;
                var extend = lstContractExtend.LastOrDefault();
                ExtendStartDateText.Text = extend.START_DATE.ToString("dd.MM.yyyy");
                ExtendEndDateText.Text = extend.END_DATE.ToString("dd.MM.yyyy");
            }
            else
                ExtendLabel.Visible = ExtendStartDateText.Visible = ExtendEndDateText.Visible = ExtendSimvolLabel.Visible = false;
        }

        private void LoadContractDetails()
        {
            string s = $@"SELECT CT.CODE || C.CODE CONTRACT_CODE,
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
                                   TO_CHAR (C.START_DATE, 'DD.MM.YYYY') SDATE,
                                   TO_CHAR (C.END_DATE, 'DD.MM.YYYY') EDATE,
                                   CUS.CODE,
                                   CUS.SURNAME || ' ' || CUS.NAME || ' ' || CUS.PATRONYMIC CUST,
                                   CUS.ID CUSTOMER_ID,
                                   CUR.CODE CUR_CODE,
                                   C.AMOUNT,
                                   CI.IMAGE
                              FROM CRS_USER.CONTRACTS C,
                                   CRS_USER.CREDIT_TYPE CT,
                                   CRS_USER.CUSTOMERS CUS,
                                   CRS_USER.CURRENCY CUR,
                                   CRS_USER.CUSTOMER_IMAGE CI
                             WHERE     C.CURRENCY_ID = CUR.ID
                                   AND C.CUSTOMER_ID = CUS.ID
                                   AND C.CREDIT_TYPE_ID = CT.ID
                                   AND CUS.ID = CI.CUSTOMER_ID(+)
                                   AND C.ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDetails", "Müqavilənin detalları açılmadı.");

            foreach (DataRow dr in dt.Rows)
            {
                ContractCodeText.Text = dr[0].ToString();
                PeriodText.Text = dr[1].ToString();
                InterestText.Text = dr[2].ToString();
                StartDateText.Text = dr[3].ToString();
                EndDateText.Text = dr[4].ToString();
                CustomerCodeText.Text = dr[5].ToString();
                CustomerNameText.Text = dr[6].ToString();
                CustomerID = dr[7].ToString();

                Currency = dr[8].ToString();
                Amount = Convert.ToDouble(dr[9].ToString());

                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    CustomerPictureBox.Image = Image.FromStream(stmBLOBData);
                    stmBLOBData.Close();
                }
                else
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            CustomerPictureBox.Properties.NullText = "Müştərinin şəkili";
                            break;
                        case "RU":
                            CustomerPictureBox.Properties.NullText = "Фотография клиентов";
                            break;
                        case "EN":
                            CustomerPictureBox.Properties.NullText = "Customer picture";
                            break;
                    }
                }
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadPayments(string currency)
        {
            string s = null,
                   sortType = (GlobalVariables.V_DefaultDateSort == 1) ? "DESC" : "ASC";

            s = $@"SELECT 1 SS,
                             CP.CUSTOMER_ID,
                             CP.CONTRACT_ID,
                             CP.PAYMENT_DATE,
                             CP.CURRENCY_RATE,
                             CP.PAYMENT_AMOUNT_AZN,
                             CP.PAYMENT_AMOUNT,
                             CP.BASIC_AMOUNT,
                             CP.DEBT,
                             CP.DAY_COUNT,
                             CP.INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_DEBT,
                             CP.TOTAL,
                             CP.ID,
                             PENALTY_AMOUNT,
                             (CASE WHEN CP.BANK_CASH = 'C' THEN 'Kassa' 
                                    WHEN CP.BANK_CASH = 'B' THEN 'Bank, ' || B.SHORT_NAME 
                                    WHEN CP.BANK_CASH = 'D' THEN 'Digər' 
                             ELSE NULL END) PAYMENT_TYPE,                             
                             BANK_CASH,
                             ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM,
                             CP.NOTE,
                             CP.INSURANCE_CHECK,
                             CP.INSURANCE_AMOUNT,
                             CP.CONTRACT_PERCENT,
                             CP.CLEARING_DATE,                             
                             CP.CLEARING_CALCULATED
                        FROM CRS_USER.CUSTOMER_PAYMENTS CP, CRS_USER.BANKS B
                       WHERE     CP.BANK_ID = B.ID(+)
                             AND CP.CONTRACT_ID = {ContractID}
                    ORDER BY CP.PAYMENT_DATE {sortType}, CP.ID {sortType}";

            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView", "Ödənişlər cədvələ yüklənmədi.");

            Payments_CurrencyRate.Visible = Payments_AmountAZN.Visible = !(currency == "AZN"); 
        }        

        private void PaymentsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Payments_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
            //if (Currency != "AZN")
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT_AZN", "Far", e);
            if (e.Column.FieldName == "TOTAL")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir
                {
                    basic_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("BASIC_AMOUNT").SummaryItem.SummaryValue);
                    calc_debt = Amount - basic_amount;
                    e.TotalValue = calc_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) // qaliq faizi hesabliyir
                {
                    interest_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    payment_interest_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("PAYMENT_INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    interest_debt = interest_amount - payment_interest_amount;
                    e.TotalValue = interest_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) // cemi hesabliyir
                {
                    e.TotalValue = interest_debt + calc_debt;
                    total = interest_debt + calc_debt;
                }
            }
        }
    }
}