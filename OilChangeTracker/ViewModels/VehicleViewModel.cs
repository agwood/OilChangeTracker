using OilChangeTracker.Models;
using System.Collections.Generic;

namespace OilChangeTracker.ViewModels
{
    public class VehicleViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}