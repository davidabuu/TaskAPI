using System.ComponentModel.DataAnnotations;

namespace TaskAPI.DTO_s
{
    public class TokenRequest

    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
