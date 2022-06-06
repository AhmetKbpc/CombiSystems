using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Payments
{
    public class InstallmentPriceModel
    {
        public string Price
        {
            get; set;
        }
        public string TotalPrice
        {
            get; set;
        }
        public int? InstallmentNumber
        {
            get; set;
        }
    }
}
