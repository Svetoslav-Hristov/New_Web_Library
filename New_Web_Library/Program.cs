using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using New_Library.Services.Core;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Interfaces;


namespace New_Web_Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            

            builder.Services.AddDefaultIdentity<User>(options =>
            {
                ConfigureIdentity(builder.Configuration, options);
            })
               .AddRoles<IdentityRole<Guid>>()
               .AddEntityFrameworkStores<LibraryDbContext>();

            builder.Services.AddControllersWithViews();
           
            builder.Services.AddScoped<IBooksService, BooksService>();

            builder.Services.AddScoped<ISystemsService, SystemsService>();

            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddScoped<IWelcomeService, WelcomeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Welcome}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
        
        private static void ConfigureIdentity(ConfigurationManager configuration,
            IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedPhoneNumber = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");
            options.Password.RequireDigit = configuration.GetValue<bool>("Identity:Password:RequireDigit");
            options.Password.RequiredLength = configuration.GetValue<int>("Identity:Password:RequiredLength");
            options.Password.RequiredUniqueChars = configuration.GetValue<int>("Identity:Password:RequiredUniqueChars");
            options.Password.RequireNonAlphanumeric = configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequireUppercase = configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireLowercase = configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.User.RequireUniqueEmail = true;



        }
    }
}
