using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class OperationDebt
    {
        public int ID { get; set; }
        public DateTime DEBT_DATE { get; set; }
        public int YR_MNTH_DY { get; set; }        
        public string ACCOUNT { get; set; }       
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal CURRENT_DEBIT { get; set; }
        public decimal CURRENT_CREDIT { get; set; }
        public string NOTE { get; set; }
        public int PARENT_ID { get; set; }        
        public int IS_MANUAL { get; set; }
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }
    }
}
