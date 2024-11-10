using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class EfLoanQuery(EfDataContext context) : LoanQuery
{
    public async Task<GetLoanDto?> GetById(int id) =>
        await (from loan in context.Set<Loan>()
            where loan.Id == id
            select new GetLoanDto
            {
                Amount = loan.Amount,
                InstallmentCount = loan.InstallmentCount,
                AnnualInterestRate = loan.AnnualInterestRate
            }).FirstOrDefaultAsync();

    public async Task<GetLoanDto[]> GetAll() =>
        await (from loan in context.Set<Loan>()
            select new GetLoanDto
            {
                Amount = loan.Amount,
                InstallmentCount = loan.InstallmentCount,
                AnnualInterestRate = loan.AnnualInterestRate
            }).ToArrayAsync();

    public async Task<GetLoanDto[]> GetAllShortPeriod() =>
        await (from loan in context.Set<Loan>()
            where loan.InstallmentCount <= 12
            select new GetLoanDto
            {
                Amount = loan.Amount,
                InstallmentCount = loan.InstallmentCount,
                AnnualInterestRate = loan.AnnualInterestRate
            }).ToArrayAsync();

    public async Task<GetLoanDto[]> GetAllLongPeriod() =>
        await (from loan in context.Set<Loan>()
            where loan.InstallmentCount > 12
            select new GetLoanDto
            {
                Amount = loan.Amount,
                InstallmentCount = loan.InstallmentCount,
                AnnualInterestRate = loan.AnnualInterestRate
            }).ToArrayAsync();
}