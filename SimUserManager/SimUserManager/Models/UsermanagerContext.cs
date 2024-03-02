using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SimUserManager.Models;

public partial class UsermanagerContext : DbContext
{
    public UsermanagerContext()
    {
    }

    public UsermanagerContext(DbContextOptions<UsermanagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departments> Departments { get; set; }

    public virtual DbSet<Positions> Positions { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    // あとで絶対消す
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=usermanager;Username=sim;Password=omniversal");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departments>(entity =>
        {
            entity.HasKey(e => e.DepartmentNo).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentNo)
                .ValueGeneratedNever()
                .HasColumnName("department_no");
            entity.Property(e => e.Department)
                .HasMaxLength(30)
                .HasColumnName("department");
            entity.Property(e => e.Section)
                .HasMaxLength(30)
                .HasColumnName("section");
        });

        modelBuilder.Entity<Positions>(entity =>
        {
            entity.HasKey(e => e.PositionNo).HasName("positions_pkey");

            entity.ToTable("positions");

            entity.Property(e => e.PositionNo)
                .ValueGeneratedNever()
                .HasColumnName("position_no");
            entity.Property(e => e.PositionName)
                .HasMaxLength(30)
                .HasColumnName("position_name");
            entity.Property(e => e.Rankvalue).HasColumnName("rankvalue");
        });

        modelBuilder.Entity<Users>(entity =>
        {

            entity.ToTable("users");

            entity.HasKey( e => e.UserId ).HasName("users_pkey");

            entity.Property(e => e.DepartmentNo).HasColumnName("department_no");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.PositionNo).HasColumnName("position_no");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .HasColumnName("user_id");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
