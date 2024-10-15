using TicketService.API;
using TicketService.Core;
using TicketService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddInfrastructureDependencies(builder.Configuration)
    .AddCoreDependencies()
    .AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();
