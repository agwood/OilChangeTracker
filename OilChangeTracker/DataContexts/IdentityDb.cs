using Microsoft.AspNet.Identity.EntityFramework;
using OilChangeTracker.Entities;
using OilChangeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OilChangeTracker.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }
}