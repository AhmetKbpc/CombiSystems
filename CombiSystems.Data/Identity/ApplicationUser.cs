using Microsoft.AspNetCore.Identity;

namespace CombiSystems.Data.Identity;

internal class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

}
