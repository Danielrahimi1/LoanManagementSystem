using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Entities.Statements;

public class Statement
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public int JobType { get; set; }
    public int IncomeGroup { get; set; }
    public decimal NetWorth { get; set; }
    public required Customer Customer { get; init; }
}