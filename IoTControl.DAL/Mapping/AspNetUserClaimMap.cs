﻿using System.Data.Entity.ModelConfiguration;
using IoTControl.Models;

namespace IoTControl.Common.Mapping
{
    public class AspNetUserClaimMap : EntityTypeConfiguration<AspNetUserClaim>
    {
        public AspNetUserClaimMap()
        {
            this.ToTable("AspNetUserClaims", "dbo");
            this.HasKey(e => e.Id);

            this.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(128);

            this.HasIndex(e => e.UserId)
                .HasName("IX_UserId");

            this.HasRequired(d => d.User)
                .WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId);
        }
    }
}