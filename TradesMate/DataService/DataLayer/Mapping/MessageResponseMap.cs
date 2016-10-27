using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class MessageResponseMap : EntityTypeConfiguration<MessageResponse>
    {
        public MessageResponseMap()
        {
            
             this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation
            this.HasRequired(p => p.Message).WithOptional(p => p.MessageResponse);
           
            //table
            ToTable("MessageResponse");
        }
    }
}
