namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetLoanDto
{
    public required decimal Amount { get; set; }
    public required int InstallmentCount { get; set; }
    public required int AnnualInterestRate { get; set; }
}