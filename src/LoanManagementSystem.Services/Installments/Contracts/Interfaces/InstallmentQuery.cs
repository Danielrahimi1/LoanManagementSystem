using System.Threading.Tasks;
using LoanManagementSystem.Services.Installments.Contracts.DTOs;

namespace LoanManagementSystem.Services.Installments.Contracts.Interfaces;

public interface InstallmentQuery
{
    Task<GetInstallmentDto> GetById(int id);
    Task<GetInstallmentDto> GetAll();
    Task<GetInstallmentDto> GetAllByCustomer(int customerId);
    Task<GetInstallmentDto> GetAllByLoan(int loanId);
    Task<GetInstallmentDto> GetAllByLoanRequest(int loanRequestId);
    Task<GetInstallmentDto> GetAllDelayed();
    Task<GetInstallmentDto> GetAllClosed();
}