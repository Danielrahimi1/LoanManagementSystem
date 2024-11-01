using System;
using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.Entities.LoanRequests;

public class LoanRequest
{
    public int Id { get; set; }
    public int Status { get; set; }
    public required int LoanId { get; set; }
    public required int CustomerId { get; set; }
    public bool DelayInRepayment { get; set; } = false;
    public DateOnly? ConfirmationDate { get; set; }
    public Installment[] Installments { get; set; } = [];
}