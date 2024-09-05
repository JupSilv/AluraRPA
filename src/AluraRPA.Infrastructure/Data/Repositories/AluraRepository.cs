using System.Data;
using System.Data.SqlClient;

namespace AluraRPA.Infrastructure.Data.Repositories;
public class AluraRepository : Repository,IAluraRepository
{
    public AluraRepository(IConfiguration configuration) : base(configuration) { }
    public async Task<Credential> GetCredentialAsync(Guid key, CancellationToken ct = default)
    {
        var sql = $@"SELECT * FROM [dbo].[CREDENTIAL] @key";

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@key", SqlDbType.VarChar).Value = key.ToString();

        await connection.OpenAsync(ct);
        using var reader = await command.ExecuteReaderAsync(ct);

        if (reader.Read())
            return new
            (
                Url: reader[0].ToString(),
                User: reader[1].ToString(),
                Password: reader[2].ToString()
            );
        return null;
    }

    public Task<Demanda> GetDemandaAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDemandaAsync(Guid idDemanda, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}