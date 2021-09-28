using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class LastPayment
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime PAYMENT_DATE { get; set; }
        public Decimal PAYMENT_AMOUNT { get; set; }
        public Decimal PAYMENT_AMOUNT_AZN { get; set; }
        public Decimal BASIC_AMOUNT { get; set; }
        public Decimal DEBT { get; set; }
        public int DAY_COUNT { get; set; }
        public Decimal ONE_DAY_INTEREST_AMOUNT { get; set; }
        public Decimal INTEREST_AMOUNT { get; set; }
        public Decimal PAYMENT_INTEREST_AMOUNT { get; set; }
        public Decimal PAYMENT_INTEREST_DEBT { get; set; }
        public Decimal TOTAL { get; set; }
        public Decimal CURRENCY_RATE { get; set; }
        public string PAYMENT_NAME { get; set; }
        public string BANK_CASH { get; set; }
        public int ORDER_ID { get; set; }
    }
}
