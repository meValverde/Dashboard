using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dashboard.Models
{
    public partial class indeklimaContext : DbContext
    {
        public indeklimaContext()
        {
        }

        public indeklimaContext(DbContextOptions<indeklimaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Temperatur> Temperaturs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:indeklima.database.windows.net,1433;Initial Catalog=indeklima;Persist Security Info=False;User ID=systemintegration;Password=mnadsp9gu32rklag3289#2knda;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temperatur>(entity =>
            {
                entity.HasKey(e => new { e.Dato, e.Tidspunkt })
                    .HasName("PK__Temperat__DAF567423CA3787A");

                entity.ToTable("Temperatur");

                entity.Property(e => e.Dato)
                    .HasColumnType("date")
                    .HasColumnName("dato");

                entity.Property(e => e.Tidspunkt)
                    .HasColumnType("time(0)")
                    .HasColumnName("tidspunkt");

                entity.Property(e => e.Grader)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("grader");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
