using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class TitheState
    {
        public class ToggleModalAction : IAction
        {
            public ToggleModalAction (string modalName, Tithe offer = null)
            {
                ModalName = modalName;
                Tithe = offer;
            }

            public string ModalName { get; }
            public Tithe Tithe { get; }
        }

        public class ChangeTypeAction : IAction
        {
            public ChangeTypeAction (FinanceType financeType)
            {
                FinanceType = financeType;
            }

            public FinanceType FinanceType { get; }
        }

        public class AddTitheAction : IAction
        {
            public AddTitheAction (CreateTitheRequest request)
            {
                Request = request;
            }

            public CreateTitheRequest Request { get; }
        }

        public class EditTitheAction : IAction
        {
            public EditTitheAction (EditTitheRequest request)
            {
                Request = request;
            }

            public EditTitheRequest Request { get; }
        }

        public class DeleteTitheAction : IAction
        {
            public DeleteTitheAction (DeleteTitheRequest request)
            {
                Request = request;
            }

            public DeleteTitheRequest Request { get; }
        }

        public class LoadTithesAction : IAction
        {}
    }
}
