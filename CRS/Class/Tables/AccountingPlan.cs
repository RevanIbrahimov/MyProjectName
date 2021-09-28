using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class AccountingPlan
    {
        public int ID { get; set; }
        public int ACCOUNT_NUMBER { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string SUB_ACCOUNT { get; set; }
        public string SUB_ACCOUNT_NAME { get; set; }
        public int ACCOUNT_TYPE_ID { get; set; }
        public int ACCOUNT_CATEGORY { get; set; }
        public int BANK_ID { get; set; }
        public string NOTE { get; set; }
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }
        public string ACCOUNT { get; set; }
    }
}
