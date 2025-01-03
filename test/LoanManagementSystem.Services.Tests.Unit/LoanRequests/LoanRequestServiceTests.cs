using FluentAssertions;
using LoanManagementSystem.Entities.Customers.Enums;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.LoanRequests.Contracts.DTOs;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Exceptions;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;
using LoanManagementSystem.TestTools.Services.Loans;

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
        Save(customer);
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        Save(lr1, lr2, lr3, lr4);
        var i1 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        Save(i1, i2, i3);
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
        Save(customer);
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        Save(lr1, lr2, lr3, lr4);
        // customer.LoanRequests.UnionWith([lr1, lr2, lr3, lr4]);
        var i1 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        var i2 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        var i3 = new InstallmentBuilder().WithFine(1).WithLoanRequest(lr1).Build(); // -5
        Save(i1, i2, i3);
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
        Save(customer);
        var lr1 = new LoanRequestBuilder().WithDelayInRepayment(true)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build();
        var lr2 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder().WithDelayInRepayment(false)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .WithCustomerId(customer.Id)
            .Build(); // +30
        Save(lr1, lr2, lr3, lr4);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr1).WithFine(1).Build(); // -5
        var i2 = new InstallmentBuilder().WithLoanRequest(lr1).WithFine(1).Build(); // -5
        var i3 = new InstallmentBuilder().WithLoanRequest(lr1).WithFine(1).Build(); // -5
        Save(i1, i2, i3);
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
        Save(customer);
        var lr1 = new LoanRequestBuilder()
            .WithDelayInRepayment(true).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build();
        var lr2 = new LoanRequestBuilder()
            .WithDelayInRepayment(false).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr3 = new LoanRequestBuilder()
            .WithDelayInRepayment(false).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr4 = new LoanRequestBuilder()
            .WithDelayInRepayment(false).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr5 = new LoanRequestBuilder()
            .WithDelayInRepayment(false).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        var lr6 = new LoanRequestBuilder()
            .WithDelayInRepayment(false).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Close).WithRate(0)
            .Build(); // +30
        Save(lr1, lr2, lr3, lr4, lr5, lr6);
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
            Installments = []
        }, config => config.Excluding(item => item.Customer));
    }

    [Fact]
    public async Task Reject_change_loan_request_status_to_reject_when_status_is_review()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var lr = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Review).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        await _sut.Reject(lr.Id);

        var result = ReadContext.Set<LoanRequest>().Single();
        result.Should().BeEquivalentTo(new LoanRequest
        {
            Id = lr.Id,
            LoanId = lr.LoanId,
            CustomerId = lr.CustomerId,
            Status = LoanRequestStatus.Reject,
            DelayInRepayment = lr.DelayInRepayment,
            Rate = lr.Rate,
            ConfirmationDate = lr.ConfirmationDate,
            Installments = []
        }, config => config.Excluding(item => item.Customer));
    }

    [Fact]
    public async Task Activate_throw_exception_when_loan_request_status_is_not_accept()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var lr = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Review).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        var actual = async () => await _sut.Activate(lr.Id);

        await actual.Should().ThrowExactlyAsync<LoanRequestMustBeAcceptedException>();
    }

    [Fact]
    public async Task Activate_change_loan_request_status_to_active_when_status_is_accept()
    {
        var cDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loan = new LoanBuilder().WithAmount(10)
            .WithInstallmentCount(3).WithAnnualInterestRate(15).Build();
        Save(loan);
        var lr = new LoanRequestBuilder().WithConfirmationDate(cDate)
            .WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        await _sut.Activate(lr.Id);

        var result = ReadContext.Set<LoanRequest>().Single();
        result.Should().BeEquivalentTo(new LoanRequest
        {
            LoanId = lr.LoanId,
            CustomerId = lr.CustomerId,
            Status = LoanRequestStatus.Active,
            DelayInRepayment = lr.DelayInRepayment,
            Rate = lr.Rate,
            ConfirmationDate = cDate,
            Installments = []
        }, config => config.Excluding(item => item.Id));
    }

    [Fact]
    public async Task Activate_generate_installments_when_activated()
    {
        var cDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loan = new LoanBuilder().WithAmount(10)
            .WithInstallmentCount(3).WithAnnualInterestRate(15).Build();
        Save(loan);
        var lr = new LoanRequestBuilder().WithConfirmationDate(cDate)
            .WithStatus(LoanRequestStatus.Accept).WithLoanId(loan.Id).Build();
        customer.LoanRequests.Add(lr);
        Save(lr);

        await _sut.Activate(lr.Id);

        var result = ReadContext.Set<Installment>().ToList();
        result.Should().AllSatisfy(i => i.LoanRequestId.Should().Be(lr.Id));
        result.Should().AllSatisfy(i =>
            i.Amount.Should().Be(Math.Truncate(loan.Amount / loan.InstallmentCount * 100) / 100M));
        result.Should().AllSatisfy(i => i.MonthlyInterest.Should()
            .Be(Math.Truncate(loan.Amount / loan.InstallmentCount * loan.AnnualInterestRate / 12 / 100 * 100) / 100M));
        result.Should().AllSatisfy(i => i.Fine.Should()
            .Be(0));
        result.Should().BeEquivalentTo([
            new
            {
                PaymentDeadLine = cDate.AddMonths(1),
            },
            new
            {
                PaymentDeadLine = cDate.AddMonths(2),
            },
            new
            {
                PaymentDeadLine = cDate.AddMonths(3),
            }
        ]);
    }

    [Fact]
    public async Task Close_throw_exception_when_loan_request_status_is_not_active()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr = new LoanRequestBuilder()
            .WithCustomerId(c1.Id).WithStatus(LoanRequestStatus.Accept).Build();
        Save(lr);

        var expected = async () => await _sut.Close(lr.Id);

        await expected.Should().ThrowExactlyAsync<LoanRequestMustBeActiveException>();
    }

    [Fact]
    public async Task Close_not_close_loan_request_when_loan_request_have_unpaid_installment()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr = new LoanRequestBuilder()
            .WithCustomerId(c1.Id).WithStatus(LoanRequestStatus.Active).Build();
        Save(lr);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr).Build();
        Save(i1);

        await _sut.Close(lr.Id);

        var actual = ReadContext.Set<LoanRequest>().Single();
        actual.Status.Should().Be(LoanRequestStatus.Active);
    }

    [Fact]
    public async Task Close_close_loan_request_when_loan_request_installments_are_paid()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr = new LoanRequestBuilder()
            .WithCustomerId(c1.Id).WithStatus(LoanRequestStatus.Active).Build();
        Save(lr);
        var i1 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2024, 1, 1)).WithLoanRequest(lr).Build();
        Save(i1);

        await _sut.Close(lr.Id);

        var actual = ReadContext.Set<LoanRequest>().Single();
        actual.Status.Should().Be(LoanRequestStatus.Close);
    }
}