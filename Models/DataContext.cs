using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnswerDAO> Answers { get; set; }

    public virtual DbSet<AppUserDAO> AppUsers { get; set; }

    public virtual DbSet<AppUserBoardMappingDAO> AppUserBoardMappings { get; set; }

    public virtual DbSet<AppUserClassroomMappingDAO> AppUserClassroomMappings { get; set; }

    public virtual DbSet<AppUserRoleMappingDAO> AppUserRoleMappings { get; set; }

    public virtual DbSet<AppUserJobMappingDAO> AppUserJobMappings { get; set; }

    public virtual DbSet<BoardDAO> Boards { get; set; }

    public virtual DbSet<CardDAO> Cards { get; set; }

    public virtual DbSet<ClassEventDAO> ClassEvents { get; set; }

    public virtual DbSet<ClassroomDAO> Classrooms { get; set; }

    public virtual DbSet<CommentDAO> Comments { get; set; }

    public virtual DbSet<JobDAO> Jobs { get; set; }

    public virtual DbSet<PermissionDAO> Permissions { get; set; }

    public virtual DbSet<PermissionRoleMappingDAO> PermissionRoleMappings { get; set; }

    public virtual DbSet<QuestionDAO> Questions { get; set; }

    public virtual DbSet<RoleDAO> Roles { get; set; }

    public virtual DbSet<TodoDAO> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:dbconn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerDAO>(entity =>
        {
            entity.ToTable("Answer");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(500);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<AppUserDAO>(entity =>
        {
            entity.ToTable("AppUser");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<AppUserBoardMappingDAO>(entity =>
        {
            entity.ToTable("AppUserBoardMapping");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserBoardMappings)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserBoardMapping_AppUser");

            entity.HasOne(d => d.Board).WithMany(p => p.AppUserBoardMappings)
                .HasForeignKey(d => d.BoardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserBoardMapping_Board");
        });

        modelBuilder.Entity<AppUserClassroomMappingDAO>(entity =>
        {
            entity.ToTable("AppUserClassroomMapping");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserClassroomMappings)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserClassroomMapping_AppUser");

            entity.HasOne(d => d.Classroom).WithMany(p => p.AppUserClassroomMappings)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserClassroomMapping_Classroom");
        });

        modelBuilder.Entity<AppUserRoleMappingDAO>(entity =>
        {
            entity.ToTable("AppUserRoleMapping");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserRoleMappings)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserRoleMapping_AppUser");

            entity.HasOne(d => d.Role).WithMany(p => p.AppUserRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserRoleMapping_Role");
        });

        modelBuilder.Entity<AppUserJobMappingDAO>(entity =>
        {
            entity.ToTable("AppUserJobMapping");

            entity.HasOne(d => d.AppUser).WithMany(p => p.AppUserJobMappings)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserJobMapping_AppUser");

            entity.HasOne(d => d.Job).WithMany(p => p.AppUserJobMappings)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserJobMapping_Job");
        });

        modelBuilder.Entity<BoardDAO>(entity =>
        {
            entity.ToTable("Board");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Boards)
                .HasForeignKey(d => d.ClassroomId)
                .HasConstraintName("FK_Board_Classroom");
        });

        modelBuilder.Entity<CardDAO>(entity =>
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

        modelBuilder.Entity<ClassEventDAO>(entity =>
        {
            entity.ToTable("ClassEvent");

            entity.Property(e => e.Code).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EndAt).HasColumnType("datetime");
            entity.Property(e => e.StartAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Classroom).WithMany(p => p.ClassEvents)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassEvent_Classroom");
        });

        modelBuilder.Entity<ClassroomDAO>(entity =>
        {
            entity.ToTable("Classroom");

            entity.Property(e => e.Code).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.HomeImg).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CommentDAO>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(d => d.ClassEvent).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ClassEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_ClassEvent");
        });

        modelBuilder.Entity<JobDAO>(entity =>
        {
            entity.ToTable("Job");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EndAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Card).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Job_Card");

            entity.HasOne(d => d.Creator).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Job_AppUser");
        });

        modelBuilder.Entity<PermissionDAO>(entity =>
        {
            entity.ToTable("Permission");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MenuName).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Path).HasMaxLength(500);
        });

        modelBuilder.Entity<PermissionRoleMappingDAO>(entity =>
        {
            entity.ToTable("PermissionRoleMapping");

            entity.HasOne(d => d.Permission).WithMany(p => p.PermissionRoleMappings)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissionRoleMapping_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.PermissionRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissionRoleMapping_Role");
        });

        modelBuilder.Entity<QuestionDAO>(entity =>
        {
            entity.ToTable("Question");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Instruction).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.CorrectAnswer).HasMaxLength(1000);
            entity.Property(e => e.StudentAnswer).HasMaxLength(1000);

            entity.HasOne(d => d.ClassEvent).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ClassEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_ClassEvent");
        });

        modelBuilder.Entity<RoleDAO>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<TodoDAO>(entity =>
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
