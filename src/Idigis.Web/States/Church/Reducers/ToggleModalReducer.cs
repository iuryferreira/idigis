using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class ToggleModalActionHandler : ActionHandler<ToggleModalAction>
        {
            public ToggleModalActionHandler (IStore store) : base(store) { }

            private ChurchState State => Store.GetState<ChurchState>();


            public override async Task<Unit> Handle (ToggleModalAction action, CancellationToken cancellationToken)
            {
                switch (action.ModalName)
                {
                    case "ChangeName":
                        State.ShowChangeNameModal = !State.ShowChangeNameModal;
                        break;
                    case "ChangePassword":
                        State.SelectedChurch = action.Church;
                        State.ShowChangePasswordModal = !State.ShowChangePasswordModal;
                        break;
                }

                return await Unit.Task;
            }
        }
    }
}
