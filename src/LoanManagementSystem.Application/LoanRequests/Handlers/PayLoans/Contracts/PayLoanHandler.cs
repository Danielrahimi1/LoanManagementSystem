using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;

namespace LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;

public interface PayLoanHandler
{
    Task Handle(ActivateLoanRequestCommand cmd);
}