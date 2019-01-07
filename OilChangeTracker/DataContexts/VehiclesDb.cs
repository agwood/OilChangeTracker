using OilChangeTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace OilChangeTracker.DataContexts
{
    public class VehiclesDb : DbContext
    {
        public VehiclesDb()
            : base("DefaultConnection")
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }

    }
}