using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace poliklinikaDemo.Models;

public partial class StolyarovaContext : DbContext
{
    public StolyarovaContext()
    {
    }

    public StolyarovaContext(DbContextOptions<StolyarovaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentsAndPatient> AppointmentsAndPatients { get; set; }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=79.174.88.58;Port=16639;Database=Stolyarova;Username=Stolyarova;Password=Stolyarova123.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("en_US.UTF-8")
            .HasPostgresExtension("pg_stat_statements");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointId).HasName("appointments_pkey");

            entity.ToTable("appointments", "poliklinika");

            entity.Property(e => e.AppointId).HasColumnName("appoint_id");
            entity.Property(e => e.AppointReason).HasColumnName("appoint_reason");
            entity.Property(e => e.SchedId).HasColumnName("sched_id");

            entity.HasOne(d => d.Sched).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.SchedId)
                .HasConstraintName("appointments_sched_id_fkey");
        });

        modelBuilder.Entity<AppointmentsAndPatient>(entity =>
        {
            entity.HasKey(e => e.AppointId).HasName("appoint_key");

            entity.ToTable("appointments_and_patient", "poliklinika");

            entity.Property(e => e.AppointId)
                .ValueGeneratedNever()
                .HasColumnName("appoint_id");
            entity.Property(e => e.PatinId).HasColumnName("patin_id");

            entity.HasOne(d => d.Appoint).WithOne(p => p.AppointmentsAndPatient)
                .HasForeignKey<AppointmentsAndPatient>(d => d.AppointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_and_patient_appoint_id_fkey");

            entity.HasOne(d => d.Patin).WithMany(p => p.AppointmentsAndPatients)
                .HasForeignKey(d => d.PatinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_and_patient_patin_id_fkey");
        });

        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.CabId).HasName("cabinets_pkey");

            entity.ToTable("cabinets", "poliklinika");

            entity.Property(e => e.CabId)
                .ValueGeneratedNever()
                .HasColumnName("cab_id");
            entity.Property(e => e.CabNumber).HasColumnName("cab_number");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("doctors_pkey");

            entity.ToTable("doctors", "poliklinika");

            entity.Property(e => e.DocId).HasColumnName("doc_id");
            entity.Property(e => e.CabId).HasColumnName("cab_id");
            entity.Property(e => e.DocFname).HasColumnName("doc_fname");
            entity.Property(e => e.DocSname).HasColumnName("doc_sname");
            entity.Property(e => e.Doclname).HasColumnName("doclname");

            entity.HasOne(d => d.Cab).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.CabId)
                .HasConstraintName("doctors_cab_id_fkey");

            entity.HasMany(d => d.Roles).WithMany(p => p.Docs)
                .UsingEntity<Dictionary<string, object>>(
                    "DocAndRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("doc_and_roles_role_id_fkey"),
                    l => l.HasOne<Doctor>().WithMany()
                        .HasForeignKey("DocId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("doc_and_roles_doc_id_fkey"),
                    j =>
                    {
                        j.HasKey("DocId", "RoleId").HasName("doc_roles_key");
                        j.ToTable("doc_and_roles", "poliklinika");
                        j.IndexerProperty<int>("DocId").HasColumnName("doc_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatinId).HasName("patients_pkey");

            entity.ToTable("patients", "poliklinika");

            entity.Property(e => e.PatinId).HasColumnName("patin_id");
            entity.Property(e => e.PatinBirthday).HasColumnName("patin_birthday");
            entity.Property(e => e.PatinFname).HasColumnName("patin_fname");
            entity.Property(e => e.PatinLname).HasColumnName("patin_lname");
            entity.Property(e => e.PatinSname).HasColumnName("patin_sname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles", "poliklinika");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.SchedId).HasName("schedule_pkey");

            entity.ToTable("schedule", "poliklinika");

            entity.Property(e => e.SchedId).HasColumnName("sched_id");
            entity.Property(e => e.DocId).HasColumnName("doc_id");
            entity.Property(e => e.SchedDate).HasColumnName("sched_date");
            entity.Property(e => e.SchedIsClosed).HasColumnName("sched_is_closed");
            entity.Property(e => e.SchedTime).HasColumnName("sched_time");

            entity.HasOne(d => d.Doc).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DocId)
                .HasConstraintName("schedule_doc_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
