namespace PersonalWebsiteBlazor.Models
{
    public class RoleModification
    {
        public string RoleName { get; set; } = default!;
        public string RoleId { get; set; } = default!;
        public string[]? AddIds { get; set; } = default!;
        public string[]? DeleteIds { get; set; } = default!;
    }
}
