using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class OfferState
    {
        public class ToggleModalActionHandler : ActionHandler<ToggleModalAction>
        {
            public ToggleModalActionHandler (IStore store) : base(store) { }

            private OfferState State => Store.GetState<OfferState>();


            public override async Task<Unit> Handle (ToggleModalAction action, CancellationToken cancellationToken)
            {
                switch (action.ModalName)
                {
                    case "Add":
                        State.ShowAddModal = !State.ShowAddModal;
                        break;
                    case "Delete":
                        State.SelectedOffer = action.OfferId;
                        State.ShowDeleteModal = !State.ShowDeleteModal;
                        break;
                }

                return await Unit.Task;
            }
        }
    }
}
