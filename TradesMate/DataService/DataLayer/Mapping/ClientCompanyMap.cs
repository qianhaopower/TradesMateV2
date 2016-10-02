using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class ClientCompanyMap : EntityTypeConfiguration<ClientCompany>
    {
        public ClientCompanyMap()
        {
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            Property(t => t.ClientId).IsRequired();


            Property(t => t.CompanyId).IsRequired();

            //relation
            this.HasKey(c => new { c.ClientId, c.CompanyId });

            //table
            ToTable("ClientCompany");
        }
    }
}
