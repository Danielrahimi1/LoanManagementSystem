using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts.Interfaces;

public interface LoanService : Service
{
    Task Create(AddLoanDto dto);
    Task Update(int id, UpdateLoanDto dto);
    Task Delete(int id);
}