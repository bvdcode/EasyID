namespace EasyID.Server.Models.Dto
{
    public class UpdateUserRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
    }
}
