using CRS.Class.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class DelaysListDAL
    {
        public static List<DelaysListView> lstDelays = new List<DelaysListView>();

        public static void InsertDelays(int ss,
                                            string contractCode,
                                            string currency,
                                            string customer,
                                            string carNumber,
                                            string phone,
                                            double delays,
                                            double monthlyAmount)
        {
            lstDelays.Add(new DelaysListView()
            {
                SS = ss,
                ContractCode = contractCode,
                Currency = currency,
                Customer = customer,
                CarNumber = carNumber,
                Phone = phone,
                Amount = delays,
                MonthlyAmount = monthlyAmount
            });
        }

        public static void RemoveAllDelays()
        {
            lstDelays.Clear();
        }
    }
}
