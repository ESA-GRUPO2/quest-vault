namespace questvault.Server.Model
{
    public class User(Guid userID, string userName, string email, string password)
    {
        public Guid UserID { get; set; } = userID;
        public string UserName { get; set; } = userName;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
        public bool IsDeactivated { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        // public SystemSettings SystemSettings { get; set;}
        public int Clearance { get; set; } = 0;

        public bool ToggleActivation() { return IsDeactivated = !IsDeactivated; }
        public bool TogglePrivate() { return IsPrivate = !IsPrivate; }
        public int UpgradeClearance() { return ++Clearance; }
    }
}
