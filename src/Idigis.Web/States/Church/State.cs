using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class ChurchState : State<ChurchState>
    {
        public override void Initialize ()
        {
            Church = new();
            Token = string.Empty;
            Authenticated = false;
            Errors = new();
        }
        public List<Error> Errors { get; set; }
        public Church Church { get; private set; }
        public string Token { get; private set; }
        public bool Authenticated { get; private set; }
    }
}
