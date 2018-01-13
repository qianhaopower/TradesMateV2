using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System;

namespace EF.Data.Mapping
{
    public class EmailHistoryMapping : EntityTypeConfiguration<EmailHistory>
    {
        public EmailHistoryMapping()
        {
            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.From).HasMaxLength(200);
            Property(t => t.ToEmailAddress).HasMaxLength(200);
            Property(t => t.Subject).HasMaxLength(200);
            Property(t => t.Body);

            ToTable("EmailHistory");
        }
    }
}
