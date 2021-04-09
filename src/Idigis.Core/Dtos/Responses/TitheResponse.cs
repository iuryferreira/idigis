using System;

namespace Idigis.Core.Dtos.Responses
{
    public class GetTitheResponse
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
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
