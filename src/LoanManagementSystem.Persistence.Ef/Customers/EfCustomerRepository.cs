using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class EfCustomerRepository(EfDataContext context) : CustomerRepository
{
    public async Task<int> GetCreditScoreById(int id) =>
        await context.Set<Customer>().Where(c => c.Id == id)
            .Select(c => c.CreditScore).FirstAsync();

    public async Task Add(Customer customer) =>
        await context.Set<Customer>().AddAsync(customer);

    public async Task<bool> IsDuplicateByNationalId(string nationalId) =>
        await context.Set<Customer>().AnyAsync(c => c.NationalId == nationalId);
    
    public async Task<bool> IsVerified(int id) =>
        await context.Set<Customer>().AnyAsync(c => c.Id == id && c.IsVerified);

    public async Task<Customer?> Find(int id) =>
        await context.Set<Customer>().FirstOrDefaultAsync(c => c.Id == id);

    public async Task Update(Customer customer) =>
        await Task.Run(() => context.Set<Customer>().Update(customer));

    public async Task Remove(Customer customer) =>
        await Task.Run(() => context.Set<Customer>().Remove(customer));
}