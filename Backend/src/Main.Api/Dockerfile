FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Main.Api/Main.Api.csproj", "src/Main.Api/"]
COPY ["src/Main.Contract/Main.Contract.csproj", "src/Main.Contract/"]
COPY ["src/Main.Domain/Main.Domain.csproj", "src/Main.Domain/"]
COPY ["src/lib/BaseDomain/BaseDomain.csproj", "src/lib/BaseDomain/"]
COPY ["src/Main.Infrastructure/Main.Infrastructure.csproj", "src/Main.Infrastructure/"]
COPY ["src/lib/BaseInfrastructure/BaseInfrastructure.csproj", "src/lib/BaseInfrastructure/"]
COPY ["src/Main.Application/Main.Application.csproj", "src/Main.Application/"]
RUN dotnet restore "src/Main.Api/Main.Api.csproj"
COPY . .
WORKDIR "/src/src/Main.Api"
RUN dotnet build "./Main.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Main.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Main.Api.dll"]
