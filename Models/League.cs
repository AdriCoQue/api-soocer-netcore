using System.ComponentModel.DataAnnotations;

namespace api_soccer.Models
{
    public class League
    {
        [Key]
        public int idleague { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool enabled { get; set; }
        public ICollection<TeamInLeague> teamsInLeague { get; set; }
    }
}
