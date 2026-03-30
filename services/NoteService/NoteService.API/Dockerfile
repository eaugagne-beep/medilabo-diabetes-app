FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["NoteService.API/NoteService.API.csproj", "NoteService.API/"]
COPY ["NoteService.Application/NoteService.Application.csproj", "NoteService.Application/"]
COPY ["NoteService.Domain/NoteService.Domain.csproj", "NoteService.Domain/"]
COPY ["NoteService.Infrastructure/NoteService.Infrastructure.csproj", "NoteService.Infrastructure/"]

RUN dotnet restore "NoteService.API/NoteService.API.csproj"

COPY . .
WORKDIR "/src/NoteService.API"
RUN dotnet publish "NoteService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "NoteService.API.dll"]