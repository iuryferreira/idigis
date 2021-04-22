using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using MediatR;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class SignoutActionReducer : ActionHandler<SignOutAction>
        {
            private readonly ILocalStorageService _storage;

            public SignoutActionReducer (IStore store, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
            }

            private ChurchState State => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (SignOutAction action, CancellationToken cancellationToken)
            {
                await _storage.ClearAsync();
                State.Authenticated = false;
                return await Unit.Task;
            }
        }
    }
}
