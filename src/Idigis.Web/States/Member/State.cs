using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class MemberState : State<MemberState>
    {
        public List<Member> Members { get; private set; }
        public int TotalMembers { get; private set; }
        public Member SelectedMember { get; private set; }
        public List<Error> Errors { get; private set; }
        public bool ShowAddModal { get; private set; }
        public bool ShowDeleteModal { get; private set; }
        public bool ShowEditModal { get; private set; }
        public bool ShowViewModal { get; private set; }

        public override void Initialize ()
        {
            Members = new();
            SelectedMember = new();
            Errors = new();
            ShowAddModal = false;
        }
    }
}
