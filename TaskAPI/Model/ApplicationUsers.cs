
using Microsoft.AspNetCore.Identity;

namespace TaskAPI.Model
{
    public class ApplicationUsers : IdentityUser
    {


        public string? FirstName { get; set; }

        public string? LastName { get; set; }


        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}
