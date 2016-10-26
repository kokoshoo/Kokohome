using Microsoft.AspNet.Identity.EntityFramework;
using MySite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MySite.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {

            public IdentityDb()
                : base("DefaultConnection")
            {
            }

        public static IdentityDb Create()
            {
                return new IdentityDb();
            }
    }
}