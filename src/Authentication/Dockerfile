FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Authentication/Authentication.csproj", "Authentication/"]
COPY ["src/Authentication.Application/Authentication.Application.csproj", "Authentication.Application/"]
COPY ["src/Authentication.Shared/Authentication.Shared.csproj", "Authentication.Shared/"]
COPY ["src/Authentication.Infrastructure/Authentication.Infrastructure.csproj", "Authentication.Infrastructure/"]
RUN dotnet restore "Authentication/Authentication.csproj"
COPY src/ .
WORKDIR "/src/Authentication"
RUN dotnet build "./Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.dll"]
