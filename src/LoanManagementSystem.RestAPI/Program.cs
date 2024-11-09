using LoanManagementSystem.Application.Installments.Handlers.PayInstallments;
using LoanManagementSystem.Application.Installments.Handlers.PayInstallments.Contracts;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans;
using LoanManagementSystem.Application.LoanRequests.Handlers.PayLoans.Contracts;
using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Persistence.Ef.LoanRequests;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Customers;
using LoanManagementSystem.Services.Customers.Contracts.Interfaces;
using LoanManagementSystem.Services.Installments.Contracts.Interfaces;
using LoanManagementSystem.Services.LoanRequests;
using LoanManagementSystem.Services.LoanRequests.Contracts.Interfaces;
using LoanManagementSystem.Services.Loans;
using LoanManagementSystem.Services.Loans.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CustomerService, CustomerAppService>();
builder.Services.AddScoped<CustomerQuery, EfCustomerQuery>();
builder.Services.AddScoped<LoanService, LoanAppService>();
builder.Services.AddScoped<LoanQuery, EfLoanQuery>();
builder.Services.AddScoped<InstallmentQuery, EfInstallmentQuery>();
builder.Services.AddScoped<PayInstallmentHandler, PayInstallmentCommandHandler>();
builder.Services.AddScoped<LoanRequestService, LoanRequestAppService>();
builder.Services.AddScoped<LoanRequestQuery, EfLoanRequestQuery>();
builder.Services.AddScoped<PayLoanHandler, PayLoanCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();