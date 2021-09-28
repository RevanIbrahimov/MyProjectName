using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class CashOperations
    {
        public int ID { get; set; }
        public int DESTINATION_ID { get; set; }
        public int OPERATION_OWNER_ID { get; set; }
        public DateTime OPERATION_DATE { get; set; }
        public string CONTRACT_CODE { get; set; }
        public decimal INCOME { get; set; }
        public decimal EXPENSES { get; set; }
        public decimal DEBT { get; set; }
        public int IS_COMMIT { get; set; }
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }
    }
}
