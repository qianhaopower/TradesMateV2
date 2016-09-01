﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EF.Data;

namespace EF.Data.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            //property
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Line1).HasMaxLength(100).IsRequired();
            Property(t => t.Line2).HasMaxLength(100);
            Property(t => t.Line3).HasMaxLength(100);
            Property(t => t.PostCode).HasMaxLength(20).IsRequired();
            Property(t => t.State).HasMaxLength(100).IsRequired();
            Property(t => t.Suburb).HasMaxLength(100).IsRequired();
            Property(t => t.City).HasMaxLength(100);


            Property(t => t.AddedDate).IsRequired();
            Property(t => t.ModifiedDate).IsRequired();

            //relation

            this.HasMany<Company>(p => p.CompanyList)
                .WithOptional(p => p.Address)
                .HasForeignKey(p => p.AddressId);

            this.HasMany<Property>(p => p.PropertyList)
              .WithRequired(p => p.Address)
              .HasForeignKey(p => p.AddressId);

            this.HasMany<Client>(p => p.ClientList)
              .WithOptional(p => p.Address)
              .HasForeignKey(p => p.AddressId);

            //table
            ToTable("Address");
        }
    }
}