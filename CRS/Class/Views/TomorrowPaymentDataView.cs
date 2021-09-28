using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class TomorrowPaymentDataView
    {
        public int ContractID { get; set; }
        public string CustomerName { get; set; }
        public string CommitmentName { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public int Interest { get; set; }
        public decimal Amount { get; set; }
        public int UsedUserID { get; set; }
    }
}
