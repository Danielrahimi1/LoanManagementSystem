using System;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

public class GetLoanRequestDto
{
    public required int LoanId { get; init; }
    public required int CustomerId { get; init; }
    public required LoanRequestStatus Status { get; set; }
    public bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
}