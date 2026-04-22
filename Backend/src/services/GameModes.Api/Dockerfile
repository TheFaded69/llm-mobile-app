FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/services/GameModes.Api/GameModes.Api.csproj", "src/services/GameModes.Api/"]
COPY ["src/services/GameModes.Application/GameModes.Application.csproj", "src/services/GameModes.Application/"]
COPY ["src/services/GameModes.Infrastructure/GameModes.Infrastructure.csproj", "src/services/GameModes.Infrastructure/"]
COPY ["src/services/GameModes.Domain/GameModes.Domain.csproj", "src/services/GameModes.Domain/"]
COPY ["src/services/GameModes.Contracts/GameModes.Contracts.csproj", "src/services/GameModes.Contracts/"]
RUN dotnet restore "src/services/GameModes.Api/GameModes.Api.csproj"
COPY . .
WORKDIR "/src/src/services/GameModes.Api"
RUN dotnet build "GameModes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameModes.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameModes.Api.dll"]
