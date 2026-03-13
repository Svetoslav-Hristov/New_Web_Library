using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Interfaces;


namespace New_Web_Library
{
    using static New_Web_Library.GCommon.EntityValidations.Admin;

    public class Program
    {
        public static async Task Main(string[] args)
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

            RegisterRepositories(builder.Services);

            RegisterServices(builder.Services);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedAdmin(services);
            }


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
        private static async Task SeedAdmin(IServiceProvider serviceProvider)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = adminRole
                });
            }

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new User
                {
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    UserName = adminEmail,
                    Email = adminEmail,
                    Age = adminAge,
                    Address = adminAddress,
                    PhoneNumber = adminPhone,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
            }
        }
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IBooksRepository, BooksRepository>();

            services.AddScoped<ISystemsRepository, SystemsRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<ITopicsRepository, TopicsRepository>();

            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<ICommentsRepository, CommentsRepository>();

            services.AddScoped<IPostsRepository, PostsRepository>();



        }
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBooksService, BooksService>();

            services.AddScoped<ISystemsService, SystemsService>();

            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IWelcomeService, WelcomeService>();

            services.AddScoped<ITopicService, TopicService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICommentsService, CommentsService>();

            services.AddScoped<IPostsService, PostsService>();

        }
    }
}
