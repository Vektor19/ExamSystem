using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.HasKey(qo => qo.QuestionOptionId);

            builder.HasOne(qo => qo.Question)
                .WithMany(q => q.QuestionOptions)
                .HasForeignKey(qo => qo.QuestionId);

            builder.HasMany(qo => qo.Answers)
                .WithOne(a => a.QuestionOption)
                .HasForeignKey(a => a.QuestionOptionId);
        }
    }
}
