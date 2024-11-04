using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class EfInstallmentRepository(EfDataContext context) : InstallmentRepository
{
    public async Task Add(Installment installment) => await context.Set<Installment>().AddAsync(installment);

    public async Task<Installment?> Find(int id) =>
        await context.Set<Installment>().FirstOrDefaultAsync(i => i.Id == id);

    public void Update(Installment installment) => context.Set<Installment>().Update(installment);
}