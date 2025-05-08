using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.ExamId);

            builder.HasOne(e => e.UserCreatedBy)
                .WithMany(u => u.CreatedExams)
                .HasForeignKey(e => e.CreatedByUserId);

            builder.HasMany(e=>e.ExamUsers)
                .WithOne(eu => eu.Exam)
                .HasForeignKey(e => e.ExamId);
            
            builder.HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId);

            builder.HasMany(e => e.Answers)
                .WithOne(a => a.Exam)
                .HasForeignKey(e => e.ExamId);

            builder.Ignore(e => e.Status);
        }
    }
}
