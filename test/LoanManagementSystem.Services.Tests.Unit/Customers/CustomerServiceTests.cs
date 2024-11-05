using FluentAssertions;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Services.Tests.Unit.Customers;

public class CustomerServiceTests : BusinessIntegrationTest
{
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _sut = CustomerServiceFactory.CreateService(SetupContext);
    }

    [Fact]
    public async Task Register_add_customer_when_given_AddCustomerDto()
    {
        var dto = new AddCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = "1234567890",
            PhoneNumber = "09001239876",
            Email = "name@gmail.com",
        };

        await _sut.Register(dto);

        var expected = ReadContext.Set<Customer>().Single();
        expected.FirstName.Should().Be(dto.FirstName);
        expected.LastName.Should().Be(dto.LastName);
        expected.NationalId.Should().Be(dto.NationalId);
        expected.PhoneNumber.Should().Be(dto.PhoneNumber);
        expected.Email.Should().Be(dto.Email);
    }

    [Theory]
    [InlineData("123asd")]
    [InlineData("12648a75311")]
    [InlineData("12648a75311 1234567899")]
    public async Task Register_throw_exception_when_nationalId_is_invalid(string nationalId)
    {
        var dto = new AddCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = nationalId,
            PhoneNumber = "09001239876",
            Email = "name@gmail.com",
        };

        var expected = async () => await _sut.Register(dto);

        await expected.Should().ThrowExactlyAsync<InvalidNationalIdException>();
    }

    [Theory]
    [InlineData("name@@gmail.com")]
    [InlineData("gmail.com")]
    public async Task Register_throw_exception_when_email_is_invalid(string email)
    {
        var dto = new AddCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = "1234567890",
            PhoneNumber = "09001239876",
            Email = email,
        };

        var expected = async () => await _sut.Register(dto);

        await expected.Should().ThrowExactlyAsync<InvalidEmailException>();
    }

    [Fact]
    public async Task Register_throw_exception_when_nationalId_is_duplicate()
    {
        var dto = new AddCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = "1234567890",
            PhoneNumber = "09001239876",
            Email = "name@gmail.com",
        };
        var dto2 = new AddCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = "1234567890",
            PhoneNumber = "09001239876",
            Email = "name@gmail.com",
        };
        await _sut.Register(dto);
        var expected = async () => await _sut.Register(dto2);

        await expected.Should().ThrowExactlyAsync<NationalIdDuplicateException>();
    }

    [Fact]
    public async Task RegisterWithStatement_add_customer_with_statement_when_given_AddCustomerWithStatementDto()
    {
        var dto = new AddCustomerWithStatementDto
        {
            FirstName = "John",
            LastName = "Smith",
            NationalId = "1234567890",
            PhoneNumber = "09001239876",
            Email = "name@gmail.com",
            JobType = "SelfEmployed",
            Income = 100M,
            NetWorth = 1000M
        };

        await _sut.RegisterWithStatement(dto);

        var expected = ReadContext.Set<Customer>().Single();
        expected.FirstName.Should().Be(dto.FirstName);
        expected.LastName.Should().Be(dto.LastName);
        expected.NationalId.Should().Be(dto.NationalId);
        expected.PhoneNumber.Should().Be(dto.PhoneNumber);
        expected.Email.Should().Be(dto.Email);
        expected.JobType.ToString().Should().Be(dto.JobType);
        expected.NetWorth.Should().Be(dto.NetWorth);
        expected.IncomeGroup.Should().Be(IncomeGroup.MoreThanTen);
    }
}