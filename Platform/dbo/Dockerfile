FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
WORKDIR /src/SOL.Database.Tenant
RUN dotnet restore
RUN dotnet build /p:NetCoreBuild=true

FROM markhobson/sqlpackage:latest AS final

WORKDIR /app
COPY --from=build /src/SOL.Database.Tenant/bin/Debug/. .

# Use environment variables for the connection string
ENTRYPOINT sqlpackage /a:Publish /sf:"./SOL.Database.Tenant.dacpac" /TargetConnectionString:"Server=${SQL_SERVER};Database=${SQL_DATABASE};User Id=${SQL_USER};Password=${SQL_PASSWORD};Encrypt=True;TrustServerCertificate=False"