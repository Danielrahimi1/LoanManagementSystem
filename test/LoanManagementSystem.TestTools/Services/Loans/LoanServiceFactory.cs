using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Loans;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;

namespace LoanManagementSystem.TestTools.Services.Loans;

public static class LoanServiceFactory
{
    public static LoanService CreateService(EfDataContext context)
    {
        var loanRepository = new EfLoanRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new LoanAppService(
            loanRepository,
            unitOfWork);
    }
}