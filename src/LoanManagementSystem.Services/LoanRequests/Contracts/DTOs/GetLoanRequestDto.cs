using System;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public class GetLoanRequestDto
{
    public required int LoanId { get; init; }
    public required int CustomerId { get; init; }
    public required int Status { get; set; }
    public bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
}