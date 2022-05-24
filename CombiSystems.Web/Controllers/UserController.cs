using CombiSystems.Business.Services.Email;
using CombiSystems.Data.Identity;
using CombiSystems.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CombiSystems.Web.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public UserController(UserManager<ApplicationUser> userManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        var model = new UpdateProfilePasswordViewModel
        {
            UserProfileVM = new UserProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name!,
                Surname = user.Surname!,
                PhoneNumber=user.PhoneNumber,
                Adress=user.Adress

            }
        };

        return View(model);
    }

}
