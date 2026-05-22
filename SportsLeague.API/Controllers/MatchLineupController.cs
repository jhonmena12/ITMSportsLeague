using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.API.DTOs.Response;
using SportsLeague.API.DTOs.Request;

namespace SportsLeague.API.Controllers
{
    [ApiController]

    [Route("api/match/{matchId}/lineup")]

    public class MatchLineupController : ControllerBase

    {

        private readonly IMatchLineupService _matchLineupService;

        private readonly IMapper _mapper;



        public MatchLineupController(

            IMatchLineupService matchLineupService, IMapper mapper)

        {

            _matchLineupService = matchLineupService;

            _mapper = mapper;

        }



        // POST api/match/{matchId}/lineup

        [HttpPost]

        public async Task<ActionResult<MatchLineupDTO>> AddPlayerToLineup(

            int matchId, CreateMatchLineupDTO dto)

        {

            try

            {

                var lineup = _mapper.Map<MatchLineup>(dto);

                var created = await _matchLineupService.AddPlayerToLineupAsync(matchId, lineup);

                var lineupWithDetails = await _matchLineupService.GetLineupByMatchAsync(matchId);

                var createdEntry = lineupWithDetails.FirstOrDefault(ml => ml.Id == created.Id);

                return CreatedAtAction(

                    nameof(GetLineup),

                    new { matchId },

                    _mapper.Map<MatchLineupDTO>(createdEntry));

            }

            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }

        }



        // GET api/match/{matchId}/lineup

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetLineup(int matchId)

        {

            try

            {

                var lineup = await _matchLineupService.GetLineupByMatchAsync(matchId);

                return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));

            }

            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        }



        // GET api/match/{matchId}/lineup/team/{teamId}

        [HttpGet("team/{teamId}")]

        public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetLineupByTeam(

            int matchId, int teamId)

        {

            try

            {

                var lineup = await _matchLineupService

                    .GetLineupByMatchAndTeamAsync(matchId, teamId);

                return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));

            }

            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        }



        // DELETE api/match/{matchId}/lineup/{id}

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteLineupEntry(int matchId, int id)

        {

            try

            {

                await _matchLineupService.DeleteLineupEntryAsync(matchId, id);

                return NoContent();

            }

            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        }

    }

}
