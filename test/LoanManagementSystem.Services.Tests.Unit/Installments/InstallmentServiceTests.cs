using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;

namespace LoanManagementSystem.Services.Tests.Unit.Installments;

public class InstallmentServiceTests : BusinessIntegrationTest
{
    private readonly InstallmentService _sut;

    public InstallmentServiceTests() =>
        _sut = InstallmentServiceFactory.CreateService(SetupContext);
}