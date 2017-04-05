//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.ModelConfiguration;
//using EF.Data;

//namespace EF.Data.Mapping
//{
//    public class SubSectionMap : EntityTypeConfiguration<SubSection>
//    {
//        public SubSectionMap()
//        {
//            //property
//            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

//            Property(t => t.Name).HasMaxLength(100);
//            Property(t => t.Description).HasMaxLength(3000);



//            Property(t => t.AddedDate).IsRequired();
//            Property(t => t.ModifiedDate).IsRequired();

//            //relation

//            this.HasMany<WorkItem>(p => p.WorkItemList)
//                .WithRequired(p => p.SubSection)
//                .HasForeignKey(p => p.SubSectionId);

//            //table
//            ToTable("SubSection");
//        }
//    }
//}
