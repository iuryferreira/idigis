using System.ComponentModel.DataAnnotations;

namespace Idigis.Shared.Dtos.Requests
{
    public class GetOfferRequest
    {
        public GetOfferRequest (string churchId, string id)
        {
            ChurchId = churchId;
            Id = id;
        }

        public string Id { get; set; }
        public string ChurchId { get; set; }
    }

    public class ListOfferRequest
    {
        public ListOfferRequest (string churchId)
        {
            ChurchId = churchId;
        }

        public string ChurchId { get; set; }
    }

    public class CreateOfferRequest
    {
        public CreateOfferRequest () { }

        public CreateOfferRequest (string churchId, decimal value)
        {
            Value = value;
            ChurchId = churchId;
        }

        public string ChurchId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Ele deve ser maior que zero.")]
        public decimal Value { get; set; }
    }

    public class EditOfferRequest
    {
        public EditOfferRequest (string churchId, string id, decimal value)
        {
            Id = id;
            Value = value;
            ChurchId = churchId;
        }

        public string Id { get; set; }
        public string ChurchId { get; set; }
        public decimal Value { get; set; }
    }

    public class DeleteOfferRequest
    {
        public DeleteOfferRequest (string churchId, string id)
        {
            Id = id;
            ChurchId = churchId;
        }

        public string Id { get; set; }
        public string ChurchId { get; set; }
    }
}
