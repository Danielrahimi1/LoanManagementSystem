// using Autofac;
// using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
// using LoanManagementSystem.Contracts.Interfaces;
// using LoanManagementSystem.Persistence.Ef.Loans;
// using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
//
// namespace LoanManagementSystem.RestAPI.Configs.Services;
//
// public class AutofacBusinessModule : Module
// {
//
//     public static void Load(ContainerBuilder container)
//     {
//         var applicationAssembly = typeof(PayInstallmentHandler).Assembly;
//         var serviceAssembly = typeof(CustomerService).Assembly;
//         var repositoryAssembly = typeof(EfLoanRepository).Assembly;
//
//         container.RegisterAssemblyTypes(applicationAssembly, serviceAssembly)
//             .AssignableTo<Service>()
//             .AsImplementedInterfaces()
//             .InstancePerLifetimeScope();
//     
//         container.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
//             .AssignableTo<Repository>()
//             .AsImplementedInterfaces()
//             .InstancePerLifetimeScope();
//     
//         container.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
//             .AssignableTo<IScope>()
//             .AsImplementedInterfaces()
//             .InstancePerLifetimeScope();
//         
//         base.Load(container);
//     }
// }