// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using New_Web_Library.Data.Models;


namespace New_Web_Library.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
     

        public LoginModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
           
        }

        [BindProperty]
        public InputModel Input { get; set; }

        
        public string ReturnUrl { get; set; }

        
        [TempData]
        public string ErrorMessage { get; set; }

        
        public class InputModel
        {

            
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Books") : returnUrl;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl != "/")
                        return LocalRedirect(returnUrl);

                    return RedirectToAction("Index", "Books", new { area = "" });


                }

                else if (result.IsLockedOut)
                {

                    return RedirectToPage("./Lockout");
                }
                
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
