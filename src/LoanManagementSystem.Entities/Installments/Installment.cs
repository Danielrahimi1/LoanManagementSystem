using System;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Entities.Installments;

public class Installment
{
    public int Id { get; set; }
    public LoanRequest? LoanRequest { get; set; }
    public required int? LoanRequestId { get; set; }
    public required int LoanId { get; set; }
    public DateOnly? PaymentDate { get; set; } = null;
    public required DateOnly PaymentDeadLine { get; set; }
    public required decimal Amount { get; set; }
    public decimal MonthlyInterestRate { get; set; }
    public decimal Fine { get; set; } = default;
}