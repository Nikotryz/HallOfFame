using HallOfFame.Data;
using HallOfFame.Data.Helpers;
using HallOfFame.Data.Repositories;
using HallOfFame.Data.Services;
using HallOfFame.Data.Validators;
using HallOfFame.Exceptions;
using HallOfFame.Logging;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(options =>
                options.AddProfile(typeof(MappingProfile))
            );

            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            builder.Services.AddProblemDetails();

            var configuration = builder.Configuration;
            var connectionString = configuration.GetConnectionString("PostgreSQL");

            builder.Services.AddDbContext<HallOfFameDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IPersonValidator, PersonValidator>();

            var logsPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!File.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }

            builder.Logging.ClearProviders();
            builder.Logging.AddProvider(new FileLoggerProvider(logsPath));
            builder.Logging.AddConsole();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<HallOfFameDbContext>();
                db.Database.Migrate();
            }

            app.UseExceptionHandler();

            app.Run();
        }
    }
}
