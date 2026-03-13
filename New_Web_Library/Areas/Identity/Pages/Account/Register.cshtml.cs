// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static New_Web_Library.GCommon.EntityValidations.Users;


namespace New_Web_Library.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;


        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;


        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }




        public class InputModel
        {



            [Required]
            [StringLength(FirstNameUserMaxLength, MinimumLength = FirstNameUserMinLength,
             ErrorMessage = "The field is required.")]

            public string FirstName { get; set; } = null!;


            [Required]
            [StringLength(LastNameUserMaxLength, MinimumLength = LastNameUserMinLength,
             ErrorMessage = "The field is required.")]
            public string LastName { get; set; } = null!;



            [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
            public string UserName { get; set; }



            [Range(UserMinAge, UserMaxAge, ErrorMessage = "Age must be between 5 and 120.")]
            public int Age { get; set; }

            [Required]
            [StringLength(AddressMaxLength, MinimumLength = AddressMinLength,
             ErrorMessage = "The field is required.")]
            public string Address { get; set; } = null!;


            [Required]
            [Phone]
            [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength,
             ErrorMessage = "The field is required.")]
            public string PhoneNumber { get; set; } = null!;


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(UserPasswordMaxLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UserPasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Books/Index");


            if (ModelState.IsValid)
            {


                if (await _userManager.FindByEmailAsync(Input.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already taken. Please use a different email.");
                    return Page();
                }

                bool isPhoneExist = await _userManager.Users.AnyAsync(u => u.PhoneNumber == Input.PhoneNumber);

                if (isPhoneExist)
                {
                    ModelState.AddModelError("Input.PhoneNumber", "Phone number already exists.");
                    return Page();

                }


                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Age = Input.Age;
                user.Address = Input.Address;
                user.PhoneNumber = Input.PhoneNumber;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {


                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    TempData["SuccessRegistration"] = "The user was successfully registered.";


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "Users");
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form




            return Page();

        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
