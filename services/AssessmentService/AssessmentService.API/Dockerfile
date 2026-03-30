FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AssessmentService.API/AssessmentService.API.csproj", "AssessmentService.API/"]
COPY ["AssessmentService.Application/AssessmentService.Application.csproj", "AssessmentService.Application/"]
COPY ["AssessmentService.Domain/AssessmentService.Domain.csproj", "AssessmentService.Domain/"]
COPY ["AssessmentService.Infrastructure/AssessmentService.Infrastructure.csproj", "AssessmentService.Infrastructure/"]

RUN dotnet restore "AssessmentService.API/AssessmentService.API.csproj"

COPY . .
WORKDIR "/src/AssessmentService.API"
RUN dotnet publish "AssessmentService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AssessmentService.API.dll"]