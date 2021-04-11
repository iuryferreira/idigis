namespace Idigis.Shared.Dtos.Responses
{
    public class GetChurchResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }

    public class CreateChurchResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }

    public class EditChurchResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }

    public class DeleteChurchResponse
    {
    }
}
