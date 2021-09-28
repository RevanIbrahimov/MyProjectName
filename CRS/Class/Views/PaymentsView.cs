using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class PaymentsView
    {
        public int SS { get; set; }
        public string PaymentDate { get; set; }
        public double CurrencyRate { get; set; }
        public double PaymentAmountAZN { get; set; }
        public double PaymentAmount { get; set; }
        public double BasicAmount { get; set; }
        public double Debt { get; set; }
        public int DayCount { get; set; }
        public double InterestAmount { get; set; }
        public double PaymentInterestAmount { get; set; }
        public double PaymentInterestDebt { get; set; }
        public double Total { get; set; }
    }
}
