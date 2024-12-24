using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Services.Loans.Contracts.Interfaces;

public interface LoanRepository : Repository
{
    Task Add(Loan loan);
    Task<Loan?> Find(int id);
    void Update(Loan loan);
    void Remove(Loan loan);
}