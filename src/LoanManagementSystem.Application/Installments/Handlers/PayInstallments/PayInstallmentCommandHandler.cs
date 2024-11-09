using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;
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
    UnitOfWork unitOfWork) : PayInstallmentHandler
{
    public async Task Handle(PayInstallmentCommand cmd)
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
        catch (Exception)
        {
            await unitOfWork.Rollback();
            throw;
        }
    }
}