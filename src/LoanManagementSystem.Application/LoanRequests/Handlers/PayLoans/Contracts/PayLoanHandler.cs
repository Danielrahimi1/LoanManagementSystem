using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;
using LoanManagementSystem.Contracts.Interfaces;

namespace LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;

public interface PayLoanHandler : Service
{
    Task Handle(ActivateLoanRequestCommand cmd);
}