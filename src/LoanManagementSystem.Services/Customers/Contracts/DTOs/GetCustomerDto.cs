namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class GetCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NationalId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public decimal Balance { get; set; } = default;
    public bool IsVerified { get; set; } = false;
    public int CreditScore { get; set; } = default;
}