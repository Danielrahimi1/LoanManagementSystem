using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class EfLoanRepository(EfDataContext context) : LoanRepository
{
    public async Task Add(Loan loan) =>
        await context.Set<Loan>().AddAsync(loan);

    public async Task<Loan?> Find(int id) =>
        await context.Set<Loan>().FirstOrDefaultAsync(loan => loan.Id == id);

    public void Update(Loan loan) =>
        context.Set<Loan>().Update(loan);

    public void Remove(Loan loan) =>
        context.Set<Loan>().Remove(loan);
}