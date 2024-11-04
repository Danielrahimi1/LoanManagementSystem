using System.Data;
using FluentMigrator;

namespace Migration.Migrations;

[Migration(2)]
public class AddStatementsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Statements")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("JobType").AsInt32().NotNullable()
            .WithColumn("IncomeGroup").AsInt32().NotNullable()
            .WithColumn("NetWorth").AsDecimal().NotNullable()
            .WithColumn("CustomerId").AsInt32().NotNullable()
                .ForeignKey("Customers","Id")
                .OnDelete(Rule.Cascade).Unique();
    }

    public override void Down()
    {
        Delete.Table("Statements");
    }
}