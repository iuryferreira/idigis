namespace Idigis.Core.Dtos.Responses
{
    public class CreateOfferResponse
    {
        public string Id { get; set; }
        public decimal Value { get; init; }
    }

    public class EditOfferResponse
    {
        public string Id { get; init; }
        public decimal Value { get; init; }
    }

    public class DeleteOfferResponse
    {
    }
}
