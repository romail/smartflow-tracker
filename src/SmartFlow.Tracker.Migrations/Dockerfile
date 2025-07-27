# --- Stage 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY src/SmartFlow.Tracker.sln ./
COPY src/SmartFlow.Tracker.Api/*.csproj ./SmartFlow.Tracker.Api/
COPY src/SmartFlow.Tracker.Application/*.csproj ./SmartFlow.Tracker.Application/
COPY src/SmartFlow.Tracker.Infrastructure/*.csproj ./SmartFlow.Tracker.Infrastructure/
COPY src/SmartFlow.Tracker.Domain/*.csproj ./SmartFlow.Tracker.Domain/
COPY src/SmartFlow.Tracker.Migrations/*.csproj ./SmartFlow.Tracker.Migrations/
COPY src/SmartFlow.Tracker.Bots/*.csproj ./SmartFlow.Tracker.Bots/
COPY src/SmartFlow.Tracker.AI/*.csproj ./SmartFlow.Tracker.AI/

# Include raw SQL migration scripts
COPY src/SmartFlow.Tracker.Migrations/Migrations/ /app/Migrations/

# Restore NuGet packages
RUN dotnet restore "SmartFlow.Tracker.sln"

# Copy all source files
COPY src/ ./

# Build and publish migration project
WORKDIR /src/SmartFlow.Tracker.Migrations
RUN dotnet publish -c Release -o /app

# --- Stage 2: Runtime ---
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app .

# Install PostgreSQL CLI tools for health check (pg_isready)
RUN apt-get update && apt-get install -y postgresql-client

# Add and configure entrypoint script
COPY src/SmartFlow.Tracker.Migrations/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Execute the entrypoint script
ENTRYPOINT ["/app/entrypoint.sh"]