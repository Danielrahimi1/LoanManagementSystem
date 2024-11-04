using System.Threading.Tasks;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts.Interfaces;

public interface LoanService
{
    Task Create(AddLoanDto dto);
    Task Update(int id, UpdateLoanDto dto);
    Task Delete(int id);
}