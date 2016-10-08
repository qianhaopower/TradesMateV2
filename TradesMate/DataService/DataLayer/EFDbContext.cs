using DataService.Entities;
using DataService.Infrastructure;
using EF.Data.Mapping;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace EF.Data
{
   public class EFDbContext :  IdentityDbContext<ApplicationUser>
    {

        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Section> Sections { get; set; }
        //public DbSet<SectionTemplate> SectionTemplates { get; set; }
        //public DbSet<SubSection> SubSections { get; set; }
        //public DbSet<SubSectionTemplate> SubSectionTemplates { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemTemplate> WorkItemTemplates { get; set; }

        public DbSet<ClientApplicaiton> ClientApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public EFDbContext()
           : base("name=DbConnectionString")
       {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;
        }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {
            
 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);

            }


            //modelBuilder.Entity<IdentityUser>()
            //     .ToTable("ApplicationUser");
            //modelBuilder.Entity<Client>();
            //modelBuilder.Entity<ApplicationUser>();
            //modelBuilder.Entity<ClientCompany>();
            //modelBuilder.Entity<Address>();
            //modelBuilder.Entity<Company>();
            //modelBuilder.Entity<Property>();
            //modelBuilder.Entity<Section>();
            //modelBuilder.Entity<WorkItem>();
            //modelBuilder.Entity<WorkItemTemplate>();

            //   modelBuilder.Configurations.Add(new ClientMap());


            //modelBuilder.Entity<SectionTemplate>();
            //modelBuilder.Entity<SubSection>();
            //modelBuilder.Entity<SubSectionTemplate>();

            //modelBuilder.Entity<ApplicationUser>();

            //modelBuilder.Entity<IdentityUserLogin>();
            //modelBuilder.Entity<IdentityRole>();
            //modelBuilder.Entity<IdentityUserRole>();




            //turn off cascade delete globally
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();


            modelBuilder.Entity<ApplicationUser>()
          .HasOptional(c => c.Client)
          .WithOptionalDependent(d => d.User);

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            base.OnModelCreating(modelBuilder);

        }
    }
}
