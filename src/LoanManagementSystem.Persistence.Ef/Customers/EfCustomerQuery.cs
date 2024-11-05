using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class EfCustomerQuery(EfDataContext context) : CustomerQuery
{
    public async Task<GetCustomerDto?> GetById(int id) =>
        await (from c in context.Set<Customer>()
            where c.Id == id
            select new GetCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore
            }).FirstOrDefaultAsync();

    public async Task<GetCustomerWithStatementDto?> GetByIdWithStatement(int id) =>
        await (from c in context.Set<Customer>()
            where c.Id == id
            select new GetCustomerWithStatementDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore,
                JobType = c.JobType.ToString(),
                IncomeGroup = c.IncomeGroup.ToString(),
                NetWorth = c.NetWorth
            }).FirstOrDefaultAsync();

    public async Task<GetCustomerDto[]> GetAll() =>
        await (from c in context.Set<Customer>()
            select new GetCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore
            }).ToArrayAsync();

    public async Task<GetCustomerWithStatementDto[]> GetAllWithStatement() =>
        await (from c in context.Set<Customer>()
            select new GetCustomerWithStatementDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore,
                JobType = c.JobType.ToString(),
                IncomeGroup = c.IncomeGroup.ToString(),
                NetWorth = c.NetWorth
            }).ToArrayAsync();

    public async Task<GetCustomerDto[]> GetRiskyCustomers() =>
        await (from i in (from i in context.Set<Installment>()
                where i.Fine > 0
                join lr in context.Set<LoanRequest>()
                    on i.LoanRequestId equals lr.Id
                group i by lr.CustomerId
                into g
                select new
                {
                    CustomerId = g.Key,
                    Delays = g.Count()
                }).Where(ig => ig.Delays > 1)
            join c in context.Set<Customer>() on i.CustomerId equals c.Id
            select new GetCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore,
            }).ToArrayAsync();

    public async Task<GetCustomerWithStatementDto[]> GetRiskyCustomersWithStatement()
    {
        var installments = (
            from i in context.Set<Installment>()
            where i.Fine > 0
            join lr in context.Set<LoanRequest>()
                on i.LoanRequestId equals lr.Id
            group i by lr.CustomerId
            into g
            select new
            {
                CustomerId = g.Key,
                Delays = g.Count()
            }).Where(ig => ig.Delays > 1);
        return await (from i in installments
            join c in context.Set<Customer>() 
                on i.CustomerId equals c.Id
            select new GetCustomerWithStatementDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Balance = c.Balance,
                IsVerified = c.IsVerified,
                CreditScore = c.CreditScore,
                JobType = c.JobType.ToString(),
                IncomeGroup = c.IncomeGroup.ToString(),
                NetWorth = c.NetWorth
            }).ToArrayAsync();
    }
}