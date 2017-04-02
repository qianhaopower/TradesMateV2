using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System;

namespace EF.Data.Mapping
{
    public class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMap()
        {
            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name).HasMaxLength(500);
            Property(t => t.Url).HasMaxLength(500);
          
            Property(t => t.AddedDateTime).IsRequired();
            Property(t => t.ModifiedDateTime).IsRequired();

            //relation

            //table
            ToTable("Attachment");
        }
    }
}
