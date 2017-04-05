using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class SectionMap : EntityTypeConfiguration<Section>
    {
        public SectionMap()
        {

            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Type).HasMaxLength(100);



            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            this.HasMany<WorkItem>(p => p.WorkItemList)
                .WithRequired(p => p.Section)
                .HasForeignKey(p => p.SectionId);

            //table
            ToTable("Section");
        }
    }
}
