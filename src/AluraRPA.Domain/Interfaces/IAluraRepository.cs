
namespace AluraRPA.Domain.Interfaces;
public interface IAluraRepository
{
    //INSERTs
    Task<bool> InsertData(DataExtracted data, CancellationToken ct = default);
}