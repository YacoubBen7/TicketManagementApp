// using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketService.Infrastructure.Data;
using TicketService.Infrastructure.Repositories;

namespace TicketService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            //Adding DbContext
            services.AddScoped<ITicketDbContext, TicketDbContext>();
            services.AddDbContext<TicketDbContext>(opt => opt.UseSqlServer(connectionString));

            //Adding Repositories
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }
    }
}
