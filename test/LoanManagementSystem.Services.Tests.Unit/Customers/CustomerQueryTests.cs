using FluentAssertions;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanRequests;
using LoanManagementSystem.TestTools.Loans;
using Xunit.Abstractions;

namespace LoanManagementSystem.Services.Tests.Unit.Customers;

public class CustomerQueryTests : BusinessIntegrationTest
{
    private readonly CustomerQuery _sut;


    public CustomerQueryTests()
    {
        _sut = new EfCustomerQuery(ReadContext);
    }

    [Fact]
    public async Task GetById_return_GetCustomerDto__when_passed_customer_id()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899")
            .WithPhoneNumber("09001239876").WithEmail("john@outlook.com")
            .WithBalance(26).WithIsVerified(true).WithCreditScore(90)
            .Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithEmail("jacob@outlook.com").WithNationalId("1234567898").Build();
        Save(customer1, customer2);

        var actual = await _sut.GetById(customer1.Id);

        actual!.FirstName.Should().Be(customer1.FirstName);
        actual.LastName.Should().Be(customer1.LastName);
        actual.NationalId.Should().Be(customer1.NationalId);
        actual.PhoneNumber.Should().Be(customer1.PhoneNumber);
        actual.Email.Should().Be(customer1.Email);
        actual.Balance.Should().Be(customer1.Balance);
        actual.IsVerified.Should().Be(customer1.IsVerified);
        actual.CreditScore.Should().Be(customer1.CreditScore);
    }

    [Fact]
    public async Task GetByIdWithStatement_return_GetCustomerWithStatementDto__when_passed_customer_id()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899").WithPhoneNumber("09001239876")
            .WithEmail("john@outlook.com").WithBalance(26)
            .WithIsVerified(true).WithCreditScore(90)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithEmail("jacob@outlook.com").WithNationalId("1234567898").Build();
        Save(customer1, customer2);

        var actual = await _sut.GetByIdWithStatement(customer2.Id);

        actual!.FirstName.Should().Be(customer2.FirstName);
        actual.LastName.Should().Be(customer2.LastName);
        actual.NationalId.Should().Be(customer2.NationalId);
        actual.PhoneNumber.Should().Be(customer2.PhoneNumber);
        actual.Email.Should().Be(customer2.Email);
        actual.Balance.Should().Be(customer2.Balance);
        actual.IsVerified.Should().Be(customer2.IsVerified);
        actual.CreditScore.Should().Be(customer2.CreditScore);
        actual.IncomeGroup.Should().Be(customer2.IncomeGroup.ToString());
        actual.JobType.Should().Be(customer2.JobType.ToString());
        actual.NetWorth.Should().Be(customer2.NetWorth);
    }

    [Fact]
    public async Task GetById_return_null_GetCustomerDto__when_passed_customer_id_not_found()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899")
            .WithPhoneNumber("09001239876").WithEmail("john@outlook.com")
            .WithBalance(26).WithIsVerified(true).WithCreditScore(90)
            .Build();
        Save(customer1);
        var actual = await _sut.GetById(-1);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_return_all_when_invoked()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899").WithPhoneNumber("09001239876")
            .WithEmail("john@outlook.com").WithBalance(26)
            .WithIsVerified(true).WithCreditScore(90).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true).WithCreditScore(89).Build();
        Save(customer1, customer2);

        var actual = await _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetCustomerDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                Balance = customer1.Balance,
                IsVerified = customer1.IsVerified,
                CreditScore = customer1.CreditScore
            },
            new GetCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                Balance = customer2.Balance,
                IsVerified = customer2.IsVerified,
                CreditScore = customer2.CreditScore
            }
        ]);
    }

    [Fact]
    public async Task GetAllWithStatement_return_all_customers_with_statement_when_invoked()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899").WithPhoneNumber("09001239876")
            .WithEmail("john@outlook.com").WithBalance(26)
            .WithIsVerified(true).WithCreditScore(90)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true).WithCreditScore(89)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.UnEmployed)
            .WithNetWorth(18546).Build();
        Save(customer1, customer2);


        var actual = await _sut.GetAllWithStatement();

        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo([
            new GetCustomerWithStatementDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                Balance = customer1.Balance,
                IsVerified = customer1.IsVerified,
                CreditScore = customer1.CreditScore,
                JobType = customer1.JobType.ToString(),
                IncomeGroup = customer1.IncomeGroup.ToString(),
                NetWorth = customer1.NetWorth
            },
            new GetCustomerWithStatementDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                Balance = customer2.Balance,
                IsVerified = customer2.IsVerified,
                CreditScore = customer2.CreditScore,
                JobType = customer2.JobType.ToString(),
                IncomeGroup = customer2.IncomeGroup.ToString(),
                NetWorth = customer2.NetWorth
            }
        ]);
    }

    [Fact]
    public async Task GetRiskyCustomers_return_all_risky_customers_when_invoked()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe").WithNationalId("1234567899")
            .WithPhoneNumber("09001239876").WithEmail("john@outlook.com").Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe").WithNationalId("1234567899")
            .WithPhoneNumber("09001239876").WithEmail("john@outlook.com").Build();
        var lr1 = new LoanRequestBuilder().Build();
        var lr2 = new LoanRequestBuilder().Build();
        var lr3 = new LoanRequestBuilder().Build();
        var in11 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in12 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in13 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in21 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in22 = new InstallmentBuilder().Build();
        lr1.Installments.UnionWith([in11]);
        lr2.Installments.UnionWith([in12]);
        lr3.Installments.UnionWith([in11]);
        customer1.LoanRequests.Add(lr1);
        customer2.LoanRequests.UnionWith([lr2,lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetRiskyCustomers();

        actual.Should().HaveCount(1);
        actual.Should().BeEquivalentTo([
            new GetCustomerDto
            {
                FirstName = customer2.FirstName,
                LastName = customer2.LastName,
                NationalId = customer2.NationalId,
                PhoneNumber = customer2.PhoneNumber,
                Email = customer2.Email,
                Balance = customer2.Balance,
                IsVerified = customer2.IsVerified,
                CreditScore = customer2.CreditScore
            }
        ]);
    }

    [Fact]
    public async Task GetRiskyCustomersWithStatement_return_all_risky_customers_with_statement_when_invoked()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899").WithPhoneNumber("09001239876")
            .WithEmail("john@outlook.com").WithBalance(26)
            .WithIsVerified(true).WithCreditScore(90)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true).WithCreditScore(89)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.UnEmployed)
            .WithNetWorth(18546).Build();
        var lr1 = new LoanRequestBuilder().Build();
        var lr2 = new LoanRequestBuilder().Build();
        var lr3 = new LoanRequestBuilder().Build();
        var in1 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in2 = new InstallmentBuilder().WithFine(0.01m).Build();
        var in3 = new InstallmentBuilder().WithFine(0.01m).Build();
        lr1.Installments.UnionWith([in1]);
        lr2.Installments.UnionWith([in2]);
        lr3.Installments.UnionWith([in3]);
        customer1.LoanRequests.UnionWith([lr1,lr2]);
        customer2.LoanRequests.Add(lr3);
        Save(customer1, customer2);

        var actual = await _sut.GetRiskyCustomersWithStatement();

        actual.Should().HaveCount(1);
        actual.Should().BeEquivalentTo([
            new GetCustomerWithStatementDto
            {
                FirstName = customer1.FirstName,
                LastName = customer1.LastName,
                NationalId = customer1.NationalId,
                PhoneNumber = customer1.PhoneNumber,
                Email = customer1.Email,
                Balance = customer1.Balance,
                IsVerified = customer1.IsVerified,
                CreditScore = customer1.CreditScore,
                JobType = customer1.JobType.ToString(),
                IncomeGroup = customer1.IncomeGroup.ToString(),
                NetWorth = customer1.NetWorth
            }
        ]);
    }
}