using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.Interfaces;

public interface IProviderRepository
{
    Task<Prestador?> GetByIdAsync(int id);
    Task AddAsync(Prestador prestador);
    Task AddSubscriptionAsync(SolicitantePrestador subscription);
    Task<bool> IsSubscribedAsync(int applicantId, int providerId);
}
