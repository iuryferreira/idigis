using System;

namespace Idigis.Core.Dtos.Requests
{
    public class CreateTitheRequest
    {
        public CreateTitheRequest (string churchId, string memberId, decimal value, DateTime date)
        {
            Value = value;
            Date = date;
            ChurchId = churchId;
            MemberId = memberId;
        }

        public string ChurchId { get; }
        public string MemberId { get; }
        public decimal Value { get; }
        public DateTime Date { get; }
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

        public string ChurchId { get; }
        public string MemberId { get; }
        public string Id { get; }
        public decimal Value { get; }
        public DateTime Date { get; }
    }

    public class DeleteTitheRequest
    {
        public DeleteTitheRequest (string churchId, string memberId, string id)
        {
            ChurchId = churchId;
            MemberId = memberId;
            Id = id;
        }

        public string ChurchId { get; }
        public string MemberId { get; }
        public string Id { get; }
    }
}
