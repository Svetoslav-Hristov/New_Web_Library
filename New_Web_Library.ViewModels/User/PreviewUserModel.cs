using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.User
{
    public class PreviewUserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        public string Address { get; set; } = null!;

        public string TelephoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsBlocked { get; set; }


    }
}
