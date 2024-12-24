using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentService : Service
{
    Task<decimal> Pay(int loanRequestId);
}