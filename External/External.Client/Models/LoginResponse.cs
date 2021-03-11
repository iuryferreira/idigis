namespace External.Client.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }
}
