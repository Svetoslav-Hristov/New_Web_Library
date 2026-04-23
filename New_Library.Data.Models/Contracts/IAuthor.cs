using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Data.Models.Contracts
{
    public interface IAuthor
    {


        public Guid Id { get; set; }
        string Name { get; set; }
        public string? Biography { get; set; }
        public string? ImageUrl { get; set; }

    }
}
