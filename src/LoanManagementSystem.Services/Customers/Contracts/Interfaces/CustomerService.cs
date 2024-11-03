using System.Threading.Tasks;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts.Interfaces;

public interface CustomerService
{
    Task Register(AddCustomerDto dto);
    Task RegisterWithStatement(AddCustomerWithStatementDto dto);
    Task Verify(int id);
    Task VerifyManually(int id);
    Task Charge(int id, UpdateBalanceDto dto);
    Task Update(int id, UpdateCustomerDto dto);
    Task Delete(int id);
}