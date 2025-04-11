using System;
using System.Collections.Generic;
using System.Linq;
using ExamSystem.Core.Entities;
using ExamSystem.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence
{
    public class ExamSystemDbContext : DbContext
    {
        public ExamSystemDbContext(DbContextOptions<ExamSystemDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<ExamUser> ExamUsers { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ExamConfiguration());
            modelBuilder.ApplyConfiguration(new ExamUserConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionOptionConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
