using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace questvault.Models
{
    public class Statistics
    {
      public int registeredUsersCount { get; set; }
      public double gameRatingAverage { get; set; }
      public List<string> LoginDateList { get; set; }
      public List<int> LoginDateCountList { get; set; }

      public List<string> AccessDateList { get; set; }
      public List<int> AccessDateCountList { get; set; }

    }
}
