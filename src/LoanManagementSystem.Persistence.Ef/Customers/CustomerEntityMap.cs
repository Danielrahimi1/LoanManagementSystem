using LoanManagementSystem.Entities.Customers;
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
        builder.Property(customer => customer.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(customer => customer.LastName).HasMaxLength(50).IsRequired();
        builder.Property(customer => customer.NationalId).HasMaxLength(10).IsRequired();
        builder.Property(customer => customer.PhoneNumber).HasMaxLength(11).IsRequired();
        builder.Property(customer => customer.Email).HasMaxLength(320).IsRequired();
        builder.Property(customer => customer.Balance).HasColumnType("decimal").HasPrecision(19, 4).IsRequired(false);
        builder.Property(customer => customer.JobType).IsRequired(false);
        builder.Property(customer => customer.IncomeGroup).IsRequired(false);
        builder.Property(customer => customer.NetWorth).IsRequired(false);

        builder.HasMany(customer => customer.LoanRequests)
            .WithOne(loanRequest => loanRequest.Customer)
            .HasForeignKey(lr => lr.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}