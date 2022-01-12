using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.gRPC.IntegrationTests.Base
{
    public sealed class ServerFixture : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        public GrpcChannel grpcChannel { get; }

        public ServerFixture()
        {
            //_factory = new CustomWebApplicationFactory();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());
            grpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions()
            {
                HttpClient = client
            }) ;
            
        }

        public void Dispose()
        {
            _factory.Dispose();
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }
}
