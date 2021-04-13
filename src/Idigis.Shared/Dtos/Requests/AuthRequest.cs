namespace Idigis.Shared.Dtos.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Hash { get; set; }
        public string ChurchId { get; set; }
        public string Name { get; set; }
    }
}
