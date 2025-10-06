namespace EasyID.Server.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public long? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string[] Groups { get; set; } = [];
        public string[] Roles { get; set; } = [];
        public string[] Permissions { get; set; } = [];
    }
}