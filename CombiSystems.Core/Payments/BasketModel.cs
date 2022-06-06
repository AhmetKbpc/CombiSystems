using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Payments
{
    public class BasketModel
    {
        public string Id
        {
            get; set;
        }
        public string Price
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Category1
        {
            get; set;
        }
        public string Category2
        {
            get; set;
        }
        public string ItemType
        {
            get; set;
        }
    }
}
