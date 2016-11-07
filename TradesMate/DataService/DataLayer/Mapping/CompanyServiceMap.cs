using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class CompanyServiceMap : EntityTypeConfiguration<CompanyService>
    {
        public CompanyServiceMap()
        {
            
             this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.CompanyId).IsRequired();
            Property(t => t.Type).IsRequired();
            
            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            //table
            ToTable("CompanyService");
        }
    }
}
