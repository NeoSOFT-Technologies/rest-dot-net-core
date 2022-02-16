using AutoMapper;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.gRPC.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AuthenticationRequest, Account.V1.AuthenticateReq>().ReverseMap();
            CreateMap<AuthenticationResponse, Account.V1.AuthenticateResp>().ReverseMap();
            CreateMap<RegistrationRequest, Account.V1.RegisterReq>().ReverseMap();
            CreateMap<RegistrationResponse, Account.V1.RegisterResp>().ReverseMap();
        }
    }
}
