using System;

namespace Idigis.Shared.Dtos.Requests
{
    public class GetTitheRequest
    {
        public GetTitheRequest (string churchId, string memberId, string id)
        {
            ChurchId = churchId;
            Id = id;
            MemberId = memberId;
        }

        public string Id { get; set; }
        public string ChurchId { get; set; }
        public string MemberId { get; set; }
    }

    public class ListTitheRequest
    {
        public ListTitheRequest (string churchId, string memberId)
        {
            ChurchId = churchId;
            MemberId = memberId;
        }

        public string ChurchId { get; set; }
        public string MemberId { get; set; }
    }

    public class CreateTitheRequest
    {
        public CreateTitheRequest (string churchId, string memberId, decimal value, DateTime date)
        {
            Value = value;
            Date = date;
            ChurchId = churchId;
            MemberId = memberId;
        }

        public string ChurchId { get; set; }
        public string MemberId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class EditTitheRequest
    {
        public EditTitheRequest (string churchId, string memberId, string id, decimal value, DateTime date)
        {
            Value = value;
            Date = date;
            ChurchId = churchId;
            MemberId = memberId;
            Id = id;
        }

        public string ChurchId { get; set; }
        public string MemberId { get; set; }
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class DeleteTitheRequest
    {
        public DeleteTitheRequest (string churchId, string memberId, string id)
        {
            ChurchId = churchId;
            MemberId = memberId;
            Id = id;
        }

        public string ChurchId { get; set; }
        public string MemberId { get; set; }
        public string Id { get; set; }
    }
}
