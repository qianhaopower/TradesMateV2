using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class CompanyMemberMap : EntityTypeConfiguration<CompanyMember>
    {
        public CompanyMemberMap()
        {
            
             this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Role).IsRequired();
            Property(t => t.Confirmed).IsRequired();
            Property(t => t.CompanyId).IsRequired();
            Property(t => t.MemberId).IsRequired();
            Property(t => t.AllowedTradeTypesInternal).HasMaxLength(50);
            Ignore(t => t.AllowedTradeTypes);



            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            this.HasMany(c => c.PropertyAllocations)
           .WithRequired(p => p.CompanyMember)
           .HasForeignKey(c => c.CompanyMemberId);

            //table
            ToTable("CompanyMember");
        }
    }
}
