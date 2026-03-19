# https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/

FROM node:alpine AS nodebuild
COPY npm ./npm
RUN mkdir -p src/Blazor/wwwroot/js
WORKDIR /npm
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
EXPOSE 8080
EXPOSE 8081

COPY ["src/Blazor/Blazor.csproj", "Blazor/"]
COPY ["src/Blazor.Component/Blazor.Component.csproj", "Blazor.Component/"]
COPY ["src/Blazor.Component.ChoreTracker/Blazor.Component.ChoreTracker.csproj", "Blazor.Component.ChoreTracker/"]
COPY ["src/Blazor.Component.WebAuthn/Blazor.Component.WebAuthn.csproj", "Blazor.Component.WebAuthn/"]
COPY ["src/Blazor.Infrastructure/Blazor.Infrastructure.csproj", "Blazor.Infrastructure/"]
COPY ["src/Blazor.Shared/Blazor.Shared.csproj", "Blazor.Shared/"]

RUN dotnet restore "Blazor/Blazor.csproj"

COPY . .

COPY --from=nodebuild /src/Blazor/wwwroot/js/index.bundle.js ./src/Blazor/wwwroot/js/

WORKDIR /src/Blazor
RUN dotnet build "Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.csproj" --no-restore -c Release -o /published

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /published/wwwroot .

COPY config/nginx.conf /etc/nginx/nginx.conf
COPY config/blazor-startup.sh /docker-entrypoint.d/01-blazor-startup.sh
RUN chmod +x /docker-entrypoint.d/01-blazor-startup.sh