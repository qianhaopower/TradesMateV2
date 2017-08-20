using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class WorkItemMap : EntityTypeConfiguration<WorkItem>
    {
        public WorkItemMap()
        {
            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Quantity);
            Property(t => t.TradeWorkType).IsRequired();
            Property(t => t.Status).IsRequired();



            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation


            //table
            ToTable("WorkItem");
        }
    }
}
