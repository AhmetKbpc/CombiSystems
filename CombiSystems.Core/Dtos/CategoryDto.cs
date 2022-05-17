using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Dtos;

public class CategoryDto : BaseDto<int>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public IList<ProductDto>? Products { get; set; }
}
