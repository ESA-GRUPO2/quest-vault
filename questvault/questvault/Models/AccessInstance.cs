using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
  public class AccessInstance
  {
    [Key]
    public long Id { get; set; }
    public DateOnly AccessDate { get; set; }
  }
}
