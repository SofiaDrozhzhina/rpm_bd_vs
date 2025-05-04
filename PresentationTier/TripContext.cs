using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PresentationTier;

public partial class TripContext : DbContext
{
    public TripContext()
    {
    }

    public TripContext(DbContextOptions<TripContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Itog> Itogs { get; set; }

    public virtual DbSet<PunktNaznach> PunktNaznaches { get; set; }

    public virtual DbSet<PunktOtpravki> PunktOtpravkis { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:/Users/sofia/OneDrive/Рабочий стол/Trip.db3");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Itog>(entity =>
        {
            entity.HasKey(e => e.IdItog);

            entity.ToTable("Itog");

            entity.HasIndex(e => e.IdItog, "IX_Itog_idItog").IsUnique();

            entity.Property(e => e.IdItog).HasColumnName("idItog");
            entity.Property(e => e.IdPunktNaznach).HasColumnName("idPunktNaznach");
            entity.Property(e => e.IdPunktOtpravki).HasColumnName("idPunktOtpravki");
            entity.Property(e => e.IdTransport).HasColumnName("idTransport");

            entity.HasOne(d => d.IdPunktNaznachNavigation).WithMany(p => p.Itogs)
                .HasForeignKey(d => d.IdPunktNaznach)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPunktOtpravkiNavigation).WithMany(p => p.Itogs)
                .HasForeignKey(d => d.IdPunktOtpravki)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdTransportNavigation).WithMany(p => p.Itogs)
                .HasForeignKey(d => d.IdTransport)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PunktNaznach>(entity =>
        {
            entity.HasKey(e => e.IdPunktNaznach);

            entity.ToTable("PunktNaznach");

            entity.HasIndex(e => e.IdPunktNaznach, "IX_PunktNaznach_idPunktNaznach").IsUnique();

            entity.Property(e => e.IdPunktNaznach).HasColumnName("idPunktNaznach");
        });

        modelBuilder.Entity<PunktOtpravki>(entity =>
        {
            entity.HasKey(e => e.IdPunktOtpravki);

            entity.ToTable("PunktOtpravki");

            entity.HasIndex(e => e.IdPunktOtpravki, "IX_PunktOtpravki_idPunktOtpravki").IsUnique();

            entity.Property(e => e.IdPunktOtpravki).HasColumnName("idPunktOtpravki");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.IdTransport);

            entity.ToTable("Transport");

            entity.HasIndex(e => e.IdTransport, "IX_Transport_idTransport").IsUnique();

            entity.Property(e => e.IdTransport).HasColumnName("idTransport");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
