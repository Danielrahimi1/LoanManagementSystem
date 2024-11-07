using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services;
using LoanManagementSystem.Services.LoanRequests;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using Moq;

namespace LoanManagementSystem.TestTools.LoanRequests;

public static class LoanRequestServiceFactory
{
    public static LoanRequestService CreateService(EfDataContext context)
    {
        var loanRequestRepository = new EfLoanRequestRepository(context);
        var customerRepository = new EfCustomerRepository(context);
        var loanRepository = new EfLoanRepository(context);
        var installmentRepository = new EfInstallmentRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        var dateService =
            new Mock<DateService>().Object; //.Setup(s => s.UtcNow).Returns(new DateOnly(2024, 11, 7));

        return new LoanRequestAppService(
            loanRequestRepository,
            customerRepository,
            loanRepository,
            installmentRepository,
            dateService,
            unitOfWork
        );
    }
}