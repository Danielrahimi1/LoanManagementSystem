using System.Threading.Tasks;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestService
{
    Task Open(AddLoanRequestDto dto);
    Task Accept(int id);
    Task Reject(int id);
    Task Activate(int id);
    Task Pay(int id);
}