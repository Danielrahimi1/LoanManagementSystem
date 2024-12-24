using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts.Interfaces;

public interface CustomerQuery : Repository
{
    Task<GetCustomerDto?> GetById(int id);
    Task<GetCustomerWithStatementDto?> GetByIdWithStatement(int id);
    Task<GetCustomerDto[]> GetAll();
    Task<GetCustomerWithStatementDto[]> GetAllWithStatement();
    Task<GetCustomerDto[]> GetRiskyCustomers();
    Task<GetCustomerWithStatementDto[]> GetRiskyCustomersWithStatement();
}