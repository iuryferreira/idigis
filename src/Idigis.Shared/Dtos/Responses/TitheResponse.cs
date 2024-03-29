using System;

namespace Idigis.Shared.Dtos.Responses
{
    public class GetTitheResponse
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string MemberName { get; set; }
        public string MemberId { get; set; }
    }

    public class CreateTitheResponse
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class EditTitheResponse
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class DeleteTitheResponse
    {
    }
}
