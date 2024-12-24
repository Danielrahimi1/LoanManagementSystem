using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts.Interfaces;

public interface LoanQuery : Repository
{
    Task<GetLoanDto?> GetById(int id);
    Task<GetLoanDto[]> GetAll();
    Task<GetLoanDto[]> GetAllShortPeriod();
    Task<GetLoanDto[]> GetAllLongPeriod();
}