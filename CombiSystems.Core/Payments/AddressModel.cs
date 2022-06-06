using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Payments
{
    public class AddressModel
    {
        public string Id
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string ZipCode
        {
            get; set;
        }
        public string ContactName
        {
            get; set;
        }
        public string City
        {
            get; set;
        }
        public string Country
        {
            get; set;
        }
    }
}
