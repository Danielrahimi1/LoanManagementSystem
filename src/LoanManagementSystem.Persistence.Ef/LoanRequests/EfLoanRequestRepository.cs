using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.LoanRequests;

public class EfLoanRequestRepository(EfDataContext context) : LoanRequestRepository
{
    public async Task Add(LoanRequest loanRequest) =>
        await context.Set<LoanRequest>().AddAsync(loanRequest);

    public async Task<LoanRequest?> Find(int id) =>
        await context.Set<LoanRequest>().FirstOrDefaultAsync(lr => lr.Id == id);

    public async Task Update(LoanRequest loanRequest) =>
        await Task.Run(() => context.Set<LoanRequest>().Update(loanRequest));
}