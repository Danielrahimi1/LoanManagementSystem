using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Statements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class CustomerEntityMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.Id).UseIdentityColumn();
        builder.Property(customer => customer.FirstName).HasMaxLength(50);
        builder.Property(customer => customer.LastName).HasMaxLength(50);
        builder.Property(customer => customer.NationalId).HasMaxLength(10);
        builder.Property(customer => customer.PhoneNumber).HasMaxLength(11);
        builder.Property(customer => customer.Email).HasMaxLength(320);
        builder.Property(customer => customer.Balance).HasColumnType("money");
        builder.HasOne(customer => customer.Statement)
            .WithOne(statement => statement.Customer)
            .HasForeignKey<Statement>(statement => statement.CustomerId).IsRequired(false);
        builder.HasMany(customer => customer.LoanRequests)
            .WithOne(loanRequest => loanRequest.Customer)
            .HasForeignKey(lr => lr.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}