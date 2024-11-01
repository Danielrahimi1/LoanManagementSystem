using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Entities.FinancialInformation;

public class FinancialInformation
{
    public int JobType { get; set; } = default;
    public decimal FinancialAssets { get; set; } = default;
    public Customer? Customer { get; set; }
    public int? CustomerId { get; set; }
    public int IncomeGroup { get; set; }
}