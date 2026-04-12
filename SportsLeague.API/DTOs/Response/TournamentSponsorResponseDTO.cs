namespace SportsLeague.API.DTOs.Response
{
    public class TournamentSponsorResponseDTO
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public required string TournamentName { get; set; }

        public int SponsorId { get; set; }
        public  required string SponsorName { get; set; }

        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
