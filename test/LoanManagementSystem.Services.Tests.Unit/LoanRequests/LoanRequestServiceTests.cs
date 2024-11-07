using FluentAssertions;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Exceptions;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanRequests;
using LoanManagementSystem.TestTools.Loans;

namespace LoanManagementSystem.Services.Tests.Unit.LoanRequests;

public class LoanRequestServiceTests : BusinessIntegrationTest
{
    private readonly LoanRequestService _sut;

    public LoanRequestServiceTests() =>
        _sut = LoanRequestServiceFactory.CreateService(SetupContext);

    [Fact]
    public async Task Open_throw_exception_when_customer_is_not_found()
    {
        var dto = new AddLoanRequestDto
        {
            LoanId = 0,
            CustomerId = 0
        };

        var actual = async () => await _sut.Open(-1, dto);

        await actual.Should().ThrowExactlyAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Open_throw_exception_when_customer_is_not_verified()
    {
        var customer = new CustomerBuilder().WithIsVerified(false).Build();
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = 0,
            CustomerId = 0
        };

        var actual = async () => await _sut.Open(customer.Id, dto);

        await actual.Should().ThrowExactlyAsync<CustomerNotVerifiedException>();
    }

    [Fact]
    public async Task Open_throw_exception_when_customer_has_active_loan_request()
    {
        var customer = new CustomerBuilder().WithIsVerified(true).Build();
        customer.LoanRequests.Add(new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).Build());
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = 0,
            CustomerId = 0
        };

        var actual = async () => await _sut.Open(customer.Id, dto);

        await actual.Should().ThrowExactlyAsync<CustomerHasActiveLoanRequestsException>();
    }

    [Fact]
    public async Task Open_throw_exception_when_customer_has_accept_loan_request()
    {
        var customer = new CustomerBuilder().WithIsVerified(true).Build();
        customer.LoanRequests.Add(new LoanRequestBuilder().WithStatus(LoanRequestStatus.Accept).Build());
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = 0,
            CustomerId = 0
        };

        var actual = async () => await _sut.Open(customer.Id, dto);

        await actual.Should().ThrowExactlyAsync<CustomerHasActiveLoanRequestsException>();
    }

    [Fact]
    public async Task Open_throw_exception_when_loan_is_not_found()
    {
        var customer = new CustomerBuilder().WithIsVerified(true).Build();
        customer.LoanRequests.Add(new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).Build());
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = -1,
            CustomerId = 0
        };

        var actual = async () => await _sut.Open(customer.Id, dto);

        await actual.Should().ThrowExactlyAsync<LoanNotFoundException>();
    }

    [Fact]
    public async Task Open_increase_rate_by_thirty_per_loan_requests_when_loan_requests_are_non_delayed()
    {
        var loan = new LoanBuilder().WithAmount(100).Build();
        Save(loan);
        var customer = new CustomerBuilder().WithNetWorth(0).WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.LessThanFive)
            .WithJobType(JobType.UnEmployed).Build();
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        customer.LoanRequests.UnionWith([lr1, lr2]);
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = loan.Id,
            CustomerId = customer.Id
        };

        await _sut.Open(customer.Id, dto);
        var actual = ReadContext.Set<LoanRequest>();

        actual.Should().ContainSingle(lr => lr.Rate == 30 + 30);
    }

    [Fact]
    public async Task Open_decrease_rate_by_five_per_installment_when_installments_are_delayed()
    {
        var loan = new LoanBuilder().WithAmount(0).Build();
        Save(loan);
        var customer = new CustomerBuilder().WithNetWorth(0).WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.LessThanFive)
            .WithJobType(JobType.UnEmployed).Build();
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var i1 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).Build(); // -5
        lr1.Installments.UnionWith([i1, i2, i3]);
        customer.LoanRequests.UnionWith([lr1, lr2, lr3]);
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = loan.Id,
            CustomerId = customer.Id
        };

        await _sut.Open(customer.Id, dto);
        var actual = ReadContext.Set<LoanRequest>();

        actual.Should().ContainSingle(lr => lr.Rate == 30 + 30 + 30 - 5 - 5 - 5);
    }

    [Fact]
    public async Task Open_increase_rate_by_twenty_when_loan_net_worth_ratio_below_half()
    {
        var loan = new LoanBuilder().WithAmount(40).Build();
        Save(loan);
        var customer = new CustomerBuilder().WithNetWorth(100).WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.LessThanFive)
            .WithJobType(JobType.UnEmployed).Build();
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var i1 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).Build(); // -5
        lr1.Installments.UnionWith([i1, i2, i3]);
        customer.LoanRequests.UnionWith([lr1, lr2, lr3]);
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = loan.Id,
            CustomerId = customer.Id
        };

        await _sut.Open(customer.Id, dto);
        var actual = ReadContext.Set<LoanRequest>();

        actual.Should().ContainSingle(lr => lr.Rate == 30 + 30 + 30 - 5 - 5 - 5 + 20);
    }

    [Fact]
    public async Task Open_increase_rate_by_ten_when_loan_net_worth_ratio_below_seven_tenths()
    {
        var loan = new LoanBuilder().WithAmount(69).Build();
        Save(loan);
        var customer = new CustomerBuilder().WithNetWorth(100).WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.LessThanFive)
            .WithJobType(JobType.UnEmployed).Build();
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var i1 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).Build(); // -5
        lr1.Installments.UnionWith([i1, i2, i3]);
        customer.LoanRequests.UnionWith([lr1, lr2, lr3]);
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = loan.Id,
            CustomerId = customer.Id
        };

        await _sut.Open(customer.Id, dto);
        var actual = ReadContext.Set<LoanRequest>();

        actual.Should().ContainSingle(lr => lr.Rate == 30 + 30 + 30 - 5 - 5 - 5 + 10);
    }

    [Fact]
    public async Task Open_cap_rate_at_hundred_when_rate_increased_by_more_than_hundred()
    {
        var loan = new LoanBuilder().WithAmount(40).Build();
        Save(loan);
        var customer = new CustomerBuilder().WithNetWorth(100).WithIsVerified(true)
            .WithIncomeGroup(IncomeGroup.LessThanFive)
            .WithJobType(JobType.UnEmployed).Build();
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr5 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr6 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var i1 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).Build(); // -5
        lr1.Installments.UnionWith([i1, i2, i3]);
        customer.LoanRequests.UnionWith([lr1, lr2, lr3, lr4, lr5, lr6]);
        Save(customer);
        var dto = new AddLoanRequestDto
        {
            LoanId = loan.Id,
            CustomerId = customer.Id
        };

        await _sut.Open(customer.Id, dto);
        var actual = ReadContext.Set<LoanRequest>();

        actual.Should().ContainSingle(lr => lr.Rate == 100);
    }

    [Fact]
    public async Task Accept_throw_exception_when_loan_request_not_found()
    {
        var actual = async () => await _sut.Accept(-1);

        await actual.Should().ThrowExactlyAsync<LoanRequestNotFoundException>();
    }

    [Fact]
    public async Task Accept_throw_exception_when_loan_request_is_not_in_review_state()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var lr = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Reject).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        var actual = async () => await _sut.Accept(lr.Id);

        await actual.Should().ThrowExactlyAsync<LoanRequestMustBeReviewedException>();
    }

    [Fact]
    public async Task Accept_change_loan_request_status_to_accept_when_status_is_review()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var lr = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Review).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        await _sut.Accept(lr.Id);
        var result = ReadContext.Set<LoanRequest>().Single();

        result.Should().BeEquivalentTo(new LoanRequest
        {
            Id = lr.Id,
            LoanId = lr.LoanId,
            CustomerId = lr.CustomerId,
            Status = LoanRequestStatus.Accept,
            DelayInRepayment = lr.DelayInRepayment,
            Rate = lr.Rate,
            ConfirmationDate = lr.ConfirmationDate,
            Customer = null,
            Installments = lr.Installments
        }, config => config.Excluding(item => item.Customer));
    }
}