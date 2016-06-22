using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class WorkItemMap : EntityTypeConfiguration<WorkItem>
    {
        public WorkItemMap()
        {
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Qauntity);
            Property(t => t.TradeWorkType).IsRequired();



            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation


            //table
            ToTable("WorkItem");
        }
    }
}
