namespace EasyID.Server.Models.Dto
{
    public class LoginRequestDto
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
