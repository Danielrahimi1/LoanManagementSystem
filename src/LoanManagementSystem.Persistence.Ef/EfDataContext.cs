using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanRequests;
using LoanManagementSystem.Entities.Loans;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef;

public class EfDataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<LoanRequest> LoanRequests { get; set; }
    public DbSet<Installment> Installments { get; set; }

    public EfDataContext(
        string connectionString)
        : this(new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString).Options)
    {
    }

    public EfDataContext(
        DbContextOptions<EfDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Customer)
            .Assembly);
    }
}