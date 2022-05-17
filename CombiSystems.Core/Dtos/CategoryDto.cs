
using CombiSystems.Core.Dtos.Abstracts;


namespace CombiSystems.Core.Dtos;

public class CategoryDto : BaseDto<int>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public IList<ProductDto>? Products { get; set; }
}
