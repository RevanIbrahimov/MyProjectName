using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class PaymentSchedules
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime MONTH_DATE { get; set; }
        public DateTime REAL_DATE { get; set; }
        public double MONTHLY_PAYMENT { get; set; }        
        public double BASIC_AMOUNT { get; set; }
        public double INTEREST_AMOUNT { get; set; }
        public double DEBT { get; set; }        
        public string CURRENCY_CODE { get; set; }       
        public int USED_USER_ID { get; set; }
        public int IS_CHANGE_DATE { get; set; }
        public int ORDER_ID { get; set; }
        public int VERSION { get; set; }
    }
}
