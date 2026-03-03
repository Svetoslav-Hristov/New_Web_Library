using System.ComponentModel.DataAnnotations;

namespace New_Web_Library.ViewModels.User
{
    using static New_Web_Library.GCommon.EntityValidations.Users;
    public class UserFormModel
    {

        [Required]
        [StringLength(FirstNameUserMaxLength, MinimumLength = FirstNameUserMinLength,
         ErrorMessage = "The field is required.")]

        public string FirstName { get; set; } = null!;


        [Required]
        [StringLength(LastNameUserMaxLength, MinimumLength = LastNameUserMinLength,
         ErrorMessage = "The field is required.")]
        public string LastName { get; set; } = null!;



        [Required]
        [StringLength(UserNameMaxLength,MinimumLength =UserNameMinLength,
            ErrorMessage = "The field is required.")]
        public string UserName { get; set; } = null!;


        [Range(5, 120, ErrorMessage = "Age must be between 5 and 120.")]
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
        [StringLength(EmailAddressMaxLength, MinimumLength = EmailAddressMinLength,
         ErrorMessage = "The field is required.")]
        public string Email { get; set; } = null!;


        [Required]
        [DataType(DataType.Password,ErrorMessage = "The field is required.")]
        public string Password { get; set; } = null!;


    }
}
