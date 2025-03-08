FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
WORKDIR /src/SOL.Database.Tenant
RUN dotnet restore
RUN dotnet build /p:NetCoreBuild=true


FROM markhobson/sqlpackage:latest AS final

WORKDIR /app
COPY --from=build /src/SOL.Database.Tenant/bin/Debug/. .

# TODO: remove the hardcoded connection string and turn it into an environment variable
ENTRYPOINT sqlpackage /a:Publish /sf:"./SOL.Database.Tenant.dacpac" /TargetConnectionString:"Server=tcp:sql-qa-eastus2-cpm-prdt-sol.database.windows.net;Initial Catalog=sql-qa-eastus2-cpm-prdt-sol;Persist Security Info=False;User ID=sqladmin;Password=3yFC*2j]0d:NRioXXpTg;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
# ENTRYPOINT ["tail", "-f", "/dev/null"]