using System.ComponentModel.DataAnnotations;

namespace CombiSystems.Web.ViewModels;

public class LoginViewModel
{
    
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email alanı gereklidir.")]
    public string Email { get; set; }

    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Şifre alanı gereklidir.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifreniz minimum 6 karakterli olmalıdır!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
