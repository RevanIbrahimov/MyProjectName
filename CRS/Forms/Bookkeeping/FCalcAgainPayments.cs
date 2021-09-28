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
    public partial class FCalcAgainPayments : DevExpress.XtraEditors.XtraForm
    {
        public FCalcAgainPayments()
        {
            InitializeComponent();
        }

        private void FCalcAgainPayments_Load(object sender, EventArgs e)
        {
            //MonthComboBox.SelectedIndex = DateTime.Today.Month;
            GlobalProcedures.FillComboBoxEdit(YearComboBox, "DIM_TIME", "DISTINCT YEAR_ID,YEAR_ID,YEAR_ID", "CALENDAR_DATE IN (SELECT DISTINCT PDATE FROM (SELECT START_DATE PDATE FROM CRS_USER.V_CONTRACTS UNION ALL SELECT PAYMENT_DATE PDATE FROM CRS_USER.V_CUSTOMER_LAST_PAYMENT)) AND YEAR_ID >= 2016 ORDER BY 1 DESC");
            YearComboBox.SelectedIndex = 0;
        }

        private void LoadPayments()
        {
            string s = $@"SELECT 1 SS,
                                   CONTRACT_CODE,
                                   START_DATE PDATE,
                                   'Alqı-Satqı ödənişi' PTYPE,
                                   C.CURRENCY_CODE
                              FROM CRS_USER.V_CONTRACTS C
                             WHERE     START_DATE BETWEEN TO_DATE ('{FromDateValue.Text}', 'DD.MM.YYYY')
                                                      AND TO_DATE ('{ToDateValue.Text}', 'DD.MM.YYYY')
                                   AND C.IS_COMMIT = 1
                                   AND C.CONTRACT_ID NOT IN (SELECT DISTINCT CONTRACT_ID
                                                               FROM CRS_USER.OPERATION_JOURNAL
                                                              WHERE ACCOUNT_OPERATION_TYPE_ID = 2)
                            UNION ALL
                            SELECT 1 SS,
                                   C.CONTRACT_CODE,
                                   CP.PAYMENT_DATE PDATE,
                                   'Lizinq ödənişi' PTYPE,
                                   C.CURRENCY_CODE
                              FROM CRS_USER.CUSTOMER_PAYMENTS CP, CRS_USER.V_CONTRACTS C
                             WHERE     CP.PAYMENT_DATE BETWEEN TO_DATE('{FromDateValue.Text}', 'DD.MM.YYYY')
                                                           AND TO_DATE('{ToDateValue.Text}', 'DD.MM.YYYY')
                                   AND CP.CONTRACT_ID = C.CONTRACT_ID
                                   AND CP.ID NOT IN (SELECT DISTINCT CUSTOMER_PAYMENT_ID
                                                       FROM CRS_USER.OPERATION_JOURNAL
                                                      WHERE ACCOUNT_OPERATION_TYPE_ID = 1)";

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadProfits");


                PaymentsGridControl.DataSource = dt;                
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Mənfəət və zərər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void YearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YearComboBox.SelectedIndex < 0)
                return;

            int month = 0, monthLastDay = 0;
            if (MonthComboBox.SelectedIndex > 0)
            {
                month = MonthComboBox.SelectedIndex;
                monthLastDay = DateTime.DaysInMonth(int.Parse(YearComboBox.Text), month);
            }

            FromDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), (month == 0) ? 1 : month, 1);
            ToDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), (month == 0) ? 12 : month, (monthLastDay == 0) ? 31 : monthLastDay);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPayments();
        }

        private void MonthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YearComboBox.SelectedIndex < 0)
                return;

            if (MonthComboBox.SelectedIndex < 0)
                return;

            if (MonthComboBox.SelectedIndex == 0)
            {
                YearComboBox_SelectedIndexChanged(sender, EventArgs.Empty);
                return;
            }

            int month = MonthComboBox.SelectedIndex,
            monthLastDay = DateTime.DaysInMonth(int.Parse(YearComboBox.Text), month);

            FromDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), month, 1);
            ToDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), month, monthLastDay);
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            PaymentsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalında jurnalda olmayan ödənişlər";
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            PaymentsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalında jurnalda olmayan ödənişlər";
            LoadPayments();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_ALL_PAYMENTS_INS_JOURNAL", "P_FROM_DATE", FromDateValue.Text, "P_TO_DATE", ToDateValue.Text, "Jurnalda olmayan lizinq ödənişləri təkrar olaraq hesablanıb jurnala daxil edilmədi.");
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_ALL_CONTRACTS_INS_JOURNAL", "P_FROM_DATE", FromDateValue.Text, "P_TO_DATE", ToDateValue.Text, "Jurnalda olmayan alqı-satqı ödənişləri təkrar olaraq hesablanıb jurnala daxil edilmədi.");
            this.Close();
        }

        private void PaymentsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
    }
}