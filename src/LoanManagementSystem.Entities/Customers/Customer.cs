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
    public decimal Balance { get; set; } = default;
    public bool IsVerified { get; set; } = default;
    public int CreditScore { get; set; } = default;
    public JobType JobType { get; set; } = JobType.UnEmployed;
    public IncomeGroup IncomeGroup { get; set; } = IncomeGroup.LessThanFive;
    public decimal NetWorth { get; set; }
    public HashSet<LoanRequest> LoanRequests { get; init; } = [];
}