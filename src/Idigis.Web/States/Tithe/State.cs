using System.Collections.Generic;
using BlazorState;
using Idigis.Web.Models;

namespace Idigis.Web.States
{
    public partial class TitheState : State<TitheState>
    {
        public List<Error> Errors { get; private set; }
        public List<Tithe> Tithes { get; private set; }
        public decimal TotalTithesInMonth { get; private set; }
        public Tithe SelectedTithe { get; private set; }
        public bool ShowAddModal { get; private set; }
        public bool ShowDeleteModal { get; private set; }
        public bool ShowEditModal { get; private set; }

        public override void Initialize ()
        {
            Errors = new();
            Tithes = new();
            SelectedTithe = new();
        }
    }
}
