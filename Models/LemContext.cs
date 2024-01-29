using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Models;

public partial class LemContext : DbContext
{
    public LemContext()
    {
    }

    public LemContext(DbContextOptions<LemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:dbconn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.ToTable("Board");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Card");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Board).WithMany(p => p.Cards)
                .HasForeignKey(d => d.BoardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Card_Board");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("Job");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PlanTime).HasMaxLength(50);

            entity.HasOne(d => d.Card).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Job_Card");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.ToTable("Todo");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Job).WithMany(p => p.Todos)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_Todo_Job");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
