using LoanManagementSystem.Entities.Statements;
using LoanManagementSystem.Entities.Statements.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Statements;

public class StatementEntityMap : IEntityTypeConfiguration<Statement>
{
    public void Configure(EntityTypeBuilder<Statement> builder)
    {
        builder.ToTable("Statements");
        builder.HasKey(statement => statement.Id);
        builder.Property(statement => statement.Id).UseIdentityColumn();
        builder.Property(statement => statement.JobType).HasConversion<int>();
        builder.Property(statement => statement.IncomeGroup).HasConversion<int>();
        builder.Property(statement => statement.NetWorth).HasColumnType("money");
        builder.HasOne(f => f.Customer)
            .WithOne(c => c.Statement)
            .HasForeignKey<Statement>(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}