using FluentAssertions;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Exceptions;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;

namespace LoanManagementSystem.Services.Tests.Unit.Installments;

public class InstallmentServiceTests : BusinessIntegrationTest
{
    private readonly InstallmentService _sut;

    public InstallmentServiceTests() =>
        _sut = InstallmentServiceFactory.CreateService(SetupContext);

    [Fact]
    public async Task Pay_throw_exception_when_loan_request_does_not_have_installment()
    {
        var expected = async () => await _sut.Pay(-1);

        await expected.Should().ThrowExactlyAsync<InstallmentNotFoundException>();
    }

    [Fact]
    public async Task Pay_set_payment_date_to_today_date_when_installment_not_null()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithPaymentDeadLine(new DateOnly(2060, 1, 1)).Build();
        Save(i1);

        await _sut.Pay(lr1.Id);

        var actual = ReadContext.Set<Installment>().Single();
        actual.PaymentDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public async Task Pay_find_installment_when_payment_deadline_is_less_than_payment_date()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100)
            .WithPaymentDeadLine(new DateOnly(2020, 1, 1)).Build();
        Save(i1);

        await _sut.Pay(lr1.Id);
        
        var actual = ReadContext.Set<Installment>().Single();
        actual.Fine.Should().Be(2);
    }

    [Fact]
    public async Task Pay_return_installment_cost_when_paid()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithLoanRequest(lr1)
            .WithAmount(100)
            .WithMonthlyInterest(2)
            .WithPaymentDeadLine(new DateOnly(2020, 1, 1)).Build();
        Save(i1);

        var actual = await _sut.Pay(lr1.Id);

        actual.Should().Be(100 + 2 + 2);
    }
}