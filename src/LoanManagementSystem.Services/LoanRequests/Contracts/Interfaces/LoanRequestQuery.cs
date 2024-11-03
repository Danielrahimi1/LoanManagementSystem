using System.Threading.Tasks;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestQuery
{
    Task<GetLoanRequestDto> GetById(int id);
    Task<GetLoanRequestDto[]> GetAllByCustomer(int customerId);
    Task<GetLoanRequestDto[]> GetAll();
    Task<GetLoanRequestDto[]> GetAllActiveLoans();
    Task<GetLoanRequestDto[]> GetAllDelayedLoans();
    Task<GetLoanRequestDto[]> GetAllDoneLoans();
    Task<GetLoanRequestDto[]> GetAllWithCustomer();
    Task<GetLoanRequestWithCustomerDto[]> GetAllActiveLoansWithCustomer();
    Task<GetLoanRequestDto[]> GetAllDelayedLoansWithCustomer();
    Task<GetLoanRequestDto[]> GetAllDoneLoansWithCustomer();
}