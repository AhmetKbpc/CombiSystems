using CombiSystems.Business.Services.Email;
using CombiSystems.Core.Emails;
using CombiSystems.Core.Identity;
using CombiSystems.Data.Identity;
using CombiSystems.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace CombiSystems.Web.Controllers;

public class HomeController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _emailService = emailService;
        _roleManager = roleManager;
        _signInManager = signInManager;
        CheckRoles();
    }

    private void CheckRoles()
    {
        foreach (var item in Roles.RoleList)
        {
            if (_roleManager.RoleExistsAsync(item).Result)
                continue;
            var result = _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = item
            }).Result;
        }
    }


    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("~/Register")]
    public IActionResult Register()
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost("~/Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Error!");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            PhoneNumber = model.PhoneNumber,
            Adress = model.Adress

        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            //Rol Atama
            var count = _userManager.Users.Count();
            result = await _userManager.AddToRoleAsync(user, count == 1 ? Roles.Admin : Roles.Passive);

            //Email gönderme - Aktivasyon
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            var email = new MailModel()
            {
                To = new List<EmailModel>
                {
                    new EmailModel()
                        { Adress = user.Email, Name = user.UserName }
                },
                Body =
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                Subject = "Confirm your email"
            };

            await _emailService.SendMailAsync(email);
            //TODO: Login olma
            return RedirectToAction("Login");
        }
        else
        {
            TempData["Message"] = result.Errors.Select(x => x.Description).Last();

        }

        var messages = string.Join("<br>", result.Errors.Select(x => x.Description));
        ModelState.AddModelError(string.Empty, messages);
        return View(model);
    }


    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        ViewBag.StatusMessage =
            result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

        if (result.Succeeded && _userManager.IsInRoleAsync(user, Roles.Passive).Result)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.Passive);
            await _userManager.AddToRoleAsync(user, Roles.User);
        }

        return View();
    }



    [HttpGet("~/Login")]
    public IActionResult Login()
    {

        if (HttpContext.User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }


    [HttpPost("~/Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

        if (result.Succeeded)
        {
            var user = _userManager.FindByNameAsync(model.Email).Result;
            

            //model.ReturnUrl = string.IsNullOrEmpty(model.ReturnUrl) ? "~/" : model.ReturnUrl;

            //model.ReturnUrl = model.ReturnUrl ?? Url.Action("Index", "Home");

            model.ReturnUrl ??= Url.Content("~/");

            return LocalRedirect(model.ReturnUrl);
        }
        else if (result.IsLockedOut)
        {
        }
        else if (result.RequiresTwoFactor)
        {
        }

        ModelState.AddModelError(string.Empty, "Username or password is incorrect");
        return View(model);
    }


    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [HttpGet("~/ResetPassword")]
    public IActionResult ResetPassword()
    {
        return View();
    }



    [HttpPost("~/ResetPassword")]
    public async Task<IActionResult> ResetPassword(string email)
    {
        if (email == null)
        {
            TempData["Message"] = "Email is Required!";
        }
        else
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["OKMessage"] = "Password update mail is sent to you.";
            }
            else
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmResetPassword", "Home", new { userId = user.Id, code = code },
                    Request.Scheme);

                var emailMessage = new MailModel()
                {
                    To = new List<EmailModel>
                {
                    new EmailModel()
                        { Adress = user.Email, Name = user.UserName }
                },
                    Body =
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Reset Password"
                };

                // await _emailService.SendMailAsync(emailMessage);

                TempData["OKMessage"] = "Password update mail is sent to you.";

            }
        }
        return View();
    }


    [HttpGet("~/ConfirmResetPassword")]
    public IActionResult ConfirmResetPassword(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return BadRequest("Bad Request");
        }

        ViewBag.Code = code;
        ViewBag.UserId = userId;
        return View();
    }

    [HttpPost("~/ConfirmResetPassword")]
    public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
    {
        if (model.Code == null || model.UserId == null)
        {
            TempData["Message"] = "Invalid Token!";
            return View(model);

        }
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User coud not be found!");
            return View();
        }
        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
        var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);
        if (result.Succeeded)
        {
            //email gönder
            TempData["OKMessage"] = "Password change is successful.";
            return RedirectToAction("Login", "Home");
        }
        else
        {
            var message = string.Join("<br>", result.Errors.Select(x => x.Description));
            TempData["Message"] = message;
            return View();
        }
    }
}



