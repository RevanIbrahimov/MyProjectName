using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Payments
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime PAYMENT_DATE { get; set; }
        public decimal PAYMENT_AMOUNT { get; set; }
        public decimal PAYMENT_AMOUNT_AZN { get; set; }
        public decimal BASIC_AMOUNT { get; set; }
        public decimal DEBT { get; set; }
        public int DAY_COUNT { get; set; }
        public decimal ONE_DAY_INTEREST_AMOUNT { get; set; }
        public decimal INTEREST_AMOUNT { get; set; }
        public decimal PAYMENT_INTEREST_AMOUNT { get; set; }
        public decimal PAYMENT_INTEREST_DEBT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal CURRENCY_RATE { get; set; }
        public string PAYMENT_NAME { get; set; }
        public string BANK_CASH { get; set; }
        public int IS_CHANGE { get; set; }
        public int ORDER_ID { get; set; }
        public int IS_CHANGED_INTEREST { get; set; }
        public decimal CHANGED_PAY_INTEREST_AMOUNT { get; set; }
        public DateTime CLEARING_DATE { get; set; }
        public int IS_CLEARING { get; set; }
        public int CLEARING_CALCULATED { get; set; }
        public decimal PENALTY_DEBT { get; set; }
        public int IS_PENALTY { get; set; }
        public decimal PENALTY_AMOUNT { get; set; }
        public decimal PAYED_PENALTY { get; set; }
    }
}
