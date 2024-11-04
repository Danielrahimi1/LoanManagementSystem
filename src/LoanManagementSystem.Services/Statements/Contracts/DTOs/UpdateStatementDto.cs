namespace LoanManagementSystem.Services.Statements.Contracts.DTOs;

public class UpdateStatementDto
{
    public int CustomerId { get; init; }
    public string JobType { get; set; } = "UnEmployed";
    public decimal Income { get; set; } = default;
    public decimal NetWorth { get; set; }
}