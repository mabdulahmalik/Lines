# Stage 1: Build Vue.js frontend
FROM node:18-alpine AS build-stage

# Set working directory
WORKDIR /app/LinesApp

# Copy package.json and package-lock.json for caching
COPY LinesApp/package*.json ./

# Install dependencies
RUN npm install --frozen-lockfile

# Copy the entire application code
COPY LinesApp . 

# Generate GraphQL types if required
RUN npm run codegen 

# Build the Vue application
RUN npm run build

# Stage 2: Serve the app using a lightweight HTTP server
FROM node:18-alpine AS serve-stage

# Install a simple HTTP server to serve static files
RUN npm install -g http-server

# Set working directory to the built app
WORKDIR /app

# Copy built files from the build stage
COPY --from=build-stage /app/LinesApp/dist .

# Expose port 8080
EXPOSE 8080

# Run the HTTP server on startup
CMD ["http-server", "-p", "8080"]
