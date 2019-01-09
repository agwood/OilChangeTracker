using OilChangeTracker.Models;
using System.Collections.Generic;

namespace OilChangeTracker.ViewModels
{
    public class OilChangesViewModel
    {
        public IEnumerable<OilChangeEvent> OilChangeEvents { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}