using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;
using LoanManagementSystem.Contracts.Interfaces;

namespace LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;

public interface PayInstallmentHandler : Service
{
    Task Handle(PayInstallmentCommand cmd);
}