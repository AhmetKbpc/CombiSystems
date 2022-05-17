using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CombiSystems.Business.Repositories.Abstracts;
using CombiSystems.Core.Entities;
using CombiSystems.Web.ViewModels.Dashboard;

namespace CombiSystems.Web.Components
{
    public class CategoryReportViewComponent : ViewComponent
    {
        private readonly IRepository<Category, int> _categoryRepo;

        public CategoryReportViewComponent(IRepository<Category, int> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IViewComponentResult Invoke()
        {
            var data = _categoryRepo.Get()
                .Include(x => x.Products)
                .Select(x => new CategoryReportViewModel()
                {
                    Name = x.Name,
                    ProductCount = x.Products.Count
                }).ToList();

            return View(data);
        }
    }
}