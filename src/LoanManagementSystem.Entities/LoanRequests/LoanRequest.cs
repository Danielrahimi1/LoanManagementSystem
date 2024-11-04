using System;
using System.Collections.Generic;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Entities.LoanRequests;

public class LoanRequest
{
    public int Id { get; init; }
    public required int LoanId { get; init; }
    public required int CustomerId { get; init; }
    public required LoanRequestStatus Status { get; set; }
    public bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
    public required Customer Customer { get; init; }
    public HashSet<Installment> Installments { get; set; } = [];
}