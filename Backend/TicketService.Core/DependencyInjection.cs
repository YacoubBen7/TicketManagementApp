using Microsoft.Extensions.DependencyInjection;
using TicketService.Core.ServicesContracts;

namespace TicketService.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDependencies(
            this IServiceCollection services
        )
        {
            // Register services

            services.AddScoped<ITicketService, Services.TicketService>();

            return services;
        }
    }
}
