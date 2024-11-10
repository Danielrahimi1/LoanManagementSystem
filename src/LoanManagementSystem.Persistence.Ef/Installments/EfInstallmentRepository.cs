using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class EfInstallmentRepository(EfDataContext context) : InstallmentRepository
{
    public async Task AddRange(params Installment[] installments) =>
        await context.Set<Installment>().AddRangeAsync(installments);

    public async Task Add(Installment installment) =>
        await context.Set<Installment>().AddAsync(installment);

    public async Task<Installment?> GetFirstUnpaidInstallment(int loanRequestId) =>
        await context.Set<Installment>().Where(i => i.LoanRequestId == loanRequestId)
            .OrderBy(i => i.PaymentDeadLine)
            .FirstOrDefaultAsync(i => i.PaymentDate == null);
    
    public async Task<int> CountDelayedInstallments(int customerId) =>
        await (from c in context.Set<Customer>()
            where c.Id == customerId
            join lr in context.Set<LoanRequest>().Where(lr => lr.DelayInRepayment == true)
                on c.Id equals lr.CustomerId
            where lr.DelayInRepayment == true
            join i in context.Set<Installment>()
                on lr.Id equals i.LoanRequestId
            where i.Fine > 0
            select i).CountAsync();

    public async Task<bool> HasUnpaidInstallments(int loanRequestId) =>
        await context.Set<Installment>()
            .AnyAsync(i => i.LoanRequestId == loanRequestId && i.PaymentDate == null);

    public async Task<Installment?> Find(int id) =>
        await context.Set<Installment>().FirstOrDefaultAsync(i => i.Id == id);

    public void Update(Installment installment) =>
        context.Set<Installment>().Update(installment);
}