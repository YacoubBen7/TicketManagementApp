using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using TicketService.API.Validators;
using TicketService.Domain.Enums;
using TicketService.Domain.Models;

namespace TicketService.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDependencies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Adding Controllers
            services.AddControllers();


            // Getting connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            // Adding CORS
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    }
                );
            });
            // Adding Custom Exception Middleware

            services.AddExceptionHandler<TicketService.API.ExceptionHandler.ExceptionHandler>();

            // Adding Health Checks
            services.AddHealthChecks().AddSqlServer(connectionString);


            // Adding Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
            });

 

            //Adding Swagger
            services.AddEndpointsApiExplorer(); 

            services.AddSwaggerGen(static opts =>
            {
                opts.ParameterFilter<SortOrderValidator>();
                opts.ParameterFilter<SortColumnValidator<Ticket>>();
                opts.ParameterFilter<StatusValidator<Status>>();
            });

            return services;
        }

        public static WebApplication UseApiMiddlewares(this WebApplication app)
        {
            //0. UseSwagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(options => { });
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                }
            );
            app.MapControllers();
            return app;
        }
    }
}
