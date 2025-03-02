# # Stage 1: Build Vue.js frontend
# FROM node:18-alpine

# # Set working directory
# WORKDIR /app

# # Copy package.json and package-lock.json for caching
# COPY package*.json ./

# # Install dependencies
# RUN npm install --frozen-lockfile

# # Copy the entire application code
# COPY . .

# # # Generate GraphQL types if required
# RUN npm run codegen

# # Build the Vue application
# RUN npm run build

# # Stage 2: Serve the app using a lightweight HTTP server
# FROM node:18-alpine AS serve-stage

# # Install a simple HTTP server to serve static files
# RUN npm install -g http-server

# # Set working directory to the built app
# WORKDIR /app

# # Copy built files from the build stage
# COPY --from=build-stage /app/dist .

# # Expose port 8080
# EXPOSE 8080

# # Run the HTTP server on startup
# CMD ["http-server", "-p", "8080"]



# First stage: Build with .NET SDK and Node.js
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app

# Copy platform source code required for schema generation
COPY ../Platform/src/SOL.Gateway ./Platform/src/SOL.Gateway
COPY ../Platform/src/SOL.Gateway/SOL.Gateway.csproj ./Platform/src/SOL.Gateway/
RUN dotnet restore ./Platform/src/SOL.Gateway/SOL.Gateway.csproj

# Build the .NET project to generate schema
RUN dotnet run --project ./Platform/src/SOL.Gateway -- schema export --output ./schema.graphql

# Second stage: Node.js for frontend
FROM node:18-alpine AS frontend
WORKDIR /app

COPY package*.json ./
RUN npm install --frozen-lockfile

# Copy schema from the previous stage
COPY --from=builder /app/schema.graphql ./schema.graphql

COPY . .
RUN npm run build

CMD ["npm", "run", "start"]
