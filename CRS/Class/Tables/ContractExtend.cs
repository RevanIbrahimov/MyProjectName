using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class ContractExtend
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public int MONTH_COUNT { get; set; }
        public int INTEREST { get; set; }        
        public Decimal DEBT { get; set; }
        public Decimal CURRENT_DEBT { get; set; }
        public Decimal INTEREST_DEBT { get; set; }
        public int CHECK_INTEREST_DEBT { get; set; }
        public Decimal MONTHLY_AMOUNT { get; set; }   
        public int VERSION { get; set; }
        public int PAYMENT_TYPE { get; set; }
        public string NOTE { get; set; }
        public int IS_CHANGE { get; set; }
    }
}
