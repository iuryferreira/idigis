using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class OfferState
    {
        public class ToggleModalAction : IAction
        {
            public ToggleModalAction (string modalName, Offer offer = null)
            {
                ModalName = modalName;
                Offer = offer;
            }

            public string ModalName { get; }
            public Offer Offer { get; }
        }

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

        public class EditOfferAction : IAction
        {
            public EditOfferAction (EditOfferRequest request)
            {
                Request = request;
            }

            public EditOfferRequest Request { get; }
        }

        public class DeleteOfferAction : IAction
        {
            public DeleteOfferAction (DeleteOfferRequest request)
            {
                Request = request;
            }

            public DeleteOfferRequest Request { get; }
        }

        public class LoadOffersAction : IAction
        {}
    }
}
