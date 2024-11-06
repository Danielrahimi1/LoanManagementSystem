using FluentAssertions;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanRequests;
using LoanManagementSystem.TestTools.Loans;

namespace LoanManagementSystem.Services.Tests.Unit.LoanRequests;

public class LoanRequestQueryTests : BusinessIntegrationTest
{
    private readonly LoanRequestQuery _sut;

    public LoanRequestQueryTests() =>
        _sut = new EfLoanRequestQuery(ReadContext);

    [Fact]
    public async Task GetById_return_GetLoanRequestDto__when_passed_loanRequest_id()
    {
        var c = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder().Build();
        var lr2 = new LoanRequestBuilder().Build();
        c.LoanRequests.UnionWith([lr1, lr2]);
        Save(c);

        var actual = await _sut.GetById(lr1.Id);

        actual!.LoanId.Should().Be(lr1.LoanId);
        actual.CustomerId.Should().Be(lr1.CustomerId);
        actual.Status.Should().Be(lr1.Status);
        actual.DelayInRepayment.Should().Be(lr1.DelayInRepayment);
        actual.Rate.Should().Be(lr1.Rate);
        actual.ConfirmationDate.Should().Be(lr1.ConfirmationDate);
    }

    [Fact]
    public async Task GetAllByCustomer_return_specific_customer_loanRequests_when_given_customer_id()
    {
        var loan1 = new LoanBuilder().Build();
        Save(loan1);
        var customer1 = new CustomerBuilder().Build();
        var lr1 = new LoanRequestBuilder()
            .WithLoanId(loan1.Id).WithRate(15)
            .WithStatus(LoanRequestStatus.Close).WithDelayInRepayment(false)
            .WithConfirmationDate(new DateOnly(2020, 1, 1))
            .Build();
        var lr2 = new LoanRequestBuilder()
            .WithLoanId(loan1.Id).WithRate(20)
            .WithStatus(LoanRequestStatus.Reject).WithDelayInRepayment(false)
            .WithConfirmationDate(new DateOnly(2019, 12, 30))
            .Build();
        customer1.LoanRequests.UnionWith([lr1, lr2]);
        var customer2 = new CustomerBuilder().Build();
        customer2.LoanRequests.UnionWith([
            new LoanRequestBuilder().Build(),
        ]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllByCustomer(customer1.Id);
        actual.Should().BeEquivalentTo([
            new GetLoanRequestDto
            {
                LoanId = lr1.LoanId,
                CustomerId = lr1.CustomerId,
                Rate = lr1.Rate,
                Status = lr1.Status,
                DelayInRepayment = lr1.DelayInRepayment,
                ConfirmationDate = lr1.ConfirmationDate
            },
            new GetLoanRequestDto
            {
                LoanId = lr2.LoanId,
                CustomerId = lr2.CustomerId,
                Rate = lr2.Rate,
                Status = lr2.Status,
                DelayInRepayment = lr2.DelayInRepayment,
                ConfirmationDate = lr2.ConfirmationDate
            }
        ]);
    }

    [Fact]
    public async Task GetById_return_null_GetCustomerDto__when_passed_customer_id_not_found()
    {
        var customer1 = new CustomerBuilder()
            .WithFirstName("John").WithLastName("Doe")
            .WithNationalId("1234567899")
            .WithPhoneNumber("09001239876").WithEmail("john@outlook.com")
            .WithBalance(26).WithIsVerified(true)
            // .WithCreditScore(90)
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
            .WithIsVerified(true)
            // .WithCreditScore(90)
            .Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true)
            // .WithCreditScore(89)
            .Build();
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
                // CreditScore = customer1.CreditScore
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
                // CreditScore = customer2.CreditScore
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
            .WithIsVerified(true)
            // .WithCreditScore(90)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true)
            // .WithCreditScore(89)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.UnEmployed)
            .WithNetWorth(18546).Build();
        Save(customer1, customer2);


        var actual = await _sut.GetAllActiveLoans();

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
                // CreditScore = customer1.CreditScore,
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
                // CreditScore = customer2.CreditScore,
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
        // var in13 = new InstallmentBuilder().WithFine(0.01m).Build();
        // var in21 = new InstallmentBuilder().WithFine(0.01m).Build();
        // var in22 = new InstallmentBuilder().Build();
        lr1.Installments.UnionWith([in11]);
        lr2.Installments.UnionWith([in12]);
        lr3.Installments.UnionWith([in11]);
        customer1.LoanRequests.Add(lr1);
        customer2.LoanRequests.UnionWith([lr2, lr3]);
        Save(customer1, customer2);

        var actual = await _sut.GetAllDoneLoans();

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
                // CreditScore = customer2.CreditScore
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
            .WithIsVerified(true)
            // .WithCreditScore(90)
            .WithIncomeGroup(IncomeGroup.MoreThanTen).WithJobType(JobType.SelfEmployed)
            .WithNetWorth(15644).Build();
        var customer2 = new CustomerBuilder()
            .WithFirstName("Jacob").WithLastName("Doe")
            .WithNationalId("1234567898").WithPhoneNumber("09001234321")
            .WithEmail("jacob@outlook.com").WithBalance(34)
            .WithIsVerified(true)
            // .WithCreditScore(89)
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
        customer1.LoanRequests.UnionWith([lr1, lr2]);
        customer2.LoanRequests.Add(lr3);
        Save(customer1, customer2);

        var actual = await _sut.GetAllDelayedLoansWithCustomer();

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
                // CreditScore = customer1.CreditScore,
                JobType = customer1.JobType.ToString(),
                IncomeGroup = customer1.IncomeGroup.ToString(),
                NetWorth = customer1.NetWorth
            }
        ]);
    }
}