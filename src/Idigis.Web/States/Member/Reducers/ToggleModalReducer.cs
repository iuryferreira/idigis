using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class MemberState
    {
        public class ToggleModalReducer : ActionHandler<ToggleModalAction>
        {
            public ToggleModalReducer (IStore store) : base(store) { }
            private MemberState State => Store.GetState<MemberState>();

            public override async Task<Unit> Handle (ToggleModalAction action, CancellationToken cancellationToken)
            {
                switch (action.ModalName)
                {
                    case "Add":
                        State.ShowAddModal = !State.ShowAddModal;
                        break;
                    case "View":
                        State.SelectedMember = action.Member;
                        State.ShowViewModal = !State.ShowViewModal;
                        break;
                    case "Edit":
                        State.SelectedMember = action.Member;
                        State.ShowEditModal = !State.ShowEditModal;
                        break;
                    case "Delete":
                        State.SelectedMember = action.Member;
                        State.ShowDeleteModal = !State.ShowDeleteModal;
                        break;
                }
                return await Unit.Task;
            }
        }
    }
}
