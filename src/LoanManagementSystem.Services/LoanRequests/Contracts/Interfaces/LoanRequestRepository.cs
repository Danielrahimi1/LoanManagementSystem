using System.Threading.Tasks;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestRepository
{
    Task Add(LoanRequest loanRequest);
    Task<LoanRequest?> Find(int id);
    Task<int> CountNonDelayedLoans(int customerId);
    Task<bool> HasActiveLoanRequests(int customerId);
    void Update(LoanRequest loanRequest);
}