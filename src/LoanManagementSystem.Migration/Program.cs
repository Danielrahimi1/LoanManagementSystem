using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Migration.Migrations;

namespace LoanManagementSystem.Migration;

public static class Program
{
    public static void Main(string[] args)
    {
        using var serviceProvider = CreateServices();
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    private static ServiceProvider CreateServices() =>
        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(
                    "Data Source=localhost;Initial Catalog=LMS;Integrated Security=True;Trust Server Certificate=True")
                .ScanIn(typeof(AddCustomersTable).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}