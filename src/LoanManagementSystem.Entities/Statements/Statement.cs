using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Statements.Enums;

namespace LoanManagementSystem.Entities.Statements;

public class Statement
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public JobType JobType { get; set; }
    public IncomeGroup IncomeGroup { get; set; }
    public decimal NetWorth { get; set; }
    public Customer Customer { get; init; }
}