using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class User : IdentityUser
    {
        public bool IsDeactivated { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        // public SystemSettings SystemSettings { get; set;}

        public string? ProfilePhotoPath { get; set; }
        public int Clearance { get; set; } = 0;

        public bool ToggleActivation() { return IsDeactivated = !IsDeactivated; }
        public bool TogglePrivate() { return IsPrivate = !IsPrivate; }
        public int UpgradeClearance()
        {
            if (Clearance >= 2) return Clearance = 2;
            return ++Clearance;
        }
        public int DowngradeClearance()
        {
            if (Clearance <= 0) return Clearance = 0;
            return --Clearance;
        }
    }
}
