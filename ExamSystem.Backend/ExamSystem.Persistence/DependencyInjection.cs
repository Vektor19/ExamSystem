using ExamSystem.Core.Interfaces.Repositories;
using ExamSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamSystem.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration) 
        {
            var provider = configuration["Database:Provider"];
            if (string.IsNullOrWhiteSpace(provider))
                throw new InvalidOperationException("Database provider is not configured.");
            switch (provider.ToLower())
            {
                case "sqlserver":
                    var sqlConnection = configuration["Database:ConnectionStrings:SqlServer"];
                    services.AddDbContext<ExamSystemDbContext>(options =>
                        options.UseSqlServer(sqlConnection));
                    break;

                case "postgresql":
                    var pgConnection = configuration["Database:ConnectionStrings:PostgreSql"];
                    services.AddDbContext<ExamSystemDbContext>(options =>
                        options.UseNpgsql(pgConnection));
                    break;

                case "sqlite":
                    var sqliteConnection = configuration["Database:ConnectionStrings:Sqlite"];
                    services.AddDbContext<ExamSystemDbContext>(options =>
                        options.UseSqlite(sqliteConnection));
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported database provider: {provider}");
            }

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IViolationRepository, ViolationRepository>();
            services.AddScoped<DbSeeder>();

            return services;
        }

    }
}
