using System.Threading.Tasks;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;

public interface LoanRequestQuery
{
    Task<GetLoanRequestDto?> GetById(int id);
    Task<GetLoanRequestDto[]> GetAllByCustomer(int customerId);
    Task<GetLoanRequestDto[]> GetAll();
    Task<GetLoanRequestDto[]> GetAllAcceptLoans();
    Task<GetLoanRequestDto[]> GetAllActiveLoans();
    Task<GetLoanRequestDto[]> GetAllDelayedLoans();
    Task<GetLoanRequestDto[]> GetAllClosedLoans();
    Task<GetLoanRequestWithCustomerDto[]> GetAllWithCustomer();
    Task<GetLoanRequestWithCustomerDto[]> GetAllAcceptLoansWithCustomer();
    Task<GetLoanRequestWithCustomerDto[]> GetAllActiveLoansWithCustomer();
    Task<GetLoanRequestWithCustomerDto[]> GetAllDelayedLoansWithCustomer();
    Task<GetLoanRequestWithCustomerDto[]> GetAllClosedLoansWithCustomer();
    Task<GetRemainingLoanDto[]> GetAllRemainingLoans();
}