using BlazorState;
using Idigis.Shared.Dtos.Requests;

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

        public class LoadStateAction : IAction { }
    }
}
