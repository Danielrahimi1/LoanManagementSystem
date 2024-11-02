using LoanManagementSystem.Entities.Statements;

namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class UpdateCustomerDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public decimal Balance { get; set; }
    public bool IsVerified { get; set; }
    public int CreditScore { get; set; }
    public Statement Statement { get; set; } = new();
}