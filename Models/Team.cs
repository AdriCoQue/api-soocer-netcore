using System.ComponentModel.DataAnnotations;

namespace api_soccer.Models
{
    public class Team
    {
        [Key]
        public int idteam { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int playersQuantity { get; set; }
        public bool enabled { get; set;  }
        public ICollection<TeamInLeague> teamsInLeague { get; set; }
    }
}
