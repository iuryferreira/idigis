namespace Idigis.Core.Dtos.Requests
{
    public class GetChurchRequest
    {
        public GetChurchRequest (string email, string id = "")
        {
            Id = id;
            Email = email;
        }

        public string Id { get; }
        public string Email { get; }
    }

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

    public class EditChurchRequest
    {
        public EditChurchRequest (string id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
    }

    public class DeleteChurchRequest
    {
        public DeleteChurchRequest (string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
