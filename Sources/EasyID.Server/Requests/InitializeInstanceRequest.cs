using EasyID.Server.Models.Dto;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceRequest
    {
        public LoginRequestDto FirstLoginRequest { get; internal set; }
    }
}
