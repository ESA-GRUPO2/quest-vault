using questvault.Utils;

namespace questvault.Models
{
    public class GameViewData
    {
        public string SearchTerm { get; set; } = "";
        public int NumberOfResults { get; set; }
        public PaginatedList<Game>? Games { get; set; }
        public IEnumerable<Genre>? Genres { get; set; }
        public IEnumerable<Platform>? Platforms  { get; set; }




    }
}
