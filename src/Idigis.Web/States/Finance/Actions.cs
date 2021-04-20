using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class FinanceState
    {
        public class LoadFinanceTypeAction : IAction {}

        public class ToggleModalAction : IAction {}

        public class ChangeTypeAction : IAction
        {
            public ChangeTypeAction (FinanceType financeType)
            {
                FinanceType = financeType;
            }

            public FinanceType FinanceType { get; }
        }

        public class LoadOffersAction : IAction
        {}
    }
}
