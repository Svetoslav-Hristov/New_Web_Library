// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using New_Web_Library.Data.Models;
using static New_Web_Library.GCommon.EntityValidations.Users;

namespace New_Web_Library.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public string Username { get; set; }

       
        [TempData]
        public string StatusMessage { get; set; }

        
        [BindProperty]
        public InputModel Input { get; set; }

       
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

            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var age = user.Age;
            var address = user.Address;

            Username = userName;

            Input = new InputModel
            {
                FirstName=firstName,
                LastName=lastName,
                UserName=userName,
                Age=age,
                Address=address,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

           
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Age = Input.Age;
            user.Address = Input.Address;
            user.PhoneNumber = Input.PhoneNumber;
            
            
            var updateResult = await _userManager.UpdateAsync(user);
            
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update profile.";
                return RedirectToPage();
            }
        

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
