using CRS.Class.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PaymentsViewDAL
    {
        public static List<PaymentsView> lstPayments = new List<PaymentsView>();

        public static void InsertPayment(int ss, 
                                            string paymentDate, 
                                            double currencyRate, 
                                            double paymentAmountAZN, 
                                            double paymentAmount,
                                            double basicAmount,
                                            double debt,
                                            int dayCount,
                                            double interestAmount,
                                            double paymentInterestAmount,
                                            double paymentInterestDebt,
                                            double total)
        {
            lstPayments.Add(new PaymentsView() { SS = ss,
                                                    PaymentDate = paymentDate,
                                                    CurrencyRate = currencyRate,
                                                    PaymentAmountAZN = paymentAmountAZN,
                                                    PaymentAmount = paymentAmount,
                                                    BasicAmount = basicAmount,
                                                    Debt = debt,
                                                    DayCount = dayCount,
                                                    InterestAmount = interestAmount,
                                                    PaymentInterestAmount = paymentInterestAmount,
                                                    PaymentInterestDebt = paymentInterestDebt,
                                                    Total = total});
        }

        public static void RemoveAllPayments()
        {
            lstPayments.Clear();
        }
    }
}
