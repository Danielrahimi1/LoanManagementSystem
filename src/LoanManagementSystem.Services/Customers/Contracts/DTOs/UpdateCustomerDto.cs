using LoanManagementSystem.Entities.Customers.Enums;

namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class UpdateCustomerDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? JobType { get; set; }
    public decimal? Income { get; set; }
    public decimal? NetWorth { get; set; }
}