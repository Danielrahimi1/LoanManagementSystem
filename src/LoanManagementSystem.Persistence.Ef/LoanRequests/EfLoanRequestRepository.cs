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

    public async Task<int> CountNonDelayedLoans(int customerId) =>
        await context.Set<LoanRequest>()
            .CountAsync(lr => lr.CustomerId == customerId && lr.DelayInRepayment == false);

    public async Task<bool> HasActiveLoanRequests(int customerId) =>
        await context.Set<LoanRequest>()
            .AnyAsync(lr => lr.CustomerId == customerId &&
                            ((int)lr.Status == 3 || (int)lr.Status == 4));

    public void Update(LoanRequest loanRequest) =>
        context.Set<LoanRequest>().Update(loanRequest);
}