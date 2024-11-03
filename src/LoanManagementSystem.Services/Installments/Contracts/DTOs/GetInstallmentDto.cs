using System;

namespace LoanManagementSystem.Services.Installments.Contracts.DTOs;

public class GetInstallmentDto
{
    public required int LoanId { get; init; }
    public required int LoanRequestId { get; init; }
    public required decimal Amount { get; init; }
    public required decimal MonthlyInterestRate { get; init; }
    public required DateOnly PaymentDeadLine { get; init; }
    public DateOnly? PaymentDate { get; init; }
    public decimal Fine { get; set; }
}