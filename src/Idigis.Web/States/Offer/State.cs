using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class OfferState : State<OfferState>
    {
        public List<Error> Errors { get; private set; }
        public List<Offer> Offers { get; private set; }
        public decimal TotalOffersInMonth { get; private set; }
        public Offer SelectedOffer { get; private set; }
        public bool ShowAddModal { get; private set; }
        public bool ShowDeleteModal { get; private set; }
        public bool ShowEditModal { get; private set; }

        public override void Initialize ()
        {
            Errors = new();
            Offers = new();
            SelectedOffer = new();
            ShowAddModal = false;
        }
    }
}
