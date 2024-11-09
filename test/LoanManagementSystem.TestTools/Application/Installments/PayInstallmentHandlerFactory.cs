using LoanManagementSystem.Application.Installments.Handlers.PayInstallments;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;

namespace LoanManagementSystem.TestTools.Application.Installments;

public static class PayInstallmentHandlerFactory
{
    public static PayInstallmentHandler CreateHandler(EfDataContext context,
        CustomerService? customerService = null,
        LoanRequestService? loanRequestService = null,
        InstallmentService? installmentService = null,
        UnitOfWork? unitOfWork = null)
    {
        customerService ??= CustomerServiceFactory.CreateService(context);
        loanRequestService ??= LoanRequestServiceFactory.CreateService(context);
        installmentService ??= InstallmentServiceFactory.CreateService(context);
        unitOfWork ??= new EfUnitOfWork(context);
        return new PayInstallmentCommandHandler(
            customerService,
            loanRequestService,
            installmentService,
            unitOfWork);
    }
}