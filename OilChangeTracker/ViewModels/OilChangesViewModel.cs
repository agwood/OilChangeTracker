using OilChangeTracker.Models;
using System.Collections.Generic;

namespace OilChangeTracker.ViewModels
{
    public class OilChangesViewModel
    {
        public IEnumerable<OilChange> OilChanges { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}