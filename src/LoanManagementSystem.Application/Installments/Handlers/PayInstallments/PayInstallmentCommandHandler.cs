using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Application.Installments.Handlers.PayInstallments;

public class PayInstallmentCommandHandler(
    CustomerService customerService,
    LoanRequestService loanRequestService,
    InstallmentService installmentService,
    UnitOfWork unitOfWork) : PayLoanHandler
{
    public async Task Handle(ActivateLoanRequestCommand cmd)
    {
        await unitOfWork.Begin();
        try
        {
            var payment = await installmentService.Pay(cmd.LoanRequestId);
            await customerService.Charge(cmd.CustomerId, new UpdateBalanceDto
            {
                Charge = -payment // discharge :)
            });
            
            await loanRequestService.Close(cmd.LoanRequestId);
            
            await unitOfWork.Commit();
        }
        catch (Exception e)
        {
            await unitOfWork.Rollback();
            throw;
        }
    }
}