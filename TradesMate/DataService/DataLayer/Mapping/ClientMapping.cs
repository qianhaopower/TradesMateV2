using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.FirstName).HasMaxLength(100);
            Property(t => t.MiddleName).HasMaxLength(100);
            Property(t => t.SurName).HasMaxLength(100);
            Property(t => t.MobileNumber).HasMaxLength(100);
            Property(t => t.Description).HasMaxLength(3000);
            Property(t => t.Email).HasMaxLength(100);

            Property(t => t.UserId).HasMaxLength(128);

            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation

            this.HasMany<Property>(p => p.Properties)
                .WithRequired(p => p.Client)
                .HasForeignKey(p => p.ClientId);

            this.HasMany(c => c.ClientCompanies)
                .WithRequired()
                .HasForeignKey(c => c.ClientId);



            this.HasRequired<DataService.Infrastructure.ApplicationUser>(p => p.User)
                .WithOptional(p => p.Client).WillCascadeOnDelete(false);


            //table
            ToTable("Client");
        }
    }
}
