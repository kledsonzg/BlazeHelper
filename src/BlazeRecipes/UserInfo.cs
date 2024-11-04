namespace BlazeHelper.BlazeRecipes
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Username {get; set;} = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string KycCountry { get; set; } = string.Empty;
        public string? KycLevel { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public bool PaymentEmails { get; set; }
        public bool MarketingEmails { get; set; }
        public bool GeneralEmails { get; set; }
        public bool SmsCommunications { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuspended { get; set; }
        public List<string> Permissions { get; set; } = new();
        public bool InitialProfileComplete { get; set; }
        public string TaxId { get; set; } = string.Empty;
        public DateTime? SuspensionExpiresAt { get; set; }
        public List<string> LockedPersonalInfoFields { get; set; } = new();
        public bool UsernameChangeRequired { get; set; }
    }
}