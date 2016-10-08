//using DataService.Entities;

//using Microsoft.AspNet.Identity.EntityFramework;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Linq;
//using System.Web;

//namespace DataService.Infrastructure
//{
//    public class AuthContext : IdentityDbContext<ApplicationUser>
//    {
//        public AuthContext()
//            : base("DbConnectionString")
//        {
//            Configuration.ProxyCreationEnabled = false;
//            Configuration.LazyLoadingEnabled = false;
//        }
//        public static AuthContext Create()
//        {
//            return new AuthContext();
//        }


//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {

//            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();



//            modelBuilder.Entity<ApplicationUser>()
//           .HasOptional(c => c.Client)
//           .WithRequired(d => d.User);



//            //turn off cascade delete globally
//            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
//            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<IdentityUser>()
//                 .ToTable("ApplicationUser");


//            //base.OnModelCreating(modelBuilder);

//            //modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
//            //modelBuilder.Entity<IdentityRole>().ToTable("SystemRoles");
//            //modelBuilder.Entity<IdentityUserRole>().ToTable("SystemUserRoles");
//            //modelBuilder.Entity<IdentityUserLogin>().ToTable("SystemUserLogins");
//            //modelBuilder.Entity<IdentityUserClaim>().ToTable("SystemUserClaims");



//        }
//        public DbSet<ClientApplicaiton> ClientApplications { get; set; }
//        public DbSet<RefreshToken> RefreshTokens { get; set; }
//    }

//}