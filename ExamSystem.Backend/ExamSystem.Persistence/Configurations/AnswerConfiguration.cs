using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(a => a.AnswerId);

            builder.HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            builder.HasOne(a => a.QuestionOption)
                .WithMany(qo => qo.Answers)
                .HasForeignKey(a => a.QuestionOptionId);

            builder.HasOne(a => a.Exam)
                .WithMany(e => e.Answers)
                .HasForeignKey(a => a.ExamId);
        }
    }
}
