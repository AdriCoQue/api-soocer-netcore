namespace api_soccer.Models.DTO.League
{
    public class LeagueDTO
    {
        public int idleague { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int teamsRegistered { get; set; }
    }
}
