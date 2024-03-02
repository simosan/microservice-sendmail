FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY ./SimUserManager.csproj /app
COPY ./SimUserManager.sln /app
COPY ./Program.cs /app
COPY ./appsettings.json /app
COPY ./libman.json /app
COPY ./wwwroot /app/wwwroot
COPY ./Properties /app/Properties
COPY ./Models /app/Models
COPY ./Controllers /app/Controllers
COPY ./Services /app/Services
COPY ./Views /app/Views

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "SimUserManager.dll"]