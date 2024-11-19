using System;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

public class GetLoanRequestDto
{
    public required int LoanId { get; init; }
    public required int CustomerId { get; init; }
    public required int Rate { get; init; }
    public required string Status { get; init; }
    public required bool DelayInRepayment { get; init; }
    public required DateOnly? ConfirmationDate { get; init; }
}