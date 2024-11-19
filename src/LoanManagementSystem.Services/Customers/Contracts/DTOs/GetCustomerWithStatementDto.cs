namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class GetCustomerWithStatementDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required decimal Balance { get; set; }
    public required bool IsVerified { get; set; }
    public required string JobType { get; set; }
    public required string IncomeGroup { get; set; }
    public required decimal NetWorth { get; set; }
}