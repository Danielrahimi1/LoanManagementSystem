namespace LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;

public class PayInstallmentCommand
{
    public required int CustomerId { get; init; }
    public required int LoanRequestId { get; init; }
}