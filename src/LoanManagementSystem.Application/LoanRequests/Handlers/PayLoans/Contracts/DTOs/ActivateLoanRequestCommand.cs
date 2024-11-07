namespace LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts.DTOs;

public class ActivateLoanRequestCommand
{
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public int LoanRequestId { get; set; }
}