using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class ExamUserConfiguration : IEntityTypeConfiguration<ExamUser>
    {
        public void Configure(EntityTypeBuilder<ExamUser> builder)
        {
            builder.HasKey(eu => eu.ExamUserId);

            builder.HasOne(eu => eu.User)
                .WithMany(u => u.ExamUsers)
                .HasForeignKey(eu => eu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(eu => eu.Exam)
                .WithMany(e => e.ExamUsers)
                .HasForeignKey(eu => eu.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(eu => eu.Violations)
                .WithOne(v => v.ExamUser)
                .HasForeignKey(v => v.ExamUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(eu => new { eu.UserId, eu.ExamId })
                .IsUnique();
        }
    }
}
