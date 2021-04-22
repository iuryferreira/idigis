using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class TitheState
    {
        public class ToggleModalActionHandler : ActionHandler<ToggleModalAction>
        {
            public ToggleModalActionHandler (IStore store) : base(store) { }

            private TitheState State => Store.GetState<TitheState>();


            public override async Task<Unit> Handle (ToggleModalAction action, CancellationToken cancellationToken)
            {
                switch (action.ModalName)
                {
                    case "Add":
                        State.ShowAddModal = !State.ShowAddModal;
                        break;
                    case "Edit":
                        State.SelectedTithe = action.Tithe;
                        State.ShowEditModal = !State.ShowEditModal;
                        break;
                    case "Delete":
                        State.SelectedTithe = action.Tithe;
                        State.ShowDeleteModal = !State.ShowDeleteModal;
                        break;
                }

                return await Unit.Task;
            }
        }
    }
}
