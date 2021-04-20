using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Web.Helpers;
using MediatR;

namespace Idigis.Web.States
{
    public partial class FinanceState
    {
        public class ChangeTypeActionHandler : ActionHandler<ChangeTypeAction>
        {
            private readonly ILocalStorageService _storage;

            public ChangeTypeActionHandler (IStore store, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
            }

            private FinanceState State => Store.GetState<FinanceState>();

            public override async Task<Unit> Handle (ChangeTypeAction action, CancellationToken cancellationToken)
            {
                State.FinanceType = action.FinanceType;
                await _storage.SetItemAsync("finance_type", CustomEncoder.Encode(State.FinanceType));
                return await Unit.Task;
            }
        }
    }
}
