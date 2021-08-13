#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/API/GloboTicket.TicketManagement.Api/GloboTicket.TicketManagement.Api.csproj", "src/API/GloboTicket.TicketManagement.Api/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Infrastructure/GloboTicket.TicketManagement.Infrastructure.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Infrastructure/"]
COPY ["src/Core/GloboTicket.TicketManagement.Application/GloboTicket.TicketManagement.Application.csproj", "src/Core/GloboTicket.TicketManagement.Application/"]
COPY ["src/Core/GloboTicket.TicketManagement.Domain/GloboTicket.TicketManagement.Domain.csproj", "src/Core/GloboTicket.TicketManagement.Domain/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Identity/GloboTicket.TicketManagement.Identity.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Identity/"]
COPY ["src/Infrastructure/GloboTicket.TicketManagement.Persistence/GloboTicket.TicketManagement.Persistence.csproj", "src/Infrastructure/GloboTicket.TicketManagement.Persistence/"]
RUN dotnet restore "src/API/GloboTicket.TicketManagement.Api/GloboTicket.TicketManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/API/GloboTicket.TicketManagement.Api"
RUN dotnet build "GloboTicket.TicketManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.TicketManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTicket.TicketManagement.Api.dll"]
