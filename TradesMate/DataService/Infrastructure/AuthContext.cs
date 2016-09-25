using DataService.Entities;

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataService.Infrastructure
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext()
            : base("DbConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        public static AuthContext Create()
        {
            return new AuthContext();
        }

        public DbSet<ClientApplicaiton> ClientApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}