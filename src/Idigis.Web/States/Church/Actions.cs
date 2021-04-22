using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class SigninAction : IAction
        {
            public SigninAction (LoginRequest request)
            {
                Request = request;
            }

            public LoginRequest Request { get; }
        }

        public class SignupAction : IAction
        {
            public SignupAction (CreateChurchRequest request)
            {
                Request = request;
            }

            public CreateChurchRequest Request { get; }
        }

        public class EditChurchAction : IAction
        {
            public EditChurchAction (EditChurchRequest request)
            {
                Request = request;
            }

            public EditChurchRequest Request { get; }
        }

        public class SignOutAction : IAction
        {}

        public class ToggleModalAction : IAction
        {
            public ToggleModalAction (string modalName, Church church = null)
            {
                ModalName = modalName;
                Church = church;
            }

            public string ModalName { get; }
            public Church Church { get; }
        }

        public class LoadStateAction : IAction
        {}
    }
}
