using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class BankOperations
    {
       public int ID { get; set; }
       public int BANK_ID { get; set; }
       public int BRANCH_ID { get; set; }
       public int ACCOUNT_ID { get; set; }
       public int  ACCOUNTING_PLAN_ID { get; set; }
       public DateTime OPERATION_DATE { get; set; }
       public int APPOINTMENT_ID { get; set; }
       public decimal INCOME { get; set; }
       public decimal EXPENSES { get; set; }
       public decimal DEBT { get; set; }
       public decimal DEBT_BANK { get; set; }
       public string  NOTE { get; set; }
       public int CONTRACT_PAYMENT_ID { get; set; }
       public string CONTRACT_CODE { get; set; }
       public int FUNDS_PAYMENT_ID { get; set; }
       public int FUNDS_CONTRACT_ID { get; set; }
       public int USED_USER_ID { get; set; }
    }
}
