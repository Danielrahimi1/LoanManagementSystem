using FluentAssertions;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Services.Installments.Contracts.DTOs;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanRequests;

namespace LoanManagementSystem.Services.Tests.Unit.Installments;

public class InstallmentQueryTests : BusinessIntegrationTest
{
    private readonly InstallmentQuery _sut;

    public InstallmentQueryTests() =>
        _sut = new EfInstallmentQuery(SetupContext);

    [Fact]
    public async Task GetById_return_installment_dto_when_given_id()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var lr = new LoanRequestBuilder().WithCustomerId(customer.Id).Build();
        Save(lr);
        var i1 = new InstallmentBuilder().WithFine(50).WithLoanRequest(lr).Build();
        var i2 = new InstallmentBuilder().WithLoanRequest(lr).Build();
        Save(i1, i2);

        var actual = await _sut.GetById(i1.Id);

        actual.Should().BeEquivalentTo(new
        {
            LoanRequestId = i1.LoanRequestId,
            Amount = i1.Amount,
            MonthlyInterest = i1.MonthlyInterest,
            PaymentDeadLine = i1.PaymentDeadLine,
            PaymentDate = i1.PaymentDate,
            Fine = i1.Fine
        });
    }

    [Fact]
    public async Task GetById_return_empty_installment_dto_when_given_id_is_invalid()
    {
        var actual = await _sut.GetById(-1);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_return_all_when_invoked()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder().WithLoanRequest(lr1).Build();
        Save(i1, i2);

        var actual = await _sut.GetAll();

        actual.Should().BeEquivalentTo([
            new 
            {
                LoanRequestId = lr1.Id,
                Amount = i1.Amount,
                MonthlyInterest = i1.MonthlyInterest,
                PaymentDeadLine = i1.PaymentDeadLine,
                PaymentDate = i1.PaymentDate,
                Fine = i1.Fine
            },
            new 
            {
                LoanRequestId = lr1.Id,
                Amount = i2.Amount,
                MonthlyInterest = i2.MonthlyInterest,
                PaymentDeadLine = i2.PaymentDeadLine,
                PaymentDate = i2.PaymentDate,
                Fine = i2.Fine
            }
        ]);
    }
}