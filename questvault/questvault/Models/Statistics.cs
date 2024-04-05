using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace questvault.Models
{
    public class Statistics
    {
      public int registeredUsersCount { get; set; }
      public List<string> dateList { get; set; }
      public List<int> dateCountList { get; set; }
    }
}
