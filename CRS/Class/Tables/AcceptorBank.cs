using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class AcceptorBank
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string SWIFT { get; set; }
        public string VOEN { get; set; }
        public string CBAR_ACCOUNT { get; set; }
        public int ORDER_ID { get; set; }
        public int USED_USER_ID { get; set; }
        public string ACCEPTOR_ACCOUNT { get; set; }
    }
}
