using System;
using System.Collections.Generic;
using System.Linq;
using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Persistence
{
    public class ExamSystemDbContext: DbContext
    {
        public ExamSystemDbContext(DbContextOptions<ExamSystemDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamUser> ExamUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
