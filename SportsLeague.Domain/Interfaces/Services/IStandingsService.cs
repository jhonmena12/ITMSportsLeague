namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IStandingsService

    {
        //keyword o palabra reseverda  "objeto"
        // se utiliza para indicar que el método puede devolver 
        // cualquier tipo de dato. en este caso, se espera que el 
        // metodo devuelve un objeto que contenga la información de la tabla de posiciones,
        // la tabla de posiciones. los maximos goleadores o las estadistica de tarjeta 
        // dependiendo del metodo especifico 

        Task<object> GetStandingsAsync(int tournamentId); // obtener la tabla d eposiciones de un torneo especial 

        Task<object> GetTopScorersAsync(int tournamentId); // obtener la lista de lso maximos goleadores de un torneo especial

        Task<object> GetCardStatsAsync(int tournamentId); // obtener la lista de jugadores con mas tarjetas amarillas y rojas en un torneo especial

    }
}
