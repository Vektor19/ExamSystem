using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExamSystem.Core.Interfaces.Security;
using ExamSystem.Infrastructure.Security;

namespace ExamSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}