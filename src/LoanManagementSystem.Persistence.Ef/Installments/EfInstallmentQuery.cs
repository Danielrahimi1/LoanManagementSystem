using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Installments.Contracts.DTOs;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class EfInstallmentQuery(EfDataContext context) : InstallmentQuery
{
    public async Task<GetInstallmentDto?> GetById(int id) =>
        await (from i in context.Set<Installment>()
            where i.Id == id
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).FirstOrDefaultAsync();


    public async Task<GetInstallmentDto[]> GetAll() =>
        await (from i in context.Set<Installment>()
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();

    public async Task<GetInstallmentDto[]> GetAllByCustomer(int customerId) =>
        await (from c in context.Set<Customer>()
            where c.Id == customerId
            join lr in context.Set<LoanRequest>()
                on c.Id equals lr.CustomerId
            join i in context.Set<Installment>()
                on lr.Id equals i.LoanRequestId
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();

    public async Task<GetInstallmentDto[]> GetAllByLoan(int loanId) =>
        await (from l in context.Set<Loan>()
            where l.Id == loanId
            join lr in context.Set<LoanRequest>()
                on l.Id equals lr.LoanId
            join i in context.Set<Installment>()
                on lr.Id equals i.LoanRequestId
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();

    public async Task<GetInstallmentDto[]> GetAllByLoanRequest(int loanRequestId) =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Id == loanRequestId
            join i in context.Set<Installment>()
                on lr.Id equals i.LoanRequestId
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();

    public async Task<GetInstallmentDto[]> GetAllDelayed() =>
        await (from i in context.Set<Installment>()
            where i.PaymentDeadLine < i.PaymentDate
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();

    public async Task<GetInstallmentDto[]> GetAllClosed() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Close
            join i in context.Set<Installment>()
                on lr.Id equals i.LoanRequestId
            select new GetInstallmentDto
            {
                LoanRequestId = i.LoanRequestId,
                Amount = i.Amount,
                MonthlyInterest = i.MonthlyInterest,
                PaymentDeadLine = i.PaymentDeadLine,
                PaymentDate = i.PaymentDate,
                Fine = i.Fine
            }).ToArrayAsync();
}