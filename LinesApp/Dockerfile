# Stage 1: Build the Vue 3 application with Vite
FROM node:18-alpine AS build-stage

# Define build arguments
ARG GQL_SCHEMA_PATH
ARG VITE_WS_API_URL
ARG VITE_API_URL

# Set environment variables
ENV GQL_SCHEMA_PATH=$GQL_SCHEMA_PATH
ENV VITE_WS_API_URL=$VITE_WS_API_URL
ENV VITE_API_URL=$VITE_API_URL

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy project files
COPY . .

# Debug: Print environment variable before running codegen
RUN echo "⚡ Using GraphQL Schema Path: $GQL_SCHEMA_PATH" && \
    ls -la && \
    cat codegen.ts || echo "❌ codegen.ts file missing!"

# Run GraphQL code generation
RUN npm run codegen

# Build the frontend
RUN npm run build


# Stage 2: Create the Nginx image and copy the build files
# FROM nginx:1.27-alpine
# COPY --from=build-stage /app/dist /usr/share/nginx/html
# COPY --from=build-stage /app/nginx.conf /etc/nginx/conf.d/default.conf
# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]