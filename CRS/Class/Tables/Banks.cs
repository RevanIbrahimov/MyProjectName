using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Banks
    {
        public int ID { get; set; }
        public string LONG_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string CODE { get; set; }
        public string SWIFT { get; set; }
        public string VOEN { get; set; }
        public string ACCOUNT { get; set; }
        public string CBAR_ACCOUNT { get; set; }
        public int IS_USED { get; set; }
        public int USED_USER_ID { get; set; }
    }
}
