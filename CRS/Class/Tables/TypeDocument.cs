using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class DocumentType
    {
        public int ID { get; set; }
        public int? DOCUMENTGROUPID { get; set; }
        public int NORESIDENT { get; set; }
        public string NAME { get; set; }
        public string PTTRN { get; set; }
        public int PERSONTYPEID { get; set; }
        public int SORTID { get; set; }
    }
}
