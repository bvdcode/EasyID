namespace EasyID.Server.Models.Dto
{
    public class UserDto
    {
        public required Guid Id { get; init; }
        public required string Email { get; init; }
        public required string Username { get; init; }
        public long? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
    }
}