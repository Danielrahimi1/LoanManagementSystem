using System;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Entities.Installments;

public class Installment
{
    public int Id { get; init; }
    public required LoanRequest LoanRequest { get; init; }
    public required int LoanRequestId { get; init; }
    public required decimal Amount { get; init; }
    public required decimal MonthlyInterest { get; init; }
    public required DateOnly PaymentDeadLine { get; init; }
    public DateOnly? PaymentDate { get; set; }
    public decimal Fine { get; set; } = default;
}