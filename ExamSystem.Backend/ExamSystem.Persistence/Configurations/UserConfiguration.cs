using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            builder.HasMany(u => u.ExamUsers)
                .WithOne(eu => eu.User)
                .HasForeignKey(eu => eu.UserId);

            builder.HasMany(u => u.CreatedExams)
                .WithOne(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedByUserId);

            builder.HasMany(u => u.Answers)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        }
    }
}
