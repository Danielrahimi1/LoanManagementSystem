namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class AddCustomerWithStatementDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public string JobType { get; set; } = "UnEmployed";
    public decimal Income { get; set; }
    public decimal NetWorth { get; set; }
}