using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class MemberState
    {
        public class LoadMembersAction : IAction {}

        public class ToggleModalAction : IAction
        {
            public ToggleModalAction (string modalName, Member member = null)
            {
                ModalName = modalName;
                Member = member;
            }

            public string ModalName { get; }
            public Member Member { get; }
        }

        public class AddMemberAction : IAction
        {
            public AddMemberAction (CreateMemberRequest request)
            {
                Request = request;
            }

            public CreateMemberRequest Request { get; }
        }

        public class DeleteMemberAction : IAction
        {
            public DeleteMemberAction (DeleteMemberRequest request)
            {
                Request = request;
            }

            public DeleteMemberRequest Request { get; }
        }

        public class EditMemberAction : IAction
        {
            public EditMemberAction (EditMemberRequest request)
            {
                Request = request;
            }

            public EditMemberRequest Request { get; }
        }
    }
}
