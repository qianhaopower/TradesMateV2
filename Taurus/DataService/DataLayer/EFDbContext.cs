﻿using DataService.Entities;
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


        public DbSet<ClientProperty> ClientProperties { get; set; }
        public DbSet<CompanyMember> CompanyMembers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<PropertyAllocation> PropertyAllocations { get; set; }
        public DbSet<PropertyCompany> PropertyCompanies { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageResponse> MessageResponses { get; set; }
        public DbSet<CompanyService> CompanyServices { get; set; }
        public DbSet<Attachment> Attchments { get; set; }
        public DbSet<EmailHistory> EmailHistories { get; set; }



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
            //turn off cascade delete globally
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();


            modelBuilder.Entity<ApplicationUser>()
              .HasOptional(c => c.Client)
              .WithOptionalDependent(d => d.User);


            modelBuilder.Entity<ApplicationUser>()
           .HasOptional(c => c.Member)
           .WithOptionalDependent(d => d.User);
            base.OnModelCreating(modelBuilder);

        }
    }
}
