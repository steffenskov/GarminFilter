# Build stage
FROM ghcr.io/cirruslabs/flutter:stable AS build

WORKDIR /app

# Copy pubspec files and get dependencies
COPY pubspec.* ./
RUN flutter pub get

# Copy the rest of the app
COPY . .

# Build for web
ARG BACKEND_URL=http://localhost:8080
RUN flutter build web --release --dart-define=BACKEND_URL=$BACKEND_URL

# Runtime stage - serve with nginx
FROM nginx:alpine

# Copy built web app to nginx
COPY --from=build /app/build/web /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
