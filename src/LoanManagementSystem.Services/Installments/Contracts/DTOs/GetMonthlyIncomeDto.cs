namespace LoanManagementSystem.Services.Installments.Contracts.DTOs;

public class GetMonthlyIncomeDto
{
    public required decimal Interest { get; set; }
    public required decimal Fine { get; set; }
}