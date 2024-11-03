using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Statements;
using LoanManagementSystem.Entities.Statements.Enums;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Customers;

public class CustomerAppService(
    CustomerRepository customerRepository,
    UnitOfWork unitOfWork
) : CustomerService
{
    public async Task Register(AddCustomerDto dto)
    {
        if (IsEmailValid(dto.Email))
        {
            throw new EmailNotValidException();
        }

        if (IsNationalIdValid(dto.NationalId))
        {
            throw new NationalIdDuplicateException();
        }

        if (await customerRepository.IsDuplicateByNationalId(dto.NationalId))
        {
            throw new NationalIdDuplicateException();
        }

        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalId = dto.NationalId,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
        };
        await customerRepository.Add(customer);
        await unitOfWork.Save();
    }

    public async Task RegisterWithStatement(AddCustomerWithStatementDto dto)
    {
        if (IsEmailValid(dto.Email))
        {
            throw new EmailNotValidException();
        }

        if (IsNationalIdValid(dto.NationalId))
        {
            throw new NationalIdDuplicateException();
        }

        if (await customerRepository.IsDuplicateByNationalId(dto.NationalId))
        {
            throw new NationalIdDuplicateException();
        }

        var incomeGroup = dto.Income > 10M ? IncomeGroup.MoreThanTen :
            dto.Income >= 5M ? IncomeGroup.FiveUptoIncludingTen : IncomeGroup.LessThanFive;

        var jobType = Enum.Parse(typeof(JobType), dto.JobType, true);

        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalId = dto.NationalId,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Statement = new Statement
            {
                JobType = (JobType)jobType,
                IncomeGroup = incomeGroup,
                NetWorth = dto.NetWorth,
            }
        };
        await customerRepository.Add(customer);
        await unitOfWork.Save();
    }

    public async Task Verify(int id)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task VerifyManually(int id)
    {
        var customer = await customerRepository.Find(id);
        if (customer is null)
        {
            throw new CustomerNotFoundException();
        }

        customer.IsVerified = true;
        await customerRepository.Update(customer);
        await unitOfWork.Save();
    }

    public async Task Charge(int id, UpdateBalanceDto dto)
    {
        var customer = await customerRepository.Find(id);
        if (customer is null)
        {
            throw new CustomerNotFoundException();
        }
        
        customer.Balance += dto.Charge;
        await customerRepository.Update(customer);
        await unitOfWork.Save();
    }

    public async Task Update(int id, UpdateCustomerDto dto)
    {
        var customer = await customerRepository.Find(id);
        if (customer is null)
        {
            throw new CustomerNotFoundException();
        }

        if (dto.NationalId != null)
        {
            if (await customerRepository.IsDuplicateByNationalId(dto.NationalId))
            {
                throw new NationalIdDuplicateException();
            }

            customer.NationalId = dto.NationalId;
        }

        customer.FirstName = dto.FirstName ?? customer.FirstName;
        customer.LastName = dto.LastName ?? customer.LastName;
        customer.PhoneNumber = dto.PhoneNumber ?? customer.PhoneNumber;
        customer.Email = dto.Email ?? customer.Email;
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    private static bool IsEmailValid(string email) =>
        new MailAddress(email).Address != email;

    private static bool IsNationalIdValid(string nationalId) =>
        new GeneratedRegexAttribute("\\d{10}").Match(nationalId);
}