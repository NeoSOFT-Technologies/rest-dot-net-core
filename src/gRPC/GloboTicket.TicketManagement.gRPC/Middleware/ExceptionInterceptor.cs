using GloboTicket.TicketManagement.Application.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace GloboTicket.TicketManagement.gRPC.Middleware
{
    public class ExceptionInterceptor:Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (ValidationException exception)
            {
                _logger.LogError(exception, "");
                var errors = "";
                for (int i = 0; i < exception.ValdationErrors.Count; i++)
                {
                    errors += exception.ValdationErrors[i];
                }
                throw new RpcException(new Status(StatusCode.Internal, errors));
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, "");
                throw new RpcException(new Status(StatusCode.Internal, exception.Message));
            }
            catch (NotFoundException exception)
            {
                _logger.LogError(exception, "");
                throw new RpcException(new Status(StatusCode.Internal, exception.Message));
            }
            catch (CryptographicException exception)
            {
                _logger.LogError(exception, "");
                throw new RpcException(new Status(StatusCode.Internal, "Entered Id does not exist"));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "", null);
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error occurred contact dev team."));
            }
    }
}
}
