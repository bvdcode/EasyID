namespace EasyID.Server.Models.Dto
{
    public class UpdateUserPasswordRequestDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
