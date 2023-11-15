using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskAPI.DTO_s
{
    public class Login
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
//var addUserRole = await _userManager.AddToRoleAsync(currentUser, registrationDto.Role!)