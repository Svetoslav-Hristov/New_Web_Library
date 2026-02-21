using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using New_Library.Services.Core;
using New_Web_Library.Data;
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

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Welcome}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
