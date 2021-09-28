using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class LeasingTotal
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public Decimal PAYMENT_AMOUNT { get; set; }
        public Decimal BASIC_AMOUNT { get; set; }
        public Decimal DEBT { get; set; }
        public int DAY_COUNT { get; set; }
        public Decimal INTEREST_AMOUNT { get; set; }
        public Decimal PAYMENT_INTEREST_AMOUNT { get; set; }
        public Decimal PAYMENT_INTEREST_DEBT { get; set; }
        public Decimal TOTAL { get; set; }
        public Decimal REQUIRED { get; set; }
        public Decimal DELAYS { get; set; }
        public Decimal NORM_DEBT { get; set; }
    }
}
