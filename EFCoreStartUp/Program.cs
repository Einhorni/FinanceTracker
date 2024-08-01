using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManagement.DbContexts;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<FinanceContext>();


IHost host = builder.Build();
host.Run();
