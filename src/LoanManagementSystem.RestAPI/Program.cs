using LoanManagementSystem.Application.Installments.Handlers.PayInstallments;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Customers;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using LoanManagementSystem.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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