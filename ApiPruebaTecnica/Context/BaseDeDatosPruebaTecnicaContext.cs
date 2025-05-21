using System;
using System.Collections.Generic;
using ApiPruebaTecnica.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ApiPruebaTecnica.Context;

public partial class BaseDeDatosPruebaTecnicaContext : DbContext
{
    public BaseDeDatosPruebaTecnicaContext()
    {
    }

    public BaseDeDatosPruebaTecnicaContext(DbContextOptions<BaseDeDatosPruebaTecnicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CreditProgram> CreditPrograms { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentSubject> StudentSubjects { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CreditProgram>(entity =>
        {
            entity.HasKey(e => e.IdCreditProgram).HasName("PRIMARY");

            entity.ToTable("credit_programs");

            entity.Property(e => e.IdCreditProgram).HasColumnName("id_credit_program");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.IdStudent).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.IdCreditProgram, "id_credit_program");

            entity.Property(e => e.IdStudent).HasColumnName("id_student");
            entity.Property(e => e.IdCreditProgram).HasColumnName("id_credit_program");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumberDocument)
                .HasMaxLength(100)
                .HasColumnName("number_document");

            entity.HasOne(d => d.IdCreditProgramNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.IdCreditProgram)
                .HasConstraintName("students_ibfk_1");
        });

        modelBuilder.Entity<StudentSubject>(entity =>
        {
            entity.HasKey(e => e.IdStudentSubject).HasName("PRIMARY");

            entity.ToTable("student_subject");

            entity.HasIndex(e => e.IdStudent, "id_student");

            entity.HasIndex(e => e.SubjectId, "subject_id");

            entity.Property(e => e.IdStudentSubject).HasColumnName("id_student_subject");
            entity.Property(e => e.IdStudent).HasColumnName("id_student");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.IdStudent)
                .HasConstraintName("student_subject_ibfk_1");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("student_subject_ibfk_2");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.IdSubject).HasName("PRIMARY");

            entity.ToTable("subjects");

            entity.HasIndex(e => e.IdCreditProgram, "id_credit_program");

            entity.Property(e => e.IdSubject).HasColumnName("id_subject");
            entity.Property(e => e.Credits)
                .HasDefaultValueSql("'3'")
                .HasColumnName("credits");
            entity.Property(e => e.IdCreditProgram).HasColumnName("id_credit_program");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.IdCreditProgramNavigation).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.IdCreditProgram)
                .HasConstraintName("subjects_ibfk_1");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.IdTeacher).HasName("PRIMARY");

            entity.ToTable("teachers");

            entity.Property(e => e.IdTeacher).HasColumnName("id_teacher");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TeacherSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("teacher_subject");

            entity.HasIndex(e => e.SubjectId, "subject_id");

            entity.HasIndex(e => e.TeacherId, "teacher_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("teacher_subject_ibfk_2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("teacher_subject_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
