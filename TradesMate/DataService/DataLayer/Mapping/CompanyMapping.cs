using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {

            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.CreditCard).HasMaxLength(50);



            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            this.HasMany<WorkItemTemplate>(p => p.WorkItemTemplateList)
                .WithRequired(p => p.Company)
                .HasForeignKey(p => p.CompanyId);


            this.HasMany(c => c.PropertyCompanies)
           .WithRequired(p => p.Company)
           .HasForeignKey(c => c.CompanyId);

            this.HasMany(c => c.CompanyMembers)
           .WithRequired(p => p.Company)
           .HasForeignKey(c => c.CompanyId);

            this.HasMany(c => c.CompanyServices)
              .WithRequired(p => p.Company)
              .HasForeignKey(c => c.CompanyId);



            //this will allow EF generate the join table. 
            //this.HasMany<Property>(s => s.Properties)
            //  .WithMany(c => c.Companies)
            //  .Map(cs =>
            //  {
            //      cs.MapLeftKey("CompanyId");
            //      cs.MapRightKey("PropertyId");
            //      cs.ToTable("CompanyProperty");
            //  });


            //table
            ToTable("Compnany");
        }
    }
}
