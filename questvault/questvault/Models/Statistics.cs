namespace questvault.Models
{
    public class Statistics
    {
        public int registeredUsersCount { get; set; }
        //public int siteAccessCount { get; set; }
        public List<LoginInstance> loginList { get; set; }

    }
}
