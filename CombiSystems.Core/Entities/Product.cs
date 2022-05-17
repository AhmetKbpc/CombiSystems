using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Entities;

public class Product : BaseEntity<Guid>
{
    public string Name { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
