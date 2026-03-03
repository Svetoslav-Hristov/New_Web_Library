using New_Web_Library.Data.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace New_Web_Library.Data.Models
{
    using static New_Web_Library.GCommon.EntityValidations.Users;
    public class User :IdentityUser<Guid>,IUser
    {


        [Required]
        [MaxLength(FirstNameUserMaxLength)]

        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameUserMaxLength)]
        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
        
        public bool IsBlocked { get; set; }

        public virtual ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
    }
}
