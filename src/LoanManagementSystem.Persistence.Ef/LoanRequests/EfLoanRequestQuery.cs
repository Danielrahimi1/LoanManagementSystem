using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Installments.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.LoanRequests;

public class EfLoanRequestQuery(EfDataContext context) : LoanRequestQuery
{
    public async Task<GetLoanRequestDto?> GetById(int id) =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Id == id
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).FirstOrDefaultAsync();

    public async Task<GetLoanRequestDto[]> GetAllByCustomer(int customerId) =>
        await (from lr in context.Set<LoanRequest>()
            where lr.CustomerId == customerId
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAll() =>
        await (from lr in context.Set<LoanRequest>()
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllAcceptLoans() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Accept
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllActiveLoans() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Active
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllDelayedLoans() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.DelayInRepayment == true
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllClosedLoans() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Close
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestWithCustomerDto[]> GetAllWithCustomer() =>
        await (from lr in context.Set<LoanRequest>()
            join c in context.Set<Customer>()
                on lr.CustomerId equals c.Id
            select new GetLoanRequestWithCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                LoanId = lr.LoanId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestWithCustomerDto[]> GetAllAcceptLoansWithCustomer() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Accept
            join c in context.Set<Customer>()
                on lr.CustomerId equals c.Id
            select new GetLoanRequestWithCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                LoanId = lr.LoanId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestWithCustomerDto[]> GetAllActiveLoansWithCustomer() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Active
            join c in context.Set<Customer>()
                on lr.CustomerId equals c.Id
            select new GetLoanRequestWithCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                LoanId = lr.LoanId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestWithCustomerDto[]> GetAllDelayedLoansWithCustomer() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.DelayInRepayment == true
            join c in context.Set<Customer>()
                on lr.CustomerId equals c.Id
            select new GetLoanRequestWithCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                LoanId = lr.LoanId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestWithCustomerDto[]> GetAllClosedLoansWithCustomer() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.Status == LoanRequestStatus.Close
            join c in context.Set<Customer>()
                on lr.CustomerId equals c.Id
            select new GetLoanRequestWithCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalId = c.NationalId,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                LoanId = lr.LoanId,
                Status = lr.Status.ToString(),
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetRemainingLoanDto[]> GetAllRemainingLoans()
    {
        return await (from lr in context.Set<LoanRequest>()
                where lr.Status == LoanRequestStatus.Active
                join i in context.Set<Installment>() on lr.Id equals i.LoanRequestId
                group i by lr into g
                select new GetRemainingLoanDto
                {
                    Status = g.Key.Status.ToString(),
                    IsDelayed = g.Key.DelayInRepayment,
                    TotalPaid = g.Where(i => i.PaymentDate != null).Sum(i=>i.Amount+i.Fine+i.MonthlyInterest),
                    Installments = g.Where(i => i.PaymentDate == null).
                        Select(i => new GetInstallmentDto
                    {
                        LoanRequestId = i.LoanRequestId,
                        Amount = i.Amount,
                        MonthlyInterest = i.MonthlyInterest,
                        PaymentDeadLine = i.PaymentDeadLine,
                        PaymentDate = null,
                        Fine = i.Fine
                    }).ToArray()
                }
            ).ToArrayAsync();
    }
}