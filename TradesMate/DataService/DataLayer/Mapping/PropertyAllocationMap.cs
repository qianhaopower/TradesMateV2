using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class PropertyAllocationMap : EntityTypeConfiguration<PropertyAllocation>
    {
        public PropertyAllocationMap()
        {
            
             this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation


            //table
            ToTable("PropertyAllocation");
        }
    }
}
