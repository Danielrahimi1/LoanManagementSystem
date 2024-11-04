namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class UpdateLoanDto
{
    public decimal? Amount { get; set; }
    public int? InstallmentCount { get; set; }
    public int? AnnualInterestRate { get; set; }
}