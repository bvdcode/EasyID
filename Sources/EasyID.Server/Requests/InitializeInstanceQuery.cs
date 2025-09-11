using EasyID.Server.Models.Dto;
using MediatR;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class IInitializeInstanceQueryHandler : IRequestHandler<InitializeInstanceQuery>
    {
        public Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
