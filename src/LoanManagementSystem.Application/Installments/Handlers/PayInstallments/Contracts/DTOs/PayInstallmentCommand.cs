namespace LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;

public class PayInstallmentCommand
{
    public int CustomerId { get; set; }
    public int LoanRequestId { get; set; }
}