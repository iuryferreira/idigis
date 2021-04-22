using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class ChurchState : State<ChurchState>
    {
        public List<Error> Errors { get; set; }
        public Church Church { get; private set; }
        public string Token { get; private set; }
        public bool Authenticated { get; private set; }
        public bool ShowChangeNameModal { get; set; }
        public bool ShowChangePasswordModal { get; set; }
        public Church SelectedChurch { get; set; }

        public override void Initialize ()
        {
            Church = new();
            Token = string.Empty;
            Authenticated = false;
            SelectedChurch = new();
            Errors = new();
        }
    }
}
