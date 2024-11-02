using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class EfLoanRepository(EfDataContext context) : LoanRepository
{
    public async Task Add(Loan loan) => await context.Set<Loan>().AddAsync(loan);

    public async Task<Loan?> Find(int id) =>
        await context.Set<Loan>().FirstOrDefaultAsync(loan => loan.Id == id);

    public async Task Update(Loan loan) =>
        await Task.Run(() => context.Set<Loan>().Update(loan));

    public async Task Remove(Loan loan) =>
        await Task.Run(() => context.Set<Loan>().Remove(loan));
}