# Self-contained deployment Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Home.csproj", "./"]
RUN dotnet restore "./Home.csproj"

# Copy source code
COPY . .

# Build and publish self-contained
RUN dotnet publish "Home.csproj" -c Release -o /app/publish \
    --self-contained true \
    --runtime linux-x64 \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=false

# Final stage - use runtime-deps for self-contained apps
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos "" --uid 1000 appuser
USER appuser

# Copy the self-contained application
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment
ENV ASPNETCORE_URLS=http://+:8080

# Run the self-contained executable
ENTRYPOINT ["./Home"]