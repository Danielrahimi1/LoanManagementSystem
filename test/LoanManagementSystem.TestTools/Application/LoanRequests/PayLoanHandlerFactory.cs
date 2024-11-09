using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.LoanRequests;

namespace LoanManagementSystem.TestTools.Application.LoanRequests;

public static class PayLoanHandlerFactory
{
    public static PayLoanHandler CreateHandler(
        EfDataContext context,
        LoanRequestService? loanRequestService = null,
        CustomerService? customerService = null,
        UnitOfWork? unitOfWork = null)

    {
        customerService ??= CustomerServiceFactory.CreateService(context);
        loanRequestService ??= LoanRequestServiceFactory.CreateService(context);
        unitOfWork ??= new EfUnitOfWork(context);
        return new PayLoanCommandHandler(
            loanRequestService,
            customerService,
            unitOfWork);
    }
}