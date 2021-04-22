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

        public string Id { get; set; }
        public string ChurchId { get; set; }
    }

    public class ListMemberRequest
    {
        public ListMemberRequest (string churchId)
        {
            ChurchId = churchId;
        }

        public string ChurchId { get; set; }
    }

    public class CreateMemberRequest
    {
        public CreateMemberRequest ()
        {
            Contact = new();
        }

        public CreateMemberRequest (string churchId, string fullName, DateTime? birthDate = null,
            DateTime? baptismDate = null, ContactType contact = null)
        {
            ChurchId = churchId;
            FullName = fullName;
            BirthDate = birthDate;
            BaptismDate = baptismDate;
            Contact = contact;
        }

        public string ChurchId { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? BaptismDate { get; set; }
        public ContactType Contact { get; set; }
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

        public string ChurchId { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? BaptismDate { get; set; }
        public ContactType Contact { get; set; }
    }

    public class DeleteMemberRequest
    {
        public DeleteMemberRequest (string churchId, string id)
        {
            ChurchId = churchId;
            Id = id;
        }

        public string ChurchId { get; set; }
        public string Id { get; set; }
    }
}
