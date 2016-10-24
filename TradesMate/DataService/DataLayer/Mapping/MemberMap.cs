using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System;

namespace EF.Data.Mapping
{
    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            this.HasKey(p => p.Id);
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.FirstName).HasMaxLength(100);
            Property(t => t.MiddleName).HasMaxLength(100);
            Property(t => t.LastName).HasMaxLength(100);
            Property(t => t.MobileNumber).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Email).HasMaxLength(100);

          //  Property(t => t.UserId).HasMaxLength(128);

            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation

            this.HasMany<CompanyMember>(p => p.CompanyMembers)
                .WithRequired(p => p.Member)
                .HasForeignKey(p => p.MemberId);

            //table
            ToTable("Member");
        }
    }
}
