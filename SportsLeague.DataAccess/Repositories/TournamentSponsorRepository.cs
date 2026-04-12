using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context) { }

        public async Task<TournamentSponsor?> GetByIdsAsync(int tournamentId, int sponsorId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.TournamentId == tournamentId && x.SponsorId == sponsorId);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId)
        {
            return await _dbSet
                .Where(x => x.SponsorId == sponsorId)
                .Include(x => x.Tournament)
                .ToListAsync();
        }
    }
}
