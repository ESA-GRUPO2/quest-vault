using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
  public class TwoFactorAuthenticationTokens
  {
    [Key, Column(Order = 0)]
    public required string UserId { get; set; }
    public string Token { get; set; } = new Random().Next(0, 1000000).ToString("D6");
    public required User User { get; set; }
  }
}
