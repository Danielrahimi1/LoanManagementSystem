using LoanManagementSystem.Entities.Customers.Enums;

namespace LoanManagementSystem.Entities.Customers;

public class Statement
{
    public int Id { get; init; }
    public JobType JobType { get; set; } = JobType.UnEmployed;
    public IncomeGroup IncomeGroup { get; set; } = IncomeGroup.LessThanFive;
    public decimal NetWorth { get; set; }
}