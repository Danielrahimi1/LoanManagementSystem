using System;
using System.Collections.Generic;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests.Enums;

namespace LoanManagementSystem.Entities.LoanRequests;

public class LoanRequest
{
    public int Id { get; init; }
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public required LoanRequestStatus Status { get; set; }
    public bool DelayInRepayment { get; set; }
    public DateOnly? ConfirmationDate { get; set; }
    public Customer Customer { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];
}