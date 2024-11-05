using System;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Entities.Installments;

public class Installment
{
    public int Id { get; init; }
    public int LoanRequestId { get; set; }
    public LoanRequest LoanRequest { get; set; }
    public required decimal Amount { get; set; }
    public required decimal MonthlyInterest { get; set; }
    public required DateOnly PaymentDeadLine { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public decimal Fine { get; set; }
}