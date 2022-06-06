using CombiSystems.Core.Entities.Abstracts;

namespace CombiSystems.Core.Entities;

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    public BillDetails? BillDetails { get; set; }

}
