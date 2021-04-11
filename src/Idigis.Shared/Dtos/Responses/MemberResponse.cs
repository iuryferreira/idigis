using System;
using Idigis.Shared.Dtos.Types;

namespace Idigis.Shared.Dtos.Responses
{
    public class GetMemberResponse
    {
        public string Id { get; init; }
        public string FullName { get; init; }
        public DateTime? BirthDate { get; init; }
        public DateTime? BaptismDate { get; init; }
        public ContactType Contact { get; init; }
    }

    public class CreateMemberResponse
    {
        public string Id { get; init; }
        public string FullName { get; init; }
        public DateTime? BirthDate { get; init; }
        public DateTime? BaptismDate { get; init; }
        public ContactType Contact { get; init; }
    }

    public class EditMemberResponse
    {
        public string Id { get; init; }
        public string FullName { get; init; }
        public DateTime? BirthDate { get; init; }
        public DateTime? BaptismDate { get; init; }
        public ContactType Contact { get; init; }
    }

    public class DeleteMemberResponse
    {
    }
}
