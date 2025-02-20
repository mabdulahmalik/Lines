# Stage 1: Build the Vue 3 application with Vite
FROM node:18-alpine AS build-stage
ARG GQL_SCHEMA_PATH
ARG VITE_WS_API_URL
ARG VITE_API_URL

ENV GQL_SCHEMA_PATH=$GQL_SCHEMA_PATH
ENV VITE_WS_API_URL=$VITE_WS_API_URL
ENV VITE_API_URL=$VITE_API_URL

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run codegen
RUN npm run build

# Stage 2: Create the Nginx image and copy the build files
FROM nginx:1.27-alpine
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY --from=build-stage /app/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]