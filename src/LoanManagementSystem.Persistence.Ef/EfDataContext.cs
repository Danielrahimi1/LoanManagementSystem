using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.Loans;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef;

public class EfDataContext(DbContextOptions<EfDataContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Loan> Loans { get; init; }
    public DbSet<LoanRequest> LoanRequests { get; init; }
    public DbSet<Installment> Installments { get; init; }

    public EfDataContext(
        string connectionString)
        : this(new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString).Options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Customer)
            .Assembly);
    }
}