using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        Task<TournamentSponsor?> GetByIdsAsync(int tournamentId, int sponsorId);
        Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId);
    }
}
