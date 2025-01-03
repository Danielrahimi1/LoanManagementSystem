using LoanManagementSystem.Persistence.Ef;
using Xunit;

namespace LoanManagementSystem.TestTools.Services.Infrastructure.DataBaseConfig.Integration.
    Fixtures;

[Collection(nameof(ConfigurationFixture))]
public class EFDataContextDatabaseFixture : DatabaseFixture
{
    public static EfDataContext CreateDataContext(string tenantId)
    {
        var connectionString =
            new ConfigurationFixture().Value.ConnectionString;


        return new EfDataContext(
            $"server=.;database=LMS;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");
    }
}