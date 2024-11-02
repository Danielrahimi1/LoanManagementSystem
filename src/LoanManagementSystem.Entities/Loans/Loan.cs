namespace LoanManagementSystem.Entities.Loans;

public class Loan
{
    public int Id { get; init; }
    public required decimal Amount { get; set; }
    public required int InstallmentCount { get; set; }
    public required int AnnualInterestRate { get; set; }
}