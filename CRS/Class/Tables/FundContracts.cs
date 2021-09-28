using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class FundContracts
    {
        public int ID { get; set; }
        public int FUNDS_SOURCE_ID { get; set; }
        public int FUNDS_SOURCE_NAME_ID { get; set; }
        public string CONTRACT_NUMBER { get; set; }
        public string REGISTRATION_NUMBER { get; set; }
        public decimal INTEREST { get; set; }
        public int PERIOD { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public int CURRENCY_ID { get; set; }
        public int STATUS_ID { get; set; }
        public string NOTE { get; set; }
        public int CHECK_END_DATE { get; set; }
        public DateTime CLOSED_DATE { get; set; }
        public int USED_USER_ID { get; set; }
    }
}
