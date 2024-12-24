using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestService : Service
{
    Task Open(int customerId, AddLoanRequestDto dto);
    Task Accept(int id);
    Task Reject(int id);
    Task<decimal> Activate(int id);
    Task Close(int id);
}