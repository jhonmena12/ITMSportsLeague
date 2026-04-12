using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _repo;
    private readonly ITournamentRepository _tournamentRepo;
    private readonly ITournamentSponsorRepository _tsRepo;

    public SponsorService(
        ISponsorRepository repo,
        ITournamentRepository tournamentRepo,
        ITournamentSponsorRepository tsRepo)
    {
        _repo = repo;
        _tournamentRepo = tournamentRepo;
        _tsRepo = tsRepo;
    }

    public async Task<IEnumerable<Sponsor>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<Sponsor?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        // 🔥 VALIDACIONES (MUY IMPORTANTES)
        if (await _repo.ExistsByNameAsync(sponsor.Name))
            throw new InvalidOperationException("El nombre ya existe");

        if (!sponsor.ContactEmail.Contains("@"))
            throw new InvalidOperationException("Email inválido");

        return await _repo.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Sponsor no existe");

        existing.Name = sponsor.Name;
        existing.ContactEmail = sponsor.ContactEmail;
        existing.Phone = sponsor.Phone;
        existing.WebsiteUrl = sponsor.WebsiteUrl;
        existing.Category = sponsor.Category;
        existing.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Sponsor no existe");

        await _repo.DeleteAsync(existing.Id);
    }

    public async Task RegisterToTournamentAsync(int sponsorId, int tournamentId, decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("El monto debe ser mayor a 0");

        var sponsor = await _repo.GetByIdAsync(sponsorId)
            ?? throw new KeyNotFoundException("Sponsor no existe");

        var tournament = await _tournamentRepo.GetByIdAsync(tournamentId)
            ?? throw new KeyNotFoundException("Tournament no existe");

        var exists = await _tsRepo.GetByIdsAsync(tournamentId, sponsorId);
        if (exists != null)
            throw new InvalidOperationException("El sponsor ya está registrado en este torneo");

        await _tsRepo.CreateAsync(new TournamentSponsor
        {
            SponsorId = sponsorId,
            TournamentId = tournamentId,
            ContractAmount = amount
        });
    }

    public async Task<IEnumerable<TournamentSponsor>> GetTournaments(int sponsorId)
        => await _tsRepo.GetBySponsorAsync(sponsorId);

    public async Task Remove(int sponsorId, int tournamentId)
    {
        var entity = await _tsRepo.GetByIdsAsync(tournamentId, sponsorId)
            ?? throw new KeyNotFoundException("Relación no existe");

        await _tsRepo.DeleteAsync(entity.Id);
    }
}
