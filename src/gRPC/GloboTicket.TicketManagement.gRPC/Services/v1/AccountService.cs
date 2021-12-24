using Account.V1;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Identity;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using Grpc.Core;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.gRPC.Services.v1
{
    public class AccountService : Account.V1.AccountProtoService.AccountProtoServiceBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public AccountService(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public override async Task<AuthenticateResp> Authenticate(AuthenticateReq request, ServerCallContext context)
        {

            var req = _mapper.Map<AuthenticationRequest>(request);
            var response = await _authenticationService.AuthenticateAsync(req);
            if (response.Message == null)
            {
                response.Message = "";
            }
            var rep = _mapper.Map<AuthenticateResp>(response);
            return rep;
        }

        public override async Task<RegisterResp> Register(RegisterReq request, ServerCallContext context)
        {
            var req = _mapper.Map<RegistrationRequest>(request);
            var response = await _authenticationService.RegisterAsync(req);
            var resp = _mapper.Map<RegisterResp>(response);
            return resp;
        }
    }
}
