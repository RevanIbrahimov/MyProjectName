using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class JuridicalPersons
    {
        public int ID { get; set; }
        public string CODE { get; set; }        
        public string NAME { get; set; }
        public string LEADING_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string VOEN { get; set; }       
        public string NOTE { get; set; }
        public int USED_USER_ID { get; set; }
    }
}
