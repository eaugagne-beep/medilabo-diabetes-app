FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GatewayService.csproj", "./"]
RUN dotnet restore "GatewayService.csproj"

COPY . .
RUN dotnet publish "GatewayService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ["ocelot.json", "./"]
EXPOSE 8080
ENTRYPOINT ["dotnet", "GatewayService.dll"]