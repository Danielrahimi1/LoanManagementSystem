using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Installments;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;

namespace LoanManagementSystem.TestTools.Services.Installments;

public static class InstallmentServiceFactory
{
    public static InstallmentService CreateService(EfDataContext context)
    {
        var installmentRepository = new EfInstallmentRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new InstallmentAppService(installmentRepository, unitOfWork);
    }
}