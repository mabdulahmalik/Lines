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
ENTRYPOINT sqlpackage /a:Publish /sf:"./SOL.Database.Tenant.dacpac" /TargetConnectionString:"Server=svc.sqldb.local;Database=CustomerDb;User Id=sa;Password=P@ssw0rd!;Persist Security Info=False;Encrypt=False"
# ENTRYPOINT ["tail", "-f", "/dev/null"]