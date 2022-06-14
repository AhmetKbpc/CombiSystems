using CombiSystems.Business.Repositories.Abstracts;
using CombiSystems.Business.Services.Email;
using CombiSystems.Core.Emails;
using CombiSystems.Core.Entities;
using CombiSystems.Core.Identity;
using CombiSystems.Data.Identity;
using CombiSystems.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace CombiSystems.Web.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<Appointment,int> _appointmentRepo;

    public UserController(UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        SignInManager<ApplicationUser> signInManager, 
        IRepository<Appointment,int> appointmentRepo)
    {
        _userManager = userManager;
        _emailService = emailService;
        _signInManager = signInManager;
        _appointmentRepo = appointmentRepo;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RoleChange()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Appointment()
    {
        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        var list = _appointmentRepo.Get().ToList();
        var model = new List<Appointment>();
        foreach (var item in list)
        {
            if (user.Id ==item.UserId)
            {
                model.Add(item);
            }
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Appointment(Appointment model)
    {

        if (!ModelState.IsValid)
        {
            TempData["Fail"] = "Appointment creating is Failed!";
            return View(model);
        }

        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        
 
        var appointment = new Appointment
        {
            AppointmentAddress = model.AppointmentAddress,
            UserId = user.Id,
            Description = model.Description
        };
        var result=_appointmentRepo.Insert(appointment);
        _appointmentRepo.Save();
        TempData["Success"] = "Appointment creating is Success!";

        return RedirectToAction();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        if (!HttpContext.User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Home");
        }

        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        var model = new UpdateProfilePasswordViewModel
        {
            UserProfileVM = new UserProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name!,
                Surname = user.Surname!,
                PhoneNumber = user.PhoneNumber,
                Adress = user.Adress

            }
        };

        return View(model);
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        var model = new UpdateProfilePasswordViewModel
        {
            UserProfileVM = new UserProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name!,
                Surname = user.Surname!,
                PhoneNumber = user.PhoneNumber,
                Adress = user.Adress

            }
        };

        return View(model);
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditProfile(UpdateProfilePasswordViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found!");
            return View(model);
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);
        if (user.Email != model.UserProfileVM!.Email && !isAdmin)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.User);
            await _userManager.AddToRoleAsync(user, Roles.Passive);
            user.EmailConfirmed = false;

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = code }, protocol: Request.Scheme);

            var emailMessage = new MailModel()
            {
                To = new List<EmailModel> { new()
                {
                    Adress = model.UserProfileVM.Email,
                    Name = model.UserProfileVM.Name
                }},
                Body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here </a>.",
                Subject = "Confirm your email"
            };

            await _emailService.SendMailAsync(emailMessage);
        }


        user.Name = model.UserProfileVM.Name;
        user.Surname = model.UserProfileVM.Surname;
        user.Email = model.UserProfileVM.Email;
        user.UserName = model.UserProfileVM.Email;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["UpdSuccess"] = "Your profile has been updated successfully";
            var userl = await _userManager.FindByNameAsync(user.UserName);
            await _signInManager.SignInAsync(userl, true);

            HttpContext.Session.SetString("User", System.Text.Json.JsonSerializer.Serialize<UserProfileViewModel>(new UserProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RegisterDate = user.RegisterDate
            }));
        }
        else
        {
            var message = string.Join("<br>", result.Errors.Select(x => x.Description));
            TempData["UpdError"] = message;
        }

        return View(model);
    }



    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(UpdateProfilePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["PassError"] = "There has been an error.";
            return RedirectToAction(nameof(Profile));
        }

        var name = HttpContext.User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(name);
        var result = await _userManager.ChangePasswordAsync(user, model.ChangePasswordVM!.CurrentPassword, model.ChangePasswordVM.NewPassword);

        if (result.Succeeded)
        {
            TempData["PassSuccess"] = "Your password has been changed successfully";
        }
        else
        {
            var message = string.Join("<br>", result.Errors.Select(x => x.Description));
            TempData["PassError"] = message;
        }


        return RedirectToAction(nameof(EditProfile));
    }





}
