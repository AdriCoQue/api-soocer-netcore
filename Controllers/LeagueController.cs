using api_soccer.Models;
using api_soccer.Models.DTO.League;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_soccer.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : ControllerBase
    {
        private readonly SoccerContext _context;

        public LeagueController(SoccerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeagues()
        {
            return await _context.leagues
                .Include(f => f.teamsInLeague)
                .Where(f => f.enabled == true)
                .Select(f => new LeagueDTO
                {
                    idleague = f.idleague,
                    name = f.name,
                    country = f.country,
                    startDate = f.startDate,
                    endDate = f.endDate,
                    teamsRegistered = f.teamsInLeague.Count
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueDTO>> GetLeague(int id)
        {
            var league = await _context.leagues
                .Include(f => f.teamsInLeague)
                .Where(f => f.enabled == true)
                .Select(f => new LeagueDTO
                {
                    idleague = f.idleague,
                    name = f.name,
                    country = f.country,
                    startDate = f.startDate,
                    endDate= f.endDate,
                    teamsRegistered = f.teamsInLeague.Count
                })
                .FirstOrDefaultAsync(f => f.idleague == id);

            if(league == null)
            {
                return NotFound();
            }
            return league;
        }

        [HttpPost]
        public async Task<ActionResult<League>> CreateLeague([FromBody] LeagueCreateDTO newLeague)
        {
            if(newLeague.startDate > newLeague.endDate)
            {
                return BadRequest("Start Date cannot be after End Date");
            }
            var league = new League
            {
                name = newLeague.name,
                country = newLeague.country,
                startDate = newLeague.startDate,
                endDate = newLeague.endDate,
                enabled = true
            };

            _context.leagues.Add(league);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLeague), new { id = league.idleague }, league);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLeague(int id, [FromBody] LeagueDTO updatedLeague)
        {
            var league = await _context.leagues.FindAsync(id);
            if (league == null) { return NotFound(); }
            league.name = updatedLeague.name;
            league.country = updatedLeague.country;
            league.startDate = updatedLeague.startDate;
            league.endDate = updatedLeague.endDate;
            league.enabled = true;

            _context.Entry(league).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("disable/{id}")]
        public async Task<ActionResult> DisableLeague(int id)
        {
            var league = await _context.leagues.FindAsync(id);
            if(league == null)
            {
                return NotFound();
            }
            league.name = league.name;
            league.country = league.country;
            league.startDate = league.startDate;
            league.endDate = league.endDate;
            league.enabled = false;

            _context.Entry(league).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLeague(int id)
        {
            var league = await _context.leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }
            _context.leagues.Remove(league);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LeagueExists(int id)
        {
            return _context.leagues.Any(e => e.idleague == id);
        }


    }
}
