using System.Threading.Tasks;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts.Interfaces;

public interface CustomerRepository : Repository
{
    // Task<int> GetCreditScoreById(int id);
    Task Add(Customer customer);
    Task<bool> IsDuplicateByNationalId(string nationalId);
    Task<bool> IsVerified(int id);
    Task<Customer?> Find(int id);
    void Update(Customer customer);
    void Remove(Customer customer);
}