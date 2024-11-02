using System.Threading.Tasks;
using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts.Interfaces;

public interface CustomerRepository
{
    Task<int> GetCreditScoreById(int id);
    Task Add(Customer customer);
    Task<bool> IsVerified(int id);
    Task<Customer?> Find(int id);
    Task<int> Update(Customer customer);
    Task<int> Remove(Customer customer);
}