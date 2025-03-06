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

ARG VITE_API_URL
ARG VITE_WS_API_URL
ARG VITE_REPORT_SERVICE_URL

ENV VITE_API_URL=${VITE_API_URL}
ENV VITE_WS_API_URL=${VITE_WS_API_URL}
ENV VITE_REPORT_SERVICE_URL=${VITE_REPORT_SERVICE_URL}

RUN npm run code-gen || true
RUN mkdir -p dist && (npm run build || true) && \
    echo "Contents of dist directory:" && \
    ls -la dist

# Stage 3: Create the ASP.NET Core runtime image and copy the build files
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app .

RUN mkdir -p ./wwwroot
COPY --from=build-stage /app/dist ./wwwroot
RUN ls -la ./wwwroot || true

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "SOL.Gateway.dll"]