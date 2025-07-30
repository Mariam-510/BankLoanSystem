using System;
using System.Collections.Generic;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BankLoanSystem.Infrastructure.DbContext;

public partial class LoanDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public LoanDbContext()
    {
    }

    public LoanDbContext(DbContextOptions<LoanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<LoanType> LoanTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db24183.public.databaseasp.net;Database=db24183;User Id=db24183;Password=x-2G9iL=T5_r;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Loans__3214EC07DC84F1D4");

            entity.HasIndex(e => e.AppUserId, "IX_Loans_AppUserId").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.LoanTypeId, "IX_Loans_LoanTypeId").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.Status, "IX_Loans_Status").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.AppUser).WithMany(p => p.Loans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_AppUsers");

            entity.HasOne(d => d.LoanType).WithMany(p => p.Loans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_LoanTypes");
        });

        modelBuilder.Entity<LoanType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanType__3214EC071CD7CD2C");

            entity.HasIndex(e => e.Name, "IX_LoanTypes_Name").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
        });

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>()
       .ToTable("AspNetUsers");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
