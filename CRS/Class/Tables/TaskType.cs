using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class TaskType
    {
        public int ID { get; set; }
        public string TYPE_NAME { get; set; }
        public string CODE { get; set; }
        public int ORDER_ID { get; set; }
        public int USED_USER_ID { get; set; }
        public int IS_INTERNAL { get; set; }
    }
}
