﻿#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ludus_Stadium.Models
{
    public partial class ludusstadiumsContext : DbContext
    {
        public ludusstadiumsContext()
        {
        }

        public ludusstadiumsContext(DbContextOptions<ludusstadiumsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<stadium> stadia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<stadium>(entity =>
            {
                entity.ToTable("stadium");

                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.adress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.openingDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}