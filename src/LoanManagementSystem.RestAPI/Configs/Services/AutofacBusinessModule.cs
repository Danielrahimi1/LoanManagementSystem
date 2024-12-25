using Autofac;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;

namespace LoanManagementSystem.RestAPI.Configs.Services;

public class AutofacBusinessModule(string connectionString) : Module
{
    private const string ConnectionStringKey = "connectionString";    
    
    protected override void Load(ContainerBuilder container)
    {
        var applicationAssembly = typeof(PayInstallmentHandler).Assembly;
        var serviceAssembly = typeof(CustomerService).Assembly;
        var repositoryAssembly = typeof(EfLoanRepository).Assembly;

        container.RegisterAssemblyTypes(applicationAssembly, serviceAssembly)
            .AssignableTo<Service>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    
        container.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
            .AssignableTo<Repository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    
        container.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
            .AssignableTo<IScope>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        container.RegisterType<EfDataContext>()
            .WithParameter(ConnectionStringKey, connectionString)
            .AsSelf()
            .InstancePerLifetimeScope();
        
        base.Load(container);
    }
}