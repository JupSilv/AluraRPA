
namespace AluraRPA.Domain.Interfaces;
public interface IAluraRepository
{
    Task<Credential> GetCredential(CancellationToken ct = default);

    //INSERTs
    Task<bool> InsertData(List<DataExtracted> data, CancellationToken ct = default);
}