using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class FinanceState
    {
        public class ToggleModalActionHandler : ActionHandler<ToggleModalAction>
        {
            public ToggleModalActionHandler (IStore store) : base(store) { }

            private FinanceState State => Store.GetState<FinanceState>();


            public override async Task<Unit> Handle (ToggleModalAction action, CancellationToken cancellationToken)
            {
                State.ShowModal = !State.ShowModal;
                return await Unit.Task;
            }
        }
    }
}
