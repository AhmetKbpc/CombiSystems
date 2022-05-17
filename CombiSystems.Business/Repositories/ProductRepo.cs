using CombiSystems.Business.Repositories.Abstracts.EntityFrameworkCore;
using CombiSystems.Core.Entities;
using CombiSystems.Data.EntityFramework;

namespace CombiSystems.Business.Repositories;

public class ProductRepo : RepositoryBase<Product, Guid>
{
    public ProductRepo(MyContext context) : base(context)
    {
    }
}