using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class ViolationConfiguration : IEntityTypeConfiguration<Violation>
    {
        public void Configure(EntityTypeBuilder<Violation> builder)
        {
            builder.HasKey(eu => eu.ViolationId);

            builder.HasOne(v => v.ExamUser)
                .WithMany(eu => eu.Violations)
                .HasForeignKey(v => v.ExamUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
