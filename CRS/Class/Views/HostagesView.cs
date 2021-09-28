using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class HostagesView
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public string HOSTAGE { get; set; }
        public decimal LIQUID_AMOUNT { get; set; }
        public decimal FIRST_PAYMENT { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public int CURRENCY_ID { get; set; }
        public string HOSTAGE_TYPE { get; set; }
    }
}
