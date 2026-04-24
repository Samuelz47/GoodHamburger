FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodHamburger.API/GoodHamburger.API.csproj", "GoodHamburger.API/"]
COPY ["GoodHamburger.Application/GoodHamburger.Application.csproj", "GoodHamburger.Application/"]
COPY ["GoodHamburger.Domain/GoodHamburger.Domain.csproj", "GoodHamburger.Domain/"]
COPY ["GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj", "GoodHamburger.Infrastructure/"]
RUN dotnet restore "GoodHamburger.API/GoodHamburger.API.csproj"
COPY . .
WORKDIR "/src/GoodHamburger.API"
RUN dotnet build "./GoodHamburger.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodHamburger.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /app/data && chown -R $APP_UID:$APP_UID /app
USER $APP_UID
ENTRYPOINT ["dotnet", "GoodHamburger.API.dll"]
