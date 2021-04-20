using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class FinanceState : State<FinanceState>
    {
        public List<Error> Errors { get; private set; }
        public List<Offer> Offers { get; private set; }
        public decimal TotalOffersInMonth { get; private set; }
        public bool ShowModal { get; private set; }
        public FinanceType FinanceType { get; private set; }

        public override void Initialize ()
        {
            Errors = new();
            Offers = new();
            ShowModal = false;
            FinanceType = FinanceType.Offer;
        }
    }
}
