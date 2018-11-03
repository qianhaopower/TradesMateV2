using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class Propertymap : EntityTypeConfiguration<Property>
    {
        public Propertymap()
        {

            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Narrative).HasMaxLength(3000);
            Property(t => t.Condition).HasMaxLength(3000);
            Property(t => t.Comment).HasMaxLength(3000);
            Property(t => t.AddressId).IsOptional();
            Property(t => t.SystemPropertyCompanyId).IsOptional();


            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            Ignore(t => t.AddressDisplay);

            //relation

            this.HasMany<Section>(p => p.SectionList)
                .WithRequired(p => p.Property)
                .HasForeignKey(p => p.PropertyId);



            this.HasMany<PropertyAllocation>(p => p.PropertyAllocations)
                .WithRequired(p => p.Property)
                .HasForeignKey(p => p.PropertyId);



            this.HasMany<ClientProperty>(p => p.ClientProperties)
                .WithRequired(p => p.Property)
                .HasForeignKey(p => p.PropertyId);

            this.HasMany<PropertyCompany>(p => p.PropertyCompanies)
           .WithRequired(p => p.Property)
           .HasForeignKey(p => p.PropertyId);

            //table
            ToTable("Property");
        }
    }
}
