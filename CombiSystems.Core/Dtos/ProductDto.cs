using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Dtos;

public class ProductDto : BaseDto<Guid>
{
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public int CategoryId { get; set; }

    public CategoryDto? Category { get; set; }
}
