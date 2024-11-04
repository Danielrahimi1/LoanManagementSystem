using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Statements.Enums;

namespace LoanManagementSystem.Entities.Statements;

public class Statement
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public JobType JobType { get; set; } = JobType.UnEmployed;
    public IncomeGroup IncomeGroup { get; set; } = IncomeGroup.LessThanFive;
    public decimal NetWorth { get; set; }
    public Customer Customer { get; init; }
}