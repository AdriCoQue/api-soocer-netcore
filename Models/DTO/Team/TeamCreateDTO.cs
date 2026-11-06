namespace api_soccer.Models.DTO.Team
{
    public class TeamCreateDTO
    {
        public string name { get; set; }
        public string country { get; set; }
        public int playersQuantity { get; set; }
    }
}
