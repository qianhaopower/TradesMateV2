//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.ModelConfiguration;
//using EF.Data;

//namespace EF.Data.Mapping
//{
//    public class SubSectionTemplateMap : EntityTypeConfiguration<SubSectionTemplate>
//    {
//        public SubSectionTemplateMap()
//        {
//            //property
//            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

//            Property(t => t.Name).HasMaxLength(100);
//            Property(t => t.Description).HasMaxLength(3000);



//            Property(t => t.AddedDate).IsRequired();
//            Property(t => t.ModifiedDate).IsRequired();

//            //relation

//            this.HasMany<WorkItemTemplate>(p => p.WorkItemTemplateList)
//                .WithRequired(p => p.SubSectionTemplate)
//                .HasForeignKey(p => p.SubSectionTemplateId);


//            this.HasMany<SubSection>(p => p.SubSectionList)
//                .WithRequired(p => p.TemplateRecord)
//                .HasForeignKey(p => p.SubSectionTemplateId);

//            //table
//            ToTable("SubSectionTemplate");
//        }
//    }
//}
