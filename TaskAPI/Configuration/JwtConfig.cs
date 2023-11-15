namespace TaskAPI.Configuration
{
    public class JwtConfig
    {
        public string? Secret { get; set; }
        public TimeSpan? ExpriryTimeFrame { get; set; }
    }
}
