#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["test/GloboTicket.TicketManagement.API.IntegrationTests/GloboTicket.TicketManagement.API.IntegrationTests.csproj", "test/GloboTicket.TicketManagement.API.IntegrationTests/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Persistence/GloboTicket.TicketManagement.Persistence.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Persistence/"]
COPY ["src/Core/GloboTicket.TicketManagement.Application/GloboTicket.TicketManagement.Application.csproj", "src/Core/GloboTicket.TicketManagement.Application/"]
COPY ["src/Core/GloboTicket.TicketManagement.Domain/GloboTicket.TicketManagement.Domain.csproj", "src/Core/GloboTicket.TicketManagement.Domain/"]
COPY ["src/API/GloboTicket.TicketManagement.Api/GloboTicket.TicketManagement.Api.csproj", "src/API/GloboTicket.TicketManagement.Api/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Infrastructure/GloboTicket.TicketManagement.Infrastructure.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Infrastructure/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Identity/GloboTicket.TicketManagement.Identity.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Identity/"]
RUN dotnet restore "test/GloboTicket.TicketManagement.API.IntegrationTests/GloboTicket.TicketManagement.API.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/test/GloboTicket.TicketManagement.API.IntegrationTests"
RUN dotnet build "GloboTicket.TicketManagement.API.IntegrationTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.TicketManagement.API.IntegrationTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTicket.TicketManagement.API.IntegrationTests.dll"]
