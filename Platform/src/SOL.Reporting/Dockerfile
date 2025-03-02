FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

WORKDIR /src/SOL.Reporting
COPY SOL.Reporting/nuget.config /root/.nuget/NuGet/

RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update
RUN apt-get install -y libfreetype6
RUN apt-get install -y libfontconfig1

WORKDIR /app
COPY --from=build /app .

EXPOSE 80

ENTRYPOINT ["dotnet", "SOL.Reporting.dll"]
