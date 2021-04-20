using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Web.Helpers;
using Idigis.Web.Models;
using Idigis.Web.Services;
using MediatR;

namespace Idigis.Web.States
{
    public partial class FinanceState
    {
        public class LoadFinanceTypeReducer : ActionHandler<LoadFinanceTypeAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public LoadFinanceTypeReducer (IStore store,IHttpService http, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
                _http = http;
            }

            private FinanceState State => Store.GetState<FinanceState>();

            public override async Task<Unit> Handle (LoadFinanceTypeAction action, CancellationToken cancellationToken)
            {
                var financeTypeEncoded = await _storage.GetItemAsStringAsync("finance_type");
                if (string.IsNullOrEmpty(financeTypeEncoded))
                {
                    return await Unit.Task;
                }
                var financeType = CustomEncoder.Decode<FinanceType>(financeTypeEncoded);
                State.FinanceType = financeType;
                return await Unit.Task;
            }
        }
    }
}
