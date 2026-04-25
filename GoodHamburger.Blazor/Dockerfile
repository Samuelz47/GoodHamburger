FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["GoodHamburger.Blazor/GoodHamburger.Blazor.csproj", "GoodHamburger.Blazor/"]

RUN dotnet restore "GoodHamburger.Blazor/GoodHamburger.Blazor.csproj"

COPY . .
WORKDIR "/src/GoodHamburger.Blazor"

RUN dotnet build "GoodHamburger.Blazor.csproj" -c Release -o /app/build
RUN dotnet publish "GoodHamburger.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY GoodHamburger.Blazor/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80


