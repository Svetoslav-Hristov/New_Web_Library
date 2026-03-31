using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Interfaces;
using System.Security.Principal;


namespace New_Web_Library
{
    using static New_Web_Library.GCommon.EntityValidations;
    using static New_Web_Library.GCommon.EntityValidations.Admin;
    using static New_Web_Library.GCommon.EntityValidations.IdentitySession;
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();



            builder.Services.AddDefaultIdentity<User>(options =>
            {
                ConfigureIdentity(builder.Configuration, options);
            })
               .AddRoles<IdentityRole<Guid>>()
               .AddEntityFrameworkStores<LibraryDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {

                options.ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeOut);

                options.SlidingExpiration = true;

                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";


                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });



            builder.Services.AddControllersWithViews();

            RegisterRepositories(builder.Services);

            RegisterServices(builder.Services);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedAdmin(services);
                await SeedExtraData(services);
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Welcome/Error500"); // за 500
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/Welcome/Error{0}");

            app.MapControllerRoute(
                name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


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


            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = "User"
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

        private static async Task SeedExtraData(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
                throw new Exception("Admin not found!");

            if (!context.Topics.Any())
            {
                Topic[] topics =
            {
            new Topic {
                Title = "Best modern novels 2026",
                CategoryId = 1 ,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Top 10 classical books",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Favorite poets",
                CategoryId = 3 ,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Epic fantasy series",
                CategoryId = 4,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Modern short stories",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Contemporary novels discussion",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Shakespeare's works",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Greek and Roman classics",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Future tech and space exploration",
                CategoryId = 5,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "World War II novels",
                CategoryId = 6,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            },
            new Topic {
                Title = "Detective series discussion",
                CategoryId = 7,
                CreatedOn = DateTime.UtcNow,
                UserId = admin.Id
            }
            };

                context.Topics.AddRange(topics);
                await context.SaveChangesAsync();

            }

            if (!context.Posts.Any())
            {

                Post[] posts =
         {
            new Post {
                Title = "Modern novel discussion",
                Content = "Let's discuss the best modern novels of 2026.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = admin.Id
            },
            new Post {
                Title = "Classical books you love",
                Content = "Share your favorite classical books.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = admin.Id
            },
            new Post {
                Title = "Poetry recommendations",
                Content = "Which poets inspire you?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 3,
                UserId = admin.Id
            },
            new Post {
                Title = "Fantasy recommendations",
                Content = "Discuss your favorite fantasy series.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 4,
                UserId = admin.Id
            },new Post {
                Title = "Modern short story debate",
                Content = "Which modern short stories are worth reading?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = admin.Id
            },
            new Post {
                Title = "Contemporary novels insights",
                Content = "Share insights on contemporary novels you've read recently.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = admin.Id
            },

            new Post {
                Title = "Exploring classic literature",
                Content = "Let's explore the themes in classic literature.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = admin.Id
            },
            new Post {
                Title = "Favorite classic authors",
                Content = "Who are your favorite classic authors and why?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = admin.Id
            }
              };

                context.Posts.AddRange(posts);
                await context.SaveChangesAsync();
            }

            if (!context.Comments.Any())
            {

                Comment[] comments =
           {

                new Comment
                {
                    Content = "I think 2026 has some really strong releases already.",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment {
                    Content = "Any recommendations for modern drama novels?",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment {
                    Content = "I've recently read a great psychological novel, highly recommend!",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                 new Comment
                {
                    Content = "Modern literature is getting more diverse, which is awesome.",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                    Content = "Do you prefer physical books or eBooks?",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                    Content = "I feel like modern novels focus more on characters than plot.",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                    Content = "Can someone suggest a good mystery novel from 2026?",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                    
                    Content = "Audiobooks are also becoming very popular lately.",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                    
                    Content = "I love how modern authors experiment with storytelling.",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                },
                new Comment
                {
                   
                    Content = "Looking forward to your suggestions!",
                    CreatedOn = DateTime.UtcNow,
                    PostId = 1,
                    UserId = admin.Id
                }
                    };

                context.Comments.AddRange(comments);
                await context.SaveChangesAsync();
            }


        }


        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<ISystemRepository, SystemRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITopicRepository, TopicRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IPostRepository, PostRepository>();



        }
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();

            services.AddScoped<ISystemService, SystemService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IWelcomeService, WelcomeService>();

            services.AddScoped<ITopicService, TopicService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IPostService, PostService>();

        }
    }
}
