using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _service;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //  GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(data));
        }

        //  GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<SponsorResponseDTO>(entity));
        }

        //  CREATE
        [HttpPost]
        public async Task<IActionResult> Create(SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);
                var created = await _service.CreateAsync(entity);

                return StatusCode(201, _mapper.Map<SponsorResponseDTO>(created));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        //  UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SponsorRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Sponsor>(dto);
                await _service.UpdateAsync(id, entity);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //  DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //  LINK SPONSOR TO TOURNAMENT
        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> Link(int id, TournamentSponsorRequestDTO dto)
        {
            try
            {
                await _service.RegisterToTournamentAsync(id, dto.TournamentId, dto.ContractAmount);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        //  GET TOURNAMENTS BY SPONSOR
        [HttpGet("{id}/tournaments")]
        public async Task<IActionResult> GetTournaments(int id)
        {
            var data = await _service.GetTournaments(id);
            return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(data));
        }

        //  REMOVE LINK
        [HttpDelete("{id}/tournaments/{tournamentId}")]
        public async Task<IActionResult> Remove(int id, int tournamentId)
        {
            try
            {
                await _service.Remove(id, tournamentId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
