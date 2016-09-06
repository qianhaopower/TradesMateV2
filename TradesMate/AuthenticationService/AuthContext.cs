using AuthenticationService.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AuthenticationService
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {
     
        }

        public DbSet<ClientApplicaiton> ClientApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}