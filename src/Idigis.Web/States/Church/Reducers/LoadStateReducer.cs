using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Web.Helpers;
using Idigis.Web.Models;
using MediatR;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class LoadStateReducer : ActionHandler<LoadStateAction>
        {
            private readonly ILocalStorageService _storage;

            public LoadStateReducer (IStore store, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
            }

            private ChurchState State => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (LoadStateAction action, CancellationToken cancellationToken)
            {
                var church = await _storage.GetItemAsStringAsync("church");
                var token = await _storage.GetItemAsStringAsync("token");
                State.Church = CustomEncoder.Decode<Church>(church);
                State.Token = CustomEncoder.Decode<string>(token);
                if (!string.IsNullOrEmpty(token))
                {
                    State.Authenticated = true;
                }
                return await Unit.Task;
            }
        }
    }
}
