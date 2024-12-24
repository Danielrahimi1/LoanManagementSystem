using Autofac;
using Autofac.Extensions.DependencyInjection;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Contracts.Interfaces;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var applicationAssembly = typeof(PayInstallmentHandler).Assembly;
var serviceAssembly = typeof(CustomerService).Assembly;
var repositoryAssembly = typeof(EfLoanRepository).Assembly;


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    b.RegisterAssemblyTypes(applicationAssembly, serviceAssembly)
        .AssignableTo<Service>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
    
    b.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
        .AssignableTo<Repository>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
    
    b.RegisterAssemblyTypes(repositoryAssembly, serviceAssembly)
        .AssignableTo<IScope>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
});

/*
builder.Services.AddScoped<UnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<LoanRepository, EfLoanRepository>();
builder.Services.AddScoped<CustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<LoanRequestRepository, EfLoanRequestRepository>();
builder.Services.AddScoped<InstallmentRepository, EfInstallmentRepository>();

builder.Services.AddScoped<LoanQuery, EfLoanQuery>();
builder.Services.AddScoped<CustomerQuery, EfCustomerQuery>();
builder.Services.AddScoped<LoanRequestQuery, EfLoanRequestQuery>();
builder.Services.AddScoped<InstallmentQuery, EfInstallmentQuery>();

builder.Services.AddScoped<LoanService, LoanAppService>();
builder.Services.AddScoped<CustomerService, CustomerAppService>();
builder.Services.AddScoped<LoanRequestService, LoanRequestAppService>();
builder.Services.AddScoped<InstallmentService, InstallmentAppService>();
builder.Services.AddScoped<PayLoanHandler, PayLoanCommandHandler>();
builder.Services.AddScoped<PayInstallmentHandler, PayInstallmentCommandHandler>();
*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();