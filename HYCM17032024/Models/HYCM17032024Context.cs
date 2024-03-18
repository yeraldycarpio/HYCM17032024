using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HYCM17032024.Models
{
    public partial class HYCM17032024Context : DbContext
    {
        public HYCM17032024Context()
        {
        }

        public HYCM17032024Context(DbContextOptions<HYCM17032024Context> options)
            : base(options)
        {
        }

        public virtual DbSet<DetFacturaVenta> DetFacturaVentas { get; set; } = null!;
        public virtual DbSet<FacturaVenta> FacturaVentas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=HYCM17032024;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetFacturaVenta>(entity =>
            {
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Producto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFactuVentaNavigation)
                    .WithMany(p => p.DetFacturaVenta)
                    .HasForeignKey(d => d.IdFactuVenta)
                    .HasConstraintName("FK__DetFactur__IdFac__398D8EEE");
            });

            modelBuilder.Entity<FacturaVenta>(entity =>
            {
                entity.Property(e => e.Cliente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Correlativo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaVenta).HasColumnType("date");

                entity.Property(e => e.TotalVenta).HasColumnType("decimal(10, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
