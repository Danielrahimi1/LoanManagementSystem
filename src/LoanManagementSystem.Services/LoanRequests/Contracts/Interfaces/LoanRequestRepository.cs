using System.Threading.Tasks;
using LoanManagementSystem.Entities.LoanRequests;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestRepository
{
    Task Add(LoanRequest loanRequest);
    Task<LoanRequest?> Find(int id);
    Task Update(LoanRequest loanRequest);
}