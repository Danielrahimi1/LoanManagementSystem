namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class AddLoanDto
{
    public required decimal Amount { get; set; }
    public required int InstallmentCount { get; set; }
    public required int AnnualInterestRate { get; set; }
}