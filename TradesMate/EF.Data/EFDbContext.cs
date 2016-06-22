using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace EF.Data
{
   public class EFDbContext : DbContext
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
        public EFDbContext()
           : base("name=DbConnectionString")
       {
         
       }

     

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType
            //     && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.Configurations.Add(configurationInstance);

            //}
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Address>();
            modelBuilder.Entity<Company>();
            modelBuilder.Entity<Property>();
            modelBuilder.Entity<Section>();
            //modelBuilder.Entity<SectionTemplate>();
            //modelBuilder.Entity<SubSection>();
            //modelBuilder.Entity<SubSectionTemplate>();
            modelBuilder.Entity<WorkItem>();
            modelBuilder.Entity<WorkItemTemplate>();
            


            //turn off cascade delete globally
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
       }
    }
}
