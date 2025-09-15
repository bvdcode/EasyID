namespace EasyID.Server.Models.Dto
{
    public class UserDto
    {
        public required Guid Id { get; init; }
        public required string Username { get; init; }
    }
}