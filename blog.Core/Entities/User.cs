using System;
using Microsoft.AspNetCore.Identity;

namespace blog.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
