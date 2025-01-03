using FluentMigrator;

namespace Migration.Migrations;

[Migration(1)]
public class AddCustomersTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Customers")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("LastName").AsString(50).NotNullable()
            .WithColumn("NationalId").AsString(10).NotNullable()
            .WithColumn("PhoneNumber").AsString(11).NotNullable()
            .WithColumn("Email").AsString(320).NotNullable()
            .WithColumn("Balance").AsDecimal(19,6).NotNullable()
            .WithColumn("IsVerified").AsBoolean().NotNullable()
            .WithColumn("JobType").AsInt32().NotNullable()
            .WithColumn("IncomeGroup").AsInt32().NotNullable()
            .WithColumn("NetWorth").AsDecimal(19,6).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Customers");
    }
}