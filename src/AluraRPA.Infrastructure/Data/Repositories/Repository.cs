namespace AluraRPA.Infrastructure.Data.Repositories;
public abstract class Repository
{
    protected string ConnectionString { get; }

    protected Repository(string connectionString)
        => ConnectionString = connectionString;

    protected Repository(IConfiguration configuration)
        => ConnectionString = configuration.GetConnectionString("Default");
}