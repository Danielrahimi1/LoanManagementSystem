using FluentAssertions;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;
using LoanManagementSystem.TestTools.Services.Loans;

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
            i1.LoanRequestId,
            i1.Amount,
            i1.MonthlyInterest,
            i1.PaymentDeadLine,
            i1.PaymentDate,
            i1.Fine
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
                i1.Amount,
                i1.MonthlyInterest,
                i1.PaymentDeadLine,
                i1.PaymentDate,
                i1.Fine
            },
            new
            {
                LoanRequestId = lr1.Id,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllByCustomer_return_all_customer_installments_when_given_customer_id()
    {
        var c1 = new CustomerBuilder().Build();
        var c2 = new CustomerBuilder().Build();
        Save(c1, c2);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        var lr2 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        var lr3 = new LoanRequestBuilder().WithCustomerId(c2.Id).Build();
        Save(lr1, lr2, lr3);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder().WithLoanRequest(lr2).Build();
        var i3 = new InstallmentBuilder().WithLoanRequest(lr3).Build();
        Save(i1, i2, i3);

        var actual = await _sut.GetAllByCustomer(c1.Id);

        actual.Should().BeEquivalentTo([
            new
            {
                i1.LoanRequestId,
                i1.Amount,
                i1.MonthlyInterest,
                i1.PaymentDeadLine,
                i1.PaymentDate,
                i1.Fine
            },
            new
            {
                i2.LoanRequestId,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllByLoan_return_all_installments_filtered_by_loan_when_given_loan_id()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var loan1 = new LoanBuilder().Build();
        var loan2 = new LoanBuilder().Build();
        Save(loan1, loan2);
        var lr1 = new LoanRequestBuilder().WithLoanId(loan1.Id).WithCustomerId(c1.Id).Build();
        var lr2 = new LoanRequestBuilder().WithLoanId(loan2.Id).WithCustomerId(c1.Id).Build();
        Save(lr1, lr2);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder().WithLoanRequest(lr2).Build();
        var i3 = new InstallmentBuilder().WithLoanRequest(lr2).Build();
        Save(i1, i2, i3);

        var actual = await _sut.GetAllByLoan(loan2.Id);

        actual.Should().BeEquivalentTo([
            new
            {
                i2.LoanRequestId,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            },
            new
            {
                i3.LoanRequestId,
                i3.Amount,
                i3.MonthlyInterest,
                i3.PaymentDeadLine,
                i3.PaymentDate,
                i3.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllByLoanRequest_return_all_installments_filtered_by_loan_request_when_given_loan_request_id()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var loan1 = new LoanBuilder().Build();
        var loan2 = new LoanBuilder().Build();
        Save(loan1, loan2);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        var lr2 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1, lr2);
        var i1 = new InstallmentBuilder().WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder().WithLoanRequest(lr2).Build();
        var i3 = new InstallmentBuilder().WithLoanRequest(lr2).Build();
        Save(i1, i2, i3);

        var actual = await _sut.GetAllByLoanRequest(lr2.Id);

        actual.Should().BeEquivalentTo([
            new
            {
                i2.LoanRequestId,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            },
            new
            {
                i3.LoanRequestId,
                i3.Amount,
                i3.MonthlyInterest,
                i3.PaymentDeadLine,
                i3.PaymentDate,
                i3.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllByDelayed_return_all_delayed_installments_when_invoked()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2020, 12, 2))
            .WithPaymentDeadLine(new DateOnly(2020, 12, 15))
            .WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2023, 5, 16))
            .WithPaymentDeadLine(new DateOnly(2023, 4, 1))
            .WithLoanRequest(lr1).Build();
        var i3 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2024, 11, 21))
            .WithPaymentDeadLine(new DateOnly(2024, 11, 10))
            .WithLoanRequest(lr1).Build();
        Save(i1, i2, i3);

        var actual = await _sut.GetAllDelayed();

        actual.Should().BeEquivalentTo([
            new
            {
                i2.LoanRequestId,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            },
            new
            {
                i3.LoanRequestId,
                i3.Amount,
                i3.MonthlyInterest,
                i3.PaymentDeadLine,
                i3.PaymentDate,
                i3.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllByClosed_return_all_installments_belong_to_closed_requests_when_invoked()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Close).WithCustomerId(c1.Id).Build();
        var lr2 = new LoanRequestBuilder().WithStatus(LoanRequestStatus.Active).WithCustomerId(c1.Id).Build();
        Save(lr1, lr2);
        var i1 = new InstallmentBuilder()
            .WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder()
            .WithLoanRequest(lr1).Build();
        var i3 = new InstallmentBuilder()
            .WithLoanRequest(lr1).Build();
        var i4 = new InstallmentBuilder()
            .WithLoanRequest(lr2).Build();
        Save(i1, i2, i3, i4);

        var actual = await _sut.GetAllClosed();

        actual.Should().BeEquivalentTo([
            new
            {
                i1.LoanRequestId,
                i1.Amount,
                i1.MonthlyInterest,
                i1.PaymentDeadLine,
                i1.PaymentDate,
                i1.Fine
            },
            new
            {
                i2.LoanRequestId,
                i2.Amount,
                i2.MonthlyInterest,
                i2.PaymentDeadLine,
                i2.PaymentDate,
                i2.Fine
            },
            new
            {
                i3.LoanRequestId,
                i3.Amount,
                i3.MonthlyInterest,
                i3.PaymentDeadLine,
                i3.PaymentDate,
                i3.Fine
            }
        ]);
    }

    [Fact]
    public async Task GetAllIncome_return_income_per_month_when_invoked()
    {
        var c1 = new CustomerBuilder().Build();
        Save(c1);
        var lr1 = new LoanRequestBuilder().WithCustomerId(c1.Id).Build();
        Save(lr1);
        var i1 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2024, 1, 15))
            .WithFine(2)
            .WithMonthlyInterest(2)
            .WithLoanRequest(lr1).Build();
        var i2 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2024, 1, 15))
            .WithFine(2)
            .WithMonthlyInterest(2)
            .WithLoanRequest(lr1).Build();
        var i3 = new InstallmentBuilder()
            .WithPaymentDate(new DateOnly(2024, 2, 15))
            .WithFine(2)
            .WithMonthlyInterest(2)
            .WithLoanRequest(lr1).Build();
        Save(i1, i2, i3);

        var actual = await _sut.GetAllIncome();

        actual.Should().BeEquivalentTo([
            new
            {
                Interest = i1.MonthlyInterest + i2.MonthlyInterest,
                Fine = i1.Fine + i2.Fine
            },
            new
            {
                Interest = i3.MonthlyInterest,
                i3.Fine
            }
        ]);
    }
}