using System.Threading.Tasks;
using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentRepository
{
    Task Add(Installment installment);
    Task<Installment?> Find(int id);
    Task Update(Installment installment);
}