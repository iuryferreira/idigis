namespace Idigis.Core.Dtos.Requests
{
    public class CreateOfferRequest
    {
        public CreateOfferRequest (string churchId, decimal value)
        {
            Value = value;
            ChurchId = churchId;
        }

        public string ChurchId { get; }
        public decimal Value { get; }
    }

    public class EditOfferRequest
    {
        public EditOfferRequest (string churchId, string id, decimal value)
        {
            Id = id;
            Value = value;
            ChurchId = churchId;
        }

        public string Id { get; }
        public string ChurchId { get; }
        public decimal Value { get; }
    }

    public class DeleteOfferRequest
    {
        public DeleteOfferRequest (string churchId, string id)
        {
            Id = id;
            ChurchId = churchId;
        }

        public string Id { get; }
        public string ChurchId { get; }
    }
}
