using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);

        Task RegisterToTournamentAsync(int sponsorId, int tournamentId, decimal amount);
        Task<IEnumerable<TournamentSponsor>> GetTournaments(int sponsorId);
        Task Remove(int sponsorId, int tournamentId);
    }
}
