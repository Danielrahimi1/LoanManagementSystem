using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;

namespace LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;

public interface PayInstallmentHandler
{
    Task Handle(PayInstallmentCommand cmd);
}