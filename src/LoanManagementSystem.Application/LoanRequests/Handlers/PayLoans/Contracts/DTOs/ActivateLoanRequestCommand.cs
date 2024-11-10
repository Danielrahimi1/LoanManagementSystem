namespace LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;

public class ActivateLoanRequestCommand
{
    public required int CustomerId { get; init; }
    public required int LoanRequestId { get; init; }
}