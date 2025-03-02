# Stage 1: Build the dotnet application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ./Platform/src .

WORKDIR /src/SOL.Gateway
RUN dotnet restore

RUN dotnet publish -c Release -o /app

# Stage 2: Build the Vue 3 application with Vite
FROM node:18-alpine AS build-stage

WORKDIR /app
COPY ./LinesApp/package*.json ./
RUN npm install
COPY ./LinesApp/ ./

RUN npm run code-gen || true
RUN npm run build || true

# Stage 3: Create the Nginx image and copy the build files
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app .
COPY --from=build-stage /app/dist ./wwwroot

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "SOL.Gateway.dll"]