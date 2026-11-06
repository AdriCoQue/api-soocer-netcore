using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api_soccer.Models
{
    [PrimaryKey(nameof(idleague), nameof(idteam))]
    public class TeamInLeague
    {
        public int idleague { get; set; }
        public int idteam { get; set; }
        public bool enabled { get; set; }
        [JsonIgnore]
        public League league { get; set; }
        [JsonIgnore]
        public Team team { get; set; }
    }
}
