using Microsoft.AspNetCore.Mvc;

namespace CombiSystems.Web.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    } 
    
    public IActionResult Register()
    {
        return View();
    }
}
