# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY src/SmartFlow.Tracker.sln ./
COPY src/SmartFlow.Tracker.Api/*.csproj ./SmartFlow.Tracker.Api/
COPY src/SmartFlow.Tracker.Application/*.csproj ./SmartFlow.Tracker.Application/
COPY src/SmartFlow.Tracker.Domain/*.csproj ./SmartFlow.Tracker.Domain/
COPY src/SmartFlow.Tracker.Infrastructure/*.csproj ./SmartFlow.Tracker.Infrastructure/
COPY src/SmartFlow.Tracker.AI/*.csproj ./SmartFlow.Tracker.AI/
COPY src/SmartFlow.Tracker.Bots/*.csproj ./SmartFlow.Tracker.Bots/
COPY src/SmartFlow.Tracker.Migrations/*.csproj ./SmartFlow.Tracker.Migrations/


RUN dotnet restore

# Copy the rest of the files
COPY . .

# Change directory to the API project
WORKDIR /src/src/SmartFlow.Tracker.Api

RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartFlow.Tracker.Api.dll"]
