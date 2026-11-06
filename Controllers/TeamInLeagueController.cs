using api_soccer.Models;
using api_soccer.Models.DTO.TeamInLeague;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_soccer.Controllers
{
    [Route("api/[controller]")]
    public class TeamInLeagueController : ControllerBase
    {
        private readonly SoccerContext _context;

        public TeamInLeagueController(SoccerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamInLeagueDTO>>> GetAllTeamsInLeagues()
        {
            return await _context.registers
                .Include(f => f.team)
                .Include(f => f.league)
                .Where(f => f.enabled==true)
                .Where(f => f.league.enabled == true)
                .Where(f => f.team.enabled == true)
                .Select(f => new TeamInLeagueDTO
                {
                    idleague = f.idleague,
                    leagueName = f.league.name,
                    idteam = f.idteam,
                    teamName = f.team.name
                }).OrderBy(f => f.leagueName)
                .ToListAsync();
        }

        [HttpGet("league/{id}")]
        public async Task<ActionResult<IEnumerable<TeamInLeagueDTO>>> GetTeamsInLeague(int id)
        {
            var teamsInLeague = await _context.registers
                .Include(f => f.team)
                .Include(f => f.league)
                .Where(f => f.league.enabled == true)
                .Where(f => f.team.enabled == true)
                .Where(f => f.idleague == id)
                .Select(f => new TeamInLeagueDTO
                {
                    idleague = f.idleague,
                    leagueName = f.league.name,
                    idteam = f.idteam,
                    teamName = f.team.name
                })
                .ToListAsync();

            if(teamsInLeague == null || teamsInLeague.Count == 0)
            {
                return NotFound();
            }
            return teamsInLeague;
        }

    [HttpGet("team/{id}")]
        public async Task<ActionResult<IEnumerable<TeamInLeagueDTO>>> GetLeaguesOfTeam(int id)
        {
            var teamsInLeague = await _context.registers
                .Where(f => f.enabled == true)
                .Include(f => f.team)
                .Include(f => f.league)
                .Where(f => f.league.enabled == true)
                .Where(f => f.team.enabled == true)
                .Where(f => f.idteam == id)
                .Select(f => new TeamInLeagueDTO
                {
                    idleague = f.idleague,
                    leagueName = f.league.name,
                    idteam = f.idteam,
                    teamName = f.team.name
                })
                .ToListAsync();

            if (teamsInLeague == null || teamsInLeague.Count == 0)
            {
                return NotFound();
            }
            return teamsInLeague;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterTeam([FromBody] RegisterCreateDTO register)
        {
            var existingLeague = await _context.leagues.FindAsync(register.idleague);
            var existingTeam = await _context.teams.FindAsync(register.idteam);

            if (existingLeague == null || existingTeam == null)
            {
                return NotFound();
            }

            var newRegister = new TeamInLeague
            {
                idleague = existingLeague.idleague,
                idteam = existingTeam.idteam,
                enabled = true
            };

            _context.registers.Add(newRegister);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("disable/{idleague}/{idteam}")]
        public async Task<ActionResult> DisableTeamRegister(int idleague, int idteam)
        {
            var register = await _context.registers.FindAsync(idleague, idleague);
            if (register == null)
            {
                return NotFound();
            }
            register.idleague = register.idleague;
            register.idteam = register.idteam;
            register.enabled = false;

            _context.Entry(register).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisterExists(idleague, idteam))
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

        [HttpDelete("{idleague}/{idteam}")]
        public async Task<ActionResult> DeleteRegister(int idleague, int idteam)
        {
            var register = await _context.registers.FindAsync(idleague, idteam);
            if (register == null)
            {
                return NotFound();
            }
            _context.registers.Remove(register);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RegisterExists(int idleague, int idteam)
        {
            return _context.registers.Any(e => e.idleague == idleague && e.idteam == idteam);
        }
    }
}
