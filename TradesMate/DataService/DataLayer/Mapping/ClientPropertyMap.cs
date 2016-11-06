using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class ClientPropertyMap : EntityTypeConfiguration<ClientProperty>
    {
        public ClientPropertyMap()
        {
            //property
            Property(t => t.Id);
            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            Property(t => t.ClientId).IsRequired();


            Property(t => t.PropertyId).IsRequired();

            Property(t => t.Role).IsRequired();
            Property(t => t.Confirmed).IsRequired();

            //relation
            // this.HasKey(c => new { c.ClientId, c.CompanyId });

            //table
            ToTable("ClientProperty");
        }
    }
}
