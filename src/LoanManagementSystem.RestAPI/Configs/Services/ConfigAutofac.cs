using Autofac;
using Autofac.Extensions.DependencyInjection;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;

namespace LoanManagementSystem.RestAPI.Configs.Services;

public static class ConfigAutofac
{
    public static void AddAutofac(
        this ConfigureHostBuilder builder,
        string connectionString)
    {
        builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.ConfigureContainer<ContainerBuilder>(b =>
                    b.RegisterModule(new AutofacBusinessModule(connectionString))
                );
        var applicationAssembly = typeof(PayInstallmentHandler).Assembly;
        var serviceAssembly = typeof(CustomerService).Assembly;
        var repositoryAssembly = typeof(EfLoanRepository).Assembly;

        builder.ConfigureContainer<ContainerBuilder>(b =>
        {
            b.RegisterAssemblyTypes(applicationAssembly, serviceAssembly)
                .AssignableTo<Service>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
    
            b.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
    
            b.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
                .AssignableTo<IScope>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        });
        
    }
}