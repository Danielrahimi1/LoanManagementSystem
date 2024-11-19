using LoanManagementSystem.Services.Installments.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

public class GetRemainingLoanDto
{
    public required string Status { get; set; }
    public required bool IsDelayed { get; set; }
    public required decimal TotalPaid { get; set; }
    public GetInstallmentDto[] Installments = [];
}