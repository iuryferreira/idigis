using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class OfferState
    {
        public class LoadFinanceTypeAction : IAction { }

        public class ToggleModalAction : IAction { }

        public class ChangeTypeAction : IAction
        {
            public ChangeTypeAction (FinanceType financeType)
            {
                FinanceType = financeType;
            }

            public FinanceType FinanceType { get; }
        }

        public class AddOfferAction : IAction
        {
            public AddOfferAction (CreateOfferRequest request)
            {
                Request = request;
            }
            public CreateOfferRequest Request { get; }
        }

        public class LoadOffersAction : IAction
        { }
    }
}
