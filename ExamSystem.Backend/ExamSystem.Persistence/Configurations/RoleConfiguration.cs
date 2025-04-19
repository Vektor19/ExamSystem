using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamSystem.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.RoleId);

            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);
            builder.HasData(
                new Role
                {
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = SystemRoles.Admin,
                    IsSystem = true
                },
                new Role
                {
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = SystemRoles.Student,
                    IsSystem = true
                },
                new Role
                {
                    RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = SystemRoles.Examinator,
                    IsSystem = true
                }
            );
        }
    }
}
