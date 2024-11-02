using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Entities.Statements;

public class Statement
{
    public int JobType { get; set; } = default;
    public int? CustomerId { get; set; }
    public int IncomeGroup { get; set; }
    public decimal NetWorth { get; set; } = default;
    public Customer? Customer { get; set; }
}