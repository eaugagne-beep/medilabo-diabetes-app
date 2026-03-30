FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PatientService.API/PatientService.API.csproj", "PatientService.API/"]
COPY ["PatientService.Application/PatientService.Application.csproj", "PatientService.Application/"]
COPY ["PatientService.Domain/PatientService.Domain.csproj", "PatientService.Domain/"]
COPY ["PatientService.Infrastructure/PatientService.Infrastructure.csproj", "PatientService.Infrastructure/"]

RUN dotnet restore "PatientService.API/PatientService.API.csproj"

COPY . .
WORKDIR "/src/PatientService.API"
RUN dotnet publish "PatientService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PatientService.API.dll"]