using System.Collections.Generic;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Entities.Customers;

public class Customer
{
    public int Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public decimal Balance { get; set; }
    public bool IsVerified { get; set; }
    // public int CreditScore { get; set; }
    public JobType JobType { get; set; }
    public IncomeGroup IncomeGroup { get; set; }
    public decimal NetWorth { get; set; }
    public HashSet<LoanRequest> LoanRequests { get; init; } = [];
}