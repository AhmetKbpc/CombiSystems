using Microsoft.AspNetCore.Identity;
using CombiSystems.Business.MappingProfiles;
using CombiSystems.Business.Repositories;
using CombiSystems.Business.Repositories.Abstracts;
using CombiSystems.Business.Services.Email;
using CombiSystems.Core.Entities;
using CombiSystems.Data.EntityFramework;
using CombiSystems.Data.Identity;

namespace CombiSystems.Web.Extensions
{
    public static class ServiceExtesions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Identity

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<MyContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.SlidingExpiration = true;
            });

            #endregion

            services.AddTransient<IEmailService, SmtpEmailService>();

            services.AddScoped<IRepository<Product, int>, ProductRepo>();
            services.AddScoped<IRepository<Category, int>, CategoryRepo>();
            services.AddScoped<IRepository<Appointment, int>, AppointmentRepo>();

            services.AddAutoMapper(options =>
            {
                options.AddProfile<EntityMappingProfile>();
            });

            return services;
        }
    }
}