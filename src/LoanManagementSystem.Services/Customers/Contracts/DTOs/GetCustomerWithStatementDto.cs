namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class GetCustomerWithStatementDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public decimal Balance { get; set; }
    public bool IsVerified { get; set; }
    public int CreditScore { get; set; }
    public string JobType { get; set; }
    public string IncomeGroup { get; set; }
    public decimal NetWorth { get; set; }
}