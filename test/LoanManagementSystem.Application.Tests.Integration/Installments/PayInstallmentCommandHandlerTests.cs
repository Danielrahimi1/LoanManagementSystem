using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts.DTOs;
using LoanManagementSystem.Entities.LoanRequests.Enums;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;
using LoanManagementSystem.TestTools.Application.Installments;
using LoanManagementSystem.TestTools.Services.Customers;
using LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Services.Installments;
using LoanManagementSystem.TestTools.Services.LoanRequests;
using LoanManagementSystem.TestTools.Services.Loans;
using Moq;
using DateOnly = System.DateOnly;

namespace LoanManagementSystem.Application.Tests.Integration.Installments;

public class PayInstallmentCommandHandlerTests : BusinessIntegrationTest
{
    private readonly PayInstallmentHandler _sut;
    private readonly Mock<CustomerService> _customerService;
    private readonly Mock<LoanRequestService> _loanRequestService;
    private readonly Mock<InstallmentService> _installmentService;

    public PayInstallmentCommandHandlerTests()
    {
        _customerService = new Mock<CustomerService>();
        _loanRequestService = new Mock<LoanRequestService>();
        _installmentService = new Mock<InstallmentService>();
        Mock<UnitOfWork> unitOfWork = new();
        _sut = PayInstallmentHandlerFactory.CreateHandler(SetupContext,
            _customerService.Object, _loanRequestService.Object, _installmentService.Object, unitOfWork.Object);
    }
    
    [Fact]
    public async Task PayLoan_add_loan_amount_to_customer_balance_when_loan_requests_activated()
    {
        var customer = new CustomerBuilder().WithBalance(10).Build();
        Save(customer);
        var loan = new LoanBuilder().WithAmount(100).Build();
        Save(loan);
        var lr = new LoanRequestBuilder()
            .WithLoanId(loan.Id).WithCustomerId(customer.Id)
            .WithStatus(LoanRequestStatus.Accept).WithRate(80)
            .Build();
        Save(lr);
        var i1 = new InstallmentBuilder()
            .WithPaymentDeadLine(new DateOnly(2025, 01, 01))
            .WithLoanRequest(lr).WithAmount(20).Build();
        Save(i1);
        var cmd = new PayInstallmentCommand
        {
            CustomerId = customer.Id,
            LoanRequestId = lr.Id
        };
        _installmentService.Setup(s => s.Pay(It.IsAny<int>())).ReturnsAsync(It.IsAny<decimal>());
        
        await _sut.Handle(cmd);
        
        _installmentService.Verify(s => s.Pay(It.IsAny<int>()));
        _customerService.Verify(s => s.Charge(It.IsAny<int>(), It.Is<UpdateBalanceDto>(dto =>
            dto.Charge == It.IsAny<decimal>())));
        _loanRequestService.Verify(s => s.Close(It.IsAny<int>()));
    }
}