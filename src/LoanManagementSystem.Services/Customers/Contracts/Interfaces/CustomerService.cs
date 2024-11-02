using System.Threading.Tasks;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts.Interfaces;

public interface CustomerService
{
    Task Register(AddCustomerDto dto);
    Task Verify(int id);
    Task VerifyManually(int id);
    Task Update(UpdateCustomerDto dto);
    Task Delete(int id);
}