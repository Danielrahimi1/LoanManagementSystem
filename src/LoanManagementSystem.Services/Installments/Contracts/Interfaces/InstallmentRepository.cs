using System.Threading.Tasks;
using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentRepository
{
    Task AddRange(params Installment[] installments);
    Task Add(Installment installment);
    Task<Installment?> GetFirstUnpaidInstallment(int loanRequestId);
    Task<int> CountDelayedInstallments(int customerId);
    Task<bool> HasUnpaidInstallments(int loanRequestId);
    Task<Installment?> Find(int id);
    void Update(Installment installment);
}