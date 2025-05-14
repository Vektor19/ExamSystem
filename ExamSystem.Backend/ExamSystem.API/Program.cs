using ExamSystem.Application;
using ExamSystem.Infrastructure;
using ExamSystem.Persistence;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<ExamExpirationChecker>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Batteries.Init();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
}

//app.UseHttpsRedirection();
app.UseCors("AllowAllPolicy");
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
