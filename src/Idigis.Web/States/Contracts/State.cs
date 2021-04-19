using System;

namespace Idigis.Web.States.Contracts
{
    public abstract class State
    {
        public event Action OnChange;
        private void NotifyStateChanged () => OnChange?.Invoke();
    }
}
