using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Enrich.Models
{
    public partial class Users
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string NhsNumber { get; set; }
    }
}
