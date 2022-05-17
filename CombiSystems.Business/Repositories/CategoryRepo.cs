using CombiSystems.Business.Repositories.Abstracts.EntityFrameworkCore;
using CombiSystems.Core.Entities;
using CombiSystems.Data.EntityFramework;

namespace CombiSystems.Business.Repositories;

public class CategoryRepo : RepositoryBase<Category, int>
{
    public CategoryRepo(MyContext context) : base(context)
    {
    }
}