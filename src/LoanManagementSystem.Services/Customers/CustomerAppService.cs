using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Customers.Enums;
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
        IsEmailValid(dto.Email);

        if (!IsNationalIdValid(dto.NationalId))
        {
            throw new InvalidNationalIdException();
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
        IsEmailValid(dto.Email);

        if (!IsNationalIdValid(dto.NationalId))
        {
            throw new InvalidNationalIdException();
        }

        if (await customerRepository.IsDuplicateByNationalId(dto.NationalId))
        {
            throw new NationalIdDuplicateException();
        }


        var incomeGroup = dto.Income > 10M ? IncomeGroup.MoreThanTen :
            dto.Income >= 5M ? IncomeGroup.FiveUptoIncludingTen : IncomeGroup.LessThanFive;

        var jobType = (JobType)Enum.Parse(typeof(JobType), dto.JobType, true);

        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalId = dto.NationalId,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            JobType = jobType,
            IncomeGroup = incomeGroup,
            NetWorth = dto.NetWorth,
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
        customerRepository.Update(customer);
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
        customerRepository.Update(customer);
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
        customer.IsVerified = false;
        customerRepository.Update(customer);
        await unitOfWork.Save();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    private static void IsEmailValid(string email)
    {
        try
        {
            _ = new MailAddress(email).Address;
        }
        catch (Exception e)
        {
            throw new InvalidEmailException();
        }
    }

    private static bool IsNationalIdValid(string nationalId) =>
        Regex.IsMatch(nationalId, @"^\d{10}\b");
}