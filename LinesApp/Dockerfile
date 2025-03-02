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


FROM node:18-alpine

# Install .NET SDK dependencies
RUN apk add --no-cache \
    icu-libs krb5-libs libgcc libintl libssl3 libstdc++ zlib \
    && apk add --no-cache --virtual .build-deps \
    curl bash icu krb5-libs libssl3 zlib \
    && curl -O https://dot.net/v1/dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh -c 8.0 \
    && rm -rf dotnet-install.sh

ENV PATH="/root/.dotnet:${PATH}"

WORKDIR /app

COPY package*.json ./
RUN npm install --frozen-lockfile

COPY . .
RUN npm run codegen  # Now it should find the `dotnet` command

CMD ["npm", "run", "start"]

