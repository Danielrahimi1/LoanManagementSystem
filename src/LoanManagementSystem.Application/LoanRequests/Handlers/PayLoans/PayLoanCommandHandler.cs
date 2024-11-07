using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans;

public class PayLoanCommandHandler(
    LoanRequestService loanRequestService,
    CustomerService customerService,
    UnitOfWork unitOfWork) : PayLoanHandler
{
    public async Task Handle(ActivateLoanRequestCommand cmd)
    {
        await unitOfWork.Begin();
        try
        {
            var loanAmount = await loanRequestService.Activate(cmd.LoanRequestId);
            await customerService.Charge(cmd.CustomerId, new UpdateBalanceDto
            {
                Charge = loanAmount
            });
            await unitOfWork.Commit();
        }
        catch (Exception e)
        {
            await unitOfWork.Rollback();
            throw;
        }
    }
}