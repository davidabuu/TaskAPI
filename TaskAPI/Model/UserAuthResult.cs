namespace TaskAPI.Model
{
    public class UserAuthResult
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public bool? Result { get; set; }
        public string? RefreshToken { get; set; }
        public List<string>? Errors { get; set; }
    }
}
