namespace Idigis.Core.Domain.Dtos.Requests
{
    public class CreateChurchRequest
    {
        public CreateChurchRequest (string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
