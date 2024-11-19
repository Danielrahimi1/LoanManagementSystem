using FluentAssertions;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.LoanRequests;

namespace LoanManagementSystem.Services.Tests.Unit.Customers;

public class CustomerServiceTests : BusinessIntegrationTest
{
    private readonly CustomerService _sut;

    public CustomerServiceTests() =>
        _sut = CustomerServiceFactory.CreateService(SetupContext);

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
    [InlineData("1234567899a")]
    [InlineData("12345678999999")]
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

    [Fact]
    public async Task VerifyManually_set_IsVerified_property_to_true_when_given_customer_id()
    {
        var customer1 = new CustomerBuilder().WithFirstName("Jacob").WithIsVerified(false).Build();
        Save(customer1);

        await _sut.VerifyManually(customer1.Id);
        
        var actual = ReadContext.Set<Customer>().Single();
        actual.IsVerified.Should().Be(true);
    }

    [Fact]
    public async Task Charge_increase_balance_with_amount_when_given_UpdateBalanceDto()
    {
        var customer1 = new CustomerBuilder().WithFirstName("Jacob").WithBalance(315).Build();
        Save(customer1);
        var dto = new UpdateBalanceDto { Charge = 321 };

        await _sut.Charge(customer1.Id, dto);
        var actual = ReadContext.Set<Customer>().Single();

        actual.Balance.Should().Be(dto.Charge + customer1.Balance);
    }

    [Fact]
    public async Task Update_update_customer_details_when_given_updateCustomerDto()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899").WithPhoneNumber("09001239876")
            .WithEmail("john@outlook.com").WithBalance(26)
            .WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.UnEmployed)
            .WithNetWorth(18546).Build();
        Save(customer1, customer2);

        var dto = new UpdateCustomerDto
        {
            FirstName = "Giovanni",
            LastName = "Rossi",
            NationalId = "1234567999",
            PhoneNumber = "09391234321",
            Email = "giovanni@outlook.com",
            JobType = JobType.SelfEmployed.ToString(),
            Income = 128,
            NetWorth = 16768
        };

        await _sut.Update(customer2.Id, dto);
        var actual = ReadContext.Set<Customer>();
        actual.Should().BeEquivalentTo([
            customer1,
            new Customer
            {
                Id = customer2.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Balance = customer2.Balance,
                IsVerified = false,
                JobType = JobType.SelfEmployed,
                IncomeGroup = IncomeGroup.MoreThanTen,
                NetWorth = dto.NetWorth.Value,
            }
        ]);
    }

    [Fact]
    public async Task Delete_remove_a_customer_when_given_id()
    {
        var customer1 = new CustomerBuilder().WithFirstName("Jacob").Build();
        var customer2 = new CustomerBuilder().WithFirstName("Joseph").Build();
        Save(customer1, customer2);

        await _sut.Delete(customer2.Id);
        
        var expected = ReadContext.Set<Customer>().Single();
        expected.FirstName.Should().Be(customer1.FirstName);
    }

    [Fact]
    public async Task Delete_throw_exception_when_given_id_not_found()
    {
        var customer1 = new CustomerBuilder().WithFirstName("Jacob").Build();
        var customer2 = new CustomerBuilder().WithFirstName("Joseph").Build();
        Save(customer1, customer2);

        var expected = async () => await _sut.Delete(-1);

        await expected.Should().ThrowExactlyAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Delete_throw_exception_when_customer_has_active_loanRequest()
    {
        var customer1 = new CustomerBuilder().WithFirstName("Jacob").Build();
        var lr = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).Build();
        customer1.LoanRequests.Add(lr);
        Save(customer1);

        var expected = async () => await _sut.Delete(customer1.Id);

        await expected.Should().ThrowExactlyAsync<CustomerHasActiveLoanRequestsException>();
    }
}