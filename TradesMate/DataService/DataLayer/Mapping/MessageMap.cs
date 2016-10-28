using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            
             this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            this.HasOptional(p => p.Client).WithMany(p => p.Messages).HasForeignKey(p => p.ClientId);
            this.HasOptional(p => p.Property).WithMany(p => p.Messages).HasForeignKey(p => p.PropertyId);
            this.HasOptional(p => p.Company).WithMany(p => p.Messages).HasForeignKey(p => p.CompanyId);
            this.HasOptional(p => p.Member).WithMany(p => p.Messages).HasForeignKey(p => p.MemberId);


            this.HasOptional(p => p.MessageResponse).WithRequired(p => p.Message);
            //table
            ToTable("Message");
        }
    }
}
