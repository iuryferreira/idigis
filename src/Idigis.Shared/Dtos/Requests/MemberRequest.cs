using System;
using Idigis.Shared.Dtos.Types;

namespace Idigis.Shared.Dtos.Requests
{
    public class GetMemberRequest
    {
        public GetMemberRequest (string churchId, string id)
        {
            ChurchId = churchId;
            Id = id;
        }

        public string Id { get; }
        public string ChurchId { get; }
    }

    public class ListMemberRequest
    {
        public ListMemberRequest (string churchId)
        {
            ChurchId = churchId;
        }

        public string ChurchId { get; }
    }

    public class CreateMemberRequest
    {
        public CreateMemberRequest (string churchId, string fullName, DateTime? birthDate = null,
            DateTime? baptismDate = null, ContactType contact = null)
        {
            ChurchId = churchId;
            FullName = fullName;
            BirthDate = birthDate;
            BaptismDate = baptismDate;
            Contact = contact;
        }

        public string ChurchId { get; }
        public string FullName { get; }
        public DateTime? BirthDate { get; }
        public DateTime? BaptismDate { get; }
        public ContactType Contact { get; }
    }

    public class EditMemberRequest
    {
        public EditMemberRequest (string churchId, string id, string fullName, DateTime? birthDate = null,
            DateTime? baptismDate = null, ContactType contact = null)
        {
            ChurchId = churchId;
            Id = id;
            FullName = fullName;
            BirthDate = birthDate;
            BaptismDate = baptismDate;
            Contact = contact;
        }

        public string ChurchId { get; }
        public string Id { get; }
        public string FullName { get; }
        public DateTime? BirthDate { get; }
        public DateTime? BaptismDate { get; }
        public ContactType Contact { get; }
    }

    public class DeleteMemberRequest
    {
        public DeleteMemberRequest (string churchId, string id)
        {
            ChurchId = churchId;
            Id = id;
        }

        public string ChurchId { get; }
        public string Id { get; }
    }
}
