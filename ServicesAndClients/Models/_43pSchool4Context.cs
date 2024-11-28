using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServicesAndClients.Models;

public partial class _43pSchool4Context : DbContext
{
    public _43pSchool4Context()
    {
    }

    public _43pSchool4Context(DbContextOptions<_43pSchool4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicePhoto> ServicePhotos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=edu.pg.ngknn.local;Port=5432;Database=43P_school4;Username=43P;Password=444444");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");

            entity.ToTable("clients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.Gendercode)
                .ValueGeneratedOnAdd()
                .HasColumnName("gendercode");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.PhotoPath)
                .HasColumnType("character varying")
                .HasColumnName("photo_path");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");

            entity.HasOne(d => d.GendercodeNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Gendercode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clients_genders_fk");
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_service_pk");

            entity.ToTable("client_service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId)
                .ValueGeneratedOnAdd()
                .HasColumnName("client_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.ServiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("service_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("client_service_clients_fk");

            entity.HasOne(d => d.Service).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("client_service_services_fk");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("genders_pk");

            entity.ToTable("genders");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("services_pk");

            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.DurationInSecond).HasColumnName("duration_in_second");
            entity.Property(e => e.MainImagePath)
                .HasColumnType("character varying")
                .HasColumnName("main_image_path");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<ServicePhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_photo_pk");

            entity.ToTable("service_photo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhotoPath)
                .HasColumnType("character varying")
                .HasColumnName("photo_path");
            entity.Property(e => e.ServiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("service_id");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicePhotos)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("service_photo_services_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
