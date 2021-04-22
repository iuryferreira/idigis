namespace Idigis.Shared.Dtos.Requests
{
    public class GetChurchRequest
    {
        public GetChurchRequest (string email, string id = "")
        {
            Id = id;
            Email = email;
        }

        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class CreateChurchRequest
    {
        public CreateChurchRequest ()
        {

        }

        public CreateChurchRequest (string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EditChurchRequest
    {
        public EditChurchRequest(){}

        public EditChurchRequest (string id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class DeleteChurchRequest
    {
        public DeleteChurchRequest (string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
