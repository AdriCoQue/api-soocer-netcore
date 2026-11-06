using api_soccer.Models;
using api_soccer.Models.DTO.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_soccer.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly SoccerContext _context;

        public TeamController(SoccerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            return await _context.teams
                .Where(f => f.enabled == true)
                .Select(f => new TeamDTO
                {
                    idteam = f.idteam,
                    name = f.name,
                    country = f.country,
                    playersQuantity = f.playersQuantity
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var team = await _context.teams
                .Include(f => f.teamsInLeague)
                .Where(f => f.enabled == true)
                .Select(f => new TeamDTO
                {
                    idteam = f.idteam,
                    name = f.name,
                    country = f.country,
                    playersQuantity = f.playersQuantity
                })
                .FirstOrDefaultAsync(f => f.idteam == id);

            if (team == null)
            {
                return NotFound();
            }
            return team;
        }

        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] TeamCreateDTO newTeam)
        {
            var team = new Team
            {
                name = newTeam.name,
                country = newTeam.country,
                playersQuantity = newTeam.playersQuantity,
                enabled = true
            };

            _context.teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeam), new { id = team.idteam}, team);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(int id, [FromBody] TeamDTO updatedTeam)
        {
            var team = await _context.teams.FindAsync(id);
            if (team == null) { return NotFound(); }
            team.name = updatedTeam.name;
            team.country = updatedTeam.country;
            team.playersQuantity=updatedTeam.playersQuantity;
            team.enabled = true;

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        [HttpDelete("disable/{id}")]
        public async Task<ActionResult> DisableTeam(int id)
        {
            var team = await _context.teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            team.name = team.name;
            team.country = team.country;
            team.playersQuantity = team.playersQuantity;
            team.enabled = false;

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var team = await _context.teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            _context.teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.teams.Any(e => e.idteam == id);
        }


    }
}
