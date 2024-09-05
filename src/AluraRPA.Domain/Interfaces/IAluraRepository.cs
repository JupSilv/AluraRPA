namespace AluraRPA.Domain.Interfaces;
public interface IAluraRepository
{
    Task<Credential> GetCredentialAsync(Guid key, CancellationToken ct = default);
    Task<Demanda> GetDemandaAsync(CancellationToken ct = default);
    Task UpdateDemandaAsync(Guid idDemanda, CancellationToken ct = default);
}