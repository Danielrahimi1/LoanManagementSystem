using LoanManagementSystem.Entities.Statements;
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
        builder.Property(statement => statement.NetWorth).HasColumnType("money");
    }
}