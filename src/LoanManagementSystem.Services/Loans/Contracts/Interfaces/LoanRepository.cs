using System.Threading.Tasks;
using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Services.Loans.Contracts.Interfaces;

public interface LoanRepository
{
    Task Add(Loan loan);
    Task<Loan?> Find(int id);
    Task Update(Loan loan);
    Task Remove(Loan loan);
}