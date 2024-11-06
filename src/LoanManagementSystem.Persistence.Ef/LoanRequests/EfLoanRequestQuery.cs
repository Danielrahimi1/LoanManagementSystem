using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
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
                Status = lr.Status,
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
                Status = lr.Status,
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
                Status = lr.Status,
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllActiveLoans() =>
        await (from lr in context.Set<LoanRequest>()
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status,
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate,
            }).ToArrayAsync();

    public async Task<GetLoanRequestDto[]> GetAllDelayedLoans() =>
        await (from lr in context.Set<LoanRequest>()
            where lr.DelayInRepayment == true
            select new GetLoanRequestDto
            {
                LoanId = lr.LoanId,
                CustomerId = lr.CustomerId,
                Status = lr.Status,
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();

    public Task<GetLoanRequestDto[]> GetAllDoneLoans()
    {
        throw new NotImplementedException();
    }

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
                Status = lr.Status,
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();
    
    public async Task<GetLoanRequestWithCustomerDto[]> GetAllActiveLoansWithCustomer() =>
        await(from lr in context.Set<LoanRequest>()
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
                Status = lr.Status,
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
                Status = lr.Status,
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
                Status = lr.Status,
                DelayInRepayment = lr.DelayInRepayment,
                ConfirmationDate = lr.ConfirmationDate,
                Rate = lr.Rate
            }).ToArrayAsync();
}