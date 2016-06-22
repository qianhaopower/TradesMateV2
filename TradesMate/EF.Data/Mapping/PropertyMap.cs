using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class Propertymap : EntityTypeConfiguration<Property>
    {
        public Propertymap()
        {
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Narrative).HasMaxLength(3000);
            Property(t => t.Condition).HasMaxLength(3000);
            Property(t => t.Comment).HasMaxLength(3000);


            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation

            this.HasMany<Section>(p => p.SectionList)
                .WithRequired(p => p.Property)
                .HasForeignKey(p => p.PropertyId);

            //table
            ToTable("Property");
        }
    }
}
