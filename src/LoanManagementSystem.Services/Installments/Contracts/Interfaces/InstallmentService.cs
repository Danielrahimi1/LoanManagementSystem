using System.Threading.Tasks;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentService
{
    Task<decimal> Pay(int loanRequestId);
}