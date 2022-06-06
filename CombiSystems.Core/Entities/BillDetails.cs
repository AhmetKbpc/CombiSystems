using CombiSystems.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Entities;

public class BillDetails: BaseEntity<int>
{
    public int ProductId { get; set; }
    public decimal SalesAmount { get; set; }
    public double Count { get; set; }
    public int BillId { get; set; }
    public Bill? Bill { get; set; }

    public Product? Product { get; set; }

}
